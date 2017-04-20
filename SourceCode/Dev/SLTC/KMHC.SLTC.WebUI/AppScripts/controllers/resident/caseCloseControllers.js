/*
创建人:张凯
创建日期:2016-02-29
说明: 新的结案管理放在左边菜单
*/
angular.module("sltcApp")
    .controller("caseCloseCtrl", ['$scope', '$state', 'dictionary', 'utility', 'webUploader', 'caseClosedRes', function ($scope, $state, dictionary, utility, webUploader, caseClosedRes) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Detail = {};
    $scope.buttonShow = false;
    $scope.willShow = false;//是否显示预立遗嘱
    $scope.deathShow = false;//是否亡故
    $scope.closeShow = false;//已结案

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.init($scope.currentResident.FeeNo);//加载当前住民的结案管理
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    //加载结案管理
    $scope.init = function (FeeNo) {
        caseClosedRes.get({ id: FeeNo }, function (data) {
            $scope.Detail = data;
            if ($scope.Detail.PalliativeCareFile != null && typeof ($scope.Detail.PalliativeCareFile) != "undefined") {
                var fi = $scope.Detail.PalliativeCareFile.split('|$|');
                if (fi.length == 2) {
                    $scope.Detail.SavedLocation = fi[1];
                    $scope.Detail.FileName = fi[0];
                } else if (fi.length == 1) {
                    $scope.Detail.SavedLocation = fi[0];
                    $scope.Detail.FileName = fi[0];
                }
            }
        });
    }

    $scope.CheckWills = function () {
        if ($scope.Detail.WillsFlag) {
            $scope.willShow = true;
        } else {
            $scope.willShow = false;
        }
    }
    $scope.CheckDeath = function () {
        if ($scope.Detail.DeathFlag) {
            $scope.deathShow = true;
        } else {
            $scope.deathShow = false;
        }
    }
    $scope.CheckCloseFlag = function () {
        if ($scope.Detail.CloseFlag) {
            $scope.closeShow = true;
        } else {
            $scope.closeShow = false;
        }
    }

    $scope.saveItem = function () {
        $scope.Detail.PalliativeCareFile = '{0}|$|{1}'.format($scope.Detail.FileName, $scope.Detail.SavedLocation);
        $scope.Detail.FeeNo = $scope.currentResident.FeeNo;
        $scope.Detail.RegNo = $scope.currentResident.RegNo;
        $scope.Detail.OrgId = $scope.currentResident.OrgId;
        caseClosedRes.save($scope.Detail, function (data) {
            utility.message($scope.currentResident.Name + "的结案信息保存成功！");
        });      
    };

    webUploader.init('#CureFilePicker', { category: 'HomePhoto' }, '文件', 'doc,docx,pdf', 'doc/*,application/pdf', function (data) {
        if (data.length > 0) {
            $scope.Detail.SavedLocation = data[0].SavedLocation;
            $scope.Detail.FileName = data[0].FileName;
            $scope.$apply();
        }
    });

    $scope.clear = function (type) {
        switch (type) {
            case "CureFile":
                $scope.Detail.SavedLocation = "";
                $scope.Detail.FileName = "";
                break;
        }
    }

}]);
