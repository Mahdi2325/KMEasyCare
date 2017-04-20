angular.module("sltcApp")
.controller("crossNavigateCtrl", ['$scope', '$state', '$location', function ($scope, $state, $location) {
    $scope.Data = {};
    $scope.FeeNo = $state.params.FeeNo;
    var residentInfo = {};
    $scope.residentSelected = function (resident) {
        residentInfo = resident;
    }

    $scope.NavigateLink = function (name) {
        var url = '';
        switch (name) {
            case 'Careplan': //照顾计划表
                url = '/dc/DCCareplanCtrl/';
                break;
            case 'NurseCareLife': //护理及生活照顾服务纪录表
                url = '/dc/NurseCareLifeService/';
                break;
            case 'DCDayLife': //日常生活照顾记录表
                url = '/dc/DCDayLifeCare/';
                break;
            case 'DCProblem': //护理及生活照顾服务纪录表
                url = '/dc/DCProblemBehavior/';
                break;
            case 'DCplan': //照顾计划表
                url = '/dc/DCTransdisciplinaryPlan/';
                break;
        }
        if (angular.isDefined(residentInfo.FeeNo)) {
            url += residentInfo.FeeNo;
        }
        $location.url(url);

    }

   
}])
