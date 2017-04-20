/*
创建人:张凯
创建日期:2016-02-24
说明: 角色管理
*/
angular.module("sltcApp")
.controller("roleListCtrl", ['$scope', '$http', '$location', '$rootScope', 'roleRes', 'DCDataDicListRes', 'utility', function ($scope, $http, $location, $rootScope, roleRes, DCDataDicListRes, utility) {
    $scope.Data = {};
    $scope.loadRoles = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: roleRes,//异步请求的res
            params: { roleName: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.Roles = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        //roleRes.get({ roleName: "", roleType: "Normal", currentPage: 1, pageSize: 10 }, function (data) {
        //    $scope.Data.Roles = data.Data;
        //});
        $scope.initorglist();
    }

    $scope.initorglist = function () {
        //机构数据 SuperAdmin
        DCDataDicListRes.get({ flag: "33", staus: 0, datatyp: "1" }, function (data) {
            $scope.Orglist = data.Data;
            $scope.options.params.orgid = $rootScope.Global.OrganizationId;
        });

        $scope.OrgISSelect = true;
        if ($rootScope.Global.MaximumPrivileges == "SuperAdmin")
        { $scope.OrgISSelect = false; }
    };

    $scope.RoleDelete = function (item) {
        if (confirm("您确定删除该角色信息吗?")) {
            roleRes.delete({ id: item.RoleId }, function (data) {
                utility.message(data.ResultMessage);

                if (data.ResultCode == 1)
                {
                    $scope.Data.Roles.splice($scope.Data.Roles.indexOf(item), 1);
                }
            });
        }
    }

    $scope.loadRoles();
}])
.controller("roleEditCtrl", ['$scope', '$http', '$location', '$stateParams', '$rootScope', 'dictionary', 'roleRes', 'moduleRes', 'DCDataDicListRes', function ($scope, $http, $location, $stateParams, $rootScope, dictionary, roleRes, moduleRes, DCDataDicListRes) {
    $scope.Data = {};
    $scope.RoleId = "0"
    if ($stateParams.id) {
        $scope.RoleId = $stateParams.id;
        roleRes.get({ id: $stateParams.id }, function (data) {
            $scope.Data.Role = data.Data;
            $scope.RoleId = data.Data.RoleId;
        });
    }
    else {
        $scope.Data.Role = {};
        $scope.Data.Role.Status = true;
        $scope.Data.Role.RoleType = "Normal";
    }

    $scope.change = function () {
        $('#roleName').trigger('blur');
    };

    
    var treeHelper = new TreeHelper(moduleRes, "#moduleTree", { id: $scope.RoleId, type: "tree", loadTreeByRole: true });

    $scope.expandAll = function () {
        treeHelper.expandAll();
    }

    $scope.collapseAll = function ()
    {
        treeHelper.collapseAll();
    }
    $scope.chooseAll = function () {
        treeHelper.checkAll();
    }

    $scope.cancelChooseAll = function () {
        treeHelper.uncheckAll();
    }
    $scope.save = function () {
        $scope.Data.Role.CheckModuleList = treeHelper.getChecked();
        roleRes.save($scope.Data.Role, function (data) {
            if ($rootScope.Global.CurrentLoginSys == "LC")
            { $location.url('/angular/RoleList'); }
            else
            { $location.url('/dc/RoleList'); }
            
        });
    }

    $scope.initorglist = function () {
        //机构数据 SuperAdmin
        DCDataDicListRes.get({ flag: "33", staus: 0, datatyp: "1" }, function (data) {
            $scope.Orglist = data.Data;
        });

        $scope.OrgISSelect = true;
        if ($rootScope.Global.MaximumPrivileges == "SuperAdmin")
        { $scope.OrgISSelect = false; }
    };

    treeHelper.loadTree();
    $scope.initorglist();


}]);

