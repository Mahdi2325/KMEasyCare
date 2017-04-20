/*
创建人:张凯
创建日期:2016-03-16
说明: 指标-非计划减重改变
*/
angular.module("sltcApp")
.controller("unPlanWeightCtrl", ['$scope', 'dictionary', 'utility', 'unPlanWeightRes', '$filter', '$state', function ($scope, dictionary, utility, unPlanWeightRes, $filter, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.curUser = utility.getUserInfo();
    $scope.currentItem = {
        RecordBy: $scope.curUser.EmpNo
    };
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息

        $scope.options.params.feeNo = $scope.currentResident.FeeNo;
        $scope.options.params.regNo = $scope.currentResident.RegNo;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();

        // $scope.listItem($scope.currentResident.RegNo, $scope.currentResident.FeeNo);//加载当前住民的非计划减重记录
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo,
            ThisWeight: resident.Weight,
            ThisHeight: resident.Height,
        };
        $scope.getBMI();
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
        //测量日期 默认带入当天 Add By Duke on 20161204
        $scope.currentItem.ThisRecDate = $filter("date")(new Date(), "yyyy-MM-dd");
    }
    //获取非计划减重
    $scope.listItem = function () {
        $scope.Data.UnPlans = [];
        unPlanWeightRes.get({ regNo: $scope.currentResident.RegNo, feeNo: $scope.currentResident.FeeNo }, function (data) {
            if (angular.isDefined(data.Data)) {
                $scope.Data.UnPlans = data.Data;
                //if ($scope.Data.UnPlans.length > 0) {
                //    $scope.currentItem = $scope.Data.UnPlans[0];
                //}
            }
        });
    }

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: unPlanWeightRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.UnPlans = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                regNo: 0,
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }
    }

    //获取最新一笔非计划减重数据
    $scope.getNewInfo = function () {
        unPlanWeightRes.get({ regNo: $scope.currentResident.RegNo, feeNo: $scope.currentResident.FeeNo }, function (data) {
            if (angular.isDefined(data.Data)) {
                $scope.Data.UnPlans = data.Data;
                if ($scope.Data.UnPlans.length > 0) {
                    $scope.currentItem = $scope.Data.UnPlans[0];
                    $scope.currentItem.Id = undefined;
                    $scope.currentItem.FeeNo = undefined;

                }
            }
        });
    }


    //删除非计划减重记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的非计划减重记录吗?")) {
            unPlanWeightRes.delete({ id: item.Id }, function () {
                //$scope.Data.UnPlans.splice($scope.Data.UnPlans.indexOf(item), 1);
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                utility.message($scope.currentResident.Name + "的非计划减重信息删除成功！");
            });
        }
    };

    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.RecordBy = item.EmpNo;
        $scope.currentItem.RecordNameBy = item.EmpName;
    }


    $scope.createItem = function (item) {
        //新增非计划减重记录，得到住民ID
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        unPlanWeightRes.save($scope.currentItem, function (data) {
            //$scope.Data.UnPlans.push(data.Data);

            $scope.options.search();
            $scope.currentItem = {
                RecordBy: $scope.curUser.EmpNo
            };
            // $scope.listItem($scope.currentResident.RegNo, $scope.currentResident.FeeNo);//加载当前住民的非计划减重记录
        });
    };

    $scope.updateItem = function (item) {
        unPlanWeightRes.save(item, function (data) {
            //angular.copy(data.Data, item);
        });
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo
        };
    };

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    };

    $scope.saveEdit = function (item) {

        if (angular.isDefined($scope.Pinfrom.$error.required)) {
            for (var i = 0; i < $scope.Pinfrom.$error.required.length; i++) {
                utility.msgwarning($scope.Pinfrom.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.Pinfrom.$error.maxlength)) {
            for (var i = 0; i < $scope.Pinfrom.$error.maxlength.length; i++) {
                utility.msgwarning($scope.Pinfrom.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined(item.FeeNo)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的非计划减重信息保存成功！");
    };

    $scope.getBMI = function () {
        $scope.currentItem.BMI = utility.BMI($scope.currentItem.ThisWeight, $scope.currentItem.ThisHeight);
        $scope.currentItem.BMIResults = utility.BMIResult($scope.currentItem.BMI);
    }
    $scope.getHeight = function () {
        if (angular.isUndefined($scope.currentItem.ThisHeight) || $scope.currentItem.ThisHeight == 0 || $scope.currentItem.ThisHeight==null) {
            if (angular.isNumber($scope.currentItem.KneeLen)) {
                $scope.currentItem.ThisHeight = ((($scope.currentItem.KneeLen * 8) / 5) * 5) / 2

            }
            //$scope.currentItem.ThisHeight = utility.CountHeight($scope.currentItem.KneeLen, $scope.currentResident.Sex, $scope.currentResident.Age);
        }

    }

    $scope.init();
}])
.controller("unPlanWeightHistoryCtrl", ['$scope', '$stateParams', '$state', 'dictionary', 'utility', 'unPlanWeightRes', function ($scope, $stateParams, $state, dictionary, utility, unPlanWeightRes) {

    var feeNo = $state.params.id;
    var regNo = $state.params.regNo;
    $scope.Data = {};
    $scope.curUser = utility.getUserInfo();
    $scope.currentItem = {
        RecordBy: $scope.curUser.EmpNo
    };
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = true;

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息

        $scope.options.params.feeNo = $scope.currentResident.FeeNo;
        $scope.options.params.regNo = $scope.currentResident.RegNo;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();

        // $scope.listItem($scope.currentResident.RegNo, $scope.currentResident.FeeNo);//加载当前住民的非计划减重记录
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo
        };
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }
    //获取非计划减重
    $scope.listItem = function () {
        $scope.Data.UnPlans = [];
        unPlanWeightRes.get({ regNo: $scope.currentResident.RegNo, feeNo: $scope.currentResident.FeeNo }, function (data) {
            if (angular.isDefined(data.Data)) {
                $scope.Data.UnPlans = data.Data;
                //if ($scope.Data.UnPlans.length > 0) {
                //    $scope.currentItem = $scope.Data.UnPlans[0];
                //}
            }
        });
    }

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: unPlanWeightRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.UnPlans = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                regNo: regNo,
                feeNo: feeNo
            }
        }
    }

    //获取最新一笔非计划减重数据
    $scope.getNewInfo = function () {
        unPlanWeightRes.get({ regNo: regNo, feeNo: feeNo }, function (data) {
            if (angular.isDefined(data.Data)) {
                $scope.Data.UnPlans = data.Data;
                if ($scope.Data.UnPlans.length > 0) {
                    $scope.currentItem = $scope.Data.UnPlans[0];
                    //$scope.currentItem.Id = undefined;
                    //$scope.currentItem.FeeNo = undefined;

                }
            }
        });
    }


    //删除非计划减重记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的非计划减重记录吗?")) {
            unPlanWeightRes.delete({ id: item.Id }, function () {
                //$scope.Data.UnPlans.splice($scope.Data.UnPlans.indexOf(item), 1);
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                utility.message(item.Name + "的非计划减重信息删除成功！");
            });
        }
    };

    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.RecordBy = item.EmpNo;
        $scope.currentItem.RecordNameBy = item.EmpName;
    }


    $scope.createItem = function (item) {
        //新增非计划减重记录，得到住民ID
        $scope.currentItem.RegNo = regNo;
        $scope.currentItem.FeeNo = feeNo;
        unPlanWeightRes.save($scope.currentItem, function (data) {
            //$scope.Data.UnPlans.push(data.Data);

            $scope.options.search();
            $scope.currentItem = {
                RecordBy: $scope.curUser.EmpNo
            };
            // $scope.listItem($scope.currentResident.RegNo, $scope.currentResident.FeeNo);//加载当前住民的非计划减重记录
        });
    };

    $scope.updateItem = function (item) {
        unPlanWeightRes.save(item, function (data) {
            //angular.copy(data.Data, item);
        });
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo
        };
    };

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    };

    $scope.saveEdit = function (item) {

        if (angular.isDefined($scope.Pinfrom.$error.required)) {
            for (var i = 0; i < $scope.Pinfrom.$error.required.length; i++) {
                utility.msgwarning($scope.Pinfrom.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.Pinfrom.$error.maxlength)) {
            for (var i = 0; i < $scope.Pinfrom.$error.maxlength.length; i++) {
                utility.msgwarning($scope.Pinfrom.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined(item.FeeNo)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message(item.Name + "的非计划减重信息保存成功！");
    };

    $scope.getBMI = function () {
        $scope.currentItem.BMI = utility.BMI($scope.currentItem.ThisWeight, $scope.currentItem.ThisHeight);
        $scope.currentItem.BMIResults = utility.BMIResult($scope.currentItem.BMI);
    }
    $scope.getHeight = function () {
        if (angular.isUndefined($scope.currentItem.ThisHeight)) {
            $scope.currentItem.ThisHeight = utility.CountHeight($scope.currentItem.KneeLen, $scope.currentResident.Sex, $scope.currentResident.Age);
        }

    }

    $scope.init();
}])
.controller("unPlanWeightListCtrl", ['$scope', 'dictionary', 'utility', 'unPlanWeightRes', 'floorRes', 'roomRes', function ($scope, dictionary, utility, unPlanWeightRes, floorRes, roomRes) {
    $scope.Data = {};
    //获取所有楼层信息
    floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {

        $scope.Data.floors = data.Data;
    });
    //获取所有房间信息
    roomRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
        $scope.Data.rooms = data.Data;
    });

    $scope.$watch('Data.FloorName',
    function (newVal, oldVal, scope) {
        if (newVal === oldVal) {
            // 只会在监控器初始化阶段运行
        } else {
            // 初始化之后发生的变化
            roomRes.get({ CurrentPage: 1, PageSize: 100, floorName: $scope.Data.FloorName }, function (data) {
                $scope.Data.rooms = data.Data;
            });
        }
    });
    $scope.currentPage = 1;
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: unPlanWeightRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.UnPlans = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                regNo: 0,
                feeNo: -1
            }
        }
    }
    function getBMI(item) {
        item.BMI = utility.BMI(parseFloat(item.ThisWeight), parseFloat(item.ThisHeight));
        item.BMIResults = utility.BMIResult(parseFloat(item.BMI));
    }
    $scope.getHeight = function (info) {
        if (angular.isUndefined(info.ThisHeight) || info.ThisHeight == 0 || info.ThisHeight == null) {
            if (angular.isNumber(info.KneeLen)) {
                info.ThisHeight = (((info.KneeLen * 8) / 5) * 5) / 2

            }
            //$scope.currentItem.ThisHeight = utility.CountHeight($scope.currentItem.KneeLen, $scope.currentResident.Sex, $scope.currentResident.Age);
        }

    }
    $scope.loadUnPlanWeight = function () {
        var floorName = !$scope.Data.FloorName ? "" : $scope.Data.FloorName;
        var roomName = !$scope.Data.RoomName ? "" : $scope.Data.RoomName;
        unPlanWeightRes.MultiQuery({ floorName: floorName, roomName: roomName, currentPage: $scope.currentPage, pageSize: 10 }, function (data) {
            $scope.RecordBy = utility.getUserInfo();
            $scope.Data.List = data.Data;
            _.forEach($scope.Data.List, function (name, index) {
                $scope.Data.List[index].RecordBy = $scope.RecordBy.EmpNo;
                $scope.Data.List[index].ThisRecDate = moment().format("YYYY-MM-DD HH:mm:ss");
                //console.log(name);
            });
            $.each(data.Data, function (index, item) {
                // item.RecordDate = $scope.Data.RecordDate;
            });
            var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {
                $scope.currentPage = curPage;
                $scope.loadUnPlanWeight();
            });

        });

    }
    $scope.saveUnPlanWeight = function () {

        var list = [];

        if ($scope.Data.List != undefined) {
            $.each($scope.Data.List, function (index, item) {
                if (item.CheckType) {
                    getBMI(item);
                    list.push(item);
                }
            })
            if (list.length > 0) {

                unPlanWeightRes.MultiSave(list, function (data) {
                    if (data.ResultCode == 0) {
                        utility.message("保存成功！");
                        $scope.loadUnPlanWeight();
                    }
                    else {
                        utility.message(data.ResultMessage);
                    }
                })
            }
            else {
                utility.message("数据录入不完整,请依次从日期开始填写！");
            }
        }
    }

    $scope.init();
}]);
