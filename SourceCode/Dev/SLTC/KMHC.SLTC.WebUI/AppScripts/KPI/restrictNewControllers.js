/*
创建人:刘美方
创建日期:2016-05-6
说明: 约束
*/
angular.module("sltcApp")
.controller("restrictNewCtrl",['$scope', '$state', '$filter', '$http', 'restrictRes', 'restrictDetailRes', 'utility',function ($scope, $state, $filter, $http, restrictRes, restrictDetailRes, utility) {
    $scope.FeeNo = $state.params.FeeNo;
    var d = new Date(), nowDate = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss"), classType = utility.getClassType(d);
    $scope.init = function () {
        $scope.curUser = utility.getUserInfo();
        $scope.curResident = $scope.curResident || {};
        $scope.curItem = {
            StartDate: nowDate, RecordBy: $scope.curUser.EmpNo, ClassType: classType,
            Days: 30, RegNo: $scope.curResident.RegNo,
            FeeNo: $scope.curResident.FeeNo
        };
        $scope.curDelItem = {};
        $scope.editShow = false;
        $scope.editDetailShow = false;
        $scope.restrictList = [];
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: restrictRes,//异步请求的res
            params: { feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo },
            success: function (data) {//请求成功时执行函数
                $scope.restrictList = data.Data;
                if ($scope.restrictList.length > 0) {
                    //$scope.editDetailShow = true;
                    //if (angular.isDefined($scope.curItem.SeqNo)) {
                    //    $scope.edit($scope.curItem);
                    //} else {
                    //    $scope.edit($scope.restrictList[0]);
                    //}
                } //else {
                //    $scope.editDetailShow = false;
                //    $scope.curItem = {
                //        StartDate: nowDate, RecordBy: $scope.curUser.EmpNo, ClassType: classType,
                //        Days: 30, RegNo: $scope.curResident.RegNo,
                //        FeeNo: $scope.curResident.FeeNo
                //    };
                //    $scope.edit($scope.curItem);
                //}
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        $scope.curItem.Detail = [];
        $scope.detailOptions = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: restrictDetailRes,//异步请求的res
            params: { seqNo: 0 },
            success: function (data) {//请求成功时执行函数
                $scope.curItem.Detail = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.curResident = resident;//获取当前住民信息
        //angular.extend($scope.curItem, { FeeNo: resident.FeeNo, RegNo: resident.RegNo });
        $scope.curItem = {
            StartDate: nowDate, RecordBy: $scope.curUser.EmpNo, ClassType: classType,
            Days: 30, RegNo: $scope.curResident.RegNo,
            FeeNo: $scope.curResident.FeeNo
        };
        $scope.options.params.feeNo = $scope.curItem.FeeNo;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
        $scope.editShow = true;
        $scope.editDetailShow = false;
    }

    $scope.staffSelected = function (item, t) {
        if (t === "RecordBy") {
            $scope.curItem.RecordBy = item.EmpNo;
            $scope.curItem.RecordByName = item.EmpName;
        } else if (t === "CancelExecBy") {
            $scope.curItem.CancelExecBy = item.EmpNo;
            $scope.curItem.CancelExecByName = item.EmpName;
        } else if (t === 'EvaluateBy') {
            $scope.curDelItem.EvaluateBy = item.EmpNo;
            $scope.curItem.EvaluateByName = item.EmpName;
        }
    }


    $scope.checkDate = function () {
        if ($scope.curItem.CancelDate != null && $scope.curItem.StartDate) {
            var days = DateDiff($scope.curItem.CancelDate, $scope.curItem.StartDate);
            if (days < 0) {
                utility.message("移除时间不能小于开始时间");
                $scope.curItem.CancelDate = "";
                return;
            }
        }
    };

    $scope.edit = function (item) {
        $scope.curItem = item;
        $scope.detailOptions.params.seqNo = item.SeqNo;
        $scope.editDetailShow = true;
        $scope.detailOptions.search();
    };

    $scope.delete = function (id) {
        if (confirm("确定删除该信息吗?")) {
            restrictRes.delete({ id: id }, function (data) {
                $scope.curItem = {
                    StartDate: nowDate, RecordBy: $scope.curUser.EmpNo, ClassType: classType,
                    Days: 30, RegNo: $scope.curResident.RegNo,
                    FeeNo: $scope.curResident.FeeNo
                };
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                utility.message("删除成功");
            });
        }
    };
    $scope.save = function () {
        $scope.checkDate();
        if (angular.isDefined($scope.form1.$error.required)) {
            for (var i = 0; i < $scope.form1.$error.required.length; i++) {
                utility.msgwarning($scope.form1.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.form1.$error.maxlength)) {
            for (var i = 0; i < $scope.form1.$error.maxlength.length; i++) {
                utility.msgwarning($scope.form1.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        restrictRes.save($scope.curItem, function (data) {
            $scope.curItem = data.Data;
            $scope.options.search();
            $scope.reset();
            utility.message("保存成功！");
        });
    };
    $scope.reset = function () {
        $scope.curItem = {
            StartDate: nowDate, RecordBy: $scope.curUser.EmpNo, ClassType: classType,
            Days: 30, RegNo: $scope.curResident.RegNo,
            FeeNo: $scope.curResident.FeeNo
        };
        $scope.editDetailShow = false;
    };

    $scope.editDetail = function (item) {

        $scope.curUser = utility.getUserInfo();
        $scope.curDelItem = item;
            
        if (angular.isUndefined($scope.curDelItem.EvaluateBy)) {
            $scope.curDelItem.EvaluateBy = $scope.curUser.EmpNo;
        }
        $("#modalDetail").modal("toggle");
    };
    $scope.delDetail = function (item) {
        if (confirm("您确定要删除该住民的压疮记录吗?")) {
            restrictDetailRes.delete({ id: item.Id }, function (data) {
                $scope.detailOptions.pageInfo.CurrentPage = 1;
                $scope.detailOptions.search();
                utility.message("删除成功");
            });
        }
    };
    $scope.saveDetail = function () {
        $scope.curDelItem.SeqNo = $scope.curItem.SeqNo;
        restrictDetailRes.save($scope.curDelItem, function (data) {
            $("#modalDetail").modal("toggle");
            $scope.detailOptions.search();
        });
    };

    $scope.init();
}]);





