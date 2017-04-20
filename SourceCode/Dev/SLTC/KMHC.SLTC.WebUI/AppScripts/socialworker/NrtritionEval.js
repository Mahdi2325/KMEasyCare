//营养评估单
angular.module("sltcApp")
.controller("NutritionEvalListCtrl", ['$scope', '$http', '$state', 'utility', 'dictionary', 'NutritionRes', 'unPlanWeightRes', 'empFileRes', 'personRes','NutrtionEvalRes',
    function ($scope, $http, $state, utility, dictionary, NutritionRes, unPlanWeightRes, empFileRes, personRes, NutrtionEvalRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        //当前住民
        $scope.currentResident = {};
        //当前分页码
        $scope.currentPage = 1;
        $scope.buttonShow = false;

        $scope.init = function () {
            $scope.NutrHisFlag = true;

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: NutritionRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.NeList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        }

        $scope.changeHW = function () {
            $scope.NutrHisFlag = false;
        };

        $scope.changeDiag = function () {
            $scope.NutrHisFlag = true;
        };


        //获取个案营养评估列表
        $scope.getNutritionEvalList = function () {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = $scope.currentResident.FeeNo;
            $scope.options.search();

            var time1 = $scope.EvalDate1 == undefined ? "" : $scope.EvalDate1;
            var time2 = $scope.EvalDate2 == undefined ? "" : $scope.EvalDate2;
            //获取生化检查数据
            NutritionRes.get({ feeNo: $scope.currentResident.FeeNo, code1: "0029", code2: "0002", code3: "0003", s_date: time1, e_date: time2 }, function (data) {
                $scope.Data.Biochemistry = data.Data;
            });
        }


        $scope.date1 = "";
        //Add by Duke on 20160816
        $scope.checkEvalDate1 = function () {
            if (angular.isDefined($scope.EvalDate1) && angular.isDefined($scope.EvalDate2)) {
                if (!checkDate($scope.EvalDate1, $scope.EvalDate2)) {
                    utility.msgwarning("开始日期必须在结束日期之前");
                    $scope.EvalDate1 = $scope.date1;
                }
                else {
                    $scope.date1 = $scope.EvalDate1;
                }
            }

        }
        $scope.date2 = "";
        //Add by Duke on 20160816
        $scope.checkEvalDate2 = function () {
            if (angular.isDefined($scope.EvalDate1) && angular.isDefined($scope.EvalDate2)) {
                if (!checkDate($scope.EvalDate1, $scope.EvalDate2)) {
                    utility.msgwarning("结束日期必须在开始日期之后");
                    $scope.EvalDate2 = $scope.date2;
                }
                else {
                    $scope.date2 = $scope.EvalDate2;
                }
            }

        }
        //选中住民
        $scope.residentSelected = function (resident) {
            if (resident.BirthDay != null && resident.BirthDay != "") {
                $scope.Age = (new Date().getFullYear() - new Date(resident.BirthDay).getFullYear());
            }
            else {
                $scope.Age = "";
            }
            $scope.currentResident = resident;

            $scope.currentItem = {};//清空编辑项

            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.FeeNo = resident.FeeNo;


            $scope.Height = resident.Height;
            $scope.Weight = resident.Weight;
            var iw = null;
            $scope.iDealWeight = "";

            if ($scope.Height != null && $scope.Weight != null) {
                $scope.iDealWeight = Math.round(22 * $scope.Height * $scope.Height / 100) / 100;
            }

            $scope.curUser = utility.getUserInfo();

            $scope.EvalDate1 = moment((new Date().getFullYear() - 1) + "-" + (moment().format("MM")) + '-' + moment().format("DD")).format("YYYY-MM-DD");
            var strday = new Date().getDate() + 1 ;
            if (strday >= 0 && strday <= 9) {
                strday = "0" + strday;
            }
            $scope.EvalDate2 = moment((new Date().getFullYear()) + "-" + (moment().format("MM")) + '-' + strday).format("YYYY-MM-DD");

            $scope.getUnplanWeight(resident.RegNo, resident.FeeNo);

            //$scope.getPerson(resident.RegNo);

            $scope.getNutritionEvalList();

            $scope.currentItem.DISEASEDIAG = resident.DiseaseDiag;
        }
        //获取非计划减重
        $scope.getUnplanWeight = function (regNo, feeNo) {
            NutrtionEvalRes.get({ FeeNo: $scope.currentResident.FeeNo, startDate: $scope.EvalDate1, endDate: $scope.EvalDate2, }, function (data) {
                $scope.Data.UnPlans = [];
                data.Data.forEach(function (item) {
                    $scope.Data.iUnPlan = {};
                    $scope.Data.iUnPlan.ThisWeight = item.CURRENTWEIGHT;
                    $scope.Data.iUnPlan.BMI = item.BMI;
                    $scope.Data.iUnPlan.ThisRecDate = item.RECORDDATE;
                    $scope.Data.UnPlans.push($scope.Data.iUnPlan);
                });

                if (data.Data.length > 0) {
                    $scope.Data.UnPlan = {};
                    $scope.Data.UnPlan.ThisWeight = data.Data[0].CURRENTWEIGHT;
                    $scope.iDealWeight = data.Data[0].IDEALWEIGHT;
                    $scope.Data.UnPlan.ThisHeight = data.Data[0].HEIGHT;
                    $scope.Data.UnPlan.ThisHeight = data.Data[0].BMI;
                }

            });


            unPlanWeightRes.get({ date1: $scope.EvalDate1, date2: $scope.EvalDate2, regNo: $scope.currentResident.RegNo, feeNo: $scope.currentResident.FeeNo }, function (data) {
                if (angular.isDefined(data.Data)) {
                    $scope.Data.UnPlans = data.Data;
                }
            });


            //unPlanWeightRes.get({ date1: $scope.EvalDate1, date2: $scope.EvalDate2, regNo: $scope.currentResident.RegNo, feeNo: $scope.currentResident.FeeNo }, function (data) {

            //    if (angular.isDefined(data.Data)) {
            //        $scope.Data.UnPlans = data.Data;
            //        if($scope.Data.UnPlans.length>0)
            //        {
            //            $scope.Data.UnPlan = $scope.Data.UnPlans[0];

            //            //理想体重范围：22*身高的平方±10%
            //            if (angular.isNumber($scope.Data.UnPlan.ThisHeight))
            //            {
            //                var idWeight = Math.round((22 * $scope.Data.UnPlan.ThisHeight * $scope.Data.UnPlan.ThisHeight) / 10000);
            //                $scope.iDealWeight = idWeight.toString() + " ± " + (Math.round(idWeight * 0.1)).toString();
            //            }
                        
            //        }
            //        else
            //        {
            //            $scope.Data.UnPlan = {};
            //            $scope.iDealWeight = "";
            //        }
            //    }
            //});
        };
        //获取人员基本信息
        //$scope.getPerson = function (regNo) {
        //    personRes.get({ regNo: 61 }, function (returnData) {
        //        if (angular.isDefined(returnData.Data)) {
        //            $scope.Data.Person = returnData.Data;

        //        }
        //    });
        //}
        $scope.resetNutritionEval = function () {
            $scope.currentItem = {};
            $scope.FeeNo = $scope.currentResident.FeeNo;
        }

        $scope.queryData = function () {
            var time1 = $scope.EvalDate1 == undefined ? "" : $scope.EvalDate1;
            var time2 = $scope.EvalDate2 == undefined ? "" : $scope.EvalDate2;
            //获取生化检查数据
            NutritionRes.get({ feeNo: $scope.currentResident.FeeNo, code1: "0029", code2: "0002", code3: "0003", s_date: time1, e_date: time2 }, function (data) {
                $scope.Data.Biochemistry = data.Data;
            });

            //获取非计划减重
            unPlanWeightRes.get({ date1: time1, date2: time2, regNo: $scope.currentResident.RegNo, feeNo: $scope.currentResident.FeeNo }, function (data) {
                if (angular.isDefined(data.Data)) {
                    $scope.Data.UnPlans = data.Data;
                }
            });
        }
        $scope.saveNutritionEval = function (item) {

            if ($scope.neForm.$valid) {//判断验证通过后才可以保存
                item.NAME = $scope.currentResident.Name;
                item.BIRTHDATE = $scope.currentResident.BirthDay;
                item.SEX = $scope.currentResident.Sex;
                item.RESIDENTNO = $scope.currentResident.ResidentNo;
                item.FEENO = $scope.FeeNo;
                item.BEDNO = $scope.currentResident.BeDNo;
                item.INDATE = $scope.currentResident.InDate;
                NutritionRes.save(item, function (data) {
                    if (angular.isDefined(item.Id)) {
                        $scope.resetNutritionEval();
                        utility.message("资料更新成功！");
                    }
                    else {
                        //$scope.Data.NeList.push(data.Data);
                        $scope.resetNutritionEval();
                        $scope.getNutritionEvalList();
                        utility.message("资料保存成功！");
                    }
                });

            }
            else {
                //验证没有通过
                $scope.getErrorMessage($scope.neForm.$error);
                $scope.errs = $scope.errArr.reverse();
                for (var n = 0; n < $scope.errs.length; n++) {
                    if (n < 3) {
                        utility.msgwarning($scope.errs[n]);
                    }
                }
            }


        };
        //编辑当前行
        $scope.rowSelect = function (item) {
            $scope.currentItem = item;
        }
        //删除当前行数据
        $scope.deleteNutritionEval = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                NutritionRes.delete({ id: item.ID }, function (data) {
                    if (data.$resolved) {
                        //var whatIndex = null;
                        //angular.forEach($scope.Data.NeList, function (cb, index) {
                        //    if (cb.id = item.ID) whatIndex = index;
                        //});

                        if (data.ResultCode == 0)
                            utility.message("资料删除成功！");
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                    }
                });
            }
        };

        $scope.PrintPreview = function (item) {
            window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("DLNCareReport", item.ID, item.EVALDATE, ''), "_blank");
        };


        //验证信息提示
        $scope.getErrorMessage = function (error) {
            $scope.errArr = new Array();
            //var errorMsg = '';
            if (angular.isDefined(error)) {
                if (error.required) {
                    $.each(error.required, function (n, value) {
                        $scope.errArr.push(value.$name + "不能为空");
                    });
                }
                if (error.email) {
                    $.each(error.email, function (n, value) {
                        $scope.errArr.push(value.$name + "邮箱验证失败");
                    });
                }
                if (error.number, function (n, value) {
                    $scope.errArr.push(value.$name + "只能录入数字");
                });

                if (error.maxlength) {
                    $.each(error.maxlength, function (n, value) {
                        $scope.errArr.push(value.$name + "录入已超过最大设定长度！");

                    });
                }
                //return errorMsg;
            }
        }
        $scope.init();
    }]);

