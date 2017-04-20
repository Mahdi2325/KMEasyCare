/*
创建人:张凯
创建日期:2016-03-09
说明: 出入量批理输入
*/
angular.module("sltcApp")
.controller("outInValueCtrl", ['$scope', '$http', '$filter', '$location', '$state', 'utility', 'residentBriefRes', 'inValueRes', 'outValueRes', 'floorRes', 'roomRes',
    function ($scope, $http, $filter, $location, $state, utility, residentBriefRes, inValueRes, outValueRes, floorRes, roomRes) {
        $scope.Data = {};
        $scope.Floor = {};
        $scope.isModify = function (list) {
            if (list != null) {
                var arr = $.grep(list, function (n, i) {
                    return n.CheckType;
                });
                return arr.length > 0;
            }
            else {
                return false;
            }
        }
        $scope.curUser = utility.getUserInfo();

        //初始化数据
        $scope.loadViews = function (Type) {
            $scope.Floor = $scope.Floor === null ? {} : $scope.Floor;
            var floorName = !$scope.Floor.FloorName ? "" : $scope.Floor.FloorName;
            var roomName = !$scope.Room ? "" : $scope.Room;
            residentBriefRes.get({ floorName: floorName, roomName: roomName }, function (data) {
                $.each(data.Data, function () {
                    this.RecDate = $scope.Data.ServiceDate;
                    this.RecordBy = $scope.curUser.EmpNo;
                });
                switch (Type) {
                    case 1:
                        $scope.Data.InList = data.Data;
                        $scope.Data.OutList = angular.copy(data.Data);
                        if (data.ResultCode == 1001)
                        {
                            utility.message(data.ResultMessage);
                        }
                        break;
                    case 2:
                        $scope.Data.InList = data.Data;

                        break;
                    case 3:

                        $scope.Data.OutList = angular.copy(data.Data);
                        break;
                    default:
                        $scope.Data.InList = data.Data;
                        $scope.Data.OutList = angular.copy(data.Data);
                }

            });
        }

        floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
            $scope.Data.floors = data.Data;

            //$scope.Data.FloorName= $scope.Data.floors[0].FloorId;
        });

        $scope.getFloor = function (floorName) {

            roomRes.get({ floorName: floorName, CurrentPage: 1, PageSize: 100 }, function (data) {
                $scope.Data.rooms = data.Data;
            });
        }
        $scope.SaveIn = function () {
            var saveData = [];
            var result = true;
            $.each($scope.Data.InList, function () {
                if (this.CheckType) {

                    if (!angular.isDefined(this.RecDate)) {
                        utility.message(this.Name + "的日期不能为空！");
                        result = false;
                        return false;
                    }
                    if (!angular.isDefined(this.InValue)) {
                        utility.message(this.Name + "的注射量不能为空！");
                        result = false;
                        return false;
                    }
                    if (!angular.isDefined(this.ClassType)) {
                        utility.message(this.Name + "的班别不能为空！");
                        result = false;
                        return false;
                    }
                    if (this.RecDate == "" || this.RecDate == null) {
                        utility.message(this.Name + "的日期不能为空！");
                        result = false;
                        return false;
                    }
                    if (this.InValue == "" || this.InValue == null) {
                        utility.message(this.Name + "的注射量不能为空！");
                        result = false;
                        return false;
                    }
                    if (this.ClassType == "" || this.ClassType == null) {
                        utility.message(this.Name + "的班别不能为空！");
                        result = false;
                        return false;
                    }
                    saveData.push(this);
                  
                }
            });
            if (result) {
                $scope.loadViews(2);
                inValueRes.save(saveData, function () {
                    utility.message("保存成功！");
                });
            }
        }

        $scope.SaveOut = function () {
            var saveData = [];
            var result = true;
            $.each($scope.Data.OutList, function () {
                if (this.CheckType) {
                    if (!angular.isDefined(this.RecDate)) {
                        utility.message(this.Name + "的日期不能为空！");
                        result = false;
                        return false;
                    }
                    if (!angular.isDefined(this.OutValue)) {
                        utility.message(this.Name + "的排放量不能为空！");
                        result = false;
                        return false;
                    }
                    if (!angular.isDefined(this.ClassType)) {
                        utility.message(this.Name + "的班别不能为空！");
                        result = false;
                        return false;
                    }
                    if (this.RecDate == "" || this.RecDate == null) {
                        utility.message(this.Name + "的日期不能为空！");
                        result = false;
                        return false;
                    }
                    if (this.InValue == "" || this.OutValue == null) {
                        utility.message(this.Name + "的排放量不能为空！");
                        result = false;
                        return false;
                    }
                    if (this.ClassType == "" || this.ClassType == null) {
                        utility.message(this.Name + "的班别不能为空！");
                        result = false;
                        return false;
                    }
                    saveData.push(this);
             
                }
            });
            if (result) {
                $scope.loadViews(3);
                outValueRes.save(saveData, function () {
                    utility.message("保存成功！");
                });
            }
        }
        $scope.checkoutNum = function (input) {
            var nubmer = parseInt(input.OutValue);
            if (isNaN(nubmer) || nubmer <= 0 || !(/^(0|\+?[1-9][0-9]*)$/.test(nubmer))) {
                {
                    alert("排放量只允许输入正整数!");
                    input.OutValue = "";
                    return false;
                }
            }
        }
        $scope.checkNum1 = function (input) {
            var nubmer = parseInt(input.InValue);
            if (isNaN(nubmer) || nubmer <= 0 || !(/^(0|\+?[1-9][0-9]*)$/.test(nubmer))) {
                {
                    alert("注射量只允许输入正整数!");
                    input.InValue = "";
                    return false;
                }
            }
        }
        //得到当前时间
        var getServiceDate = function () {
            return new Date().currentTime() + ":00";
        }

        $scope.Data.ServiceDate = getServiceDate();

        //调整时间
        $scope.changeDate = function () {
            $.each($scope.Data.InList, function () {
                this.RecDate = $scope.Data.ServiceDate;
            });
            $.each($scope.Data.OutList, function () {
                this.RecDate = $scope.Data.ServiceDate;
            });
        }

        $scope.staffSelected = function (item, data) {
            data.RecordBy = item.EmpNo;
        }
    }])
