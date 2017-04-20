/*****************************************************************************
 * Filename: multidisciplinaryCarePlan
 * Creator:	Lei Chen
 * Create Date: 2016-03-13 
 * Modifier:
 * Modify Date:
 * Description:跨专业照护计划
 ******************************************************************************/
function DateDiff(endDate, startDate) {
    if (isEmpty(endDate) || isEmpty(startDate)) {
        return '';
    }
    var s1 = new Date(startDate);
    var s2 = new Date(endDate);
    var total = (s2 - s1) / 1000;
    var day = parseInt(total / (24 * 60 * 60));//计算整数天数
    return day;
}

function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = year + seperator1 + month + seperator1 + strDate;
    return currentdate;
}
angular.module("sltcApp")
.controller("carePlanListCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'carePlanListRes', 'carePlanDetailRes', 'carePlanGoalRes', 'carePlanActivityRes','utility',
    function ($scope, $http, $location, $state, dictionary, carePlanListRes, carePlanDetailRes, carePlanGoalRes, carePlanActivityRes, utility) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.buttonshow = false;

        $scope.maxErrorTips = 3;
        $scope.Data = {};
        $scope.Data.NSCPL = {};
        $scope.Data.CarePlanList = {};
        $scope.care_div = "ui-inline-hide";
        $scope.Data.Filter =
                {
                    "Id": 1, "ItemType": '', "LevelPR": '', "Category": "", "RegNO": null, "Date": '', 'Name': '', 'CurrentPage': 0, 'PageSize': 0, 'TotalRecords': 0
                };

        //当前住民
        $scope.currentResident = {};

        $scope.init = function () {
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: carePlanDetailRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.CarePlanList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        }

        

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.currentResident = resident;
            $scope.currentResident.FeeNo = resident.FeeNo;
            $scope.currentResident.Name = resident.Name;
            $scope.currentResident.RegNo = resident.RegNo;
     
            $scope.Data.CarePlanList = {};
            $scope.buttonshow = true;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.params.FeeNo = $scope.currentResident.FeeNo;
            $scope.care_div = "ui-inline-hide";
            $scope.options.search();
        };

        $scope.ShowDetail = function (seqNo) {
            $scope.care_div = "ui-inline-show";
            carePlanGoalRes.get({ seqNo: seqNo }, function (data) {
                $scope.Data.CarePlanGoalList = data.Data;
            });
            carePlanActivityRes.get({ seqNo: seqNo }, function (data) {
                $scope.Data.CarePlanActivityList = data.Data;
            });
        }
        $scope.ChangeFinishFlag = function (Item) {
            if (Item.FINISHFLAG) {
                Item.FINISHDATE = new Date();
            } else {
                Item.FINISHDATE = null;
                Item.UNFINISHREASON = "";
            }
        }

        $scope.DeletePlan = function (Item) {
            if (confirm("删除该计划将同时删除所有关联照护目标及措施,确定要删除吗?")) {
                carePlanDetailRes.delete({ id: Item.SEQNO }, function (data) {
                    if (data.ResultCode == 0) {

                        $scope.Data.CarePlanList = {};
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.pageInfo.PageSize = 10;
                        $scope.options.params.FeeNo = $scope.currentResident.FeeNo;
                        $scope.care_div = "ui-inline-hide";
                        $scope.options.search();

                       // $scope.Data.CarePlanList.splice($scope.Data.CarePlanList.indexOf(Item), 1);
                        $scope.care_div = "ui-inline-hide";
                    }
                });
            }
        }

        $scope.SaveGoalAndActivity = function () {
            if (!$scope.Validation()) { return; }
            if ($scope.Data.CarePlanGoalList.length > 0) {
                angular.forEach($scope.Data.CarePlanGoalList, function (data, index, array) {
                    carePlanGoalRes.save(data, function (data) {
                        if (data.ResultCode == 0) {
                        } else {
                            alert(data.ResultMessage);
                        }
                    });
                });
            }
            if ($scope.Data.CarePlanActivityList.length > 0) {
                angular.forEach($scope.Data.CarePlanActivityList, function (data, index, array) {
                    carePlanActivityRes.save(data, function (data) {
                        if (data.ResultCode == 0) {
                        } else {
                            alert(data.ResultMessage);
                        }
                    });
                });
            }
            utility.message("保存成功!");
        }

        $scope.Preview = function (seqno)
        {
            window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("H10Report", seqno, "", ""), "_blank");
        }

        $scope.Validation = function () {
            var errorTips = 0;
            if (angular.isDefined($scope.myForm.$error.required)) {
                var msg = "";
                for (var i = 0; i < $scope.myForm.$error.required.length; i++) {
                    msg = $scope.myForm.$error.required[i].$name + " 为必填项";
                    utility.msgwarning(msg);
                    errorTips++;
                    if (errorTips >= $scope.maxErrorTips) {
                        return false;
                    }
                }
            }

            if (angular.isDefined($scope.myForm.$error.maxlength)) {
                var msg = "";
                for (var i = 0; i < $scope.myForm.$error.maxlength.length; i++) {
                    msg = $scope.myForm.$error.maxlength[i].$name + "超过设定长度 ";
                    utility.msgwarning(msg);
                    errorTips++;
                    if (errorTips >= $scope.maxErrorTips) {
                        return false;
                    }
                }
            }
            if (errorTips > 0) { return false; }
            return true;
        }

        $scope.init();
    }]);

