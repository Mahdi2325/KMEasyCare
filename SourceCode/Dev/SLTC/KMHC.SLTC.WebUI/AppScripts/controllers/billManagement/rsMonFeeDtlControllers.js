/*
创建人: 吴晓波
创建日期:2017-2-6
说明:住民申报费用明细
*/
angular.module("sltcApp")
.controller("rsMonFeeDtlCtrl", ['$scope', '$filter', '$stateParams', 'rsFeeMonDtlRes', 'utility', '$state', function ($scope, $filter, $stateParams, rsFeeMonDtlRes, utility, $state) {
    var NsMonFeeID = $state.params.id;
    var RsMonFeeID = $state.params.rsMonFeeId;
    var YearMon = $state.params.YearMon;
    $scope.NsMonFeeID = NsMonFeeID;
    $scope.RsMonFeeID = RsMonFeeID;
    $scope.YearMon = YearMon;
    $scope.Data = {};
    $scope.Data.RSMonFees = {};
    $scope.Data.RSNciMonFeeDtlList = [];
    $scope.Data.RSSelfMonFeeDtlList = [];

    $scope.init = function () {
        //获取住民费用汇总信息
        rsFeeMonDtlRes.get({ currentPage: 1, pageSize: 10, orgMonFeeId: $scope.RsMonFeeID }, function (data) {
            $scope.Data.RSMonFees = data.Data[0];
            if ($scope.Data.RSMonFees != null && $scope.Data.RSMonFees != undefined && $scope.Data.RSMonFees != '') {
                $scope.Data.RSMonFees.RA = $scope.Data.RSMonFees.Hospday * $scope.Data.RSMonFees.Ncipaylevel
            };
        });
    };

    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: rsFeeMonDtlRes,//异步请求的res
        params: { orgMonFeeId: $scope.RsMonFeeID, feeType: "NCI" },
        success: function (data) {//请求成功时执行函数
            $scope.Data.RSNciMonFeeDtlList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    $scope.optionsSELF = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: rsFeeMonDtlRes,//异步请求的res
        params: { orgMonFeeId: $scope.RsMonFeeID, feeType: "SELF" },
        success: function (data) {//请求成功时执行函数
            $scope.Data.RSSelfMonFeeDtlList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    $scope.closeRSMonFeeDtl = function () {
        $state.go('MonFeeUploadEdit', { NSMonFeeID: $scope.NsMonFeeID });
        $state.stateName = "MonFeeUploadEdit";
    };

    $scope.init();
}])