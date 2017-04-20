/*
创建人: 张正泉
创建日期:2016-02-20
说明:费用录入
*/

angular.module("sltcApp")
    .controller("hspRecordList", ['$scope', '$http', '$location', '$filter', '$stateParams', '$timeout', 'hspRecordRes',  function ($scope, $http, $location, $filter, $stateParams, $timeout, hspRecordRes) {
        $scope.init = function () {
            $scope.Data = {};
            $scope.timer = {};
            $scope.timer2 = {};
            $scope.startDate = $filter("date")(new Date(), "yyyy-MM-dd");
            //初始化一些常用参数
            $scope.charge = { ResidentID: "", ChargeDate: $scope.startDate, Quantity: 1, Price: 0 };
            if ($stateParams.id) {
                $scope.charge.ResidentID = $stateParams.id;
            }
            $scope.$watch("charge.ResidentID", function () {
                $timeout.cancel($scope.timer);
                $scope.timer = $timeout(search($scope.charge.ResidentID), 500);
            });

            $scope.$watch("charge.ChargeCode", function () {
                $timeout.cancel($scope.timer2);
                $scope.timer2 = $timeout(function () {
                    if ($scope.charge.ChargeCode) {
                        chargeItemRes.query({ itemCode: $scope.charge.ChargeCode }, function (data) {
                            if (data.length > 0) {
                                $scope.charge.ChargeCode = data[0].itemCode;
                                $scope.charge.ChargeName = data[0].itemName;
                                $scope.charge.Price = data[0].price;
                                $scope.charge.Unit = data[0].unit;
                                $scope.charge.ChargeType = data[0].itemType;
                            }
                        });
                    }
                }, 500);
            });

            $scope.$watch("charge.Price", function () {
                $scope.charge.NeedPay = $scope.charge.Quantity * $scope.charge.Price;
            });

            $scope.$watch("charge.Quantity", function () {
                $scope.charge.NeedPay = $scope.charge.Quantity * $scope.charge.Price;
            });
        };

        function search(residentId) {
            if (residentId) {
                chargeDetailRes.query({ ResidentID: residentId }, function (data) {
                    $scope.Data.charges = data;
                });
            } else {
                $scope.charge.ResidentName = "";
                $scope.Data.charges = [];
            }
        }

        $scope.search = $scope.reload = function () {
            search($scope.charge.ResidentID);
        };

        $scope.submit = function () {
            $scope.charge.RealPay = $scope.charge.NeedPay;
            if ($scope.charge.ResidentID && $scope.charge.NeedPay) {
                chargeDetailRes.save($scope.charge, function () {
                    if ($scope.charge.ResidentID) {
                        search($scope.charge.ResidentID);
                    }
                });
            }
        };
        $scope.dateRangeClick = function (e) {
            var t;
            if (e.target && (t = $(e.target).attr("range"))) {
                switch (t) {
                    case "week":
                        $scope.endDate = $scope.startDate;
                        break;
                    case "month":
                        $scope.endDate = $scope.startDate;
                        break;
                    default:
                        $scope.endDate = $scope.startDate;
                        break;
                }
            }
        };
        $scope.txtResidentIDChange = function (resident) {
            $scope.charge.ResidentID = resident.id;
        }
        $scope.selectResidentItem = function (data) {
            $scope.charge.ResidentID = data.id;
            $scope.charge.ResidentName = data.Name;
        };
        $scope.selectChargeItem = function (data) {
            $scope.charge.ChargeCode = data.itemCode;
            $scope.charge.Price = data.price;
            $scope.charge.Unit = data.unit;
            $scope.charge.ChargeType = data.itemType;
        };
        $scope.rowClick = function (charge) {
            $scope.charge = angular.copy(charge);
        }

        $scope.delete = function (id) {
            if (confirm("确定删除该费用信息吗?")) {
                chargeDetailRes.delete({ id: id }, function (data) {
                    if (data.$resolved) {
                        var whatIndex = null;
                        angular.forEach($scope.Data.charges, function (cb, index) {
                            if (cb.id === id) {
                                whatIndex = index;
                            }
                        });
                        $scope.Data.charges.splice(whatIndex, 1);
                        utility.message("删除成功");
                    }
                });
            }
        };

        $scope.init();
    }])
    .controller("hspRecordEditCtrl", ['$scope', '$http', '$location', '$stateParams', 'dictionary', 'chargeDetailRes', function ($scope, $http, $location, $stateParams, dictionary, chargeDetailRes) {
        $scope.init = function () {
            $scope.Data = {};

            if ($stateParams.id) {
                chargeDetailRes.get({ id: $stateParams.id }, function (data) {
                    $scope.Data.charge = data;
                });
                $scope.isAdd = false;
            } else {
                $scope.isAdd = true;
            }
        };

        $scope.submit = function () {
            chargeDetailRes.save($scope.Data.charge, function (data) {
                $location.url("/angular/chargeList");
            });
        };
        $scope.init();
    }]);
