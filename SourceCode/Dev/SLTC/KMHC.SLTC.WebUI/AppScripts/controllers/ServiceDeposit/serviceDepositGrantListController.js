angular.module("sltcApp")

 //护理险服务保证金列表
.controller("serviceDepositGrantListCtrl", ['$scope', '$compile', 'NCIPServiceDepositGrantListRes', 'utility', function ($scope, $compile, NCIPServiceDepositGrantListRes, utility) {
    $scope.Data = {};
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIPServiceDepositGrantListRes,//异步请求的res
            params: {},
            success: function (data) {//请求成功时执行函数
                $scope.Data.ServiceDepositGrantList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    $scope.init();

}])


