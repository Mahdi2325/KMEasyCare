/*
创建人:郝元彦
创建日期:2016-02-22
说明:收费组合管理
*/

angular.module("sltcApp")
.controller("chargeGroupCtrl", ['$scope', 'utility', 'chargeGroupRes', 'chargeGroupDelRes', function ($scope,utility,chargeGroupRes, chargeGroupDelRes) {
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeGroupRes,//异步请求的res
            params: { groupName: "" },
            success: function (data) {//请求成功时执行函数
                $scope.chargeGroups = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.searchName = function ()
    {
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
        //$scope.init();
    };

    $scope.deleteItem = function (item) {
        chargeGroupRes.delete({ id: item.Id }, function () {
            $scope.options.search();
            utility.message("删除成功");
        });
    };

    $scope.init();
}])
.controller("chargeGroupEditCtrl", ['$scope', '$location', '$stateParams', 'utility', 'chargeGroupRes', 'chargeGroupDelRes', function ($scope, $location, $stateParams,utility, chargeGroupRes, chargeGroupDelRes) {

    $scope.updateindex = 0;

    $scope.init = function () {
        $scope.curChgItem = {};
        $scope.DeleteItem = [];
        $scope.ModifyOrCreateItem = [];
        $scope.curChgItemAdd = true;
        if ($stateParams.id) {
            $scope.isAdd = true;
            chargeGroupRes.get({ id: $stateParams.id }, function (data) {
                $scope.curItem = data.Data;
            });

        } else {
            $scope.isAdd = false;
            $scope.curItem = { GroupItems: [] };
        }
    }

    $scope.updateItem = function (item) {
        var len = $scope.DeleteItem.length,
            ids = $scope.DeleteItem.join(",");
        chargeGroupRes.save(item, function (data) {
            if (data.ResultCode == 1001) {
                utility.message("已存在相同编码或名称的套餐数据！");
            }
            else {
                if (len > 0) {
                    chargeGroupDelRes.delete({ id: ids }, function (data) {
                        $scope.DeleteItem = [];
                        utility.message("修改成功");
                        $location.url("/angular/charge/chargegroup");
                    });
                } else {
                        utility.message("修改成功");
                        $location.url("/angular/charge/chargegroup");
                }
            }
          
        });
    };

    $scope.charegeClick = function (item) {
        $scope.updateindex = item.Id;
        $("#chgGroupEditAdd span").removeClass('glyphicon-plus');
        $("#chgGroupEditAdd span").addClass('glyphicon-save');
        $scope.curChgItem = item;
        $scope.curChgItemAdd = false; 
    }

    $scope.deleteCharegeDetl = function (item, $event) {
        if ($scope.updateindex == item.Id)
        {
            $("#chgGroupEditAdd span").removeClass('glyphicon-save');
            $("#chgGroupEditAdd span").addClass('glyphicon-plus');
        }

        if (item) {
            $scope.curItem.GroupItems.splice($scope.curItem.GroupItems.indexOf(item), 1);
            if (item.Id) {
                $scope.DeleteItem.push(item.Id);
            }
        }
        $event.stopPropagation();
    }

    $scope.createItem = function (item) {
        chargeGroupRes.save(item, function (data) {
            if (data.ResultCode == 1001) {
                utility.message("已存在相同编码或名称的套餐数据！");
            }
            else {
                utility.message("添加成功");
                $location.url("/angular/charge/chargegroup");
            }

        });
    };

    $scope.cancelEdit = function () {
        $location.url("/angular/charge/chargegroup");
    };

    $scope.saveEdit = function (item) {
        if ($scope.curItem.GroupItems.length == 0)
        {
            utility.message("套餐列表不能为空！");
            return;
        }
        if (item.Id) {
            $scope.updateItem(item);
           
        } else {
            $scope.createItem(item);
        }
    };

    $scope.selectChargeItem = function (item) {
        if (item) {
            angular.extend($scope.curChgItem, {
                CostItemId: item.Id,
                ItemUnit: item.ItemUnit,
                CostItemNo: item.CostItemNo,
                CostName: item.CostName,
                Price: item.Price,
                RepeatCount: 0,
                Period: "Day"
            });
        }
    };


    $scope.saveChargeItem = function () {
        $("#chgGroupEditAdd span").removeClass('glyphicon-save');
        $("#chgGroupEditAdd span").addClass('glyphicon-plus');
        
        if (!$scope.curChgItem.Id && $scope.curChgItemAdd && $scope.curChgItem.CostItemNo) {
            $scope.curItem.GroupItems.push($scope.curChgItem);
        }
        $scope.curChgItem = {};
        $scope.curChgItemAdd = true;
    }

    $scope.init();
}]);
