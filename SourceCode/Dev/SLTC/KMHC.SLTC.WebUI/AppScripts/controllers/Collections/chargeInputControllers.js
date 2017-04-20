/*
创建人: 张正泉
创建日期:2016-02-20
说明:费用录入
*/

angular.module("sltcApp")
.controller("chargeInputCtrl", ['$scope', '$filter', '$stateParams', 'chargeDetailRes', 'utility', '$state', function ($scope, $filter, $stateParams, chargeDetailRes, utility, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.loadPage = function () {
        $scope.curResident = $scope.curResident || {};
        //初始化一些常用参数
        $scope.charge = { OccurTime: $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss"), Quantity: 1, Price: 0 };
    };

    //$scope.search = $scope.reload = function (FeeNo) {
    //    FeeNo = FeeNo || $scope.FeeNo;
    //    if (FeeNo) {
    //        chargeDetailRes.get({ FeeNo: FeeNo }, function (data) {
    //            $scope.charges = data.Data;
    //        });
    //    }
    //};


    $scope.init = function () {
        $scope.charge = {};
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeDetailRes,//异步请求的res
            params: { FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo },
            success: function (data) {//请求成功时执行函数
                $scope.charges = data.Data;

            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.$watch("charge.Price", function () {
        var charge = $scope.charge;
        charge.TotalPrice = charge.Quantity * charge.Price;
    });

    $scope.$watch("charge.Quantity", function () {
        var charge = $scope.charge;
        charge.TotalPrice = charge.Quantity * charge.Price;
    });

    $scope.submit = function () {
        if (typeof ($scope.charge.BillId) != "undefined") {
            utility.message("当前收费项目已生成账单！");
            return;
        }

        if ($scope.curResident.FeeNo && $scope.charge.TotalPrice) {
            $scope.charge.FeeNo = $scope.curResident.FeeNo;
            chargeDetailRes.save($scope.charge, function (data) {
                if (!$scope.charge.Id) {
                    $scope.charges.push(data.Data);
                } else {
                    for (var i = $scope.charges.length - 1; i > -1; i--) {
                        if ($scope.charges[i].Id == data.Data.Id) {
                            $scope.charges[i] = data.Data;
                        }
                    }
                }
                $scope.options.search();
                $scope.loadPage();
                utility.message("费用录入成功");
            });
        }
    };


    $scope.txtResidentIDChange = function (resident) {
        $scope.curResident = resident;

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = resident.FeeNo;
        $scope.options.search();
        $scope.loadPage();
    }


    $scope.selectChargeItem = function (data) {
        angular.extend($scope.charge, {
            CostItemNo: data.CostItemNo,
            CostName: data.CostName,
            Price: data.Price,
            Unit: data.unit,
            ItemType: data.ItemType,
            BillId: data.BillId
        });
    };

    $scope.rowDbClick = function (charge) {
        $scope.charge = angular.copy(charge);
    }

    $scope.delete = function (item) {
        if (confirm("确定删除该费用信息吗?")) {
            chargeDetailRes.delete({ id: item.Id }, function (data) {
                $scope.charges.splice($scope.charges.indexOf(item), 1);
                $scope.options.search();
                $scope.loadPage();
                utility.message("删除成功");
            });
        }
    };

    $scope.init();
}])
.controller("billListCtrl", ['$scope', '$stateParams', 'chargeDetailRes', 'utility', 'billRes', '$state', function ($scope, $stateParams, chargeDetailRes, utility, billRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    //function search(FeeNo, OrgId) {
    //    billRes.query({ currentPage: 1, pageSize: 10, FeeNo: FeeNo, OrgId: OrgId }, function (data) {
    //        $scope.Data.bills = data;
    //    });
    //}

    $scope.init = function () {
        $scope.Data.bills = {};
        $scope.curItem = {};
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: billRes,//异步请求的res
            params: { FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo, OrgId: "", },
            success: function (data) {//请求成功时执行函数
                $scope.Data.bills = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.search = $scope.reload = function () {
        if (angular.isDefined($scope.curResident.FeeNo) && angular.isDefined($scope.curResident.OrgId)) {
            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
            $scope.options.params.OrgId = $scope.curResident.OrgId;
            $scope.options.search();
            //search($scope.curResident.FeeNo, $scope.curResident.OrgId);
        }
    };
    $scope.Data = {};
    // 当前住民
    $scope.curResident = {}
    $scope.detailClick = function (billId) {
        if (billId) {
            chargeDetailRes.get({ currentPage: 1, pageSize: 100, BillId: billId, FeeNo: $scope.curResident.FeeNo, OrgId: $scope.curResident.OrgId }, function (data) {
                if (data.Data.length > 0) {
                    $scope.Data.BillDetails = data.Data;
                } else {
                    $scope.Data.BillDetails = [];
                }
            });
        }
    };

    $scope.txtResidentIDChange = function (resident) {
        $scope.curResident = resident;//获取当前住民信息

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = $scope.curResident.FeeNo;
        $scope.options.params.OrgId = $scope.curResident.OrgId;
        $scope.options.search();
    }

    $scope.init();

}])
.controller("payBillsCtrl", ['$scope', '$stateParams', 'billRes', 'BillPayRes', 'chargeDetailRes', 'utility', 'relationDtlRes', '$state', function ($scope, $stateParams, billRes, BillPayRes, chargeDetailRes, utility, relationDtlRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.Pay = {};
    $scope.Data.curBill = {};
    // 当前住民
    $scope.curResident = {}

    $scope.disabledcheck = false;
    $scope.txtResidentIDChange = function (resident) {
        $scope.curResident = resident;//获取当前住民信息
        $scope.options.pageInfo.pageSize = 10;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.params.FeeNo = $scope.curResident.FeeNo;
        $scope.options.params.OrgId = $scope.curResident.OrgId;

        $scope.options.search();

        relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
            $scope.Data.ContactList = data.Data;
        });
        //$scope.initBill($scope.curResident.FeeNo, $scope.curResident.OrgId);//加载当前住民的账单记录
    }

    $scope.init = function () {
        $scope.BillKey = {};
        $scope.Data.bills = {};

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: billRes,//异步请求的res
            params: { FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo, OrgId: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.bills = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }


    //$scope.initBill = function (FeeNo, OrgId) {
    //    $scope.BillKey = {};
    //    billRes.query({ currentPage: 1, pageSize: 10, FeeNo: FeeNo, OrgId: OrgId }, function (data) {
    //        $scope.Data.bills = data;
    //    });
    //}

    $scope.searchPay = function (BillId) {
        $scope.Pay = {};
        if (BillId) {
            BillPayRes.get({ currentPage: 1, pageSize: 10, BillId: BillId }, function (data) {
                if (data.Data && data.Data.length > 0) {
                    $scope.Pay.billPayHis = data.Data;
                } else {
                    $scope.Pay.billPayHis = [];
                }
            });
        }$scope.Data.curBill.RecrodBy
    }

    $scope.GenerateBill = function () {
        billRes.get({ FeeNo: $scope.curResident.FeeNo, OrgId: $scope.curResident.OrgId }, function (data) {
            if (data.ResultCode == 1002) {
                utility.msgwarning("当前个人缴费账单已是最新数据！");
            }
            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
            $scope.options.params.OrgId = $scope.curResident.OrgId;
            $scope.options.pageInfo.pageSize = 10;
            $scope.options.pageInfo.CurrentPage = 1;

            $scope.options.search();
        });


        // $scope.initBill($scope.curResident.FeeNo, $scope.curResident.OrgId);
    }

    $scope.needPayDetailClick = function (bill) {

        $scope.Data.curBill.BillId = bill.Id;
        $scope.Data.curBill.Summary = bill.Cost;
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.Data.curBill.RecrodBy = $scope.curUser.EmpNo;
            $scope.Data.curBill.RecrodByName = $scope.curUser.EmpName;
        }


        if (bill.BillState == "Close") {
            $("#disabledcheck").attr('disabled', true);
            $("#disabledcheck").text("缴费已完成");
        }
        else {
            $("#disabledcheck").text("确认缴费");
        }


        if (bill.Id) {
            BillPayRes.get({ key: bill.Id }, function (data) {
                if (data) {
                    $scope.Data.curBill.Received = data.Data;
                    $scope.Data.curBill.Balance = $scope.Data.curBill.Summary - $scope.Data.curBill.Received;
                } else {
                    $scope.Data.curBill.Received = 0;
                    $scope.Data.curBill.Balance = bill.Cost;
                }
            });

            chargeDetailRes.get({ currentPage: 1, pageSize: 10, BillId: bill.Id }, function (data) {
                if (data.Data.length > 0) {
                    $scope.Data.curBill.BillDetail = data.Data;
                } else {
                    $scope.Data.curBill.BillDetail = [];
                }
            });
        }
    };

    //选择填写人员
    $scope.staffByInSelected = function (item) {
        $scope.Data.curBill.RecrodBy = item.EmpNo;
    }

    $scope.checkBill = function () {
        if ($scope.Data.curBill.Cost > $scope.Data.curBill.Balance) {
            utility.msgwarning("当次收取金额不能大于未收余额！");
            $scope.Data.curBill.Cost = "";
            return;
        }
    }

    $scope.submitPay = function () {
        if ($scope.Data.curBill.Cost > 0 && $scope.Data.curBill.BillId) {

            $scope.Data.curBill.Received += $scope.Data.curBill.Cost;
            $scope.Data.curBill.Balance = $scope.Data.curBill.Summary - $scope.Data.curBill.Received;
            if ($scope.Data.curBill.Balance == 0) {
                $scope.Data.curBill.BillStatus = "003";
            } else {
                $scope.Data.curBill.BillStatus = "002";
            }
            console.log($scope.Data.curBill);
            BillPayRes.save($scope.Data.curBill, function () {
                utility.message($scope.curResident.Name + "的缴费信息保存成功！");
                $scope.clearPaydetail();
                $scope.options.search();
            });
        }
    }

    $scope.deleteBill = function (Id) {
        if (confirm("确定删除该条账单数据吗?")) {
            billRes.delete({ id: Id }, function (data) {
                utility.message(data.ResultMessage);
                $scope.options.params.FeeNo = $scope.curResident.FeeNo;
                $scope.options.params.OrgId = $scope.curResident.OrgId;
                $scope.options.pageInfo.pageSize = 10;
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
            });
        }
    };



    $scope.clearPaydetail = function () {
        $scope.Data.curBill.PayBillTime = "";
        $scope.Data.curBill.RecrodBy = "";
        $scope.Data.curBill.AccountantCode = "";
        $scope.Data.curBill.Payor = "";
        $scope.Data.curBill.Cost = "";
        $scope.Data.curBill.InvoiceNo = "";
    };


    $scope.init();
}]);
