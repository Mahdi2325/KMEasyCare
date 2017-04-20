//创建人：张祥
//创建日期：2016-03-29
//预约登记入住
angular.module("sltcApp").controller('PreipdCtrl', ['$scope', '$state', 'dictionary', 'PreipdRes', 'deptRes', 'utility', function ($scope, $state, dictionary, PreipdRes, deptRes, utility) {
    //显示列表页
    $scope.displayMode = "list";
    //初始化Data
    $scope.Data = {}
    //初始化编辑Ietm
    $scope.currentItem = {};
    //
    $scope.Info = {};
    $scope.TextShow = false;
    //
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: PreipdRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.priepedList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };
        deptRes.get({}, function (data) {
            $scope.Info.Depts = data.Data;
        });
    };

    $scope.change = function (x) {
        if (x == "D") {
            $scope.TextShow = true;
        }
        else {
            $scope.TextShow = false;
        }
    };
    //$scope.keyup = function (x) {
    //    //var l = x.length;
    //    //if (l == 1) {
    //    //    var regx = /^[A-Za-z]/;
    //    //    var rs = regx.exec(x);
    //    //    if (rs == null) {
    //    //        $scope.currentItem.IdNo = "";
    //    //    }
    //    //}
    //    //else if (l > 1 && l <= 10) {

    //    //    var c = x.substring(l - 1)
    //    //    var regx = /[0-9]/;
    //    //    var rs = regx.exec(c);
    //    //    if (rs == null) {
    //    //        $scope.currentItem.IdNo = x.substring(0, l - 1)
    //    //    }

    //    //}
    //    //else {
    //    //    $scope.currentItem.IdNo = x.substring(0, 10)
    //    //}
    //};

    function checkOut(x) {
        return cardVerif(x)
    }
    //新建资料编辑页
    $scope.CreatePreipd = function () {
        $scope.recStatus = true;
        $scope.TextShow = false;
        $scope.displayMode = "edit";
    };

    //编辑修改
    $scope.rowSelect = function (item) {
        $scope.recStatus = false;
        if (item.RecStatus == "D") {
            $scope.TextShow = true;
        }
        else {
            $scope.TextShow = false;
        }
        $scope.currentItem = item;
        $scope.displayMode = "edit";
    };

    //编辑保存
    $scope.savePreipd = function (item) {
        if (angular.isDefined(item.PreFeeNo)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        $scope.displayMode = "list";
    };

    //修改保存
    $scope.updateItem = function (item) {
        //item.$save();
        PreipdRes.save($scope.currentItem, function (data) {
            $scope.options.search();
            utility.message("保存成功！");
        });
        $scope.currentItem = {};
        $scope.displayMode = "edit";
    };

    //创建保存
    $scope.createItem = function (item) {
        $scope.currentItem.RecStatus = "P";
        PreipdRes.save($scope.currentItem, function (data) {
            $scope.options.search();
            utility.message("保存成功！");
        });
        $scope.currentItem = {};
        $scope.displayMode = "edit";
    };

    //删除
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该预约登记信息吗?")) {
            PreipdRes.delete({ recId: item.PreFeeNo }, (function () {
                $scope.options.search();
                //$scope.Data.priepedList.splice($scope.Data.priepedList.indexOf(item), 1);
            }));
        }
    };

    //取消编辑返回列表页
    $scope.cancelPreipd = function () {
        //if ($scope.currentItem && $scope.currentItem.$get) {
        //    $scope.currentItem.$get();
        //}
        $scope.options.search();
        $scope.currentItem = {};
        $scope.displayMode = "list";
    };

    $scope.customValidity = function (validity, name) {
        var b = true;
        switch (name) {
            case "ContactName":
                b = $scope.currentItem.PName === $scope.currentItem.ContactName;
                break;
            case "ContactTel":
                b = $scope.currentItem.Phone === $scope.currentItem.ContactTel;
                break;
        }
        validity(name, b);
    };

    $scope.init();
}]);
