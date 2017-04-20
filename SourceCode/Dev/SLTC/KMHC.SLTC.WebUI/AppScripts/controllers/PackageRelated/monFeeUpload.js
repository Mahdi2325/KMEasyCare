angular.module("sltcApp")
.controller("MonFeeUploadCtrl", ['$scope', '$state', '$http', '$compile', 'billV2Res', 'utility', function ($scope, $state, $http,$compile, billV2Res, utility) {
    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: billV2Res,//异步请求的res
        success: function (data) {//请求成功时执行函数
            $scope.MonthFeeList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        },
        params: { date: '' }
    }
    $scope.GetMonth = function () {
        $http({
            url: 'api/billv2/GetYearMonth',
            method: 'GET'
        }).success(function (data) {
            $scope.monthArr = data;
        });
    }
    $scope.GetMonth();
    $scope.upload = function () {
        if (!$scope.MONTH) {
            utility.msgwarning("请先选择月份");
            return;
        }
        $http({
            url: 'api/billv2/UploadMonthData?date=' + $scope.MONTH,
            method: 'GET'
        }).success(function (data, header, config, status) {
            $state.go("MonFeeList");
        }).error(function (data, header, config, status) {
            //处理响应失败
            utility.msgwarning(data.ExceptionMessage);
        });
    }
    $scope.onMonthChanged = function () {
        $http({
            url: 'api/billv2/GetMonthData?date=' + $scope.MONTH,
            method: 'GET'
        }).success(function (data) {
            $scope.monthData = data;
            $scope.options.params.date = $scope.MONTH;
            $scope.options.search();
        });
    };
    $scope.show = function () {
        var html = '<div km-include km-template="Views/PackageRelated/DeducTionList.html" km-controller="deducTionCtrl"  km-include-params="{key:\'' + $scope.MONTH + '\',type:1}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">巡检扣款记录</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            cssClass: 'visit-dialog',
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
    $scope.Print = function () {
        window.open('/MonthFee/Preview?date=' + $scope.MONTH);
    }
}])
.controller("deducTionCtrl", ['$scope', '$location', '$rootScope', '$compile', '$http', 'utility', 'DeductionRes', function ($scope, $location, $rootScope, $compile, $http, utility, DeductionRes) {
    var key = $scope.kmIncludeParams.key;
    var type = $scope.kmIncludeParams.type;
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: DeductionRes,//异步请求的res
            params: { key: key,type:type },
            success: function (data) {//请求成功时执行函数
                $scope.DeductionList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };
    };
    $scope.init();
}])
.controller("MonFeeUploadEditCtrl", ['$scope', '$stateParams', '$http', '$compile', 'billV2Res', 'utility', function ($scope, $stateParams, $http,$compile, billV2Res, utility) {

    $http({
        url: 'api/billv2/ShowMonthData?NSMonFeeID=' + $stateParams.NSMonFeeID,
        method: 'GET'
    }).success(function (data, header, config, status) {
        $scope.monthData = data.OrgMonthData.Data;
        $scope.monthData.deDuction = data.deDuction;
    }).error(function (data, header, config, status) {
        //处理响应失败
        //alert("护理险平台无法连接，请联系管理员！")
        utility.msgwarning("护理险平台无法连接，请联系管理员！");
    });

    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: billV2Res,//异步请求的res
        success: function (data) {//请求成功时执行函数
            $scope.MonthFeeList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        },
        params: { NSMonFeeID: $stateParams.NSMonFeeID }
    }

    $scope.show = function () {
        var html = '<div km-include km-template="Views/PackageRelated/DeducTionList.html" km-controller="deducTionCtrl"  km-include-params="{key:\'' + $stateParams.NSMonFeeID + '\',type:2}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">巡检扣款记录</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            cssClass: 'visit-dialog',
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
}]);