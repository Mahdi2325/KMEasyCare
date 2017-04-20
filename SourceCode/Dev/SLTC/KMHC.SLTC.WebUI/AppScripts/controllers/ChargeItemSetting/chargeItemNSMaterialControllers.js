/*
创建人: 杨鹏
创建日期:2017-1-10
说明:机构耗材管理
*/

angular.module("sltcApp")
.controller("chargeItemNSMaterialCtrl", ['$scope', 'utility', 'chargeItemNSMaterialRes', function ($scope, utility, chargeItemNSMaterialRes) {
    $scope.init = function () {
        $scope.Keyword = "";
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeItemNSMaterialRes,//异步请求的res
            params: { keyWord: "" },
            success: function (data) {//请求成功时执行函数
                //if (data.Data == null || data.Data == undefined) {
                //    utility.message("查询不到耗材信息!");
                //    return;
                //} else {
                //    if (data.Data.length == 0) {
                //        utility.message("查询不到耗材信息!");
                //        return;
                //    }
                //}
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
        if (confirm("确定删除该耗材信息吗?")) {
            chargeItemNSMaterialRes.delete({ id: item.MaterialId }, function () {
                $scope.chargeItems.splice($scope.chargeItems.indexOf(item), 1);
                $scope.options.search();
                utility.message("删除成功");
            });
        }
    };

    $scope.init();
}])
.controller("chargeItemNSMaterialEditCtrl", ['$scope', '$location', '$http', '$stateParams', 'utility', 'chargeItemNSMaterialRes', 'nciMaterialRes', function ($scope, $location, $http, $stateParams, utility, chargeItemNSMaterialRes, nciMaterialRes) {
    $scope.nciItemList = {};
    $scope.selectedNCIItem = {};
    $scope.curItem = {};
    $scope.selectNCIItemModal = $("#selectNCIItem");
    $scope.nsMaterialListUrl = "/angular/ChargeItemSetting/MaterialList";
    $scope.init = function () {
        if ($stateParams.id) {
            chargeItemNSMaterialRes.get({ id: $stateParams.id }, function (data) {
                $scope.curItem = data.Data;
                $scope.MaterialId = data.MaterialId;

                if ($scope.curItem.Status == 0) {
                    $('input[name="radio"]:eq(0)').attr('checked', 'true');
                } else {
                    $('input[name="radio"]:eq(1)').attr('checked', 'true');
                }
            });

        } else {
            $scope.curItem = {};
        }

        if ($scope.curItem.MaterialName != null || $scope.curItem.MaterialName != undefined) {
            $scope.options.params.keyWord = $scope.curItem.MaterialName;
            $scope.options.search();
        }
    }

    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: nciMaterialRes,//异步请求的res
        params: { keyWord: "" },
        success: function (data) {//请求成功时执行函数
            //if (data.Data == null || data.Data == undefined) {
            //    utility.message("查询不到护理险耗材信息!");
            //} else {
            //    if (data.Data.length == 0) {
            //        utility.message("查询不到护理险耗材信息!");
            //    }
            //}
            $scope.nciItemList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }

    $scope.modalWndToggle = function () {
        $scope.selectNCIItemModal.modal("toggle");
        $scope.searchNCIMaterial();
    };

    $scope.searchNCIMaterial = function () {
        if ($scope.curItem.MaterialName != null || $scope.curItem.MaterialName != undefined) {
            $scope.options.params.keyWord = $scope.curItem.MaterialName;
        }
        $scope.options.search();
    };

    $scope.createItem = function (item) {
        chargeItemNSMaterialRes.save(item, function (newItem) {
            utility.message("添加成功");
            $location.url($scope.nsMaterialListUrl);
        });
    };

    $scope.updateItem = function (item) {
        chargeItemNSMaterialRes.save(item, function () {
            utility.message("修改成功");
            $location.url($scope.nsMaterialListUrl);
        });
    };

    $scope.cancelEdit = function () {
        $location.url($scope.nsMaterialListUrl);
    };

    $scope.saveEdit = function (item) {
        if ($scope.curItem.MaterialName == undefined || $scope.curItem.MaterialName == '' || $scope.curItem.MaterialName == null) {
            utility.msgwarning("名称不得是空白!");
            return;
        };

        if ($scope.curItem.MaterialType == undefined || $scope.curItem.MaterialType == '' || $scope.curItem.MaterialType == null) {
            utility.msgwarning("耗材种类不得是空白!");
            return;
        };

        if ($scope.curItem.Units == undefined || $scope.curItem.Units == '' || $scope.curItem.Units == null) {
            utility.msgwarning("单位不得是空白!");
            return;
        };

        if ($scope.curItem.UnitPrice == undefined || $scope.curItem.UnitPrice == '' || $scope.curItem.UnitPrice == null) {
            utility.msgwarning("单价不得是空白!");
            return;
        };

        if ($scope.curItem.AccountingId == undefined || $scope.curItem.AccountingId == '' || $scope.curItem.AccountingId == null) {
            utility.msgwarning("会计科目不得是空白!");
            return;
        };

        if ($scope.curItem.ChargeTypeId == undefined || $scope.curItem.ChargeTypeId == '' || $scope.curItem.ChargeTypeId == null) {
            utility.msgwarning("费用类别不得是空白!");
            return;
        };



        
        if ($scope.curItem.IsNCIItem &&  $scope.curItem.MCMaterialCode == "") {
            utility.message("请点击[查询带入护理险耗材目录]并选择护理险耗材!");
            return;
        }

        if (angular.isDefined(item.MaterialId)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
    };

    $scope.selectNCIMaterial = function (item) {
        chargeItemNSMaterialRes.getByMCMaterialCode({ mcMaterialCode: item.MaterialCode }, function (data) {
            if (data && data.Data) {
                utility.message("此耗材已经存在!");
            }
            else {
                $scope.updateDataFromNCIItem(item);
            }
        });
    };

    $scope.updateDataFromNCIItem = function (item) {
        $scope.selectedNCIItem = item;
        $scope.curItem.MaterialName = item.MaterialName;
        $scope.curItem.MaterialType = item.MaterialType;
        $scope.curItem.MCMaterialCode = item.MaterialCode;
        $scope.curItem.GuidePrice = item.GuidePrice;
        $scope.modalWndToggle();
    }

    $scope.onIsNCIItemValueChange = function () {
        if (!$scope.curItem.IsNCIItem) {
            $scope.selectedNCIItem = null;
            $scope.curItem.MCMaterialCode = "";
            $scope.curItem.GuidePrice = 0;
        }
    }

    $scope.SetStatus = function () {
        var item = $('#wrap input[name="radio"]:checked').val();
        if (item == 0) {
            $scope.curItem.Status = 0;
        } else {
            $scope.curItem.Status = 1;
        }
    }

    $scope.init();
}])