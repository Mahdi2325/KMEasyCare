/*
创建人:张凯
创建日期:2016-02-24
说明: 用户管理
*/
angular.module("sltcApp")
.controller("userListCtrl", ['$scope', '$http', '$location', '$state', '$rootScope', 'userRes', 'DCDataDicListRes', 'utility', function ($scope, $http, $location, $state, $rootScope, userRes, DCDataDicListRes, utility) {
    $scope.Data = {};
    $scope.loadUsers = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: userRes,//异步请求的res
            params: { keyWord: "", orgid: $rootScope.Global.OrganizationId },
            success: function (data) {//请求成功时执行函数
                $scope.Data.Users = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        //userRes.query({}, function (data) {
        //    $scope.Data.Users = data;
        //});
        $scope.initorglist();
    }

    $scope.UserDelete = function (item) {
        if (confirm("您确定删除该用户信息吗?")) {
            userRes.delete({ id: item.UserId }, function (data) {
                if (data.ResultCode == 1)
                {
                    utility.message("删除成功");
                    $scope.Data.Users.splice($scope.Data.Users.indexOf(item), 1);
                }
                else
                {
                    utility.msgwarning(data.ResultMessage);
                }
            });
        }
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

    $scope.loadUsers();

    //cloudAdminUi.handleDataTables("users");

}])
.controller("userEditCtrl", ['$scope', '$http', '$location', 'utility', '$stateParams', '$rootScope', 'DCDataDicListRes', 'userRes', 'roleRes', function ($scope, $http, $location,utility, $stateParams, $rootScope, DCDataDicListRes, userRes, roleRes) {
    $scope.Data = {};
    $scope.isPwdChanged = true;
    $scope.Data.User = {};
    $scope.Roles = [];
    var ms = $('#selRoles').magicSuggest({
        placeholder: "",
        displayField: 'RoleName',
        style: "height: auto;",
        valueField: "RoleId",
        editable: false
    });
    if (!$stateParams.id) {
        roleRes.get({ pageSize: 0, orgid: $rootScope.Global.OrganizationId }, function (obj) {
            ms.setData(obj.Data);
        });
    } else {
        $scope.isPwdChanged = false;
    }

    //选择员工
    $scope.staffSelected = function (item) {
        $scope.Data.User.EmpNo = item.EmpNo;
    }
    if ($stateParams.id) {
        userRes.get({ id: $stateParams.id }, function (data) {
            $scope.Data.User = data;
            $scope.orgid = data.OrgId;
            roleRes.get({ pageSize: 0, orgid: data.OrgId }, function (obj) {
                ms.setData(obj.Data);
                if (data.RoleId != null) {
                    ms.setValue(data.RoleId);
                    //$('#selRoles').attr("style", "height: 34px;");
                }
            });


        });
        $scope.lockOrg =true;
    } else {
        $scope.Data.User.Status = true;
        $scope.lockOrg = false;
        $scope.orgid = $rootScope.Global.OrganizationId;
}

    $scope.initorglist = function () {
        //机构数据 SuperAdmin
        DCDataDicListRes.get({ flag: "33", staus: 0, datatyp: "1"
        }, function (data) {
            $scope.Orglist = data.Data;
           // $scope.orgid = $rootScope.Global.OrganizationId;
    });

        $scope.OrgISSelect = true;
        if ($rootScope.Global.MaximumPrivileges == "SuperAdmin") {
            $scope.OrgISSelect = false;
            $scope.showStatus = true;
        }
        else if ($rootScope.Global.MaximumPrivileges == "Admin") {
            $scope.showStatus = true;
        }
        else {
            $scope.showStatus = false;
            $scope.orgid = $rootScope.Global.OrganizationId;
    }
};
    $scope.initorglist();

    $scope.ChangeOrg = function (orgId) {
        $scope.orgid = orgId;
        $scope.Data.User.EmpNo = '';
        ms.clear();
        if ($scope.orgid == "") {
            ms.setData([]);
            return;
    }
        roleRes.get({ pageSize: 0, orgid: $scope.orgid
        }, function (obj) {
            ms.setData(obj.Data);
    });
}
    $scope.save = function () {
        if (ms.getValue().length == 0) {
            utility.message("角色为必填项!");
            return;
        }
        var userId = 0;
        if (!isEmpty($scope.Data.User.UserId)) {
            userId = $scope.Data.User.UserId;
            if (angular.isDefined($scope.newPassword) && $scope.newPassword != "") {
                $scope.Data.User.Pwd = $scope.newPassword;
            }
        } else {
            if (isEmpty($scope.newPassword)) {
                utility.message("密码不能为空!");
                return;
            }
            $scope.Data.User.Pwd = $scope.newPassword;
        }
        
      
        //相同ORG 相同员工不能绑定两个登录账户
       // var url = "api/users?currentPage=1&keyWord=" + $scope.Data.User.EmpNo + "&orgid=" + $scope.orgid + "&pageSize=10&fingtype=withempno";
        var url = "api/users?empNo=" + $scope.Data.User.EmpNo + "&loginName=" + $scope.Data.User.LogonName + "&orgid=" + $scope.orgid + "&userId=" + userId + "&fingtype=withempno";
        $http.get(url).success(function (response) {
                    if (response.ResultCode == 1) {
                        alert("此[选择用户]中的员工已经与其它登录名称做了关联，不能再与本登录用户做绑定，请修改！");
                        return;
                    } else if (response.ResultCode == 2) {
                        alert("此[" + $scope.Data.User.LogonName + "]登录名系统中已存在，请修改！");
                        return;
                    }
                
         
                    $scope.Data.User.RoleId = ms.getValue();
                    $scope.Data.User.OrgId = $scope.orgid;
                    userRes.save($scope.Data.User, function (data) {
                        if ($rootScope.Global.CurrentLoginSys == "LC")
                            { $location.url('/angular/UserList'); }
                        else
                            { $location.url('/dc/UserList');
                    }

                });
    });




}

}]);

