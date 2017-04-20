angular.module("sltcApp")
.controller("costEntryCtrl", ['$scope', '$state', '$compile', 'costEntryRes', 'utility', function ($scope, $state, $compile, costEntryRes, utility) {
    $scope.Data = {};
    $scope.FeeNo = $state.params.FeeNo;
    $scope.currentResident = {};
    $scope.DeleteRec = {};
    $scope.DrugRec = [];
    $scope.MaterialRec = [];
    $scope.ServiceRec = [];

    $scope.DrugRecord = {};
    $scope.MaterialRecord = {};
    $scope.ServiceRecord = {};

    $scope.drugList = [];
    $scope.matList = [];
    $scope.serList = [];

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
            ajaxObject: costEntryRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.costEntry = data.Data;
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
        $scope.DrugRec = [];
        $scope.MaterialRec = [];
        $scope.ServiceRec = [];
        $scope.currentResident = resident;
        $scope.options.params.feeNo = resident.FeeNo;
        $scope.options.search();
    }

    $scope.deleteDrugItem = function (item) {
        $scope.drugList = _.reject($scope.drugList, { CNName: item.CNName });
    }
    $scope.deleteMatItem = function (item) {
        $scope.matList = _.reject($scope.matList, { MaterialName: item.MaterialName });
    }
    $scope.deleteSerItem = function (item) {
        $scope.serList = _.reject($scope.serList, { ServiceName: item.ServiceName });
    }


    $scope.selectedDrugs = function (allItem) {
        $scope.drugList = _.forEach(_.uniq(_.union($scope.drugList, allItem), "DrugId"), function (name) {
            name.FEEITEMCOUNT = 1;
            name.Qty = Math.ceil(name.FEEITEMCOUNT / name.ConversionRatio);
            name.COST = Math.ceil(name.FEEITEMCOUNT / name.ConversionRatio) * name.UnitPrice;  //药品总价=取整（开药数量/转换比例）*单价
            //name.COST = name.UnitPrice;
            name.TakeTime = getNowFormatData();  //考虑申报问题，暂时注释
        });
    }

    $scope.selectedMats = function (allItem) {
        $scope.matList = _.forEach(_.uniq(_.union($scope.matList, allItem), "MaterialId"), function (name) {
            name.FEEITEMCOUNT = 1;
            name.COST = name.UnitPrice;
            name.TakeTime = getNowFormatData();
        });
    }

    $scope.selectedSers = function (allItem) {
        $scope.serList = _.forEach(_.uniq(_.union($scope.serList, allItem), "ServiceId"), function (name) {
            name.FEEITEMCOUNT = 1;
            name.COST = name.UnitPrice;
            name.TakeTime = getNowFormatData();
        });
    }

    $scope.chargeDrugItemCount = function (drug) {
        if (drug.FEEITEMCOUNT == null || drug.FEEITEMCOUNT == '' || drug.FEEITEMCOUNT == undefined) {
            return;
        } else {
            if (drug.FEEITEMCOUNT < 0) {
                drug.FEEITEMCOUNT = 1;
                drug.Qty = Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio);
                drug.COST = (Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio) * drug.UnitPrice).toFixed(2);  //药品总价=取整（开药数量/转换比例）*单价
                //drug.COST = drug.UnitPrice;
            } else {
                drug.Qty = Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio);
                drug.COST = (Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio) * drug.UnitPrice).toFixed(2);  //药品总价=取整（开药数量/转换比例）*单价
                //drug.COST = drug.FEEITEMCOUNT * drug.UnitPrice;
            }
        }
    }

    $scope.chargeMatItemCount = function (mat) {
        if (mat.FEEITEMCOUNT == null || mat.FEEITEMCOUNT == '' || mat.FEEITEMCOUNT == undefined) {
            return;
        } else {
            if (mat.FEEITEMCOUNT < 0) {
                mat.FEEITEMCOUNT = 1;
                mat.COST = mat.UnitPrice;
            } else {
                mat.COST = (mat.FEEITEMCOUNT * mat.UnitPrice).toFixed(2);
            }
        }
    }

    $scope.chargeSerItemCount = function (ser) {
        if (ser.FEEITEMCOUNT == null || ser.FEEITEMCOUNT == '' || ser.FEEITEMCOUNT == undefined) {
            return;
        } else {
            if (ser.FEEITEMCOUNT < 0) {
                ser.FEEITEMCOUNT = 1;
                ser.COST = ser.UnitPrice;
            } else {
                ser.COST = (ser.FEEITEMCOUNT * ser.UnitPrice).toFixed(2);
            }
        }
    }

    $scope.deleteItem = function (item) {
        //添加未缴费 已退费状态也可以删除逻辑
        if (item.Status == 0 || item.Status == 8) {
            if (confirm("您确定要删除" + item.Name + "的费用记录吗?")) {
                $scope.DeleteRec.RecType = item.RecordType;
                $scope.DeleteRec.RecId = item.Id;
                costEntryRes.updateRecord($scope.DeleteRec, function () {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            };
        } else {
            utility.msgwarning("未缴费的项目才可以删除！");
        };
    };

    $scope.checkRes = function () {
        if (!angular.isDefined($scope.currentResident.FeeNo)) {
            utility.msgwarning("请先选择住民");
            return false;
        }
    }

    var keepGoing = true;
    $scope.CommonRec = {};

    $scope.save = function () {
        keepGoing = true;

        if (!angular.isDefined($scope.currentResident.FeeNo)) {
            utility.msgwarning("请先选择住民");
            return;
        };

        _.forEach($scope.drugList, function (name) {
            if (keepGoing) {
                if (name.FEEITEMCOUNT == null || name.FEEITEMCOUNT == '' || name.FEEITEMCOUNT == undefined) {
                    utility.msgwarning("药品《" + name.CNName + "》的数量不能是空白！");
                    $scope.DrugRec = [];
                    $scope.MaterialRec = [];
                    $scope.ServiceRec = [];
                    keepGoing = false;
                    return;
                };

                if (name.TakeTime == null || name.TakeTime == '' || name.TakeTime == undefined) {
                    utility.msgwarning("药品《" + name.CNName + "》的使用时间不能是空白！");
                    $scope.DrugRec = [];
                    $scope.MaterialRec = [];
                    $scope.ServiceRec = [];
                    keepGoing = false;
                    return;
                };

                $scope.DrugRecord.DrugId = name.DrugId;
                $scope.DrugRecord.FeeNo = $scope.currentResident.FeeNo;
                $scope.DrugRecord.CnName = name.CNName;
                $scope.DrugRecord.ConversionRatio = name.ConversionRatio;
                $scope.DrugRecord.Form = name.Form;
                $scope.DrugRecord.PrescribeUnits = name.PrescribeUnits;

                $scope.DrugRecord.DrugQty = name.FEEITEMCOUNT;
                $scope.DrugRecord.Units = name.Units;
                $scope.DrugRecord.Qty = name.Qty;
                $scope.DrugRecord.Unitprice = name.UnitPrice;
                $scope.DrugRecord.Cost = name.COST;
                $scope.DrugRecord.Dosage = name.StandardUsage;
                $scope.DrugRecord.Takeway = name.DrugUsageMode;
                if (name.Frequency == null || name.Frequency == undefined) {
                    $scope.DrugRecord.Ferq = "";
                } else {
                    $scope.DrugRecord.Ferq = name.Frequency;
                };
                $scope.DrugRecord.TakeTime = name.TakeTime;
                $scope.DrugRecord.IsNciItem = name.IsNCIItem;
                $scope.DrugRecord.Comment = name.Comment;
                $scope.DrugRecord.RecType = "D";

                $scope.DrugRec.push($scope.DrugRecord);
                $scope.DrugRecord = {};
            }
        });
        _.forEach($scope.matList, function (name) {
            if (keepGoing) {
                if (name.FEEITEMCOUNT == null || name.FEEITEMCOUNT == '' || name.FEEITEMCOUNT == undefined) {
                    utility.msgwarning("耗材《" + name.MaterialName + "》的数量不能是空白！");
                    $scope.DrugRec = [];
                    $scope.MaterialRec = [];
                    $scope.ServiceRec = [];
                    keepGoing = false;
                    return;
                };

                if (name.TakeTime == null || name.TakeTime == '' || name.TakeTime == undefined) {
                    utility.msgwarning("耗材《" + name.MaterialName + "》的使用时间不能是空白！");
                    $scope.DrugRec = [];
                    $scope.MaterialRec = [];
                    $scope.ServiceRec = [];
                    keepGoing = false;
                    return;
                };

                $scope.MaterialRecord.MaterialId = name.MaterialId;
                $scope.MaterialRecord.FeeNo = $scope.currentResident.FeeNo;
                $scope.MaterialRecord.MaterialName = name.MaterialName;
                $scope.MaterialRecord.Units = name.Units;
                $scope.MaterialRecord.Qty = name.FEEITEMCOUNT;
                $scope.MaterialRecord.Unitprice = name.UnitPrice;
                $scope.MaterialRecord.Cost = name.COST;
                $scope.MaterialRecord.TakeTime = name.TakeTime;
                $scope.MaterialRecord.IsNciItem = name.IsNCIItem;
                $scope.MaterialRecord.Comment = name.Comment;
                $scope.MaterialRecord.RecType = "M";
                $scope.MaterialRec.push($scope.MaterialRecord);
                $scope.MaterialRecord = {};
            }
        });
        _.forEach($scope.serList, function (name) {
            if (keepGoing) {
                if (name.FEEITEMCOUNT == null || name.FEEITEMCOUNT == '' || name.FEEITEMCOUNT == undefined) {
                    utility.msgwarning("服务项目《" + +name.ServiceName + "》的数量不能是空白！");
                    $scope.DrugRec = [];
                    $scope.MaterialRec = [];
                    $scope.ServiceRec = [];
                    keepGoing = false;
                    return;
                };

                if (name.TakeTime == null || name.TakeTime == '' || name.TakeTime == undefined) {
                    utility.msgwarning("服务项目《" + name.ServiceName + "》的使用时间不能是空白！");
                    $scope.DrugRec = [];
                    $scope.MaterialRec = [];
                    $scope.ServiceRec = [];
                    keepGoing = false;
                    return;
                };

                $scope.ServiceRecord.ServiceId = name.ServiceId;
                $scope.ServiceRecord.FeeNo = $scope.currentResident.FeeNo;
                $scope.ServiceRecord.ServiceName = name.ServiceName;
                $scope.ServiceRecord.Units = name.Units;
                $scope.ServiceRecord.Qty = name.FEEITEMCOUNT;
                $scope.ServiceRecord.Unitprice = name.UnitPrice;
                $scope.ServiceRecord.Cost = name.COST;
                $scope.ServiceRecord.TakeTime = name.TakeTime;
                $scope.ServiceRecord.IsNciItem = name.IsNCIItem;
                $scope.ServiceRecord.Comment = name.Comment;
                $scope.ServiceRecord.RecType = "S";
                $scope.ServiceRec.push($scope.ServiceRecord);
                $scope.ServiceRecord = {};
            }
        });


        if (keepGoing) {
            if ($scope.DrugRec.length == 0 && $scope.MaterialRec.length == 0 && $scope.ServiceRec.length == 0) {
                utility.msgwarning("没有收费项目！");
                return;
            } else {

                $scope.DrugRecs = {};
                $scope.DrugRecs.Data = $scope.DrugRec;
                $scope.CommonRec.drugRec = $scope.DrugRecs;

                $scope.MaterialRecs = {};
                $scope.MaterialRecs.Data = $scope.MaterialRec;
                $scope.CommonRec.materialRec = $scope.MaterialRecs;

                $scope.ServiceRecs = {};
                $scope.ServiceRecs.Data = $scope.ServiceRec;
                $scope.CommonRec.serviceRec = $scope.ServiceRecs;

                $scope.CommonRec.RecType = "S";
                costEntryRes.save($scope.CommonRec, function (data) {
                    $scope.CommonRec.RecType = "D";
                    costEntryRes.save($scope.CommonRec, function (data) {
                        $scope.CommonRec.RecType = "M";
                        costEntryRes.save($scope.CommonRec, function (data) {
                            $scope.drugList = [];
                            $scope.matList = [];
                            $scope.serList = [];
                            $scope.ServiceRec = [];
                            $scope.DrugRec = [];
                            $scope.MaterialRec = [];
                            $scope.options.pageInfo.CurrentPage = 1;
                            $scope.options.search();
                            utility.message("保存成功");
                        });
                    });
                });
            };
        };
    };


    $scope.getRecordDetail = function (id, type) {
        var html = '<div km-include ' +
               'km-template="Views/ChargeInput/RecordDetail.html" ' +
               'km-controller="RecordDetailCtrl" km-include-params="{id:\'' +id + '\',type:\'' +type + '\'}"</div>';
        BootstrapDialog.show({
                title: '<label class="control-label">详细信息</label>',
            closable: true,
                size: BootstrapDialog.SIZE_WIDE,
            message: html,
                onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj) ($scope);
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
}])
.controller("RecordDetailCtrl", ['$scope', 'costEntryRes', '$stateParams', function ($scope, costEntryRes, $stateParams) {
    $scope.type = "";
    $scope.currentItem = {};
    if (angular.isDefined($scope.kmIncludeParams.id) && $scope.kmIncludeParams.id != "") {
        $scope.Id = $scope.kmIncludeParams.id;
        $scope.type = $scope.kmIncludeParams.type;
    }
    if (angular.isDefined($scope.kmIncludeParams.id)) {
        costEntryRes.get({ type: $scope.kmIncludeParams.type, id: $scope.kmIncludeParams.id }, function (data) {
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
    }
}])