angular.module("sltcApp")
.controller("carePlanDetailCtr", ['$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'carePlanDetailRes', 'carePlanGoalRes', 'carePlanActivityRes',
    function ($scope, $http, $location, $state, utility, cloudAdminUi, carePlanDetailRes, carePlanGoalRes, carePlanActivityRes) {

        var feeNo = $state.params.feeNo;
        var regName = $state.params.regName;
        var regNo = $state.params.regNo;
        $scope.Data = {};
        //当前住民
        $scope.currentResident = {};

        $scope.Data.RegNo = regNo;
        $scope.Data.FeeNo = feeNo;
        $scope.Data.RegName = regName;


        $scope.currentResident.FeeNo = feeNo;
        $scope.currentResident.Name = regName;
        $scope.currentResident.RegNo = regNo;

        $scope.buttonshow = false;

        $scope.maxErrorTips = 3;
      
        $scope.Data.NSCPL = {};
        $scope.Data.CarePlanList = {};
        $scope.care_div = "ui-inline-hide";
        $scope.Data.Filter =
                {
                    "Id": 1, "ItemType": '', "LevelPR": '', "Category": "", "RegNO": null, "Date": '', 'Name': '', 'CurrentPage': 0, 'PageSize': 0, 'TotalRecords': 0
                };

    
        $scope.init = function () {

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: carePlanDetailRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.CarePlanList = data.Data;
                    $scope.buttonshow = true;

                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    FeeNo: feeNo
                }
            }
        }

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.currentResident = resident;
            $scope.currentResident.FeeNo = resident.FeeNo;
            $scope.currentResident.Name = resident.Name;
            $scope.currentResident.RegNo = resident.RegNo;

            $scope.Data.RegNo = $scope.currentResident.RegNo;
            $scope.Data.FeeNo = $scope.currentResident.FeeNo;
            $scope.Data.RegName = $scope.currentResident.Name;

            $scope.Data.CarePlanList = {};
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.params.FeeNo = $scope.currentResident.FeeNo;
            $scope.care_div = "ui-inline-hide";
            $scope.options.search();
        };

        $scope.ShowDetail = function (seqNo) {
            $scope.care_div = "ui-inline-show";
            carePlanGoalRes.get({ seqNo: seqNo }, function (data) {
                $scope.Data.CarePlanGoalList = data.Data;
            });
            carePlanActivityRes.get({ seqNo: seqNo }, function (data) {
                $scope.Data.CarePlanActivityList = data.Data;
            });
        }
        $scope.ChangeFinishFlag = function (Item) {
            if (Item.FINISHFLAG) {
                Item.FINISHDATE = new Date();
            } else {
                Item.FINISHDATE = null;
                Item.UNFINISHREASON = "";
            }
        }

        $scope.Preview = function (seqno) {
            window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("H10Report", seqno, "", ""), "_blank");
        }

        $scope.DeletePlan = function (Item) {
            if (confirm("删除该计划将同时删除所有关联照护目标及措施,确定要删除吗?")) {
                carePlanDetailRes.delete({ id: Item.SEQNO }, function (data) {
                    if (data.ResultCode == 0) {

                        $scope.Data.CarePlanList = {};
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.pageInfo.PageSize = 10;
                        $scope.options.params.FeeNo = $scope.currentResident.FeeNo;
                        $scope.care_div = "ui-inline-hide";
                        $scope.options.search();

                        // $scope.Data.CarePlanList.splice($scope.Data.CarePlanList.indexOf(Item), 1);
                        $scope.care_div = "ui-inline-hide";
                    }
                });
            }
        }

        $scope.SaveGoalAndActivity = function () {
            if (!$scope.Validation()) { return; }
            if ($scope.Data.CarePlanGoalList.length > 0) {
                angular.forEach($scope.Data.CarePlanGoalList, function (data, index, array) {
                    carePlanGoalRes.save(data, function (data) {
                        if (data.ResultCode == 0) {
                        } else {
                            alert(data.ResultMessage);
                        }
                    });
                });
            }
            if ($scope.Data.CarePlanActivityList.length > 0) {
                angular.forEach($scope.Data.CarePlanActivityList, function (data, index, array) {
                    carePlanActivityRes.save(data, function (data) {
                        if (data.ResultCode == 0) {
                        } else {
                            alert(data.ResultMessage);
                        }
                    });
                });
            }
            utility.message("保存成功!");
        }

        $scope.Validation = function () {
            var errorTips = 0;
            if (angular.isDefined($scope.myForm.$error.required)) {
                var msg = "";
                for (var i = 0; i < $scope.myForm.$error.required.length; i++) {
                    msg = $scope.myForm.$error.required[i].$name + " 为必填项";
                    utility.msgwarning(msg);
                    errorTips++;
                    if (errorTips >= $scope.maxErrorTips) {
                        return false;
                    }
                }
            }

            if (angular.isDefined($scope.myForm.$error.maxlength)) {
                var msg = "";
                for (var i = 0; i < $scope.myForm.$error.maxlength.length; i++) {
                    msg = $scope.myForm.$error.maxlength[i].$name + "超过设定长度 ";
                    utility.msgwarning(msg);
                    errorTips++;
                    if (errorTips >= $scope.maxErrorTips) {
                        return false;
                    }
                }
            }
            if (errorTips > 0) { return false; }
            return true;
        }

        $scope.init();

    }]);

