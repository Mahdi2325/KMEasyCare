/*
创建人:郝元彦
创建日期:2016-02-29
说明:固定费用设定
*/
//修改
//张正泉 2016-03-30 对接后端系统

angular.module("sltcApp")
.controller("fixedChargeCtrl", ['$scope', 'utility', 'fixedChargeRes', 'fixedChargeChargeGroupRes', 'chargeGroupRes', '$state', function ($scope, utility, fixedChargeRes, fixedChargeChargeGroupRes, chargeGroupRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.buttonShow = false;
    $scope.itemEdit = false;

    //当前正在编辑的项目
    $scope.loadPage = function () {
        $scope.curItem = { Period: "Month", RepeatCount: 0 };
        $scope.curResident = $scope.curResident || {};
        $scope.itemEdit = false;
    }

    $scope.init = function () {
        $scope.curItem = {};
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: fixedChargeRes,//异步请求的res
            params: { feeno: $scope.FeeNo == "" ? -1 : $scope.FeeNo },
            success: function (data) {//请求成功时执行函数
                $scope.fixedCharges = data.Data;
                chargeGroupRes.get({}, function (data) {
                    $scope.chargeGroups = data.Data;
                });
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.deleteItem = function (item) {
        fixedChargeRes.delete({ id: item.Id }, function () {
            $scope.fixedCharges.splice($scope.fixedCharges.indexOf(item), 1);
            $scope.options.search();
            utility.message("删除成功");
            $scope.itemEdit = false;
        });
    };

    $scope.createItem = function (item) {
        //保存
        fixedChargeRes.save(item, function (newItem) {
            $scope.fixedCharges.push(newItem.Data);
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.search();
            utility.message("添加成功");
        });
        $scope.loadPage();
    };

    $scope.updateItem = function (item) {
        fixedChargeRes.save(item, function () {

            $scope.loadPage();
            utility.message("修改成功");
        });
    };

    $scope.rowSelect = function (item) {
        $scope.itemEdit = true;
        $scope.curItem = item;
    };


    $scope.saveEdit = function (item) {
        item.FeeNo = $scope.curResident.FeeNo;
        item.RegNo = $scope.curResident.RegNo;
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        $scope.itemEdit = false;
    };

    //选中收费项目回调函数
    $scope.chargeSelected = function (chargeItem) {
            angular.extend($scope.curItem, {
                CostItemNo: chargeItem.CostItemNo,
                CostName: chargeItem.CostName,
                ItemUnit: chargeItem.ItemUnit,
                Price: chargeItem.Price,
                CostItemId: chargeItem.Id
            });     
    }

    //选中住民回调函数
    $scope.residentSelected = function (resident) {
        $scope.curResident = resident;//设置当前住民
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.feeno = $scope.curResident.FeeNo;
        $scope.options.search();

        $scope.loadPage();
        if (angular.isDefined($scope.curResident.FeeNo)) {
            $scope.buttonShow = true;
            $scope.itemEdit = false;
        }
    }

    // 导入费用套餐
    $scope.inputChargeGroup = function (groupId) {
        fixedChargeChargeGroupRes.save({ id: groupId, regNo: $scope.curResident.RegNo, feeNo: $scope.curResident.FeeNo, StartDate: $scope.curItem.GroupStartDate, EndDate: $scope.curItem.GroupEndDate }, function (data) {
            if (data.ResultCode == 1001) {
                utility.message("所选套餐内容为空，请添加此套餐内容！");
            }
            else {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.pageInfo.PageSize = 10;
                $scope.options.params.feeno = $scope.curResident.FeeNo;

                $scope.options.search();
                //$scope.options.params.feeno = $scope.curResident.FeeNo;
                //$scope.init();//加载住民固定费用设置
            }

        });
    }

    //开始结束时间校验
    $scope.datepickChange = function () {
        if (angular.isDefined($scope.curItem.GroupStartDate) && angular.isDefined($scope.curItem.GroupEndDate)) {
            if ($scope.curItem.GroupStartDate != "" && $scope.curItem.GroupEndDate != "") {
                if (!checkDate($scope.curItem.GroupStartDate, $scope.curItem.GroupEndDate)) {
                    utility.msgwarning("开始时间应在结束时间之前");
                    $scope.curItem.GroupEndDate = "";
                }
                else {
                    $scope.curItem.GroupStartDate = $scope.curItem.GroupStartDate;
                }
            }
            else {
                $scope.curItem.GroupStartDate = $scope.curItem.GroupStartDate;
            }
        }
    };

    $scope.dateChange = function () {
        if (angular.isDefined($scope.curItem.StartDate) && angular.isDefined($scope.curItem.EndDate)) {
            if ($scope.curItem.StartDate != "" && $scope.curItem.EndDate != "") {
                if (!checkDate($scope.curItem.StartDate, $scope.curItem.EndDate)) {
                    utility.msgwarning("开始时间应在结束时间之前");
                    $scope.curItem.EndDate = "";
                }
                else {
                    $scope.curItem.StartDate = $scope.curItem.StartDate;
                }
            }
            else {
                $scope.curItem.StartDate = $scope.curItem.StartDate;
            }
        }
    };
    $scope.init();
}]);
