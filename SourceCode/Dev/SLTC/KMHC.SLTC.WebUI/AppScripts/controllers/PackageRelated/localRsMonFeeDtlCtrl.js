
angular.module("sltcApp")
.controller("localRsMonFeeDtlCtrl", ['$scope', '$filter', '$stateParams', '$http', 'billV2Res', 'utility', '$state', function ($scope, $filter, $stateParams, $http, billV2Res, utility, $state) {
    var FeeNo = $state.params.FeeNo;
    var YearMon = $state.params.YearMon;
    $scope.FeeNo = FeeNo;
    $scope.YearMon = YearMon;
    $scope.Data = {};
    $scope.Data.RSMonFees = {};
    $scope.Data.RSNciMonFeeDtlList = [];
    $scope.Data.RSSelfMonFeeDtlList = [];

    $http({
        url: 'api/billv2/GetRSMonFees?date='+YearMon+'&feeNo='+FeeNo,
        method: 'GET'
    }).success(function (data, header, config, status) {
        $scope.Data.RSMonFees = data;
    }).error(function (data, header, config, status) {
        //处理响应失败
        alert("Error:数据获取出错异常！");
    });

    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: billV2Res,//异步请求的res
        params: { feeNo: FeeNo, date: YearMon,feeType:'NCI' },
        success: function (data) {//请求成功时执行函数
            $scope.Data.RSNciMonFeeDtlList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    $scope.optionsSELF = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: billV2Res,//异步请求的res
        params: { feeNo: FeeNo, date: YearMon, feeType: 'SELF' },
        success: function (data) {//请求成功时执行函数
            $scope.Data.RSSelfMonFeeDtlList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    $scope.closeRSMonFeeDtl = function () {
        $state.go('MonFeeUpload');
    };

   
}])