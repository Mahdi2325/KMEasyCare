/*
创建人:张凯
创建日期:2016-03-15
说明: 团体活动 groupActivityRes
      成员个别化活动评估 memberAssessRes
*/
angular.module("sltcApp")
.controller("groupActivityListCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'groupActivityRes', function ($scope, $http, $location, $stateParams, utility, groupActivityRes) {

    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: groupActivityRes,//异步请求的res
        params: { activityName: "" },
        success: function (data) {//请求成功时执行函数
            $scope.ItemList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }

    $scope.ItemDelete = function (item) {
        if (confirm("您确定要删除团体活动信息吗?")) {
            groupActivityRes.delete({ id: item.Id }, function () {
                $scope.ItemList.splice($scope.ItemList.indexOf(item), 1);
                utility.message($scope.ActivityName + "的团体活动信息已删除！");
            });
        }
    };
}])
.controller("groupActivityEditCtrl", ['$scope', '$http', '$location', '$filter', '$state', 'utility', 'groupActivityRes', 'webUploader', 'evalsheetRes', 'empFileRes', 'evaluationRes', 'groupActivityEvalRes', 'nursingRecordRes', function ($scope, $http, $location, $filter, $state, utility, groupActivityRes, webUploader, evalsheetRes, empFileRes, evaluationRes, groupActivityEvalRes, nursingRecordRes) {
    $scope.Data = {};
    $scope.RegQuestionList = [];
    $scope.Maker = {};
    $scope.RegQuestion = {};
    $scope.ActivityData = {};
    $scope.record = {};
    $scope.record.Content = "";
    $scope.init = function () {
        $scope.maxErrorTips = 3;

        if ($state.params.id) {
            groupActivityRes.get({ id: $state.params.id }, function (data) {
                $scope.Data = data.Data;
                evalsheetRes.get({ Code: 'ACTIVITY' }, function (response) {
                    if (response.Data != null) {
                        $scope.ActivityData = response.Data;
                        $scope.Maker = $scope.ActivityData.MakerItemList;
                    }
                    empFileRes.get({ empNo: '' }, function (data) {
                        $scope.EmpList = data.Data;
                    });
                    if (!isEmpty($scope.Data.AttendNo) && !isEmpty($scope.Data.AttendName)) {
                        var arr1 = $scope.Data.AttendNo.split(',');
                        var arr2 = $scope.Data.AttendName.split(',');
                        var arr = [];
                        for (var i = 0; i < arr1.length; i++) {
                            arr.push({ Name: arr2[i], FeeNo: arr1[i] })
                        }
                        $scope.InitEvalList(arr);
                    }
                });
            });
        } else {
            evalsheetRes.get({ Code: 'ACTIVITY' }, function (response) {
                $scope.ActivityData = response.Data;
                $scope.Maker = $scope.ActivityData.MakerItemList;
                empFileRes.get({ empNo: '' }, function (data) {
                    $scope.EmpList = data.Data;
                })
            });
            $scope.RegQuestion.EVALDATE = $filter("date")(new Date(), "yyyy-MM-dd");
        }
        $scope.disabledSaveEval = true;

    }



    $scope.calcResult = function () {
        evaluationRes.save($scope.ActivityData, function (response) {
            $scope.EvalResult = response;
            $scope.RegQuestion.SCORE = $scope.EvalResult.Score;
            $scope.RegQuestion.ENVRESULTS = $scope.EvalResult.Result;
        });
    }

    $scope.calcSubResult = function (data) {
        evaluationRes.save(data, function (response) {
            var calResult = response;
            data.SCORE = calResult.Score;
            data.ENVRESULTS = calResult.Result;
        });
    }



    $scope.ChangeEvalBy = function (Item) {
        $scope.RegQuestion.EVALUATEBY = Item.EmpNo;
        $scope.RegQuestion.EVALUATEName = Item.EmpName;
    }


    $scope.saveActivity = function () {
        if (angular.isDefined($scope.myForm.$error.pattern)) {
            for (var i = 0; i < $scope.myForm.$error.pattern.length; i++) {
                var name = $scope.myForm.$error.pattern[i].$name, msg;
                switch (name) {
                    case "评估总平均":
                        msg = "必须为整数!";
                }
                utility.msgwarning(name + msg);
            }
        }
        if (!utility.Validation($scope.myForm.$error)) {
            return;
        }

        groupActivityRes.save($scope.Data, function (data) {
            $location.url('/angular/GroupActivityList');
            utility.message($scope.Data.ActivityName + "的团体活动信息保存成功！");
        });
    }



    $scope.ImportFromTem = function () {

        $scope.RegQuestion.QuestionDataList = [];
        for (var i = 0; i < $scope.Maker.length; i++) {
            var questionData = {};
            questionData.QUESTIONID = $scope.ActivityData.QUESTIONID;
            questionData.MAKERID = $scope.Maker[i].MAKERID;
            questionData.EVALUATEBY = $scope.RegQuestion.EVALUATEBY;
            questionData.LIMITEDVALUEID = $scope.Maker[i].LIMITEDVALUEID;
            $scope.RegQuestion.MakerItemList = $scope.Maker;
            $scope.RegQuestion.QuestionDataList.push(questionData);
        }

    }

    $scope.BatchImport = function () {
        if (!utility.Validation($scope.myForm1.$error)) {
            return;
        }
        $scope.ImportFromTem();
        for (var i = 0; i < $scope.RegQuestionList.length; i++) {
            $scope.RegQuestionList[i].QuestionDataList = angular.copy($scope.RegQuestion.QuestionDataList);
            $scope.RegQuestionList[i].MakerItemList = angular.copy($scope.RegQuestion.MakerItemList);
            $scope.RegQuestionList[i].SCORE = angular.copy($scope.RegQuestion.SCORE);
            $scope.RegQuestionList[i].ENVRESULTS = angular.copy($scope.RegQuestion.ENVRESULTS);
            $scope.RegQuestionList[i].EVALUATEBY = angular.copy($scope.RegQuestion.EVALUATEBY);
            $scope.RegQuestionList[i].EVALUATEName = angular.copy($scope.RegQuestion.EVALUATEName);
            $scope.RegQuestionList[i].EVALDATE = angular.copy($scope.RegQuestion.EVALDATE);
            $scope.RegQuestionList[i].DESCRIPTION = "团体活动批量保存";
        }
        if ($scope.RegQuestionList.length > 0) {
            $scope.disabledSaveEval = false;
            utility.message("导入模板数据成功!");
        } else {
            utility.message("请先选择参与人员!");
        }
    }
    $scope.nurseStaffSelected = function (item) {
        $scope.record.RecordBy = item.EmpNo;
    }

    $scope.subNurseStaffSelected = function (item, Item) {
        Item.RecordBy = item.EmpNo;
    }

    $scope.BatchImportNursingData = function (record) {

        angular.forEach($scope.NursingList, function (item) {
            item.RecordDate = record.RecordDate;
            item.RecordBy = record.RecordBy;
            item.ClassType = record.ClassType;
            item.Content = record.Content;
            item.PrintFlag = record.PrintFlag;
            item.SynchronizeFlag = record.SynchronizeFlag;
            item.IsSave = true;
        })
    }

    $scope.SaveBatchEval = function () {
        if ($scope.RegQuestionList.length > 0) {
            groupActivityEvalRes.save({ Data: $scope.RegQuestionList }, function (res) {
                $scope.disabledSaveEval = true;
                utility.message("保存成功!");
            })
        } else {
            utility.message("参与人员不能为空!");
        }

    }
    $scope.SaveBatchNursing = function () {
        $scope.NursingRecList = {};
        $scope.NursingRecList.list = [];
        if ($scope.NursingList.length > 0) {
            for (var i = 0; i < $scope.NursingList.length; i++) {
                if ($scope.NursingList[i].RecordDate == "") {
                    utility.message($scope.NursingList[i].Name + "的记录时间不能为空!");
                    return;
                }
                if ($scope.NursingList[i].RecordBy == "") {
                    utility.message($scope.NursingList[i].Name + "的护理人员不能为空!");
                    return;
                }
                if ($scope.NursingList[i].ClassType == "") {
                    utility.message($scope.NursingList[i].Name + "的班别类型不能为空!");
                    return;
                }
                if ($scope.NursingList[i].Content == "") {
                    utility.message($scope.NursingList[i].Name + "的记录内容不能为空!");
                    return;
                }
                if ($scope.NursingList[i].IsSave) {
                    $scope.NursingRecList.list.push($scope.NursingList[i]);
                }
            }
            if ($scope.NursingRecList.list.length == 0)
            {
                utility.message("无数据保存!");
                return;
            }
            nursingRecordRes.saveBatch($scope.NursingRecList, function (data) {
                if (data.ResultCode == 1) {
                    $scope.NursingList = $scope.InitNursingList;
                    utility.message("保存成功！");
                }
            })
        }
    }
    $scope.InitEvalList = function (data, add) {
        $scope.RegQuestionList = [];
        $scope.NursingList = [];
        $scope.InitNursingList = [];
        $scope.ImportFromTem();
        for (var i = 0; i < data.length; i++) {
            var d = angular.copy($scope.RegQuestion);
            d.FEENO = data[i].FeeNo;
            d.NAME = data[i].Name;
            d.QUESTIONNAME = "团体活动";
            d.QUESTIONID = $scope.ActivityData.QUESTIONID;
            $scope.RegQuestionList.push(d);

            var recordItem = angular.copy($scope.record);
            recordItem.FeeNo = data[i].FeeNo;
            recordItem.Name = data[i].Name;
            recordItem.RegNo = data[i].RegNo;
            recordItem.RecordDate = "";
            recordItem.RecordBy = "";
            recordItem.ClassType = "";
            recordItem.Content = "";
            $scope.NursingList.push(recordItem);
            if (add) {
                if (i == 0) {
                    $scope.Data.AttendName = data[i].Name;
                    $scope.Data.AttendNo = data[i].FeeNo;
                } else {
                    $scope.Data.AttendName += ',' + data[i].Name;
                    $scope.Data.AttendNo += ',' + data[i].FeeNo;
                }
            }
        }
        $scope.InitNursingList = angular.copy($scope.NursingList);
    }

    $scope.$on('transMember', function (event, data) {
        $scope.InitEvalList(data, true);
        if ($scope.RegQuestionList.length > 0) {
            $scope.Data.AttendNumber = $scope.RegQuestionList.length;
            $scope.disabledSaveEval = false;
        }
        $('#modalChooseResident').modal('hide');
    });

    //选择填写人员
    $scope.staffSelected = function (item, type) {
        if (type === "Leader") {
            $scope.Data.LeaderName = item.EmpNo;
        } else if (type === "Leader1") {
            $scope.Data.LeaderName1 = item.EmpNo;
        } else if (type === "Leader2") {
            $scope.Data.LeaderName2 = item.EmpNo;
        } else if (type === "InputName") {
            $scope.Data.CreateBy = item.EmpNo;
        }
    }
    $scope.init();

    webUploader.init('#Picture1PathPicker', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
        if (data.length > 0) {
            $scope.Data.Picture1 = data[0].SavedLocation;
            $scope.$apply();
        }
    });
    webUploader.init('#Picture2PathPicker', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
        if (data.length > 0) {
            $scope.Data.Picture2 = data[0].SavedLocation;
            $scope.$apply();
        }
    });



    $scope.clear = function (type) {

        switch (type) {
            case "PhotoPicker1":
                $scope.Data.Picture1 = "";

                break;
            case "PhotoPicker2":
                $scope.Data.Picture2 = "";

                break;
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
        if (angular.isDefined($scope.myForm.$error.number)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.number.length; i++) {
                msg = $scope.myForm.$error.number[i].$name + "必须为数字 ";
                utility.msgwarning(msg);
                errorTips++;
                if (errorTips >= $scope.maxErrorTips) {
                    return false;
                }
            }
        }

        if (angular.isDefined($scope.myForm.$error.pattern)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.pattern.length; i++) {
                msg = $scope.myForm.$error.pattern[i].$name + "格式不正确 ";
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

    $scope.$watch("Data.ActivityName", function (newValue) {
        if (!angular.isDefined($scope.Data.GroupCompetence)) $scope.Data.GroupCompetence = "";
        if (!angular.isDefined($scope.Data.IndividualSummary)) $scope.Data.IndividualSummary = "";
        if (!angular.isDefined($scope.Data.Suggestion)) $scope.Data.Suggestion = "";
        $scope.record.Content = "活动名称:" + newValue + "\n活动流程团体动力:" + $scope.Data.GroupCompetence + "\n特殊个案个别纪要:" + $scope.Data.IndividualSummary + "\n成果评估(建议改进事项):" + $scope.Data.Suggestion;
    })
    $scope.$watch("Data.GroupCompetence", function (newValue) {
        if (!angular.isDefined($scope.Data.ActivityName)) $scope.Data.ActivityName = "";
        if (!angular.isDefined($scope.Data.IndividualSummary)) $scope.Data.IndividualSummary = "";
        if (!angular.isDefined($scope.Data.Suggestion)) $scope.Data.Suggestion = "";
        $scope.record.Content = "活动名称:" + $scope.Data.ActivityName + "\n活动流程团体动力:" + newValue + "\n特殊个案个别纪要:" + $scope.Data.IndividualSummary + "\n成果评估(建议改进事项):" + $scope.Data.Suggestion;
    })
    $scope.$watch("Data.IndividualSummary", function (newValue) {
        if (!angular.isDefined($scope.Data.ActivityName)) $scope.Data.ActivityName = "";
        if (!angular.isDefined($scope.Data.GroupCompetence)) $scope.Data.GroupCompetence = "";
        if (!angular.isDefined($scope.Data.Suggestion)) $scope.Data.Suggestion = "";
        $scope.record.Content = "活动名称:" + $scope.Data.ActivityName + "\n活动流程团体动力:" + $scope.Data.GroupCompetence + "\n特殊个案个别纪要:" + newValue + "\n成果评估(建议改进事项):" + $scope.Data.Suggestion;
    })
    $scope.$watch("Data.Suggestion", function (newValue) {
        if (!angular.isDefined($scope.Data.ActivityName)) $scope.Data.ActivityName = "";
        if (!angular.isDefined($scope.Data.GroupCompetence)) $scope.Data.GroupCompetence = "";
        if (!angular.isDefined($scope.Data.IndividualSummary)) $scope.Data.IndividualSummary = "";
        $scope.record.Content = "活动名称:" + $scope.Data.ActivityName + "\n活动流程团体动力:" + $scope.Data.GroupCompetence + "\n特殊个案个别纪要:" + $scope.Data.IndividualSummary + "\n成果评估(建议改进事项):" + newValue;
    })

}])
.controller("chooseResidentModalCtr", ['$scope', '$http', '$state', 'utility', 'floorRes', 'residentBriefRes', function ($scope, $http, $stateParams, utility, floorRes, residentBriefRes) {

    $scope.init = function () {
        $scope.FloorId = "";
        $scope.Name = "";
        $scope.residents = [];
        $scope.copyResidents = [];
        $scope.regSelected = [];
        floorRes.get({ floorName: "" }, function (data) {
            $scope.floors = data.Data;
        });
    }
    $scope.init();
    $scope.Search = function () {
        residentBriefRes.get({ regName: $scope.Name, floorId: $scope.FloorId, residentNo: "" }, function (data) {
            $scope.residents = data.Data;
            $scope.copyResidents = angular.copy($scope.residents);
        });
    }

    $scope.Close = function () {
        $scope.init();
        $('#modalChooseResident').modal('hide');
    }

    $scope.Save = function () {
        if ($scope.regSelected.length > 0) {
            $scope.$emit('transMember', $scope.regSelected);
            $scope.init();
        } else {
            utility.message("请选择住民!");
        }
    }

    $scope.addEmp = function (type) {
        if (type == "right") {//添加员工
            var temp = [];
            var selectedRegNo = 0;
            $("#sltFrom option:selected").each(function () {
                var exists = false;
                var feeNo = parseInt(this.value);
                for (var i = 0; i < $scope.regSelected.length; i++) {
                    if (this.value == $scope.regSelected[i].FeeNo) {
                        exists = true;
                        break;
                    }
                }
                angular.forEach($scope.copyResidents, function (item) {
                    if (feeNo == item.FeeNo) {
                        selectedRegNo = item.RegNo;
                    }
                })
                if (!exists) {
                    temp.push({ FeeNo: this.value, Name: this.text, RegNo: selectedRegNo });
                }
                for (var j = 0; j < $scope.residents.length; j++) {
                    if ($scope.residents[j].FeeNo == this.value) {
                        $scope.residents.splice(j, 1);
                    }
                }
            });
            $scope.regSelected = $scope.regSelected.concat(temp);
        } else {//减少员工
            var temp = [];
            var selectedRegNo = 0;
            $("#sltTo option:selected").each(function () {
                var exists = false;
                var feeNo = parseInt(this.value);
                for (var i = 0; i < $scope.residents.length; i++) {
                    if (this.value == $scope.residents[i].FeeNo) {
                        exists = true; break;
                    }
                }

                angular.forEach($scope.copyResidents, function (item) {
                    if (feeNo == item.FeeNo) {
                        selectedRegNo = item.RegNo;
                    }
                })


                if (!exists) {
                    temp.push({ FeeNo: this.value, Name: this.text, RegNo: selectedRegNo });
                }
                for (var j = 0; j < $scope.regSelected.length; j++) {
                    if ($scope.regSelected[j].FeeNo == this.value) {
                        $scope.regSelected.splice(j, 1);
                    }
                }
            });
            $scope.residents = $scope.residents.concat(temp);
        }

    }

}])
.controller("memberAssessCtrl", ['$scope', '$stateParams', 'utility', 'memberAssessRes', function ($scope, $stateParams, utility, memberAssessRes) {
    //$stateParams.id
    $scope.Data = {};
    $scope.currentItem = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;
    if ($stateParams.id) {
        //加载所选活动的成员个别化活动评估
        memberAssessRes.query({ GAID: $stateParams.id }, function (data) {
            $scope.Data.AssessList = data;
        });
    }

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        if (angular.isDefined($scope.currentResident.id)) {
            $scope.buttonShow = true;
        }
    }

    //删除活动评估记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的个别化活动评估记录吗?")) {
            item.$delete().then(function () {
                $scope.Data.AssessList.splice($scope.Data.AssessList.indexOf(item), 1);
            });
            utility.message(item.ResidentName + "的活动评估记录已删除！");
        }
    };


    $scope.createItem = function (item) {
        //新增零用金收支记录，得到住民ID
        $scope.currentItem.ResidentId = $scope.currentResident.id;
        $scope.currentItem.ResidentName = $scope.currentResident.Name;
        $scope.currentItem.GAID = $stateParams.id;
        memberAssessRes.save($scope.currentItem, function (data) {
            $scope.Data.AssessList.push(data);

        });
    };

    $scope.updateItem = function (item) {
        item.$save();
    };


    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
        if (angular.isDefined(item.id)) {
            $scope.buttonShow = true;
        }
    };

    $scope.saveEdit = function (item) {
        var showName = "";
        if (angular.isDefined(item.id)) {
            $scope.updateItem(item);
            showName = item.ResidentName;
        } else {
            $scope.createItem(item);
            showName = $scope.currentResident.Name;
        }
        utility.message(showName + "的活动评估记录保存成功！");
        $scope.currentItem = {};
        $scope.buttonShow = false;
    };




}])
.controller("activityPhotoCtrl", ['$scope', '$http', '$location', '$stateParams', 'webUploader', 'utility', 'groupActivityRes', function ($scope, $http, $location, $stateParams, webUploader, utility, groupActivityRes) {
    $scope.Detail = {};
    $scope.save = function () {
        groupActivityRes.save($scope.Detail, function (data) {
            $location.url('/angular/GroupActivityList');
            utility.message($scope.Data.Detail.GAName + "的团体活动图片信息保存成功！");
        });
    }

    $scope.init = function () {
        if ($stateParams.id) {
            groupActivityRes.get({ id: $stateParams.id }, function (data) {
                $scope.Detail = data;
                if (!data.GAPicture || data.GAPicture.length == 0) {
                    $scope.Detail.GAPicture = [];
                    for (var i = 0; i < 3; i++) {
                        $scope.Detail.GAPicture.push({
                            index: i + 1,
                            PhotoUrl: "",
                            PhotoName: ""
                        });
                    }
                }
            });
        }

        //seajs.use(['/Scripts/webuploader', '/Content/webuploader.css'], function () {
        webUploader.init('#PhotoPicker1', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.Detail.GAPicture[0].PhotoUrl = data[0].SavedLocation;
                $scope.Detail.GAPicture[0].PhotoName = data[0].FileName;
                $scope.$apply();
            }
        });

        webUploader.init('#PhotoPicker2', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.Detail.GAPicture[1].PhotoUrl = data[0].SavedLocation;
                $scope.Detail.GAPicture[1].PhotoName = data[0].FileName;
                $scope.$apply();
            }
        });

        webUploader.init('#FilePicker', { category: 'HomePhoto' }, '文件', 'doc,docx', 'doc/*', function (data) {
            if (data.length > 0) {
                $scope.Detail.GAPicture[2].PhotoUrl = data[0].SavedLocation;
                $scope.Detail.GAPicture[2].PhotoName = data[0].FileName;
                $scope.$apply();
            }
        });
    }

    $scope.clear = function (type) {

        switch (type) {
            case "PhotoPicker1":
                $scope.Detail.GAPicture[0].PhotoUrl = "";
                $scope.Detail.GAPicture[0].PhotoName = "";
                break;
            case "PhotoPicker2":
                $scope.Detail.GAPicture[1].PhotoUrl = "";
                $scope.Detail.GAPicture[1].PhotoName = "";
                break;
            case "FilePicker":
                $scope.Detail.GAPicture[2].PhotoUrl = "";
                $scope.Detail.GAPicture[2].PhotoName = "";
                break;
        }
    }

    $scope.init();

}])
