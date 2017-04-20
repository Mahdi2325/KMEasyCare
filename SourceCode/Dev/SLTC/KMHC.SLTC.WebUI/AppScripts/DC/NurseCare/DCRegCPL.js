angular.module("sltcApp").controller("DCRegCPLCtrl", ['$rootScope', '$scope', 'DCRegCPLRes', 'utility', '$state', function ($rootScope, $scope, DCRegCPLRes, utility, $state) {

    $scope.FeeNo = $state.params.FeeNo;
    $scope.init = function () {
        $scope.OrgName = $rootScope.Global.Organization;
        $scope.buttonPrintShow = true;
    }

    $scope.Data = {};

    $scope.residentSelected = function (resident) {
        $scope.FeeNo = resident.FeeNo;
        $scope.buttonPrintShow = false;
        //
        $scope.currentResident = resident;
        $scope.currentResident.BirthDay = FormatDate(resident.BirthDay);

        //加载最近一笔数据到表单
        DCRegCPLRes.get({ feeNo: resident.FeeNo }, function (data) {
            $scope.Data.RegCPLList = data.Data;
        })
    }
    function FormatDate(strTime) {
        if (strTime == null || strTime == "") {
            return "";
        }
        var date = new Date(strTime);
        return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
    }


    $scope.PrintPreview = function () {
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            if ($scope.Data.RegCPLList==null)
            {
                utility.message("无打印数据");
                return;
            }
            window.open('/DC_Report/PreviewNursingReport?templateName=DCN1.4&feeNo=' + $scope.currentResident.FeeNo);
        } else {
            utility.message("无打印数据！");
        }

    }
    $scope.DownloadWord = function () {
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            if ($scope.Data.RegCPLList == null) {
                utility.message("无导出数据");
                return;
            }
            window.open('/DC_Report/DownLoadReport?templateName=DCN1.4&feeNo=' + $scope.currentResident.FeeNo);
        } else {
            utility.message("无导出数据！");
        }

    }
    $scope.init();
}])
