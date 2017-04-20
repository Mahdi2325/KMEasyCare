
angular.module("sltcApp")
.controller("FeeRecordCtrl", ['$scope', '$filter', 'feeRecordRes', '$stateParams', 'utility', '$state', function ($scope, $filter, feeRecordRes, $stateParams, utility, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.IsHasRecord = false;
    $scope.isdisbtn = false;
    var taskStatusValue = "";


    $scope.init = function () {
        $scope.loadSearchInfo();
    };

    //初始化搜索框初始信息
    $scope.loadSearchInfo = function () {
        //上月开始时间和结束时间
        var preMonthDate = new Date();
        preMonthDate.setDate(1);
        preMonthDate.setMonth(preMonthDate.getMonth() - 1);
        var preYear = preMonthDate.getFullYear();
        var preMonth = preMonthDate.getMonth();
        //$scope.sDate = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
        //$scope.eDate = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");

        $scope.billMonth = new Date(preYear, preMonth).format("yyyy-MM");
        $scope.feeRecordList = {};
        //类型
        var taskStatusselect = $("#taskStatus").select2({
            placeholder: "选择类型",
            allowClear: true,
            closeOnSelect: false,
        });
        $scope.isdisbtn = false;
        $scope.searchInfo();
    };

    //获得某月的天数
    function getMonthDays(nowYear, myMonth) {
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }

    $scope.txtResidentIDChange = function (resident) {
        $scope.curResident = resident;
        $scope.FeeNo = $scope.curResident.FeeNo;
        $scope.loadSearchInfo();
    }


    $scope.checksDate = function () {
        if (angular.isDefined($scope.sDate) && angular.isDefined($scope.eDate)) {
            if (!checkDate($scope.sDate, $scope.eDate)) {
                utility.msgwarning("开始日期必须在结束日期之前");
                $scope.eDate = "";
                return;
            }
        }
    }

    $scope.checkeDate = function () {
        if ($scope.sDate != "" && $scope.eDate != "") {
            if (angular.isDefined($scope.sDate) && angular.isDefined($scope.eDate)) {
                if (!checkDate($scope.sDate, $scope.eDate)) {
                    utility.msgwarning("结束日期必须在开始日期之后");
                    $scope.eDate = "";
                    return;
                }
            }
        }
    }

    $scope.SetSearchDate = function () {
        if ($scope.billMonth == "" || $scope.billMonth == null) {
            $scope.sDate = "";
            $scope.eDate = "";
            utility.msgwarning("结算月份不能为空");
            return;
        }
        else {
            var yearstr = $scope.billMonth.slice(0, 5);
            var monthstr = $scope.billMonth.slice(5, 7);
            monthstr = monthstr > 9 ? monthstr : monthstr.slice(1, 2);
            utility.getFeeIntervalByMonth(monthstr).$promise.then(function (res) {
                $scope.sDate = yearstr + res.StartDate;
                $scope.eDate = yearstr + res.EndDate;
            });
        }
    };


    $scope.searchInfo = function () {
        $scope.feeRecordList = {};
        $scope.SetSearchDate();

        if ($('#taskStatus').val() == null) {
            utility.msgwarning("请选择生成账单的使用记录类型！");
            return;
        }
        else {
            taskStatusValue = $('#taskStatus').val().toString();
        }

        if ($scope.FeeNo != "") {
            $scope.feeRecordList = {};
            feeRecordRes.get({ FeeNo: $scope.FeeNo, sDate: $scope.sDate, eDate: $scope.eDate, taskStatus: taskStatusValue }, function (data) {
                if (data.ResultCode == -1) {
                    utility.msgwarning("获取数据失败，" + data.ResultMessage);
                    $scope.IsHasRecord = false;
                }
                else if (data.ResultCode == 1001) {
                    utility.message("所选时间内,查无数据！");
                    $scope.IsHasRecord = false;
                }
                else {
                    $scope.feeRecordList = data.Data;
                    $scope.IsHasRecord = true;
                }
            });
        }
        else {
            utility.msgwarning("请选择住民！");
            return;
        }
    };

    $scope.GenerateBill = function () {
        $scope.isdisbtn = true;
        $scope.BillV2Info = {};
        $scope.BillV2Info.Feeno = $scope.FeeNo;
        $scope.BillV2Info.STime = $scope.sDate;
        $scope.BillV2Info.ETime = $scope.eDate;
        $scope.BillV2Info.IdNo = $scope.curResident.IdNo;
        $scope.BillV2Info.BillMonth = $scope.billMonth;
        $scope.BillV2Info.feeRecordList = [];

        $scope.feeRecordList.forEach(function (item) {
            if (item.IsChecked) {
                $scope.BillV2Info.feeRecordList.push(item);
            }
        });

        feeRecordRes.save($scope.BillV2Info, function (data) {
            if (data.ResultCode == 1001) {
                utility.message(data.ResultMessage);
                $scope.searchInfo();
                $scope.isdisbtn = false;
            }
            else if (data.ResultCode == -1) {
                utility.msgwarning(data.ResultMessage);
                $scope.isdisbtn = false;
            }
        });
    }
    $scope.init();
}])