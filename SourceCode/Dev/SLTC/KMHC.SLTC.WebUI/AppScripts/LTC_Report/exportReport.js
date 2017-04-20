angular.module("sltcApp")
    .controller('exportReportCtrl', ['$scope', 'utility', 'floorRes', '$state', function ($scope, utility, floorRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    //当前选中的报表项
    $scope.currentCode = "";
    //住民FeeNo
    $scope.feeNo = 0;
    $scope.showtype = false;

    $scope.init = function () {
        var date = new Date();
        $scope.startDate = date.format("yyyy-MM-dd");
        $scope.endDate = date.format("yyyy-MM-dd");
        floorRes.get({ floorName: "" }, function (data) {
            $scope.floors = data.Data;

        });
    }
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.feeNo = resident.FeeNo
        $scope.floorId = "";
    }
    //导出
    $scope.exportRecort = function () {
        if ($scope.currentCode == "P18Report") {
            if ($scope.classType == null) {
                utility.message("请选择班别！");
                return;
            }
        }

        switch ($scope.currentCode) {
            //巴氏量表评估
            case "ADL":
                window.open("/EvalReport/Export?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("ADLReport", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //柯氏量表评估
            case "KARNOFSKY":
                window.open("/EvalReport/Export?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("ColeScaleReport", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //压疮风险评估
            case "SORE":
                window.open("/EvalReport/Export?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("PrsSoreReport", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //跌倒风险评估
            case "FALL":
                window.open("/EvalReport/Export?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("FallReport", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //给药记录单
            case "PILL":
                window.open("/EvalReport/Export?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("PillReport", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //工具性日常生活功能量表评估
            case "IADL":
                window.open("/EvalReport/Export?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("IADLReport", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //护理记录
            case "P18Report":
                window.open("/EvalReport/CarePlanExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&classType={4}&floorId={5}&printOrder={6}".format("P18Report", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.classType, $scope.floorId, $scope.printOrder), "_blank");
                break;
                //护理计划
            case "H10Report":
                window.open("/EvalReport/CarePlanExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("H10Report", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //护理需求评估
            case "H35Report":
                window.open("/EvalReport/CarePlanExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("H35Report", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
                //个案画活动需求评估及计划
            case "DLN1.1":
                window.open("/EvalReport/CarePlanExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("DLN1.1", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId), "_blank");
                break;
            case "NewRegEnvAdaptation":
                window.open("/ResidentReport/ResidentExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("LTC_NEWEnvironmentTutor", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId));
                break;
            case "NewResideEntenvRec":
                window.open("/ResidentReport/ResidentExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}&floorId={4}".format("LTC_NEWEnvironmentRec", $scope.startDate, $scope.endDate, $scope.feeNo, $scope.floorId));
                break;
            case "PersonList":
                window.open("/ResidentReport/ResidentExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}".format("PA001", $scope.startDate, $scope.endDate, $scope.feeNo));
                break;
                //住民在院证明
            case "HosProofReport":
                if ($scope.feeNo == 0)
                {
                   utility.msgwarning("请先选择需要打印住院证明的住民！");
                    return;
                }
                window.open("/EvalReport/OtherExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}".format("HosProofReport", $scope.startDate, $scope.endDate, $scope.feeNo));
                break;
            default:
        }

    }

    //选中某个报表
    $scope.clickRecort = function (val) {
        if (val == "P18Report") {
            $scope.showtype = true;
        }
        else {
            $scope.showtype = false;
        }
        $scope.currentCode = val;
    }

    $scope.init();
}])
    .controller('exportPeopleFeeReportCtrl', ['$scope','ReportTempManageRes', 'utility', 'floorRes', '$state', function ($scope,ReportTempManageRes, utility, floorRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    //当前选中的报表项
    $scope.currentCode = "";
    //住民FeeNo
    $scope.feeNo = 0;
    $scope.showtype = false;
    $scope.reportTypeList = [{ name: "费用清单", value: "FeeList" }];
    $scope.reportType = 'FeeList';
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.feeNo = resident.FeeNo;
        $scope.floorId = "";
    }

    $scope.init = function () {

        floorRes.get({ floorName: "" }, function (data) {
            $scope.floors = data.Data;

        });

        $scope.data = {};
        $scope.eDate = moment().format('YYYY-MM');
        $scope.sDate = moment().subtract(1, "M").format('YYYY-MM');
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: ReportTempManageRes,//异步请求的res
            params: { startDate: $scope.sDate, endDate: $scope.eDate, feeNo: $scope.feeNo, mark: "FeeList" },
            success: function (data) {//请求成功时执行函数
                if($scope.feeNo==0){
                    utility.msgwarning("请先选择住民~");
                    return;
                }
                if (!data.Data.feeRecordList.length) {
                    utility.showNoData();
                    return;
                }
                $scope.Data = data.Data;
                if (!$scope.reportType) return;
                var source = '';
                switch ($scope.reportType) {
                    case "FeeList":
                        source = "#FeeList";
                        break;
                    default:
                        source = "#FeeList";
                        break;
                }
                var code = $(source).html();
                var template = Handlebars.compile(code);
                var dom = template($scope.Data);
                $("#reportListTab").html(dom);
            },
            error: function (error) {
                utility.msgwarning("！");
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
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
            $scope.options.params.feeNo = $scope.feeNo;
            $scope.options.params.startDate = $scope.sDate;
            $scope.options.params.endDate = $scope.eDate;
            $scope.options.params.mark = $scope.reportType;
            $scope.options.search();
        }

    //导出
    $scope.exportRecort = function () {
        switch ($scope.reportType) {
            case "FeeList":
                window.open("/EvalReport/FeeListExport?templateName={0}&startDate={1}&endDate={2}&feeNo={3}".format("FeeListReport", $scope.sDate, $scope.eDate, $scope.feeNo), "_blank");
                break;
            default:
                utility.message("请选择报表类型！");
        }

    }
    //批量导出
    $scope.exportAllRecort = function () {
        switch ($scope.reportType) {
            case "FeeList":
                utility.message("只导出享有护理险人员的费用清单！");
                window.open("/EvalReport/AllFeeListExport?templateName={0}&startDate={1}&endDate={2}".format("FeeListReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            default:
                utility.message("请选择报表类型！");
        }

    }


    $scope.init();
}]);
