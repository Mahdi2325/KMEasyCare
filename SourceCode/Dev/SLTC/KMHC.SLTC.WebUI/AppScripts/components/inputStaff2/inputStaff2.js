angular.module("extentComponent")
.controller("inputStaff2Controller", ['$scope', '$http', '$location', '$rootScope', '$state', 'empFileRes', 'roleRes', 'DCDataDicListRes', function ($scope, $http, $location, $rootScope, $state, empFileRes, roleRes, DCDataDicListRes) {
    $scope.Data = {};
    var orgid = $scope.kmIncludeParams.orgid;
    var empgroup = $scope.kmIncludeParams.empgroup;
    $scope.loadEmpFile = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: empFileRes,//异步请求的res
            params: { empName: "", orgid: orgid, empGroup: empgroup },
            success: function (data) {//请求成功时执行函数
                $scope.Data.empFiles = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.StaffSelect = function (item) {
        $scope.$emit("km-on-staff-confirm-click", item);
        $scope.$emit("km-on-dialog-close-click");
    }

    $scope.loadEmpFile();
}])