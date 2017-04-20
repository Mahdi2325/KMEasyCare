/*****************************************************************************
 * Filename: nursing
 * Creator:	Lei Chen
 * Create Date: 2016-03-01 13:34:19
 * Modifier:
 * Modify Date:
 * Description:评估表
 ******************************************************************************/
angular.module("sltcApp").controller("evaluationListCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'evalsheetRes', 'utility',
    function ($scope, $http, $location, $state, dictionary, evalsheetRes, utility) {
        var code = $state.params.code;
        $scope.Data = {};
        $scope.Data.QuestionName = $state.params.qName;
        $scope.Data.Code = code;
        $scope.Data.Question = {};
        $scope.Data.Filter =
           {
               'FeeNO': null, 'Sex': '', 'Code': '', "Id": 0, "ItemType": '', "RegNO": null, "Date": '', 'Name': '', 'CurrentPage': 0, 'PageSize': 0, 'TotalRecords': 0, 'residengno': ''
           };
        $scope.Data.Filter.Code = code;
        evalsheetRes.get($scope.Data.Filter, function (data) {
            if (data.ResultCode > 0) {
                utility.message(data.ResultMessage);
            } else {
                $scope.Data.Question = data.Data;
                $scope.Data.QuestionId = data.Id;
            }
        });

        $scope.Search = function () {
            evalsheetRes.get($scope.Data.Filter, function (data) {
                $scope.Data.Question = data.Data;
            });
        }

        $scope.JumpToAdd = function (qId, regNo, feeNo, regName, recId, qName) {

            var url = '/angular/EvalSheetTemp/' + regNo + '/' + feeNo + '/' + qId + '/' + regName + '/' + recId + '/' + qName;
            $location.url(url);
        }

        $scope.JumpToHistory = function (qId, feeNo, regName, qName) {
            var url = '/angular/EvalSheetHistory/' + qId + '/' + feeNo + '/' + regName + '/' + qName;
            $location.url(url);
        }

        $scope.JumpToReport = function (feeNo) {
            //var url = '/api/Report/H76压疮风险评估?feeNo=' + feeNo;
            window.location.href = 'api/Report/H76压疮风险评估?feeNo=' + feeNo;
            //$location.url(url);
        }
    }
]);

