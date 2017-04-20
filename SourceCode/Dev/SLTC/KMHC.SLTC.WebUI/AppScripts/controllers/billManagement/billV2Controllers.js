
angular.module("sltcApp")
.controller("BillV2Ctrl", ['$scope', '$filter', '$stateParams', 'billV2Res', 'utility', '$state', function ($scope, $filter, $stateParams, billV2Res, utility, $state) {
    $scope.FeeNo = $state.params.FeeNo;

    //开始时间和结束时间
    var preMonthDate = new Date();
    preMonthDate.setDate(1);
    preMonthDate.setMonth(preMonthDate.getMonth());
    var preYear = preMonthDate.getFullYear();
    var preMonth = preMonthDate.getMonth();
    $scope.sDate = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
    $scope.eDate = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");

    //获得某月的天数
    function getMonthDays(nowYear, myMonth) {
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }

    $scope.init = function () {
        $scope.billV2List = {};
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: billV2Res,//异步请求的res
            params: { FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo, sDate: $scope.sDate, eDate: $scope.eDate },
            success: function (data) {//请求成功时执行函数
                if (data.Data.length == 0) {
                    utility.message("查询区间内，未查询到有效的账单数据！");
                    $scope.billV2List = {};
                    return;
                }
                else {
                    $scope.billV2List = data.Data;
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.checksDate = function () {
        if (angular.isDefined($scope.sDate) && angular.isDefined($scope.eDate)) {
            if (!checkDate($scope.sDate, $scope.eDate)) {
                utility.msgwarning("账单开始日期必须在账单结束日期之前");
                $scope.eDate = "";
                return;
            }
        }
    }

    $scope.checkeDate = function () {
        if ($scope.sDate != "" && $scope.eDate != "") {
            if (angular.isDefined($scope.sDate) && angular.isDefined($scope.eDate)) {
                if (!checkDate($scope.sDate, $scope.eDate)) {
                    utility.msgwarning("账单结束日期必须在账单开始日期之后");
                    $scope.eDate = "";
                    return;
                }
            }
        }
        else {
            utility.msgwarning("账单开始日期或账单结束日期不能为空");
            return;
        }
    }

    $scope.searchInfo = function ()
    {
        $scope.checksDate();
        $scope.checkeDate();

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = $scope.FeeNo;
        $scope.options.params.sDate = $scope.sDate;
        $scope.options.params.eDate = $scope.eDate;
        $scope.options.search();
    }

    $scope.txtResidentIDChange = function (resident) {
        $scope.checksDate();
        $scope.checkeDate();

        $scope.curResident = resident;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = resident.FeeNo;
        $scope.options.params.sDate = $scope.sDate;
        $scope.options.params.eDate = $scope.eDate;
        $scope.FeeNo = resident.FeeNo;
        $scope.options.search();
    }

    $scope.getFeeRerordInfo = function (billId)
    {
        $scope.drugRecordList = {};
        $scope.materialRecordList = {};
        $scope.serviceRecordList = {};
        $scope.residentBalance = [];

        billV2Res.get({ feeNo: $scope.FeeNo, billId: billId }, function (data) {
            if (data.ResultCode == -1 || data.ResultCode == 1001) {
                utility.msgwarning("查询失败" + data.ResultMessage);
            }
            else {
                $scope.drugRecordList = data.Data.drugRecordList;
                $scope.materialRecordList = data.Data.materialRecordList;
                $scope.serviceRecordList = data.Data.serviceRecordList;
                $scope.residentBalance = data.Data.residentBalance;
            }
        });
    }

    $scope.init();
}])