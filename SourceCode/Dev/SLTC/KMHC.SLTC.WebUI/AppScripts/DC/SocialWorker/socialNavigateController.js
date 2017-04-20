angular.module("sltcApp")
.controller("socialNavigateCtrl", ['$http', '$scope', '$state', '$location', function ($http, $scope, $state, $location) {
    $scope.Data = {};
    $scope.FeeNo = $state.params.FeeNo;
    var residentInfo = {};
    $scope.residentSelected = function (resident) {
        residentInfo = resident;
    }
    $scope.NavigateLink = function (name) {
        var url = '';
        switch (name) {
            case 'IpdregIn': //收案
                url = '/dc/IpdregIn/';
                break;
            case 'PersonBasicInfor': //个案基本资料
                url = '/dc/PersonBasicInfor/';
                break;
            case 'PersonLifeHistory': //个案生活史
                url = '/dc/PersonLifeHistory/';
                break;
            case 'OneDayLife':  //一天的生活
                url = '/dc/OneDayLife/';
                break;
            case 'SocialEval':  //社工个案评估及处於计划表
                url = '/dc/SocialEval/';
                break;
            case 'PersonReferral':  //个案转介单
                url = '/dc/PersonReferral/';
                break;
            case 'RegLifeQualityEval': //收案
                url = '/dc/RegLifeQualityEval/';
                break;
            case 'IpdregOut': //收案
                url = '/dc/IpdregOut/';
                break;
            case "RegQuestionEvalRec"://受托长辈适应程度评估表
                url = '/dc/RegQuestionEvalRec/';
                break;
        }
        if (angular.isDefined(residentInfo.FeeNo)) {
            url += residentInfo.FeeNo;
        }
        $location.url(url);
    }

   
}])
