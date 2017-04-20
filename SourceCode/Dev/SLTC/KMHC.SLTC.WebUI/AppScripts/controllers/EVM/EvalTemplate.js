/*
创建人:张祥
创建日期:2016-09-08
修改人:
修改日期:
说明: 评估模板管理
*/
angular.module("sltcApp")
.controller("EvalTemplateCtrl", ['$scope', 'evalTemplateRes', 'utility', '$rootScope', function ($scope, evalTemplateRes, utility, $rootScope) {
    $scope.Data = {};
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置 
            ajaxObject: evalTemplateRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.evalTemplateList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                keyWord: "",
                orgId: "tpl",
            }
        }

        //如果是超级管理员显示机构 其它角色不显示
        if ($rootScope.Global.MaximumPrivileges == "SuperAdmin") {
            $scope.showAction = true;
            $scope.showAdd = true;
        }
        else if ($rootScope.Global.MaximumPrivileges == "Admin") {
            $scope.showAction = false;
            $scope.showAdd = true;
        }
        else {
            $scope.showAction = false;
            $scope.showAdd = false;
        }
    };
    $scope.init();

    //查询
    $scope.search = function (keyValue) {
        if (angular.isDefined(keyValue)) {
            $scope.options.params.keyWord = keyValue;
        }
        $scope.options.params.orgId = "tpl";
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.search();
    }

    //删除评估量表
    $scope.deleteEvalTemplate = function (item) {
        if (confirm("您确定要删除该评估问题吗?")) {
            evalTemplateRes.delete({ id: item.Id }, function (data) {
                if (data.ResultCode == 1) {
                    $scope.Data.evalTemplateList.splice($scope.Data.evalTemplateList.indexOf(item), 1);
                    utility.message("删除成功！");
                }
                else {
                    utility.message("删除异常！");
                }
            });
        }
    };

}]);