angular.module("sltcApp").controller("evaluationSheetCtr", ['$scope', '$http', '$location', '$state', 'utility', '$filter', 'cloudAdminUi', 'evalsheetRes', 'evaluationRes', 'empFileRes', 'evaluationHisRes', 'adminHandoversRes',
    function ($scope, $http, $location, $state, utility, $filter, cloudAdminUi, evalsheetRes, evaluationRes, empFileRes, evaluationHisRes, adminHandoversRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.QuestionName = $state.params.qName;
        $scope.QuestionCode = $state.params.qCode;
        //Add by Duke on 20160809 不需要打印功能的隐藏打印按钮
        switch ($scope.QuestionCode) {
            case "GDS":
                //case "FALL":
            case "BEHAVIOR":
            case "ACTIVITY":
            case "NOS":
                $scope.showPrint = false;
                break;
            default: $scope.showPrint = true;
        }
        $scope.IsNewCreate = false;
        $scope.RegQuestion = {};
        $scope.EvalResult = {};
        $scope.Data = {};
        $scope.EmpList = {};
        $scope.Data.MakerItemList = {};
        $scope.Data.MakerItemList.Answers = {};
        $scope.MakerItemByCategory = {};


        $scope.LoadInfo = function () {
            $scope.maxErrorTips = 3
            cloudAdminUi.handleGoToTop();

            evalsheetRes.get({ Code: $scope.QuestionCode }, function (response) {
                $scope.Data = response.Data;
                if (response.Data == null && response.ResultMessage != '') {
                    utility.message(response.ResultMessage);
                    return;
                }
                $scope.Code = $scope.Data.CODE;
                $scope.MakerItemByCategory = getMakerItemListByCategory($scope.Data.MakerItemList);
                empFileRes.get({ empNo: '' }, function (data) {
                    $scope.EmpList = data.Data;
                })

                $scope.RegQuestion.EVALUATEBY = utility.getUserInfo().EmpNo;
                $scope.RegQuestion.QUESTIONID = $scope.Data.QUESTIONID;
                $scope.RegQuestion.EVALDATE = $filter("date")(new Date(), "yyyy-MM-dd");
            });
        };



        $scope.init = function () {
            $scope.LoadInfo();

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: evaluationHisRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.QuestionList = data.Data;
                    if (data.Data != null && data.Data.length > 0) {
                        $scope.IsNewCreate = true;
                    }
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    QuestionId: -1,
                    FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        };

        $scope.staffSelected = function (item, flag) {
            switch (flag) {
                case "EVALUATEBY": {
                    $scope.RegQuestion.EVALUATEBY = item.EmpNo;
                    break;
                }
                case "NEXTEVALUATEBY": {
                    $scope.RegQuestion.NEXTEVALUATEBY = item.EmpNo;
                    break;
                }
            }
        }

        $scope.GetHistory = function () {
            $scope.options.params.QuestionId = $scope.RegQuestion.QUESTIONID;
            $scope.options.params.FeeNo = $scope.currentResident.FeeNo;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();

            //evaluationHisRes.get({ QuestionId: $scope.RegQuestion.QUESTIONID, FeeNo: $scope.currentResident.FeeNo }, function (response) {
            //    $scope.QuestionList = response.Data;
            //    if (response.Data != null && response.Data.length > 0) {
            //        $scope.IsNewCreate = true;
            //    }
            //});
        }

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.currentResident = resident;
            $scope.currentResident.FeeNo = resident.FeeNo;
            $scope.RegQuestion.FEENO = resident.FeeNo;
            $scope.RegQuestion.REGNO = resident.RegNo;
            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.currentItem = {};
            $scope.LoadInfo();
            $scope.GetHistory();
            $scope.RegQuestion.NEXTEVALDATE = "";
            $scope.RegQuestion.Gap = '';
            $scope.RegQuestion.SCORE = '';
            $scope.RegQuestion.ENVRESULTS = '';
            $scope.Reference = '';
            $scope.RegQuestion.EVALDATE = '';
            $scope.RegQuestion.Gap = '';
            $scope.RegQuestion.EVALUATEBY = '';
            $scope.RegQuestion.NEXTEVALDATE = '';
            $scope.RegQuestion.NEXTEVALUATEBY = '';
            $scope.curUser = utility.getUserInfo();

        };

        $scope.calcResult = function (rightAnswer) {
            if (angular.isDefined($scope.currentResident) && $scope.currentResident != null && angular.isDefined($scope.currentResident.FeeNo) && $scope.currentResident.FeeNo != '') {
                if ($scope.Data.SCOREFLAG) {
                    $scope.Data.FEENO = $scope.currentResident.FeeNo;
                    $scope.Data.REGNO = $scope.currentResident.RegNo;;
                    evaluationRes.save($scope.Data, function (response) {
                        $scope.EvalResult = response;
                        $scope.RegQuestion.SCORE = $scope.EvalResult.Score;
                        $scope.RegQuestion.ENVRESULTS = $scope.EvalResult.Result;
                        $scope.Reference = $scope.EvalResult.Result;
                    });
                }
            } else {
                utility.message("请先选择住民!");
            }
        }

        $scope.Print = function (item) {
            if (angular.isDefined(item.RECORDID)) {
                if (item.RECORDID == 0) {
                    utility.message("无打印数据！");
                    return;
                }
                switch ($scope.QuestionCode) {
                    case "ADL": window.open('/DC_Report/PreviewLTC_NursingReport?templateName=ADLReport&id=' + item.RECORDID);
                        break;
                    case "MMSE": window.open('/DC_Report/PreviewLTC_NursingReport?templateName=MMSEReport&id=' + item.RECORDID);
                        break;
                    case "SPMSQ": window.open('/DC_Report/PreviewLTC_NursingReport?templateName=SPMSQReport&id=' + item.RECORDID);
                        break;
                    case "IADL": window.open('/DC_Report/PreviewLTC_NursingReport?templateName=IADLReport&id=' + item.RECORDID);
                        break;
                    case "KARNOFSKY": window.open('/DC_Report/PreviewLTC_NursingReport?templateName=ColeScaleReport&id=' + item.RECORDID);
                        break;
                    case "SORE": window.open('/DC_Report/PreviewLTC_NursingReport?templateName=PrsSoreReport&id=' + item.RECORDID);
                        break;
                    case "FALL": window.open('/Report/Preview?templateName=P10Report&key=' + item.RECORDID);
                        break;
                }

            } else {
                utility.message("无打印数据！");
            }
        }

        $scope.Edit = function (item) {

            evalsheetRes.get({ qId: item.QUESTIONID, regNo: $scope.RegQuestion.REGNO, recordId: item.RECORDID }, function (response) {
                $scope.Data = response.Data;
                $scope.Code = $scope.Data.CODE;
                $scope.MakerItemByCategory = getMakerItemListByCategory($scope.Data.MakerItemList);
                $scope.RegQuestion.QUESTIONID = $scope.Data.QUESTIONID;
            });
            evalsheetRes.get({ recordId: item.RECORDID }, function (response) {
                $scope.RegQuestion = response.Data;
                $scope.RegQuestion.Gap = DateDiff($scope.RegQuestion.NEXTEVALDATE, $scope.RegQuestion.EVALDATE);
                var envResult = $scope.RegQuestion.ENVRESULTS;
                $scope.Reference = envResult;
                //$scope.RegQuestion.EVALDATE = transferDate($scope.RegQuestion.EVALDATE);
            });
        }

        $scope.Delete = function (Item) {
            if (confirm("确定删除该评估记录吗?")) {
                evaluationHisRes.delete({ recId: Item.RECORDID }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                        //$scope.QuestionList.splice($scope.QuestionList.indexOf(Item), 1);
                    }
                });
            }
        }

        $scope.GetLatestEvlRecord = function () {
            evaluationRes.get({ 'feeNo': $scope.currentResident.FeeNo, 'quetionId': $scope.RegQuestion.QUESTIONID }, function (response) {
                if (response.Data.RECORDID > 0) {
                    $scope.RegQuestion = response.Data;
                    $scope.RegQuestion.Gap = DateDiff($scope.RegQuestion.NEXTEVALDATE, $scope.RegQuestion.EVALDATE);
                    var envResult = $scope.RegQuestion.ENVRESULTS;
                    $scope.Reference = envResult;
                    evalsheetRes.get({ qId: $scope.RegQuestion.QUESTIONID, regNo: $scope.RegQuestion.REGNO, recordId: $scope.RegQuestion.RECORDID }, function (response) {
                        $scope.Data = response.Data;
                        $scope.MakerItemByCategory = getMakerItemListByCategory($scope.Data.MakerItemList);
                    });
                    $scope.RegQuestion.RECORDID = 0;
                    $scope.RegQuestion.EVALDATE = $filter("date")(new Date(), "yyyy-MM-dd");
                    utility.message("带入最新评估历史记录成功!");
                } else {
                    utility.message("没有最新数据导入!");
                }
            });
        }

        $scope.Save = function () {
            if (angular.isDefined($scope.currentResident) && $scope.currentResident != null && angular.isDefined($scope.currentResident.FeeNo) && $scope.currentResident.FeeNo != '') {
                if (!$scope.Validation()) { return; }
                $scope.RegQuestion.QuestionDataList = [];
                $scope.Data.MakerItemList = [];
                for (var j = 0; j < $scope.MakerItemByCategory.length; j++) {
                    $scope.Data.MakerItemList = $scope.Data.MakerItemList.concat($scope.MakerItemByCategory[j].data);
                }
                for (var i = 0; i < $scope.Data.MakerItemList.length; i++) {
                    var questionData = {};
                    questionData.QUESTIONID = $scope.Data.QUESTIONID;
                    questionData.MAKERID = $scope.Data.MakerItemList[i].MAKERID;
                    questionData.LIMITEDVALUEID = $scope.Data.MakerItemList[i].LIMITEDVALUEID;
                    $scope.RegQuestion.QuestionDataList.push(questionData);
                }
                evalsheetRes.save($scope.RegQuestion, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.RegQuestion.SCORE = "";
                        $scope.RegQuestion.ENVRESULTS = "";
                        $scope.Reference= "";
                        $scope.LoadInfo();
                        $scope.GetHistory();
                        utility.message("保存成功");
                        if ($scope.RegQuestion.NEXTEVALDATE) {
                            var assTask = {};
                            assTask.KEY = $scope.QuestionCode;
                            assTask.NEXTEVALDATE = $scope.RegQuestion.NEXTEVALDATE;
                            assTask.NEXTEVALUATEBY = $scope.RegQuestion.NEXTEVALUATEBY;
                            assTask.FEENO = $scope.currentResident.FeeNo;
                            adminHandoversRes.assSave(assTask, function (data) { })
                        }


                    } else {
                        utility.message(data.ResultMessage);
                    }
                });
            } else {
                utility.message("请先选择住民!");
            }
        }

        $scope.setNextValDate = function (gap) {
            if (isNumber(gap)) {
                if (gap > 0) {
                    var currentDate = $scope.RegQuestion.EVALDATE;
                    currentDate = currentDate.substring(0, 10);
                    $scope.RegQuestion.NEXTEVALDATE = GetNextEvalDate(currentDate, gap);
                } else if (gap < 0) {
                    $scope.RegQuestion.Gap = '';
                    utility.message("间隔天数不能为负数");
                }
            }
        }

        $scope.ChangeNextEvalDate = function () {
            if ($scope.RegQuestion.EVALDATE != "" && $scope.RegQuestion.NEXTEVALDATE != "") {
                var days = DateDiff($scope.RegQuestion.NEXTEVALDATE, $scope.RegQuestion.EVALDATE)
                if (days < 0) {
                    utility.message("下次评估日期不能小于评估日期");
                    $scope.RegQuestion.NEXTEVALDATE = "";
                    $scope.RegQuestion.Gap = '';
                } else {
                    $scope.RegQuestion.Gap = days;
                }
            };
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
        $scope.init();//初始化
    }]);


