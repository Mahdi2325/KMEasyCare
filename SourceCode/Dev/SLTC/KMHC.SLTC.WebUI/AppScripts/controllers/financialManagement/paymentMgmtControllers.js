/*
创建人: 吴晓波
创建日期:2017-01-10
说明:收费作业-缴费
*/
angular.module("sltcApp")
.controller("PaymentCtrl", ['$rootScope', '$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi',
    function ($rootScope, $scope, $http, $location, $state, utility, cloudAdminUi) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.curResident = { RegNo: 0, FeeNo: 0 };
        $scope.txtResidentIDChange = function (resident) {
            $scope.curResident = resident;
            $scope.$broadcast("residentChange", { FeeNo: $scope.curResident.FeeNo });
        }
    }
]).controller("billPaymentCtrl", ['$scope', '$stateParams', 'billPaymentRes', 'rsChargeGroupRes', 'billV2PayRes', 'residentBalanceRes', 'advanceChargeRes', 'billRes', 'BillPayRes', 'chargeDetailRes', 'utility', 'relationDtlRes', '$state',
    function ($scope, $stateParams, billPaymentRes, rsChargeGroupRes, billV2PayRes, residentBalanceRes, billRes, BillPayRes, advanceChargeRes, chargeDetailRes, utility, relationDtlRes, $state) {
        if ($state.params.FeeNo != undefined && $state.params.FeeNo != '') {
            $scope.curResident.FeeNo = $state.params.FeeNo;
        };

        $scope.checkedFlag = true;
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
        //预缴费列表
        $scope.Data.bills = {};
        //住民绑定套餐信息
        $scope.Data.rsChargeGroupDtl = {};
        $scope.Data.rsChargeGroupAllDtl = {};

        //护理险报销费用
        $scope.NCIAmt = 0;
        //固定费用
        $scope.CntAmt = 0;
        $scope.SummaryAmount = 0;
        //********************************************
        //本次应收金额 ①
        $scope.ThisTtlAmt = 0;
        //上次预收款金额 ②
        $scope.LastPreAmt = 0;
        //本次自负(费)金额 ③
        $scope.ThisSelfAmt = 0;
        //本次预收款金额  *根据固定套餐周期类型 ④
        $scope.ThisPreAmt = 0;
        //本次应付金额 ⑤
        $scope.ThisPayAmt = 0;
        //本次实收金额
        $scope.CurAmount = 0;
        //账户可用余额 ⑥
        $scope.BLANCE = 0;

        //②=账单内固定费用总和
        //③=账单内非固定费用总和
        //④=月份的根据周期类型*固定套餐费用-报销费用
        //if ⑥<② { ①、⑤=③+④+②-⑥ }
        // else { ①、⑤=③+④ }
        //********************************************

        $scope.ZiFuAmt = 0;
        $scope.ZiFeiAmt = 0;
        $scope.TtlAmount = 0;
        $scope.GuDingZiFeiAmt = 0;
        $scope.GuDingAmt = 0;
        $scope.NoGuDingAmt = 0;

        //未缴金额
        $scope.UnpaidAmount = 0;

        $scope.CmpCurAmount = 0;

        //自费金额
        $scope.Data.bills.SelfPay = 0;
        //个人自负
        $scope.Data.bills.NCIItemSelfPay = 0;

        var SaveCount = 0;
        var SaveCounts = 0;
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

        //下次预收款天数
        $scope.NextPreDays = 0;
        //住民账户
        $scope.Data.ResidentBalances = {};

        //财务结算周期
        $scope.Data.NciFinancialMonth = {};

        //下次预收款月份是否可以修改
        $scope.NextPreMonthFlag = false;
        $scope.NextPreMonth = "";
        $scope.rsCertStatus = 0;
        //固定套餐名称
        $scope.ChargeGroupNames = [];
        $scope.ChargeGName = "";
        //是否有固定套餐
        $scope.IsHaveFlag = false;
        //固定套餐总费用
        $scope.ChargeGroupSumAmt = 0;

        $scope.CostomMonths = 0;
        $scope.CostomYears = 0

        $scope.BillV2Dtls = {};

        var checkCount = 0;

        $scope.LastBalanceTime = "";

        var curdate = new Date();
        var curyear = curdate.getFullYear();
        var curmonth = curdate.getMonth() + 1;

        $scope.NextPreDaysFlag = true;
        $scope.NextPreMonthRightFlag = true;

        //系统日期
        $scope.PayBillCurTime = getNowFormatDate();

        //获取当前日期
        function getNowFormatDate() {
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
        };

        //获取药品计价数量
        function getDrugQty(FeeItemCount, ConversionRatio) {
            var DrugQty = Math.ceil(FeeItemCount / ConversionRatio);
            return DrugQty;
        };

        function left(mainStr, lngLen) {
            if (mainStr.length >= lngLen) {
                if (lngLen > 0) {
                    return mainStr.substring(0, lngLen)
                } else {
                    return null;
                };
            };
        };

        function mid(mainStr, starnum, endnum) {
            if (mainStr.length >= 0) {
                return mainStr.substr(starnum, endnum)
            } else { return null }
        }

        $scope.changeCheckFlag = function () {
            if ($scope.checkedFlag == true) {
                $scope.checkedFlag = true;
                $scope.NextPreMonthFlag = false;
                $scope.changeNextPreMonth();
            } else {
                $scope.checkedFlag = false;
                $scope.NextPreMonthFlag = true;
                $scope.ThisPreAmt = 0;
                $scope.ThisTtlAmt = $scope.ThisSelfAmt;
                $scope.CurAmount = $scope.ThisTtlAmt;
            };
        };

        $scope.changeNextPreMonth = function () {
            if ($scope.NextPreMonth != undefined && $scope.NextPreMonth != null) {
                var date_r = /^((\d{4})-(\d{2}))$/;
                if (date_r.test($scope.NextPreMonth) == false) {
                    $scope.NextPreMonthRightFlag = false;
                    $scope.ThisPreAmt = 0;
                    $scope.ThisTtlAmt = $scope.ThisSelfAmt;
                    $scope.CurAmount = $scope.ThisTtlAmt;
                    utility.msgwarning("预收款月份格式不正确!");
                } else {
                    $scope.NextPreMonthRightFlag = true;
                    $scope.GetPreMonthAndPreDays($scope.LastBalanceTime, $scope.rsCertStatus);
                };
            } else {
                if ($scope.checkedFlag == true) {
                    $scope.NextPreMonthRightFlag = false;
                    $scope.ThisTtlAmt = $scope.ThisSelfAmt;
                    $scope.CurAmount = $scope.ThisTtlAmt;
                    utility.msgwarning("预收款月份不能是空白!");
                };
            };
        };

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: billPaymentRes,//异步请求的res
            params: { FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo, Status: 0 },
            success: function (data) {//请求成功时执行函数
                $scope.Data.bills = data.Data;
                if ($scope.Data.bills.length == 0) {
                    $scope.SummaryAmount = 0;
                    $scope.LastPreAmt = 0;
                    $scope.ThisSelfAmt = 0;
                    $scope.ThisPreAmt = 0;
                    $scope.ThisPayAmt = 0;
                    $scope.CurAmount = 0;
                    $scope.NCIAmt = 0;
                    $scope.CntAmt = 0;
                    $scope.ThisTtlAmt = 0;
                    $scope.InvoiceNo = '';
                    $scope.ReceivedAmount = '';
                    $scope.UnpaidAmount = '';
                    $scope.billShowChargeFlag = false;
                    $scope.billShowFlag = false;
                    $scope.select_all = false;
                } else {
                    angular.forEach($scope.Data.bills, function (i) {
                        i.checked = true;
                    });
                    $scope.billShowChargeFlag = true;
                    $scope.billShowFlag = true;
                    $scope.select_all = true;
                    $scope.LastPreAmt = $scope.Data.bills[0].LastPreAmt;
                    $scope.ThisSelfAmt = $scope.Data.bills[0].ThisSelfAmt;
                    $scope.LastBalanceTime = $scope.Data.bills[0].BalanceEndTime;
                };

                if ($scope.Data.ResidentBalances == undefined || $scope.Data.ResidentBalances == null) {
                    $scope.rsCertStatus = 1;
                } else {
                    $scope.rsCertStatus = $scope.Data.ResidentBalances.CertStatus;
                };

                $scope.GetPreMonthAndPreDays($scope.LastBalanceTime, $scope.rsCertStatus);

            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };

        $scope.optionsRschargeGroupDtl = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: rsChargeGroupRes,//异步请求的res
            params: { FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo },
            success: function (data) {//请求成功时执行函数
                $scope.Data.rsChargeGroupDtl = data.Data;
                angular.forEach($scope.Data.rsChargeGroupDtl, function (i) {
                    if (i.ConversionRatio > 1) {
                        i.FeeItemCount = getDrugQty(i.FeeItemCount, i.ConversionRatio);
                    };
                });
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };

        //获取账单明细
        $scope.queryBillsList = function (billId) {
            $scope.billoptions.pageInfo.CurrentPage = 1;
            $scope.billoptions.params.billid = billId;
            $scope.billoptions.search();
        };

        //获取固定套餐信息
        $scope.GetrsChargeGroup = function () {
            rsChargeGroupRes.get({ FeeNo: $scope.curResident.FeeNo, Type: "GetALLDtl" }, function (data) {
                $scope.Data.rsChargeGroupAllDtl = data.Data;
                $scope.ChargeGName = "";
                $scope.ChargeGroupSumAmt = 0;
                $scope.ThisPreAmt = 0;
                if ($scope.Data.rsChargeGroupAllDtl != undefined && $scope.Data.rsChargeGroupAllDtl != null) {
                    if ($scope.Data.rsChargeGroupAllDtl.length > 0) {
                        if ($scope.Data.rsChargeGroupAllDtl.length == 1) {
                            $scope.ChargeGName = $scope.Data.rsChargeGroupAllDtl[0].ChargeGroupName;

                            if ($scope.Data.rsChargeGroupAllDtl[0].ConversionRatio > 1) {
                                $scope.Data.rsChargeGroupAllDtl[0].FeeItemCount = getDrugQty($scope.Data.rsChargeGroupAllDtl[0].FeeItemCount, $scope.Data.rsChargeGroupAllDtl[0].ConversionRatio);
                            };

                            $scope.ChargeGroupSumAmt = $scope.Data.rsChargeGroupAllDtl[0].FeeItemCount * $scope.Data.rsChargeGroupAllDtl[0].UnitPrice;

                            if ($scope.Data.rsChargeGroupAllDtl[0].ChargeGroupPeriod == "001") { //日
                                if ($scope.NextPreDaysFlag == true) {
                                    $scope.ThisPreAmt = $scope.ChargeGroupSumAmt * $scope.NextPreDays;
                                };
                            } else if ($scope.Data.rsChargeGroupAllDtl[0].ChargeGroupPeriod == "002") { //月
                                $scope.ThisPreAmt = $scope.ChargeGroupSumAmt;
                            };
                        } else {
                            angular.forEach($scope.Data.rsChargeGroupAllDtl, function (i) {
                                if ($scope.ChargeGroupNames.length == 0) {
                                    $scope.ChargeGroupNames.push(i.ChargeGroupName);
                                } else {
                                    var index = $scope.ChargeGroupNames.indexOf(i.ChargeGroupName);
                                    if (index == -1) {
                                        $scope.ChargeGroupNames.push(i.ChargeGroupName);
                                    };
                                };

                                if (i.ConversionRatio > 1) {
                                    i.FeeItemCount = getDrugQty(i.FeeItemCount, i.ConversionRatio);
                                };

                                $scope.ChargeGroupSumAmt = $scope.ChargeGroupSumAmt + (i.FeeItemCount * i.UnitPrice);

                                if (i.ChargeGroupPeriod == "001") { //日
                                    if ($scope.NextPreDaysFlag == true) {
                                        $scope.ThisPreAmt = $scope.ThisPreAmt + i.FeeItemCount * i.UnitPrice * $scope.NextPreDays;
                                    };
                                } else if (i.ChargeGroupPeriod == "002") { //月
                                    $scope.ThisPreAmt = $scope.ThisPreAmt + i.FeeItemCount * i.UnitPrice;
                                };
                            });

                            angular.forEach($scope.ChargeGroupNames, function (item) {
                                $scope.ChargeGName = $scope.ChargeGName + item + "，";
                            });

                            $scope.ChargeGName = $scope.ChargeGName.substring(0, $scope.ChargeGName.length - 1);
                        };
                        $scope.IsHaveFlag = false;
                    } else {
                        $scope.IsHaveFlag = true;
                        $scope.ChargeGName = "尚无住民固定套餐"
                    };
                } else {
                    $scope.IsHaveFlag = true;
                    $scope.ChargeGName = "尚无住民固定套餐"
                };

                $scope.getThisTtlAmt();
            });
        };

        //获取住民账户信息
        $scope.GetrsBalances = function () {
            billPaymentRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
                $scope.Data.ResidentBalances = data.Data[0];
                if ($scope.Data.ResidentBalances != undefined) {
                    if ($scope.Data.ResidentBalances.CertNo == null) {
                        $scope.Data.ResidentBalances.CertStatus = 1;
                        $scope.rsCertStatus = $scope.Data.ResidentBalances.CertStatus;
                    } else {
                        $scope.rsCertStatus = $scope.Data.ResidentBalances.CertStatus;
                    };

                    if ($scope.Data.ResidentBalances.CertStatus == 0) {
                        $scope.HaveNCIFlag = "是";
                    } else {
                        $scope.HaveNCIFlag = "否";
                    };
                };
            });
        };

        //获取缴费人信息
        $scope.GetrsRelation = function () {
            relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
                $scope.Data.ContactList = data.Data;
            });
        };

        //获取当前登录用户信息
        $scope.GetCurUser = function () {
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.Data.curBill.RecrodBy = $scope.curUser.EmpNo;
                $scope.Data.curBill.RecrodByName = $scope.curUser.EmpName;
            };
        };

        //检查金额
        $scope.checkBill = function () {
            if ($scope.CurAmount === undefined || $scope.CurAmount === '' || $scope.CurAmount === null) {
                utility.msgwarning("本次实收金额不能是空白！");
                return;
            };

            if ($scope.LastPreAmt > $scope.Data.ResidentBalances.Blance) {
                utility.msgwarning("预收款账户余额不足,请先缴纳预收款," + "当前余额：" + $scope.Data.ResidentBalances.Blance);
                return;
            };

            if ($scope.ThisTtlAmt != $scope.ThisSelfAmt + $scope.ThisPreAmt) {
                utility.msgwarning("本次自负(费)金额+本次预收款金额与本次应收金额不符合！");
                return;
            };

            if ($scope.ThisTtlAmt != $scope.CurAmount) {
                utility.msgwarning("本次应收金额与本次实收金额不符合！");
                return;
            };
        };

        //计算本次应收金额和本次实收金额
        $scope.getThisTtlAmt = function () {
            $scope.ThisTtlAmt = $scope.ThisSelfAmt + $scope.ThisPreAmt;
            $scope.CurAmount = $scope.ThisTtlAmt;
        };

        //获取住民已收总金额
        $scope.sumAmount = function () {
            billPaymentRes.get({ currentPage: 1, pageSize: 100000000, FeeNo: $scope.curResident.FeeNo, Status: 2 }, function (data) {
                $scope.Data.receivedBills = data.Data;
                $scope.ReceivedAmount = 0;
                angular.forEach($scope.Data.receivedBills, function (i) {
                    $scope.ReceivedAmount = $scope.ReceivedAmount + i.SelfPay + i.NCIItemSelfPay;
                });

                $scope.ReceivedAmount = $scope.ReceivedAmount.toFixed(2);
            });
        };

        //获取预收款月份和预收款天数
        $scope.GetPreMonthAndPreDays = function (LastBalanceTime, CertStatus) {
            var date = new Date();
            var sep = "-";
            var year = date.getFullYear();
            var month = date.getMonth() + 1;

            if ($scope.NextPreMonth != undefined && $scope.NextPreMonth != null && $scope.NextPreMonth != "") {
                var a = $scope.NextPreMonth;
                var b = a.split("-");
                year = b[0];
                month = b[1];
            } else {
                if (LastBalanceTime == null || LastBalanceTime == '') {
                    if (month >= 1 && month <= 9) {
                        month = "0" + month;
                    }
                    $scope.NextPreMonth = year + sep + month;
                } else {
                    var timestamp = Date.parse(new Date(LastBalanceTime));
                    var newDate = new Date(timestamp);
                    year = newDate.getFullYear();
                    month = newDate.getMonth() + 2;
                    if (month >= 1 && month <= 9) {
                        month = "0" + month;
                    }
                    $scope.NextPreMonth = year + sep + month;
                };
            }

            var year2 = 0;
            if (month == "01") {
                year2 = parseInt(year) + 1;
            } else {
                year2 = year;
            };

            month = parseInt(month);

            billPaymentRes.get({ FMonth: month }, function (data) {
                var day = 0;
                var daycount = 0;
                var ncidaycount = 0;
                $scope.Data.NciFinancialMonth = data.Data;
                if ($scope.Data.NciFinancialMonth != undefined && $scope.Data.NciFinancialMonth != null) {
                    var date1 = $scope.Data.NciFinancialMonth.StartDate;
                    var date2 = $scope.Data.NciFinancialMonth.EndDate;
                    date1 = year + "-" + date1;
                    date2 = year2 + "-" + date2;
                    var timestamp = Date.parse(new Date(date1));
                    var timestamp2 = Date.parse(new Date(date2));
                    ncidaycount = Math.abs((timestamp2 - timestamp)) / (1000 * 60 * 60 * 24) + 1;
                } else {
                    day = new Date(year, month, 0);
                    ncidaycount = day.getDate();
                };

                //if (ncidaycount < 0 || ncidaycount > 31) {
                //    $scope.NextPreDaysFlag = false;
                //    utility.msgwarning("下次预收款天数不正确!");
                //} else {
                //    $scope.NextPreDaysFlag = true;
                //};

                //护理保险的天数
                if (CertStatus == 0) {
                    daycount = ncidaycount;
                } else {
                    day = new Date(year, month, 0);
                    //获取天数：
                    daycount = day.getDate();
                    //if (daycount < 0 || daycount > 31) {
                    //    $scope.NextPreDaysFlag = false;
                    //    utility.msgwarning("下次预收款天数不正确!");
                    //} else {
                    //    $scope.NextPreDaysFlag = true;
                    //};
                };
                $scope.NextPreDays = daycount;
                $scope.GetrsChargeGroup();
            });
        };

        //初始化加载缴费账单
        $scope.init = function () {
            $scope.GetrsBalances();
            $scope.GetrsRelation();
            $scope.GetCurUser();
            $scope.getThisTtlAmt();
            $scope.sumAmount();

            $scope.billoptions = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: billPaymentRes,//异步请求的res
                params: { billid: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.BillsDTLs = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            };
        };

        //接受广播
        $scope.$on("residentChange", function (e, data) {
            $scope.checkedFlag = true;
            $scope.NextPreMonthFlag = false;
            $scope.NextPreMonth = "";
            $scope.GetrsBalances();
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 1;
            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
            $scope.options.params.Status = 0;
            $scope.options.search();

            //获取住民绑定套餐详细信息
            $scope.optionsRschargeGroupDtl.pageInfo.CurrentPage = 1;
            $scope.optionsRschargeGroupDtl.pageInfo.PageSize = 10;
            $scope.optionsRschargeGroupDtl.params.FeeNo = $scope.curResident.FeeNo;
            $scope.optionsRschargeGroupDtl.search();

            $scope.GetrsRelation();
            $scope.GetCurUser();
            $scope.getThisTtlAmt();
            $scope.sumAmount();
        });

        //单选
        $scope.selectOne = function () {
            checkCount = 0;
            $scope.LastPreAmt = 0;
            $scope.ThisSelfAmt = 0;
            $scope.ZiFuAmt = 0;
            $scope.ZiFeiAmt = 0;
            $scope.NCIAmt = 0;
            $scope.TtlAmount = 0;
            $scope.TtlZiFeiAmount = 0;
            $scope.GuDingZiFeiAmt = 0;
            $scope.GuDingAmt = 0;
            $scope.NoGuDingAmt = 0;

            angular.forEach($scope.Data.bills, function (i) {
                //单选初始化选中列表
                if (i.checked == true) {
                    $scope.checked.push(i.BillId);
                    $scope.TtlAmount = $scope.TtlAmount + i.NCIItemTotalCost + i.SelfPay;
                    $scope.NCIAmt = $scope.NCIAmt + i.NCIPay;
                    $scope.ZiFuAmt = $scope.ZiFuAmt + i.NCIItemSelfPay;
                    $scope.ZiFeiAmt = $scope.ZiFeiAmt + i.SelfPay;

                    billPaymentRes.get({ currentPage: 1, pageSize: 10000, billid: i.BillId }, function (data) {
                        $scope.BillV2Dtls = data.Data;
                        angular.forEach($scope.BillV2Dtls, function (dtls) {
                            billPaymentRes.get({ FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo, FeeRecordId: dtls.CHARGEITEMID, ChargeRecordType: dtls.CHARGERECORDTYPE }, function (data) {
                                if (data.Data.length > 0) {
                                    if (data.Data[0].IsNciItem != true) {
                                        $scope.GuDingZiFeiAmt = $scope.GuDingZiFeiAmt + dtls.COST;
                                    } else {
                                        $scope.GuDingAmt = $scope.GuDingAmt + dtls.COST;
                                    }
                                } else {
                                    $scope.NoGuDingAmt = $scope.NoGuDingAmt + dtls.COST;
                                };

                                if ($scope.GuDingAmt - $scope.GuDingZiFeiAmt > 0) {
                                    //总自负费=护理险总费用-护理险固定费用
                                    $scope.TtlZiFeiAmount = $scope.TtlAmount - ($scope.GuDingAmt - $scope.GuDingZiFeiAmt);

                                    if ($scope.TtlZiFeiAmount >= 0) {
                                        //固定费用大于报销金额
                                        if (($scope.GuDingAmt - $scope.GuDingZiFeiAmt) > $scope.NCIAmt) {
                                            $scope.LastPreAmt = $scope.GuDingAmt - $scope.GuDingZiFeiAmt - $scope.NCIAmt;
                                            $scope.ThisSelfAmt = $scope.TtlZiFeiAmount;
                                        }
                                        else {
                                            $scope.LastPreAmt = 0;
                                            $scope.ThisSelfAmt = $scope.TtlZiFeiAmount - ($scope.NCIAmt - $scope.GuDingAmt);
                                        }
                                    }
                                    else {
                                        $scope.LastPreAmt = $scope.GuDingAmt - $scope.GuDingZiFeiAmt;
                                        $scope.ThisSelfAmt = ($scope.GuDingAmt - $scope.GuDingZiFeiAmt) - $scope.TtlAmount;
                                    }
                                }
                                else {
                                    $scope.LastPreAmt = 0;
                                    $scope.ThisSelfAmt = $scope.ZiFeiAmt + $scope.ZiFuAmt;
                                };

                                $scope.getThisTtlAmt();
                            });
                        });
                    });
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
                if ($scope.checkedFlag == true) {
                    $scope.ThisTtlAmt = $scope.ThisPreAmt;
                    $scope.CurAmount = $scope.ThisTtlAmt;
                } else {
                    $scope.ThisTtlAmt = 0;
                    $scope.CurAmount = $scope.ThisTtlAmt;
                }
            };

            if (checkCount == 0) {
                $scope.select_all = false;
            } else {
                //单选对比是否全选
                if ($scope.Data.bills.length == checkCount) {
                    $scope.select_all = true;
                } else {
                    $scope.select_all = false;
                };
            };
            //每次单选完清空选中列表
            $scope.checked = [];
        };

        //多选
        $scope.m = [];
        $scope.checked = [];
        $scope.selectAll = function () {
            if ($scope.select_all) {
                $scope.checked = [];
                angular.forEach($scope.Data.bills, function (i) {
                    i.checked = false;
                });

                if ($scope.checkedFlag == true) {
                    $scope.ThisTtlAmt = $scope.ThisPreAmt;
                    $scope.CurAmount = $scope.ThisTtlAmt;
                } else {
                    $scope.ThisTtlAmt = 0;
                    $scope.CurAmount = $scope.ThisTtlAmt;
                }

                $scope.LastPreAmt = 0;
                $scope.ThisSelfAmt = 0;
                $scope.ZiFuAmt = 0;
                $scope.ZiFeiAmt = 0;
                $scope.TtlAmount = 0;
                $scope.NCIAmt = 0;
                $scope.TtlZiFeiAmount = 0;
                $scope.GuDingZiFeiAmt = 0;
                $scope.GuDingAmt = 0;
                $scope.NoGuDingAmt = 0;

                $scope.billShowChargeFlag = false;
                $scope.select_all = false;
            } else {
                $scope.ThisTtlAmt = 0;
                $scope.LastPreAmt = 0;
                $scope.ThisSelfAmt = 0;
                $scope.ZiFuAmt = 0;
                $scope.ZiFeiAmt = 0;
                $scope.TtlAmount = 0;
                $scope.NCIAmt = 0;
                $scope.TtlZiFeiAmount = 0;
                $scope.GuDingZiFeiAmt = 0;
                $scope.GuDingAmt = 0;
                $scope.NoGuDingAmt = 0;

                angular.forEach($scope.Data.bills, function (i) {
                    i.checked = true;
                    $scope.checked.push(i.BillId);
                    $scope.TtlAmount = $scope.TtlAmount + i.NCIItemTotalCost + i.SelfPay;
                    $scope.NCIAmt = $scope.NCIAmt + i.NCIPay;
                    $scope.ZiFuAmt = $scope.ZiFuAmt + i.NCIItemSelfPay;
                    $scope.ZiFeiAmt = $scope.ZiFeiAmt + i.SelfPay;

                    billPaymentRes.get({ currentPage: 1, pageSize: 10000, billid: i.BillId }, function (data) {
                        $scope.BillV2Dtls = data.Data;
                        angular.forEach($scope.BillV2Dtls, function (dtls) {
                            billPaymentRes.get({ FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo, FeeRecordId: dtls.CHARGEITEMID, ChargeRecordType: dtls.CHARGERECORDTYPE }, function (data) {
                                if (data.Data.length > 0) {
                                    if (data.Data[0].IsNciItem != true) {
                                        $scope.GuDingZiFeiAmt = $scope.GuDingZiFeiAmt + dtls.COST;
                                    } else {
                                        $scope.GuDingAmt = $scope.GuDingAmt + dtls.COST;
                                    }
                                } else {
                                    $scope.NoGuDingAmt = $scope.NoGuDingAmt + dtls.COST;
                                };

                                if ($scope.GuDingAmt - $scope.GuDingZiFeiAmt > 0) {
                                    //总自负费=护理险总费用-护理险固定费用
                                    $scope.TtlZiFeiAmount = $scope.TtlAmount - ($scope.GuDingAmt - $scope.GuDingZiFeiAmt);

                                    if ($scope.TtlZiFeiAmount >= 0) {
                                        //固定费用大于报销金额
                                        if (($scope.GuDingAmt - $scope.GuDingZiFeiAmt) > $scope.NCIAmt) {
                                            $scope.LastPreAmt = $scope.GuDingAmt - $scope.GuDingZiFeiAmt - $scope.NCIAmt;
                                            $scope.ThisSelfAmt = $scope.TtlZiFeiAmount;
                                        }
                                        else {
                                            $scope.LastPreAmt = 0;
                                            $scope.ThisSelfAmt = $scope.TtlZiFeiAmount - ($scope.NCIAmt - $scope.GuDingAmt);
                                        }
                                    }
                                    else {
                                        $scope.LastPreAmt = $scope.GuDingAmt - $scope.GuDingZiFeiAmt;
                                        $scope.ThisSelfAmt = ($scope.GuDingAmt - $scope.GuDingZiFeiAmt) - $scope.TtlAmount;
                                    }
                                }
                                else {
                                    $scope.LastPreAmt = 0;
                                    $scope.ThisSelfAmt = $scope.ZiFeiAmt + $scope.ZiFuAmt;
                                };

                                $scope.getThisTtlAmt();
                            });
                        });
                    });
                });

                $scope.billShowChargeFlag = true;
                $scope.select_all = true;
            };
        };

        //确认缴费
        $scope.savePayment = function () {
            if ($scope.NextPreMonthRightFlag == false) {
                utility.msgwarning("预收款月份格式不正确！");
                return;
            };

            if ($scope.NextPreDaysFlag == false) {
                utility.msgwarning("计算预收款月份天数不正确，请联系管理员！");
                return;
            };

            if ($scope.PAYER == undefined || $scope.PAYER == '' || $scope.PAYER == null) {
                utility.msgwarning("缴费人不能是空白！");
                return;
            };

            if ($scope.PAYMENTTYPE == undefined || $scope.PAYMENTTYPE == '' || $scope.PAYMENTTYPE == null) {
                utility.msgwarning("请选择缴费方式！");
                return;
            };

            if ($scope.CurAmount === undefined || $scope.CurAmount === '' || $scope.CurAmount === null) {
                utility.msgwarning("本次实收金额不能是空白！");
                return;
            };

            if ($scope.LastPreAmt > $scope.Data.ResidentBalances.Blance) {
                utility.msgwarning("预收款账户余额不足,请先缴纳预收款," + "当前余额：" + $scope.Data.ResidentBalances.Blance);
                return;
            };

            if ($scope.ThisTtlAmt != $scope.ThisSelfAmt + $scope.ThisPreAmt) {
                utility.msgwarning("本次自负(费)金额+本次预收款金额与本次应收金额不符合！");
                return;
            };

            if ($scope.ThisTtlAmt != $scope.CurAmount) {
                utility.msgwarning("本次应收金额与本次实收金额不符合！");
                return;
            };

            //是否预付下次预收款  是：账户余额=账户余额-上次预收款+本次预收款  否：账户余额=账户余额-上次预收款
            if ($scope.checkedFlag == true) {
                $scope.Data.ResidentBalances.Blance = $scope.Data.ResidentBalances.Blance - $scope.LastPreAmt + $scope.ThisPreAmt;
            } else {
                $scope.Data.ResidentBalances.Blance = $scope.Data.ResidentBalances.Blance - $scope.LastPreAmt;
            };

            $scope.Data.tempBillsV2Pay.SELFPAY = 0;
            $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY = 0;
            $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST = 0;
            $scope.Data.tempBillsV2Pay.NCIPAY = 0;
            SaveCounts = 0;
            angular.forEach($scope.Data.bills, function (i) {
                if (i.checked == true) {
                    //自费总费用
                    $scope.Data.tempBillsV2Pay.SELFPAY = $scope.Data.tempBillsV2Pay.SELFPAY + i.SelfPay;
                    //个人总自负
                    $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY = $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY + i.NCIItemSelfPay;
                    //护理险项目总费用
                    $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST = $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST + i.NCIItemTotalCost;
                    //护理险可报销费用
                    $scope.Data.tempBillsV2Pay.NCIPAY = $scope.Data.tempBillsV2Pay.NCIPAY + i.NCIPay;

                    SaveCounts = SaveCounts + 1;
                };
            });

            if (SaveCounts == 0) {
                utility.msgwarning("请先选择一个账单进行缴费！");
                return;
            }

            $scope.Data.billsV2Pay.SELFPAY = $scope.Data.tempBillsV2Pay.SELFPAY;
            $scope.Data.billsV2Pay.NCIITEMSELFPAY = $scope.Data.tempBillsV2Pay.NCIITEMSELFPAY;
            $scope.Data.billsV2Pay.NCIITEMTOTALCOST = $scope.Data.tempBillsV2Pay.NCIITEMTOTALCOST;
            $scope.Data.billsV2Pay.NCIPAY = $scope.Data.tempBillsV2Pay.NCIPAY;
            $scope.Data.billsV2Pay.ACCOUNTBALANCEPAY = $scope.LastPreAmt;
            $scope.Data.billsV2Pay.EMPNAME = "";
            $scope.Data.billsV2Pay.OPERATOR = $scope.Data.curBill.RecrodBy;
            $scope.Data.billsV2Pay.PAYER = $scope.PAYER;
            $scope.Data.billsV2Pay.PAYMENTTYPE = $scope.PAYMENTTYPE;
            $scope.Data.billsV2Pay.INVOICENO = $scope.InvoiceNo;
            $scope.Data.billsV2Pay.FEENO = $scope.curResident.FeeNo;
            $scope.Data.billsV2Pay.CREATEBY = null;
            $scope.Data.billsV2Pay.CREATETIME = null;
            $scope.Data.billsV2Pay.UPDATEBY = null;
            $scope.Data.billsV2Pay.UPDATETIME = null;
            $scope.Data.billsV2Pay.ISDELETE = false;

            //累计缴费额度=累计自费金额+累计个人自负
            $scope.Data.ResidentBalances.TotalPayment = $scope.Data.ResidentBalances.TotalPayment + $scope.Data.billsV2Pay.SELFPAY + $scope.Data.billsV2Pay.NCIITEMSELFPAY;
            //累计产生费用额度=累计自费金额+累计护理险项目总费用
            $scope.Data.ResidentBalances.TotalCost = $scope.Data.ResidentBalances.TotalCost + $scope.Data.billsV2Pay.SELFPAY + $scope.Data.billsV2Pay.NCIITEMTOTALCOST;
            //累计报销额度=累计护理险可报销费用
            $scope.Data.ResidentBalances.TotalNCIPay = $scope.Data.ResidentBalances.TotalNCIPay + $scope.Data.billsV2Pay.NCIPAY;
            //护理险超支金额=累计个人自负
            $scope.Data.ResidentBalances.TotalNCIOverspend = $scope.Data.ResidentBalances.TotalNCIOverspend + $scope.Data.billsV2Pay.NCIITEMSELFPAY;


            $scope.BillV2List = {};
            $scope.BillV2List.BillV2Lists = [];

            SaveCount = 0;
            billV2PayRes.save($scope.Data.billsV2Pay, function (data) {
                angular.forEach($scope.Data.bills, function (i) {
                    if (i.checked == true) {
                        i.BillPayId = data.Data.BILLPAYID;
                        i.Status = 2;
                        i.BalanceOperator = $scope.Data.curBill.RecrodBy;
                        $scope.BillV2List.BillV2Lists.push(i);
                        SaveCount = SaveCount + 1;
                    };
                });

                billPaymentRes.save($scope.BillV2List, function (data) {
                    if (data.ResultCode == 1001) {
                        residentBalanceRes.save($scope.Data.ResidentBalances, function (data) {
                            $scope.InvoiceNo = '';
                            $scope.init();

                            $scope.options.params.currentPage = 1;
                            $scope.options.params.pageSize = 10;
                            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
                            $scope.options.params.Status = 0;
                            $scope.options.search();
                            $scope.optionsRschargeGroupDtl.pageInfo.CurrentPage = 1;
                            $scope.optionsRschargeGroupDtl.pageInfo.PageSize = 10;
                            $scope.optionsRschargeGroupDtl.params.FeeNo = $scope.curResident.FeeNo;
                            $scope.optionsRschargeGroupDtl.search();

                            utility.message("住民" + $scope.Data.ResidentBalances.Name + "的" + SaveCount + "个账单缴费成功！");
                        });
                    };
                });
            });
        };

        //账单明细
        $scope.showBillsList = function (item) {
            $scope.billId = item.BillId;
            $scope.queryBillsList($scope.billId);
            $("#modalDetail").modal("toggle");
        };

        //关闭账单明细
        $scope.cancel = function () {
            $("#modalDetail").modal("toggle");
        };

        //关闭账单明细
        $scope.cancelChargeGroup = function () {
            $("#modalchargeGroup").modal("toggle");
        };

        //选择填写人员
        $scope.staffSelected = function (item) {
            $scope.Data.curBill.RecrodBy = item.EmpNo;
        };

        $scope.init();
    }
]).controller("billPaymentRecCtrl", ['$scope', '$stateParams', 'billPaymentRecRes', 'billRes', 'BillPayRes', 'chargeDetailRes', 'utility', 'relationDtlRes', '$state',
    function ($scope, $stateParams, billPaymentRecRes, billRes, BillPayRes, chargeDetailRes, utility, relationDtlRes, $state) {
        $scope.Data = {};
        $scope.Pay = {};
        $scope.Data.curBill = {};
        $scope.disabledcheck = false;
        $scope.BillKey = {};
        $scope.Data.billRecs = {};

        //接受广播
        $scope.$on("residentChange", function (e, data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.params.FeeNo = $scope.curResident.FeeNo;
            $scope.options.params.Status = 2;
            $scope.options.search();

            relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
                $scope.Data.ContactList = data.Data;
            });
        });

        //获取缴费记录
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: billPaymentRecRes,//异步请求的res
            params: { currentPage: 1, pageSize: 10, FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo, Status: 2 },
            success: function (data) {//请求成功时执行函数
                $scope.Data.billRecs = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };

        //初始化加载历史缴费账单
        $scope.init = function () {
            billPaymentRecRes.get({
                currentPage: 1, pageSize: 10, FeeNo: $scope.curResident.FeeNo, Status: 2
            }, function (data) {
                $scope.Data.billRecs = data.Data;
            });
        };

        //账单明细
        $scope.showBillsList = function (item) {
            $scope.queryBillsList(item.BillId);
            $scope.queryBillRecsList(item.BillPayId);
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
            billPaymentRecRes.get({ currentPage: 1, pageSize: 10, billid: billId }, function (data) {
                $scope.BillsDTLs = data.Data;
            });
        };

        //账单缴费记录
        $scope.showBillRecsList = function (item) {
            $scope.queryBillRecsList(item.BillPayId);
            $("#ChargeRecListModal").modal("toggle");
            $scope.displayMode = "edit";
        };

        //关闭账单缴费记录
        $scope.close = function () {
            $("#ChargeRecListModal").modal("toggle");
            $scope.displayMode = "list";
        }

        //获取账单缴费记录
        $scope.queryBillRecsList = function (billPayid) {
            billPaymentRecRes.get({ currentPage: 1, pageSize: 10, billPayId: billPayid, billCharge: "Rec" }, function (data) {
                $scope.BillRecDTLs = data.Data[0];
                $scope.GetPAYMENTTYPE($scope.BillRecDTLs.PAYMENTTYPE);
            });
        };

        //缴费方式状态转换
        $scope.GetPAYMENTTYPE = function (PAYMENTTYPE) {
            if (PAYMENTTYPE == "001") {
                $scope.BPAYMENTTYPE = "现金";
            } else if (PAYMENTTYPE == "002") {
                $scope.BPAYMENTTYPE = "刷卡";
            } else if (PAYMENTTYPE == "003") {
                $scope.BPAYMENTTYPE = "支票";
            } else if (PAYMENTTYPE == "004") {
                $scope.BPAYMENTTYPE = "汇款";
            }
            else {
                $scope.BPAYMENTTYPE = "";
            };
        };

        $scope.init();
    }
]).controller("advanceChargeCtrl", ['$scope', '$stateParams', 'advanceChargeRes', 'residentBalanceRes', 'billRes', 'BillPayRes', 'chargeDetailRes', 'utility', 'relationDtlRes', '$state',
    function ($scope, $stateParams, advanceChargeRes, residentBalanceRes, billRes, BillPayRes, chargeDetailRes, utility, relationDtlRes, $state) {
        $scope.Data = {};
        $scope.Pay = {};
        $scope.Data.curBill = {};
        $scope.disabledcheck = false;
        $scope.BillKey = {};
        $scope.Data.prebills = {};
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
            advanceChargeRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
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
                ajaxObject: advanceChargeRes,//异步请求的res
                params: {
                    FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo
                },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.prebills = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }

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
            }
        }



        //选择填写人员
        $scope.staffSelected = function (item) {
            $scope.Data.curBill.RecrodBy = item.EmpNo;
        }

        //预收款，确定收费
        $scope.saveEdit = function (ResidentBalances, PreCharges, RecrodBy) {
            if (PreCharges != undefined) {
                if (PreCharges.AMOUNT == undefined || PreCharges.AMOUNT == '' || PreCharges.AMOUNT == null) {
                    utility.msgwarning("预收款金额不能是空白！");
                    return;
                };

                if (PreCharges.RECEIPTNO == undefined || PreCharges.RECEIPTNO == '' || PreCharges.RECEIPTNO == null) {
                    utility.msgwarning("收据编号不能是空白！");
                    return;
                };

                if (PreCharges.PAYER == undefined || PreCharges.PAYER == '' || PreCharges.PAYER == null) {
                    utility.msgwarning("缴费人不能是空白！");
                    return;
                };

                if (PreCharges.PAYMENTTYPE == undefined || PreCharges.PAYMENTTYPE == '' || PreCharges.PAYMENTTYPE == null) {
                    utility.msgwarning("请选择缴费方式！");
                    return;
                };
            } else {
                utility.msgwarning("必填项不能是空白！");
                return;
            };


            if (ResidentBalances.Status == 0) {
                if (PreCharges.AMOUNT <= 0) {
                    utility.msgwarning("预收款金额必须大于零");
                    return;
                } else {
                    if (ResidentBalances.BalanceID != undefined && ResidentBalances.BalanceID != null && !isEmpty(ResidentBalances.BalanceID)) {
                        $scope.PreCharges.BALANCEID = ResidentBalances.BalanceID;
                        $scope.PreCharges.OPERATOR = RecrodBy;
                        ResidentBalances.PreAmount = PreCharges.AMOUNT;
                        advanceChargeRes.save(PreCharges, function (data) {
                            residentBalanceRes.save(ResidentBalances, function (data) {
                                $scope.init();
                                $scope.clearPaydetail();
                                utility.message(ResidentBalances.Name + "的预收款缴费成功！");
                            });
                        });
                    };
                };
            } else {
                utility.msgwarning("住民账户处于停用状态，无法进行预收款缴费");
                return;
            };
        };


        //修改住民账户状态和是否享受护理险
        $scope.GetStatus = function (STATUS, ISHAVENCI) {
            if (STATUS == 0) {
                $scope.BStatus = "正常";
            } else {
                $scope.BStatus = "停用";
            };

            if (ISHAVENCI) {
                $scope.BIsHaveNCI = "是";
            } else {
                $scope.BIsHaveNCI = "否";
            };
        };

        //保存成功，清空享受护理险否、预售金额备注
        $scope.clearPaydetail = function () {
            $scope.PreCharges.AMOUNT = "";
            $scope.PreCharges.RECEIPTNO = "";
            $scope.PreCharges.COMMENT = "";
        };

        $scope.init();
    }
]).controller("RegAccountInfoCtrl", ['$scope', '$stateParams', 'advanceChargeRes', 'utility', '$state',
    function ($scope, $stateParams, advanceChargeRes, utility, $state) {
        $scope.ResidentBalance = {};

        //接受广播
        $scope.$on("residentChange", function (e, data) {
            //获取住民账户信息
            advanceChargeRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
                $scope.ResidentBalance = data.Data[0];
                if ($scope.ResidentBalance != undefined) {
                    $scope.GetStatus($scope.ResidentBalance.Status, $scope.ResidentBalance.IsHaveNCI);
                };
            });
        });


        //初始化数据
        $scope.init = function () {
            //获取住民账户信息
            advanceChargeRes.get({ FeeNo: $scope.curResident.FeeNo }, function (data) {
                $scope.ResidentBalance = data.Data[0];
                if ($scope.ResidentBalance != undefined) {
                    if ($scope.ResidentBalance.CertNo == null) {
                        $scope.ResidentBalance.CertStatus = 1;
                    };
                    $scope.GetStatus($scope.ResidentBalance.Status, $scope.ResidentBalance.CertStatus);
                };
            });
        };


        //修改住民账户状态和是否享受护理险
        $scope.GetStatus = function (STATUS, CertStatus) {
            if (STATUS == 0) {
                $scope.BStatus = "正常";
            } else {
                $scope.BStatus = "停用";
            };

            if (CertStatus == 0) {
                $scope.BIsHaveNCI = "是";
            } else {
                $scope.BIsHaveNCI = "否";
                $scope.ResidentBalance.NCIPayLevel = '';
                $scope.ResidentBalance.NCIPayScale = '';
                $scope.ResidentBalance.CertNo = '';
                $scope.ResidentBalance.CertStartTime = '';
                $scope.ResidentBalance.CertExpiredTime = '';
            };
        };

        $scope.init();
    }
]);
