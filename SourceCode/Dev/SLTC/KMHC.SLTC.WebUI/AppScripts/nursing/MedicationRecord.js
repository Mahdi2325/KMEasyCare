/*
创建:张祥
创建日期:2016-04-06
说明: 用药记录
*/
angular.module("sltcApp").controller("MedicationRecordCtrl", ['$scope', 'MedicationRecordRes', 'utility', '$state',
    function ($scope, MedicationRecordRes, utility, $state) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        // 当前住民
        $scope.currentResident = {};
        $scope.init = function () {
            var myDate = new Date().format("yyyy-MM-dd");
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: MedicationRecordRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.MedicationRecordList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
                ,
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                    StartDate: "",
                    EndDate: ""
                }
            }
        }
        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.currentResident = resident;//获取当前住民信息
            $scope.Data.MedicationRecordList = {};
            $scope.options.params.feeNo = $scope.currentResident.FeeNo;
            $scope.options.params.StartDate = "";
            $scope.options.params.EndDate = "";
            $scope.options.search();
        };

        $scope.Search = function () {
            if (!angular.isDefined($scope.currentResident.FeeNo)) {
                utility.message("请选择住民！");
                return;
            }
            var startDate = "";
            var endDate = "";
            if (angular.isDefined($scope.StartDate)) {
                startDate = $scope.StartDate;
            }
            if (angular.isDefined($scope.EndDate)) {
                endDate = $scope.EndDate
            }
            $scope.Data.MedicationRecordList = {};
            $scope.options.params.feeNo = $scope.currentResident.FeeNo;
            $scope.options.params.StartDate = startDate;
            $scope.options.params.EndDate = endDate;
            $scope.options.search();
        };

        $scope.init();
    }])
