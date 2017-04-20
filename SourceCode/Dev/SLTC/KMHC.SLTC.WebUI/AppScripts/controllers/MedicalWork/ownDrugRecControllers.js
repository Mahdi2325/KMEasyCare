/*
创建人: 吴晓波
创建日期:2017-2-6
说明:住民申报费用明细
*/
angular.module("sltcApp")
.controller("ownDrugRecCtrl", ['$scope', '$filter', '$stateParams', 'ownDrugRecRes', 'chargeItemNSDrugRes', 'relationDtlRes', 'utility', '$state', function ($scope, $filter, $stateParams, ownDrugRecRes, chargeItemNSDrugRes,relationDtlRes, utility, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.curResident = { RegNo: 0, FeeNo: 0 };

    $scope.displayMode = "list";
    $scope.Keyword = "";
    $scope.ownDrugId = -1;
    $scope.Data = {};
    $scope.currentItem = {};
    $scope.Data.OwnDrugRecList = [];
    $scope.Data.OwnDrugDtlList = [];
    $scope.Data.NsDrugList = [];
    $scope.Data.EditNsDrugList = [];
    $scope.Data.NsDrugList.Qty = 1;
    $scope.Data.PreOwnDrugDtlList = [];
    $scope.buttonShow = false;
    $scope.isAdd = true;
    $scope.nsDrug = {};
    $scope.nsDrug.Qty = 1;
    $scope.keyword = "";
    $scope.EditOwnDrugId = "";

    $scope.residentSelected = function (resident) {
        $scope.curResident = resident;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = resident.FeeNo;
        $scope.FeeNo = resident.FeeNo;
        $scope.options.search();

        //获取担保人信息
        relationDtlRes.get({ FeeNo: $scope.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
            $scope.Data.ContactList = data.Data;
        });

        if (angular.isDefined(resident.FeeNo)) {
            $scope.buttonShow = true;
        }
    };

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: ownDrugRecRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.OwnDrugRecList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        };

        $scope.optionsDtl = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: ownDrugRecRes,//异步请求的res
            params: { FeeNo: $scope.curResident.FeeNo == "" ? -1 : $scope.curResident.FeeNo, OwnDrugId: $scope.ownDrugId },
            success: function (data) {//请求成功时执行函数
                $scope.Data.OwnDrugDtlList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };

        $scope.optionsNsDrug = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: chargeItemNSDrugRes,//异步请求的res
            params: { status:0,keyWord: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.NsDrugList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };

        //获取当前登录用户信息
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem.RecrodBy = $scope.curUser.EmpNo;
            $scope.currentItem.OpertorName = $scope.curUser.EmpName;
        };

        //获取担保人信息
        relationDtlRes.get({ FeeNo: $scope.curResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
            $scope.Data.ContactList = data.Data;
        });
    };

    //获取收药人信息
    $scope.staffSelected = function (item) {
        $scope.currentItem.RecordBy = item.EmpNo;
        $scope.currentItem.OpertorName = item.EmpName;
    }


    //查询药物
    $scope.searchNsDrug = function () {
        $scope.optionsNsDrug.pageInfo.CurrentPage = 1;
        $scope.optionsNsDrug.pageInfo.PageSize = 10;
        if (!angular.isDefined($scope.keyword)) {
            $scope.keyword = "";
        }
        $scope.optionsNsDrug.params.keyWord = $scope.keyword;
        $scope.optionsNsDrug.search();
    };

    //选择药品
    $scope.rowSelect = function (item) {
        $scope.isAdd = true;
        $scope.editName = "确定添加";
        $scope.nsDrug = item;
        if ($scope.nsDrug.Qty == undefined || $scope.nsDrug.Qty == "" || $scope.nsDrug.Qty == null) {
            $scope.nsDrug.Qty = 1;
        };
    };
    
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
    };
    
    //打开新增自带药品页面
    $scope.openWin = function (item) {
        if (item) {
            $scope.EditOwnDrugId = item.OwnDrugId;
            $scope.currentItem.OpertorTime = item.OpertorTime;
            $scope.currentItem.OpertorName = item.OpertorName;
            $scope.currentItem.SponsorName = item.SponsorName;
            $scope.currentItem.Reason = item.Reason;

            $scope.optionsDtl.pageInfo.CurrentPage = 1;
            $scope.optionsDtl.pageInfo.PageSize = 10;
            $scope.optionsDtl.params.FeeNo = $scope.FeeNo;
            $scope.optionsDtl.params.OwnDrugId = item.OwnDrugId;
            $scope.optionsDtl.search();
        }
        else {
            $scope.EditOwnDrugId = 0;
            $scope.optionsDtl.pageInfo.CurrentPage = 1;
            $scope.optionsDtl.pageInfo.PageSize = 10;
            $scope.optionsDtl.params.FeeNo = $scope.FeeNo;
            $scope.optionsDtl.params.OwnDrugId = -1;
            $scope.optionsDtl.search();
            $scope.currentItem = { OpertorName: $scope.curUser.EmpName };
            $scope.currentItem.OpertorTime = getNowFormatData();
        };
        $scope.displayMode = "edit";
    };

    $scope.deleteItem = function (item) {
        if (confirm("确定删除该自带药品记录吗?")) {
            ownDrugRecRes.delete({ id: item.OwnDrugId }, function () {
                $scope.Data.OwnDrugRecList.splice($scope.Data.OwnDrugRecList.indexOf(item), 1);
                $scope.options.search();
                utility.message("删除成功");
            });
        };
    };

    $scope.savePreOwnDrugDtl = function () {
        $scope.Data.OwnDrugDtlList = $scope.Data.PreOwnDrugDtlList;
        $scope.closeWin();
    };

    //取消操作
    $scope.cancelOwnDrugRec = function () {
        $scope.displayMode = "list";
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = $scope.FeeNo;
        $scope.options.search();
    };

    //关闭弹出窗体
    $scope.closeWin = function () {
        $scope.keyword = "";
        $("#NsDrugModal").modal("toggle");
        $scope.displayMode = "edit";
    };

    //弹出添加或编辑药物窗体
    $scope.openWin_vdr = function (item) {
        $("#NsDrugModal").modal("toggle");
        $scope.optionsNsDrug.pageInfo.CurrentPage = 1;
        $scope.optionsNsDrug.pageInfo.PageSize = 10;
        $scope.optionsNsDrug.params.keyWord = "";
        $scope.optionsNsDrug.search();

        if ($scope.Data.OwnDrugDtlList != undefined || $scope.Data.OwnDrugDtlList != null) {
            $scope.Data.PreOwnDrugDtlList = $scope.Data.OwnDrugDtlList;
        };

        $scope.displayMode = "edit";
    };

    //删除处方里的药品
    $scope.deleteOwnDrugPre = function (item) {
        $scope.isAdd = true;
        $scope.editName = "确定添加";
        $scope.Data.PreOwnDrugDtlList.splice($scope.Data.PreOwnDrugDtlList.indexOf(item), 1);
        utility.message("删除成功");
    };

    $scope.editOwnDrugPre = function (item) {
        $scope.isAdd = false;
        $scope.editName = "确定修改";

        chargeItemNSDrugRes.get({ id: item.DrugId }, function (data) {
            $scope.EditNsDrugList = data.Data;
            $scope.nsDrug = item;
            $scope.nsDrug.Spec = $scope.EditNsDrugList.Spec;
            $scope.nsDrug.DrugUsageMode = $scope.EditNsDrugList.DrugUsageMode;
            $scope.nsDrug.DrugUsage = $scope.EditNsDrugList.DrugUsage;
            $scope.nsDrug.Attention = $scope.EditNsDrugList.Attention;
            $scope.nsDrug.AttentionOldMan = $scope.EditNsDrugList.AttentionOldMan;
        });
    };

    //添加保存处方的一条药品信息
    $scope.addOwnDrugDtl = function (item) {
        var isExit = false;
        if (angular.isDefined($scope.medForm.$error.required)) {
            for (var i = 0; i < $scope.medForm.$error.required.length; i++) {
                utility.msgwarning($scope.medForm.$error.required[i].$name + "为必填项!");
                if (i > 1) {
                    return;
                }
            }
            return;
        };

        if (item.CNName == undefined || item.CNName == "" || item.CNName == null) {
            utility.msgwarning("请选择一个药品进行添加!");
            return;
        };

        angular.forEach($scope.Data.PreOwnDrugDtlList, function (preOwnDrug) {
            if (preOwnDrug.CNName == item.CNName) {
                isExit = true;
            };
        });


        if ($scope.isAdd) {
            if (isExit) {
                utility.msgwarning("《" + item.CNName + "》药品已存在,点击编辑进行修改!");
                return;
            } else {
                $scope.Data.PreOwnDrugDtlList.push(item);
                $scope.editName = "确定添加";
                utility.message("添加成功");
            };
        }
        else {
            $scope.isAdd = true;
            $scope.editName = "确定添加"
            utility.message("修改成功");
        };
    };

    //保存自带药品记录
    $scope.saveOwnDrugDec = function (item) {
        if (angular.isDefined($scope.vrForm.$error.required)) {
            for (var i = 0; i < $scope.vrForm.$error.required.length; i++) {
                utility.msgwarning($scope.vrForm.$error.required[i].$name + "为必填项!");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.vrForm.$error.maxlength)) {
            for (var i = 0; i < $scope.vrForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.vrForm.$error.maxlength[i].$name + "超过设定长度!");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.vrForm.$error.pattern)) {
            for (var i = 0; i < $scope.vrForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.vrForm.$error.pattern[i].$name + "格式错误!");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        $scope.OwnDrugDtlList = {};
        $scope.OwnDrugDtlList.OwnDrugDtlLists = [];

        if (angular.isDefined(item)) {
            item.OwnDrugId = $scope.EditOwnDrugId;
            if (angular.isDefined($scope.Data.OwnDrugDtlList)) {
                if ($scope.Data.OwnDrugDtlList.length > 0) {
                    item.FeeNo = $scope.FeeNo;
                    ownDrugRecRes.save(item, function (data) {
                        $scope.OwnDrugDtlList.UpdateOwnDrugId = data.Data.OwnDrugId;
                        angular.forEach($scope.Data.OwnDrugDtlList, function (i) {
                            i.OwnDrugId = data.Data.OwnDrugId;
                            $scope.OwnDrugDtlList.OwnDrugDtlLists.push(i);
                        });
                        ownDrugRecRes.SaveOwnDrugDtl($scope.OwnDrugDtlList, function (data) {
                            $scope.Data.OwnDrugDtlList = [];
                            $scope.Data.NsDrugList = [];
                            $scope.Data.NsDrugList.Qty = 1;
                            $scope.Data.PreOwnDrugDtlList = [];
                            $scope.OwnDrugDtlList = {};
                            $scope.OwnDrugDtlList.OwnDrugDtlLists = [];
                            $scope.cancelOwnDrugRec();
                            utility.message("保存成功");
                        });
                    });
                } else {
                    utility.msgwarning("没有自带药品信息,请点击新增药品进行添加!");
                    return;
                };
            };
        };
    };
    $scope.init();
}])