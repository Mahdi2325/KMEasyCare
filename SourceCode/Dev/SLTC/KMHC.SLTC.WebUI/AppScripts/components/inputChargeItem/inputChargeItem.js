angular.module("extentComponent")
.directive("inputChargeItem", ['chargeItemRes', function (chargeItemRes) {
    return {
        resctict: "E",
        templateUrl: "/AppScripts/components/inputChargeItem/inputChargeItem.html",
        scope: {
            value: "@value",
            callbackFn: "&callback",
            required: "@required"
        },
        controller: ['$scope', function ($scope) {
            chargeItemRes.get({}, function (data) {
                $scope.chargeItems = data.Data;
            });
            
            $scope.change = function() {
                $scope.showList = (angular.isDefined($scope.searchWords) && $scope.searchWords != "");
                if ($scope.showList) {
                    chargeItemRes.get({ keyWords: $scope.searchWords, currentPage: 1, pageSize: 100}, function (data) {
                        $scope.chargeItems = data.Data
                    });
                }
            }


            $scope.rowCick = function (item) {
                $scope.searchWords = item.CostItemNo;//输入框显示选择的编码
                $scope.callbackFn({ item: item });//回调函数
                $scope.showList = false;//隐藏列表
            };

            //监控传入值的改变,同步关键字显示
            $scope.$watch("value", function(newValue) {
                $scope.searchWords = newValue;
            });
        }]
    }
}]);
