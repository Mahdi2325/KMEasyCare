/*
创建人:刘美方
创建日期:2016-06-28
说明: 工作提醒，公告
*/
angular.module("sltcApp")
    .controller("assignTaskCtrl", [
        '$scope', '$state', '$location', 'utility', 'assignTaskRes', 'reAssignTaskRes', 'empFileRes', 'noticeRes', '$filter',
        function ($scope, $state, $location, utility, assignTaskRes, reAssignTaskRes, empFileRes, noticeRes, $filter) {
            $scope.Data = [];
            $scope.curItemId = 0;
            $scope.empFiles = [];
            $scope.Notices = [];
            $scope.buttonShow = false;
            $scope.taskStatusValue = "";
            $scope.filter = { recStatus: null, newRecFlag: null, sDate: null, edDate: null };
            //R0 :未读 R1：已读 F0：未完成 F1：已完成
            var taskStatusselect = $("#taskStatus").select2({
                placeholder: "选择状态",
                allowClear: true,
            });

            $scope.init = function () {
                var taskStatusValue = $('#taskStatus').val().toString();
                $scope.options = {
                    buttons: [], //需要打印按钮时设置
                    ajaxObject: assignTaskRes, //异步请求的res
                    params: { sDate: "", eDate: "", recStatus: '', newRecFlag: '', taskStatus: taskStatusValue },
                    success: function (data) { //请求成功时执行函数
                        $scope.Data = data.Data;
                    },
                    pageInfo: {
                        CurrentPage: 1,
                        PageSize: 10
                    }
                }
                $scope.staffOptions = {
                    buttons: [], //需要打印按钮时设置
                    ajaxObject: empFileRes, //异步请求的res
                    params: { name: "", empGroup: "" },
                    success: function (data) { //请求成功时执行函数
                        $scope.empFiles = data.Data;
                    },
                    pageInfo: {
                        CurrentPage: 1,
                        PageSize: 10
                    }
                }
                $scope.noticeOptions = {
                    buttons: [], //需要打印按钮时设置
                    ajaxObject: noticeRes, //异步请求的res
                    params: { sDate: "", eDate: "", recStatus: "", newRecFlag: "" },
                    success: function (data) { //请求成功时执行函数
                        $scope.Notices = data.Data;
                    },
                    pageInfo: {
                        CurrentPage: 1,
                        PageSize: 10
                    }
                }
            };
            $scope.init();
            $scope.sDate = "";
            $scope.eDate = "";
            $scope.checksDate = function () {
                if (angular.isDefined($scope.options.params.sDate) && angular.isDefined($scope.options.params.eDate)) {
                    if (!checkDate($scope.options.params.sDate, $scope.options.params.eDate)) {
                        utility.msgwarning("开始日期必须在结束日期之前");
                        $scope.options.params.sDate = $scope.sDate;
                    }
                    else {
                        $scope.sDate = $scope.options.params.sDate;
                    }
                }
            }

            $scope.checkeDate = function () {
                if ($scope.options.params.sDate != "" && $scope.options.params.eDate != "") {
                    if (angular.isDefined($scope.options.params.sDate) && angular.isDefined($scope.options.params.eDate)) {
                        if (!checkDate($scope.options.params.sDate, $scope.options.params.eDate)) {
                            utility.msgwarning("结束日期必须在开始日期之后");
                            $scope.options.params.eDate = $scope.eDate;
                        }
                        else {
                            $scope.eDate = $scope.options.params.eDate;
                        }
                    }
                }
                else {
                    $scope.eDate = $scope.options.params.eDate;
                }
            }

            //查询所有
            $scope.options.searchAll = function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.params.sDate = "";
                $scope.options.params.eDate = "";
                $scope.options.params.recStatus = "";
                $scope.options.params.newRecFlag = "";
                $scope.options.params.taskStatus = "";
                taskStatusselect.val(null).trigger("change");
                $scope.options.search();
            };

            //工作状态：未完成
            $scope.options.unSearch = function () {
                $scope.options.params.sDate = "";
                $scope.options.params.eDate = "";
                $scope.options.params.newRecFlag = "";
                $scope.options.params.recStatus = false;
                $scope.options.search();
            }
            //工作状态：已完成
            $scope.options.edSearch = function () {
                $scope.options.params.sDate = "";
                $scope.options.params.eDate = "";
                $scope.options.params.newRecFlag = "";
                $scope.options.params.recStatus = true;
                $scope.options.search();
            }

            $scope.options.searchDate = function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.params.taskStatus = $('#taskStatus').val() == null ? "" : $('#taskStatus').val().toString();
                $scope.options.search();
            }


            //工作提醒：未读
            $scope.options.newMsgSearch = function () {
                $scope.options.params.sDate = "";
                $scope.options.params.eDate = "";
                $scope.options.params.recStatus = "";
                $scope.options.params.newRecFlag = true;
                $scope.options.search();
            }
            //工作提醒：已读
            $scope.options.edMsgSearch = function () {
                $scope.options.params.sDate = "";
                $scope.options.params.eDate = "";
                $scope.options.params.recStatus = "";
                $scope.options.params.newRecFlag = false;
                $scope.options.search();
            }

            //完成状态 --已完成/未完成
            $scope.checkItem = function (Item, idx) {
                if (Item.RecStatus) {
                    Item.RecStatus = false;
                    Item.FinishDate = "";
                } else {
                    Item.RecStatus = true;
                    Item.FinishDate = $filter("date")(new Date(), "yyyy-MM-dd");
                    Item.NewrecFlag = false;
                }
                if (idx == 1) {
                    assignTaskRes.get({ Id: Item.Id, recStatus: Item.RecStatus, finishDate: Item.FinishDate, newrecFlag: Item.NewrecFlag }, function (data) {
                    });
                }
            }

            //消息状态--未读/已读
            $scope.checkNewItem = function (Item, idx) {
                if (Item.NewrecFlag) {
                    Item.NewrecFlag = false;
                } else {
                    Item.NewrecFlag = true;
                }
                if (idx == 1) {
                    assignTaskRes.get({ Id: Item.Id, newRecFlag: Item.NewrecFlag }, function (data) {
                    });
                }
            }

            //编辑
            $scope.changeEdit = function (item) {
                $scope.Item = item ? item : {};
                $("#AssignTaskModal").modal("toggle");
                $scope.buttonShow = true;
                $scope.displayMode = "edit";
            };

            //保存
            $scope.save = function (item) {
                assignTaskRes.save(item, function (data) {
                    utility.message("保存成功");
                    $("#AssignTaskModal").modal("toggle");
                    $scope.displayMode = "edit";
                    $scope.options.search();
                });
            };
            //取消
            $scope.cancel = function () {
                $scope.options.search();
                $scope.curItemId = 0;
                $("#AssignTaskModal").modal("toggle");
                $scope.displayMode = "list";
            };

            $scope.autoChangeNewRecFlag = function (Item) {
                Item.NewrecFlag = false;
                $scope.checkAssPerDate();
            };
            $scope.checkAssPerDate = function () {
                if (angular.isDefined($scope.Item.AssignDate) && angular.isDefined($scope.Item.PerformDate)) {
                    if (!checkDate($scope.Item.AssignDate, $scope.Item.PerformDate)) {
                        utility.msgwarning("交付日期应在应执行日期之前");
                        $scope.Item.PerformDate = "";
                    }
                }
            };

            $scope.reAssign = function () {
                if ($scope.chooseEmps.length === 0) {
                    utility.message("请选择照会人员！");
                    return;
                }
                reAssignTaskRes.save({ OldTask: $scope.curItem, TaskEmpFiles: $scope.chooseEmps }, function (res) {
                    utility.message("重新分配成功");
                    $('#modalDetail').modal('toggle');
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                });
            };


            $scope.deleteItem = function (item) {
                if (confirm("您确定要删除该条工作提醒吗?")) {
                    reAssignTaskRes.delete({ id: item.Id }, function (data) {
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                        utility.message("删除成功");
                    });
                }
            };

            $scope.openReAssign = function (item) {
                $scope.curItem = item;
                $scope.chooseEmps = [];
                $scope.x = false;
                $("#modalDetail").modal("toggle");
                $scope.staffOptions.pageInfo.CurrentPage = 1;
                $scope.staffOptions.search();
            };
            $scope.chooseEmps = [];
            $scope.x = false; //默认未选中
            $scope.all = function (c) { //全选
                $scope.chooseEmps = [];
                if (c) {
                    $scope.x = true;
                    angular.forEach($scope.empFiles, function (data) {
                        $scope.chooseEmps.push({ EmpNo: data.EmpNo, EmpName: data.EmpName });
                    });
                } else {
                    $scope.x = false;
                }
            };
            $scope.chk = function (item, x) { //单选或者多选
                if (x) {
                    $scope.chooseEmps.push({ EmpNo: item.EmpNo, EmpName: item.EmpName });
                } else {
                    angular.forEach($scope.chooseEmps, function (data) {
                        if (data.EmpNo === item.EmpNo) {
                            $scope.chooseEmps.splice($scope.chooseEmps.indexOf(data), 1);
                        }
                    });
                }
            };
            //$scope.openNotice = function(item) {
            //};
        }
    ])
