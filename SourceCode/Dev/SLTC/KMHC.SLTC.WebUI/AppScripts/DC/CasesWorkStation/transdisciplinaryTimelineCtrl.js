angular.module("sltcApp").controller("transdisciplinaryTimelineCtrl", ['$rootScope', '$scope', 'utility', '$state', '$filter', 'CasesTimelineRes', function ($rootScope, $scope, utility, $state, $filter, CasesTimelineRes) {
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
        $scope.FeeNo = resident.FeeNo;
        $('#feeNo').val(resident.FeeNo);
        if ($scope.FeeNo != null && $scope.FeeNo != "") {
           // $scope.btn_s = false;
        }
        if (!angular.isDefined($scope.StartDate) || $scope.StartDate == null) {
            $scope.StartDate = "";
        }
        if (!angular.isDefined($scope.EndDate) || $scope.EndDate == null) {
            $scope.EndDate = "";
        }
        $('#startDate').val($scope.StartDate);
        $('#endDate').val($scope.EndDate);
       // $scope.Search();
    }

    $scope.ChangeDate = function (type) {
        if (type == 'start') {
            $('#startDate').val($scope.StartDate);
        } else {
            $('#endDate').val($scope.EndDate);
        }
    }

    $scope.Search = function (tag) {
        if (!angular.isDefined($scope.StartDate) || $scope.StartDate == null) {
            $scope.StartDate = "";
        }
        if (!angular.isDefined($scope.EndDate) || $scope.EndDate == null) {
            $scope.EndDate = "";
        }
   
       
    };

}])

