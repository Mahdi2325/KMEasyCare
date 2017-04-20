/*
创建人: 张正泉
创建日期:2016-02-20
说明:楼层管理
*/

angular.module("sltcApp")
    .controller("deptListCtrl", ['$scope', '$http', '$location', '$state', '$rootScope', 'DCDataDicListRes', 'dictionary', 'deptRes', 'utility', function ($scope, $http, $location, $state, $rootScope, DCDataDicListRes, dictionary, deptRes, utility) {

        $scope.init = function () {
            $scope.Data = {};

            //$scope.$watch("keyword",function(newValue) {
            //    if (newValue) {
            //        deptRes.query({ }, function(data) {
            //            $scope.Data.depts = data;
            //        });
            //    }
            //});

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: deptRes,//异步请求的res
                params: { name: "", orgid: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.depts = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }

            $scope.initorglist();
        };

        $scope.initorglist = function () {
            //机构数据 SuperAdmin
            DCDataDicListRes.get({ flag: "33", staus: 0, datatyp: "1" }, function (data) {
                $scope.Orglist = data.Data;
                $scope.orgid = $rootScope.Global.OrganizationId;
            });

            $scope.OrgISSelect = true;
            if ($rootScope.Global.MaximumPrivileges == "SuperAdmin")
            { $scope.OrgISSelect = false; }
        };

        $scope.delete = function (item) {
            if (confirm("确定删除该部门信息吗?")) {
                deptRes.delete({ deptNo: item.DeptNo, orgId: item.OrgId }, function (data) {
                    if (data.$resolved) {
                        //var whatIndex = null;
                        //angular.forEach($scope.Data.depts, function (cb, index) {
                        //    if (cb.id === id) {
                        //        whatIndex = index;
                        //    }
                        //});
                        //$scope.Data.depts.splice(whatIndex, 1);
                        $scope.Data.depts.splice($scope.Data.depts.indexOf(item), 1);
                        utility.message("删除成功");
                    }
                });
            }
        };

        $scope.search = $scope.reload = function () {

            var url = "api/Depts?currentPage=1&name=" + $scope.keyword + "&orgid=" + $scope.orgid + "&pageSize=10";

            $http.get(url).success(function (response) {
                $scope.Data.depts = response.Data;
            })
            //debugger;
            //deptRes.query({ name:$scope.keyword, orgid: $scope.orgid}, function (data) {
            //     $scope.Data.depts = data.Data; 
            //})

        };
        $scope.init();
    }])
    .controller("deptEditCtrl", ['$scope', '$location', '$stateParams', '$rootScope', 'utility', 'deptRes', 'orgRes', function ($scope, $location, $stateParams, $rootScope, utility, deptRes, orgRes) {

        $scope.init = function () {
            var curUser = utility.getUserInfo();
            $scope.Data = {};
            $scope.DeptNo = "null";

            orgRes.get({ CurrentPage: 1, PageSize: 10 }, function (data) {
                $scope.Data.orgs = data.Data;
            });

            if ($stateParams.id) {

                deptRes.get({ id: $stateParams.id, orgid: "" }, function (data) {

                    $scope.Data.dept = data;
                    $scope.DeptNo = data.DeptNo;

                });
                $scope.isAdd = false;
                $scope.showOrg = false;
            } else {
                if ($rootScope.Global.MaximumPrivileges == "SuperAdmin") {
                    $scope.isAdd = true;
                    $scope.showOrg = false;
                }
                else {
                    $scope.isAdd = true;
                    $scope.showOrg = true;
                    $scope.Data.dept = { OrgId: $rootScope.Global.OrganizationId }
                }
            }
        };

        $scope.submit = function () {
            deptRes.save($scope.Data.dept, function (data) {
                if ($rootScope.Global.CurrentLoginSys == "LC")
                { $location.url("/angular/deptList"); }
                else
                { $location.url("/dc/deptList"); }

            });
        };
        $scope.init();
    }]);