angular.module("sltcApp")
.controller("multidisciplinaryCarePlanCtr", ['$scope', '$http', '$location', '$state', 'cloudAdminUi', 'utility', 'carePlanRes', 'carePlanListRes', 'empFileRes', 'nurDemandEvalRes', 'healthManageRes',
    function ($scope, $http, $location, $state, cloudAdminUi, utility, carePlanRes, carePlanListRes, empFileRes, nurDemandEvalRes, healthManageRes) {
        var seqNo = $state.params.seqNo;
        var regNo = $state.params.regNo;
        var feeNo = $state.params.feeNo;
        var regName = $state.params.regName;
        $scope.Data = {};
        $scope.Data.NSCPL = {};
        $scope.Data.Demand = {};
        $scope.Data.Health = {};
        $scope.Data.SeqNo = seqNo;
        $scope.Data.RegNo = regNo;
        $scope.Data.feeNo = feeNo;
        $scope.Data.RegName = regName;


        $scope.Data.Category = {};
        $scope.Data.Filter =
                {
                    "Id": 1, "ItemType": '', "LevelPR": '', "Category": "", "RegNO": null, "Date": '', 'Name': '', 'CurrentPage': 0, 'PageSize': 0, 'TotalRecords': 0
                };

        $scope.init = function () {
            cloudAdminUi.handleGoToTop();
            $scope.maxErrorTips = 3;
            $scope.Data.E00 = [{ value: "", name: "" }, { value: "001", name: "H1N1流感" }, { value: "002", name: "肺炎疫苗" }, { value: "003", name: "流行性感冒疫苗" }];

            $scope.Data.Gap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];

            empFileRes.get({ empNo: '' }, function (data) {
                $scope.Data.EmpList = data.Data;
            })



            carePlanListRes.get({ seqNo: seqNo }, function (data) {
                $scope.Data.NSCPL = data.Data;
                carePlanRes.get({ category: '' }, function (data) {
                    $scope.Data.Level = data.Data;
                });
                if (!isEmpty($scope.Data.NSCPL)) {
                    if (!isEmpty($scope.Data.NSCPL.CPLEVEL)) {
                        carePlanRes.get({ category: $scope.Data.NSCPL.CPTYPE, levelPR: $scope.Data.NSCPL.CPLEVEL }, function (data) {
                            $scope.Data.Diag = data.Data;
                        });
                    }
                } else {
                    $scope.Data.NSCPL = {};
                    $scope.Data.NSCPL.REGNO = regNo;
                    $scope.Data.NSCPL.FEENO = feeNo;
                    $scope.Data.NSCPL.STARTDATE = getNowFormatDate();

                    $scope.curUser = utility.getUserInfo();
                    $scope.Data.NSCPL.EMPNO = $scope.curUser.EmpNo;


                    carePlanRes.get({ allCategory: '' }, function (data) {
                        $scope.Data.NSCPL.CPTYPE = data.Data;
                    });
                }
                carePlanRes.get({ allCategory: '' }, function (data) {
                    $scope.Data.Category = data.Data;
                });

                //导因+特徽 Bob 20161201
                carePlanRes.get({ cp_no: $scope.Data.NSCPL.CPDIAG, isReason: false }, function (data) {
                    $scope.NSDESCList = data.Data;
                });
                carePlanRes.get({ cp_no: $scope.Data.NSCPL.CPDIAG, isReason: true }, function (data) {
                    $scope.CPREASONList = data.Data;
                });

            });

            updateEvalResult();
            healthManageRes.get({ feeNo: feeNo }, function (data) {
                if (data.Data != null) {
                    $scope.Data.Health.HEALTHINFO = data.Data.HEALTHINFO;
                };

            });
        };

        $scope.init();

        $scope.ChangeFinishDate = function () {
            if (isNumber($scope.Data.NSCPL.NEEDDAYS)) {
                var gap = $scope.Data.NSCPL.NEEDDAYS;
                var currentDate = moment($scope.Data.NSCPL.STARTDATE).format("YYYY-MM-DD");
                $scope.Data.NSCPL.TARGETDATE = GetNextEvalDate(currentDate, gap);
            }
        }

        function updateEvalResult() {
            nurDemandEvalRes.get({ feeNo: feeNo }, function (data) {
                $scope.Data.EvalResult = data.Data;
                for (var i = 0; i < $scope.Data.EvalResult.length; i++) {
                    //忧郁
                    if ($scope.Data.EvalResult[i].CODE == 'GDS') {
                        $scope.Data.Demand.YY_RESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.YY_SCORE = $scope.Data.EvalResult[i].SCORE;
                    } else if ($scope.Data.EvalResult[i].CODE == 'ADL') {
                        //ADL
                        $scope.Data.Demand.ADLRESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.ADLSCORE = $scope.Data.EvalResult[i].SCORE;
                    }
                    else if ($scope.Data.EvalResult[i].CODE == 'MMSE') {
                        //MMSE 失智量表
                        $scope.Data.Demand.MMSEDESC = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.MMSESCORE = $scope.Data.EvalResult[i].SCORE;
                    } else if ($scope.Data.EvalResult[i].CODE == 'SPMSQ') {
                        //SPMSQ 简易心智量表
                        $scope.Data.Demand.SPMSQESC = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.SPMSQCORE = $scope.Data.EvalResult[i].SCORE;
                    }
                    else if ($scope.Data.EvalResult[i].CODE == 'IADL') {
                        //IADL 工具
                        $scope.Data.Demand.iADLRESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.iADLSCORE = $scope.Data.EvalResult[i].SCORE;
                    } else if ($scope.Data.EvalResult[i].CODE == 'KARNOFSKY') {
                        $scope.Data.Demand.KS_RESULTS = $scope.Data.EvalResult[i].ENVRESULTS;

                    } else if ($scope.Data.EvalResult[i].CODE == 'FALL') {
                        //跌倒
                        $scope.Data.Demand.FALLRESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.FALLSCORE = $scope.Data.EvalResult[i].SCORE;
                    } else if ($scope.Data.EvalResult[i].CODE == 'SORE') {
                        $scope.Data.Demand.PRESSURESULT = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.PRESSURESORE = $scope.Data.EvalResult[i].SCORE;
                        //压疮
                    }
                }
            });
        }

        $scope.staffSelected = function (item) {
            $scope.Data.NSCPL.EMPNO = item.EmpNo;
        }

        $scope.ChangeCat = function (type) {
            carePlanRes.get({ category: type }, function (data) {
                $scope.Data.Level = data.Data;

            });
        }

        $scope.ChangeLevel = function (level) {
            carePlanRes.get({ category: $scope.Data.NSCPL.CPTYPE, levelPR: level }, function (data) {
                $scope.Data.Diag = data.Data;
                $scope.Data.NSCPL.NSDESC = "";
                $scope.Data.NSCPL.CPREASON = "";
            });
        }


        $scope.ChangeDiag = function (diag) {
            carePlanRes.get({ cp_no: diag, isReason: false }, function (data) {
                $scope.NSDESCList = data.Data;
            });
            carePlanRes.get({ cp_no: diag, isReason: true }, function (data) {
                $scope.CPREASONList = data.Data;
            });
        }

        $scope.addReason = function (type) {
            if (type == "right") {
                var temp = [];
                $("#sltReasonFrom option:selected").each(function (i, data) {
                    if (!angular.isDefined($scope.Data.NSCPL.CPREASON)) {
                        $scope.Data.NSCPL.CPREASON = '';
                    }
                    if ($scope.Data.NSCPL.CPREASON == '') {
                        $scope.Data.NSCPL.CPREASON = data.value;
                    } else {
                        $scope.Data.NSCPL.CPREASON += '\r' + data.value;
                    }
                });
            }
        }



        $scope.addNSDESC = function (type) {
            if (type == "right") {
                var temp = [];
                $("#sltFrom option:selected").each(function (i, data) {
                    if (!angular.isDefined($scope.Data.NSCPL.NSDESC)) {
                        $scope.Data.NSCPL.NSDESC = '';
                    }
                    if ($scope.Data.NSCPL.NSDESC == '') {
                        $scope.Data.NSCPL.NSDESC = data.value;
                    } else {
                        $scope.Data.NSCPL.NSDESC += '\r' + data.value;
                    }
                });
            }
        }

        $scope.Save = function (type) {
            if (!utility.Validation($scope.myForm.$error)) {
                return;
            }
            if ($scope.Data.NSCPL.TARGETDATE != '' && $scope.Data.NSCPL.TARGETDATE < $scope.Data.NSCPL.STARTDATE) {
                utility.msgwarning("预计完成日不能大於开始日期");
                return;
            }
            carePlanRes.save($scope.Data.NSCPL, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("保存成功!");
                    if (type == 'goal') {
                        $location.url('/angular/AddCarePlanGoal/' + data.Data.SEQNO + '/' + data.Data.FEENO + '/' + data.Data.REGNO + '/' + $scope.Data.NSCPL.CPDIAG + '/' + $scope.Data.RegName);
                    } else if (type == 'activity') {
                        $location.url('/angular/AddCarePlanActivity/' + data.Data.SEQNO + '/' + data.Data.FEENO + '/' + data.Data.REGNO + '/' + $scope.Data.NSCPL.CPDIAG + '/' + $scope.Data.RegName);
                    } else {
                        $location.url('/angular/CarePlanDetail/' + data.Data.FEENO + '/' + data.Data.REGNO + '/' + $scope.Data.RegName);
                    }
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }

    }
]);


