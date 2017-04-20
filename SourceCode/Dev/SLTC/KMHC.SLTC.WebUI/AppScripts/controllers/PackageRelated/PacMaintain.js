angular.module("sltcApp")
.controller("PacMaintainCtrl", ['$scope', '$state', '$stateParams', '$location', 'pacMaintainRes', 'utility', function ($scope, $state, $stateParams, $location, pacMaintainRes, utility) {
    $scope.drugList = [];
    $scope.matList = [];
    $scope.serList = [];
    $scope.ChargeGroupEntryUrl = "/angular/chargeGroList";

    $scope.init = function () {
        if ($stateParams.id) {
            $scope.isAdd = false;
            pacMaintainRes.get({ id: $stateParams.id }, function (data) {
                $scope.item = data.Data;
                _.forEach(_.where($scope.item.CHARGEITEM, { CHARGEITEMTYPE: 1 }), function (name) {
                    var drugItem = {
                        DrugId: name.CHARGEITEMID,
                        FEEITEMCOUNT: name.FEEITEMCOUNT,
                        Qty: Math.ceil(name.FEEITEMCOUNT / name.CONVERSIONRATIO),
                        COST: Math.ceil(name.FEEITEMCOUNT / name.CONVERSIONRATIO) * name.UNITPRICE,  //药品总价=取整（开药数量/转换比例）*单价
                        PrescribeUnits: name.PRESCRIBEUNITS,
                        ConversionRatio: name.CONVERSIONRATIO,
                        CNName: name.NAME,
                        MCDrugCode: name.MCCODE,
                        NSDrugCode: name.NSCODE,
                        Spec:name.SPEC,
                        UnitPrice: name.UNITPRICE,
                        CGCIID: name.CGCIID,
                        Units:name.UNITS
                    };
                    $scope.drugList.push(drugItem);
                });
                _.forEach(_.where($scope.item.CHARGEITEM, { CHARGEITEMTYPE: 2 }), function (name) {
                    var drugItem = {
                        MaterialId: name.CHARGEITEMID,
                        FEEITEMCOUNT: name.FEEITEMCOUNT,
                        Qty: name.FEEITEMCOUNT,
                        COST: name.FEEITEMCOUNT * name.UNITPRICE,
                        PrescribeUnits: name.PRESCRIBEUNITS,
                        ConversionRatio: name.CONVERSIONRATIO,
                        MaterialName: name.NAME,
                        MCMaterialCode: name.MCCODE,
                        NSMaterialCode: name.NSCODE,
                        Spec: name.SPEC,
                        UnitPrice: name.UNITPRICE,
                        CGCIID: name.CGCIID,
                        Units: name.UNITS
                    };
                    $scope.matList.push(drugItem);
                });
                _.forEach(_.where($scope.item.CHARGEITEM, { CHARGEITEMTYPE: 3 }), function (name) {
                    var drugItem = {
                        ServiceId: name.CHARGEITEMID,
                        FEEITEMCOUNT: name.FEEITEMCOUNT,
                        Qty: name.FEEITEMCOUNT,
                        COST: name.FEEITEMCOUNT * name.UNITPRICE,
                        PrescribeUnits: name.PRESCRIBEUNITS,
                        ConversionRatio: name.CONVERSIONRATIO,
                        ServiceName: name.NAME,
                        MCServiceCode: name.MCCODE,
                        NSServiceCode: name.NSCODE,
                        UnitPrice: name.UNITPRICE,
                        CGCIID: name.CGCIID,
                        Units: name.UNITS
                    };
                    $scope.serList.push(drugItem);
                });

            });

        } else {
            $scope.isAdd = true;
            $scope.item = { CHARGEITEM: [] };
        }
    };
    $scope.init();

    $scope.cancelEdit = function () {
        $location.url($scope.ChargeGroupEntryUrl);
    };

    $scope.selectedDrugs = function (item) {
        $scope.drugList = _.forEach(_.uniq(_.union($scope.drugList, item), "DrugId"), function (name) {
            name.FEEITEMCOUNT = 1;
            name.Qty = Math.ceil(name.FEEITEMCOUNT / name.ConversionRatio);
            name.COST = Math.ceil(name.FEEITEMCOUNT / name.ConversionRatio) * name.UnitPrice;  //药品总价=取整（开药数量/转换比例）*单价
        });
    }
    $scope.selectedMats = function (item) {
        $scope.matList = _.forEach(_.uniq(_.union($scope.matList, item), "MaterialId"), function (name) {
            name.FEEITEMCOUNT = 1;
            name.COST = name.UnitPrice;
        });
    }
    $scope.selectedSers = function (item) {
        $scope.serList = _.forEach(_.uniq(_.union($scope.serList, item), "ServiceId"), function (name) {
            name.FEEITEMCOUNT = 1;
            name.COST = name.UnitPrice;
        });
    }

    $scope.chargeDrugItemCount = function (drug) {
        if (drug.FEEITEMCOUNT == null || drug.FEEITEMCOUNT == '' || drug.FEEITEMCOUNT == undefined) {
            return;
        } else {
            if (drug.FEEITEMCOUNT < 0) {
                drug.Qty = Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio);
                drug.COST = Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio) * drug.UnitPrice;  //药品总价=取整（开药数量/转换比例）*单价
            } else {
                drug.Qty = Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio);
                drug.COST = Math.ceil(drug.FEEITEMCOUNT / drug.ConversionRatio) * drug.UnitPrice;  //药品总价=取整（开药数量/转换比例）*单价
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
                mat.COST = mat.FEEITEMCOUNT * mat.UnitPrice;
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
                ser.COST = ser.FEEITEMCOUNT * ser.UnitPrice;
            }
        }
    }


    $scope.deleteDrugItem = function (item) {
        if (item.CGCIID) {
                pacMaintainRes.DeleteChargeItem({ id: item.CGCIID }, function () {
                    $scope.drugList = _.reject($scope.drugList, { CNName: item.CNName });
                });
            } else {
                $scope.drugList = _.reject($scope.drugList, { CNName: item.CNName });
            }
    }
    $scope.deleteMatItem = function (item) {
        $scope.matList = _.reject($scope.matList, { MaterialName: item.MaterialName });
    }
    $scope.deleteSerItem = function (item) {
        $scope.serList = _.reject($scope.serList, { ServiceName: item.ServiceName });
    }
    $scope.save = function (item) {
        if (angular.isDefined($scope.vrForm.$error.required)) {
            for (var i = 0; i < $scope.vrForm.$error.required.length; i++) {
                utility.msgwarning($scope.vrForm.$error.required[i].$name + "为必填项！");
            }
            return;
        }

        if (angular.isDefined($scope.vrForm.$error.maxlength)) {
            for (var i = 0; i < $scope.vrForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.vrForm.$error.maxlength[i].$name + "超过设定长度！");
            }
            return;
        }
        if (angular.isDefined($scope.vrForm.$error.pattern)) {
            for (var i = 0; i < $scope.vrForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.vrForm.$error.pattern[i].$name + "格式错误！");
            }
            return;
        }
        var chargeItemList = [];
        _.forEach($scope.drugList, function (name) {
            var chargeItem = {
                CHARGEITEMID: name.DrugId,
                CHARGEITEMTYPE: 1,
                FEEITEMCOUNT: name.FEEITEMCOUNT,
                CGCIID: name.CGCIID
            };
            chargeItemList.push(chargeItem);
        });
        _.forEach($scope.matList, function (name) {
            var chargeItem = {
                CHARGEITEMID: name.MaterialId,
                CHARGEITEMTYPE: 2,
                FEEITEMCOUNT: name.FEEITEMCOUNT,
                CGCIID: name.CGCIID
            };
            chargeItemList.push(chargeItem);
        });
        _.forEach($scope.serList, function (name) {
            var chargeItem = {
                CHARGEITEMID: name.ServiceId,
                CHARGEITEMTYPE: 3,
                FEEITEMCOUNT: name.FEEITEMCOUNT,
                CGCIID: name.CGCIID
            };
            chargeItemList.push(chargeItem);
        });

        item.CHARGEITEM = chargeItemList;
        
        if (item.CHARGEITEM.length == 0) {
            utility.msgwarning("套餐不能没有收费项目！");
            return;
        } else {
            pacMaintainRes.save(item, function (data) {
                utility.message("套餐信息存档成功！");
                $location.url("/angular/chargeGroList");
            });
        }
    };
}])
.controller("chargeGroListCtrl", ['$scope', '$state', 'pacMaintainRes', 'utility', function ($scope, $state, pacMaintainRes, utility) {
    $scope.Data = {};
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: pacMaintainRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.chargeGroList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }
    }
    $scope.deleteItem = function (item) {
        if (confirm("确定删除"+"“"+item.CHARGEGROUPNAME+"”"+"套餐吗?")) {
            pacMaintainRes.delete({ id: item.CHARGEGROUPID }, function () {
                $scope.options.search();
                utility.message("删除成功");
            }, function () {
                utility.msgwarning("套餐已被使用，不能删除");
            });
        }
    };
    $scope.init();
}]);