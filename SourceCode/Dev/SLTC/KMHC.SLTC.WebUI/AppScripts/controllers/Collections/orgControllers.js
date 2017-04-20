/*
创建人:张凯
创建日期:2016-02-24
说明: 机构管理
*/
angular.module("sltcApp")
    .controller("orgListCtrl", ['$scope', '$http', '$location', '$state', 'orgRes', 'utility', function ($scope, $http, $location, $state, orgRes, utility) {
        $scope.Data = {};
        $scope.loadOrgs = function() {
            $scope.options = {
                buttons: [], //需要打印按钮时设置
                ajaxObject: orgRes, //异步请求的res
                params: { OrgName: "" },
                success: function(data) { //请求成功时执行函数
                    $scope.Data.Orgs = data.Data;
                },
                pageInfo: {
//分页信息
                    CurrentPage: 1,
                    PageSize: 10
                }
            }
            //orgRes.get({}, function (data) {
            //    $scope.Data.Orgs = data.Data;
            //});
        }
        
        $scope.OrgDelete = function(item) {
            if (confirm("您确定删除该机构信息吗?")) {
                orgRes.delete({ id: item.OrgId }, function (data) {
                    //$scope.options.search();
                    if (data.ResultCode == 1)
                    {
                        $scope.Data.Orgs.splice($scope.Data.Orgs.indexOf(item), 1);
                        utility.message("删除成功");
                    }
                    else
                    {
                        utility.message(data.ResultMessage);
                    }
                });
            }
        }
        $scope.loadOrgs();

    }])
    .controller("orgDtlCtrl", ['$scope', '$http', '$location', '$stateParams', '$rootScope','utility', 'orgRes', 'roleRes', 'groupRes', 'moduleRes',
        function ($scope, $http, $location, $stateParams, $rootScope,utility, orgRes, roleRes, groupRes, moduleRes) {
        $scope.Data = {};
        var roleId = "0";
        if ($stateParams.id) {
            //roleRes.get({ id: $stateParams.id }, function (data) {
            //    $scope.Data.Role = data.Data;
            //});
            orgRes.get({ id: $stateParams.id }, function(data) {
                $scope.Data.Org = data.Data;
                if ($scope.Data.Org.RoleId != null) {
                    roleId = data.Data.RoleId;
                }
                treeHelper = new TreeHelper(moduleRes, "#moduleTree", { id: roleId, type: "tree" });
                treeHelper.loadTree();
            });
        } else {
            treeHelper = new TreeHelper(moduleRes, "#moduleTree", { id: roleId, type: "tree" });
            treeHelper.loadTree();
        }

        $scope.expandAll = function() {
            treeHelper.expandAll();
        }

        $scope.collapseAll = function() {
            treeHelper.collapseAll();
        }

        $scope.cancelChooseAll = function () {
            treeHelper.uncheckAll();
            //treeHelper.setParam({ id: "0000000007", type: "tree" });
            //treeHelper.loadTree();
        }
        $scope.chooseAll = function () {
            treeHelper.checkAll();
            //treeHelper.setParam({ id: "0aca6362c6", type: "tree" });
            //treeHelper.loadTree();
        }
        $scope.loadTree = function() {
            if (angular.isDefined($scope.Data.Org)) {
                treeHelper.setParam({ id: $scope.Data.Org.RoleTemplateId, type: "tree" });
                treeHelper.loadTree();
            }
        }

        groupRes.get({}, function(data) {
            $scope.Data.Groups = data.Data;
        });

        roleRes.get({ roleType: "SuperAdmin", Status: true, pageSize: 10 }, function(data) {
            $scope.Data.RoleTemplate = data.Data;
        });

        $scope.save = function() {
            $scope.Data.Org.CheckModuleList = treeHelper.getChecked();
            orgRes.save($scope.Data.Org, function (data) {
                if ($rootScope.Global.CurrentLoginSys == "LC")
                { $location.url('/angular/OrgList'); }
                else
                { $location.url('/dc/OrgList'); }
                
            });
            utility.message($scope.Data.Org.OrgName + "的信息保存成功！");
        }
    }]);
//.controller("codeListCtrl", function ($scope, $http, $location, $stateParams, codeRes, utility) {
//    $scope.Data = {};
//    //初始化
//    $scope.init = function () {
//        $scope.Dic = {};
//        $scope.Data = {};
//        codeRes.query({}, function (data) {
//            $scope.CodeList = data;
//        });
//    }

//    $scope.init();//初始化

//    $scope.save = function () {
//        if ($scope.options.length > 0) {
//            $scope.curItem.Options = JSON.parse($scope.options)
//            codeRes.save($scope.curItem, function (data) {
//                if (!$scope.curItem.id) {
//                    $scope.CodeList.push($scope.curItem)
//                }
//            });
//            $('#modalDetail').modal('hide');
//            utility.message("保存信息保存成功！");
//            $("#modalDetail").modal("hide");
//        }
//    }

//    $scope.editOrCreate = function (item) {
//        if (item) {
//            $scope.curItem = item;
//            $scope.options = JSON.stringify(item.Options);;
//        } else {
//            $scope.curItem = {};
//            $scope.options = "";
//        }
        
//    }
//    //根据关键字过滤结果
//    $scope.filterItems = function (item) {
//        if ($scope.Keyword) {
//            return (angular.isDefined(item.CodeName) && item.CodeName.indexOf($scope.Keyword) >= 0)
//             ||
//             (angular.isDefined(item.CodeId) && item.CodeId.indexOf($scope.Keyword.toUpperCase()) >= 0)
//        }
//        return true;
//    };
//    $scope.delete = function (id) {
//        codeRes.delete({ id: id }, function (data) {
//            if (data.$resolved) {
//                var whatIndex = null;
//                angular.forEach($scope.CodeList, function (cb, index) {
//                    if (cb.id === id) {
//                        whatIndex = index;
//                    }
//                });
//                $scope.CodeList.splice(whatIndex, 1);
//            }
//            utility.message("信息删除成功！");
//        });
//    }
//}
//);

