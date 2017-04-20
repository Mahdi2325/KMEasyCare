/*
 * Desc:RegEvaluate(社工评估)
 * Date:2016-3-24
 * Author:Dennis yang(杨金高)
 */
angular.module("sltcApp").controller("regEvalueateCtrl", ['$scope', '$state', 'dictionary', 'utility', 'regevaluateRes', 'empFileRes', 'personRes', 'adminHandoversRes',
    function ($scope, $state, dictionary, utility, regevaluateRes, empFileRes, personRes, adminHandoversRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        //当前住民管路对象
        $scope.currentPipeline = {};
        $scope.currentItem = {};
        //当前住民
        $scope.currentResident = {};
        //部分控件是否可见
        $scope.buttonShow = false;

        $scope.currentItem.Id = 0;

        //初始化函数(如加载下拉列表人员数据...)
        $scope.init = function () {
            $scope.Data.regEvaluates = {};
            empFileRes.get({ empNo: '' }, function (data) {
                $scope.Data.EmpList = data.Data;
            });
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: regevaluateRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.regEvaluates = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        }


        //获取住民评估记录
        $scope.getRegEvaluateByNo = function (feeNo) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = feeNo;
            $scope.options.search();
            $scope.curUser = utility.getUserInfo();
            $scope.currentItem.EvaluateBy = $scope.curUser.EmpNo;
            $scope.buttonShow = true;
            if ($scope.currentItem.EvalDate == "" || $scope.currentItem.EvalDate == null || $scope.currentItem.EvalDate == undefined)
                $scope.currentItem.EvalDate = new Date().format("yyyy-MM-dd");
        };

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.currentResident = resident;
            $scope.currentResident.FeeNo = resident.FeeNo;

            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.currentItem = {};
            $scope.getRegEvaluateByNo(resident.FeeNo);//加载当前住民的评估记录 需求名称
        };

        $scope.ChangeEvalDate = function () {
            if (isNumber($scope.currentItem.Gap && angular.isDefined($scope.currentItem.EvalDate))) {
                if ($scope.currentItem.Gap > 0) {
                    var currentDate = $scope.currentItem.EvalDate;
                    currentDate = currentDate.substring(0, 10);
                    $scope.currentItem.NextEvalDate = GetNextEvalDate(currentDate, $scope.currentItem.Gap);
                } else if ($scope.currentItem.Gap < 0) {
                    $scope.currentItem.Gap = '';
                    utility.message("间隔天数不能为负数");
                }
            }
            $scope.setNextValDate($scope.currentItem.Gap);
        };


        //设置下次间隔日期
        $scope.setNextValDate = function (gap) {
            if (isNumber(gap)) {
                if (gap >= 0 && angular.isDefined($scope.currentItem.EvalDate)) {
                    var currentDate = $scope.currentItem.EvalDate;
                    currentDate = currentDate.substring(0, 10);
                    $scope.currentItem.NextEvalDate = GetNextEvalDate(currentDate, gap);
                } else if (gap < 0) {
                    $scope.currentItem.Gap = '';
                    utility.message("间隔天数不能为负数");
                }
            }
        }

        //改变下次申请日期
        $scope.ChangeNextEvalDate = function () {
            if ($scope.currentItem.EvalDate != "" && $scope.currentItem.NextEvalDate != "") {
                var days = DateDiff($scope.currentItem.NextEvalDate, $scope.currentItem.EvalDate);
                if (days < 0) {
                    utility.message("下次评估日期不能小于本次评估日期");
                    $scope.currentItem.NextEvalDate = "";
                    $scope.currentItem.Gap = '';
                } else {
                    $scope.currentItem.Gap = days;
                }
            };
        }

        $scope.rowSelect = function (item) {
            if (angular.isDefined(item.Id)) {
                $scope.currentItem = item;
                if (item.NextEvalDate != null && item.EvalDate!=null)
                {
                    var Gap = DateDiff(new Date(item.NextEvalDate).format("yyyy-MM-dd"), new Date(item.EvalDate).format("yyyy-MM-dd"));
                    $scope.currentItem.Gap = Gap;
                }        //加载时自动计算时间差
            }
            $("#historyModal").modal("toggle");
        };

        $scope.deleteEvaluate = function (item) {
            if (angular.isDefined(item.Id)) {
                if (confirm("您确定要删除该条记录吗?")) {
                    regevaluateRes.delete({ id: item.Id }, function (data) {
                        utility.message("资料删除成功！");
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                    });
                }
            }
        };


        //选择填写人员
        $scope.staffSelected = function (item, flag) {
            if (flag == "selEvaluteBy") {
                $scope.currentItem.EvaluateBy = item.EmpNo;
            }
            else if (flag == "selNextEvaluator") {
                $scope.currentItem.NextEvaluateBy = item.EmpNo;
            }
        }

        $scope.staffSelected1 = function (item, flag) {
            $scope.currentItem.NextEvaluateBy = item.EmpNo;
        }


        //保存社工评估记录
        $scope.saveRegEvaluate = function (item) {
            if (!angular.isDefined($scope.currentResident.FeeNo))
            {
                utility.message("请选择住民！");
                return;
            }

            if ($scope.eaForm.$valid) {//判断验证通过后才可以保存
                $scope.currentItem.FeeNo = $scope.currentResident.FeeNo
                regevaluateRes.save(item, function (data) {

                    if (data.ResultCode == 0) {
                        utility.message("保存成功！");
                        $scope.currentItem.Id = data.Data.Id;
                        if (item.NextEvalDate) {
                            var assTask = {};
                            assTask.KEY = "EvaluateAdd";
                            assTask.NEXTEVALDATE = item.NextEvalDate;
                            assTask.NEXTEVALUATEBY = item.NextEvaluateBy;
                            assTask.FEENO = item.FeeNo;
                            adminHandoversRes.assSave(assTask, function (data) { })
                        }
                    }
                    else utility.message("保存失败，请联系管理员！");
                });
            }
            else {
                //验证没有通过
                $scope.getErrorMessage($scope.eaForm.$error);
                $scope.errs = $scope.errArr.reverse();
                for (var n = 0; n < $scope.errs.length; n++) {
                    if (n != 3) {
                        utility.msgwarning($scope.errs[n]);
                    }
                    //if (n > 2) break;
                }
            }

        };
        $scope.openHistory = function () {
            if ($scope.currentResident.FeeNo != '') {
                $scope.getRegEvaluateByNo($scope.currentResident.FeeNo);//加载当前住民的评估记录 需求名称

                // $("#historyModal").modal("toggle");
            }
        }

        $scope.resetForm = function (item) {
            $scope.currentItem = {};
            //重置用于评估新增会获取相关基本资料表信息，用于填充某些字段如个人专长．．．
            personRes.get({ id: $scope.currentResident.RegNo }, function (data) {
                //$scope.Data.Person = data.Data;
                $scope.currentItem.Expertise = data.Data.Skill;
                $scope.currentItem.MedicalHistory = data.Data.PersonalHistory;
            });
        }

        //验证信息提示
        $scope.getErrorMessage = function (error) {
            $scope.errArr = new Array();
            //var errorMsg = '';
            if (angular.isDefined(error)) {
                if (error.required) {
                    $.each(error.required, function (n, value) {
                        if (value.$name == "") {
                            value.$name = "评估人员"
                        }
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
