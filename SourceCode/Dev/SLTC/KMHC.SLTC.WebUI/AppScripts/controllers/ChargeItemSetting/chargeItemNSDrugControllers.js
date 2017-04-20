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
                //if (data.Data == null || data.Data == undefined) {
                //    utility.message("查询不到药品信息!");
                //    return;
                //} else {
                //    if (data.Data.length == 0) {
                //        utility.message("查询不到药品信息!");
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
            utility.message("查询长度过长,请重新输入!");
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    }

    $scope.deleteItem = function (item) {
        if (confirm("确定删除该药品信息吗?")) {
            chargeItemNSDrugRes.delete({ id: item.DrugId }, function () {
                $scope.chargeItems.splice($scope.chargeItems.indexOf(item), 1);
                $scope.options.search();
                utility.message("删除成功");
            });
        }
    };

    $scope.init();
}])
.controller("chargeItemNSDrugEditCtrl", ['$scope', '$location', '$http', '$stateParams', 'utility', 'chargeItemNSDrugRes', 'nciDrugRes', function ($scope, $location, $http, $stateParams, utility, chargeItemNSDrugRes, nciDrugRes) {
    $scope.nciItemList = {};
    $scope.selectedNCIItem = {};
    $scope.curItem = {};
    $scope.selectNCIItemModal = $("#selectNCIItem");
    $scope.nsDrugListUrl = "/angular/ChargeItemSetting/DrugList";

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: nciDrugRes,//异步请求的res
            params: { keyWord: "" },
            success: function (data) {//请求成功时执行函数
                //if (data.Data == null || data.Data == undefined) {
                //    utility.message("查询不到护理险药品信息!");
                //} else {
                //    if (data.Data.length == 0) {
                //        utility.message("查询不到护理险药品信息!");
                //    }
                //}
                $scope.nciItemList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        if ($stateParams.id) {
            chargeItemNSDrugRes.get({ id: $stateParams.id }, function (data) {
                $scope.curItem = data.Data;
                $scope.DrugId = data.DrugId;

                if ($scope.curItem.Status == 0) {
                    $('input[name="radio"]:eq(0)').attr('checked', 'true');
                } else {
                    $('input[name="radio"]:eq(1)').attr('checked', 'true');
                }
            });
        } else {
            $scope.curItem = {};
            $scope.curItem.MCType = "004";
        }

        if ($scope.curItem.CNName != null || $scope.curItem.CNName != undefined) {
            $scope.options.params.keyWord = $scope.curItem.CNName;
            $scope.options.search();
        }

        $scope.curItem.ConversionRatio = 1;
    }

    $scope.modalWndToggle = function () {
        $scope.selectNCIItemModal.modal("toggle");
        $scope.searchNCIDrug();
    };

    $scope.searchNCIDrug = function () {
        if ($scope.curItem.CNName != null || $scope.curItem.CNName != undefined) {
            $scope.options.params.keyWord = $scope.curItem.CNName;
        }
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
        if ($scope.curItem.CNName == undefined || $scope.curItem.CNName == '' || $scope.curItem.CNName == null) {
            utility.msgwarning("中文名称不得是空白!");
            return;
        };

        if ($scope.curItem.DrugType == undefined || $scope.curItem.DrugType == '' || $scope.curItem.DrugType == null) {
            utility.msgwarning("药品种类不得是空白!");
            return;
        };



        if ($scope.curItem.Spec == undefined || $scope.curItem.Spec == '' || $scope.curItem.Spec == null) {
            utility.msgwarning("规格不得是空白!");
            return;
        };

        if ($scope.curItem.Units == undefined || $scope.curItem.Units == '' || $scope.curItem.Units == null) {
            utility.msgwarning("计价单位不得是空白!");
            return;
        };

        if ($scope.curItem.MinPackage == undefined || $scope.curItem.MinPackage == '' || $scope.curItem.MinPackage == null) {
            utility.msgwarning("最小包装剂量不得是空白!");
            return;
        };

        if ($scope.curItem.ConversionRatio == undefined || $scope.curItem.ConversionRatio == '' || $scope.curItem.ConversionRatio == null) {
            utility.msgwarning("转换比不得是空白!");
            return;
        };

        if ($scope.curItem.PrescribeUnits == undefined || $scope.curItem.PrescribeUnits == '' || $scope.curItem.PrescribeUnits == null) {
            utility.msgwarning("开药单位不得是空白!");
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

        if ($scope.curItem.IsNCIItem && $scope.curItem.MCDrugCode == "") {
            utility.message("请点击[查询带入护理险药品目录]并选择护理险药品!");
            return;
        }

        if (angular.isDefined(item.DrugId)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
    };

    $scope.selectNCIDrug = function (item) {
        chargeItemNSDrugRes.getByMCDrugCode({ mcDrugCode: item.DrugCode }, function (data) {
            if (data && data.Data) {
                utility.message("此药品已经存在!");
            }
            else {
                $scope.updateDataFromNCIItem(item);
            }
        });
    };

    $scope.updateDataFromNCIItem = function (item) {
        $scope.selectedNCIItem = item;
        $scope.curItem.CNName = item.CNName;
        $scope.curItem.ENName = item.ENName;
        $scope.curItem.MCType = item.MCType;
        $scope.curItem.DrugType = item.DrugType;
        $scope.curItem.MCDrugCode = item.DrugCode;
        $scope.curItem.Form = item.Form;
        $scope.curItem.GuidePrice = item.GuidePrice;
        $scope.curItem.PinYin = item.PinYin;
        $scope.curItem.Spec = item.Spec;
        $scope.curItem.BatchNo = item.BatchNo;

        $scope.modalWndToggle();
    }

    $scope.onIsNCIItemValueChange = function () {
        if (!$scope.curItem.IsNCIItem) {
            $scope.selectedNCIItem = null;
            $scope.curItem.MCDrugCode = "";
            $scope.curItem.GuidePrice = 0;
            $scope.curItem.MCType = "004";
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