angular.module("sltcApp").controller("evalSheetHistoryCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'evalsheetRes', 'evaluationHisRes',
    function ($scope, $http, $location, $state, dictionary, evalsheetRes, evaluationHisRes) {
        $scope.Data = {};
        var residentName = $state.params.regName;
        var questionId = $state.params.qId;
        var feeNo = $state.params.feeNo;
        var questionName = $state.params.qName;
        $scope.QuestionId = questionId;
        $scope.QuestionName = questionName;
        $scope.Data.QuestionId = questionId;
        $scope.Data.FeeNo = feeNo;
        $scope.init = function () {
            evaluationHisRes.get({ QuestionId: questionId, FeeNo: feeNo }, function (response) {
                $scope.QuestionList = response.Data;
            });
        }
        $scope.init();//初始化

        $scope.Delete = function (Item) {
            if (confirm("确定删除该评估记录吗?")) {
                evaluationHisRes.delete({ recId: Item.RECORDID }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.QuestionList.splice($scope.QuestionList.indexOf(Item), 1);
                    }
                });
            }
        }

        $scope.Edit = function (item) {
            var url = '/angular/EvalSheetTemp/' + item.REGNO + '/' + item.FEENO + '/' + item.QUESTIONID + '/' + residentName + '/' + item.RECORDID + '/' + questionName;
            $location.url(url);
        }

    }
]);

