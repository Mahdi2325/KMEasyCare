angular.module("sltcApp").controller("OldtransdisciplinaryTimelineCtrl", ['$rootScope', '$scope', 'utility', '$state', '$filter', function ($rootScope, $scope, utility, $state, $filter) {
    $scope.FeeNo = $state.params.FeeNo;

    $scope.btn_s = true;
    $scope.showRemarks = false;
    $scope.init = function () {
        $scope.residentInfo = {};
        $scope.Data = {};
        if ($scope.FeeNo == null || $scope.FeeNo == "") {
            $scope.btn_s = true;
        }


    };
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.Data = {};
        $scope.residentInfo = resident;
        $scope.FeeNo = resident.FeeNo
        if ($scope.FeeNo != null && $scope.FeeNo != "") {
           // $scope.btn_s = false;
        }
       // $scope.Search();
    }

    $scope.Search = function (tag) {
        if (!angular.isDefined($scope.StartDate) || $scope.StartDate == null) {
            $scope.StartDate = "";
        }
        if (!angular.isDefined($scope.EndDate) || $scope.EndDate == null) {
            $scope.EndDate = "";
        }
        if (angular.isDefined($scope.FeeNo) && $scope.FeeNo != null && $scope.FeeNo != "") {
            window.open('/DC_Report/Timeline?feeNo=' + $scope.FeeNo + '&name=' + $scope.residentInfo.Name + '&startDate=' + $scope.StartDate + '&endDate=' + $scope.EndDate + '&tag=' + tag, '_blank');
        } else {
            utility.message("请先在住民列表中选择住民!");
        }
       
    };

}])

