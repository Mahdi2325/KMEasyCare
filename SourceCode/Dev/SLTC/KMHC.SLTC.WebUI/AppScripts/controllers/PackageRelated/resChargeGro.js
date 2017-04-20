angular.module("sltcApp")
.controller("ResChargeGroCtrl", ['$scope', '$state', 'resChargeGroRes', 'utility', function ($scope, $state, resChargeGroRes, utility) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: resChargeGroRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.RcgList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: -1
            }
        }
    };

    $scope.listItem = function (residentId) {
        $scope.Data.RcgList = {};
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.params.feeNo = residentId;
        $scope.options.search();
    }

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo);
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { ExecuteBy: $scope.curUser.EmpNo, ExecutorName: $scope.curUser.EmpName };
        }
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    $scope.deleteItem = function (item) {
        if (confirm("确定删除该信息吗?")) {
            resChargeGroRes.delete({ id: item.CGRID }, function (data) {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                utility.message("删除成功");
            });
        }
    };

    $scope.createItem = function (item) {
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.RegName = $scope.currentResident.Name;
        $scope.currentItem.OrgId = $scope.currentResident.OrgId;
        $scope.currentItem.CHARGEGROUPID = item.CHARGEGROUP.id;
        resChargeGroRes.save($scope.currentItem, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
            $scope.currentItem = {};
        });
    };

    $scope.updateItem = function (item) {
        resChargeGroRes.save($scope.currentItem, function (data) {
            $scope.currentItem = {};
        });
    };


    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
        $scope.currentItem.CHARGEGROUP = { id: item.CHARGEGROUPID, name: item.CHARGEGROUPNAME };

    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined($scope.ReqForm.$error.required)) {
            for (var i = 0; i < $scope.ReqForm.$error.required.length; i++) {
                utility.msgwarning($scope.ReqForm.$error.required[i].$name + "为必填项！");
            }
            return;
        }

        if (angular.isDefined($scope.ReqForm.$error.maxlength)) {
            for (var i = 0; i < $scope.ReqForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.ReqForm.$error.maxlength[i].$name + "超过设定长度！");
            }
            return;
        }
        if (angular.isDefined($scope.ReqForm.$error.pattern)) {
            for (var i = 0; i < $scope.ReqForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.ReqForm.$error.pattern[i].$name + "格式错误！");
            }
            return;
        }
        //起始和结束时间已拿掉
        //if (item.OVERALLBEGINDATE > item.OVERALLENDDATE) {
        //    utility.msgwarning("套餐起始时间不能大于套餐到期时间！");
        //    return;
        //}
        if (_.find($scope.Data.RcgList, { CHARGEGROUPID: item.CHARGEGROUP.id })) {
            utility.msgwarning("已经存在该套餐！");
            return;
        }
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的套餐信息保存成功！");
    };

    $scope.init();
}]);