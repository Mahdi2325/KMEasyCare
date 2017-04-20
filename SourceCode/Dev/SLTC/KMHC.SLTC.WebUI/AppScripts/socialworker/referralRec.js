angular.module('sltcApp')
.controller("referralRecCtrl", ['$scope', '$state', 'utility', 'dictionary', 'referralRecRes', 'empFileRes',
    function ($scope, $state, utility, dictionary, referralRecRes, empFileRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        //当前住民
        $scope.currentResident = {};

        $scope.buttonShow = false;
        //当前分页码
        $scope.currentPage = 1;
        //当前操作对象
        $scope.currentItem = null;

        $scope.init = function () {
            empFileRes.get({ empNo: '' }, function (data) {
                $scope.Data.EmpList = data.Data;
            })
            $scope.Data.ReplyType = [
                    { value: '1', text: '邮件回复' },
                    { value: '2', text: '电话回复' }
            ];
            $scope.Data.Reason = [
                { value: '1', text: '转介原因一' },
                    { value: '2', text: '转介原因二' }
            ];

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: referralRecRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.referralRecs = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        };

        //选中住民
        $scope.residentSelected = function (resident) {

            $scope.currentResident = resident;

            $scope.getReferralByNo(resident.FeeNo);//加载当前住民的生活记录列表


            $scope.currentItem = {};//清空编辑项

            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.currentItem.RegNo = resident.RegNo;
            $scope.currentItem.FeeNo = resident.FeeNo;

            $scope.curUser = utility.getUserInfo();
            $scope.currentItem.EmpNo = $scope.curUser.EmpNo;
            $scope.currentItem.RecDate = new Date().format("yyyy-MM-dd");
        }
        //获取住民生活记录
        $scope.getReferralByNo = function (feeNo) {
            $scope.buttonShow = true;
            $scope.Data.referralRecs = {};

            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = feeNo;
            $scope.options.search();
            //referralRecRes.get({ feeNo: feeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {

            //    $scope.referralRecs = data.Data;
            //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

            //        $scope.currentPage = curPage;
            //        $scope.getReferralByNo(feeNo);
            //    });
            //});
        }
        $scope.checkTel = function (input) {

            if (isNaN(input) || input.length <= 0 || !(/^((0\d{2,3}-\d{7,8})|((\d|-){8,15})|(^$)|(1[3584]\d{9})|(([-_－—\s\(]?)([\(]?)((((0?)|((00)?))(((\s){0,2})|([-_－—\s]?)))|(([\)]?)[+]?))(886)?([\)]?)([-_－—\s]?)([\(]?)[0]?[1-9]{1}([-_－—\s\)]?)[1-9]{2}[-_－—]?[0-9]{3}[-_－—]?[0-9]{3}))$/.test(input))) {
                {
                    utility.message("请输入正确的电话号码/手机号如:+886912345678 或  0987799756");
                    //alert("请输入正确的电话号码/手机号!");
                    $scope.currentItem.UnitTel = "";
                    return false;
                }
            }
            //
        }
        //删除一条记录 
        $scope.deleteItem = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                referralRecRes.delete({ id: item.Id }, function (data) {
                    if (data.ResultCode == 0)
                        utility.message("资料删除成功！");

                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                });
            }
        };

        //数据保存函数
        $scope.saveReferral = function (item) {
            item.RegNo = $scope.currentResident.RegNo;
            item.FeeNo = $scope.currentResident.FeeNo;
            if ($scope.rfrForm.$valid) {//判断验证通过后才可以保存
                referralRecRes.save(item, function (data) {
                    if (data.ResultCode == 0) {
                        if (angular.isDefined(item.Id)) {
                            utility.message("资料更新成功！");
                        }
                        else {
                            //$scope.referralRecs.push(data.Data);
                            utility.message("资料保存成功！");
                        }
                        $scope.currentItem = {};
                        $scope.getReferralByNo($scope.currentResident.FeeNo);//加载当前住民的生活记录列表
                    }
                    else {
                        utility.message("操作失败,请联系管理员!")
                    }
                });
            }
            else {
                //验证没有通过
                $scope.getErrorMessage($scope.rfrForm.$error);
                $scope.errs = $scope.errArr.reverse();
                for (var n = 0; n < $scope.errs.length; n++) {
                    if (n < 3) {
                        utility.msgwarning($scope.errs[n]);
                    }
                    //if (n > 2) break;
                }
            }

        }
        ///编辑或新增时弹出窗
        $scope.openWin = function (item) {

            $scope.currentItem = item ? item : {};
            $("#modalReferralRec").modal("toggle");
        };

        //窗口关闭操作
        $scope.cancelEdit = function () {
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }

            $scope.currentItem = {};
            $("#modalReferralRec").modal("toggle");
        };
        //选择填写人员
        $scope.staffSelected = function (item) {
            $scope.currentItem.EmpNo = item.EmpNo;
            $scope.currentItem.EmpNoName = item.EmpName;
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
