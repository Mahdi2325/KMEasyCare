/*
创建人:张祥
创建日期:2016-09-19
修改人:
修改日期:
说明: 评估模板管理
*/
angular.module("sltcApp")
.controller("EvalTemplateSetCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'evalTempSetRes', 'DCDataDicListRes', '$rootScope', function ($scope, $http, $location, $state, utility, evalTempSetRes, DCDataDicListRes, $rootScope) {
    $scope.Status = false;
    //如果是超级管理员显示机构 其它角色不显示
    if ($rootScope.Global.MaximumPrivileges == "SuperAdmin") {
        $scope.OrgISSelect = true;
    }
    else {
        $scope.OrgISSelect = false;
    }
    //机构数据 SuperAdmin
    DCDataDicListRes.get({ flag: "33", staus: 0, datatyp: "1" }, function (data) {
        $scope.Orglist = data.Data;
        $scope.orgId = $rootScope.Global.OrganizationId;
    });
    //evalTempSetRes.get({ CurrentPage: 1, PageSize: 1000 }, function (data) {
    //    $scope.reportList = data.Data;
    //});

    $scope.MajorName = function (CategoryId) {
        return CategoryId == 1 ? "社工类评估" : CategoryId == 2 ? "护理类评估" : CategoryId == 3 ? "满意度调查问卷" : "其它";
    };

    $scope.selectAll = function () {
        $.each($scope.reportList, function (i, v) {
            $.each(v.Items, function (ci, cv) {
                cv.Status = !$scope.Status;
            });
        });
    };

    $scope.chk = function () {
        var num_all = $("#chkdiv :checkbox").size();
        var num_checked = $("#chkdiv :checkbox:checked").size(); //选中个数 

        alert(num_all);
        alert(num_checked);
        if (num_all == num_checked) { //若选项总个数等于选中个数  
            $scope.Status = true;
        } else {
            $scope.Status = false;
        }
    };


    $scope.save = function () {
        var postData = $scope.reportList;
        $scope.pData = {};
        $scope.pData.Items = postData;
        $scope.pData.OrgId = $scope.orgId;
        evalTempSetRes.save($scope.pData, function () {
            utility.message("保存成功！");
        });
    };

    $scope.$watch('orgId', function (newValue, oldValue) {
        if (angular.isDefined(newValue)) {
            evalTempSetRes.get({ CurrentPage: 1, PageSize: 1000, orgId: newValue }, function (data) {
                $scope.reportList = data.Data;
                $scope.Status = false;
            });
        }
    });
}])
