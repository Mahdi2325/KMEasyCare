angular.module('sltcApp')
.controller('lifeListCtrl', ['$scope', '$state', '$stateParams', 'utility', 'dictionary', 'liferecordsRes', 'empFileRes',
function ($scope, $state,$stateParams, utility, dictionary, liferecordsRes, empFileRes) {

    var feeNo = $state.params.id;

    //$scope.buttonShow = true;

    $scope.init = function () {

        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.info = { RecordBy: $scope.curUser.EmpNo };
        }

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: liferecordsRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.LifeList = data.Data;
                if ($scope.LifeList != null) {

                    for (var n = 0; n < $scope.LifeList.length; n++) {
                        if ($scope.LifeList[n].RecordDate != null)
                            $scope.LifeList[n].RecordDate = newDate($scope.LifeList[n].RecordDate).format("yyyy-MM-dd hh:mm:ss")
                    }
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: { feeNo: feeNo }
        }


    };
    $scope.saveLife = function (item) {
        if ($scope.lifeForm.$valid) {
            var list = [];
            if (item != null) {
                item.FeeNo = $stateParams.id;
                list.push(item);
            }
            if (list.length > 0) {
                liferecordsRes.save(list, function (data) {
                    if (data.ResultCode == 0) {
                        utility.message("资料保存成功！");
                        $scope.init();
                    }
                    else {
                        utility.message(data.ResultMessage);
                    }
                })
            }
        }
        else {
            //验证没有通过
            $scope.getErrorMessage($scope.lifeForm.$error);
            $scope.errs = $scope.errArr.reverse();
            for (var n = 0; n < $scope.errs.length; n++) {
                if (n != 3) {
                    utility.msgwarning($scope.errs[n]);
                }
                //if (n > 2) break;
            }
        }
    };
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该条记录吗?")) {
            liferecordsRes.delete({ id: item.Id }, function (data) {
                if (data.$resolved) {
                    //    var whatIndex = null;
                    //    angular.forEach($scope.LifeList, function (cb, index) {
                    //        if (cb.id = item.Id) whatIndex = index;
                    //    });
                    if (data.ResultCode == 0)
                        $scope.LifeList.splice($scope.LifeList.indexOf(item), 1);
                    utility.message("资料删除成功！");
                    //$scope.LifeList.splice(whatIndex, 1);
                }
            });
        }
    }
    $scope.editLife = function (item) {
        $scope.info = item;
        //$scope.buttonShow = false;
    };
    //验证信息提示
    $scope.getErrorMessage = function (error) {
        $scope.errArr = new Array();
        //var errorMsg = '';
        if (angular.isDefined(error)) {
            if (error.required) {
                $.each(error.required, function (n, value) {
                    if (value.$name == null || value.$name == "") {
                        $scope.errArr.push("填写人员不能为空");
                    }
                    else {
                        $scope.errArr.push(value.$name + "不能为空");
                    }

                });
            }
            if (error.email) {
                $.each(error.email, function (n, value) {
                    $scope.errArr.push(value.$name + "邮箱验证失败");
                });
            }
            if (error.number, function (n, value) {
                $scope.errArr.push(value.$name + "只能录入数字");
            });

            //return errorMsg;
        }

    };
    $scope.staffSelected = function (item) {
        $scope.info.RecordBy = item.EmpNo;
        //$scope.info.RecordByName = item.EmpNo;
    }
    $scope.init();
}])
.controller('lifeRecordListCtrl', ['$scope', 'utility', 'dictionary', 'liferecordsRes', 'empFileRes',
function ($scope, utility, dictionary, liferecordsRes, empFileRes) {
    var feeNo = $stateParams.feeNo || 0;
    $scope.Data = {};

    //当前生活记录
    $scope.currentItem = null;
    //当前住民
    $scope.currentResident = {};
    //$scope.buttonShow = false;
    //当前分页码
    $scope.currentPage = 1;

    $scope.init = function () {
        empFileRes.get({ empNo: '' }, function (data) {
            $scope.Data.EmpList = data.Data;
        })
    }
    //选中住民
    $scope.residentSelected = function (resident) {

        $scope.currentResident = resident;

        $scope.getRecordByNo(resident.FeeNo);//加载当前住民的生活记录列表

        $scope.currentItem = {};//清空编辑项
        if (angular.isDefined($scope.currentResident)) {
            //$scope.buttonShow = true;
        }
        $scope.currentItem.RegNo = resident.RegNo;
        $scope.currentItem.FeeNo = resident.FeeNo;

        $scope.curUser = utility.getUserInfo();
        $scope.currentItem.RecordBy = $scope.curUser.EmpNo;
    }
    //获取住民生活记录
    $scope.getRecordByNo = function (feeNo) {
        $scope.Data.lifeRecords = {};
        liferecordsRes.get({ feeNo: feeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {
            $scope.Data.lifeRecords = data.Data;
            var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

                $scope.currentPage = curPage;
                $scope.getRecordByNo(feeNo);
            });
        });
    }
    $scope.saveLifeRecord = function (item) {

        liferecordsRes.save(item, function (data) {
            //utility.message("资料保存成功！");
            //$scope.Data.lifeRecords.push(data);
            if (angular.isDefined(item.Id)) {
                utility.message("资料更新成功！");
            }
            else {
                $scope.Data.lifeRecords.push(data.Data);
                utility.message("资料保存成功！");
            }
        });
        $scope.currentItem = {};
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
    }
    //删除生活记录
    $scope.lifeRecordDelete = function (item) {
        if (confirm("您确定要删除该条记录吗?")) {
            liferecordsRes.delete({ id: item.Id }, function (data) {
                if (data.$resolved) {
                    //var whatIndex = null;
                    //angular.forEach($scope.Data.lifeRecords, function (cb, index) {
                    //    if (cb.id = item.Id) whatIndex = index;
                    //});

                    if (data.ResultCode == 0)
                        utility.message("生活记录删除成功！");
                    $scope.Data.lifeRecords.splice($scope.Data.lifeRecords.indexOf(item), 1);

                }
            });
        }
    }
    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    }

    Date.prototype.format = function (format) {
        var date = {
            "M+": this.getMonth() + 1,
            "d+": this.getDate(),
            "h+": this.getHours(),
            "m+": this.getMinutes(),
            "s+": this.getSeconds(),
            "q+": Math.floor((this.getMonth() + 3) / 3),
            "S+": this.getMilliseconds()
        };
        if (/(y+)/i.test(format)) {
            format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
        }
        for (var k in date) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1
                       ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
            }
        }
        return format;
    };
    $scope.resetLR = function () {
        $scope.currentItem = {};
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
    }
    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.RecordBy = item.EmpNo;
        $scope.currentItem.RecordByName = item.EmpNo;
    }
    $scope.init();
}]);