angular.module("sltcApp")
.controller("addCarePlanGoalCtr", ['$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'carePlanGoalRes', 'carePlanListRes',
    function ($scope, $http, $location, $state, utility, cloudAdminUi, carePlanGoalRes, carePlanListRes) {
        var seqId = $state.params.seqNo;
        var regNo = $state.params.regNo;
        var feeNo = $state.params.feeNo;
        var regname = $state.params.regName;
        var cpNo = $state.params.cpNo;
        $scope.Data = {};
        $scope.Data.NSCPLGOAL = {};
        $scope.Data.SeqNo = seqId;
        $scope.Data.RegNo = regNo;
        $scope.Data.FeeNo = feeNo;
        $scope.Data.CpNo = cpNo;
        $scope.Data.RegName = regname;
        $scope.Data.NSCPLGOAL.SEQNO = seqId;
        $scope.Data.Category = {};
        $scope.Data.GoalList = {};
        $scope.init = function () {
            $scope.maxErrorTips = 3;
            carePlanListRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.NSCPL = data.Data;
                $scope.Data.NSCPLGOAL.RECDATE = $scope.Data.NSCPL.STARTDATE;
            });
            cloudAdminUi.handleGoToTop();
            $scope.saveClass = "ui-inline-show";
            $scope.updateClass = "ui-inline-hide";
            $scope.Data.E00 = [{ value: "", name: "" }, { value: "001", name: "H1N1流感" }, { value: "002", name: "肺炎疫苗" }, { value: "003", name: "流行性感冒疫苗" }];

            $scope.Data.Gap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];
            carePlanGoalRes.get({ cp_no: cpNo }, function (data) {
                $scope.Data.GoalPL = data.Data;

            });

            carePlanGoalRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.GoalList = data.Data;

            });

        }
        $scope.init();

        $scope.addGola = function (type) {
            if (type == "right") {//添加员工
                var temp = [];
                $("#sltFrom option:selected").each(function (i, data) {
                    if (!angular.isDefined($scope.Data.NSCPLGOAL.CPLGOAL)) {
                        $scope.Data.NSCPLGOAL.CPLGOAL = '';
                    }
                    if ($scope.Data.NSCPLGOAL.CPLGOAL == '') {
                        $scope.Data.NSCPLGOAL.CPLGOAL = data.value;
                    } else {
                        $scope.Data.NSCPLGOAL.CPLGOAL += '\r' + data.value;
                    }
                });
            }
        }

        $scope.Validation = function () {
            var errorTips = 0;
            if (angular.isDefined($scope.myForm.$error.required)) {
                var msg = "";
                for (var i = 0; i < $scope.myForm.$error.required.length; i++) {
                    msg = $scope.myForm.$error.required[i].$name + " 为必填项";
                    utility.msgwarning(msg);
                    errorTips++;
                    if (errorTips >= $scope.maxErrorTips) {
                        return false;
                    }
                }
            }

            if (angular.isDefined($scope.myForm.$error.maxlength)) {
                var msg = "";
                for (var i = 0; i < $scope.myForm.$error.maxlength.length; i++) {
                    msg = $scope.myForm.$error.maxlength[i].$name + "超过设定长度 ";
                    utility.msgwarning(msg);
                    errorTips++;
                    if (errorTips >= $scope.maxErrorTips) {
                        return false;
                    }
                }
            }
            if (errorTips > 0) { return false; }
            return true;
        }

        $scope.Save = function () {
            if (!utility.Validation($scope.myForm.$error)) {
                return;
            }
            carePlanGoalRes.save($scope.Data.NSCPLGOAL, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("保存成功!");
                    $scope.Data.GoalList.push(data.Data)
                    $scope.Data.FinishFlag = 0;
                    $scope.Data.NSCPLGOAL = {};
                    $scope.Data.NSCPLGOAL.SEQNO = seqId;
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }

        $scope.Edit = function (currentItem) {
            $scope.saveClass = "ui-inline-hide";
            $scope.updateClass = "ui-inline-show";
            $scope.Data.NSCPLGOAL = currentItem;
            $scope.Data.NSCPLGOAL.RECDATE = transferDate($scope.Data.NSCPLGOAL.RECDATE);
        }

        $scope.ChangeFinishFlag = function () {
            if ($scope.Data.NSCPLGOAL.FINISHFLAG) {
                $scope.Data.NSCPLGOAL.FINISHDATE = getNowFormatDate();
            } else {
                $scope.Data.NSCPLGOAL.FINISHDATE = null;
            }
        }

        $scope.Update = function () {
            if (!$scope.Validation()) { return; }
            carePlanGoalRes.save($scope.Data.NSCPLGOAL, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("更新成功!");
                    $scope.Data.NSCPLGOAL = {};
                    $scope.Data.NSCPLGOAL.SEQNO = seqId;
                    $scope.saveClass = "ui-inline-show";
                    $scope.updateClass = "ui-inline-hide";
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }

        $scope.Delete = function (currentItem) {
            if (confirm("确定删除该目标信息吗?")) {
                carePlanGoalRes.delete({ id: currentItem.ID }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.Data.GoalList.splice($scope.Data.GoalList.indexOf(currentItem), 1);
                    }
                });
            }
        }


    }
]);


