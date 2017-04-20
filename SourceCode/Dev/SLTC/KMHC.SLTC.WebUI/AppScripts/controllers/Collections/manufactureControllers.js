/*
创建人: 刘美方
创建日期:2016-03-8
说明:厂商管理
*/

angular.module("sltcApp")
    .controller("manufactureListCtrl", ['$scope', '$http', '$location', '$state', 'manufactureRes', 'utility', function ($scope, $http, $location, $state, manufactureRes, utility) {
        $scope.init = function () {
            $scope.Data = {};
            //$scope.CurrentPage = 1;
            //$scope.keyword = "";
            //$scope.search();
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: manufactureRes,//异步请求的res
                params: { keyword: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.manufactures = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }
        };
        $scope.delete = function (id) {
            if (confirm("确定删除该信息吗?")) {
                manufactureRes.delete({ id: id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                    //if (data.$resolved) {
                    //    var whatIndex = null;
                    //    angular.forEach($scope.Data.manufactures, function (cb, index) {
                    //        if (cb.id === id) {
                    //            whatIndex = index;
                    //        }
                    //    });
                    //    $scope.Data.manufactures.splice(whatIndex, 1);
                    //    utility.message("删除成功");
                    //}
                });
            }
        };

        //$scope.search = $scope.reload = function () {
        //    manufactureRes.get({ currentPage: $scope.CurrentPage, pageSize: 10, name: $scope.keyword }, function (obj) {
        //        $scope.Data.manufactures = obj.Data;
        //        var pager = new Pager('pager', $scope.CurrentPage, obj.PagesCount, function (curPage) {
        //            $scope.CurrentPage = curPage;
        //            $scope.search();
        //        });
        //    });
        //};

        $scope.init();
    }])
    .controller("manufactureEditCtrl", ['$scope', '$http', '$location', '$state', '$stateParams', 'manufactureRes', 'utility', function ($scope, $http, $location, $state, $stateParams, manufactureRes, utility) {
        $scope.init = function () {
            $scope.Data = {};
            if ($stateParams.id) {
                manufactureRes.get({ id: $stateParams.id }, function (data) {
                    $scope.Data.manufacture = data;
                });
                $scope.isAdd = false;
            } else {
                $scope.isAdd = true;
            }
        };
        $scope.submit = function () {

            if (angular.isDefined($scope.form1.$error.required)) {
                for (var i = 0; i < $scope.form1.$error.required.length; i++) {
                    utility.msgwarning($scope.form1.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.form1.$error.maxlength)) {
                for (var i = 0; i < $scope.form1.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.form1.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            manufactureRes.save($scope.Data.manufacture, function (data) {
                $state.go('manufactureList');
            });
        };
        $scope.init();
    }]);