angular.module("sltcApp")
.controller("EditEvalTemplateCtrl", ['$scope', 'evalTemplateRes', 'exportEvalRes', 'evalQuestionResultRes', 'evalAnswerRes', 'evalQuestionRes', 'exportQueResRes', 'DCDataDicListRes', '$stateParams', 'utility', '$rootScope', function ($scope, evalTemplateRes, exportEvalRes, evalQuestionResultRes, evalAnswerRes, evalQuestionRes, exportQueResRes, DCDataDicListRes, $stateParams, utility, $rootScope) {
    $scope.Data = {};
    $scope.currentItem = {};
    $scope.MakerItem = {};
    $scope.TempMakerItem = {};
    $scope.Limited = {};
    $scope.QuestionResults = {};
    $scope.showEdit = false;
    $scope.init = function () {
        if ($stateParams.id) {
            evalTemplateRes.get({ id: $stateParams.id }, function (data) {
                $scope.editEvalTemplate(data.Data);
            });
            $scope.showExport = false;
        }
        else {
            evalTemplateRes.get({ keyWord: "", orgId: "tpl", currentPage: 1, pageSize: 0 }, function (data) {
                $scope.Data.evalModelList = data.Data;
            });
            $scope.currentItem.OrgId = "tpl";
            $scope.Data.MakerItemList = [];
            $scope.Data.QuestionResultsList = [];
            $scope.showExport = true;
        }


    };
    $scope.init();

    //导入模板
    $scope.choseModle = function () {

        $scope.questionId = "";
        $("#choseModel").modal("toggle");

    };
    $scope.choseModle2 = function () {
        $scope.questionId = "";
        $("#choseModel2").modal("toggle");
    };

    //保存导入数据
    $scope.saveModel = function (val) {
        switch (val) {
            case "Que":
                if (!$scope.questionId == "") {
                    exportEvalRes.save({ QuestionId: $scope.currentItem.QuestionId, OrgId: "", ExportQuestionId: $scope.questionId }, function (data) {
                        evalTemplateRes.get({ questionId: $scope.currentItem.QuestionId }, function (data) {
                            $scope.Data.MakerItemList = data.Data;
                        });
                        $("#choseModel").modal("toggle");
                        utility.message("评估问题数据导入成功！");
                    });
                }
                break;
            case 'Res':
                if (!$scope.questionId == "") {
                    exportQueResRes.save({ QuestionId: $scope.currentItem.QuestionId, OrgId: "", ExportQuestionId: $scope.questionId }, function (data) {
                        evalTemplateRes.get({ questionId: $scope.currentItem.QuestionId, qrMark: "" }, function (data) {
                            $scope.Data.QuestionResultsList = data.Data;
                        });
                        $("#choseModel2").modal("toggle");
                        utility.message("评估结果数据导入成功！");
                    });
                }
                break;

        }




    };
    //关闭导入模板选择界面
    $scope.closeModel = function () {
        $("#choseModel").modal("toggle");
    };
    $scope.closeModel2 = function () {
        $("#choseModel2").modal("toggle");
    };
    //新增评估量表
    $scope.addEvalTemplate = function () {
        $scope.showEdit = false;
        $scope.showExport = true;
        $scope.currentItem = {};
        $scope.Data.MakerItemList = [];
        $scope.Data.QuestionResultsList = [];
        //$scope.currentItem.OrgId = $rootScope.Global.OrganizationId;
        $scope.currentItem.OrgId = "tpl";
        //$scope.editEvalTemp = true;
        $scope.showTab = false;
    };
    //新增评估题目
    $scope.addMakerItem = function () {
        $scope.MakerItem = {};
        $scope.MakerItem.IsShow = true;
        $("#EvalMakeItem").modal("toggle");
    };
    //新增答案
    $scope.addLimited = function (item) {
        $scope.Limited = {};
        $scope.TempMakerItem = item;
        $scope.Limited.LimitedId = item.LimitedId;
        $("#Limited").modal("toggle");
    };
    //新增评估结果
    $scope.addQuestionResults = function () {
        $scope.QuestionResults = {};
        $("#QuestionResults").modal("toggle");
    };

    //编辑评估量表
    $scope.editEvalTemplate = function (item) {
        $scope.showEdit = false;
        $scope.showExport = false;
        $scope.showTab = true;
        //$scope.editEvalTemp = true;
        $scope.currentItem = item;
        evalTemplateRes.get({ questionId: item.QuestionId }, function (data) {
            $scope.Data.MakerItemList = data.Data;
        });

        evalTemplateRes.get({ questionId: item.QuestionId, qrMark: "" }, function (data) {
            $scope.Data.QuestionResultsList = data.Data;
        });
    };
    //编辑评估题目
    $scope.editMakerItem = function (item) {
        $scope.MakerItem = item;
        $("#EvalMakeItem").modal("toggle");
    };
    //编辑答案
    $scope.editLimited = function (item) {
        $scope.Limited = item;
        $("#Limited").modal("toggle");
    };
    //编辑评估结果
    $scope.editQuestionResults = function (item) {
        $scope.QuestionResults = item;
        $("#QuestionResults").modal("toggle");
    };

    //保存评估量表
    $scope.saveEvalTemplate = function (item) {
        if (angular.isDefined($scope.evlaTemplateForm.$error.required)) {
            for (var i = 0; i < $scope.evlaTemplateForm.$error.required.length; i++) {
                utility.msgwarning($scope.evlaTemplateForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evlaTemplateForm.$error.maxlength)) {
            for (var i = 0; i < $scope.evlaTemplateForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.evlaTemplateForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evlaTemplateForm.$error.pattern)) {
            for (var i = 0; i < $scope.evlaTemplateForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.evlaTemplateForm.$error.pattern[i].$name + "格式错误！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        if (angular.isDefined(item.Id)) {

            evalTemplateRes.save(item, function (data) {
                utility.message("评估信息更新成功！");
            });
        } else {
            evalTemplateRes.save(item, function (data) {
                $scope.currentItem = data.Data;
                //$scope.Data.evalTemplateList.push(data.Data);
                $scope.showTab = true;
                utility.message("评估信息保存成功！");
            });
        }
    };
    //取消编辑评估量表返回首页
    $scope.cancelEvalTemplate = function () {
        //$scope.editEvalTemp = false;
        //$scope.options.search();
        window.location.href = "angular/EvalTemplateManage";
    };

    //保存评估问题
    $scope.saveEvalQuestion = function (item) {
        if (angular.isDefined($scope.evalMakeItemForm.$error.required)) {
            for (var i = 0; i < $scope.evalMakeItemForm.$error.required.length; i++) {
                utility.msgwarning($scope.evalMakeItemForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evalMakeItemForm.$error.maxlength)) {
            for (var i = 0; i < $scope.evalMakeItemForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.evalMakeItemForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evalMakeItemForm.$error.pattern)) {
            for (var i = 0; i < $scope.evalMakeItemForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.evalMakeItemForm.$error.pattern[i].$name + "格式错误！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        item.QuestionId = $scope.currentItem.QuestionId;
        if (angular.isDefined(item.MakerId)) {
            evalQuestionRes.save(item, function (data) {
                $("#EvalMakeItem").modal("toggle");
                utility.message("评估问题更新成功！");
            });
        } else {
            evalQuestionRes.save(item, function (data) {
                $scope.Data.MakerItemList.push(data.Data);
                $("#EvalMakeItem").modal("toggle");
                utility.message("评估问题保存成功！");
            });
        }
    };
    //取消编辑评估问题
    $scope.cancelEvalQuestion = function () {
        $("#EvalMakeItem").modal("toggle");
    };

    //保存评估问题答案
    $scope.saveLimited = function (item) {
        if (angular.isDefined($scope.evalAnsForm.$error.required)) {
            for (var i = 0; i < $scope.evalAnsForm.$error.required.length; i++) {
                utility.msgwarning($scope.evalAnsForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evalAnsForm.$error.maxlength)) {
            for (var i = 0; i < $scope.evalAnsForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.evalAnsForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evalAnsForm.$error.pattern)) {
            for (var i = 0; i < $scope.evalAnsForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.evalAnsForm.$error.pattern[i].$name + "格式错误！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        if (angular.isDefined(item.LimitedValueId)) {
            evalAnswerRes.save(item, function (data) {
                $("#Limited").modal("toggle");
                utility.message("评估问题更新成功！");
            });
        } else {
            evalAnswerRes.save(item, function (data) {
                if ($scope.TempMakerItem.MakerItemLimitedValue == null) {
                    $scope.TempMakerItem.MakerItemLimitedValue = []
                }
                $scope.TempMakerItem.MakerItemLimitedValue.push(data.Data);
                $("#Limited").modal("toggle");
                utility.message("评估问题保存成功！");
            });
        }
    };
    //取消编辑评估问题答案
    $scope.cancelLimited = function () {
        $("#Limited").modal("toggle");
    };

    //保存评估结果
    $scope.saveQuestionResults = function (item) {
        if (angular.isDefined($scope.evalQueResForm.$error.required)) {
            for (var i = 0; i < $scope.evalQueResForm.$error.required.length; i++) {
                utility.msgwarning($scope.evalQueResForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evalQueResForm.$error.maxlength)) {
            for (var i = 0; i < $scope.evalQueResForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.evalQueResForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.evalQueResForm.$error.pattern)) {
            for (var i = 0; i < $scope.evalQueResForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.evalQueResForm.$error.pattern[i].$name + "格式错误！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        item.QuestionId = $scope.currentItem.QuestionId;
        if (angular.isDefined(item.ResultId)) {
            evalQuestionResultRes.save(item, function (data) {
                $("#QuestionResults").modal("toggle");
                utility.message("评估结果更新成功！");
            });
        } else {
            evalQuestionResultRes.save(item, function (data) {
                $scope.Data.QuestionResultsList.push(data.Data);
                $("#QuestionResults").modal("toggle");
                utility.message("评估结果保存成功！");
            });
        }
    };
    //取消编辑评估结果
    $scope.cancelQuestionResults = function () {
        $("#QuestionResults").modal("toggle");
    };

    //删除评估量表
    $scope.deleteEvalTemplate = function (item) {
        if (confirm("您确定要删除该评估问题吗?")) {
            evalTemplateRes.delete({ id: item.Id }, function (data) {
                if (data.ResultCode == 1) {
                    $scope.Data.evalTemplateList.splice($scope.Data.evalTemplateList.indexOf(item), 1);
                    utility.message("删除成功！");
                }
                else {
                    utility.message("删除异常！");
                }
            });
        }
    };
    //删除评估问题
    $scope.deleteMakerItem = function (item) {
        if (confirm("您确定要删除该评估问题吗?")) {
            evalQuestionRes.delete({ id: item.MakerId }, function (data) {
                if (data.ResultCode == 1) {
                    $scope.Data.MakerItemList.splice($scope.Data.MakerItemList.indexOf(item), 1);
                    utility.message("删除成功！");
                }
                else {
                    utility.message("删除异常！");
                }
            });
        }
    };
    //删除问题答案
    $scope.deleteLimited = function (Item, item) {
        if (confirm("您确定要删除该答案吗?")) {
            evalAnswerRes.delete({ id: item.LimitedValueId }, function () {
                Item.MakerItemLimitedValue.splice(Item.MakerItemLimitedValue.indexOf(item), 1);
                utility.message("删除成功！");
            });
        }
    };
    //删除评估结果
    $scope.deleteQuestionResults = function (item) {
        if (confirm("您确定要删除该评估结果吗?")) {
            evalQuestionResultRes.delete({ id: item.ResultId }, function () {
                $scope.Data.QuestionResultsList.splice($scope.Data.QuestionResultsList.indexOf(item), 1);
                utility.message("删除成功！");
            });
        }
    };

}]);