.controller("noticeDetailCtrl", ['$scope', '$stateParams', 'noticeRes', function ($scope, $stateParams, noticeRes) {
    $scope.data = {};
    $scope.init = function () {
        noticeRes.get({ id: $stateParams.id }, function (data) {
            $scope.data = data.Data;
        });
    };
    $scope.init();
}]).controller("noticeCtrl", ['$scope', '$state', '$location', 'utility', 'noticeRes', function ($scope, $state, $location, utility, noticeRes) {
    $scope.Notices = [];
    $scope.init = function () {
        $scope.options = {
            buttons: [],
            ajaxObject: noticeRes,
            params: { sDate: "", eDate: "", recStatus: "", newRecFlag: "" },
            success: function (data) {
                $scope.Notices = data.Data;
            },
            pageInfo: {
                CurrentPage: 1,
                PageSize: 10
            }
        }
    };
    $scope.sDate = "";
    $scope.eDate = "";
    $scope.checksDate = function () {
        if (angular.isDefined($scope.options.params.sDate) && angular.isDefined($scope.options.params.eDate)) {
            if (!checkDate($scope.options.params.sDate, $scope.options.params.eDate)) {
                utility.msgwarning("开始日期必须在结束日期之前");
                $scope.options.params.sDate = $scope.sDate;
            }
            else {
                $scope.sDate = $scope.options.params.sDate;
            }
        }
    }

    $scope.checkeDate = function () {
        if (angular.isDefined($scope.options.params.sDate) && angular.isDefined($scope.options.params.eDate)) {
            if (!checkDate($scope.options.params.sDate, $scope.options.params.eDate)) {
                utility.msgwarning("结束日期必须在开始日期之后");
                $scope.options.params.eDate = $scope.eDate;
            }
            else {
                $scope.eDate = $scope.options.params.eDate;
            }
        }
    }
    $scope.delete = function (id) {
        if (confirm("确定删除该信息吗?")) {
            noticeRes.delete({ id: id }, function (data) {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                utility.message("删除成功");
            });
        }
    };
    $scope.init();
}
]).controller("noticeEditCtrl", ['$scope', '$http', '$state', '$stateParams', 'noticeRes', function ($scope, $http, $state, $stateParams, noticeRes) {
    $scope.init = function () {
        $scope.Data = {};
        if ($stateParams.id) {
            noticeRes.get({ id: $stateParams.id }, function (data) {
                $scope.Data = data.Data;
                $scope.info = { content: $scope.Data.Contents };
            });
            $scope.isAdd = false;
        } else {
            $scope.isAdd = true;
        }

        $scope.config = {
            width: '100%',
            cssPath: '../Content/kindeditor/kindeditor-4.1.5/plugins/code/prettify.css',
            uploadJson: '../Content/kindeditor/kindeditor-4.1.5/asp.net/upload_json.ashx',
            fileManagerJson: '../Content/kindeditor/kindeditor-4.1.5/asp.net/file_manager_json.ashx',
            allowFileManager: true
        };
    };
    $scope.submit = function () {
        $scope.Data.Contents = $scope.info.content;
        noticeRes.save($scope.Data, function (data) {
            $state.go('Notice');
        });
    };
    $scope.init();
}]);;


