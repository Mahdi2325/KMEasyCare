/*
创建人: 吴晓波
创建日期:2017-01-10
说明:收费作业-退费
*/
angular.module("sltcApp")
.controller("RefundCtrl", ['$rootScope', '$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi',
    function ($rootScope, $scope, $http, $location, $state, utility, cloudAdminUi) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.curResident = { RegNo: 0, FeeNo: 0 };
        $scope.txtResidentIDChange = function (resident) {
            $scope.curResident = resident;
            $scope.$broadcast("residentChange", { FeeNo: $scope.curResident.FeeNo });
        }
    }
]).controller("billRefundCtrl", ['$scope', '$stateParams', 'billPaymentRes', 'billRefundRes', 'billPaymentRecRes', 'RefundRes', 'residentBalanceRes', 'residentBalanceRefundRes', 'advanceChargeRes', 'billRes', 'BillPayRes', 'chargeDetailRes', 'utility', 'relationDtlRes', '$state',
    function ($scope, $stateParams, billPaymentRes, billRefundRes, billPaymentRecRes, RefundRes, residentBalanceRes, residentBalanceRefundRes, billRes, BillPayRes, advanceChargeRes, chargeDetailRes, utility, relationDtlRes, $state) {
        if ($state.params.FeeNo != undefined && $state.params.FeeNo != '') {
            $scope.curResident.FeeNo = $state.params.FeeNo;
        }

        $scope.Data = {};
        $scope.Data.curBill = {};
        //历史缴费记录
        $scope.Data.billsV2Pay = {};
        $scope.Data.tempBillsV2Pay = {};
        //联系人列表
        $scope.Data.ContactList = {};
        $scope.disabledcheck = false;
        //列表权限Flag
        $scope.billShowFlag = true;
        $scope.billShowChargeFlag = true;
        $scope.select_billRecAll = false;
        //预缴费列表
        $scope.Data.bills = {};
        //应收金额
        $scope.SummaryAmount = 0;
        //未缴金额
        $scope.UnpaidAmount = 0;
        //实收金额
        $scope.CurAmount = 0;
        $scope.CmpCurAmount = 0;

        //自费金额
        $scope.Data.bills.SelfPay = 0;
        //个人自负
        $scope.Data.bills.NCIItemSelfPay = 0;

        var SaveCount = 0;
        //自费金额
        $scope.Data.tempBillsV2Pay.SELFPAY = 0;
        //个人自负
        $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY = 0;
        //护理险项目总费用
        $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST = 0;
        //护理险可报销费用
        $scope.Data.tempBillsV2Pay.NCIPAY = 0;

        //已收金额
        $scope.ReceivedAmount = 0;
        //系统日期
        $scope.PayBillCurTime = getNowFormatData();

        //账户可用余额
        $scope.BLANCE = 0;
        //住民账户
        $scope.Data.ResidentBalances = {};

        //获取当前日期
        function getNowFormatData() {
            var date = new Date();
            var sep = "-";
            var sep2 = ":";
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            var strdate = date.getDate();
            if (month >= 1 && month <= 9) {
                month = "0" + month;
            }
            if (strdate >= 1 && strdate <= 9) {
                strdate = "0" + strdate;
            }
            var currentdate = year + sep + month + sep + strdate;
            return currentdate;
        }

        //接受广播
        $scope.$on("residentChange", function (e, data) {
            select_billRecAll = false;
            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
            $scope.options.search();

            //获取住民账户信息
            billPaymentRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
                $scope.Data.ResidentBalances = data.Data[0];
            });

            //获取缴费人信息
            relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
                $scope.Data.ContactList = data.Data;
            });
        });

        //初始化加载缴费账单
        $scope.init = function () {
            select_billRecAll = false;
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: billPaymentRecRes,//异步请求的res
                params: { FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo, Status: 2 },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.bills = data.Data;
                    if ($scope.Data.bills.length != undefined) {
                        if ($scope.Data.bills.length == 0) {
                            $scope.SummaryAmount = 0;
                            $scope.billShowChargeFlag = false;
                            $scope.billShowFlag = false;
                        } else {
                            $scope.billShowChargeFlag = true;
                            $scope.billShowFlag = true;
                        }
                    } else {
                        $scope.SummaryAmount = 0;
                        $scope.billShowChargeFlag = false;
                        $scope.billShowFlag = false;
                    };
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }

            //获取住民账户信息
            billPaymentRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
                $scope.Data.ResidentBalances = data.Data[0];
            });

            //获取历史缴费账单
            billPaymentRecRes.get({ currentPage: 1, pageSize: 10, FeeNo: $scope.curResident.FeeNo, Status: 2 }, function (data) {
                $scope.Data.bills = data.Data;
                if ($scope.Data.bills.length != undefined) {
                    if ($scope.Data.bills.length == 0) {
                        $scope.billShowChargeFlag = false;
                        $scope.billShowFlag = false;
                    } else {
                        $scope.billShowChargeFlag = true;
                        $scope.billShowFlag = true;
                    };
                } else {
                    $scope.billShowChargeFlag = false;
                    $scope.billShowFlag = false;
                };
            });

            //获取缴费人信息
            relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
                $scope.Data.ContactList = data.Data;
            });

            //获取当前登录用户信息
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.Data.curBill.RecrodBy = $scope.curUser.EmpNo;
                $scope.Data.curBill.RecrodByName = $scope.curUser.EmpName;
            };
        };


        //多选
        $scope.m = [];
        $scope.checked = [];
        $scope.selectAll = function () {
            if ($scope.select_billRecAll) {
                $scope.checked = [];
                angular.forEach($scope.Data.bills, function (i) {
                    i.checked = false;
                    $scope.SummaryAmount = 0;
                    $scope.CurAmount = $scope.SummaryAmount;
                })
                $scope.billShowChargeFlag = false;
                $scope.select_billRecAll = false;
            } else {
                $scope.SummaryAmount = 0;
                angular.forEach($scope.Data.bills, function (i) {
                    i.checked = true;
                    $scope.checked.push(i.BillId);
                    $scope.SummaryAmount = $scope.SummaryAmount + i.SelfPay + i.NCIItemSelfPay;
                })

                $scope.billShowChargeFlag = true;
                $scope.select_billRecAll = true;
            };
        };

        $scope.SaveCount = 0;
        //确认缴费
        $scope.savePayment = function () {
            if ($scope.SummaryAmount == undefined || $scope.SummaryAmount === '' || $scope.SummaryAmount == null) {
                utility.msgwarning("退费金额不能是空白!");
                return;
            };

            if ($scope.RECEIVER == undefined || $scope.RECEIVER == '' || $scope.RECEIVER == null) {
                utility.msgwarning("收款人不能是空白!");
                return;
            };

            if ($scope.PAYMENTTYPE == undefined || $scope.PAYMENTTYPE == '' || $scope.PAYMENTTYPE == null) {
                utility.msgwarning("请选择缴费方式!");
                return;
            };

            if ($scope.REFUNDREASON == undefined || $scope.REFUNDREASON == '' || $scope.REFUNDREASON == null) {
                utility.msgwarning("退款原因不能是空白!");
                return;
            };

            if ($scope.SummaryAmount !== '' && $scope.SummaryAmount !== undefined) {
                $scope.Data.tempBillsV2Pay.SELFPAY = 0;
                $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY = 0;
                $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST = 0;
                $scope.Data.tempBillsV2Pay.NCIPAY = 0;
                angular.forEach($scope.Data.bills, function (i) {
                    if (i.checked == true) {
                        if (i.Status == 20) {
                            $scope.Data.tempBillsV2Pay.SELFPAY = 0;
                            $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY = 0;
                            $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST = 0;
                            $scope.Data.tempBillsV2Pay.NCIPAY = 0;
                            utility.msgwarning("无法退费已上传账单，账单号：" + i.BillId + "!");
                            return;
                        };

                        //自费金额
                        $scope.Data.tempBillsV2Pay.SELFPAY = $scope.Data.tempBillsV2Pay.SELFPAY + i.SelfPay;
                        //个人自负
                        $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY = $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY + i.NCIItemSelfPay;
                        //护理险项目总费用
                        $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST = $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST + i.NCIItemTotalCost;
                        //护理险可报销费用
                        $scope.Data.tempBillsV2Pay.NCIPAY = $scope.Data.tempBillsV2Pay.NCIPAY + i.NCIPay;
                    };
                });

                $scope.Data.billsV2Pay.SELFPAY = $scope.Data.tempBillsV2Pay.SELFPAY;
                $scope.Data.billsV2Pay.NCIITEMSELFPAY = $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY;
                $scope.Data.billsV2Pay.NCIITEMTOTALCOST = $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST;
                $scope.Data.billsV2Pay.NCIPAY = $scope.Data.tempBillsV2Pay.NCIPAY;
                $scope.Data.billsV2Pay.REFUNDREASON = $scope.REFUNDREASON;
                $scope.Data.billsV2Pay.REFUNDAMOUNT = $scope.SummaryAmount;
                $scope.Data.billsV2Pay.COMMENT = $scope.COMMENT;
                $scope.Data.billsV2Pay.EMPNAME = "";
                $scope.Data.billsV2Pay.OPERATOR = $scope.Data.curBill.RecrodBy;
                $scope.Data.billsV2Pay.PAYMENTTYPE = $scope.PAYMENTTYPE;
                $scope.Data.billsV2Pay.RECEIVER = $scope.RECEIVER;
                $scope.Data.billsV2Pay.FEENO = $scope.curResident.FeeNo;
                $scope.Data.billsV2Pay.NEWBILLID = null;
                $scope.Data.billsV2Pay.CREATEBY = null;
                $scope.Data.billsV2Pay.CREATETIME = null;
                $scope.Data.billsV2Pay.UPDATEBY = null;
                $scope.Data.billsV2Pay.UPDATETIME = null;
                $scope.Data.billsV2Pay.ISDELETE = false;

                //累计缴费额度=累计自费金额+累计个人自负
                $scope.Data.ResidentBalances.TotalPayment = $scope.Data.ResidentBalances.TotalPayment - ($scope.Data.billsV2Pay.SELFPAY + $scope.Data.billsV2Pay.NCIITEMSELFPAY);
                //累计产生费用额度=累计自费金额+累计护理险项目总费用
                $scope.Data.ResidentBalances.TotalCost = $scope.Data.ResidentBalances.TotalCost - ($scope.Data.billsV2Pay.SELFPAY + $scope.Data.billsV2Pay.NCIITEMTOTALCOST);
                //累计报销额度=累计护理险可报销费用
                $scope.Data.ResidentBalances.TotalNCIPay = $scope.Data.ResidentBalances.TotalNCIPay - $scope.Data.billsV2Pay.NCIPAY;
                //护理险超支金额=累计个人自负
                $scope.Data.ResidentBalances.TotalNCIOverspEnd = $scope.Data.ResidentBalances.TotalNCIOverspEnd - $scope.Data.billsV2Pay.NCIITEMSELFPAY;

                $scope.BillV2List = {};
                $scope.BillV2List.BillV2Lists = [];

                RefundRes.save($scope.Data.billsV2Pay, function (data) {
                    angular.forEach($scope.Data.bills, function (i) {
                        if (i.checked == true) {
                            i.ReFundRecordId = data.Data.REFUNDRECORDID;
                            i.Status = 8;
                            i.RefundOperator = $scope.Data.curBill.RecrodBy;
                            $scope.BillV2List.BillV2Lists.push(i);
                            SaveCount = SaveCount + 1;
                        };
                    });


                    billRefundRes.save($scope.BillV2List, function (data) {
                        if (data.ResultCode == 1001) {
                            residentBalanceRefundRes.save($scope.Data.ResidentBalances, function (data) {
                                $scope.init();
                                $scope.REFUNDREASON = '';
                                $scope.COMMENT = '';
                                $scope.SummaryAmount = 0;
                                utility.message("住民" + $scope.Data.ResidentBalances.Name + "的" + SaveCount + "个账单退费成功!");
                            });
                        };
                    });
                });


            } else {
                utility.msgwarning("请选择一个账单进行退费!");
            };
        };

        var checkCount = 0;
        //单选
        $scope.selectOne = function () {
            $scope.SummaryAmount = 0;
            checkCount = 0;
            angular.forEach($scope.Data.bills, function (i) {
                //单选初始化选中列表
                if (i.checked == true) {
                    $scope.checked.push(i.BillId);
                    $scope.SummaryAmount = $scope.SummaryAmount + i.SelfPay + i.NCIItemSelfPay;
                    checkCount = checkCount + 1;
                };

                var index = $scope.checked.indexOf(i.BillId);
                if (i.checked && index === -1) {
                    $scope.checked.push(i.BillId)
                } else if (!i.checked && index !== -1) {
                    $scope.checked.splice(index, 1);
                };
            });

            if (checkCount > 0) {
                $scope.billShowChargeFlag = true;
            } else {
                $scope.billShowChargeFlag = false;
            };


            if (checkCount == 0) {
                $scope.select_billRecAll = false;
            } else {
                //单选对比是否全选
                if ($scope.Data.bills.length == checkCount) {
                    $scope.select_billRecAll = true;
                } else {
                    $scope.select_billRecAll = false;
                };
            };

            //每次单选完清空选中列表
            $scope.checked = [];
        };


        //账单明细
        $scope.showBillsList = function (item) {
            $scope.billId = item.BillId;
            $scope.queryBillsList($scope.billId);
            $("#BillsListModal").modal("toggle");
            $scope.displayMode = "edit";
        };

        //关闭账单明细
        $scope.cancel = function () {
            $("#BillsListModal").modal("toggle");
            $scope.displayMode = "list";
        };

        //获取账单明细
        $scope.queryBillsList = function (billId) {
            billPaymentRes.get({ currentPage: 1, pageSize: 10, billid: billId }, function (data) {
                $scope.BillsDTLs = data.Data;
            });
        };

        //选择填写人员
        $scope.staffSelected = function (item) {
            $scope.Data.curBill.RecrodBy = item.EmpNo;
        };

        $scope.init();
    }
]).controller("billRefundRecCtrl", ['$scope', '$stateParams', 'billRefunRecRes', 'billRes', 'BillPayRes', 'chargeDetailRes', 'utility', 'relationDtlRes', '$state',
    function ($scope, $stateParams, billRefunRecRes, billRes, BillPayRes, chargeDetailRes, utility, relationDtlRes, $state) {
        $scope.BillRufundsDTLs = {};
        $scope.BillRufundRecsDTLs = {};
        $scope.billRefundRecs = {};

        //接受广播
        $scope.$on("residentChange", function (e, data) {
            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
            $scope.options.search();
        });

        //初始化加载历史缴费账单
        $scope.init = function () {
            //获取退费记录
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: billRefunRecRes,//异步请求的res
                params: {
                    FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo, Status: 8
                },
                success: function (data) {//请求成功时执行函数
                    $scope.billRefundRecs = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            };

            billRefunRecRes.get({
                currentPage: 1, pageSize: 10, FeeNo: $scope.curResident.FeeNo, Status: 8
            }, function (data) {
                $scope.billRefundRecs = data.Data;
            });
        };

        //账单明细
        $scope.showBillRefundsList = function (item) {
            $scope.queryBillsList(item.BillId);
            $scope.queryBillRecsList(item.ReFundRecordId);
            $("#BillRecsListModal").modal("toggle");
            $scope.displayMode = "edit";
        };

        //关闭账单明细
        $scope.cancel = function () {
            $("#BillRecsListModal").modal("toggle");
            $scope.displayMode = "list";
        }

        //获取账单明细
        $scope.queryBillsList = function (billId) {
            billRefunRecRes.get({ currentPage: 1, pageSize: 10, billid: billId }, function (data) {
                $scope.BillRufundsDTLs = data.Data;
            });
        };

        //账单缴费记录
        $scope.showBillRefundRecsList = function (item) {
            $scope.queryBillRecsList(item.BILLPAYID);
            $("#ChargeRecListModal").modal("toggle");
            $scope.displayMode = "edit";
        };

        //关闭账单缴费记录
        $scope.close = function () {
            $("#ChargeRecListModal").modal("toggle");
            $scope.displayMode = "list";
        }

        //获取账单缴费记录
        $scope.queryBillRecsList = function (RefundRecordid) {
            billRefunRecRes.get({ currentPage: 1, pageSize: 10, RefundRecordId: RefundRecordid }, function (data) {
                $scope.BillRufundRecsDTLs = data.Data[0];
                $scope.GetPAYMENTTYPE($scope.BillRufundRecsDTLs.PAYMENTTYPE);
            });
        };

        //缴费方式状态转换
        $scope.GetPAYMENTTYPE = function (PAYMENTTYPE) {
            if (PAYMENTTYPE == "001") {
                $scope.RefundPaymentType = "现金";
            } else if (PAYMENTTYPE == "002") {
                $scope.RefundPaymentType = "刷卡";
            } else if (PAYMENTTYPE == "003") {
                $scope.RefundPaymentType = "支票";
            } else if (PAYMENTTYPE == "004") {
                $scope.RefundPaymentType = "汇款";
            }
            else {
                $scope.RefundPaymentType = "";
            };
        };

        $scope.init();
    }
]).controller("advanceChargeRefundCtrl", ['$scope', '$stateParams', 'advanceChargeRes', 'advanceChargeRefundRes', 'residentBalanceRefundRes', 'billRes', 'BillPayRes', 'chargeDetailRes', 'utility', 'relationDtlRes', '$state',
    function ($scope, $stateParams, advanceChargeRes, advanceChargeRefundRes, residentBalanceRefundRes, billRes, BillPayRes, chargeDetailRes, utility, relationDtlRes, $state) {
        $scope.Data = {};
        $scope.Pay = {};
        $scope.Data.curBill = {};
        $scope.disabledcheck = false;
        $scope.BillKey = {};
        $scope.Data.preRefundbills = {};
        $scope.Data.ResidentBalances = {};
        $scope.Data.ContactList = {};


        //接受广播
        $scope.$on("residentChange", function (e, data) {
            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
            $scope.options.search();

            //获取缴费人信息
            relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
                $scope.Data.ContactList = data.Data;
            });

            //获取住民账户信息
            advanceChargeRefundRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
                $scope.Data.ResidentBalances = data.Data[0];
                if ($scope.Data.ResidentBalances != undefined) {
                    $scope.GetStatus($scope.Data.ResidentBalances.Status, $scope.Data.ResidentBalances.IsHaveNCI);
                };
            });
        });

        //初始化数据
        $scope.init = function () {
            //获取预收款操作
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: advanceChargeRefundRes,//异步请求的res
                params: {
                    FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo
                },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.preRefundbills = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            };

            //获取缴费人信息
            relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
                $scope.Data.ContactList = data.Data;
            });

            //获取住民账户信息
            advanceChargeRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
                $scope.Data.ResidentBalances = data.Data[0];
                if ($scope.Data.ResidentBalances != undefined) {
                    $scope.GetStatus($scope.Data.ResidentBalances.Status, $scope.Data.ResidentBalances.IsHaveNCI);
                };
            });

            //获取当前登录用户信息
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.Data.curBill.RecrodBy = $scope.curUser.EmpNo;
                $scope.Data.curBill.RecrodByName = $scope.curUser.EmpName;
            };
        };

        //选择填写人员
        $scope.staffSelected = function (item) {
            $scope.Data.curBill.RecrodBy = item.EmpNo;
        }

        //确定退款
        $scope.saveEdit = function (ResidentBalances, Refund, RecrodBy) {
            if (Refund != undefined) {
                if (Refund.REFUNDAMOUNT == undefined || Refund.REFUNDAMOUNT == '' || Refund.REFUNDAMOUNT == null) {
                    utility.msgwarning("退款金额不能是空白!");
                    return;
                };

                if (Refund.RECEIVER == undefined || Refund.RECEIVER == '' || Refund.RECEIVER == null) {
                    utility.msgwarning("收款人不能是空白!");
                    return;
                };

                if (Refund.PAYMENTTYPE == undefined || Refund.PAYMENTTYPE == '' || Refund.PAYMENTTYPE == null) {
                    utility.msgwarning("请选择缴费方式!");
                    return;
                };

                if (Refund.REASON == undefined || Refund.REASON == '' || Refund.REASON == null) {
                    utility.msgwarning("退款原因不能是空白!");
                    return;
                };
            } else {
                utility.msgwarning("必填项不能是空白!");
                return;
            };

            if (ResidentBalances.Status == 0) {
                if (Refund.REFUNDAMOUNT <= 0) {
                    utility.msgwarning("退款金额必须大于零");
                } else {
                    if (ResidentBalances.BalanceID != undefined && ResidentBalances.BalanceID != null && !isEmpty(ResidentBalances.BalanceID)) {
                        $scope.Refund.BALANCEID = ResidentBalances.BalanceID;
                        $scope.Refund.OPERATOR = RecrodBy;
                        ResidentBalances.RefundAmount = Refund.REFUNDAMOUNT;
                        advanceChargeRefundRes.save(Refund, function (data) {
                            residentBalanceRefundRes.save(ResidentBalances, function (data) {
                                $scope.init();
                                $scope.clearPaydetail();
                                utility.message(ResidentBalances.Name + "退款成功!");
                            });
                        });
                    } else {
                        utility.msgwarning("没有住民账户，请尝试选择一个住民");
                    };
                };
            } else {
                utility.msgwarning("住民账户处于停用状态，无法进行退款");
            };
        };


        //检查金额
        $scope.checkRefundAmount = function () {
            if ($scope.Refund.REFUNDAMOUNT > $scope.Data.ResidentBalances.Blance) {
                $scope.Refund.REFUNDAMOUNT = '';
                utility.msgwarning("退款金额不得大于账户余额!");
                return;
            };
        };

        //修改住民账户状态和是否享受护理险
        $scope.GetStatus = function (STATUS, ISHAVENCI) {
            if (STATUS == 0) {
                $scope.BSTATUS = "正常";
            } else {
                $scope.BSTATUS = "停用";
            };

            $scope.BISHAVENCI = ISHAVENCI ? "是" : "否";
        };

        //保存成功，清空退款金额、原因
        $scope.clearPaydetail = function () {
            $scope.Refund.REFUNDAMOUNT = "";
            $scope.Refund.REASON = "";
        };

        $scope.init();
    }]);
