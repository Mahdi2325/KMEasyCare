/*
创建人:张凯
创建日期:2016-03-10
说明: 零用金
*/
angular.module("sltcApp")
.controller("pinMoneyCtrl", ['$scope', 'dictionary', 'utility', 'pinMoneyRes', '$state', function ($scope, dictionary, utility, pinMoneyRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.currentItem = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: pinMoneyRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.PinMoneys = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                OrgId: -1,
            }
        }
    }

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息

        $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//加载当前住民的零用金记录
        $scope.currentItem = {}//清空编辑项
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }
    //获取住民的零用金
    $scope.listItem = function (FeeNo, OrgId) {

        $scope.Data.PinMoneys = {};
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = FeeNo;
        $scope.options.params.OrgId = OrgId;
        $scope.options.search();
        //pinMoneyRes.query({ currentPage: 1, pageSize: 10, FeeNo: FeeNo ,OrgId: OrgId}, function (data) {
        //    $scope.Data.PinMoneys = data;
        //});
    }

    //删除零用金记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的零用金记录吗?")) {
            pinMoneyRes.delete({ id: item.Id }, function () {
                $scope.Data.PinMoneys.splice($scope.Data.PinMoneys.indexOf(item), 1);
                $scope.currentItem = {};//清空编辑项
                $scope.options.search();
                utility.message($scope.currentResident.Name + "的零用金信息删除成功！");
            });
        }
    };


    $scope.createItem = function (item) {
        //新增零用金收支记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.OrgId = $scope.currentResident.OrgId;
        pinMoneyRes.save($scope.currentItem, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.search();
        });
        $scope.currentItem = {};
    };

    $scope.updateItem = function (item) {
        pinMoneyRes.save(item, function (data) {
            $scope.currentItem = {};
        });
    };


    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的零用金信息保存成功！");
    };

    $scope.init();
}]);