angular.module("sltcApp")
.controller("addCarePlanActivityCtr", ['$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'carePlanActivityRes', 'carePlanListRes',
    function ($scope, $http, $location, $state, utility, cloudAdminUi, carePlanActivityRes, carePlanListRes) {
        var seqId = $state.params.seqNo;
        var regNo = $state.params.regNo;
        var feeNo = $state.params.feeNo;
        var cpNo = $state.params.cpNo;
        var regname = $state.params.regName;
        $scope.Data = {};
        $scope.Data.NSCPLACTIVITY = {};
        $scope.Data.SeqNo = seqId;
        $scope.Data.RegNo = regNo;
        $scope.Data.FeeNo = feeNo;
        $scope.Data.CpNo = cpNo;
        $scope.Data.RegName = regname;
        $scope.Data.NSCPLACTIVITY.SEQNO = seqId;
        $scope.Data.NSCPLACTIVITY.REGNO = regNo;
        $scope.Data.Category = {};
        $scope.Data.ActivityList = {};
        $scope.init = function () {
            $scope.maxErrorTips = 3;
            carePlanListRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.NSCPL = data.Data;
                $scope.Data.NSCPLACTIVITY.RECDATE = $scope.Data.NSCPL.STARTDATE;
            });
            cloudAdminUi.handleGoToTop();
            $scope.saveClass = "ui-inline-show";
            $scope.updateClass = "ui-inline-hide";
            $scope.Data.E00 = [{ value: "", name: "" }, { value: "001", name: "H1N1流感" }, { value: "002", name: "肺炎疫苗" }, { value: "003", name: "流行性感冒疫苗" }];
            $scope.Data.OPERATOR = [{ value: '1', name: "操作员一" }, { value: '2', name: "操作员二" }, { value: '3', name: "操作员三" }, { value: '4', name: "操作员四" }];
            $scope.Data.Gap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];

            carePlanActivityRes.get({ cp_no: cpNo }, function (data) {
                $scope.Data.ActivityPL = data.Data;

            });
            carePlanActivityRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.ActivityList = data.Data;
            });

        }
        $scope.init();

        $scope.addActivity = function (type) {
            if (type == "right") {
                var temp = [];
                $("#sltFrom option:selected").each(function (i, data) {
                    if (!angular.isDefined($scope.Data.NSCPLACTIVITY.CPLACTIVITY)) {
                        $scope.Data.NSCPLACTIVITY.CPLACTIVITY = '';
                    }
                    if ($scope.Data.NSCPLACTIVITY.CPLACTIVITY == '') {
                        $scope.Data.NSCPLACTIVITY.CPLACTIVITY = data.value;
                    } else {
                        $scope.Data.NSCPLACTIVITY.CPLACTIVITY += '\r' + data.value;
                    }
                });
            }
        }

        $scope.Save = function () {
            //if (!$scope.Validation()) { return; }
            if (!utility.Validation($scope.myForm.$error)) {
                return;
            }
            carePlanActivityRes.save($scope.Data.NSCPLACTIVITY, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("保存成功!");
                    $scope.Data.ActivityList.push(data.Data)
                    $scope.Data.NSCPLACTIVITY = {};
                    $scope.Data.NSCPLACTIVITY.SEQNO = seqId;
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }

        $scope.Edit = function (currentItem) {
            $scope.saveClass = "ui-inline-hide";
            $scope.updateClass = "ui-inline-show";
            $scope.Data.NSCPLACTIVITY = currentItem;
            $scope.Data.NSCPLACTIVITY.RECDATE = transferDate($scope.Data.NSCPLACTIVITY.RECDATE);
        }

        $scope.ChangeFinishFlag = function () {
            if ($scope.Data.NSCPLACTIVITY.FINISHFLAG) {
                $scope.Data.NSCPLACTIVITY.FINISHDATE = getNowFormatDate();
            } else {
                $scope.Data.NSCPLACTIVITY.FINISHDATE = null;
            }
        }

        $scope.Update = function () {
            if (!utility.Validation($scope.myForm.$error)) {
                return;
            }
            carePlanActivityRes.save($scope.Data.NSCPLACTIVITY, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("更新成功!");
                    $scope.Data.NSCPLACTIVITY = {};
                    $scope.Data.NSCPLACTIVITY.SEQNO = seqId;
                    $scope.saveClass = "ui-inline-show";
                    $scope.updateClass = "ui-inline-hide";
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }

        $scope.Delete = function (currentItem) {
            if (confirm("确定删除该措施信息吗?")) {
                carePlanActivityRes.delete({ id: currentItem.ID }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.Data.ActivityList.splice($scope.Data.ActivityList.indexOf(currentItem), 1);
                    }
                });
            }
        }

    }
]);

