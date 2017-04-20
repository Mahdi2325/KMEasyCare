angular.module("extentComponent")
.directive("residentList", ['DCResidentRes', function (DCResidentRes) {
    return {
        resctict: "EA",
        templateUrl: "/AppScripts/components/residentList/residentList.html",
        scope: {
            //value: "@value",
            callbackFn: "&callback"
        },
        controller: ['$scope', '$attrs', function ($scope, $attrs) {
            $scope.resident = { residentName: '', ipdFlag: "I", stationCode: '', residentNo: '' };
            DCResidentRes.get($scope.resident, function (data) {
                $scope.residentList = data.Data;
                var FeeNo = $attrs['feeno'];
                var item = {}; var isExist = false;
                if (angular.isDefined(FeeNo) && FeeNo != "") {
                    for (var i = 0; $scope.residentList.length; i++) {
                        if ($scope.residentList[i].FeeNo == FeeNo) {
                            item = $scope.residentList[i]; isExist = true;
                            $scope.colorId = FeeNo;
                            break;
                        }
                    }
                    if (isExist) {
                        $scope.callbackFn({ resident: item });
                    }
                }
            });
            $scope.rowClick = function (item) {
                $scope.callbackFn({ resident: item });//回调函数
            };

            $scope.Search = function () {
                DCResidentRes.get($scope.resident, function (data) {
                    $scope.residentList = data.Data;
                });
            }
        }],
        link: function (scope, element, attrs) {
     
        }
    }
}]);
