angular.module("sltcApp").controller("DCRegNursingDiagCtrl", ['$rootScope', '$scope', 'DCRegNursingDiagRes', 'utility', '$state', function ($rootScope,$scope, DCRegNursingDiagRes, utility, $state) {

    $scope.FeeNo = $state.params.FeeNo;
    $scope.init = function () {
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
        DCRegNursingDiagRes.get({ feeNo: resident.FeeNo }, function (data) {
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
            window.open('/DC_Report/PreviewNursingReport?templateName=DCN1.4&feeNo=' + $scope.currentResident.FeeNo);
        } else {
            utility.message("无法打印！");
        }

    }
    $scope.DownloadWord = function () {
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            window.open('/DC_Report/DownLoadReport?templateName=DCN1.4&feeNo=' + $scope.currentResident.FeeNo);
        } else {
            utility.message("无法导出！");
        }

    }
}])
//重复的可以删掉
