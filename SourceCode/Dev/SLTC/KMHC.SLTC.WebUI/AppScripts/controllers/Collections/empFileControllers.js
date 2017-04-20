/*
创建人:张凯
创建日期:2016-02-23
说明: 员工管理
*/
angular.module("sltcApp")
.controller("staffListCtrl", ['$scope', '$http', '$location', '$rootScope', '$state', 'empFileRes', 'roleRes', 'DCDataDicListRes', function ($scope, $http, $location, $rootScope, $state, empFileRes, roleRes, DCDataDicListRes) {
    $scope.Data = {};
    //$scope.searchWords = "";
    $scope.loadEmpFile = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: empFileRes,//异步请求的res
            params: { empName: "", orgid: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.empFiles = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        //empFileRes.get({ empNo: $scope.searchWords, empName: $scope.searchWords, empGroup: "", currentPage: 1, pageSize: 10 }, function (response) {
        //    $scope.Data.empFiles = response.Data;
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

    $scope.StaffDelete = function (item) {
        if (confirm("您确定删除该员工信息吗?")) {
            empFileRes.delete({ empNo: item.EmpNo, orgId: $scope.options.params.orgid }, function (data) {
                $scope.Data.empFiles.splice($scope.Data.empFiles.indexOf(item), 1);
               // $scope.loadEmpFile();
            });
        }
    }

    $scope.loadEmpFile();
}])
.controller("staffEditCtrl", ['$scope', '$http', '$location', '$stateParams', '$rootScope', 'utility', 'empFileRes', 'orgRes', 'roleRes', 'deptRes', 'groupRes', 'DCDataDicListRes',
    function ($scope, $http, $location, $stateParams, $rootScope, utility, empFileRes, orgRes, roleRes, deptRes, groupRes, DCDataDicListRes) {
        $scope.Data = {};
        $scope.Data.empFile = {};
        $scope.EmpNo = 'null';
        $scope.Info = {};
        $scope.loadModelList = function () {
            //orgRes.get({ }, function (data) {
            //    $scope.Info.Orgs = data.Data;
            //});

            roleRes.get({}, function (data) {
                $scope.Info.Roles = data.Data;
            });

            deptRes.get({}, function (data) {
                $scope.Info.Depts = data.Data;
            });

            groupRes.get({}, function (data) {
                $scope.Info.Groups = data.Data;
            });

            $scope.initorglist();
        };

        //如果选择了机构，部门跟着级联过滤zhongyh
        $scope.Orgchange = function () {
            deptRes.get({ orgid: $scope.Data.empFile.OrgId }, function (data) {
                $scope.Info.Depts = data.Data;
            });
        }
        //根据身份证获取性别
        $scope.Getsex = function (psidno) {
            if (psidno == undefined || psidno==null) {
                return;
            }
            var sexno;
            if (psidno.length == 10) {
                sexno = psidno.substring(1, 2);
            } else if (psidno.length == 15) {
                sexno = psidno.substring(14, 15)
            } else if (psidno.length == 18) {
                sexno = psidno.substring(16, 17)
            }
            else {
                alert("错误的身份证号码，请核对！");
                return;
            }
            var tempid = sexno % 2;
            if (tempid == 0) {
                $scope.Data.empFile.Sex = "F";
            } else {
                $scope.Data.empFile.Sex = "M";
            }
        }
        $scope.initorglist = function () {
            //机构数据 SuperAdmin
            DCDataDicListRes.get({ flag: "33", staus: 0, datatyp: "1" }, function (data) {
                $scope.Orglist = data.Data;
                //$scope.options.params.orgid = $rootScope.Global.OrganizationId;
            });

            $scope.OrgISSelect = true;
            if ($rootScope.Global.MaximumPrivileges == "SuperAdmin")
            { $scope.OrgISSelect = false; }
        };

        $scope.loadModelList();



        //$scope.Show = {};
        //$scope.selectOrg = function (id) {
        //    if (id == null) return;
        //    orgRes.get({ id: id }, function (data) {
        //        $scope.Orglist = data;
        //        $scope.Show.OrgName = $scope.Orglist.OrgName;
        //        $scope.Data.empFile.OrgId = $scope.Orglist.OrgId;
        //    });
        //    $scope.Orglist = {};
        //    $('#modalOrgList').modal('hide');
        //}

        //$scope.selectRole = function (id) {
        //    if (id == null) return;
        //    roleRes.get({ id: id }, function (data) {
        //        $scope.Rolelist = data;
        //        $scope.Show.RoleName = $scope.Rolelist.RoleName;
        //        $scope.Data.empFile.JobTitle = $scope.Rolelist.RoleId;
        //    });
        //    $scope.Rolelist = {};
        //    $('#modalRoleList').modal('hide');
        //}

        //$scope.selectGroup = function (id) {
        //    if (id == null) return;
        //    groupRes.get({ id: id }, function (data) {
        //        $scope.Grouplist = data;
        //        $scope.Show.GroupName = $scope.Grouplist.GroupName;
        //        $scope.Data.empFile.EmpGroup = $scope.Grouplist.GroupId;
        //    });
        //    $scope.Grouplist = {};
        //    $('#modalGroupList').modal('hide');
        //}

        if ($stateParams.id) {
            empFileRes.get({ id: $stateParams.id }, function (data) {
                $scope.Data.empFile = data;
                $scope.EmpNo = $scope.Data.empFile.EmpNo;
                //$scope.selectOrg($scope.Data.empFile.OrgId);
                //$scope.selectRole($scope.Data.empFile.JobTitle);
                //$scope.selectGroup($scope.Data.empFile.EmpGroup);
            });
            $scope.lockOrg = true;
        }
        else {
            $scope.lockOrg = false;
        }

        $scope.save = function () {
            empFileRes.save($scope.Data.empFile, function (data) {
                if (data.ResultCode == 1001) {
                    utility.message("保存失败，存在相同的身份证号码员工！");
                    return;
                }
                else {
                    if ($rootScope.Global.CurrentLoginSys == "LC")
                    { $location.url('/angular/StaffList'); }
                    else
                    { $location.url('/dc/StaffList'); }

                    utility.message($scope.Data.empFile.EmpName + "的信息保存成功！");
                }
            });
           
        }
    }]);

