///创建人:刘美方
///创建日期:2016-02-28
///说明:预收款管理
///update by zhangkai,2016-03-11,add prePaymentEditCtrl
angular.module("sltcApp")
.controller("prePaymentCtrl", ['$scope', '$state', 'dictionary', 'prePaymentRes', function ($scope, $state, dictionary, prePaymentRes) {
    var residentId = $state.params.id;
    $scope.init = function () {
        var dicType = ["PaymentType", "PaymentWay", "AssistInput"];
        dictionary.get(dicType, function (dic) {
            $scope.Dic = dic;
            for (var name in dic) {
                if ($scope.Dic.hasOwnProperty(name)) {
                    if ($scope.Dic[name][0] != undefined) {
                        $scope.Data[name] = $scope.Dic[name][0].Value;
                    }
                }
            }
            $scope.prePayments = prePaymentRes.query();
        });
        //$scope.$emit('tabChange', '.PrePayment');
    }

    $scope.displayMode = "list";
    $scope.currentItem = null;

    $scope.Keyword = "";

    $scope.deleteItem = function (item) {
        item.$delete().then(function () {
            $scope.prePayments.splice($scope.prePayments.indexOf(item), 1);
        });
        $scope.displayMode = "list";
    };

    $scope.createItem = function (item) {
        new prePaymentRes(item).$save().then(function (newItem) {
            $scope.prePayments.push(newItem);
            $scope.displayMode = "list";
        });
    };

    $scope.updateItem = function (item) {
        item.$save();
        $scope.displayMode = "list";
    };


    $scope.editOrCreate = function (item) {
        $scope.currentItem = item ? item : {};
        $scope.displayMode = "edit";
    };

    $scope.cancelEdit = function () {
        //还原
        if ($scope.currentItem && $scope.currentItem.$get) {
            $scope.currentItem.$get();
        }
        $scope.currentItem = {};

        $scope.displayMode = "list";
    };
    

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.id)) {
            $scope.updateItem(item);
        } else {
            item.ResidentId = residentId;
            //item.ResidentName = residentId;
            $scope.createItem(item);
        }
    };
    $scope.init();

}])
.controller("prePaymentEditCtrl", ['$scope', 'utility', 'prePaymentRes', 'relationDtlRes', '$state', function ($scope, utility, prePaymentRes, relationDtlRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.curUser = utility.getUserInfo();
    if (typeof ($scope.curUser) != 'undefined') {
        $scope.currentItem = { Undertaker: $scope.curUser.EmpNo };
    }

    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;

   
  

    var divwidth = $("#payordiv").width();

    $scope.autowidth = divwidth;


    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//加载当前住民的预收款记录
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { Undertaker: $scope.curUser.EmpNo };
        }

        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }




    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: prePaymentRes,//异步请求的res
            params: { FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo, OrgId: "", },
            success: function (data) {//请求成功时执行函数
                $scope.Data.PrePayments = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }

        var ss = $("#ContNamediv").width();
        $("#txtPayor").css('width', ss);
        $("#spanwidth").css("margin-left", ss - 20);
        $("#txtPayor").css('margin-left', -(ss - 20));
        $("#inputwidth").css('width', ss - 20);
    }

    //获取住民的预收款
    $scope.listItem = function (FeeNo, OrgId) {
        $scope.Data.PrePayments = {};

        //prePaymentRes.query({ currentPage: 1, pageSize: 10, FeeNo: FeeNo, OrgId: OrgId }, function (data) {
        //    $scope.Data.PrePayments = data;
        //});
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = FeeNo;
        $scope.options.params.OrgId = OrgId;
        
        $scope.options.search();

        relationDtlRes.get({ FeeNo: FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
            $scope.Data.ContactList = data.Data;
        });
    }
    //删除预收款记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的预收款记录吗?")) {
            prePaymentRes.delete({ id: item.Id }, function () {
                $scope.Data.PrePayments.splice($scope.Data.PrePayments.indexOf(item), 1);
                $scope.curUser = utility.getUserInfo();
                if (typeof ($scope.curUser) != 'undefined') {
                    $scope.currentItem = { Undertaker: $scope.curUser.EmpNo };
                }

                $scope.options.search();
                utility.message($scope.currentResident.Name + "的预收款信息删除成功！");
            });
        }
    };


    $scope.createItem = function (item) {
        //新增预收款收支记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.OrgId = $scope.currentResident.OrgId;

        prePaymentRes.save($scope.currentItem, function (data) {
            $scope.options.search();
        });
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { Undertaker: $scope.curUser.EmpNo };
        }

    };

    $scope.updateItem = function (item) {
        prePaymentRes.save(item, function (data) {
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { Undertaker: $scope.curUser.EmpNo };
            }

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
        utility.message($scope.currentResident.Name + "的预收款信息保存成功！");
    };


    //员工选择框回调函数
    $scope.staffSelected = function (item) {
        $scope.currentItem.Undertaker = item.EmpNo;
    }

    $scope.init();
}]);
