angular.module("sltcApp")
.controller("complainRecCtrl", ['$scope', '$http', '$state', 'utility', 'dictionary', 'complainrecRes', 'empFileRes',
    function ($scope, $http, $state, utility, dictionary, complainrecRes, empFileRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        //当前住民
        $scope.currentResident = {};
        //当前分页码
        $scope.currentPage = 1;
        $scope.buttonShow = false;

        $scope.init = function () {
            empFileRes.get({ empNo: '' }, function (data) {
                $scope.Data.EmpList = data.Data;
            });

            $scope.Data.QuestionResult = [{ value: "已解决", text: "已解决" }, { value: "未解决", text: "未解决" }, { value: "处理中", text: "处理中" }];

            $scope.Data.QuestionType = [{ value: "行政法令查询", text: "行政法令查询" }, { value: "其他", text: "其他" }, { value: "院民权益维护", text: "院民权益维护" }, { value: "院务遗举发", text: "院务遗举发" }, { value: "院务与革建议", text: "院务与革建议" }];

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: complainrecRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.complainrecList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        }
        //选中住民
        $scope.residentSelected = function (resident) {

            $scope.currentResident = resident;

            $scope.currentItem = {};//清空编辑项

            $scope.getComplainRecByNo(resident.FeeNo);//加载当前住民的生活记录列表
            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.currentItem.RegNo = resident.RegNo;
            $scope.currentItem.FeeNo = resident.FeeNo;

            $scope.curUser = utility.getUserInfo();
            $scope.currentItem.ProcessBy = $scope.curUser.EmpNo;
            $scope.currentItem.RecDate = new Date().format("yyyy-MM-dd");
        }
        //获取住民申诉记录
        $scope.getComplainRecByNo = function (feeNo) {
            $scope.buttonShow = true;
            $scope.Data.complainrecList = {};

            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = feeNo;
            $scope.options.search();

            //complainrecRes.get({ feeNo: feeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {
                
            //    $scope.Data.complainrecList = data.Data;
            //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

            //        $scope.currentPage = curPage;
            //        $scope.getComplainRecByNo(feeNo);
            //    });
            //});
        }

        $scope.saveComplainRec = function (item) {
            $scope.currentItem.RegNo = $scope.currentResident.RegNo;
            $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
            if ($scope.clrForm.$valid) {//判断验证通过后才可以保存
                complainrecRes.save(item, function (data) {
                    if (angular.isDefined(item.Id)) {
                        utility.message("资料更新成功！");
                    }
                    else {
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();

                        utility.message("资料保存成功！");
                    }
                });
                $scope.currentItem = {};
                $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
                $scope.getComplainRecByNo($scope.currentItem.FeeNo);
            }
            else {
                //验证没有通过
                $scope.getErrorMessage($scope.clrForm.$error);
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
            $scope.currentItem = item;
        }

        $scope.deleteComplain = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                complainrecRes.delete({ id: item.Id }, function (data) {
                        if (data.ResultCode == 0)
                            utility.message("资料删除成功！");
                    //$scope.getComplainRecByNo($scope.currentItem.FeeNo);


                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                });
            }
        };
        $scope.resetComplain = function () {
            $scope.currentItem = {};
            $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        }
        //选择填写人员
        $scope.staffSelected_1 = function (item) {
            $scope.currentItem.ProcessBy = item.EmpNo;
            $scope.currentItem.ProcessName = item.EmpName;
        }
        $scope.staffSelected_2 = function (item) {
            $scope.currentItem.EmpNo = item.EmpNo;
            $scope.currentItem.SSName = item.EmpName;
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

