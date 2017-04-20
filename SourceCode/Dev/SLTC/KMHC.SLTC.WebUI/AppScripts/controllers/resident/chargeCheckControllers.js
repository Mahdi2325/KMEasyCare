
angular.module("sltcApp")
.controller("chargeCheckCtrl", ['$scope', '$state', 'utility', 'chargeCheckRes', function ($scope, $state, utility, chargeCheckRes) {
    $scope.FeeNo = $state.params.FeeNo;
    //var id = $state.params.id;
    $scope.buttonShow = false;
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentItem = {};
        $scope.currentResident = resident;//获取当前住民信息
        $scope.initChargeCheck();//加载当前住民的入住审核
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }
    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: chargeCheckRes,//异步请求的res
        params: { feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo, orgId: 0 },
        success: function (data) {//请求成功时执行函数
            $scope.Datas = data.Data;
            if ($scope.Datas.length > 0) {
                //$scope.rowSelect(data.Data[0]);
            }
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }

    //加载入住审核
    $scope.initChargeCheck = function () {
        if ($scope.currentResident) {
            $scope.options.params.feeNo = $scope.currentResident.FeeNo;
            $scope.options.params.orgId = $scope.currentResident.OrgId;
            $scope.options.search();
        }
    }

    $scope.deleteItem = function (item) {
        if (confirm("确定删除该信息吗?")) {
            chargeCheckRes.delete({ id: item.Id }, function (data) {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                utility.message("删除成功");
            });
        }
    };

    $scope.createItem = function (item) {
        //新增需求管理记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.OrgId = $scope.currentResident.OrgId;
        chargeCheckRes.save($scope.currentItem, function (data) {
            $scope.options.search();
        });
        $scope.currentItem = {};
    };

    $scope.updateItem = function (item) {
        chargeCheckRes.save($scope.currentItem, function (data) {
            $scope.options.search();
        });
        $scope.currentItem = {};
    };


    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    };

    $scope.saveEdit = function (item) {
        if (!$scope.Validation()) { return; }
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的辅助信息保存成功！");
    };
    $scope.Validation = function () {
        var errorTips = 0;
        if (angular.isDefined($scope.myForm.$error.required)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.required.length; i++) {
                msg = $scope.myForm.$error.required[i].$name + " 为必填项";
                utility.msgwarning(msg);
                errorTips++;
                if (errorTips >= $scope.maxErrorTips) {
                    return false;
                }
            }
        }

        if (angular.isDefined($scope.myForm.$error.maxlength)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.maxlength.length; i++) {
                msg = $scope.myForm.$error.maxlength[i].$name + "超过设定长度 ";
                utility.msgwarning(msg);
                errorTips++;
                if (errorTips >= $scope.maxErrorTips) {
                    return false;
                }
            }
        }
        if (angular.isDefined($scope.myForm.$error.pattern)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.pattern.length; i++) {
                msg = $scope.myForm.$error.pattern[i].$name + "格式输入不对 ";
                utility.msgwarning(msg);
                errorTips++;
                if (errorTips >= $scope.maxErrorTips) {
                    return false;
                }
            }
        }
        if (errorTips > 0) { return false; }
        return true;
    }



}]);
