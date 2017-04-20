angular.module("sltcApp")
.controller("chargeGroupEntryCtrl", ['$scope', '$state', '$compile', 'costEntryRes', 'chargeGroupEntryRes', 'chargeGroupDrugRes', 'chargeGroupMaterialRes', 'chargeGroupServiceRes', 'utility', function ($scope, $state, $compile, costEntryRes, chargeGroupEntryRes, chargeGroupDrugRes, chargeGroupMaterialRes, chargeGroupServiceRes, utility) {
    $scope.Data = {};
    $scope.currentResident = {};
    $scope.chargeItemData = {};
    $scope.currentItem = {};
    $scope.FeeNo = $state.params.FeeNo;

    //获取当前时间
    function getNowFormatData() {
        var date = new Date();
        var sep = "-";
        var sep2 = ":";
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var strdate = date.getDate();

        var strhour = date.getHours();
        var strminutes = date.getMinutes();
        var strsecond = date.getSeconds();

        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strdate >= 1 && strdate <= 9) {
            strdate = "0" + strdate;
        }

        if (strhour >= 1 && strhour <= 9) {
            strhour = "0" + strhour;
        }

        if (strminutes >= 1 && strminutes <= 9) {
            strminutes = "0" + strminutes;
        }

        if (strsecond >= 1 && strsecond <= 9) {
            strsecond = "0" + strsecond;
        }

        var currentdate = year + sep + month + sep + strdate + " " + strhour + sep2 + strminutes + sep2 + strsecond;
        return currentdate;
    }

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeGroupEntryRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.chargeGroupList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }
    }

    $scope.init();

    $scope.residentSelected = function (resident) {
        $scope.Data = {};
        $scope.RecordList = [];
        $scope.currentItem.CHARGEGROUP = "";
        $scope.ChargeGrps = [];
        $scope.currentResident = resident;
        $scope.options.params.feeNo = resident.FeeNo;
        $scope.options.search();
    }


    $scope.ChargeGrps = [];
    $scope.selectedChageGroRec = function (items) {
        $scope.CHARGEGROUPID = items.CHARGEGROUPID;
        if (items.length > 1) {
            _.forEach(items, function (item) {
                var index = $scope.ChargeGrps.indexOf(item.CHARGEGROUPID);
                if (index == -1) {
                    $scope.ChargeGrps.push(item.CHARGEGROUPID)
                    _.forEach(item.CHARGEITEM, function (itemchargeitem) {
                        itemchargeitem.TAKETIME = getNowFormatData();
                        itemchargeitem.FEENO = $scope.currentResident.FeeNo;
                        if (itemchargeitem.CHARGEITEMTYPE == 1) {
                            itemchargeitem.QTY = Math.ceil(itemchargeitem.FEEITEMCOUNT / itemchargeitem.CONVERSIONRATIO);
                            itemchargeitem.COST = Math.ceil(itemchargeitem.FEEITEMCOUNT / itemchargeitem.CONVERSIONRATIO) * itemchargeitem.UNITPRICE;  //药品总价=取整（开药数量/转换比例）*单价
                            $scope.RecordList.push(itemchargeitem);
                        } else {
                            itemchargeitem.QTY = itemchargeitem.FEEITEMCOUNT;
                            itemchargeitem.COST = itemchargeitem.FEEITEMCOUNT * itemchargeitem.UNITPRICE;
                            itemchargeitem.CONVERSIONRATIO = "";
                            $scope.RecordList.push(itemchargeitem);
                        };
                    });
                };
            });
        } else {
            var index = $scope.ChargeGrps.indexOf(items.CHARGEGROUPID);
            if (index == -1) {
                $scope.ChargeGrps.push(items.CHARGEGROUPID)
                _.forEach(items.CHARGEITEM, function (itemchargeitem) {
                    itemchargeitem.TAKETIME = getNowFormatData();
                    itemchargeitem.FEENO = $scope.currentResident.FeeNo;
                    if (itemchargeitem.CHARGEITEMTYPE == 1) {
                        itemchargeitem.QTY = Math.ceil(itemchargeitem.FEEITEMCOUNT / itemchargeitem.CONVERSIONRATIO);
                        itemchargeitem.COST = Math.ceil(itemchargeitem.FEEITEMCOUNT / itemchargeitem.CONVERSIONRATIO) * itemchargeitem.UNITPRICE;  //药品总价=取整（开药数量/转换比例）*单价
                        $scope.RecordList.push(itemchargeitem);
                    } else {
                        itemchargeitem.QTY =itemchargeitem.FEEITEMCOUNT;
                        itemchargeitem.COST = itemchargeitem.FEEITEMCOUNT * itemchargeitem.UNITPRICE; 
                        itemchargeitem.CONVERSIONRATIO = "";
                        $scope.RecordList.push(itemchargeitem);
                    };
                });
            };
        };
    };

    $scope.chargeItemCount = function (item) {
        if (item.FEEITEMCOUNT == null || item.FEEITEMCOUNT == '' || item.FEEITEMCOUNT == undefined) {
            return;
        } else {
            if (!angular.isNumber(item.FEEITEMCOUNT)) {
                item.FEEITEMCOUNT = 1;
                if (item.CHARGEITEMTYPE == 1) {
                    item.QTY = Math.ceil(item.FEEITEMCOUNT / item.CONVERSIONRATIO);
                    item.COST = Math.ceil(item.FEEITEMCOUNT / item.CONVERSIONRATIO) * item.UNITPRICE;  //药品总价=取整（开药数量/转换比例）*单价
                } else {
                    item.QTY = item.FEEITEMCOUNT;
                    item.COST = item.FEEITEMCOUNT * item.UNITPRICE;
                    item.CONVERSIONRATIO = "";
                };
                return;
            }

            if (item.FEEITEMCOUNT < 0) {
                item.FEEITEMCOUNT = 1;
                                if (item.CHARGEITEMTYPE == 1) {
                    item.QTY = Math.ceil(item.FEEITEMCOUNT / item.CONVERSIONRATIO);
                    item.COST = Math.ceil(item.FEEITEMCOUNT / item.CONVERSIONRATIO) * item.UNITPRICE;  //药品总价=取整（开药数量/转换比例）*单价
                } else {
                    item.QTY = item.FEEITEMCOUNT;
                    item.COST = item.FEEITEMCOUNT * item.UNITPRICE;
                    item.CONVERSIONRATIO = "";
                };
            }
        }
    }

    $scope.deleteGroupItem = function (item) {
        $scope.RecordList = _.reject($scope.RecordList, { NAME: item.NAME });
        if ($scope.RecordList.count = 0) {
            $scope.currentItem.CHARGEGROUP = "";
            $scope.ChargeGrps = [];
        };
    };


    //$scope.flag = false;
    //mod By Duke:CheckSave 拿掉
    //var keepGoing = true;
    //$scope.checkSave = function (items) {
    //    keepGoing = true;
    //    $scope.flag = false;
    //    _.forEach(items, function (item) {
    //        if (keepGoing) {
    //            if (item.FEEITEMCOUNT == null || item.FEEITEMCOUNT == '' || item.FEEITEMCOUNT == undefined) {
    //                $scope.flag = true;
    //                utility.msgwarning("《" + item.NAME + "》的数量不能是空白！");
    //                keepGoing = false;
    //                return;
    //            } else {
    //                if (!angular.isNumber(item.FEEITEMCOUNT)) {
    //                    $scope.flag = true;
    //                    utility.msgwarning("《" + item.NAME + "》的数量格式不正确！");
    //                    keepGoing = false;
    //                    return;
    //                };
    //            };

    //            if (item.TAKETIME == null || item.TAKETIME == '' || item.TAKETIME == undefined) {
    //                $scope.flag = true;
    //                utility.msgwarning("《" + item.NAME + "》的使用时间不能是空白！");
    //                keepGoing = false;
    //                return;
    //            };
    //        };
    //    });
    //};
    $scope.openChargeItem = function (item) {
        $("#chargeItemModal").modal("toggle");
        $scope.Data.ChargeItemList = item.ChargeItemList;
        $scope.Data.ChargeGroupName = item.CHARGEGROUPNAME;
    }

    $scope.closeWin=function()
    {
        $("#chargeItemModal").modal("toggle");
    }

    $scope.save = function () {
        if ($scope.RecordList.length == 0 || $scope.RecordList == null) {
            utility.msgwarning("没有套餐收费项目，请先至套餐管理进行维护！");
            return;
        }
        $scope.ChargeItemData = {};
        $scope.ChargeGroupRec = {};
        $scope.ChargeGroupRec.CHARGEGROUPID = $scope.CHARGEGROUPID;
        $scope.ChargeGroupRec.FeeNo = $scope.currentResident.FeeNo;
        $scope.ChargeItemData.ChargeGroupRec = $scope.ChargeGroupRec;
        $scope.ChargeItemData.ChargeItem = $scope.RecordList;
        //$scope.checkSave($scope.RecordList);
        //建议 By Duke: start end 之间的可以优化为数据$scope.RecordList一次性传过去逻辑在service里处理保存:  
        chargeGroupEntryRes.save($scope.ChargeItemData, function (data) {
            if (data.IsSuccess) {
                $scope.options.search();
                utility.message("保存成功！");
            };
        });

        //******start******
        //if (!$scope.flag) {
        //    _.forEach($scope.RecordList, function (item) {
        //        item.FeeNo = $scope.currentResident.FeeNo;
        //        if (item.CHARGEITEMTYPE == 1) {
        //            chargeGroupDrugRes.save(item, function (data) {
        //                $scope.ChargeGroupRec.ChargeGroupId = item.CHARGEGROUPID;
        //                $scope.ChargeGroupRec.feeNo = item.FeeNo;
        //                $scope.ChargeGroupRec.ChargeRecordType = item.CHARGEITEMTYPE;
        //                $scope.ChargeGroupRec.ChargeRecordId = data.Data.DRUGRECORDID;
        //                chargeGroupEntryRes.save($scope.ChargeGroupRec, function (data) {
        //                    $scope.options.search();
        //                });
        //            });
        //        } else if (item.CHARGEITEMTYPE == 2) {
        //            chargeGroupMaterialRes.save(item, function (data) {
        //                $scope.ChargeGroupRec.ChargeGroupId = item.CHARGEGROUPID;
        //                $scope.ChargeGroupRec.feeNo = item.FeeNo;
        //                $scope.ChargeGroupRec.ChargeRecordType = 2;
        //                $scope.ChargeGroupRec.ChargeRecordId = data.Data.MATERIALRECORDID;
        //                chargeGroupEntryRes.save($scope.ChargeGroupRec, function (data) {
        //                    $scope.options.search();
        //                });
        //            });
        //        } else if (item.CHARGEITEMTYPE == 3) {
        //            chargeGroupServiceRes.save(item, function (data) {
        //                $scope.ChargeGroupRec.ChargeGroupId = item.CHARGEGROUPID;
        //                $scope.ChargeGroupRec.feeNo = item.FeeNo;
        //                $scope.ChargeGroupRec.ChargeRecordType = item.CHARGEITEMTYPE;
        //                $scope.ChargeGroupRec.ChargeRecordId = data.Data.SERVICERECORDID;
        //                chargeGroupEntryRes.save($scope.ChargeGroupRec, function (data) {
        //                    $scope.options.search();
        //                });
        //            });
        //        }

        //    });
        //******end******
        $scope.currentItem.CHARGEGROUP = "";
        $scope.ChargeGrps = [];
        $scope.RecordList = [];
        utility.message("保存成功！");
        //$scope.flag = false;
        // }
    };

    $scope.DeleteRec = {}
    $scope.deleteItem = function (item) {
        var UnpaidCount = 0 //添加已退费状态也可以删除
        angular.forEach(item.ChargeItemList, function (chargeItem) {
            if (chargeItem.STATUS != 0 && chargeItem.STATUS != 8) {
                UnpaidCount = UnpaidCount + 1;
            }
        })
        if (UnpaidCount == 0) {
            if (confirm("您确定要删除" + item.NAME + "的费用记录吗?")) {
                $scope.DeleteRec.RecId = item.CHARGERECORDID;
                $scope.DeleteRec.CgcrId = item.CGCRID;
                costEntryRes.updateRecord($scope.DeleteRec, function () {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            }
        } else {
            utility.msgwarning("未缴费的项目才可以删除！");
        }

    };

    $scope.getRecordDetail = function (id, type) {
        var html = '<div km-include ' +
               'km-template="Views/ChargeInput/RecordDetail.html" ' +
               'km-controller="ChargGroupRecordDetailCtrl" km-include-params="{id:\'' + id + '\',type:\'' + type + '\'}"</div>';
        BootstrapDialog.show({
            title: '<label class="control-label">详细信息</label>',
            closable: true,
            size: BootstrapDialog.SIZE_WIDE,
            message: html,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            },
            buttons: [{
                label: '关闭',
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }

            ]
        });
    }

    $scope.checkRes = function () {
        if (!angular.isDefined($scope.currentResident.FeeNo)) {
            utility.msgwarning("请先选择住民");
            $scope.RecordList = [];
            return false;
        }
    }
}]).controller("ChargGroupRecordDetailCtrl", ['$scope', 'costEntryRes', '$stateParams', function ($scope, costEntryRes, $stateParams) {
    $scope.type = "";
    $scope.currentItem = {};
    if (angular.isDefined($scope.kmIncludeParams.id) && $scope.kmIncludeParams.id != "") {
        $scope.Id = $scope.kmIncludeParams.id;
        $scope.type = $scope.kmIncludeParams.type;
        if ($scope.type == 1) {
            $scope.type = "D";
        } else if ($scope.type == 2) {
            $scope.type = "M";
        } else if ($scope.type == 3) {
            $scope.type = "S";
        }
    }
    if (angular.isDefined($scope.kmIncludeParams.id)) {
        costEntryRes.get({ type: $scope.type, id: $scope.Id }, function (data) {
            if (data.Data != null) {
                $scope.currentItem = data.Data;
                if ($scope.type == "D") {
                    $scope.nameHeader = "药品";
                    $scope.currentItem.Name = data.Data.CnName;
                }

                if ($scope.type == "M") {
                    $scope.nameHeader = "耗材";
                    $scope.currentItem.Name = data.Data.MaterialName;
                }

                if ($scope.type == "S") {
                    $scope.nameHeader = "服务";
                    $scope.currentItem.Name = data.Data.ServiceName;
                }
            }
        });
    };
}]);