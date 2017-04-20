
angular.module("sltcApp")
.controller("payGrantCtrl", ['$scope', '$filter', '$stateParams', 'payGrantServiceRes', 'utility', '$state', function ($scope, $filter, $stateParams, payGrantServiceRes, utility, $state) {
    $scope.Data = {};

    var preMonthDate = new Date();
    var preYear = preMonthDate.getFullYear();
    $scope.YearList = [{ "value": preYear - 3, "text": preYear - 3 }, { "value": preYear - 2, "text": preYear - 2 }, { "value": preYear - 1, "text": preYear - 1 }, { "value": preYear, "text": preYear }, { "value": preYear + 1, "text": preYear + 1 }, { "value": preYear + 2, "text": preYear + 2 }, { "value": preYear + 3, "text": preYear + 3 }]


    $scope.init = function () {
        $scope.Year = preYear;
        $scope.nsId = 1;
        $scope.loadPayGrant($scope.Year, $scope.nsId);
    };

    $scope.loadPayGrant = function (year, nsid) {
        $scope.payGrantList = {};
        payGrantServiceRes.get({ year: $scope.Year }, function (data) {

            if (data.ResultCode == 101) {
                utility.message(data.ResultMessage);
            }
            else {
                $scope.payGrantList = data.Data;
            }
            
        });
    };

    $scope.searchInfo = function () {
        $scope.loadPayGrant($scope.Year, $scope.nsId);
    };

    $scope.DetailsInfo = function (payGrant) {
        $scope.payGrantModel = {};

        payGrantServiceRes.get({ grantId: payGrant.GrantID, type: 0 }, function (data) {
            if (data.ResultCode == 101) {
                utility.message(data.ResultMessage);
            }
            else if (data.ResultCode == -1)
            {
                utility.message(data.ResultMessage);
            }
            else {
                $scope.payGrantModel = data.Data;
            }
        });
    };

    $scope.init();
}])