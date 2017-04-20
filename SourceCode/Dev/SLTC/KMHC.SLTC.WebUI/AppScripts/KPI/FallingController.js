/*
创建人:张祥
创建日期:2016-03-15
说明: 跌倒
*/
angular.module("sltcApp").controller("fallingCtrl", ['$scope', '$state', 'dictionary', 'webUploader', 'fallRes', 'utility', function ($scope, $state, dictionary, webUploader, fallRes, utility) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.curUser = utility.getUserInfo();
    $scope.curUser = utility.getUserInfo();
    if (typeof ($scope.curUser) != 'undefined') {
        $scope.currentItem = { RecordBy: $scope.curUser.EmpNo, SettleBy: $scope.curUser.EmpNo };
    }
    // 当前住民
    $scope.currentResident = {};
    $scope.buttonShow = false;
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: fallRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.fallList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: -1
            }
        }

        webUploader.init('#EventPathPickerOne', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.currentItem.Pict1 = data[0].SavedLocation;
                $scope.$apply();
            }
        });
        webUploader.init('#EventPathPickerTwo', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.currentItem.Pict2 = data[0].SavedLocation;
                $scope.$apply();
            }
        });
    }
    $scope.clear = function (type) {
        switch (type) {
            case "EventOnePath":
                $scope.currentItem.Pict1 = "";
                break;
            case "EventTwoPath":
                $scope.currentItem.Pict2 = "";
                break;
        }
    }

    //选中住民
    $scope.residentSelected = function (resident) {



        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo);//加载当前住民的跌倒记录
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { RecordBy: $scope.curUser.EmpNo, SettleBy: $scope.curUser.EmpNo };
        }
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    $scope.staffSelected = function (item, type) {
        if (type === "RecordBy") {
            $scope.currentItem.RecordBy = item.EmpNo;
            $scope.currentItem.RecordNameBy = item.EmpName;
        } else if (type === "SettleBy") {
            $scope.currentItem.SettleBy = item.EmpNo;
            $scope.currentItem.SettleNameBy = item.EmpName;
        }
    }

    $scope.listItem = function (FeeNo) {
        $scope.Data.fallList = {};
        $scope.options.params.feeNo = FeeNo;
        $scope.options.search();
        //$scope.options.pageInfo.CurrentPage = 1;
        //$scope.options.pageInfo.PageSize = 10;
        //fallRes.get({ CurrentPage: 1, PageSize: 10, feeNo: FeeNo }, function (data) {
        //    $scope.Data.fallList = data.Data;
        //    $scope.options.sumInfo = { RecordsCount: data.RecordsCount, PagesCount: data.PagesCount };
        //    $scope.options.renderPage = $scope.options.pageInfo.CurrentPage;
        //});

    }

    //删除跌倒记录
    $scope.deleteItem = function (item)
    {
        if (confirm("您确定要删除该住民的跌倒记录吗?")) {
            fallRes.delete({ id: item.ID }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
               // $scope.Data.fallList.splice($scope.Data.fallList.indexOf(item), 1);
            });
        }
    };

    $scope.saveFallEdit = function (item) {
        if (!angular.isDefined($scope.currentItem.RecordBy)) {
            utility.msgwarning("在场人员为必填项！");
            return;
        }

        if (!angular.isDefined($scope.currentItem.SettleBy)) {
            utility.msgwarning("处理人员为必填项！");
            return;
        }

        if (angular.isDefined($scope.FallForm.$error.required)) {
            for (var i = 0; i < $scope.FallForm.$error.required.length; i++) {
                utility.msgwarning($scope.FallForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.FallForm.$error.maxlength)) {
            for (var i = 0; i < $scope.FallForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.FallForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined(item.ID)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的跌倒信息保存成功！");
    };

    $scope.createItem = function (item) {
        //新增跌倒记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;//住院序号
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;//病例号
        fallRes.save($scope.currentItem, function (data) {
            $scope.options.search();
            //$scope.Data.fallList.push(data);
        });
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { RecordBy: $scope.curUser.EmpNo, SettleBy: $scope.curUser.EmpNo };
        }
    };

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    };

    $scope.updateItem = function (item) {
        fallRes.save(item, function (data) {
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { RecordBy: $scope.curUser.EmpNo, SettleBy: $scope.curUser.EmpNo };
            }
        });
    };
    $scope.init();

}]);





