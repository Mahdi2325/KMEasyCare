angular.module("sltcApp")
.controller('TsgQuestionCtrl', ['$scope', 'categoryRes', function ($scope, categoryRes) {
    $scope.questionList = {};
    $scope.tsgCategoryList = {};
    $scope.TsgQuestionData = {};

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: categoryRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.questionList = data.Data;
                if ($scope.questionList != null) {
                    categoryRes.get({ type: "" }, function (data) {
                        $scope.tsgCategoryList = data.Data;
                        if ($scope.tsgCategoryList != null) {
                            angular.forEach($scope.questionList, function (question) {
                                angular.forEach($scope.tsgCategoryList, function (item) {
                                    if (question.CategoryCode == item.Code) {
                                        question.CategoryName = item.Name;
                                    }
                                })
                            })
                        }
                    });
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                keyword: ""
            }
        }
    }
    $scope.init();

    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除问题吗?")) {
            categoryRes.delete({ id: item.Id }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
            });
        }
    };
}]);
angular.module("sltcApp")
.controller('EditTsgQuestionCtrl', ['$scope', '$interval', '$stateParams', 'categoryRes', 'utility', function ($scope, $interval, $stateParams, categoryRes, utility) {

   
    $scope.currentItem = {};
    $scope.tsgCategoryList = {};
    $scope.init = function () {
        categoryRes.get({ type: "" }, function (data) {
            $scope.tsgCategoryList = data.Data;
        });

        if ($stateParams.id) {
            categoryRes.get({ id: $stateParams.id }, function (data) {
                if (data.Data != null) {
                    $scope.currentItem.Question = data.Data.TsgQuestion;
                    $scope.currentItem.Answer = data.Data.TsgAnswer;
                    $scope.info = { content: $scope.currentItem.Answer.Description };
                }
            });
        }
        else {
            $scope.currentItem.Question = {};
            $scope.currentItem.Answer = {};
            $scope.currentItem.Question.Status = true;
        }

        $scope.config = {
            width: '100%',
            cssPath: '../Content/kindeditor/kindeditor-4.1.5/plugins/code/prettify.css',
            uploadJson: '../Content/kindeditor/kindeditor-4.1.5/asp.net/upload_json.ashx',
            fileManagerJson: '../Content/kindeditor/kindeditor-4.1.5/asp.net/file_manager_json.ashx',
            allowFileManager: true
        };




    }
    $scope.init();

    $scope.saveTsgQuestion = function (item) {


        if (angular.isDefined($scope.cartegoryForm.$error.required)) {
            for (var i = 0; i < $scope.cartegoryForm.$error.required.length; i++) {
                utility.msgwarning($scope.cartegoryForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.cartegoryForm.$error.maxlength)) {
            for (var i = 0; i < $scope.cartegoryForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.cartegoryForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.cartegoryForm.$error.pattern)) {
            for (var i = 0; i < $scope.cartegoryForm.$error.pattern.length; i++) {
                utility.msgwarning($scope.cartegoryForm.$error.pattern[i].$name + "格式错误！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        $scope.TsgQuestionData = {};
        $scope.TsgQuestionData.TsgQuestion = item.Question;
        $scope.TsgQuestionData.TsgAnswer = item.Answer;
        $scope.TsgQuestionData.TsgAnswer.Description = $scope.info.content;
        categoryRes.save($scope.TsgQuestionData, function (data) {
            window.location.href = "angular/TsgQuestionList";
        })
    }
}]);
