/*
创建人: 肖国栋
创建日期:2016-06-24
说明:报表管理
*/
angular.module("sltcApp")
.controller("reportSetCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'reportManageSetRes', function ($scope, $http, $location, $state, utility, reportManageSetRes) {
    $scope.Status = false;

    reportManageSetRes.get({ CurrentPage: 1, PageSize: 1000 }, function (data) {
        $scope.reportList = data.Data;
    });

    $scope.MajorName = function (majorType) {
        return majorType == 1 ? "社工" : majorType == 2 ? "护理" : majorType == 3 ? "指标" : "";
    };

    $scope.selectAll = function () {
        $.each($scope.reportList, function (i, v) {
            $.each(v.Items, function (ci, cv) {
                cv.Status = !$scope.Status;
            });
        });
    };

    $scope.chk = function () {
        var num_all = $("#chkdiv :checkbox").size();
        var num_checked = $("#chkdiv :checkbox:checked").size(); //选中个数 

        alert(num_all);
        alert(num_checked);
        if (num_all == num_checked) { //若选项总个数等于选中个数  
            $scope.Status = true;
        } else {
            $scope.Status = false;
        }
    };


        $scope.save = function () {
        var postData = $scope.reportList;
        reportManageSetRes.save(postData, function () {
            utility.message("保存成功！");
        });
    };
}])
.controller("reportManageCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'reportManageRes', function ($scope, $http, $location, $state, utility, reportManageRes) {
    $scope.FeeNo = $state.params.FeeNo;
    var date = new Date();
    $scope.code = "";
    $scope.reportType = "";
    $scope.feeNo = 0;
    $scope.startDate = new Date(date.getFullYear(), date.getMonth() - 1, date.getDate()).format("yyyy-MM-dd");
    $scope.endDate = date.format("yyyy-MM-dd");
    reportManageRes.get({ CurrentPage: 1, PageSize: 1000 }, function (data) {
        $scope.reportList = data.Data;
    });

    $scope.MajorName = function (majorType) {
        return majorType == 1 ? "社工" : majorType == 2 ? "护理" : majorType == 3 ? "指标" : "";
    };

    $scope.residentSelected = function (item) {
        $scope.feeNo = item.FeeNo;
    };

    $scope.clickRecort = function (item) {
        $scope.code = item.Code;
        $scope.reportType = item.ReportType;
        $scope.filterType = item.FilterType;
        switch ($scope.code) {
            case "H20":
                utility.message("仅要确定起始日期年份");
                break;
            case "P21Report":
                utility.message(("仅要确定起始日期"));
                break;
            case "P11Report":
                utility.message(("仅要确定起始日期年月"));
                break;
            case "H49":
                utility.message("仅要确定起始日期年份");
                break;
            case "H50":
                utility.message("仅要确定起始日期年份");
                break;
            case "P22Report":
                utility.message(("仅要确定起始日期年月"));
                break;
            case "P23Report":
                utility.message(("仅要确定起始日期年月"));
                break;
        }
    };

    //获得某月的天数
    function getMonthDays(nowYear, myMonth) {
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }

    //获得本季度的开始月份
    function getQuarterStartMonth(){
        var quarterStartMonth = 0;
        if(nowMonth<3){
            quarterStartMonth = 0;
        }
        if(2<6){
            quarterStartMonth = 3;
        }
        if(5<9){
            quarterStartMonth = 6;
        }
        if(nowMonth>8){
            quarterStartMonth = 9;
        }
        return quarterStartMonth;
    }

    $scope.setTime = function (type) {
        var nowMonth = date.getMonth(); //当前月
        var nowYear = date.getFullYear(); //当前月
        var day = date.getDay();
        var monday = day != 0 ? day - 1 : 6;

        switch (type) {
            case 1:
                $scope.startDate = new Date(new Date().setDate(date.getDate() - 1)).format("yyyy-MM-dd");
                $scope.endDate = new Date(new Date().setDate(date.getDate()- 1)).format("yyyy-MM-dd");
                break;
            case 2:
                $scope.startDate = date.format("yyyy-MM-dd");
                $scope.endDate = date.format("yyyy-MM-dd");
                break;
            case 3:
                $scope.startDate = new Date(new Date().setDate(date.getDate() + 1)).format("yyyy-MM-dd");
                $scope.endDate = new Date(new Date().setDate(date.getDate() + 1)).format("yyyy-MM-dd");
                break;
            case 4:
                $scope.startDate = new Date(new Date().setDate(date.getDate() - monday - 7)).format("yyyy-MM-dd");
                $scope.endDate = new Date(new Date().setDate(date.getDate() - monday - 1)).format("yyyy-MM-dd");
                break;
            case 5:
                $scope.startDate = new Date(new Date().setDate(date.getDate() - monday)).format("yyyy-MM-dd");
                $scope.endDate = new Date(new Date().setDate(date.getDate() + 6 - monday)).format("yyyy-MM-dd");
                break;
            case 6:
                $scope.startDate = new Date(new Date().setDate(date.getDate() - monday + 7)).format("yyyy-MM-dd");
                $scope.endDate = new Date(new Date().setDate(date.getDate() - monday + 13)).format("yyyy-MM-dd");
                break;
            case 7:
                var preMonthDate = new Date(); //上月日期
                preMonthDate.setDate(1);
                preMonthDate.setMonth(preMonthDate.getMonth() - 1);
                var preYear = preMonthDate.getFullYear();
                var preMonth = preMonthDate.getMonth();
                $scope.startDate = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
                $scope.endDate = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");
                break;
            case 8:
                $scope.startDate = new Date(nowYear, nowMonth, 1).format("yyyy-MM-dd");
                $scope.endDate = new Date(nowYear, nowMonth, getMonthDays(nowYear, nowMonth)).format("yyyy-MM-dd");
                break;
            case 9:
                var nextMonthDate = new Date(); //下月日期
                nextMonthDate.setDate(1);
                nextMonthDate.setMonth(nextMonthDate.getMonth() + 1);
                var nextYear = nextMonthDate.getFullYear();
                var nextMonth = nextMonthDate.getMonth();
                $scope.startDate = new Date(nextYear, nextMonth, 1).format("yyyy-MM-dd");
                $scope.endDate = new Date(nextYear, nextMonth, getMonthDays(nextYear, nextMonth)).format("yyyy-MM-dd");
                break;
            case 10:
                $scope.startDate = new Date(nowYear - 1, 0, 1).format("yyyy-MM-dd");
                $scope.endDate = new Date(nowYear - 1, 11, 31).format("yyyy-MM-dd");
                break
            case 11:
                $scope.startDate = new Date(nowYear, 0, 1).format("yyyy-MM-dd");
                $scope.endDate = new Date(nowYear, 11, 31).format("yyyy-MM-dd");
                break;
        }

    };

    $scope.executeRecort = function () {
        if ($scope.code == "" || $scope.code == null || typeof ($scope.code) == "undefined") {
            utility.message("请先选择报表");
            return;
        }
        var msg = "";
        if ($scope.code == "H76" || $scope.code == "H77" || $scope.code == "P15Report") {
            var days = DateDiff($scope.endDate, $scope.startDate);
            if (days < 0) {
                utility.message("结束日期不能小于开始日期");
                $scope.endDate = "";
                return;
            }
        }

        var vs = $scope.filterType.split("|");
        $.each(vs, function (i, v) {
            switch (v) {
                case "b":
                    if ($scope.feeNo == "0" || $scope.feeNo == null || typeof ($scope.feeNo) == "undefined") {
                        msg += "住民、";
                    }
                    break;
                case "s":
                    if ($scope.startDate == "" || $scope.startDate == null || typeof ($scope.startDate) == "undefined") {
                        msg += "开始日期、";
                    }
                    break;
                case "e":
                    if ($scope.endDate == "" || $scope.endDate == null || typeof ($scope.endDate) == "undefined") {
                        msg += "结束日期、";
                    }
                    break;
            }
        });
        if (msg != "") {
            msg = "请先选择" + msg.substring(0, msg.length - 1);
            utility.message(msg);
            return;
        }
        switch ($scope.reportType) {
            case "1"://导出excle 
                var feeNo= $scope.feeNo;
                window.open("/api/Report/{0}?feeNo={1}&startDate={2}&endDate={3}".format($scope.code, feeNo, $scope.startDate, $scope.endDate), "_blank");
                break;
            case "2"://导出word
                window.open("/Report/Export?templateName={0}&key={1}&startDate={2}&endDate={3}".format($scope.code, $scope.feeNo, $scope.startDate, $scope.endDate), "_blank");
                break;
            case "3"://打印word
                window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format($scope.code, $scope.feeNo, $scope.startDate, $scope.endDate), "_blank");
                break;
        };
    };

    $scope.active = function (code) {
        return $scope.code == code ? " active" : "";
    };
}]);

