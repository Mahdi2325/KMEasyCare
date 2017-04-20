/*
创建人: 杨鹏
创建日期:2017-1-10
说明:机构服务项目管理
*/

angular.module("sltcApp")
.controller("chargeItemNSServiceCtrl", ['$scope', 'utility', 'chargeItemNSServiceRes', function ($scope, utility, chargeItemNSServiceRes) {
    $scope.init = function () {
        $scope.Keyword = "";
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeItemNSServiceRes,//异步请求的res
            params: { keyWord: "" },
            success: function (data) {//请求成功时执行函数
                //if (data.Data == null || data.Data == undefined) {
                //    utility.message("查询不到服务项目信息!");
                //    return;
                //} else {
                //    if (data.Data.length == 0) {
                //        utility.message("查询不到服务项目信息!");
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
        if (confirm("确定删除该服务项目吗?")) {
            chargeItemNSServiceRes.delete({ id: item.ServiceId }, function () {
                $scope.chargeItems.splice($scope.chargeItems.indexOf(item), 1);
                $scope.options.search();
                utility.message("删除成功");
            });
        }
    };

    $scope.init();
}])
.controller("chargeItemNSServiceEditCtrl", ['$scope', '$location', '$http', '$stateParams', 'utility', 'chargeItemNSServiceRes', 'nciServiceRes', function ($scope, $location, $http, $stateParams, utility, chargeItemNSServiceRes, nciServiceRes) {
    $scope.nciItemList = {};
    $scope.selectedNCIItem = {};
    $scope.curItem = {};
    $scope.selectNCIItemModal = $("#selectNCIItem");
    $scope.nsServiceListUrl = "/angular/ChargeItemSetting/ServiceList";
    $scope.DrugStatusCheckd = true;
    $scope.init = function () {
        if ($stateParams.id) {
            chargeItemNSServiceRes.get({ id: $stateParams.id }, function (data) {
                $scope.curItem = data.Data;
                $scope.ServiceId = data.ServiceId;

                if ($scope.curItem.Status == 0) {
                    $('input[name="radio"]:eq(0)').attr('checked', 'true');
                } else {
                    $('input[name="radio"]:eq(1)').attr('checked', 'true');
                }
            });

        } else {
            $scope.curItem = {};
        }

        if ($scope.curItem.ServiceName != null || $scope.curItem.ServiceName != undefined) {
            $scope.options.params.keyWord = $scope.curItem.ServiceName;
            $scope.options.search();
        }
    }

    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: nciServiceRes,//异步请求的res
        params: { keyWord: "" },
        success: function (data) {//请求成功时执行函数
            //if (data.Data == null || data.Data == undefined) {
            //    utility.message("查询不到护理险服务项目信息!");
            //} else {
            //    if (data.Data.length == 0) {
            //        utility.message("查询不到护理险服务项目信息!");
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
        $scope.searchNCIService();
    };

    $scope.searchNCIService = function () {
        if ($scope.curItem.ServiceName != null || $scope.curItem.ServiceName != undefined) {
            $scope.options.params.keyWord = $scope.curItem.ServiceName;
        }
        $scope.options.search();
    };

    $scope.createItem = function (item) {
        chargeItemNSServiceRes.save(item, function (newItem) {
            utility.message("添加成功");
            $location.url($scope.nsServiceListUrl);
        });
    };

    $scope.updateItem = function (item) {
        chargeItemNSServiceRes.save(item, function () {
            utility.message("修改成功");
            $location.url($scope.nsServiceListUrl);
        });
    };

    $scope.cancelEdit = function () {
        $location.url($scope.nsServiceListUrl);
    };

    $scope.saveEdit = function (item) {
        if ($scope.curItem.ServiceName == undefined || $scope.curItem.ServiceName == '' || $scope.curItem.ServiceName == null) {
            utility.msgwarning("名称不得是空白!");
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



        if ($scope.curItem.IsNCIItem &&  $scope.curItem.MCServiceCode == "") {
            utility.message("请点击[查询带入护理险服务项目目录]并选择护理险服务项目!");
            return;
        }

        if (angular.isDefined(item.ServiceId)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
    };

    $scope.selectNCIService = function (item) {
        chargeItemNSServiceRes.getByMCServiceCode({ mcServiceCode: item.ServiceCode }, function (data) {
            if (data && data.Data) {
                utility.message("此服务项目已经存在!");
            }
            else {
                $scope.updateDataFromNCIItem(item);
            }
        });
    };

    $scope.updateDataFromNCIItem = function (item) {
        $scope.selectedNCIItem = item;
        $scope.curItem.ServiceName = item.ServiceName;
        $scope.curItem.MCServiceCode = item.ServiceCode;
        $scope.curItem.Units = item.Units;
        $scope.curItem.PinYin = item.PinYin;
        $scope.curItem.GuidePrice = item.GuidePrice;
        $scope.modalWndToggle();
    }

    $scope.onIsNCIItemValueChange = function () {
        if (!$scope.curItem.IsNCIItem) {
            $scope.selectedNCIItem = null;
            $scope.curItem.MCServiceCode = "";
            $scope.curItem.GuidePrice = 0;
        }
    }

    
    $scope.SetStatus = function () {
        var item = $('#wrap input[name="radio"]:checked').val();
        if (item==0){
            $scope.curItem.Status = 0;
        } else {
            $scope.curItem.Status = 1;
        }
    }

    $scope.init();
}])