.controller("outInValueRecordCtrl", ['$scope', 'dictionary', 'utility', 'inValueRes', 'outValueRes', 'residentDetailRes', function ($scope, dictionary, utility, inValueRes, outValueRes, residentDetailRes) {
    $scope.Data = {};
    $scope.currentItemIn = {};
    $scope.currentItemOut = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo);//加载当前住民的出入量记录
        $scope.currentItem = {}//清空编辑项
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }
    //获取住民的出入量
    $scope.listItem = function (feeNo) {
        $scope.Data.InList = [];
        $scope.Data.OutList = [];
        //输入量
        inValueRes.get({ feeNo: feeNo }, function (data) {
            $scope.Data.InList = data.Data;
        });
        //输出量
        outValueRes.get({ feeNo: feeNo }, function (data) {
            $scope.Data.OutList = data.Data;
        });
    }

    $scope.staffByInSelected = function (item) {
        $scope.currentItemIn.RecordBy = item.EmpNo;
        $scope.currentItemIn.RecordNameBy = item.EmpName;
    }

    $scope.staffByOutSelected = function (item) {
        $scope.currentItemOut.RecordBy = item.EmpNo;
        $scope.currentItemOut.RecordNameBy = item.EmpName;
    }
    $scope.clearStaffIn = function () {
        if (angular.isDefined($scope.currentItemIn)) {
            $scope.currentItemIn.RecordBy = "";
        }
    }
    $scope.clearStaffOut = function () {
        if (angular.isDefined($scope.currentItemOut)) {
            $scope.currentItemOut.RecordBy = "";
        }
    }

    //删除输入量记录
    $scope.deleteIn = function (item) {
        if (confirm("您确定要删除该住民的输入量记录吗?")) {
            inValueRes.delete({ id: item.InNo }, function () {
                $scope.Data.InList.splice($scope.Data.InList.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的输入量记录信息删除成功！");
            });
        }
    };
    //删除输出量记录
    $scope.deleteOut = function (item) {
        if (confirm("您确定要删除该住民的输出量记录吗?")) {
            outValueRes.delete({ id: item.OutNo }, function () {
                $scope.Data.OutList.splice($scope.Data.OutList.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的输出量记录信息删除成功！");
            });
        }
    };

    $scope.rowSelectIn = function (item) {
        $scope.currentItemIn = item;
    };

    $scope.rowSelectOut = function (item) {
        $scope.currentItemOut = item;
    };

    $scope.saveIn = function (item) {
        var saveInFlag = false;
        if ($scope.Data.InList.length > 0) {
            $.each($scope.Data.InList, function (m, list) {
                if (list.InNo !== (angular.isDefined(item.InNo) ? list.InNo : 0) && list.ClassType == item.ClassType && list.RecDate.toString().substring(0, 10) == item.RecDate.toString().substring(0, 10)) {
                    saveInFlag = true;
                }
            });
        }
        if (saveInFlag) {
            utility.message("相同班别的输入量已经存在，请查看！");
            return;
        }

        $scope.currentItemIn.RegNo = $scope.currentResident.RegNo;
        $scope.currentItemIn.FeeNo = $scope.currentResident.FeeNo;
        inValueRes.save($scope.currentItemIn, function (data) {
            if (!angular.isDefined(item.InNo)) {
                $scope.Data.InList.push(data.Data);
            }
        });
        $scope.currentItemIn = {};
        utility.message($scope.currentResident.Name + "的输入量信息保存成功！");
    };

    $scope.saveOut = function (item) {
        var saveOutFlag = false;
        if ($scope.Data.OutList.length > 0) {
            $.each($scope.Data.OutList, function (m, list) {
                if (list.OutNo !== (angular.isDefined(item.OutNo) ? list.OutNo : 0) && list.ClassType == item.ClassType && list.RecDate.toString().substring(0, 10) == item.RecDate.toString().substring(0, 10)) {
                    saveOutFlag = true;
                }
            });
        }
        if (saveOutFlag) {
            utility.message("相同班别的输出量已经存在，请查看！");
            return;
        }

        $scope.currentItemOut.RegNo = $scope.currentResident.RegNo;
        $scope.currentItemOut.FeeNo = $scope.currentResident.FeeNo;
        outValueRes.save($scope.currentItemOut, function (data) {
            if (!angular.isDefined(item.OutNo)) {
                $scope.Data.OutList.push(data.Data);
            }
        });
        $scope.currentItemOut = {};
        utility.message($scope.currentResident.Name + "的输出量信息保存成功！");
    };
}])
.controller("outInValueListCtrl", ['$scope', '$stateParams', 'utility', 'inValueRes', 'outValueRes', function ($scope, $stateParams, utility, inValueRes, outValueRes) {
    var feeNo = $stateParams.feeNo || 0;
    $scope.init = function () {
        $scope.InList = [];
        $scope.OutList = [];
        $scope.curUser = utility.getUserInfo();
        $scope.currentItemIn = { FeeNo: feeNo, RecordBy: $scope.curUser.EmpNo };
        $scope.currentItemOut = { FeeNo: feeNo, RecordBy: $scope.curUser.EmpNo };
        $scope.inOptions = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: inValueRes,//异步请求的res
            params: { feeNo: feeNo },
            success: function (data) {//请求成功时执行函数
                $scope.InList = data.Data;
                if ($scope.InList.length > 0) {
                    if (angular.isUndefined($scope.currentItemIn.InNo)) {
                        $scope.editIn($scope.InList[0]);
                    }
                } else {
                    $scope.resetIn();
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        $scope.outOptions = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: outValueRes,//异步请求的res
            params: { feeNo: feeNo },
            success: function (data) {//请求成功时执行函数
                $scope.OutList = data.Data;
                if ($scope.OutList.length > 0) {
                    if (angular.isUndefined($scope.currentItemOut.OutNo)) {
                        $scope.editOut($scope.OutList[0]);
                    }
                } else {
                    $scope.resetOut();
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    $scope.staffByInSelected = function (item) {
        $scope.currentItemIn.RecordBy = item.EmpNo;
        $scope.currentItemIn.RecordNameBy = item.EmpName;
    };

    $scope.staffByOutSelected = function (item) {
        $scope.currentItemOut.RecordBy = item.EmpNo;
        $scope.currentItemOut.RecordNameBy = item.EmpName;
    };
    $scope.clearStaffIn = function () {
        if (angular.isDefined($scope.currentItemIn)) {
            $scope.currentItemIn.RecordBy = "";
        }
    }
    $scope.clearStaffOut = function () {
        if (angular.isDefined($scope.currentItemOut)) {
            $scope.currentItemOut.RecordBy = "";
        }
    }
    //删除输入量记录
    $scope.deleteIn = function (item) {
        if (confirm("您确定要删除该住民的输入量记录吗?")) {
            inValueRes.delete({ id: item.InNo }, function () {
                $scope.currentItemIn = { FeeNo: feeNo };
                $scope.inOptions.pageInfo.CurrentPage = 1;
                $scope.inOptions.search();
                utility.message("输入量记录信息删除成功！");
            });
        }
    };
    //删除输出量记录
    $scope.deleteOut = function (item) {
        if (confirm("您确定要删除该住民的输出量记录吗?")) {
            outValueRes.delete({ id: item.OutNo }, function () {
                $scope.currentItemOut = { FeeNo: feeNo };
                $scope.outOptions.pageInfo.CurrentPage = 1;
                $scope.outOptions.search();
                utility.message("输出量记录信息删除成功！");
            });
        }
    };

    $scope.editIn = function (item) {
        $scope.currentItemIn = item;
    };

    $scope.editOut = function (item) {
        $scope.currentItemOut = item;
    };

    $scope.saveIn = function (item) {
        //item.FeeNo = feeNo;
        var list = [];
        list.push(item);
        inValueRes.save(list, function (data) {
            $scope.inOptions.search();
            utility.message("输入量信息保存成功！");
        });

    };

    $scope.saveOut = function (item) {
        var list = [];
        list.push(item);
        outValueRes.save(list, function (data) {
            $scope.outOptions.search();
            utility.message("输出量信息保存成功！");
        });
    };


    $scope.clearStaffIn = function () {
        if (angular.isDefined($scope.currentItemIn)) {
            $scope.currentItemIn.RecordBy = $scope.curUser.EmpNo;
        }
    }
    $scope.clearStaffOut = function () {
        if (angular.isDefined($scope.currentItemOut)) {
            $scope.currentItemOut.RecordBy = $scope.curUser.EmpNo;
        }
    }
    $scope.resetIn = function () {
        $scope.curUser = utility.getUserInfo();
        $scope.currentItemIn = { FeeNo: feeNo, RecordBy: $scope.curUser.EmpNo };
        $scope.currentItemOut = { FeeNo: feeNo, RecordBy: $scope.curUser.EmpNo };

    };
    $scope.resetOut = function () {
        $scope.curUser = utility.getUserInfo();
        $scope.curUser = utility.getUserInfo();
        $scope.currentItemIn = { FeeNo: feeNo, RecordBy: $scope.curUser.EmpNo };
        $scope.currentItemOut = { FeeNo: feeNo, RecordBy: $scope.curUser.EmpNo };
    };

    $scope.init();
}]);