angular.module("sltcApp").controller("nurDemandListCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'cloudAdminUi', 'nurDemandEvalRes', function ($scope, $http, $location, $state, dictionary, cloudAdminUi, nurDemandEvalRes) {

    $scope.init = function () {

        cloudAdminUi.handleGoToTop();
        $scope.Dic = {};
        $scope.Data = {};
        $scope.TabContent = {};

        $scope.EvalDateGap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];

        nurDemandEvalRes.get({}, function (data) {
            $scope.Data.Question = data.Data;
        });
    }

    $scope.init(); //初始化

    $scope.Search = function () {
        nurDemandEvalRes.get($scope.Data.Filter, function (data) {
            $scope.Data.Question = data.Data;
        });
    }



}
]);


angular.module("sltcApp").controller("nurDemandSheetCtr", ['$scope', '$http', '$location', '$state', '$filter', 'utility', 'cloudAdminUi', 'nurDemandEvalRes', 'empFileRes', 'empFileResGet', 'vitalSignRes', 'adminHandoversRes', 'personRes',
    function ($scope, $http, $location, $state, $filter, utility, cloudAdminUi, nurDemandEvalRes, empFileRes, empFileResGet, vitalSignRes, adminHandoversRes, personRes) {
        var id = $state.params.id;
        var feeNo = $state.params.feeNo;
        var regNo = $state.params.regNo;   // 为空时 为编辑， 不为空时 为新增
        $scope.healthFlag = true;
        $scope.infectionFlag = true;

        $scope.init = function () {

            $scope.VITALSIGNFLAG = true;
            $scope.APPEARANCEFLAG = true;
            $scope.BREATHPROBLEMFLAG = true;
            $scope.NUTRITIONFLAG = true;
            $scope.EXCRETIONFLAG = true;
            $scope.SLEEPFLAG = true;
            $scope.ACTIVEFUNFLAG = true;
            $scope.SKINFLAG = true;
            $scope.FEELINGFLAG = true;
            $scope.INTERACTIONFLAG = true;
            $scope.ALLERGYFLAG = true;
            $scope.showOther = true;


            $scope.Dic = {};
            $scope.Data = {};
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.Data.Demand = { EVALUATEBY: $scope.curUser.EmpNo };
            }
            $scope.Data.Evaluation = {};
            cloudAdminUi.handleGoToTop();
            empFileRes.get({ empNo: '' }, function (data) {
                $scope.EmpList = data.Data;
            });
            personRes.get({ regNo: regNo, mark: "" }, function (response) {
                if (response.Data != null) {
                    if (response.Data.HaveDrugAllergy != null) {
                        if (response.Data.HaveDrugAllergy) {
                            $scope.Data.Demand.ALLERGY_DRUG = response.Data.DrugAllergy;
                        }
                        else {
                            $scope.Data.Demand.ALLERGY_DRUG = "无";
                        }
                    }
                    if (response.Data.HaveFoodAllergy != null) {
                        if (response.Data.HaveFoodAllergy) {
                            $scope.Data.Demand.ALLERGY_FOOD = response.Data.FoodAllergy;;
                        }
                        else {
                            $scope.Data.Demand.ALLERGY_FOOD = "无";
                        }
                    }
                }
            });
            //$scope.EvalDateGap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];
            if (regNo != null && regNo != '') { //新增
                updateEval();
                $scope.Data.Demand.EVALDATE = $filter("date")(new Date(), "yyyy-MM-dd");
                empFileResGet.get({ regon: regNo }, function (data) {
                    //$scope.Data.Demand.HEALTHDESC = data.Data.PersonalHistory;
                    $scope.Data.Demand.HEALTHDESC = data.Data.DiseaseDiag;
                    $scope.Data.Demand.INFECTIONDESC = data.Data.InfecFlag;
                    $scope.Data.Demand.HEIGHT = data.Data.Height;
                    $scope.Data.Demand.WEIGHT = data.Data.Weight;
                });
                vitalSignRes.get({ regNo: regNo, feeNo: feeNo }, function (data) {
                    if (angular.isDefined(data.Data)) {
                        if (data.Data.length > 0) {
                            $scope.Data.Demand.BODYTEMP = data.Data[0].Bodytemp;
                            $scope.Data.Demand.PULSE = data.Data[0].Pulse;
                            $scope.Data.Demand.BREATHE = data.Data[0].Breathe;
                            $scope.Data.Demand.SBP = data.Data[0].SBP;
                            $scope.Data.Demand.DBP = data.Data[0].DBP;
                        }
                    }
                });
            } else { //编辑
                if (id > 0) {
                    nurDemandEvalRes.get({ demandId: id, getDemand: '' }, function (data) {
                        $scope.Data.Demand = data.Data;
                        $scope.ChangeNextEvalDate();
                    });
                }
                $scope.Data.Demand.REGNO = regNo;
            }
            $scope.Data.Demand.FEENO = feeNo;
        }

        $scope.init(); //初始化

        $scope.setNextValDate = function (gap) {
            if (isNumber(gap)) {
                if (gap > 0) {
                    var currentDate = $scope.Data.Demand.EVALDATE;
                    currentDate = currentDate.substring(0, 10);
                    $scope.Data.Demand.NEXTEVALDATE = GetNextEvalDate(currentDate, gap);
                } else if (gap < 0) {
                    $scope.Data.Demand.Gap = '';
                    utility.message("间隔天数不能为负数");
                }
            }
        }

        $scope.checkHealth = function (isShow) {
            if (isShow) {
                $scope.healthFlag = false;
            }
            else {
                $scope.healthFlag = true;
                $scope.Data.Demand.HEALTHDESC = "";
            }
        };

        $scope.checkInfection = function (isShow) {
            if (isShow) {
                $scope.infectionFlag = false;
            }
            else {
                $scope.infectionFlag = true;
                $scope.Data.Demand.INFECTIONDESC = "";
            }
        };

        $scope.checkVITALSIGNFLAG = function (isShow) {
            if (isShow) {
                $scope.VITALSIGNFLAG = false;
            }
            else {
                $scope.VITALSIGNFLAG = true;
            }
        };

        $scope.checkAPPEARANCEFLAG = function (isShow) {
            if (isShow) {
                $scope.APPEARANCEFLAG = false;
            }
            else {
                $scope.APPEARANCEFLAG = true;
            }
        };

        $scope.checkBREATHPROBLEMFLAG = function (isShow) {
            if (isShow) {
                $scope.BREATHPROBLEMFLAG = false;
            }
            else {
                $scope.BREATHPROBLEMFLAG = true;
            }
        };

        $scope.checkNUTRITIONFLAG = function (isShow) {
            if (isShow) {
                $scope.NUTRITIONFLAG = false;
            }
            else {
                $scope.NUTRITIONFLAG = true;
            }
        };

        $scope.checkEXCRETIONFLAG = function (isShow) {
            if (isShow) {
                $scope.EXCRETIONFLAG = false;
            }
            else {
                $scope.EXCRETIONFLAG = true;
            }
        };

        $scope.checkSLEEPFLAG = function (isShow) {
            if (isShow) {
                $scope.SLEEPFLAG = false;
            }
            else {
                $scope.SLEEPFLAG = true;
            }
        };

        $scope.checkACTIVEFUNFLAG = function (isShow) {
            if (isShow) {
                $scope.ACTIVEFUNFLAG = false;
            }
            else {
                $scope.ACTIVEFUNFLAG = true;
            }
        };

        $scope.checkSKINFLAG = function (isShow) {
            if (isShow) {
                $scope.SKINFLAG = false;
            }
            else {
                $scope.SKINFLAG = true;
            }
        };


        $scope.checkFEELINGFLAG = function (isShow) {
            if (isShow) {
                $scope.FEELINGFLAG = false;
            }
            else {
                $scope.FEELINGFLAG = true;
            }
        };

        $scope.checkINTERACTIONFLAG = function (isShow) {
            if (isShow) {
                $scope.INTERACTIONFLAG = false;
            }
            else {
                $scope.INTERACTIONFLAG = true;
            }
        };

        $scope.checkALLERGYFLAG = function (isShow) {
            if (isShow) {
                $scope.ALLERGYFLAG = false;
            }
            else {
                $scope.ALLERGYFLAG = true;
            }
        };

        $scope.checkshowOther = function (isShow) {
            if (isShow) {
                $scope.showOther = false;
            }
            else {
                $scope.showOther = true;
            }
        };



        $scope.ChangeNextEvalDate = function () {
            if ($scope.Data.Demand.EVALDATE != "" && $scope.Data.Demand.NEXTEVALDATE != "") {
                var days = DateDiff($scope.Data.Demand.NEXTEVALDATE, $scope.Data.Demand.EVALDATE)
                if (days < 0) {
                    utility.message("下次评估日期不能小于评估日期");
                    $scope.Data.Demand.NEXTEVALDATE = "";
                    $scope.Data.Demand.Gap = '';
                } else {
                    $scope.Data.Demand.Gap = days;
                }
            };
        }

        $scope.Save = function () {
            if (!utility.Validation($scope.myForm.$error)) {
                return;
            }
            nurDemandEvalRes.save($scope.Data.Demand, function (data) {
                if (data.ResultCode == 0) {
                    utility.message("保存成功");
                    if ($scope.Data.Demand.NEXTEVALDATE) {
                        var assTask = {};
                        assTask.KEY = "NursingDemand";
                        assTask.NEXTEVALDATE = $scope.Data.Demand.NEXTEVALDATE;
                        assTask.NEXTEVALUATEBY = $scope.Data.Demand.NEXTEVALUATEBY;
                        assTask.FEENO = feeNo;
                        adminHandoversRes.assSave(assTask, function (data) { })
                    }
                    $location.url('/angular/NursingDemandList');
                } else {
                    utility.message(data.ResultMessage);
                }
            });
        }

        $scope.staffSelected = function (item, flag) {
            switch (flag) {
                case "EVALUATEBY": {
                    $scope.Data.Demand.EVALUATEBY = item.EmpNo;
                    break;
                }
                case "NEXTEVALUATEBY": {
                    $scope.Data.Demand.NEXTEVALUATEBY = item.EmpNo;
                    break;
                }
            }
        }

        $scope.UpdateEvalResult = function () {
            updateEval();
            utility.message("已为您带入最新评估结果!");
        }

        function updateEval(isUpdate) {
            nurDemandEvalRes.get({ feeNo: feeNo }, function (data) {
                $scope.Data.EvalResult = data.Data;
                for (var i = 0; i < $scope.Data.EvalResult.length; i++) {
                    //忧郁
                    if ($scope.Data.EvalResult[i].CODE == 'GDS') {
                        $scope.Data.Demand.YY_RESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.YYREF = $scope.Data.EvalResult[i].QueResult;
                    } else if ($scope.Data.EvalResult[i].CODE == 'ADL') {
                        //ADL
                        $scope.Data.Demand.ADLRESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.ADLSCORE = $scope.Data.EvalResult[i].SCORE;
                        $scope.Data.ADLREF = $scope.Data.EvalResult[i].QueResult;
                    }
                    else if ($scope.Data.EvalResult[i].CODE == 'MMSE') {
                        //MMSE 失智量表
                        $scope.Data.Demand.MMSEDESC = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.Demand.MMSESCORE = $scope.Data.EvalResult[i].SCORE;
                        $scope.Data.MMSEREF = $scope.Data.EvalResult[i].QueResult;
                    } else if ($scope.Data.EvalResult[i].CODE == 'IADL') {
                        //IADL 工具
                        $scope.Data.Demand.iADLRESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.iADLREF = $scope.Data.EvalResult[i].QueResult;
                    } else if ($scope.Data.EvalResult[i].CODE == 'KARNOFSKY') {
                        $scope.Data.Demand.KS_RESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.KSREF = $scope.Data.EvalResult[i].QueResult;
                    } else if ($scope.Data.EvalResult[i].CODE == 'FALL') {
                        //跌倒
                        $scope.Data.Demand.FALLRESULTS = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.FALLREF = $scope.Data.EvalResult[i].QueResult;
                    } else if ($scope.Data.EvalResult[i].CODE == 'SORE') {
                        $scope.Data.Demand.PRESSURESORE = $scope.Data.EvalResult[i].ENVRESULTS;
                        $scope.Data.PRESSUREREF = $scope.Data.EvalResult[i].QueResult;
                        //压疮
                    }
                }
            });

        }

    }
]);


