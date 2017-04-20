/*
创建人: 吴晓波
创建日期:2017-04-06
说明:费用管理—医嘱开立
*/
angular.module("sltcApp")
.controller("docOrderCtrl", ['$scope', '$filter', '$stateParams', '$compile', 'utility', '$state', 'cloudAdminUi', 'docOrderRes', 'residentV2Res', function ($scope, $filter, $stateParams, $compile, utility, $state, cloudAdminUi, docOrderRes, residentV2Res) {
    cloudAdminUi.handleGoToTop();
    //日期正则表达
    var date_s = /^(?:19|20)[0-9][0-9]-(?:(?:0[1-9])|(?:1[0-2]))-(?:(?:[0-2][1-9])|(?:[1-3][0-1])) (?:(?:[0-2][0-3])|(?:[0-1][0-9])):[0-5][0-9]:[0-5][0-9]$/;
    $scope.FeeNo = $state.params.FeeNo;
    $scope.RegNo = -1;
    $scope.Data = {};
    //当前用户
    $scope.curUser = [];
    $scope.Data.curUser = {};
    //住民信息
    $scope.Data.rs = {};
    //开立医嘱
    $scope.Data.docOrder = {};
    //护理险资格
    $scope.NciFlag = "";
    //医嘱记录
    $scope.Data.docOrders = {};
    //医嘱开立明细
    $scope.Data.docOrderDtls = [];
    //当前默认医生嘱托
    $scope.curNAcRemark = '';
    //当前默认途径
    $scope.curTakeWay = '';
    //已开医嘱明细
    $scope.Data.docOrderDtlsInfo = [];
    //已开医嘱对应收费项目列表
    $scope.Data.chargeItems = {};
    $scope.Data.chargeItems.FeeCode = "";
    $scope.Data.chargeItems.ChargeGroupId = "";
    $scope.Data.chargeItems.ItemType = "";

    //参数：查询医嘱明细的医嘱号
    $scope.OrderNo = "";

    //当前日期
    var currdate = new Date().format("yyyy-MM-dd");

    //新开医嘱信息
    $scope.Data.newOrders = {};
    $scope.Data.newOrders.StartDate = new Date().format("yyyy-MM-dd hh:mm:ss");
    $scope.Data.newOrders.EndDate = "";
    $scope.Data.newOrders.LOrderType = true;
    $scope.Data.newOrders.OrderType = 1;

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
    $scope.Data.SearchOrders.StartDateShow = true;
    $scope.Data.SearchOrders.EndDateShow = true;

    $scope.Data.SearchOrders.StopOption = "001";
    $scope.Data.SearchOrders.AuditOption = "001";
    $scope.Data.SearchOrders.CheckOption = "001";
    $scope.Data.SearchOrders.CancelOption = "003";
    $scope.Data.SearchOrders.TimeOption = "001";

    $scope.QueryTJ = [];
    $scope.QueryTJ.OrderType = 99;
    $scope.QueryTJ.ConfirmFlag = 99;
    $scope.QueryTJ.CheckFlag = 99;
    $scope.QueryTJ.StopFlag = 0;
    $scope.QueryTJ.CancelFlag = 0;
    $scope.QueryTJ.TimeFlag = 0;
    $scope.QueryTJ.StartDate = $scope.Data.SearchOrders.StartDate;
    $scope.QueryTJ.EndDate = $scope.Data.SearchOrders.EndDate;

    //选择项目各列表
    $scope.drugList = [];
    $scope.matList = [];
    $scope.serList = [];
    //选择套餐列表
    $scope.grpList = [];
    $scope.grpInfo = {};
    $scope.currentItem = {};
    $scope.grpItemAmount = 0;
    $scope.grpItemUnitsPrice = 0;

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
            loadType: 1,
            sortType: 1
        },
        success: function (data) {//请求成功时执行函数
            if (data.Data.length == 0) {
                utility.message("未查询到医嘱数据！");
                $scope.Data.docOrders = {};
                return;
            }
            else {
                $scope.Data.docOrders = data.Data;
            }
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

    //初始化操作
    $scope.init = function () {
        $scope.GetCurUser();
        $scope.GetRsInfo();
        $("[data-toggle='tooltip']").tooltip();
    };

    //选择住民
    $scope.txtResidentIDChange = function (resident) {
        //清空开立医嘱列表
        $scope.Data.docOrderDtls = [];

        //清空新开医嘱信息
        $scope.Data.newOrders = {};
        $scope.Data.newOrders.StartDate = new Date().format("yyyy-MM-dd hh:mm:ss");
        $scope.Data.newOrders.EndDate = "";
        $scope.Data.newOrders.LOrderType = true;
        $scope.Data.newOrders.OrderType = 1;

        $scope.curResident = resident;
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
        $scope.FeeNo = resident.FeeNo;
        $scope.RegNo = resident.RegNo;
        $scope.options.search();
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
        if ($scope.FeeNo == "") {
            utility.msgwarning("请选择住民！");
            return;
        } else {
            residentV2Res.get({ id: $scope.FeeNo }, function (data) {
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
    };

    //住民检查
    $scope.checkRes = function () {
        if (!angular.isDefined($scope.curResident.FeeNo)) {
            utility.msgwarning("请先选择住民");
            return false;
        };
    };

    //选择药品项目
    $scope.selectedDrugs = function (allItem) {
        $scope.drugList = _.forEach(_.uniq(_.union($scope.drugList, allItem), "DrugId"), function (name) {
            name.FeeItemCount = 1;
            name.TakeWay = name.DrugUsageMode;
            name.ChargeQty = Math.ceil(name.FeeItemCount / name.ConversionRatio);
            name.Amount = Math.ceil(name.FeeItemCount / name.ConversionRatio) * name.UnitPrice;  //药品总价=取整（开药数量/转换比例）*单价
        });
    };

    //选择耗材项目
    $scope.selectedMats = function (allItem) {
        $scope.matList = _.forEach(_.uniq(_.union($scope.matList, allItem), "MaterialId"), function (name) {
            name.FeeItemCount = 1;
            name.TakeWay = "";
            name.PrescribeUnits = name.Units;
            name.ChargeQty = name.FeeItemCount;
            name.Amount = name.ChargeQty * name.UnitPrice;
        });
    };

    //选择耗材项目
    $scope.selectedSers = function (allItem) {
        $scope.serList = _.forEach(_.uniq(_.union($scope.serList, allItem), "ServiceId"), function (name) {
            name.FeeItemCount = 1;
            name.TakeWay = "";
            name.PrescribeUnits = name.Units;
            name.ChargeQty = name.FeeItemCount;
            name.Amount = (name.ChargeQty * name.UnitPrice).toFixed(3);
        });
    };

    //重置选择套餐列表
    $scope.emptyGroupInfo = function () {
        $scope.currentItem = {};
        $scope.grpList = [];
    };

    //选择套餐
    $scope.selectedChageGroRec = function (items) {
        if (items.length > 0) {
            _.forEach(items, function (item) {
                var index = $scope.grpList.indexOf(item.CHARGEGROUPID);
                if (index == -1) {
                    $scope.grpInfo.grpNo = item.CHARGEGROUPID;
                    $scope.grpInfo.grpName = item.CHARGEGROUPNAME;
                    $scope.grpInfo.grpQty = 1;
                    
                    _.forEach(item.CHARGEITEM, function (itemchargeitem) {
                        if (itemchargeitem.CHARGEITEMTYPE == 1) {
                            itemchargeitem.UnitPrice = Math.ceil(itemchargeitem.FEEITEMCOUNT / itemchargeitem.CONVERSIONRATIO) * itemchargeitem.UNITPRICE;  //药品总价=取整（开药数量/转换比例）*单价
                        } else {
                            itemchargeitem.UnitPrice = itemchargeitem.FEEITEMCOUNT * itemchargeitem.UNITPRICE;
                        };
                        $scope.grpItemUnitsPrice = $scope.grpItemUnitsPrice + itemchargeitem.UnitPrice;
                        $scope.grpItemAmount = $scope.grpItemAmount + itemchargeitem.UnitPrice;
                    });

                    $scope.grpInfo.grpUnitPrice = $scope.grpItemUnitsPrice;
                    $scope.grpInfo.grpAmount = $scope.grpItemUnitsPrice;
                    $scope.grpList.push($scope.grpInfo);
                    $scope.grpItemAmount = 0;
                    $scope.grpItemUnitsPrice = 0;
                    $scope.grpInfo = {};
                };
            });
        };
    };

    //修改选择药品项目开立数量
    $scope.chargeDrugItemCount = function (drug) {
        if (drug.FeeItemCount == null || drug.FeeItemCount == '' || drug.FeeItemCount == undefined) {
            return;
        } else {
            if (drug.FeeItemCount < 0) {
                drug.FeeItemCount = 1;
                drug.ChargeQty = Math.ceil(drug.FeeItemCount / drug.ConversionRatio);
                drug.Amount = (Math.ceil(drug.FeeItemCount / drug.ConversionRatio) * drug.UnitPrice).toFixed(3);  //药品总价=取整（开药数量/转换比例）*单价
            } else {
                drug.ChargeQty = Math.ceil(drug.FeeItemCount / drug.ConversionRatio);
                drug.Amount = (Math.ceil(drug.FeeItemCount / drug.ConversionRatio) * drug.UnitPrice).toFixed(3);  //药品总价=取整（开药数量/转换比例）*单价
            };
        };
    };

    //修改选择耗材项目开立数量
    $scope.chargeMatItemCount = function (mat) {
        if (mat.FeeItemCount == null || mat.FeeItemCount == '' || mat.FeeItemCount == undefined) {
            return;
        } else {
            if (mat.FeeItemCount < 0) {
                mat.FeeItemCount = 1;
                mat.ChargeQty = mat.FeeItemCount;
                mat.Amount = mat.UnitPrice;
            } else {
                mat.ChargeQty = mat.FeeItemCount;
                mat.Amount = (mat.FeeItemCount * mat.UnitPrice).toFixed(3);
            };
        };
    };

    //修改选择服务项目开立数量
    $scope.chargeSerItemCount = function (ser) {
        if (ser.FeeItemCount == null || ser.FeeItemCount == '' || ser.FeeItemCount == undefined) {
            return;
        } else {
            if (ser.FeeItemCount < 0) {
                ser.FeeItemCount = 1;
                ser.ChargeQty = ser.FeeItemCount;
                ser.Amount = ser.UnitPrice;
            } else {
                ser.ChargeQty = ser.FeeItemCount;
                ser.Amount = (ser.FeeItemCount * ser.UnitPrice).toFixed(3);
            };
        };
    };

    //修改选择套餐开立数量
    $scope.chargeGrpCount = function (gl) {
        if (gl.grpQty == null || gl.grpQty == '' || gl.grpQty == undefined) {
            return;
        } else {
            if (gl.grpQty < 0) {
                gl.grpQty = 1;
            } else {
                gl.grpAmount = (gl.grpQty * gl.grpUnitPrice).toFixed(3);
            };
        };
    };

    //删除药品选择项目
    $scope.deleteDrugItem = function (item) {
        $scope.drugList = _.reject($scope.drugList, { CNName: item.CNName });
    };

    //删除耗材选择项目
    $scope.deleteMatItem = function (item) {
        $scope.matList = _.reject($scope.matList, { MaterialName: item.MaterialName });
    };

    //删除服务选择项目
    $scope.deleteSerItem = function (item) {
        $scope.serList = _.reject($scope.serList, { ServiceName: item.ServiceName });
    };

    //删除选择套餐
    $scope.deleteGrpItem = function (gl) {
        $scope.grpList.splice($scope.grpList.indexOf(gl), 1);
    };

    var keepGoing = true;
    $scope.addChargeItemsRecord = {};
    
    //确认添加项目
    $scope.saveItem = function () {
        keepGoing = true;
        if ($scope.drugList.length > 0 || $scope.matList.length > 0 || $scope.serList.length > 0) {
            _.forEach($scope.drugList, function (name) {
                if (keepGoing) {
                    if (name.FeeItemCount == null || name.FeeItemCount == "" || name.FeeItemCount == undefined) {
                        utility.msgwarning("药品《" + name.CNName + "》的计价数量不能是空白！");
                        $scope.addChargeItems = [];
                        keepGoing = false;
                        return;
                    };

                    $scope.addChargeItemsRecord.NOrderNo = 0;
                    $scope.addChargeItemsRecord.NOrderType = $scope.Data.newOrders.OrderType;
                    $scope.addChargeItemsRecord.NFeeCode = name.DrugId;
                    $scope.addChargeItemsRecord.NOrderName = name.CNName;
                    $scope.addChargeItemsRecord.NItemType = 1;
                    $scope.addChargeItemsRecord.NAcRemark = "";
                    $scope.addChargeItemsRecord.NPrescribeUnits = name.PrescribeUnits;
                    $scope.addChargeItemsRecord.NConversionRatio = name.ConversionRatio;
                    $scope.addChargeItemsRecord.NTakeQty = name.FeeItemCount;
                    $scope.addChargeItemsRecord.NTakeDay = $scope.Data.newOrders.Days;
                    $scope.addChargeItemsRecord.NTakeFreq = "ONCE";
                    $scope.addChargeItemsRecord.NTakeFreqQty = 1;
                    $scope.addChargeItemsRecord.NTakeWay = name.TakeWay;
                    $scope.addChargeItemsRecord.NUnits = name.Units;
                    $scope.addChargeItemsRecord.NUnitPrice = name.UnitPrice;
                    $scope.addChargeItemsRecord.NChargeQty = name.ChargeQty;
                    $scope.addChargeItemsRecord.NAmount = name.Amount;
                    $scope.addChargeItemsRecord.NFirstDayQuantity = -1;
                    $scope.addChargeItemsRecord.NStartDate = $scope.Data.newOrders.StartDate;
                    $scope.addChargeItemsRecord.NEndDate = $scope.Data.newOrders.EndDate
                    $scope.addChargeItemsRecord.FeeNo = $scope.curResident.FeeNo;
                    $scope.addChargeItemsRecord.RegNo = $scope.curResident.RegNo;

                    $scope.Data.docOrderDtls.push($scope.addChargeItemsRecord);
                    $scope.addChargeItemsRecord = {};
                };
            });

            _.forEach($scope.matList, function (name) {
                if (keepGoing) {
                    if (name.FeeItemCount == null || name.FeeItemCount == "" || name.FeeItemCount == undefined) {
                        utility.msgwarning("耗材《" + name.MaterialName + "》的计价数量不能是空白！");
                        $scope.addChargeItems = [];
                        keepGoing = false;
                        return;
                    };

                    $scope.addChargeItemsRecord.NOrderNo = 0;
                    $scope.addChargeItemsRecord.NOrderType = $scope.Data.newOrders.OrderType;
                    $scope.addChargeItemsRecord.NFeeCode = name.MaterialId;
                    $scope.addChargeItemsRecord.NOrderName = name.MaterialName;
                    $scope.addChargeItemsRecord.NItemType = 2;
                    $scope.addChargeItemsRecord.NAcRemark = "";
                    $scope.addChargeItemsRecord.NPrescribeUnits = name.Units;
                    $scope.addChargeItemsRecord.NConversionRatio = 1;
                    $scope.addChargeItemsRecord.NTakeQty = name.FeeItemCount;
                    $scope.addChargeItemsRecord.NTakeDay = $scope.Data.newOrders.Days;
                    $scope.addChargeItemsRecord.NTakeFreq = "ONCE";
                    $scope.addChargeItemsRecord.NTakeFreqQty = 1;
                    $scope.addChargeItemsRecord.NTakeWay = "";
                    $scope.addChargeItemsRecord.NUnits = name.Units;
                    $scope.addChargeItemsRecord.NUnitPrice = name.UnitPrice;
                    $scope.addChargeItemsRecord.NChargeQty = name.ChargeQty;
                    $scope.addChargeItemsRecord.NAmount = name.Amount;
                    $scope.addChargeItemsRecord.NFirstDayQuantity = -1;
                    $scope.addChargeItemsRecord.NStartDate = $scope.Data.newOrders.StartDate;
                    $scope.addChargeItemsRecord.NEndDate = $scope.Data.newOrders.EndDate
                    $scope.addChargeItemsRecord.FeeNo = $scope.curResident.FeeNo;
                    $scope.addChargeItemsRecord.RegNo = $scope.curResident.RegNo;
                    $scope.Data.docOrderDtls.push($scope.addChargeItemsRecord);
                    $scope.addChargeItemsRecord = {};
                };


            });

            _.forEach($scope.serList, function (name) {
                if (keepGoing) {
                    if (name.FeeItemCount == null || name.FeeItemCount == "" || name.FeeItemCount == undefined) {
                        utility.msgwarning("服务《" + name.ServiceName + "》的计价数量不能是空白！");
                        $scope.addChargeItems = [];
                        keepGoing = false;
                        return;
                    };

                    $scope.addChargeItemsRecord.NOrderNo = 0;
                    $scope.addChargeItemsRecord.NOrderType = $scope.Data.newOrders.OrderType;
                    $scope.addChargeItemsRecord.NFeeCode = name.ServiceId;
                    $scope.addChargeItemsRecord.NOrderName = name.ServiceName;
                    $scope.addChargeItemsRecord.NItemType = 3;
                    $scope.addChargeItemsRecord.NAcRemark = "";
                    $scope.addChargeItemsRecord.NPrescribeUnits = name.Units;
                    $scope.addChargeItemsRecord.NConversionRatio = 1;
                    $scope.addChargeItemsRecord.NTakeQty = name.FeeItemCount;
                    $scope.addChargeItemsRecord.NTakeDay = $scope.Data.newOrders.Days;
                    $scope.addChargeItemsRecord.NTakeFreq = "ONCE";
                    $scope.addChargeItemsRecord.NTakeFreqQty = 1;
                    $scope.addChargeItemsRecord.NTakeWay = "";
                    $scope.addChargeItemsRecord.NUnits = name.Units;
                    $scope.addChargeItemsRecord.NUnitPrice = name.UnitPrice;
                    $scope.addChargeItemsRecord.NChargeQty = name.ChargeQty;
                    $scope.addChargeItemsRecord.NAmount = name.Amount;
                    $scope.addChargeItemsRecord.NFirstDayQuantity = -1;
                    $scope.addChargeItemsRecord.NStartDate = $scope.Data.newOrders.StartDate;
                    $scope.addChargeItemsRecord.NEndDate = $scope.Data.newOrders.EndDate
                    $scope.addChargeItemsRecord.FeeNo = $scope.curResident.FeeNo;
                    $scope.addChargeItemsRecord.RegNo = $scope.curResident.RegNo;

                    $scope.Data.docOrderDtls.push($scope.addChargeItemsRecord);
                    $scope.addChargeItemsRecord = {};
                };


            });

            if (keepGoing) {
                $scope.drugList = [];
                $scope.matList = [];
                $scope.serList = [];
                $scope.cancelselItem();
            };
        } else {
            utility.message("请先选择项目!");
            return;
        };
    };

    var keepGrpGoing = true;
    //确认添加套餐
    $scope.saveGrp = function () {
        if ($scope.grpList.length > 0) {
            keepGrpGoing = true;
            _.forEach($scope.grpList, function (name) {
                if (keepGrpGoing) {
                    if (name.grpQty == null || name.grpQty == "" || name.grpQty == undefined) {
                        utility.msgwarning("套餐《" + name.grpName + "》的数量不能是空白！");
                        $scope.addChargeItems = [];
                        keepGrpGoing = false;
                        return;
                    };

                    $scope.addChargeItemsRecord.NOrderNo = 0;
                    $scope.addChargeItemsRecord.NOrderType = $scope.Data.newOrders.OrderType;
                    $scope.addChargeItemsRecord.NOrderName = name.grpName;
                    $scope.addChargeItemsRecord.NItemType = 4;
                    $scope.addChargeItemsRecord.NAcRemark = "";
                    $scope.addChargeItemsRecord.NPrescribeUnits = "次";
                    $scope.addChargeItemsRecord.NConversionRatio = 1;
                    $scope.addChargeItemsRecord.NTakeQty = name.grpQty;
                    $scope.addChargeItemsRecord.NTakeDay = $scope.Data.newOrders.Days;
                    $scope.addChargeItemsRecord.NTakeFreq = "ONCE";
                    $scope.addChargeItemsRecord.NTakeFreqQty = 1;
                    $scope.addChargeItemsRecord.NTakeWay = "";
                    $scope.addChargeItemsRecord.NUnits = "次";
                    $scope.addChargeItemsRecord.NUnitPrice = name.grpAmount;
                    $scope.addChargeItemsRecord.NChargeQty = name.grpQty;
                    $scope.addChargeItemsRecord.NAmount = name.grpAmount;
                    $scope.addChargeItemsRecord.NFirstDayQuantity = -1;
                    $scope.addChargeItemsRecord.NStartDate = $scope.Data.newOrders.StartDate;
                    $scope.addChargeItemsRecord.NEndDate = $scope.Data.newOrders.EndDate;
                    $scope.addChargeItemsRecord.NChargeGroupId = name.grpNo;
                    $scope.addChargeItemsRecord.FeeNo = $scope.curResident.FeeNo;
                    $scope.addChargeItemsRecord.RegNo = $scope.curResident.RegNo;

                    $scope.Data.docOrderDtls.push($scope.addChargeItemsRecord);
                    $scope.addChargeItemsRecord = {};
                };
            });

            if (keepGrpGoing) {
                $scope.currentItem = {};
                $scope.grpList = [];
                $scope.cancelselGrp();
            };
        } else {
            utility.message("请先选择套餐!");
            return;
        };
    };

    //新开医嘱
    $scope.newOrder = function () {
        if ($scope.FeeNo == "") {
            utility.msgwarning("请选择住民！");
            return;
        } else {
            if (angular.isDefined($scope.Data.newOrders.StartDate) && $scope.Data.newOrders.StartDate != null && $scope.Data.newOrders.StartDate != "") {
                if (date_s.test($scope.Data.newOrders.StartDate) == true) {
                    if ($scope.Data.newOrders.StartDate > currdate) {
                        $scope.Data.docOrder.NStartDate = $scope.Data.newOrders.StartDate;
                    } else {
                        utility.msgwarning("开始时间不能小于今天！");
                        return;
                    };
                } else {
                    utility.msgwarning("开始时间格式不正确！");
                    return;
                };
            } else {
                utility.msgwarning("开始时间不能是空白！");
                return;
            };

            if (angular.isDefined($scope.Data.newOrders.EndDate) && $scope.Data.newOrders.EndDate != null && $scope.Data.newOrders.EndDate != "") {
                if (date_s.test($scope.Data.newOrders.EndDate) == true) {
                    if ($scope.Data.newOrders.EndDate > currdate) {
                        if ($scope.Data.newOrders.StartDate < $scope.Data.newOrders.EndDate) {
                            $scope.Data.docOrder.NEndDate = $scope.Data.newOrders.EndDate;
                        } else {
                            utility.msgwarning("开始时间不能大于结束时间！");
                            return;
                        };
                    } else {
                        utility.msgwarning("结束时间不能小于今天！");
                        return;
                    };
                } else {
                    utility.msgwarning("结束时间格式不正确！");
                    return;
                };
            } else {
                if ($scope.Data.newOrders.LOrderType == true) {
                    $scope.Data.docOrder.NEndDate = $scope.Data.newOrders.EndDate;
                } else {
                    utility.msgwarning("临嘱的结束时间不能为空白！");
                    return;
                };
            };

            if ($scope.Data.docOrderDtls.length < 1000) {
                $scope.Data.docOrder.NSortNumber = 1;
                $scope.Data.docOrder.NTakeDay = $scope.Data.newOrders.Days;
                $scope.Data.docOrder.NFirstDayQuantity = -1;
                $scope.Data.docOrder.NOrderNo = -1;
                if ($scope.Data.newOrders.LOrderType == true) {
                    $scope.Data.docOrder.NOrderType = 1;
                } else {
                    $scope.Data.docOrder.NOrderType = 2;
                };
                $scope.Data.docOrderDtls.push(angular.copy($scope.Data.docOrder));
            } else {
                utility.msgwarning("一次保存的医嘱总数超过1000个，请先进行保存！");
                return;
            };
        };
    };

    //修改开立医嘱的天数
    $scope.chargeNewOrderDays = function (days) {
        if (isNumber(days)) {
            if ($scope.Data.newOrders.StartDate != "" || angular.isDefined($scope.Data.newOrders.StartDate) || $scope.Data.newOrders.StartDate != null) {
                if (days > 0) {
                    if (date_s.test($scope.Data.newOrders.StartDate) == true) {
                        if ($scope.Data.newOrders.StartDate > currdate) {
                            var currentDate = $scope.Data.newOrders.StartDate;
                            var tmpDate = Date.parse(new Date(currentDate.replace(/-/g, "/"))) + (86400000 * days);
                            $scope.Data.newOrders.EndDate = new Date(tmpDate).format("yyyy-MM-dd hh:mm:ss");
                            if (angular.isDefined($scope.Data.docOrderDtls)) {
                                if ($scope.Data.docOrderDtls.length > 0) {
                                    angular.forEach($scope.Data.docOrderDtls, function (item) {
                                        item.NTakeDay = $scope.Data.newOrders.Days;
                                        item.NEndDate = $scope.Data.newOrders.EndDate;
                                    });
                                };
                            };
                        } else {
                            utility.msgwarning("开始时间不能小于今天!");
                        };
                    } else {
                        utility.msgwarning("开始时间格式不正确!");
                    };
                } else if (days < 0) {
                    $scope.Data.newOrders.Days = '';
                    utility.msgwarning("天数不能为负数");
                };
            } else {
                utility.msgwarning("开始时间不能为空白!");
            };
        } else {
            $scope.Data.newOrders.Days = '';
            utility.msgwarning("天数格式不正确!");
        };
    };

    //删除医嘱明细
    $scope.Deletedodtls = function (dodtls) {
        if (confirm("您确定要删除此条医嘱明细吗?")) {
            $scope.Data.docOrderDtls.splice($scope.Data.docOrderDtls.indexOf(dodtls), 1);
        };
    };

    //关闭选择项目
    $scope.cancelselItem = function () {
        $("#modalNewItem").modal("toggle");
    };

    //关闭选择套餐
    $scope.cancelselGrp = function () {
        $("#modalNewGroup").modal("toggle");
    };

    //修改嘱型
    $scope.LchargeOrderType = function () {
        if ($scope.Data.newOrders.LOrderType == true) {
            $scope.Data.newOrders.LOrderType = true;
            $scope.Data.newOrders.SOrderType = false;
            $scope.Data.newOrders.OrderType = 1;
        } else {
            $scope.Data.newOrders.LOrderType = false;
            $scope.Data.newOrders.SOrderType = true;
            $scope.Data.newOrders.OrderType = 2;
            if ($scope.Data.newOrders.EndDate == "") {
                var currentDate = $scope.Data.newOrders.StartDate;
                var tmpDate = Date.parse(new Date(currentDate.replace(/-/g, "/"))) + 86400000;
                $scope.Data.newOrders.EndDate = new Date(tmpDate).format("yyyy-MM-dd hh:mm:ss");

                var dt1 = new Date($scope.Data.newOrders.StartDate.replace(/-/g, '/'));
                var dt2 = new Date($scope.Data.newOrders.EndDate.replace(/-/g, '/'));
                $scope.Data.newOrders.Days = DateDiff(dt2, dt1);
            };
        };

        if (angular.isDefined($scope.Data.docOrderDtls)) {
            if ($scope.Data.docOrderDtls.length > 0) {
                angular.forEach($scope.Data.docOrderDtls, function (item) {
                    item.NOrderType = $scope.Data.newOrders.OrderType;
                    if (item.NEndDate == null || item.NEndDate == undefined || item.NEndDate == "") {
                        item.NTakeDay = $scope.Data.newOrders.Days;
                        item.NEndDate = $scope.Data.newOrders.EndDate;
                    };
                });
            };
        };
    };

    //修改开立医嘱类型
    $scope.SchargeOrderType = function () {
        if ($scope.Data.newOrders.SOrderType == true) {
            $scope.Data.newOrders.SOrderType = true;
            $scope.Data.newOrders.LOrderType = false;
            $scope.Data.newOrders.OrderType = 2;
            if ($scope.Data.newOrders.EndDate == "") {
                var currentDate = $scope.Data.newOrders.StartDate;
                var tmpDate = Date.parse(new Date(currentDate.replace(/-/g, "/"))) + 86400000;
                $scope.Data.newOrders.EndDate = new Date(tmpDate).format("yyyy-MM-dd hh:mm:ss");
                var dt1 = new Date($scope.Data.newOrders.StartDate.replace(/-/g, '/'));
                var dt2 = new Date($scope.Data.newOrders.EndDate.replace(/-/g, '/'));
                $scope.Data.newOrders.Days = DateDiff(dt2, dt1);
            };
        } else {
            $scope.Data.newOrders.SOrderType = false;
            $scope.Data.newOrders.LOrderType = true;
            $scope.Data.newOrders.OrderType = 1;
        };

        if (angular.isDefined($scope.Data.docOrderDtls)) {
            if ($scope.Data.docOrderDtls.length > 0) {
                angular.forEach($scope.Data.docOrderDtls, function (item) {
                    item.NOrderType = $scope.Data.newOrders.OrderType;
                    if (item.NEndDate == null || item.NEndDate == undefined || item.NEndDate == "") {
                        item.NTakeDay = $scope.Data.newOrders.Days;
                        item.NEndDate = $scope.Data.newOrders.EndDate;
                    };
                });
            };
        };
    };

    //修改开立医嘱开始时间
    $scope.chargeStartDate = function () {
        if (angular.isDefined($scope.Data.newOrders.StartDate) && $scope.Data.newOrders.StartDate != null && $scope.Data.newOrders.StartDate != "") {
            if (date_s.test($scope.Data.newOrders.StartDate) == true) {
                if ($scope.Data.newOrders.StartDate > currdate) {
                    if (angular.isDefined($scope.Data.newOrders.EndDate) && $scope.Data.newOrders.EndDate != null && $scope.Data.newOrders.EndDate != "") {
                        if ($scope.Data.newOrders.StartDate < $scope.Data.newOrders.EndDate) {
                            var dt1 = new Date($scope.Data.newOrders.StartDate.replace(/-/g, '/'));
                            var dt2 = new Date($scope.Data.newOrders.EndDate.replace(/-/g, '/'));
                            $scope.Data.newOrders.Days = DateDiff(dt2, dt1);
                            if (angular.isDefined($scope.Data.docOrderDtls)) {
                                if ($scope.Data.docOrderDtls.length > 0) {
                                    angular.forEach($scope.Data.docOrderDtls, function (item) {
                                        item.NTakeDay = $scope.Data.newOrders.Days;
                                        item.NStartDate = $scope.Data.newOrders.StartDate;
                                    });
                                };
                            };
                        } else {
                            $scope.Data.newOrders.Days = "";
                            utility.msgwarning("开始时间不能大于结束时间！");
                            return;
                        };
                    } else {

                        if (isNumber($scope.Data.newOrders.Days)) {
                            if ($scope.Data.newOrders.Days > 0) {
                                var currentDate = $scope.Data.newOrders.StartDate;
                                var tmpDate = Date.parse(new Date(currentDate.replace(/-/g, "/"))) + (86400000 * $scope.Data.newOrders.Days);
                                $scope.Data.newOrders.EndDate = new Date(tmpDate).format("yyyy-MM-dd hh:mm:ss");

                                if (angular.isDefined($scope.Data.docOrderDtls)) {
                                    if ($scope.Data.docOrderDtls.length > 0) {
                                        angular.forEach($scope.Data.docOrderDtls, function (item) {
                                            item.NTakeDay = $scope.Data.newOrders.Days;
                                            item.NEndDate = $scope.Data.newOrders.EndDate;
                                        });
                                    };
                                };
                            } else if ($scope.Data.newOrders.Days < 0) {
                                $scope.Data.newOrders.Days = "";
                                utility.msgwarning("天数不能为负数!");
                                return;

                            };
                        } else if (dodtls.NTakeDay < 0) {
                            $scope.Data.newOrders.Days = "";
                            utility.msgwarning("天数格式不正确!");
                            return;
                        };

                        if (angular.isDefined($scope.Data.docOrderDtls)) {
                            if ($scope.Data.docOrderDtls.length > 0) {
                                angular.forEach($scope.Data.docOrderDtls, function (item) {
                                    item.NStartDate = $scope.Data.newOrders.StartDate;
                                });
                            };
                        };
                    };
                } else {
                    $scope.Data.newOrders.Days = "";
                    utility.msgwarning("开始时间不能小于今天！");
                    return;
                };
            } else {
                $scope.Data.newOrders.Days = "";
                utility.msgwarning("开始时间格式不正确！");
                return;
            };
        } else {
            $scope.Data.newOrders.Days = "";
            utility.msgwarning("开始时间不能是空白！");
            return;
        };
    };

    //修改开立医嘱结束时间
    $scope.chargeEndDate = function () {
        if (angular.isDefined($scope.Data.newOrders.EndDate) && $scope.Data.newOrders.EndDate != null && $scope.Data.newOrders.EndDate != "") {
            if (date_s.test($scope.Data.newOrders.EndDate) == true) {
                if ($scope.Data.newOrders.EndDate > currdate) {
                    if ($scope.Data.newOrders.StartDate < $scope.Data.newOrders.EndDate) {
                        var dt1 = new Date($scope.Data.newOrders.StartDate.replace(/-/g, '/'));
                        var dt2 = new Date($scope.Data.newOrders.EndDate.replace(/-/g, '/'));
                        $scope.Data.newOrders.Days = DateDiff(dt2, dt1);
                        if (angular.isDefined($scope.Data.docOrderDtls)) {
                            if ($scope.Data.docOrderDtls.length > 0) {
                                angular.forEach($scope.Data.docOrderDtls, function (item) {
                                    item.NTakeDay = $scope.Data.newOrders.Days;
                                    item.NEndDate = $scope.Data.newOrders.EndDate;
                                });
                            };
                        };
                    } else {
                        $scope.Data.newOrders.Days = "";
                        utility.msgwarning("开始时间不能大于结束时间！");
                        return;
                    };
                } else {
                    $scope.Data.newOrders.Days = "";
                    utility.msgwarning("结束时间不能小于今天！");
                    return;
                };
            } else {
                $scope.Data.newOrders.Days = "";
                utility.msgwarning("结束时间格式不正确！");
                return;
            };
        } else {
            if ($scope.Data.newOrders.LOrderType == true) {
                if (!angular.isDefined($scope.Data.docOrderDtls)) {
                    $scope.Data.newOrders.Days = "";
                    if ($scope.Data.docOrderDtls.length > 0) {
                        angular.forEach($scope.Data.docOrderDtls, function (item) {
                            item.NTakeDay = $scope.Data.newOrders.Days;
                            item.NEndDate = $scope.Data.newOrders.EndDate;
                        });
                    };
                };
            } else {
                $scope.Data.newOrders.Days = "";
                utility.msgwarning("临嘱的结束时间不能为空白！");
                return;
            };
        };
    };

    //修改单个开立医嘱天数
    $scope.chargeSingleOrderTakeDay = function (dodtls) {
        if (isNumber(dodtls.NTakeDay)) {
            if (dodtls.NStartDate != "" || angular.isDefined(dodtls.NStartDate) || dodtls.NStartDate != null) {
                if (dodtls.NTakeDay > 0) {
                    if (date_s.test(dodtls.NStartDate) == true) {
                        if (dodtls.NStartDate > currdate) {
                            var currentDate = dodtls.NStartDate;
                            var tmpDate = Date.parse(new Date(currentDate.replace(/-/g, "/"))) + (86400000 * dodtls.NTakeDay);
                            dodtls.NEndDate = new Date(tmpDate).format("yyyy-MM-dd hh:mm:ss");
                        } else {
                            utility.msgwarning("开始时间不能小于今天!");
                        };
                    } else {
                        utility.msgwarning("开始时间格式不正确!");
                    };
                } else if (dodtls.NTakeDay < 0) {
                    dodtls.NTakeDay = "";
                    utility.msgwarning("天数不能为负数!");
                };
            } else {
                utility.msgwarning("开始时间不能为空白!");
            };
        } else {
            dodtls.NTakeDay = "";
            utility.msgwarning("天数格式不正确!");
        };
    };

    //修改单个开立医嘱开始时间
    $scope.chargeSingleOrderStartDate = function (dodtls) {
        if (angular.isDefined(dodtls.NStartDate) && dodtls.NStartDate != null && dodtls.NStartDate != "") {
            if (date_s.test(dodtls.NStartDate) == true) {
                if (dodtls.NStartDate > currdate) {
                    if (angular.isDefined(dodtls.NEndDate) && dodtls.NEndDate != null && dodtls.NEndDate != "") {
                        if (dodtls.NStartDate < dodtls.NEndDate) {
                            var dt1 = new Date(dodtls.NStartDate.replace(/-/g, '/'));
                            var dt2 = new Date(dodtls.NEndDate.replace(/-/g, '/'));
                            dodtls.NTakeDay = DateDiff(dt2, dt1);
                        } else {
                            dodtls.NTakeDay = "";
                            utility.msgwarning("开始时间不能大于结束时间！");
                            return;
                        };
                    };
                } else {
                    dodtls.NTakeDay = "";
                    utility.msgwarning("开始时间不能小于今天！");
                    return;
                };
            } else {
                dodtls.NTakeDay = "";
                utility.msgwarning("开始时间格式不正确！");
                return;
            };
        } else {
            dodtls.NTakeDay = "";
            utility.message("开始时间不能为空白!");
            return;
        }
    };

    //修改单个开立医嘱结束时间
    $scope.chargeSingleOrderEndDate = function (dodtls) {
        if (angular.isDefined(dodtls.NEndDate) && dodtls.NEndDate != null && dodtls.NEndDate != "") {
            if (date_s.test(dodtls.NEndDate) == true) {
                if (dodtls.NEndDate > currdate) {
                    if (dodtls.NStartDate < dodtls.NEndDate) {
                        var dt1 = new Date(dodtls.NStartDate.replace(/-/g, '/'));
                        var dt2 = new Date(dodtls.NEndDate.replace(/-/g, '/'));
                        dodtls.NTakeDay = DateDiff(dt2, dt1);
                    } else {
                        dodtls.NTakeDay = "";
                        utility.msgwarning("开始时间不能大于结束时间！");
                        return;
                    };
                } else {
                    dodtls.NTakeDay = "";
                    utility.msgwarning("结束时间不能小于今天！");
                    return;
                };
            } else {
                dodtls.NTakeDay = "";
                utility.msgwarning("结束时间格式不正确！");
                return;
            };
        } else {
            if (dodtls.NOrderType == 2) {
                dodtls.NTakeDay = "";
            } else {
                dodtls.NTakeDay = "";
                utility.msgwarning("临嘱的结束时间不能为空白！");
                return;
            };
        };
    };

    //修改单个开立医嘱剂量
    $scope.chargeSingleOrderTakeQty = function (dodtls) {
        if (isNumber(dodtls.NTakeQty)) {
            if (dodtls.NTakeQty == null || dodtls.NTakeQty == '' || dodtls.NTakeQty == undefined) {
                utility.msgwarning("剂量不能为空白!");
            } else {
                if (dodtls.NTakeQty > 0) {
                    dodtls.NChargeQty = Math.ceil(dodtls.NTakeQty * dodtls.NTakeFreqQty  / dodtls.NConversionRatio);
                    dodtls.NAmount = (dodtls.NChargeQty * dodtls.NUnitPrice).toFixed(3);
                } else {
                    dodtls.NTakeQty = "";
                    utility.msgwarning("剂量必须大于0!");
                };
            };
        } else {
            dodtls.NTakeQty = "";
            utility.msgwarning("剂量格式不正确!");
        };
    };

    //修改频率
    $scope.FreqSelected = function (dodtls, item) {
        dodtls.NTakeFreq = item.FREQNO;
        dodtls.NTakeFreqQty = item.FREQQTY;
        if (isNumber(dodtls.NTakeQty)) {
            if (dodtls.NTakeQty == null || dodtls.NTakeQty == '' || dodtls.NTakeQty == undefined) {
                utility.msgwarning("剂量不能为空白!");
            } else {
                if (dodtls.NTakeQty > 0) {
                    dodtls.NChargeQty = Math.ceil(dodtls.NTakeQty * dodtls.NTakeFreqQty / dodtls.NConversionRatio);
                    dodtls.NAmount = (dodtls.NChargeQty * dodtls.NUnitPrice).toFixed(3);
                } else {
                    dodtls.NTakeQty = "";
                    utility.msgwarning("剂量必须大于0!");
                };
            };

        } else {
            dodtls.NTakeQty = "";
            utility.msgwarning("剂量格式不正确!");
        };
    };

    //修改医嘱名称
    $scope.OrderNameSelected = function (dodtls, item) {
        dodtls.NOrderNo = 0;
        dodtls.NOrderName = item.NAME;
        dodtls.NFeeCode = item.FeeCode;
        dodtls.NItemType = item.ITEMTYPE;
        dodtls.NPrescribeUnits = item.PRESCRIBEUNITS;
        dodtls.NConversionRatio = item.CONVERSIONRATIO;
        dodtls.NUnits = item.UNITS;
        dodtls.NUnitPrice = item.UNITPRICE;
        dodtls.NTakeDay = $scope.Data.newOrders.Days;
        dodtls.NTakeFreq = "ONCE";
        dodtls.NTakeFreqQty = 1;
        dodtls.NTakeWay = item.DRUGUSAGEMODE;
        dodtls.NFirstDayQuantity = -1;
        if (dodtls.NTakeQty == null || angular.isDefined(dodtls.NTakeQty) || dodtls.NTakeQty == "") {
            dodtls.NTakeQty = 1;
        };
        dodtls.NChargeQty = Math.ceil(dodtls.NTakeQty / item.CONVERSIONRATIO);
        dodtls.NAmount = (Math.ceil(dodtls.NTakeQty / item.CONVERSIONRATIO) * dodtls.NUnitPrice).toFixed(3);
        dodtls.NStartDate = $scope.Data.newOrders.StartDate;
        dodtls.NEndDate = $scope.Data.newOrders.EndDate;
        dodtls.FeeNo = $scope.curResident.FeeNo;
        dodtls.RegNo = $scope.curResident.RegNo;


        $scope.Data.docOrder.NSortNumber = 1;
        $scope.Data.docOrder.NTakeDay = $scope.Data.newOrders.Days;
        $scope.Data.docOrder.NFirstDayQuantity = -1;
        $scope.Data.docOrder.NOrderNo = -1;
        $scope.Data.docOrder.NStartDate = $scope.Data.newOrders.StartDate;
        $scope.Data.docOrder.NEndDate = $scope.Data.newOrders.EndDate;
        if ($scope.Data.newOrders.LOrderType == true) {
            $scope.Data.docOrder.NOrderType = 1;
        } else {
            $scope.Data.docOrder.NOrderType = 2;
        };
        $scope.Data.docOrderDtls.push(angular.copy($scope.Data.docOrder));
    };

    //点击某个医生嘱托
    $scope.copyCurAcRemark = function (item) {
        $scope.curNAcRemark = item.NAcRemark;
    };

    //点击某个频率
    $scope.copyCurTakeFreq = function (item) {
        $scope.curNTakeFreq = item.NTakeFreq;
    };

    //点击某个途径
    $scope.copyCurTakeWay = function (item) {
        $scope.curTakeWay = item.NTakeWay;
    };

    //复制全列医生嘱托
    $scope.copyFullColAcRemark = function () {
        if (angular.isDefined($scope.Data.docOrderDtls)) {
            if ($scope.Data.docOrderDtls.length > 0) {
                for (var i = 0; i < $scope.Data.docOrderDtls.length; i++) {
                    $scope.Data.docOrderDtls[i].NAcRemark = $scope.curNAcRemark;
                };
            };
        };
    };

    //复制全列频率
    $scope.copyFullColTakeFreq = function () {
        if (angular.isDefined($scope.Data.docOrderDtls)) {
            if ($scope.Data.docOrderDtls.length > 0) {
                for (var i = 0; i < $scope.Data.docOrderDtls.length; i++) {
                    $scope.Data.docOrderDtls[i].NTakeFreq = $scope.curNTakeFreq;
                };
            };
        };
    };

    //复制全列途径
    $scope.copyFullColTakeWay = function () {
        if (angular.isDefined($scope.Data.docOrderDtls)) {
            if ($scope.Data.docOrderDtls.length > 0) {
                for (var i = 0; i < $scope.Data.docOrderDtls.length; i++) {
                    $scope.Data.docOrderDtls[i].NTakeWay = $scope.curTakeWay;
                };
            };
        };
    };

    //医嘱开立
    $scope.save = function () {
        if ($scope.FeeNo == "") {
            utility.message("请选择住民！");
            return;
        };

        if (angular.isDefined($scope.docOrderForm.$error.maxlength)) {
            for (var i = 0; i < $scope.docOrderForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.docOrderForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                };
            };
            return;
        };

        var saveFlag = true;
        var rows = 1;
        var saverows = 0;
        if (angular.isDefined($scope.Data.docOrderDtls)) {
            if ($scope.Data.docOrderDtls.length > 0) {
                for (var i = 0; i < $scope.Data.docOrderDtls.length; i++) {
                    if ($scope.Data.docOrderDtls[i].NOrderNo != -1) {
                        saverows++;
                    };

                    if ($scope.Data.docOrderDtls[i].NOrderType == "" || $scope.Data.docOrderDtls[i].NOrderType == null || $scope.Data.docOrderDtls[i].NOrderType == undefined) {
                        utility.msgwarning("第" + rows + "行医嘱类型为必填项！");
                        saveFlag = false;
                    };

                    if ($scope.Data.docOrderDtls[i].NOrderType == 2) {
                        if ($scope.Data.docOrderDtls[i].NEndDate == "" || $scope.Data.docOrderDtls[i].NEndDate == null || $scope.Data.docOrderDtls[i].NEndDate == undefined) {
                            utility.msgwarning("第" + rows + "行临嘱的结束时间为必填项！");
                            saveFlag = false;
                        };
                    };

                    if ($scope.Data.docOrderDtls[i].NOrderNo != -1) {
                        if ($scope.Data.docOrderDtls[i].NOrderName == "" || $scope.Data.docOrderDtls[i].NOrderName == null || $scope.Data.docOrderDtls[i].NOrderName == undefined) {
                            utility.msgwarning("第" + rows + "行医嘱名称为必填项！");
                            saveFlag = false;
                        };

                        if ($scope.Data.docOrderDtls[i].NTakeQty == "" || $scope.Data.docOrderDtls[i].NTakeQty == null || $scope.Data.docOrderDtls[i].NTakeQty == undefined) {
                            utility.msgwarning("第" + rows + "行剂量为必填项！");
                            saveFlag = false;
                        };

                        if ($scope.Data.docOrderDtls[i].NTakeFreq == "" || $scope.Data.docOrderDtls[i].NTakeFreq == null || $scope.Data.docOrderDtls[i].NTakeFreq == undefined) {
                            utility.msgwarning("第" + rows + "行频率为必填项！");
                            saveFlag = false;
                        };

                        if ($scope.Data.docOrderDtls[i].NFirstDayQuantity == "" || $scope.Data.docOrderDtls[i].NFirstDayQuantity == null || $scope.Data.docOrderDtls[i].NFirstDayQuantity == undefined) {
                            utility.msgwarning("第" + rows + "行首日量为必填项！");
                            saveFlag = false;
                        };

                        if ($scope.Data.docOrderDtls[i].NStartDate > $scope.Data.docOrderDtls[i].NEndDate) {
                            utility.msgwarning("第" + rows + "开始时间不能大于结束时间！");
                            saveFlag = false;
                        };
                    } else {
                        if ($scope.Data.docOrderDtls[i].NStartDate < currdate) {
                            utility.msgwarning("第" + rows + "行开始时间不能小于今天！");
                            saveFlag = false;
                        };
                    };
                    
                    rows++;
                };

                if (saveFlag) {
                    $scope.IpdOrderList = {};
                    $scope.IpdOrderList.IpdOrderLists = [];

                    angular.forEach($scope.Data.docOrderDtls, function (i) {
                        $scope.IpdOrderList.IpdOrderLists.push(i);
                    });

                    docOrderRes.saveOrders($scope.IpdOrderList, function (data) {
                        if (data.ResultCode == -1) {
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
                            $scope.options.search();

                            $scope.Data.docOrderDtls = [];

                            if (saverows > 0) {
                                utility.message("保存成功");
                            };
                        };
                    });
                };
            } else {
                utility.message("没有医嘱信息！");
                return;
            };
        };
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
        $scope.options.search();
    };

    //审核医嘱
    $scope.AuditOrder = function (item) {
        item.ConfirmFlag = 1;
        docOrderRes.save(item, function (data) {
            if (data.ResultCode == -1) {
                item.ConfirmFlag = 0;
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
                $scope.options.search();
                utility.message("审核成功");
            };
        });
    };

    //查看医嘱
    $scope.LookUpOrder = function (item) {
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
                }else {
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

    //编辑医嘱
    $scope.EditOrder = function (item) {
        if ($scope.Data.docOrderDtls.length > 0) {
            if (confirm("您有未保存的医嘱，是否继续编辑医嘱？")) {
                $scope.Data.docOrderDtls = [];
                $scope.Data.docOrder = {};
                $scope.Data.docOrderDtls.push(angular.copy($scope.Data.docOrder));
                $scope.Data.docOrderDtls[0].NOrderNo = item.OrderNo;
                $scope.Data.docOrderDtls[0].NOrderType = item.OrderType;
                $scope.Data.docOrderDtls[0].NFeeCode = item.FeeCode;
                $scope.Data.docOrderDtls[0].NOrderName = item.OrderName;
                $scope.Data.docOrderDtls[0].NAcRemark = item.AcRemark;
                $scope.Data.docOrderDtls[0].NPrescribeUnits = item.PrescribeUnits;
                $scope.Data.docOrderDtls[0].NConversionRatio = item.ConversionRatio;
                $scope.Data.docOrderDtls[0].NTakeQty = item.TakeQty;
                $scope.Data.docOrderDtls[0].NTakeDay = item.TakeDay;
                $scope.Data.docOrderDtls[0].NTakeFreq = item.TakeFreq;
                $scope.Data.docOrderDtls[0].NTakeWay = item.TakeWay;
                $scope.Data.docOrderDtls[0].NUnits = item.Units;
                $scope.Data.docOrderDtls[0].NUnitPrice = item.UnitPrice;
                $scope.Data.docOrderDtls[0].NChargeQty = item.ChargeQty;
                $scope.Data.docOrderDtls[0].NAmount = item.Amount;
                $scope.Data.docOrderDtls[0].NFirstDayQuantity = item.FirstDayQuantity;
                $scope.Data.docOrderDtls[0].NStartDate = item.StartDate;
                $scope.Data.docOrderDtls[0].NEndDate = item.EndDate;

                $scope.Data.docOrder.NSortNumber = 1;
                $scope.Data.docOrder.NTakeDay = $scope.Data.newOrders.Days;
                $scope.Data.docOrder.NFirstDayQuantity = -1;
                $scope.Data.docOrder.NOrderNo = -1;
                $scope.Data.docOrder.NStartDate = $scope.Data.newOrders.StartDate;
                $scope.Data.docOrder.NEndDate = $scope.Data.newOrders.EndDate;
                if ($scope.Data.newOrders.LOrderType == true) {
                    $scope.Data.docOrder.NOrderType = 1;
                } else {
                    $scope.Data.docOrder.NOrderType = 2;
                };
                $scope.Data.docOrderDtls.push(angular.copy($scope.Data.docOrder));

            };
        } else {
            $scope.Data.docOrderDtls = [];
            $scope.Data.docOrder = {};
            $scope.Data.docOrderDtls.push(angular.copy($scope.Data.docOrder));
            $scope.Data.docOrderDtls[0].NOrderNo = item.OrderNo;
            $scope.Data.docOrderDtls[0].NOrderType = item.OrderType;
            $scope.Data.docOrderDtls[0].NFeeCode = item.FeeCode;
            $scope.Data.docOrderDtls[0].NOrderName = item.OrderName;
            $scope.Data.docOrderDtls[0].NAcRemark = item.AcRemark;
            $scope.Data.docOrderDtls[0].NPrescribeUnits = item.PrescribeUnits;
            $scope.Data.docOrderDtls[0].NConversionRatio = item.ConversionRatio;
            $scope.Data.docOrderDtls[0].NTakeQty = item.TakeQty;
            $scope.Data.docOrderDtls[0].NTakeDay = item.TakeDay;
            $scope.Data.docOrderDtls[0].NTakeFreq = item.TakeFreq;
            $scope.Data.docOrderDtls[0].NTakeWay = item.TakeWay;
            $scope.Data.docOrderDtls[0].NUnits = item.Units;
            $scope.Data.docOrderDtls[0].NUnitPrice = item.UnitPrice;
            $scope.Data.docOrderDtls[0].NChargeQty = item.ChargeQty;
            $scope.Data.docOrderDtls[0].NAmount = item.Amount;
            $scope.Data.docOrderDtls[0].NFirstDayQuantity = item.FirstDayQuantity;
            $scope.Data.docOrderDtls[0].NStartDate = item.StartDate;
            $scope.Data.docOrderDtls[0].NEndDate = item.EndDate;

            $scope.Data.docOrder.NSortNumber = 1;
            $scope.Data.docOrder.NTakeDay = $scope.Data.newOrders.Days;
            $scope.Data.docOrder.NFirstDayQuantity = -1;
            $scope.Data.docOrder.NOrderNo = -1;
            $scope.Data.docOrder.NStartDate = $scope.Data.newOrders.StartDate;
            $scope.Data.docOrder.NEndDate = $scope.Data.newOrders.EndDate;
            if ($scope.Data.newOrders.LOrderType == true) {
                $scope.Data.docOrder.NOrderType = 1;
            } else {
                $scope.Data.docOrder.NOrderType = 2;
            };
            $scope.Data.docOrderDtls.push(angular.copy($scope.Data.docOrder));
        };
    };

    //停止医嘱
    $scope.StopOrder = function (item) {
        if (confirm("您确定要停止《" + item.OrderName + "》的医嘱吗?")) {
            item.StopFlag = 2;
            item.StopCheckFlag = 1;
            docOrderRes.save(item, function (data) {
                if (data.ResultCode == -1) {
                    item.StopFlag = 0;
                    item.StopCheckFlag = 0;
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
                    $scope.options.search();
                    utility.message("停止成功");
                };
            });
        };
    };

    //作废医嘱
    $scope.CancelOrder = function (item) {
        if (confirm("您确定要作废《" + item.OrderName + "》的医嘱吗?")) {
            item.DeleteFlag = 1;
            docOrderRes.save(item, function (data) {
                if (data.ResultCode == -1) {
                    item.DeleteFlag = 0;
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
                    $scope.options.search();
                    utility.message("作废成功");
                };
            });
        };
    };

    //关闭医嘱详细信息
    $scope.cancelOrderDtls = function () {
        $("#modalOrderDtls").modal("toggle");
    };

    $scope.init();
}])