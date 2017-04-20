angular.module("sltcApp")
.controller("MonFeeListCtrl", ['$scope', '$state', '$http', 'billV2Res', 'utility', function ($scope, $state, $http, billV2Res, utility) {
    $scope.init = function () {
        $http({
            url: 'api/billv2/GetOrgMonthDataList?beginTime=' + (typeof ($scope.sDate) == "undefined" ? "" : $scope.sDate) + '&endTime=' + (typeof ($scope.eDate) == "undefined" ? "" : $scope.eDate),
            method: 'GET'
        }).success(function (data, header, config, status) {
            $scope.MonthFeeList = data.Data;

        }).error(function (data, header, config, status) {
            //处理响应失败
            //alert("护理险平台无法连接，请联系管理员！")
            utility.msgwarning("护理险平台无法连接，请联系管理员！");
        })
    }
    $scope.init();
    $scope.ShowDetail = function (NSMonFeeID) {
        $state.go("MonFeeUploadEdit", { NSMonFeeID: NSMonFeeID });
    }
    $scope.add = function () {
        $state.go("MonFeeUpload");
    }
    $scope.Print = function (NSMonFeeID, YearMonth) {
        window.open('/MonthFee/Preview?nSMonFeeID=' + NSMonFeeID + '&date=' + YearMonth);
    }
    $scope.cancel = function (date) {
        $http({
            url: 'api/billv2/CancelMonthData?date=' + date,
            method: 'GET'
        }).success(function (data, header, config, status) {
            $scope.init();

        }).error(function (data, header, config, status) {
            //处理响应失败
            //alert("护理险平台无法连接，请联系管理员！")
            utility.msgwarning("护理险平台无法连接，请联系管理员！");
        })
    }
}]);