angular.module("sltcApp").controller("nursingDemandHisCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'evalsheetRes', 'nurDemandHisRes',
    function ($scope, $http, $location, $state, dictionary, evalsheetRes, nurDemandHisRes) {
        $scope.Data = {};
        var residentName = $state.params.regName;
        var questionId = $state.params.qId;
        var feeNo = $state.params.feeNo;

        $scope.Data.QuestionId = questionId;
        $scope.Data.FeeNo = feeNo;
        $scope.Data.Filter =
       {
           'FeeNO': feeNo, 'Sex': '', "Id": 0, "ItemType": '', "RegNO": null, "Date": '', 'Name': '', 'CurrentPage': 0, 'PageSize': 0, 'TotalRecords': 0
       };
        $scope.init = function () {
            nurDemandHisRes.get({ FeeNO: feeNo }, function (response) {
                $scope.QuestionList = response.Data;
            });
        }
        $scope.init();//初始化

        $scope.Delete = function (Item) {
            if (confirm("确定删除该评估记录吗?")) {
                nurDemandHisRes.delete({ id: Item.ID }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.QuestionList.splice($scope.QuestionList.indexOf(Item), 1);
                    }
                });
            }
        }

        $scope.Edit = function (item) {
            var url = '/angular/NursingDemandSheet/' + item.FEENO + '/' + "" + '/' + item.ID;
            $location.url(url);
        }

        $scope.Preview = function (item) {
            window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("H35Report", item.ID, "", ""), "_blank");
        }

    }
]);