angular.module("sltcApp")
.controller("carePlanAssessCtr", ['$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'carePlanAssessRes', 'carePlanListRes', 'carePlanRes', 'empFileRes', 'carePlanGoalRes', 'carePlanActivityRes', 'nursingRecordRes',
    function ($scope, $http, $location, $state, utility, cloudAdminUi, carePlanAssessRes, carePlanListRes, carePlanRes, empFileRes, carePlanGoalRes, carePlanActivityRes, nursingRecordRes) {
        var seqId = $state.params.seqNo;
        var regNo = $state.params.regNo;
        var feeNo = $state.params.feeNo;
        var cpNo = $state.params.cpNo;
        var regname = $state.params.regName;
        $scope.Data = {};
        $scope.Data.NSCPL = {};
        $scope.Data.SeqNo = seqId;
        $scope.Data.RegNo = regNo;
        $scope.Data.FeeNo = feeNo;
        $scope.Data.CpNo = cpNo;
        $scope.Data.Assess = {};
        $scope.Data.RegName = regname;
        $scope.Data.Assess.SEQNO = seqId;
        $scope.Data.AssessList = {};
        $scope.init = function () {
            $scope.maxErrorTips = 3;
            cloudAdminUi.handleGoToTop();
            $scope.Data.SyncNurRec = true;
            $scope.saveClass = "ui-inline-show";
            $scope.updateClass = "ui-inline-hide";
            $scope.assessClass = "ui-inline-hide";
            $scope.Data.E00 = [{ value: "", name: "" }, { value: "001", name: "H1N1流感" }, { value: "002", name: "肺炎疫苗" }, { value: "003", name: "流行性感冒疫苗" }];
            $scope.Data.OPERATOR = [{ value: '1', name: "操作员一" }, { value: '2', name: "操作员二" }, { value: '3', name: "操作员三" }, { value: '4', name: "操作员四" }];
            $scope.Data.Gap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];
            $scope.Data.Assess.RECDATE = getNowFormatDate();
            $scope.Data.NSCPL.FINISHDATE = getNowFormatDate();
            carePlanListRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.NSCPL = data.Data;
                if (!isEmpty($scope.Data.NSCPL.CPLEVEL)) {
                    carePlanRes.get({ category: $scope.Data.NSCPL.CPTYPE, levelPR: $scope.Data.NSCPL.CPLEVEL }, function (data) {
                        $scope.Data.Diag = data.Data;
                    });
                }
            });

            carePlanGoalRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.CarePlanGoalList = data.Data;
            });
            carePlanActivityRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.CarePlanActivityList = data.Data;
            });

            carePlanAssessRes.get({ cp_no: cpNo }, function (data) {
                $scope.Data.AssessPL = data.Data;
            });
            carePlanAssessRes.get({ seqNo: seqId }, function (data) {
                $scope.Data.AssessList = data.Data;
            });

            empFileRes.get({ empNo: '' }, function (data) {
                $scope.Data.EmpList = data.Data;
            })

        }
        $scope.init();

        $scope.addAssess = function (type) {
            if (type == "right") {
                var temp = [];
                $("#sltFrom option:selected").each(function (i, data) {
                    if (!angular.isDefined($scope.Data.Assess.VALUEDESC)) {
                        $scope.Data.Assess.VALUEDESC = '';
                    }
                    if ($scope.Data.Assess.VALUEDESC == '') {
                        $scope.Data.Assess.VALUEDESC = data.value;
                    } else {
                        $scope.Data.Assess.VALUEDESC += '\r' + data.value;
                    }
                });
            }
        }

        $scope.ChangeFinishFlag = function () {
            if ($scope.Data.NSCPL.FINISHFLAG!="") {
                $scope.Data.NSCPL.FINISHDATE = getNowFormatDate();
            } else {
                $scope.Data.NSCPL.FINISHDATE = null;
            }
            var diff = DateDiff($scope.Data.NSCPL.FINISHDATE, $scope.Data.NSCPL.STARTDATE);
            $scope.Data.NSCPL.TOTALDAYS = diff;
        }

        $scope.ChangeTotalDays = function () {
            var diff = DateDiff($scope.Data.NSCPL.FINISHDATE, $scope.Data.NSCPL.STARTDATE);
            $scope.Data.NSCPL.TOTALDAYS = diff;
        }

        $scope.SaveNSCPL = function () {

            if (isEmpty($scope.Data.NSCPL.FINISHDATE)) {
                utility.msgwarning("完成时间为必填项!");
                return;
            }
            if (!utility.Validation($scope.myForm.$error)) {
                return;
            }
            if ($scope.Data.NSCPL.TOTALDAYS < 0) {
                utility.message("完成时间不能早于开始时间!");
                return;
            }
            carePlanRes.save($scope.Data.NSCPL, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("保存成功!");
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }

        $scope.EditAssess = function (currentItem) {
            $scope.assessClass = "ui-inline-show";
            $scope.saveClass = "ui-inline-hide";
            $scope.updateClass = "ui-inline-show";
            $scope.Data.Assess = currentItem;
            $scope.Data.Assess.RECDATE = transferDate($scope.Data.Assess.RECDATE);
        }

        $scope.EditOrCreate = function () {

            $scope.assessClass = "ui-inline-show";
        }

        $scope.SaveAssess = function () {

            carePlanAssessRes.save($scope.Data.Assess, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("保存成功!");
                    $scope.Data.AssessList.push(data.Data);
                    $scope.Data.Assess = {};
                    $scope.Data.Assess.SEQNO = seqId;
                    $scope.assessClass = "ui-inline-hide";
                    if ($scope.Data.SyncNurRec) {
                        $scope.record = {};
                        $scope.record.FeeNo = feeNo;
                        $scope.record.RegNo = regNo;
                        //$scope.record.RecordDate = data.Data.RECDATE;
                        $scope.record.RecordDate = new Date();
                        var recNameBy = "";
                        var recName = data.Data.EXECUTEBY;
                        var recDate = "";
                        var EmpList = [];
                        EmpList = $scope.Data.EmpList
                        for (var i = 0; i < EmpList.length; i++) {
                            if (EmpList[i].EmpName == recName) {
                                recNameBy = EmpList[i].EmpNo;
                            }
                        }
                        if (angular.isDefined(data.Data.RECDATE)) {
                            recDate = data.Data.RECDATE.substring(0, 4) + "年" + data.Data.RECDATE.substring(5, 7) + "月" + data.Data.RECDATE.substring(8, 10) + "日";
                        }
                        $scope.record.RecordNameBy = recName;
                        $scope.record.RecordBy = recNameBy;
                        $scope.record.Content = "评值人员：" + recName + " ；评值日期：" + recDate + " ； 评值内容：" + data.Data.VALUEDESC;
                        nursingRecordRes.save($scope.record, function () {
                            utility.message("同步写入护理记录成功!");
                        });
                    }
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }



        $scope.UpdateAssess = function () {
            //if (!$scope.Validation()) { return; }
            if (!utility.Validation($scope.myForm.$error)) {
                return;
            }
            carePlanAssessRes.save($scope.Data.Assess, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("更新成功!");
                    $scope.Data.Assess = {};
                    $scope.Data.Assess.SEQNO = seqId;
                    $scope.assessClass = "ui-inline-hide";
                    $scope.saveClass = "ui-inline-show";
                    $scope.updateClass = "ui-inline-hide";
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }


        $scope.DeleteAssess = function (currentItem) {
            if (confirm("确定删除该评值信息吗?")) {
                carePlanAssessRes.delete({ id: currentItem.ID }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.Data.AssessList.splice($scope.Data.AssessList.indexOf(currentItem), 1);
                    }
                });
            }
        }
    }
]);
