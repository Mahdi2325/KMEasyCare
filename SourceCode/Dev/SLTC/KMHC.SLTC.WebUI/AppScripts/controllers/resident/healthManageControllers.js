///创建人:肖国栋
///创建日期:2016-02-23
///说明:健康管理
///update by zhangkai,2016-03-12,update healthManageCtrl
angular.module("sltcApp")
.controller("healthManageCtrl", ['$rootScope', '$scope', '$state', 'utility', 'healthManageRes', 'nurDemandHisRes', function ($rootScope, $scope, $state, utility, healthManageRes, nurDemandHisRes) {
    //var id = $state.params.id;
    //$scope.init = function () {
    //    $scope.Detail = residentData.HealthManage;
    //    $(".datepicker").datepicker({
    //        dateFormat: "yy-mm-dd",
    //        changeMonth: true,
    //        changeYear: true
    //    });
    //    var dicType = ["FoodHabit", "StopFoodReason", "ClanCaseHistory"];
    //    dictionary.get(dicType, function (dic) {
    //        $scope.Dic = dic;
    //        for (var name in dic) {
    //            if ($scope.Dic.hasOwnProperty(name)) {
    //                if ($scope.Dic[name][0] != undefined) {
    //                    $scope.Detail[name] = $scope.Dic[name][0].Value;
    //                }
    //            }
    //        }
    //    });

    //    $scope.orgs = orgRes.query();

    //    $rootScope.$on('refreshTabData', function (event, data) {
    //        $scope.Detail = residentData.HealthManage;
    //    });
    //}

    //$scope.init();
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Detail = {};
    $scope.CareDemand = {};
    $scope.buttonShow = false;
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.initHealth($scope.currentResident.FeeNo);//加载当前住民的健康管理信息
       
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    $scope.GetLatestCareDemand = function () {
        nurDemandHisRes.get({ id: $scope.currentResident.FeeNo }, function (data) {
            var CareDemand = data.Data;
            if (!isEmpty(CareDemand)) {
                $scope.Detail.ALLERGY = "药物过敏史:{0}\r食物过敏:{1}\r其他过敏:{2}".format(CareDemand.ALLERGY_DRUG, CareDemand.ALLERGY_FOOD, CareDemand.ALLERGY_OTHERS);
                if (isEmpty(CareDemand.ALLERGY_DRUG) && isEmpty(CareDemand.ALLERGY_FOOD) && isEmpty(CareDemand.ALLERGY_OTHERS)) {
                    utility.message("最新护理评估，过敏史没有填写！");
                } else {
                    utility.message("已为您带入最新一笔护理评估过敏史记录！");
                }
            } else {
                utility.message("最新护理评估，过敏史没有填写！");
            }
        });
    }

    //加载健康管理信息
    $scope.initHealth = function (FeeNo) {
        if (FeeNo != "") {
            healthManageRes.get({ feeNo: FeeNo }, function (data) {
                $scope.Detail = data.Data;
             
            });
        }
    }

    $scope.saveItem = function () {
        $scope.Detail.FEENO = $scope.currentResident.FeeNo;
        $scope.Detail.REGNO = $scope.currentResident.RegNo;
        new healthManageRes($scope.Detail).$save();
        utility.message($scope.currentResident.Name + "的健康管理信息保存成功！");
    };

}]);
