angular.module("sltcApp")
    .controller("personExtendCtrl", ['$scope', '$http', '$state', '$location', 'utility', 'personExtendRes', 'floorRes', 'roomRes', function ($scope, $http, $state, $location, utility, personExtendRes, floorRes, roomRes) {
        var type = $scope.kmIncludeParams.type;
        //var feeNoArr = JSON.parse($scope.kmIncludeParams.feeNoArr);
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: personExtendRes,//异步请求的res
            params: { FEENO: $scope.kmIncludeParams.feeNoArr },
            success: function (data) {//请求成功时执行函数
                $scope.Persons = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }



    }]);