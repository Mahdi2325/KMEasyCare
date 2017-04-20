/*
创建人:张凯
创建日期:2016-03-10
说明: 请假
*/
angular.module("sltcApp")
.controller("resBirthDayListCtrl", ['$scope', '$filter', '$location', 'utility', 'resBirthDayListRes', '$state', function ($scope, $filter, $location, utility, resBirthDayListRes, $state) {
    //开始时间和结束时间
    var preMonthDate = new Date();
    preMonthDate.setDate(1);
    preMonthDate.setMonth(preMonthDate.getMonth());
    var preYear = preMonthDate.getFullYear();
    var preMonth = preMonthDate.getMonth();
    $scope.StartDate = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
    $scope.EndDate = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");

    //获得某月的天数
    function getMonthDays(nowYear, myMonth) {
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }


    //初始化数据源
    $scope.Data = {};
    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: resBirthDayListRes,//异步请求的res
        params: { sDate: $scope.StartDate, eDate: $scope.EndDate, keyWord: ""},
        success: function (data) {//请求成功时执行函数
            $scope.leaveHosps = data.Data;
            if ($scope.leaveHosps.length == 0) {
                utility.msgwarning("没有住民的生日在此查询区间内");
            }
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }

    $scope.checksDate = function () {
        if (angular.isDefined($scope.StartDate) && angular.isDefined($scope.EndDate)) {
            if (!checkDate($scope.StartDate, $scope.EndDate)) {
                utility.msgwarning("生日开始日期应该在生日截止日期之前");
                $scope.EndDate = "";
                return;
            }
        }
    }

    $scope.checkeDate = function () {
        if ($scope.StartDate != "" && $scope.EndDate != "") {
            if (angular.isDefined($scope.StartDate) && angular.isDefined($scope.EndDate)) {
                if (!checkDate($scope.StartDate, $scope.EndDate)) {
                    utility.msgwarning("生日截止日期应该在生日开始日期之后");
                    $scope.EndDate = "";
                    return;
                }
            }
        }
        else {
            utility.msgwarning("生日开始日期或截止日期不能为空");
            return;
        }
    }



    $scope.searchInfo = function () {
        $scope.options.params.sDate = $scope.StartDate;
        $scope.options.params.eDate = $scope.EndDate;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

}]);

