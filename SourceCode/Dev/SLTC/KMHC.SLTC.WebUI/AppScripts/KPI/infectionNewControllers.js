/*
创建人:刘美方
创建日期:2016-07-5
说明: 感染
*/
angular.module("sltcApp")
.controller("infectionNewCtrl", ['$scope', '$state', '$filter', '$http', 'utility', 'infectionRes', 'infectionItemRes', 'symptomItemRes', 'labExamRes','infectionSympotmRes',
    function ($scope, $state, $filter, $http, utility, infectionRes, infectionItemRes, symptomItemRes, labExamRes, infectionSympotmRes) {
        $scope.FeeNo = $state.params.FeeNo;
        var nowDate = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss");
        $scope.dicInfection = [];
        $scope.dicSymptom = [];
        //$scope.chooseSymptom = [];
        $scope.init = function () {
            $scope.curUser = utility.getUserInfo();
            $scope.curResident = $scope.curResident || {};
            $scope.initData();
            $scope.curDetail = {};
            $scope.editShow = false;
            $scope.editDetailShow = false;
            $scope.list = [];
            $scope.options = {
                buttons: [], 
                ajaxObject: infectionRes, 
                params: { regNo: 0, feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo },
                success: function (data) { 
                    $scope.list = data.Data;
                    //if ($scope.list.length > 0) {
                    //    $scope.editDetailShow = true;
                    //    if (angular.isDefined($scope.curItem.SeqNo)) {
                    //        $scope.edit($scope.curItem);
                    //    } else {
                    //        $scope.edit($scope.list[0]);
                    //    }
                    //} else {
                    //    $scope.editDetailShow = false;
                    //    $scope.initData();
                    //}
                },
                pageInfo: {
                    CurrentPage: 1,
                    PageSize: 10
                }
            }
            $scope.LabExamList = [];
            $scope.detailOptions = {
                buttons: [], 
                ajaxObject: labExamRes, 
                params: { seqNo: 0 },
                success: function (data) { 
                    $scope.LabExamList = data.Data;
                },
                pageInfo: {
                    CurrentPage: 1,
                    PageSize: 10
                }
            }
            $scope.symptomOptions = {
                buttons: [], 
                ajaxObject: infectionSympotmRes,
                params: { seqNo: 0 },
                success: function (data) { 
                    $scope.symptomList = data.Data;
                    $scope.accoutScore();
                },
                pageInfo: {
                    CurrentPage: 1,
                    PageSize: 100
                }
            }

            infectionItemRes.get({}, function (data) {
                $scope.dicInfection = data.Data;
            });
            symptomItemRes.get({ ItemCode: "" }, function (data) {
                $scope.dicSymptomAll = data.Data;
            });
        };

        $scope.chooseSymptom = [];
        $scope.x = false; //默认未选中
        $scope.all = function (c) { //全选
            $scope.chooseSymptom = [];
            if (c) {
                $scope.x = true;
                angular.forEach($scope.symptomList, function (data) {
                    $scope.chooseSymptom.push(data.Id);
                });
            } else {
                $scope.x = false;
            }
        };
        $scope.chk = function (item, x) { //单选或者多选
            if (x) {
                $scope.chooseSymptom.push(item.Id);
            } else {
                angular.forEach($scope.symptomList, function (data) {
                    if (data.Id === item.Id) {
                        $scope.chooseSymptom.splice($scope.chooseSymptom.indexOf(item.Id), 1);
                    }
                });
            }
        };

        $scope.ItemChange = function (id) {
            if (angular.isDefined(id)) {
                $scope.dicSymptom = $.grep($scope.dicSymptomAll, function(bp) {
                    return bp.ItemCode === id;
                });
            } else {
                $scope.dicSymptom = [];
            }
        };

        $scope.accoutScore = function () {
            var score = 0;
            var infectionScore = $scope.getInfectionItemScore($scope.curItem.ItemNo);
            angular.forEach($scope.symptomList, function (data) {
                score+=$scope.getSympotmScore(data.Sympotm);
            });
            $scope.curItem.ItemScore = score;
            if ($scope.curItem.ItemScore > infectionScore) {
                $scope.curItem.IfcFlag = true;
            } else {
                $scope.curItem.IfcFlag = false;
            }
        };

        $scope.getSympotmScore = function (id) {
            for (var i = 0; i < $scope.dicSymptomAll.length; i++) {
                if ($scope.dicSymptomAll[i].SympotmCode === id) {
                    return $scope.dicSymptomAll[i].Score;
                }
            }
            return 0;
        };

        $scope.getInfectionItemScore = function (id) {
            for (var i = 0; i < $scope.dicInfection.length; i++) {
                if ($scope.dicInfection[i].ItemCode === id) {
                    return $scope.dicInfection[i].Score;
                }
            }
            return 0;
        };

        $scope.symptomChange = function(type) {
            if (type === "right") { //添加证状
                var temp = [];
                $("#sltFrom option:selected").each(function() {
                    temp.push({
                        SeqNo: $scope.curItem.SeqNo,
                        OccurDate: $scope.OccurDate,
                        CreateDate: nowDate,
                        CreateBy: $scope.curUser.EmpNo,
                        ItemNo: $scope.curItem.ItemNo,
                        Sympotm: this.value,
                        ItemName: this.text,
                        id: 0
                    });
                });
                //增加
                if (temp.length === 0) {
                    utility.message("请选择症状！");
                    return;
                }
                if (angular.isDefined($scope.curItem.SeqNo)) {
                    infectionSympotmRes.save(temp, function(data) {
                        $scope.symptomOptions.pageInfo.CurrentPage = 1;
                        $scope.symptomOptions.search();
                    });
                } else {
                    if (angular.isDefined($scope.form1.$error.required)) {
                        for (var i = 0; i < $scope.form1.$error.required.length; i++) {
                            utility.msgwarning($scope.form1.$error.required[i].$name + "为必填项！");
                            if (i > 1) {
                                return;
                            }
                        }
                        return;
                    }

                    if (angular.isDefined($scope.form1.$error.maxlength)) {
                        for (var i = 0; i < $scope.form1.$error.maxlength.length; i++) {
                            utility.msgwarning($scope.form1.$error.maxlength[i].$name + "超过设定长度！");
                            if (i > 1) {
                                return;
                            }
                        }
                        return;
                    }
                    infectionRes.save($scope.curItem, function (data) {
                        $scope.curItem = data.Data;
                        $scope.options.search();
                        $scope.symptomOptions.params.seqNo = $scope.curItem.SeqNo;
                        $scope.detailOptions.params.seqNo = $scope.curItem.SeqNo;
                        for (var i = 0; i < temp.length; i++) {
                            temp[i].SeqNo = $scope.curItem.SeqNo;
                        }
                        infectionSympotmRes.save(temp, function (data) {
                            $scope.symptomOptions.pageInfo.CurrentPage = 1;
                            $scope.symptomOptions.search();
                            $scope.all(false);
                            $scope.allFlag = false;
                            //utility.message("保存成功！");
                        });
                    });
                }


            } else { //减少员工
                if ($scope.chooseSymptom.length === 0) {
                    utility.message("请选择要删除症状！");
                    return;
                }
                infectionSympotmRes.delete({ ids: $scope.chooseSymptom.join(',') }, function (data) {
                    $scope.all(false);
                    $scope.allFlag = false;
                    //$scope.chooseSymptom = [];
                    $scope.symptomOptions.pageInfo.CurrentPage = 1;
                    $scope.symptomOptions.search();
                });
            }
        };

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.curResident = resident; //获取当前住民信息
            $scope.initData();
            $scope.options.params.feeNo = $scope.curItem.FeeNo;
            $scope.options.params.regNo = $scope.curItem.RegNo;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
            $scope.editShow = true;
            $scope.editDetailShow = false;
            $scope.LabExamList = [];
        };

        $scope.staffSelected = function (item, t) {
            if (t === "RecordBy") {
                $scope.curItem.RecordBy = item.EmpNo;
            }
        };


        $scope.edit = function (item) {
            $scope.curItem = item;
            $scope.ItemChange(item.ItemNo);
            $scope.editDetailShow = true;
            $scope.symptomOptions.params.seqNo = item.SeqNo;
            $scope.symptomOptions.search();
            $scope.detailOptions.params.seqNo = item.SeqNo;
            $scope.detailOptions.search();
        };

        $scope.delete = function (id) {
            if (confirm("确定删除该信息吗?")) {
                infectionRes.delete({ id: id }, function (data) {
                    $scope.initData();
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            }
        };


        $scope.checkDetailDate = function() {
            if ($scope.curItem.ExamDate != "" && $scope.curItem.RptDate != "") {
                var days = DateDiff($scope.curItem.RptDate, $scope.curItem.ExamDate);
                if (days < 0) {
                    utility.message("报告回复日期不能小于送检日期");
                    $scope.curItem.ExamDate = "";
                    $scope.curItem.RptDate = "";
                    return;
                }
            }
        };
        $scope.save = function () {
            if ($scope.curItem.IfcType == null) {
                utility.message("请选择感染类型！");
                return;
            }
            if ($scope.curItem.IfcDate == null) {
                utility.message("请选择感染日期！");
                return;
            }
            if ($scope.curItem.Category==null) {
                utility.message("请选择分类统计！");
                return;
            }

            if ($scope.curItem.SecStartDate != "" && $scope.curItem.SecEndDate != "") {
                var days = DateDiff($scope.curItem.SecEndDate, $scope.curItem.SecStartDate);
                if (days < 0) {
                    utility.message("结束日期不能小于隔离日期");
                    $scope.curItem.SecEndDate = "";
                    $scope.curItem.SecDays = "";
                    return;
                }
            }

            if ($scope.curItem.ItemNo == null) {
                utility.message("请选择感染项目！");
                return;
            }

            if (angular.isDefined($scope.form1.$error.maxlength)) {
                for (var i = 0; i < $scope.form1.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.form1.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            infectionRes.save($scope.curItem, function (data) {
                $scope.curItem = data.Data;
                $scope.options.search();
                $scope.reset();
                utility.message("保存成功！");
            });
        };
        $scope.reset = function () {
            $scope.initData();
            $scope.editDetailShow = false;
        };

        $scope.editDetail = function (item) {
            $scope.curDetail = item;
            $("#modalDetail").modal("toggle");
        };
        $scope.delDetail = function (item) {
            if (confirm("您确定要删除该住民的疼痛记录吗?")) {
                labExamRes.delete({ id: item.Id }, function (data) {
                    $scope.detailOptions.pageInfo.CurrentPage = 1;
                    $scope.detailOptions.search();
                    utility.message("删除成功");
                });
            }
        };
        $scope.saveDetail = function () {
            $scope.checkDetailDate();
            $scope.curDetail.SeqNo = $scope.curItem.SeqNo;
            labExamRes.save($scope.curDetail, function (data) {
                $("#modalDetail").modal("toggle");
                $scope.detailOptions.search();
            });
        };

        $scope.initData = function() {
            $scope.curItem = {
                RecDate: nowDate,
                RecordBy: $scope.curUser.EmpNo,
                RegNo: $scope.curResident.RegNo,
                FeeNo: $scope.curResident.FeeNo
            };
            $scope.OccurDate = nowDate;
            $scope.dicSymptom = [];
            $scope.symptomList = [];
            $scope.chooseSymptom = [];
        };

        $scope.showItemName = function (id) {
            for (var i = 0; i < $scope.dicInfection.length; i++) {
                if ($scope.dicInfection[i].ItemCode === id) {
                    return $scope.dicInfection[i].ItemName;
                }
            }
            return "";
        };

        $scope.setNextValDate = function (secDays) {
            if (isNumber(secDays)) {
                if (secDays > 0) {
                    var currentDate = $scope.curItem.SecStartDate;
                    currentDate = currentDate.substring(0, 10);
                    $scope.curItem.SecEndDate = GetNextEvalDate(currentDate, secDays);
                } else if (secDays < 0) {
                    $scope.curItem.SecDays = '';
                    utility.message("天数不能为负数");
                }
            }
        }

        $scope.changeSecStartDate = function () {
            if (isNumber($scope.curItem.SecDays)) {
                if ($scope.curItem.SecDays > 0) {
                    var currentDate = $scope.curItem.SecStartDate;
                    currentDate = currentDate.substring(0, 10);
                    $scope.curItem.SecEndDate = GetNextEvalDate(currentDate, $scope.curItem.SecDays);
                } else if ($scope.curItem.SecDays < 0) {
                    $scope.curItem.SecDays = '';
                    utility.message("天数不能为负数");
                }
            }
            $scope.setNextValDate($scope.curItem.SecDays);
        }

        $scope.changeSecEndDate = function () {
            if ($scope.curItem.SecStartDate != "" && $scope.curItem.SecEndDate != "") {
                var days = DateDiff($scope.curItem.SecEndDate, $scope.curItem.SecStartDate);
                if (days < 0) {
                    utility.message("结束日期不能小于隔离日期");
                    $scope.curItem.SecEndDate = "";
                    $scope.curItem.SecDays = "";
                } else {
                    $scope.curItem.SecDays = days;
                }
            };
        }

        $scope.init();
    }]);





