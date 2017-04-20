/*
创建人:张凯
创建日期:2016-03-24
说明: 集团管理
*/
angular.module("sltcApp")
.controller("groupListCtrl", ['$scope', '$http', '$location', '$state', 'groupRes', 'utility', function ($scope, $http, $location, $state, groupRes, utility) {
    $scope.Data = {};
    $scope.loadGroups = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: groupRes,//异步请求的res
            params: { groupName: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.Groups = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        //groupRes.get({GroupName : ""}, function (data) {
        //    $scope.Data.Groups = data.Data;
        //});
    }
   
    $scope.GroupDelete = function (item) {
        if (confirm("您确定删除该集团信息吗?")) {
             
            groupRes.delete({ id: item.GroupId }, function (data) {
                
                if (data.ResultCode == -1) {
                    utility.message(data.ResultMessage);
                }
                else {
                    $scope.Data.Groups.splice($scope.Data.Groups.indexOf(item), 1);

                    //groupRes.get({ groupName: "" }, function (dataitem) {
                    //    $scope.Data.Groups = data.Dataitem;
                    //});
                }
                //$scope.loadGroups();
            });
        }
    }
    $scope.loadGroups();
}])
.controller("groupEditCtrl", ['$scope', '$http', '$location','$rootScope', '$stateParams', 'groupRes', function ($scope, $http, $location, $rootScope,$stateParams, groupRes) {
    $scope.Data = {};
    if ($stateParams.id) {
        groupRes.get({ id: $stateParams.id }, function (data) {
            $scope.Data.Group = data.Data;
        });
    }

    $scope.save = function () {
        console.log($scope.Data.Group);
        groupRes.save($scope.Data.Group, function (data) {
             
            if ($rootScope.Global.CurrentLoginSys == "LC")
            { $location.url('/angular/GroupList'); }
            else
            { $location.url('/dc/GroupList'); }

        });
    }


}]);