//function transferDate(originalDate) {
//    if (!isEmpty(originalDate)) {
//        var d = new Date(Date.parse(originalDate.replace(/-/g, "/"))).format("yyyy-MM-dd");;
//        return d;
//    }
//}
function transferDate(originalDate) {
    if (!isEmpty(originalDate)) {
        var d = new Date(Date.parse(originalDate)).format("yyyy-MM-dd");;
        return d;
    }
}
Date.prototype.format = function (format) {
    var date = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S+": this.getMilliseconds()
    };
    if (/(y+)/i.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }
    for (var k in date) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                   ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
        }
    }
    return format;
}

function isEmpty(value) {
    if (value == null || value == "" || value == "undefined" || value == undefined || value == "null") { return true; } else {  //value = value.replace(/\s/g, "");
        if (value == "") {
            return true;
        }
        return false;
    }
}
function isNumber(value) {
    if (isNaN(value)) {
        return false;
    }
    else {
        return true;
    }
}



function getScore(makerItemList) {
    var score = 0;
    $.each(makerItemList, function (i, item) {
        if (isNumber(item.LIMITEDVALUEID)) {
            for (var j = 0; j < item.Answers.length; j++) {
                if (item.Answers[j].LIMITEDVALUEID == item.LIMITEDVALUEID) {
                    score += item.Answers[j].LIMITEDVALUE;
                    break;
                }
            }
        }
    })
    return score;
}
function getEvalResult(evalSheet, score) {
    var result = "";
    $.each(evalSheet, function (i, item) {
        if (score >= item.LOWBOUND && score <= item.UPBOUND) {
            result = item.RESULTNAME; return false;
        }
    })
    return result
}

function getRightAnswerCount(answers, rightAnswer) {
    var result = 0;
    $.each(answers, function (i, item) {
        if (rightAnswer == item.MakerValue) {
            result++;
        }
    })
    return result
}

function getMakerItemListByCategory(arr) {
    var map = {}, dest = [];
    for (var i = 0; i < arr.length; i++) {
        var ai = arr[i];
        if (!map[ai.CATEGORY]) {
            dest.push({ CATEGORY: ai.CATEGORY, data: [ai] });
            map[ai.CATEGORY] = ai;
        } else {
            for (var j = 0; j < dest.length; j++)
            { var dj = dest[j]; if (dj.CATEGORY == ai.CATEGORY) { dj.data.push(ai); break; } }
        }
    }
    return dest;
}

//function getValidateMsg(name) {
//    switch (name) {
//        case '身高': case '体重': case '体温': case '脉搏次数': case '呼吸次数': case '血压/收缩': case '血压/舒张': case '脉搏': case '呼吸': case '耳温': case '低压': case '高压': return '必须为非负数字！'; break;
//    }
//}








