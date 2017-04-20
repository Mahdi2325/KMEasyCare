/*
创建人: 吴晓波
创建日期:2017-04-07
说明:费用管理—医嘱发送
*/
angular.module("sltcApp")
.controller("nurSendOrderCtrl", ['$scope', '$filter', '$stateParams', 'docOrderRes', 'utility', '$state', 'cloudAdminUi', 'residentV2Res', function ($scope, $filter, $stateParams, docOrderRes, utility, $state, cloudAdminUi, residentV2Res) {
    cloudAdminUi.handleGoToTop();
    //日期正则表达
    var date_s = /^(?:19|20)[0-9][0-9]-(?:(?:0[1-9])|(?:1[0-2]))-(?:(?:[0-2][1-9])|(?:[1-3][0-1])) (?:(?:[0-2][0-3])|(?:[0-1][0-9])):[0-5][0-9]:[0-5][0-9]$/;
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    //当前用户
    $scope.curUser = [];
    $scope.Data.curUser = {};
    //住民信息
    $scope.Data.rs = {};
    //医嘱浏览
    $scope.Data.docOrders = {};
    //已开医嘱明细
    $scope.Data.docOrderDtlsInfo = [];
    //已开医嘱对应收费项目列表
    $scope.Data.chargeItems = {};
    $scope.Data.chargeItems.FeeCode = "";
    $scope.Data.chargeItems.ChargeGroupId = "";
    $scope.Data.chargeItems.ItemType = "";
    //查询医嘱明细
    //获取当前日期
    function getNowFormatTime() {
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
        var currentdate = year + sep + month + sep + strdate + " 00:00:00";
        return currentdate;
    };

    $scope.Data.SearchOrders = {};
    $scope.Data.SearchOrders.StartDate = getNowFormatTime();
    $scope.Data.SearchOrders.EndDate = getNowFormatTime();
    $scope.Data.SearchOrders.ALLchargeOrderType = true;
    $scope.Data.SearchOrders.StartDateShow = false;
    $scope.Data.SearchOrders.EndDateShow = false;
    $scope.Data.SearchOrders.AuditOption = "002";
    $scope.Data.SearchOrders.CheckOption = "001";
    $scope.Data.SearchOrders.StopOption = "001";
    $scope.Data.SearchOrders.CancelOption = "003";
    $scope.Data.SearchOrders.TimeOption = "002";

    //查询医嘱条件
    $scope.QueryTJ = [];
    $scope.QueryTJ.OrderType = 99;
    $scope.QueryTJ.ConfirmFlag = 1;
    $scope.QueryTJ.CheckFlag = 99;
    $scope.QueryTJ.StopFlag = 0;
    $scope.QueryTJ.CancelFlag = 0;
    $scope.QueryTJ.TimeFlag = 1;
    $scope.QueryTJ.StartDate = $scope.Data.SearchOrders.StartDate;
    $scope.QueryTJ.EndDate = $scope.Data.SearchOrders.EndDate;

    //未发送医嘱记录
    $scope.SelectAllShowFlag = false;
    $scope.select_all = false;
    $scope.Data.NoSendOrderDtls = [];
    //已发送医嘱记录
    $scope.Data.SentOrderDtls = [];
    $scope.showPostFlag = false;
    $scope.LoadType = 2;

    //获取医嘱浏览列表信息
    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: docOrderRes,//异步请求的res
        params: {
            feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
            orderType: $scope.QueryTJ.OrderType,
            confirmFlag: $scope.QueryTJ.ConfirmFlag,
            checkFlag: $scope.QueryTJ.CheckFlag,
            stopFlag: $scope.QueryTJ.StopFlag,
            cancelFlag: $scope.QueryTJ.CancelFlag,
            timeFlag: $scope.QueryTJ.TimeFlag,
            startDate: $scope.QueryTJ.StartDate,
            endDate: $scope.QueryTJ.EndDate,
            loadType: $scope.FeeNo == "" ? 2 : 1,
            sortType: 2
        },
        success: function (data) {//请求成功时执行函数
            $scope.Data.docOrders = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    //获取医嘱浏览列表的医嘱明细信息
    $scope.optionsOrderDtlsInfo = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: docOrderRes,//异步请求的res
        params: { orderNo: $scope.OrderNo == "" ? -1 : $scope.OrderNo, feeCode: $scope.Data.chargeItems.FeeCode == "" ? -1 : $scope.Data.chargeItems.FeeCode, chargeGroupId: $scope.Data.chargeItems.ChargeGroupId == "" ? -1 : $scope.Data.chargeItems.ChargeGroupId, itemType: $scope.Data.chargeItems.ItemType == "" ? -1 : $scope.Data.chargeItems.ItemType },
        success: function (data) {//请求成功时执行函数
            if (data.Data.length == 0) {
                $scope.Data.chargeItems = {};
                return;
            }
            else {
                $scope.Data.chargeItems = data.Data;
            }

        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    //获取已发送医嘱列表
    $scope.optionsSentOrder = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: docOrderRes,//异步请求的res
        params: {
            feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
            loadType: $scope.LoadType
        },
        success: function (data) {//请求成功时执行函数
            $scope.Data.SentOrderDtls = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

     //初始化
    $scope.init = function () {
        $scope.GetCurUser();
        $scope.GetRsInfo();
    };

    //选择住民
    $scope.txtResidentIDChange = function (resident) {
        $scope.Data.NoSendOrderDtls = [];

        $scope.curResident = resident;
        $scope.FeeNo = resident.FeeNo;

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.feeNo = resident.FeeNo;
        $scope.options.params.orderType = $scope.QueryTJ.OrderType;
        $scope.options.params.confirmFlag = $scope.QueryTJ.ConfirmFlag;
        $scope.options.params.checkFlag = $scope.QueryTJ.CheckFlag;
        $scope.options.params.stopFlag = $scope.QueryTJ.StopFlag;
        $scope.options.params.cancelFlag = $scope.QueryTJ.CancelFlag;
        $scope.options.params.timeFlag = $scope.QueryTJ.TimeFlag;
        $scope.options.params.startDate = $scope.QueryTJ.StartDate;
        $scope.options.params.endDate = $scope.QueryTJ.EndDate;
        $scope.options.params.loadType = $scope.FeeNo == "" ? 2 : 1;
        $scope.options.search();

        $scope.optionsSentOrder.pageInfo.CurrentPage = 1;
        $scope.optionsSentOrder.pageInfo.PageSize = 10;
        if ($scope.FeeNo == "") {
            $scope.LoadType = 2;
        } else {
            $scope.LoadType = 1;
        }
        $scope.optionsSentOrder.params.feeNo = resident.FeeNo;
        $scope.optionsSentOrder.params.loadType = $scope.LoadType;
        $scope.optionsSentOrder.search();

        $scope.GetRsInfo();
    };

    //获取当前登录用户信息
    $scope.GetCurUser = function () {
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.Data.curUser.RecrodBy = $scope.curUser.EmpNo;
            $scope.Data.curUser.RecrodByName = $scope.curUser.EmpName;
        };
    };

    //获取住民入住信息
    $scope.GetRsInfo = function () {
        residentV2Res.get({ id: $scope.FeeNo == "" ? -1 : $scope.FeeNo }, function (data) {
            $scope.Data.rs = data.Data[0];
            if ($scope.Data.rs != undefined && $scope.Data.rs != null && $scope.Data.rs != "") {
                if ($scope.Data.rs.CertStatus == 0) {
                    $scope.NciFlag = "是";
                } else {
                    $scope.NciFlag = "否";
                };
            };
        });
    };

    //修改查询：医嘱类型条件
    $scope.AllSearchOrderType = function () {
        $scope.Data.SearchOrders.ALLchargeOrderType = true;
        $scope.Data.SearchOrders.LSearchOrderType = false;
        $scope.Data.SearchOrders.SSearchOrderType = false;
        $scope.QueryTJ.OrderType = 99;
        $scope.QueryOrder();
    };

    //修改查询：医嘱类型条件
    $scope.LSearchOrderType = function () {
        $scope.Data.SearchOrders.ALLchargeOrderType = false;
        $scope.Data.SearchOrders.LSearchOrderType = true;
        $scope.Data.SearchOrders.SSearchOrderType = false;
        $scope.QueryTJ.OrderType = 1;
        $scope.QueryOrder();
    };

    //修改查询：医嘱类型条件
    $scope.SSearchOrderType = function () {
        $scope.Data.SearchOrders.ALLchargeOrderType = false;
        $scope.Data.SearchOrders.LSearchOrderType = false;
        $scope.Data.SearchOrders.SSearchOrderType = true;
        $scope.QueryTJ.OrderType = 2;
        $scope.QueryOrder();
    };

    //修改查询：医嘱停用条件
    $scope.chargeStopOption = function () {
        if ($scope.Data.SearchOrders.StopOption == "") {
            $scope.QueryTJ.StopFlag = 99;
        } else if ($scope.Data.SearchOrders.StopOption == "001") {
            $scope.QueryTJ.StopFlag = 0;
        } else if ($scope.Data.SearchOrders.StopOption == "002") {
            $scope.QueryTJ.StopFlag = 1;
        } else {
            $scope.QueryTJ.StopFlag = 99;
        };
        $scope.QueryOrder();
    };

    //修改查询：医嘱审核条件
    $scope.chargeAuditOption = function () {
        if ($scope.Data.SearchOrders.AuditOption == "") {
            $scope.QueryTJ.ConfirmFlag = 99;
        } else if ($scope.Data.SearchOrders.AuditOption == "001") {
            $scope.QueryTJ.ConfirmFlag = 99;
        } else if ($scope.Data.SearchOrders.AuditOption == "002") {
            $scope.QueryTJ.ConfirmFlag = 1;
        } else {
            $scope.QueryTJ.ConfirmFlag = 0;
        };
        $scope.QueryOrder();
    };

    //修改查询医嘱校对条件
    $scope.chargeCheckOption = function () {
        if ($scope.Data.SearchOrders.CheckOption == "") {
            $scope.QueryTJ.CheckFlag = 99;
        } else if ($scope.Data.SearchOrders.CheckOption == "001") {
            $scope.QueryTJ.CheckFlag = 99;
        } else if ($scope.Data.SearchOrders.CheckOption == "002") {
            $scope.QueryTJ.CheckFlag = 1;
        } else {
            $scope.QueryTJ.CheckFlag = 0;
        };
        $scope.QueryOrder();
    };

    //修改查询：医嘱时间条件
    $scope.chargeSearchTimeOption = function () {
        if ($scope.Data.SearchOrders.TimeOption == "") {
            $scope.Data.SearchOrders.StartDateShow = true;
            $scope.Data.SearchOrders.EndDateShow = true;
            $scope.QueryTJ.TimeFlag = 0;
            $scope.QueryOrder();
        } else if ($scope.Data.SearchOrders.TimeOption == "001") {
            $scope.Data.SearchOrders.StartDateShow = true;
            $scope.Data.SearchOrders.EndDateShow = true;
            $scope.QueryTJ.TimeFlag = 0;
            $scope.QueryOrder();
        } else {
            $scope.Data.SearchOrders.StartDateShow = false;
            $scope.Data.SearchOrders.EndDateShow = false;
            $scope.QueryTJ.TimeFlag = 1;
            $scope.QueryOrder();
        };
    };

    //修改查询：医嘱作废条件
    $scope.chargeCancelOption = function () {
        if ($scope.Data.SearchOrders.CancelOption == "") {
            $scope.QueryTJ.CancelFlag = 99;
        } else if ($scope.Data.SearchOrders.CancelOption == "001") {
            $scope.QueryTJ.CancelFlag = 99;
        } else if ($scope.Data.SearchOrders.CancelOption == "002") {
            $scope.QueryTJ.CancelFlag = 1;
        } else {
            $scope.QueryTJ.CancelFlag = 0;
        };
        $scope.QueryOrder();
    };

    //修改查询：开始执行日期
    $scope.chargeSearchStartDate = function () {
        $scope.QueryTJ.StartDate = $scope.Data.SearchOrders.StartDate;
        $scope.QueryOrder();
    }

    //修改查询：结束执行日期
    $scope.chargeSearchEndDate = function () {
        $scope.QueryTJ.EndDate = $scope.Data.SearchOrders.EndDate;
        $scope.QueryOrder();
    }

    //查询医嘱--通过条件筛选
    $scope.QueryOrder = function () {
        if (angular.isDefined($scope.QueryTJ.StartDate) && $scope.QueryTJ.StartDate != null && $scope.QueryTJ.StartDate != "") {
            if (date_s.test($scope.QueryTJ.StartDate) != true) {
                utility.msgwarning("开始时间格式不正确！");
                return;
            };
        } else {
            utility.msgwarning("开始时间不能是空白！");
            return;
        };

        if (angular.isDefined($scope.QueryTJ.EndDate) && $scope.QueryTJ.EndDate != null && $scope.QueryTJ.EndDate != "") {
            if (date_s.test($scope.QueryTJ.EndDate) != true) {
                utility.msgwarning("结束时间格式不正确！");
                return;
            };
        } else {
            utility.msgwarning("结束时间不能是空白！");
            return;
        };

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.feeNo = $scope.FeeNo == "" ? -1 : $scope.FeeNo;
        $scope.options.params.orderType = $scope.QueryTJ.OrderType;
        $scope.options.params.confirmFlag = $scope.QueryTJ.ConfirmFlag;
        $scope.options.params.checkFlag = $scope.QueryTJ.CheckFlag;
        $scope.options.params.stopFlag = $scope.QueryTJ.StopFlag;
        $scope.options.params.cancelFlag = $scope.QueryTJ.CancelFlag;
        $scope.options.params.timeFlag = $scope.QueryTJ.TimeFlag;
        $scope.options.params.startDate = $scope.QueryTJ.StartDate;
        $scope.options.params.endDate = $scope.QueryTJ.EndDate;
        $scope.options.params.loadType = $scope.FeeNo == "" ? 2 : 1;
        $scope.options.search();
    };

    //校对医嘱
    $scope.checkOrder = function (item) {
        item.CheckFlag = 1;
        docOrderRes.save(item, function (data) {
            if (data.ResultCode == -1) {
                item.CheckFlag = 0;
                utility.msgwarning(data.ResultMessage);
            }
            else {
                $scope.options.params.feeNo = $scope.FeeNo == "" ? -1 : $scope.FeeNo;
                $scope.options.params.orderType = $scope.QueryTJ.OrderType;
                $scope.options.params.confirmFlag = $scope.QueryTJ.ConfirmFlag;
                $scope.options.params.checkFlag = $scope.QueryTJ.CheckFlag;
                $scope.options.params.stopFlag = $scope.QueryTJ.StopFlag;
                $scope.options.params.cancelFlag = $scope.QueryTJ.CancelFlag;
                $scope.options.params.timeFlag = $scope.QueryTJ.TimeFlag;
                $scope.options.params.startDate = $scope.QueryTJ.StartDate;
                $scope.options.params.endDate = $scope.QueryTJ.EndDate;
                $scope.options.params.loadType = $scope.FeeNo == "" ? 2 : 1;
                $scope.options.search();
                utility.message("校对成功");
            };
        });
    };

    //查看医嘱
    $scope.LookUpOrder = function (item) {
        $scope.txtPostNurseName = "";
        $scope.txtPostDate = "";
        $scope.txtOrderType = "";
        $scope.txtItemType = "";
        $scope.txtConfirmFlag = "";
        $scope.txtDeleteFlag = "";
        $scope.txtCheckFlag = "";
        $scope.txtStopFlag = "";
        $scope.txtStopCheckFlag = "";
        $scope.Data.docOrderDtlsInfo = {};

        $scope.showPostFlag = false;

        if (angular.isDefined(item.PostNurseName) && item.PostNurseName != null && item.PostNurseName != "") {
            $scope.showPostFlag = true;
            $scope.txtPostNurseName = item.PostNurseName;
        };

        if (angular.isDefined(item.PostDate) && item.PostDate != null && item.PostDate != "") {
            $scope.showPostFlag = true;
            $scope.txtPostDate = item.PostDate;
        };

        docOrderRes.get({ orderNo: item.OrderNo }, function (data) {
            $scope.Data.docOrderDtlsInfo = data.Data[0];
            if (angular.isDefined($scope.Data.docOrderDtlsInfo)) {
                if ($scope.Data.docOrderDtlsInfo.OrderType == 1) {
                    $scope.txtOrderType = "长嘱";
                } else {
                    $scope.txtOrderType = "临嘱";
                };

                if ($scope.Data.docOrderDtlsInfo.ItemType == 1) {
                    $scope.txtItemType = "药品";
                } else if ($scope.Data.docOrderDtlsInfo.ItemType == 2) {
                    $scope.txtItemType = "耗材";
                } else if ($scope.Data.docOrderDtlsInfo.ItemType == 3) {
                    $scope.txtItemType = "服务";
                } else if ($scope.Data.docOrderDtlsInfo.ItemType == 4) {
                    $scope.txtItemType = "套餐";
                };

                if ($scope.Data.docOrderDtlsInfo.ConfirmFlag == 0) {
                    $scope.txtConfirmFlag = "未审核";
                } else {
                    $scope.txtConfirmFlag = "已审核";
                };

                if ($scope.Data.docOrderDtlsInfo.DeleteFlag == 0) {
                    $scope.txtDeleteFlag = "未作废";
                } else {
                    $scope.txtDeleteFlag = "已作废";
                };

                if ($scope.Data.docOrderDtlsInfo.CheckFlag == 0) {
                    $scope.txtCheckFlag = "未校对";
                } else {
                    $scope.txtCheckFlag = "已校对";
                };

                if ($scope.Data.docOrderDtlsInfo.StopFlag == 0) {
                    $scope.txtStopFlag = "未停止";
                } else if ($scope.Data.docOrderDtlsInfo.StopFlag == 1) {
                    $scope.txtStopFlag = "结束时间到期自动停止";
                } else if ($scope.Data.docOrderDtlsInfo.StopFlag == 2) {
                    $scope.txtStopFlag = "医生强制停止";
                } else if ($scope.Data.docOrderDtlsInfo.StopFlag == 3) {
                    $scope.txtStopFlag = "关账停止";
                } else if ($scope.Data.docOrderDtlsInfo.StopFlag == 4) {
                    $scope.txtStopFlag = "结案停止";
                } else {
                    $scope.txtStopFlag = "已停止";
                };

                if ($scope.Data.docOrderDtlsInfo.StopCheckFlag == 0) {
                    $scope.txtStopCheckFlag = "未确认停止";
                } else {
                    $scope.txtStopCheckFlag = "已确认停止";
                };

                $scope.txtUnitPrice = item.UnitPrice;
                $scope.txtAmount = item.Amount;

                $scope.optionsOrderDtlsInfo.pageInfo.CurrentPage = 1;
                $scope.optionsOrderDtlsInfo.pageInfo.PageSize = 10;
                $scope.optionsOrderDtlsInfo.params.feeCode = item.FeeCode == null ? -1 : item.FeeCode;
                $scope.optionsOrderDtlsInfo.params.orderNo = item.OrderNo;
                $scope.optionsOrderDtlsInfo.params.chargeGroupId = item.ChargeGroupId == null ? -1 : item.ChargeGroupId;
                $scope.optionsOrderDtlsInfo.params.itemType = item.ItemType;
                $scope.optionsOrderDtlsInfo.search();
            };
        });
    };

    //下载医嘱
    $scope.loadOrder = function () {
        if ($scope.FeeNo == "") {
            utility.msgwarning("请选择住民！");
            return;
        } else {
            $scope.Data.NoSendOrderDtls = [];
            docOrderRes.get({
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                orderType: $scope.QueryTJ.OrderType,
                confirmFlag: $scope.QueryTJ.ConfirmFlag,
                checkFlag: $scope.QueryTJ.CheckFlag,
                stopFlag: $scope.QueryTJ.StopFlag,
                cancelFlag: $scope.QueryTJ.CancelFlag,
                timeFlag: $scope.QueryTJ.TimeFlag,
                startDate: $scope.QueryTJ.StartDate,
                endDate: $scope.QueryTJ.EndDate,
                loadType: 1
            }, function (data) {
                $scope.Data.NoSendOrderDtls = data.Data;
                if (data.Data.length > 0) {
                    $scope.SelectAllShowFlag = true;
                    $scope.select_all = true;
                } else {
                    utility.message("未下载到医嘱数据！");
                    $scope.SelectAllShowFlag = false;
                    $scope.select_all = false;
                };
            });
        };
    };

    //下载所有住民医嘱
    $scope.loadAllOrder = function () {
        $scope.Data.NoSendOrderDtls = [];
        docOrderRes.get({
            feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
            orderType: $scope.QueryTJ.OrderType,
            confirmFlag: $scope.QueryTJ.ConfirmFlag,
            checkFlag: $scope.QueryTJ.CheckFlag,
            stopFlag: $scope.QueryTJ.StopFlag,
            cancelFlag: $scope.QueryTJ.CancelFlag,
            timeFlag: $scope.QueryTJ.TimeFlag,
            startDate: $scope.QueryTJ.StartDate,
            endDate: $scope.QueryTJ.EndDate,
            loadType: 2
        }, function (data) {
            $scope.Data.NoSendOrderDtls = data.Data;
            if (data.Data.length > 0) {
                $scope.SelectAllShowFlag = true;
                $scope.select_all = true;
            } else {
                utility.message("未下载到医嘱数据！");
                $scope.SelectAllShowFlag = false;
                $scope.select_all = false;
            };
        });
    };

    //全选未发送医嘱
    $scope.selectAll = function () {
        if ($scope.select_all) {
            angular.forEach($scope.Data.NoSendOrderDtls, function (i) {
                i.checkNoSendFlag = false;
            });
            $scope.select_all = false;
        } else {
            angular.forEach($scope.Data.NoSendOrderDtls, function (i) {
                i.checkNoSendFlag = true;
            });
            $scope.select_all = true;
        };
    };

    //单选未发送医嘱
    $scope.selectOne = function () {
        var checkCount = 0;
        angular.forEach($scope.Data.NoSendOrderDtls, function (i) {
            if (i.checkNoSendFlag == false) {
                checkCount++;
            };
        });

        if (checkCount > 0) {
            $scope.select_all = false;
        } else {
            $scope.select_all = true;
        };
    }

    //删除未发送医嘱
    $scope.DeleteNoSendOrder = function (nsods) {
        var tmpDate = new Date(nsods.PostDate).format("yyyy-MM-dd")
        //删除未发送医嘱
        if (confirm("您确定要删除执行时间为" + tmpDate + "的《" + nsods.OrderName + "》医嘱吗?")) {
            $scope.Data.NoSendOrderDtls.splice($scope.Data.NoSendOrderDtls.indexOf(nsods), 1);
            utility.message("删除成功");
        };
    };

    //撤回已发送医嘱
    $scope.DeleteSentOrder = function (sods) {
        var tmpDate = new Date(sods.PostDate).format("yyyy-MM-dd")
        if (confirm("您确定要撤回执行时间为" + tmpDate + "的《" + sods.OrderName + "》医嘱吗?")) {
            docOrderRes.deleteSentOrders(sods, function (data) {
                if (data.ResultCode == -1) {
                    utility.msgwarning(data.ResultMessage);
                }
                else {
                    $scope.optionsSentOrder.pageInfo.CurrentPage = 1;
                    $scope.optionsSentOrder.pageInfo.PageSize = 10;
                    if ($scope.FeeNo == "") {
                        $scope.optionsSentOrder.params.feeNo = -1;
                        $scope.LoadType = 2;
                    } else {
                        $scope.optionsSentOrder.params.feeNo = $scope.FeeNo;
                        $scope.LoadType = 1;
                    }
                    $scope.optionsSentOrder.params.loadType = $scope.LoadType;
                    $scope.optionsSentOrder.search();

                    $scope.QueryOrder();

                    utility.message("撤回成功");
                };
            });
        };
    };

    //医嘱发送
    $scope.post = function () {
        if ($scope.Data.NoSendOrderDtls.length > 0) {
            $scope.NoSendIpdOrderList = {};
            $scope.NoSendIpdOrderList.NoSendIpdOrderLists = [];

            angular.forEach($scope.Data.NoSendOrderDtls, function (i) {
                $scope.NoSendIpdOrderList.NoSendIpdOrderLists.push(i);
            });

            docOrderRes.saveSendOrders($scope.NoSendIpdOrderList, function (data) {
                if (data.ResultCode == -1) {
                    utility.msgwarning(data.ResultMessage);
                }
                else {
                    $scope.optionsSentOrder.pageInfo.CurrentPage = 1;
                    $scope.optionsSentOrder.pageInfo.PageSize = 10;
                    if ($scope.FeeNo == "") {
                        $scope.optionsSentOrder.params.feeNo = -1;
                        $scope.LoadType = 2;
                    } else {
                        $scope.optionsSentOrder.params.feeNo = $scope.FeeNo;
                        $scope.LoadType = 1;
                    }
                    $scope.optionsSentOrder.params.loadType = $scope.LoadType;
                    
                    $scope.optionsSentOrder.search();

                    if ($scope.FeeNo == "") {
                        $scope.loadAllOrder();

                    } else {
                        $scope.loadOrder();
                    };

                    $scope.QueryOrder();
                    utility.message("发送成功");
                };
            });
        } else {
            utility.msgwarning("没有未发送医嘱数据，请先下载！");
            return;
        };
    };

    $scope.init();
}])