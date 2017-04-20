angular.module("sltcApp")
.controller("reportTempManageCtrl", ['$scope','$q', 'ReportTempManageRes', 'utility', function ($scope,$q, ReportTempManageRes, utility) {
    Handlebars.registerHelper("prettifyDate", function (timestamp) {
        return timestamp ? moment(timestamp).format('YYYY-MM-DD') : null;
    });
    $scope.init = function () {
        $scope.data = {};
        $scope.showTab = false;
        $scope.eDate = moment().format('YYYY-MM');
        $scope.sDate = moment().subtract(1, "M").format('YYYY-MM');
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: ReportTempManageRes,//异步请求的res
            params: { startDate: $scope.sDate, endDate: $scope.eDate, mark: "" },
            success: function (data) {//请求成功时执行函数
                if (data.RecordsCount == 0) {
                    utility.msgwarning("暂无数据！");
                }
                $scope.Data = data.Data;
                $scope.showTab = !!$scope.Data;
                if (!$scope.reportType) return;
                var source = '';
                switch ($scope.reportType) {
                    case "MonthFee":
                        source = "#MonthFee";
                        break;
                    default:
                        source = "#MonthFee";
                        break;
                }
                var code = $(source).html();
                var template = Handlebars.compile(code);
                var dom = template($scope.Data);
                $("#reportListTab").html(dom);
            },
            error: function (error) {
                utility.msgwarning("护理险平台无法连接，请联系管理员！");
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 1000
            }
        }
    };

    $scope.exportRecort = function () {
        switch ($scope.reportType) {
            case "MonthFee":
                window.open("/ExportExcelReport/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("MonthFeeReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            default:
                utility.message("请选择报表类型！");
        }
    }

    $scope.Search = function () {
        if (!$scope.sDate) {
            utility.message("请选择开始月份！");
            return;
        }
        if (!$scope.eDate) {
            utility.message("请选择结束月份！");
            return;
        }
        if (!$scope.reportType) {
            utility.message("请选择报表名称！");
            return;
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.mark = $scope.reportType;
        $scope.options.params.startDate = $scope.sDate;
        $scope.options.params.endDate = $scope.eDate;
        $scope.options.search();
    }


    $scope.init();


}])