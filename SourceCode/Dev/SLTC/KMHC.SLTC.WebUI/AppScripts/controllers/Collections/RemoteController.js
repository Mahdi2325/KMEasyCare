/*
创建人:Bob Du
创建日期:2016-09-02
说明: 日历的增删改查
*/
angular.module("sltcApp")
   .controller("remoteCtrl", ['$scope', '$http', '$state', '$stateParams', '$rootScope', 'adminHandoversRes', function ($scope, $http, $state, $stateParams, $rootScope, adminHandoversRes) {
       $scope.hide = function () {
           $scope.$parent.hide();
       }
       $scope.init = function () {
        $scope.Data = {};
        if ($scope.kmIncludeParams.id!="") {
            adminHandoversRes.get({ id: $scope.kmIncludeParams.id }, function (data) {
                $scope.Data = data;
            });
            $scope.isAdd = false;
        } else {
            $scope.isAdd = true;
        }
    };
       $scope.submit = function () {
       $scope.Data.PerformDate = $scope.kmIncludeParams.date;
       adminHandoversRes.save($scope.Data, function (data) {
           if ($scope.isAdd) {
               $scope.$emit('renderEvent', { id: data.Data.Id, title: $scope.Data.Content, start: $scope.Data.PerformDate });
           } else {
               $scope.$emit('updateEvent', { id: $scope.Data.Id, title: $scope.Data.Content, start: $scope.Data.PerformDate });
           }
           
        });
       };
       $scope.deleteItem = function () {
           if (confirm("您确定要删除?")) {
               adminHandoversRes.delete({ id: $scope.kmIncludeParams.id }, function () {
                   $scope.$emit('removeEvents');
               });
           }
       };
    $scope.init();
}]);;


