angular.module("sltcApp")
.controller("subsidyListCtrl", ['$scope', '$state', '$filter', 'utility', 'dictionary', 'subsidyRecRes', 'empFileRes',
    function ($scope, $state, $filter, utility, dictionary, subsidyRecRes, empFileRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        //当前住民
        $scope.currentResident = {};

        $scope.buttonShow = false;
        //当前分页码
        $scope.currentPage = 1;

        $scope.init = function () {
            //加载评估人员字典列表
            $scope.Data.Creator = [{ value: "张三", text: "张三" }, { value: "李四", text: "李四" }, { value: "王五", text: "王五" }, { value: "赵六", text: "赵六" }];
            empFileRes.get({ empNo: '' },
                function (data) {
                    $scope.Data.EmpList = data.Data;
                });

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: subsidyRecRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.subsidys = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        }

        //获取住民生活记录
        $scope.getSubsidyByNo = function (feeNo) {
            $scope.buttonShow = true;
            $scope.Data.subsidys = {};

            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = feeNo;
            $scope.options.search();

            //subsidyRecRes.get({ feeNo: feeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {

            //    $scope.Data.subsidys = data.Data;
            //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

            //        $scope.currentPage = curPage;
            //        $scope.getSubsidyByNo(feeNo);
            //    });
            //});
        }

        $scope.currentItem = null;

        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { ApplyBy: $scope.curUser.EmpNo };
        }

        //选中住民
        $scope.residentSelected = function (resident) {

            $scope.currentResident = resident;

            $scope.getSubsidyByNo(resident.FeeNo)
            //$scope.getSubsidyByNo(resident.FeeNo);//加载当前住民的生活记录列表

            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { ApplyBy: $scope.curUser.EmpNo };
            }
            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.currentItem.RegNo = resident.RegNo;
            $scope.currentItem.FeeNo = resident.FeeNo;
            //$scope.curUser = utility.getUserInfo();
            //$scope.currentItem.ApplyBy = $scope.curUser.EmpNo;
            //$scope.currentItem.ApplyDate = new Date().format("yyyy-MM-dd");
        }

        //设置下次间隔日期
        $scope.setNextValDate = function (gap) {
            if (isNumber(gap)) {
                if (gap > 0) {
                    var currentDate = $scope.currentItem.ApplyDate;
                    currentDate = currentDate.substring(0, 10);
                    $scope.currentItem.NextApplyDate = GetNextEvalDate(currentDate, gap);
                } else if (gap < 0) {
                    $scope.currentItem.Gap = '';
                    utility.message("间隔天数不能为负数");
                }
            }
        }

        //改变申请时间
        $scope.ChangeApplyDate = function () {
            if (isNumber($scope.currentItem.Gap)) {
                if ($scope.currentItem.Gap > 0) {
                    var currentDate = $scope.currentItem.ApplyDate;
                    currentDate = currentDate.substring(0, 10);
                    $scope.currentItem.NextApplyDate = GetNextEvalDate(currentDate, $scope.currentItem.Gap);
                } else if ($scope.currentItem.Gap < 0) {
                    $scope.currentItem.Gap = '';
                    utility.message("间隔天数不能为负数");
                }
            }

            $scope.setNextValDate($scope.currentItem.Gap);
        }

        //改变下次申请日期
        $scope.ChangeNextApplyDate = function () {
            if ($scope.currentItem.ApplyDate != "" && $scope.currentItem.NextApplyDate != "") {
                var days = DateDiff($scope.currentItem.NextApplyDate, $scope.currentItem.ApplyDate);
                if (days < 0) {
                    utility.message("下次申请日期不能小于本次日期");
                    $scope.currentItem.NextApplyDate = "";
                    $scope.currentItem.Gap = '';
                } else {
                    $scope.currentItem.Gap = days;
                }
            };
        }


        $scope.saveSubsidy = function (item) {

            if ($scope.subsidyForm.$valid) {//判断验证通过后才可以保存
                subsidyRecRes.save(item, function (data) {

                    if (angular.isDefined(item.Id)) {
                        utility.message($scope.currentResident.Name + "的补助申请更新成功！");
                    }
                    else {
                        data.Data.CreateDate = new Date(data.Data.CreateDate).format("yyyy-MM-dd");

                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                        utility.message($scope.currentResident.Name + "的补助申请成功！");
                    }
                    $scope.curUser = utility.getUserInfo();
                    if (typeof ($scope.curUser) != 'undefined') {
                        $scope.currentItem = { ApplyBy: $scope.curUser.EmpNo };
                    }
                    $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
                });
            }
            else {
                //验证没有通过
                $scope.getErrorMessage($scope.subsidyForm.$error);
                $scope.errs = $scope.errArr.reverse();
                for (var n = 0; n < $scope.errs.length; n++) {
                    if (n < 3) {
                        utility.msgwarning($scope.errs[n]);
                    }
                    //if (n > 2) break;
                }
            }
        };

        $scope.rowSelect = function (item) {
            var Gap = DateDiff(new Date(item.NextApplyDate).format("yyyy-MM-dd"), new Date(item.ApplyDate).format("yyyy-MM-dd"));
            $scope.currentItem = item;
            $scope.currentItem.Gap = Gap;
        }

        $scope.resetSubsidy = function (item) {
            $scope.currentItem = {};
            $scope.currentItem.RegNo = $scope.currentResident.RegNo;
            $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        }
        $scope.subsidyDelete = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                subsidyRecRes.delete({ id: item.Id }, function (data) {
                    if (data.ResultCode == 0)
                        utility.message("资料删除成功！");
                    //$scope.Data.subsidys.splice(whatIndex, 1);
                    // $scope.getSubsidyByNo($scope.currentResident.FeeNo);
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                });
            }
        }
        $scope.editOrCreate = function (item) {
            $scope.currentItem = item ? item : {};
            $("#modalSubsidy").modal("toggle");
        };

        $scope.cancelEdit = function () {
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { ApplyBy: $scope.curUser.EmpNo };
            }
            $("#modalSubsidy").modal("toggle");
        };

        //选择填写人员
        //$scope.staffSelected1= function (item) {
        //    $scope.currentItem.ApplyBy = item.EmpNo;
        //    $scope.currentItem.ApplyByName = item.EmpName;

        //}
        $scope.staffSelected2 = function (item) {
            $scope.currentItem.NextApplyBy = item.EmpNo;
            $scope.currentItem.NextApplyByName = item.EmpName;
        }
        //选择填写人员
        $scope.staffSelected1 = function (item) {
            $scope.currentItem.ApplyBy = item.EmpNo;
            $scope.currentItem.ApplyByName = item.EmpName;
        }
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

