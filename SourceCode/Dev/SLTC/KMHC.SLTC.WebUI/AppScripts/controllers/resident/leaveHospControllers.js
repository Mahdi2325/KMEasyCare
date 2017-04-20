/*
创建人:张凯
创建日期:2016-03-10
说明: 请假
*/
angular.module("sltcApp")
.controller("leaveHospCtrl", ['$scope', '$filter', '$location', 'utility', 'leaveHospRes', 'relationDtlRes', '$state', function ($scope, $filter, $location,utility, leaveHospRes, relationDtlRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.ipd = $state.params.IpdFlag;
    $scope.lastLeaveStartDate = "";
    $scope.lastLeaveReturnDate = "";
    $scope.Data = {};
    $scope.LastData = {};
    $scope.currentPage = 1;
    $scope.currentItem = {};
    $scope.code = 'H79';

    // 当前住民
    $scope.currentResident = {}

    $scope.buttonShow = false;
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.getlastLeaveHospInfo("residentSelected");
        //获取最新一笔请假记录
        $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//加载当前住民的请假记录
        $scope.currentItem = {}//清空编辑项
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
        relationDtlRes.get({ FeeNo: $scope.currentResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
            $scope.Data.ContactList = data.Data;
        });
    }

    //上次离院记录信息
    $scope.getlastLeaveHospInfo = function (workState) {
        $scope.lastLeaveStartDate = "";
        $scope.lastLeaveReturnDate = "";
        var num = 0;
        if (workState == "residentSelected") {
            num = 0;
        }
        else if (workState == "rowSelect") {
            num = 1;
        }

        leaveHospRes.get({ FeeNo: $scope.currentResident.FeeNo, OrgId: $scope.currentResident.OrgId }, function (data) {
            if (data.Data[num] != null) {
                if (data.Data[num].StartDate != null) {
                    $scope.lastLeaveStartDate = data.Data[num].StartDate.replace("T", " ");
                }

                if (data.Data[num].ReturnDate != null) {
                    $scope.lastLeaveReturnDate = data.Data[num].ReturnDate.replace("T", " ");
                }
            }
        });
    };

    $scope.change = function () {
        var o = $("#selContName").find("option:selected");
        if (o.length > 0) {
            $scope.currentItem.ContTel = o.attr("contTel");
            $scope.currentItem.ContRel = o.attr("contrel");
        }
    }

    //获取请假记录
    //$scope.listItem = function (FeeNo, OrgId) {
    //    $scope.Data.leaveHosps = {};
    //    leaveHospRes.query({ currentPage: 1, pageSize: 10, FeeNo: FeeNo, OrgId: OrgId }, function (data) {
    //        $scope.Data.leaveHosps = data;
    //    });
    //}
    $scope.listItem = function (FeeNo, OrgId) {
        /*$scope.Data.items = {};
        $scope.options.params.feeNo = FeeNo;
        $scope.options.search();*/
        $scope.buttonShow = true;
        $scope.Data.leaveHosps = {};

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.params.feeNo = FeeNo;
        $scope.options.params.OrgId = OrgId;
        $scope.options.search();
        //leaveHospRes.get({ feeNo: FeeNo, currentPage: $scope.currentPage, pageSize: 5, OrgId: OrgId }, function (data) {

        //    $scope.Data.leaveHosps = data.Data;
        //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

        //        $scope.currentPage = curPage;
        //        $scope.listItem(FeeNo, OrgId);
        //    });
        //});
    }

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: leaveHospRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.leaveHosps = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                OrgId: ''
            }
        }

        var ss = $("#ContNamediv").width();
        $("#selContName").css('width', ss);
        $("#spanwidth").css("margin-left", ss - 20);
        $("#selContName").css('margin-left', -(ss - 20));
        $("#inputwidth").css('width', ss - 20);
    }


    //删除请假记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的请假记录吗?")) {
            leaveHospRes.delete({ id: item.Id }, function () {
                
                $scope.options.pageInfo.CurrentPage = 1;

                $scope.options.search();
                utility.message($scope.currentResident.Name + "的请假信息删除成功！");
                $scope.getlastLeaveHospInfo("residentSelected");
            });
        }
    };

    $scope.createItem = function (item) {
        //新增请假记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;

        leaveHospRes.save($scope.currentItem, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;

            $scope.options.search();
        });
        $scope.currentItem = {};
    };

    $scope.updateItem = function (item) {
        leaveHospRes.save(item, function (data) {
            $scope.currentItem = {};
        });
    };
    $scope.btncurrentItem = function () {
        $scope.currentItem = {};
        $scope.getlastLeaveHospInfo("residentSelected");
    }

    $scope.outPrint = function () {
        if ($scope.currentResident.FeeNo > 0) {
            if ($scope.Data.leaveHosps.length == 0) {
                utility.message("无导出报表数据！");
                return;
            }
            else {
                window.open("/api/Report/{0}?feeNo={1}&startDate={2}&endDate={3}".format($scope.code, $scope.currentResident.FeeNo, '', ''), "_blank");
            }
        }
        else {
            utility.message("请先选择住民！");
            return;
        }
    };

    $scope.rowSelect = function (item) {
        $scope.getlastLeaveHospInfo("rowSelect");
        $scope.currentItem = item;
        $scope.currentItem.StartDate = item.StartDate.replace("T", " ");
        $scope.currentItem.ReturnDate = item.ReturnDate.replace("T", " ");
    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }

        $scope.getlastLeaveHospInfo("residentSelected");
        utility.message($scope.currentResident.Name + "的请假信息保存成功！");
    };

    $scope.startendchage = function () {
        if (typeof ($scope.currentItem.StartDate) != "undefault" && typeof ($scope.currentItem.EndDate) != "undefault") {
            if ($scope.currentItem.StartDate > $scope.currentItem.EndDate) {
                utility.message("结束时间不能小于开始时间");
                $scope.currentItem.EndDate = "";
                return;
            }
        }
    }

    //校验上次请假记录
    $scope.checkLastLeaveHosp = function () {
        if ($scope.lastLeaveStartDate != "") {
            if ($scope.lastLeaveReturnDate == "") {
                utility.message("请完成上次请假记录");
                $scope.currentItem.StartDate = "";
                return;
            }

            if ($scope.lastLeaveReturnDate != "") {
                if ($scope.lastLeaveReturnDate > $scope.currentItem.StartDate) {
                    utility.message("处于上次请假区间，请选择大于" + $scope.lastLeaveReturnDate + "日期！");
                    $scope.currentItem.StartDate = "";
                    return;
                }
            }
        }
    }

    $scope.dateChange = function () {
        $scope.checkLastLeaveHosp();
        $scope.startendchage();
        if (typeof ($scope.currentItem.StartDate) != "undefault" && typeof ($scope.currentItem.ReturnDate) != "undefault") {
            if ($scope.currentItem.StartDate != "" && $scope.currentItem.ReturnDate != "") {
                if ($scope.currentItem.StartDate > $scope.currentItem.ReturnDate) {
                    utility.message("实际返回时间不能小于开始时间");
                    $scope.currentItem.ReturnDate = "";
                    return;
                }
                else if ($scope.currentItem.StartDate != "" && $scope.currentItem.ReturnDate != "") {
                    var date1 = Date.parse($scope.currentItem.StartDate.replace("-", "/"));    //开始时间
                    var date2 = Date.parse($scope.currentItem.ReturnDate.replace("-", "/"));    //结束时间
                    var date3 = date2 - date1;  //时间差的毫秒数
                    var days = Math.floor(date3 / (24 * 3600 * 1000))
                    var leave1 = date3 % (24 * 3600 * 1000)    //计算天数后剩余的毫秒数
                    var hours = Math.floor(leave1 / (3600 * 1000))
                    $scope.currentItem.LeHour = days * 24 + hours;
                }
            }
        }
        else {
            $scope.currentItem.LeHour = "";
        };
    };

    $scope.hrefLeaveHospList = function () {
        $location.url('/angular/LeaveHospList');
    };

    $scope.init();

}])
.controller("leaveHospListCtrl", ['$scope', '$filter', '$location', 'utility', 'leaveHospRes', '$state', function ($scope, $filter, $location,utility, leaveHospRes, $state) {
    //开始时间和结束时间
    var preMonthDate = new Date();
    //preMonthDate.setDate(1);
    //preMonthDate.setMonth(preMonthDate.getMonth());
    var preYear = preMonthDate.getFullYear();
    var preMonth = preMonthDate.getMonth();
    var lastMonth = preMonthDate.getMonth()-1;
    var currentDate = preMonthDate.getDate();
    if (currentDate > 7) {
        $scope.StartDate = new Date(preYear, preMonth, preMonthDate.getDate()-7).format("yyyy-MM-dd");
    }
    else {
        $scope.StartDate = new Date(preYear, lastMonth, getMonthDays(preYear, lastMonth) + currentDate - 7).format("yyyy-MM-dd");
    }
    $scope.EndDate = new Date(preYear, preMonth, preMonthDate.getDate()).format("yyyy-MM-dd");

    //获得某月的天数
    function getMonthDays(nowYear, myMonth) {
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }
    $scope.leaveState = "未归";
    $scope.levState = 0;
    //初始化数据源
    $scope.Data = {};
    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: leaveHospRes,//异步请求的res
        params: { sDate: $scope.StartDate, eDate: $scope.EndDate, keyWord: "", levStatus: 0 },
        success: function (data) {//请求成功时执行函数
            $scope.leaveHosps = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }

    $scope.checksDate = function () {
        if (angular.isDefined($scope.StartDate) && angular.isDefined($scope.EndDate)) {
            if (!checkDate($scope.StartDate, $scope.EndDate)) {
                utility.msgwarning("请假时间开始日期应该在请假时间截止日期之前");
                $scope.EndDate = "";
                return;
            }
        }
    }

    $scope.checkeDate = function () {
        if ($scope.StartDate != "" && $scope.EndDate != "") {
            if (angular.isDefined($scope.StartDate) && angular.isDefined($scope.EndDate)) {
                if (!checkDate($scope.StartDate, $scope.EndDate)) {
                    utility.msgwarning("请假时间截止日期应该在请假时间开始日期之后");
                    $scope.EndDate = "";
                    return;
                }
            }
        }
        else {
            utility.msgwarning("请假时间开始日期或截止日期不能为空");
            return;
        }
    }


    //删除请假记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的请假记录吗?")) {
            leaveHospRes.delete({ id: item.Id }, function () {

                $scope.options.pageInfo.CurrentPage = 1;

                $scope.options.search();
                utility.message($scope.currentResident.Name + "的请假信息删除成功！");
            });
        }
    };


    $scope.edit = function (item) {
        $location.url('/angular/LeaveHosp/{0}/{1}'.format(item.FeeNo, item.IpdFlag));
    }

    $scope.residentInOutRec = function (item) {
        $location.url('/angular/LeaveHosp/0/I');
    }
    
    $scope.searchLeavStatus = function () {
        if ($scope.levState) {
            $scope.levState = 0;
            $scope.leaveState="未归";
            $scope.options.params.sDate = $scope.StartDate;
            $scope.options.params.eDate = $scope.EndDate;
            $scope.options.params.levStatus = 0;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
        }
        else {
            $scope.levState = 1;
            $scope.leaveState = "全部";
            $scope.options.params.sDate = $scope.StartDate;
            $scope.options.params.eDate = $scope.EndDate;
            $scope.options.params.levStatus = 1;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
        }
    }


    $scope.searchInfo = function () {
        $scope.options.params.sDate = $scope.StartDate;
        $scope.options.params.eDate = $scope.EndDate;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

}]);

