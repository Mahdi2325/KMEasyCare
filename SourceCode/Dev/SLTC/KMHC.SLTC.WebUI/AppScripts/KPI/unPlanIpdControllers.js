/*
创建人:张凯
创建日期:2016-03-16
说明: 指标-非计划住院
*/
angular.module("sltcApp")
.controller("unPlanIpdCtrl", ['$scope', 'dictionary', 'utility', 'unPlanIpdRes', 'relationDtlRes', 'visitHospitalRes', '$state', function ($scope, dictionary, utility, unPlanIpdRes, relationDtlRes, visitHospitalRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.lastInDate = "";
    $scope.lastOutDate = "";
    $scope.curUser = utility.getUserInfo();
    $scope.currentItem = {
        RecordBy: $scope.curUser.EmpNo
    };
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息


        $scope.options.params.feeNo = $scope.currentResident.FeeNo;
        $scope.options.params.regNo = $scope.currentResident.RegNo;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();

       // $scope.listItem($scope.currentResident.RegNo, $scope.currentResident.FeeNo);//加载当前住民的非计划住院记录
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo
        };
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }

        relationDtlRes.get({ FeeNo: $scope.currentResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
            $scope.Data.ContactList = data.Data;
        });
        $scope.getLastUnPlans("newData");
    }
    //获取非计划住院
    //$scope.listItem = function (regNo, feeNo) {
    //    $scope.Data.UnPlans = [];
    //    unPlanIpdRes.get({ regNo: regNo, feeNo: feeNo }, function (data) {
    //        if (angular.isDefined(data.Data)) {
    //            $scope.Data.UnPlans = data.Data;
    //            if ($scope.Data.UnPlans.length > 0) {
    //                $scope.currentItem = $scope.Data.UnPlans[0];
    //                $scope.getLastUnPlans("oldData");
    //            }
    //        }
    //    });
    //}

    $scope.init = function () {
        $scope.Data.UnPlans = [];
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: unPlanIpdRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.UnPlans = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                regNo: 0,
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }

        var ss = $("#ContNamediv").width();
        $("#selVisitorName").css('width', ss);
        $("#spanwidth").css("margin-left", ss - 20);
        $("#selVisitorName").css('margin-left', -(ss - 20));
        $("#inputwidth").css('width', ss - 20);
        $scope.Info = {};
        visitHospitalRes.get({}, function (data) {
            $scope.Info.VisitHosp = data.Data;
        });
    }

    $scope.change = function () {
        var o = $("#selVisitorName").find("option:selected");
        if (o.length > 0) {
            $scope.currentItem.EscortRelation = o.attr("contrel");
        }
    }


    //获取最新一笔非计划住院
    $scope.getLastUnPlans = function (workState)
    {
        $scope.lastInDate = "";
        $scope.lastOutDate = "";
        var num = 1;
        if (workState == "newData") {
            num = 0;
        }
        else if (workState == "oldData") {
            num = 1;
        }
        unPlanIpdRes.get({ regNo: $scope.currentResident.RegNo, feeNo:$scope.currentResident.FeeNo }, function (data) {
            if (data.Data[num] != null) {
                if (data.Data[num].InDate != null) {
                    $scope.lastInDate = data.Data[num].InDate.replace("T", " ");
                }

                if (data.Data[num].OutDate != null) {
                    $scope.lastOutDate = data.Data[num].OutDate.replace("T", " ");
                }
            }
        });
    }




    //清除文本输入框
    $scope.clearItem = function () {
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo
        };
        $scope.getLastUnPlans("newData");
    }



    //删除非计划住院
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的非计划住院吗?")) {
            unPlanIpdRes.delete({ id: item.Id }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();

                //$scope.Data.UnPlans.splice($scope.Data.UnPlans.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的非计划住院信息删除成功！");
                $scope.clearItem();

            });
        }
    };

    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.RecordBy = item.EmpNo;
    }

    $scope.createItem = function (item) {
        //新增非计划住院，得到住民ID
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        unPlanIpdRes.save($scope.currentItem, function (data) {
            $scope.options.search();
            //$scope.Data.UnPlans.push(data.Data);
        });
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo
        };
    };

    $scope.updateItem = function (item) {
        unPlanIpdRes.save(item, function (data) {
            //angular.copy(data.Data, item);
        });
        $scope.currentItem = {
            RecordBy: $scope.curUser.EmpNo
        };
    };

    $scope.rowSelect = function (item) {
        $scope.getLastUnPlans("oldData");
        $scope.currentItem = item;
    };

    $scope.saveEdit = function (item) {

        if (!angular.isDefined($scope.currentItem.RecordBy)) {
            utility.msgwarning("填写人员为必填项！");
            return;
        }
        else {
            if ($scope.currentItem.RecordBy == "") {
                utility.msgwarning("填写人员为必填项！");
                return;
            }
        }

        if (angular.isDefined($scope.unPlanfrom.$error.required)) {
            for (var i = 0; i < $scope.unPlanfrom.$error.required.length; i++) {
                utility.msgwarning($scope.unPlanfrom.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.unPlanfrom.$error.maxlength)) {
            for (var i = 0; i < $scope.unPlanfrom.$error.maxlength.length; i++) {
                utility.msgwarning($scope.unPlanfrom.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined(item.FeeNo)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的非计划住院信息保存成功！");
        $scope.getLastUnPlans("newData");
    };

    $scope.dateChange = function () {
        $scope.checkLastUnPlanIpd();
        if (angular.isString($scope.currentItem.OutDate) && angular.isString($scope.currentItem.InDate)) {
            var indate = new Date($scope.currentItem.InDate);
            var outDate = new Date($scope.currentItem.OutDate);
            var days = indate.dateDiff('d', outDate);
            if (days < 0) {
                $scope.currentItem.OutDate = "";
                utility.message("出院日期不能小于住院日期！");
                return;
            }
            $scope.currentItem.IpdDays = days;
        }
        $scope.autoOutFlag();
    };

    $scope.autoOutFlag = function () {
        if (angular.isString($scope.currentItem.OutDate)) {
            if ($scope.currentItem.OutDate != "") {
                $scope.currentItem.OutFlag = true;
            }
            else {
                $scope.currentItem.OutFlag = false;
                $scope.currentItem.OutReason = "";
            }
        }
        else {
            $scope.currentItem.OutFlag = false;
            $scope.currentItem.OutReason = "";
        }
    }

    $scope.checkOutFlag = function () {
        if ($scope.currentItem.OutFlag) {
            $scope.currentItem.OutDate = $scope.getNowFormatDate();
            $scope.dateChange();
        }
        else {
            $scope.currentItem.OutDate = "";
            $scope.currentItem.OutReason = "";
            $scope.currentItem.IpdDays = "";
        }
    }

    $scope.checkLastUnPlanIpd = function () {
        if ($scope.lastInDate != "") {
            if ($scope.lastOutDate == "") {
                utility.message("当前住民非计划住院中，请完成上次非计划住院记录(出院信息)");
                $scope.currentItem.InDate = "";
                return;
            }

            if ($scope.lastOutDate != "") {
                if ($scope.lastOutDate > $scope.currentItem.InDate) {
                    utility.message("处於上次非计划住院区间，请选择大于" + $scope.lastOutDate + "日期！");
                    $scope.currentItem.InDate = "";
                    return;
                }
            }
        }
    }


    $scope.getNowFormatDate = function () {
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        if (date.getHours >= 0 && date.getHours <= 9)
        {
            date.getHours = "0" + date.getHours;
        }
        if (date.getMinutes >= 0 && date.getMinutes <= 9)
        {
            date.getMinutes = "0" + date.getMinutes;
        }
        if (date.getSeconds >= 0 && date.getSeconds <= 9) {
            date.getSeconds = "0" + date.getSeconds;
        }

        var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
                + " " + date.getHours() + seperator2 + date.getMinutes()
                + seperator2 + date.getSeconds();
        return currentdate;
    }

    $scope.init();
}]);
