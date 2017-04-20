/*
创建人: 张正泉
创建日期:2016-02-20
说明:楼层管理
*/

angular.module("sltcApp")
    .controller("companyListCtrl", ['$scope', '$http', '$location', '$state', 'dictionary', 'companyRes', 'utility', function ($scope, $http, $location, $state, dictionary, companyRes, utility) {

        $scope.init = function () {
            $scope.Data = {};
            companyRes.query({}, function (data) {
                $scope.Data.companys = data;
            });
            $scope.$watch("keyword", function (newValue) {
                if (newValue) {
                    companyRes.query({}, function (data) {
                        $scope.Data.companys = data;
                    });
                }
            });
        };

        $scope.delete = function (id) {
            if (confirm("确定删除该楼层信息吗?")) {
                companyRes.delete({ id: id }, function (data) {
                    if (data.$resolved) {
                        var whatIndex = null;
                        angular.forEach($scope.Data.companys, function (cb, index) {
                            if (cb.id === id) {
                                whatIndex = index;
                            }
                        });
                        $scope.Data.companys.splice(whatIndex, 1);
                        utility.message("删除成功");
                    }
                });
            }
        };

        $scope.search = $scope.reload = function () {
            if ($scope.keyword) {
                companyRes.query({ Code: $scope.keyword }, function (data) {
                    $scope.Data.companys = data;
                });
            }
        };

        $scope.init();
    }])
    .controller("companyEditCtrl", ['$scope', '$http', '$location', '$stateParams', 'dictionary', 'companyRes', 'orgRes', function ($scope, $http, $location, $stateParams, dictionary, companyRes, orgRes) {

        $scope.init = function () {
            $scope.Data = {};


            if ($stateParams.id) {
                companyRes.get({ id: $stateParams.id }, function (data) {
                    $scope.Data.company = data;
                });
                $scope.isAdd = false;
            } else {
                $scope.isAdd = true;
            }
        };

        $scope.submit = function () {
            companyRes.save($scope.Data.company, function (data) {
                $location.url("/angular/companyList");
            });
        };
        $scope.init();
    }]);
