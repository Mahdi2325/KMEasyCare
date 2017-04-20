angular.module("sltcApp")
.controller('TsgCtrl', ['$scope', 'categoryRes', function ($scope, categoryRes) {
    $scope.tsgCategoryList = {};
    $scope.init=function(){
        categoryRes.get({}, function (data) {
            $scope.tsgCategoryList = data.Data;
        });
    }
    $scope.init();
}]);