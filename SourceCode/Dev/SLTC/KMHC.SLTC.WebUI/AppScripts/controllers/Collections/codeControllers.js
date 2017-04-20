/*
创建人: 刘美方
创建日期:2016-04-20
说明:字典管理
*/

angular.module("sltcApp")
    .controller("codeListCtrl", ['$scope', '$rootScope', '$http', '$location', '$state', 'codeFileRes', 'utility', function ($scope, $rootScope, $http, $location, $state, codeFileRes, utility) {
        var roleType = $rootScope.Global.MaximumPrivileges;
        $scope.init = function () {
            $scope.items = [];
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: codeFileRes,//异步请求的res
                params: { keyword: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.items = [];
                    if (roleType == "SuperAdmin") {
                        angular.forEach(data.Data, function (item) {
                            item.isShow = false;
                            item.isDel = true;
                            $scope.items.push(item)
                        })
                    }
                    else if (roleType == "Admin") {
                        angular.forEach(data.Data, function (item) {
                            if (item.MODIFYFLAG != "H") {
                                item.isShow = false;
                                item.isDel = false;
                                $scope.items.push(item)
                            }
                        })
                    }
                    else if (roleType == "Normal") {
                        angular.forEach(data.Data, function (item) {
                            if (item.MODIFYFLAG == "A") {
                                item.isShow = false;
                                item.isDel = false;
                                $scope.items.push(item)
                            }
                            else if (item.MODIFYFLAG == "N") {
                                item.isShow = true;
                                item.isDel = false;
                                $scope.items.push(item)
                            }
                            else if (item.MODIFYFLAG == "Y") {
                                item.isShow = false;
                                item.isDel = true;
                                $scope.items.push(item)
                            }
                        })
                    }

                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }
        };
        $scope.delete = function (item) {
            if (confirm("确定删除该信息吗?")) {
                codeFileRes.delete({ ID: item.ITEMTYPE }, function (data) {
                    //$scope.options.pageInfo.CurrentPage = 1;
                    //$scope.options.search();
                    $scope.items.splice($scope.items.indexOf(item), 1);
                    utility.message("删除成功");
                });
            }
        };

        $scope.init();
    }])
.controller("codeEditCtrl", ['$scope', '$rootScope', '$location', '$stateParams', 'utility', 'codeFileRes', 'codeDtlRes', function ($scope, $rootScope, $location, $stateParams, utility, codeFileRes, codeDtlRes) {
    var roleType = $rootScope.Global.MaximumPrivileges;
    $scope.CodeFlaglist = $rootScope.Dics["A00.104"];
    $scope.init = function () {
        $scope.isAdd = true;
        $scope.isItemAdd = true;
        $scope.curItem = {};
        $scope.items = [];
        $scope.itemEdit = {};
        if ($stateParams.id) {
            $scope.isAdd = false;
            codeFileRes.get({ code: $stateParams.id }, function (data) {
                $scope.curItem = data;
                $scope.itemEdit.ITEMTYPE = $scope.curItem.ITEMTYPE;
                if (roleType == "SuperAdmin") {
                    $scope.isAddOnly = false;
                    $scope.isNormal = false;
                    $scope.isShowItemDetail = true;
                    $scope.isShowFoot = true;
                }
                else if (roleType == "Admin") {
                    $scope.CodeFlaglist.splice(1,1)
                    $scope.isAddOnly = false;
                    $scope.isNormal = false;
                    $scope.isShowItemDetail = true;
                    $scope.isShowFoot = true;
                }
                else if (roleType == "Normal") {
                    if ($scope.curItem.MODIFYFLAG == "A")
                    {
                        $scope.isAddOnly = true;
                        $scope.isNormal = true;
                        $scope.isShowItemDetail = true;
                        $scope.isShowFoot = true;
                    }
                    else if ($scope.curItem.MODIFYFLAG == "N") {
                        $scope.isAddOnly = false;
                        $scope.isNormal = true;
                        $scope.isShowItemDetail = false;
                        $scope.isShowFoot = false;
                    }
                    else
                    {

                        $scope.isAddOnly = false;
                        $scope.isNormal = true;
                        $scope.isShowItemDetail = true;
                        $scope.isShowFoot = true;
                    }
                }
            });
            codeDtlRes.get({ type: $stateParams.id }, function (data) {
                $scope.items = data.Data;
            });
        }
        else
        {
            if (roleType == "SuperAdmin") {
                $scope.isAddOnly = false;
                $scope.isNormal = false;
                $scope.isShowItemDetail = true;
                $scope.isShowFoot = true;
            }
            else if (roleType == "Admin") {
                $scope.CodeFlaglist.splice(1, 1)
                $scope.isAddOnly = false;
                $scope.isNormal = false;
                $scope.isShowItemDetail = true;
                $scope.isShowFoot = true;
            }
            else if (roleType == "Normal") {
                    $scope.curItem.MODIFYFLAG == "Y"
                    $scope.isAddOnly = false;
                    $scope.isNormal = true;
                    $scope.isShowItemDetail = true;
                    $scope.isShowFoot = true;  
            }
        }
    }
    $scope.save = function (item) {
        codeFileRes.save(item, function () {
            if ($scope.isAdd) {
                $scope.itemEdit.ITEMTYPE = item.ITEMTYPE;
            }
            utility.message("添加成功");
            // $location.url("/angular/CodeList");
        });
    };
    $scope.edit = function (item) {
        $scope.isItemAdd = false;
        $scope.itemEdit = item;
    }
    $scope.reset = function () {
        $scope.isItemAdd = true;
        $scope.itemEdit = {};
        $scope.itemEdit.ITEMTYPE = $scope.curItem.ITEMTYPE;
    }
    $scope.saveItem = function (item) {
        if (!angular.isDefined($scope.itemEdit.ITEMTYPE)) {
            utility.message("保存失败,请先保存大项字典。");
            return;
        }
        if (!angular.isDefined(item.ITEMCODE) && !angular.isDefined(item.ITEMNAME)) {
            utility.message("保存失败,必填项没有填！");
            return;
        }
        if ($scope.itemEdit.ITEMCODE.length <= 0 && $scope.itemEdit.ITEMNAME.length <= 0) {
            utility.message("保存失败,必填项没有填！");
            return;
        }
        if ((angular.isString($scope.itemEdit.ITEMCODE) && $scope.itemEdit.ITEMCODE.length > 20) ||
            (angular.isString($scope.itemEdit.ITEMNAME) && $scope.itemEdit.ITEMNAME.length > 100) ||
            (angular.isString($scope.itemEdit.DESCRIPTION) && $scope.itemEdit.DESCRIPTION.length > 256)) {
            utility.message("保存失败,字段过长！");
            return;
        };
        codeDtlRes.save(item, function () {
            utility.message("保存成功");
            $scope.itemEdit = {};
            $scope.itemEdit.ITEMTYPE = $scope.curItem.ITEMTYPE;
            if ($scope.isItemAdd) {
                $scope.items.push(item);
            }


        });
    };

    $scope.deleteItem = function (item) {
        if (confirm("确定删除该信息吗?")) {
            codeDtlRes.delete({ type: item.ITEMTYPE, code: item.ITEMCODE }, function (data) {
                utility.message("删除成功");
                $scope.items.splice($scope.items.indexOf(item), 1);
            });
        }
    };

    $scope.returnList = function () {
        $location.url('/angular/CodeList');
    };

    $scope.init();
}]);
