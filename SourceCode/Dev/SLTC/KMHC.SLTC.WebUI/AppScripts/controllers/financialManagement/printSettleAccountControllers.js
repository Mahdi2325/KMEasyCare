/*
创建人: 吴晓波
创建日期:2017-03-09
说明:结算单打印
*/
angular.module("sltcApp")
.controller("printSettleAccountCtrl", ['$rootScope', '$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi',
    function ($rootScope, $scope, $http, $location, $state, utility, cloudAdminUi) {
        $scope.FeeNo = $state.params.FeeNo;

        var preMonthDate = new Date();
        preMonthDate.setDate(1);
        preMonthDate.setMonth(preMonthDate.getMonth() - 1);
        var preYear = preMonthDate.getFullYear();
        var preMonth = preMonthDate.getMonth();
        $scope.beginDate = new Date(preYear, preMonth).format("yyyy-MM");
        $scope.endDate = $scope.beginDate;

        $scope.txtResidentIDChange = function (resident) {
            $scope.FeeNo = resident.FeeNo
        }

        $scope.Print = function (AppcertId) {
            if ($scope.FeeNo == undefined || $scope.FeeNo == null || $scope.FeeNo === "") {
                utility.msgwarning("请选择一个住民!");
                return;
            };

            if (!$scope.beginDate || !$scope.endDate) {
                utility.msgwarning("开始月份或结束月份不能为空");
                return;
            }
            if ($scope.beginDate > $scope.endDate) {
                utility.msgwarning("开始月份不能大于结束月份");
                return;
            }

            window.open("/SettleAccountReport/SettleAccountReport?templateName={0}&beginDate={1}&endDate={2}&feeNo={3}".format("SettleAccountReport", $scope.beginDate, $scope.endDate, $scope.FeeNo));
        };
    }
]);
