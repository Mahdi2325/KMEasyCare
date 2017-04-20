/*
创建人: 郝元彦
创建日期:2016-02-20
说明:收费项目管理
*/

angular.module("sltcApp")
.controller("chargeItemCtrl", ['$scope', 'utility', 'chargeItemRes', function ($scope,utility, chargeItemRes) {
    $scope.init = function () {
        $scope.Keyword = "";
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeItemRes,//异步请求的res
            params: { keyWords: "" },
            success: function (data) {//请求成功时执行函数
                $scope.chargeItems = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.searchInfo = function ()
    {
        if ($scope.options.params.keyWords.length > 50)
        {
            $scope.options.params.keyWords = "";
            utility.message("搜索长度过长,请重新输入!");
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    }

    $scope.deleteItem = function (item) {
        chargeItemRes.delete({ id: item.Id }, function () {
            $scope.chargeItems.splice($scope.chargeItems.indexOf(item), 1);
            $scope.options.search();
            utility.message("删除成功");
        });

    };

    $scope.init();
}])
.controller("chargeItemEditCtrl", ['$scope', '$location', '$stateParams', 'utility', 'chargeItemRes', function ($scope, $location, $stateParams, utility, chargeItemRes) {
    $scope.CostItemNo = "null";

    $scope.init = function () {
        if ($stateParams.id) {
            $scope.isAdd = true;
            chargeItemRes.get({ id: $stateParams.id }, function (data) {
                $scope.curItem = data.Data;
                $scope.CostItemNo = data.CostItemNo;
            });

        } else {
            $scope.isAdd = false;
            $scope.curItem = {};
        }
    }

    $scope.createItem = function (item) {
        chargeItemRes.save(item, function (newItem) {
            utility.message("添加成功");
            $location.url("/angular/charge/chargeitem");
        });
    };

    $scope.updateItem = function (item) {
        chargeItemRes.save(item, function () {
            utility.message("修改成功");
            $location.url("/angular/charge/chargeitem");
        });
    };

    $scope.cancelEdit = function () {
        $location.url("/angular/charge/chargeitem");
    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
    };
    $scope.init();
}]);
