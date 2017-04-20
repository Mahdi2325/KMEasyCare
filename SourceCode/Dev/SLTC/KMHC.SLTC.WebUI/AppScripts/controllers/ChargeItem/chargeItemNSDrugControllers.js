/*
创建人: 杨鹏
创建日期:2017-1-10
说明:机构药品管理
*/

angular.module("sltcApp")
.controller("chargeItemNSDrugCtrl", ['$scope', 'utility', 'chargeItemNSDrugRes', function ($scope, utility, chargeItemNSDrugRes) {
    $scope.init = function () {
        $scope.Keyword = "";
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeItemNSDrugRes,//异步请求的res
            params: { keyWord: "" },
            success: function (data) {//请求成功时执行函数
                $scope.chargeItems = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.searchInfo = function () {
        if ($scope.options.params.keyWord.length > 50) {
            $scope.options.params.keyWord = "";
            utility.message("搜索长度过长,请重新输入!");
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    }

    $scope.deleteItem = function (item) {
        chargeItemNSDrugRes.delete({ id: item.DrugId }, function () {
            $scope.chargeItems.splice($scope.chargeItems.indexOf(item), 1);
            $scope.options.search();
            utility.message("删除成功");
        });

    };

    $scope.init();
}])
.controller("chargeItemNSDrugEditCtrl", ['$scope', '$location', '$http', '$stateParams', 'utility', 'chargeItemNSDrugRes', 'nciDrugRes', function ($scope, $location, $http, $stateParams, utility, chargeItemNSDrugRes, nciDrugRes) {
    $scope.nciItemList = {};
    $scope.selectedNCIItem = {};
    $scope.curItem = {};
    $scope.selectNCIItemModal = $("#selectNCIItem");
    $scope.btnQueryNCIItem = $("#btnQueryNCIItem");
    $scope.nsDrugListUrl = "/angular/ChargeItem/DrugList";
    $scope.init = function () {
        if ($stateParams.id) {
            chargeItemNSDrugRes.get({ id: $stateParams.id }, function (data) {
                $scope.curItem = data.Data;
                $scope.DrugId = data.DrugId;
            });

        } else {
            $scope.curItem = {};
        }
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: nciDrugRes,//异步请求的res
            params: { keyWord: "" },
            success: function (data) {//请求成功时执行函数
                $scope.nciItemList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.modalWndToggle = function () {
        $scope.selectNCIItemModal.modal("toggle");
    };

    $scope.searchNCIDrug = function () {
        if (!$scope.curItem.CNName || $scope.curItem.CNName == "") {
            utility.message("请输入药品中文名称!");
            return;
        }
        $scope.options.params.keyWord = $scope.curItem.CNName;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

    $scope.createItem = function (item) {
        chargeItemNSDrugRes.save(item, function (newItem) {
            utility.message("添加成功");
            $location.url($scope.nsDrugListUrl);
        });
    };

    $scope.updateItem = function (item) {
        chargeItemNSDrugRes.save(item, function () {
            utility.message("修改成功");
            $location.url($scope.nsDrugListUrl);
        });
    };

    $scope.cancelEdit = function () {
        $location.url($scope.nsDrugListUrl);
    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.DrugId)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
    };

    $scope.selectNCIDrug = function (item) {
        $scope.selectedNCIItem = item;
        $scope.curItem.CNName = item.CNName;
        $scope.curItem.ENName = item.ENName;
        $scope.curItem.MCType = item.MCType;
        $scope.curItem.DrugType = item.DrugType;
        $scope.curItem.MCDrugCode = item.DrugCode;
        $scope.curItem.Form = item.Form;
        $scope.curItem.GuidePrice = item.GuidePrice;
        $scope.modalWndToggle();
    };
    $scope.init();
}])