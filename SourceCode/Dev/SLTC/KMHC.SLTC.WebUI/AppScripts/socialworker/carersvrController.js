angular.module("sltcApp")
.controller("carersvrCtrl", ['$scope', '$state', '$filter', 'utility', 'dictionary', 'carersvrRes', 'empFileRes', 'carePlanRes', 'carePlanActivityRes', 'relationDtlRes',
    function ($scope, $state, $filter, utility, dictionary, carersvrRes, empFileRes, carePlanRes, carePlanActivityRes, relationDtlRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        //当前操作对象
        $scope.currentItem = {};
        //当前住民
        $scope.currentResident = {};

        $scope.buttonShow = false;

        //当前分页码
        $scope.currentPage = 1;



        $scope.init = function () {
            //加载人员字典列表
            empFileRes.get({ empNo: '' }, function (data) {
                $scope.Data.EmpList = data.Data;
            });

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: carersvrRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.carersvrs = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: -1
                }
            }

            var ss = $("#ContNamediv").width();
            $("#selContName").css('width', ss);
            $("#spanwidth").css("margin-left", ss - 20);
            $("#selContName").css('margin-left', -(ss - 20));
            $("#inputwidth").css('width', ss - 20);
        }


        //选中住民
        $scope.residentSelected = function (resident) {

            $scope.currentResident = resident;

            $scope.getCarerSvrByNo(resident.FeeNo)

            $scope.loadQL();

            $scope.currentItem = {};//清空编辑项
            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.currentItem.RegNo = resident.RegNo;
            $scope.currentItem.FeeNo = resident.FeeNo;
            $scope.currentItem.SvrAddress = "001";
            $scope.currentItem.SvrType = "002";
            $scope.currentItem.RecDate = new Date().format("yyyy-MM-dd");
            $scope.curUser = utility.getUserInfo();
            $scope.currentItem.Carer = $scope.curUser.EmpNo;
            relationDtlRes.get({ FeeNo: $scope.currentResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
                $scope.Data.ContactList = data.Data;
            });
        }
        //获取社工服务记录
        $scope.getCarerSvrByNo = function (feeNo) {

            $scope.buttonShow = true;
            $scope.Data.carersvrs = {};

            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = feeNo;
            $scope.options.search();

            //carersvrRes.get({ feeNo: feeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {

            //    $scope.carersvrs = data.Data;
            //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

            //        $scope.currentPage = curPage;
            //        $scope.getCarerSvrByNo(feeNo);
            //    });
            //});
        }
        //加载问题层面列表数据
        $scope.loadQL = function () {
            carePlanRes.get({ category: '' }, function (data) {
                $scope.Data.Level = data.Data;
            });
        };
        //改变问题层面获取问题焦点字典列表
        $scope.changeLevel = function (newItem) {
            carePlanRes.get({ category: "001", levelPR: newItem }, function (data) {
                $scope.Data.Diag = data.Data;
                $scope.currentItem.ProcessActivity = "";
            });
        }
        //根据问题焦点改变获取处理措施字典列表
        $scope.changeQuestion = function (newItem) {
            carePlanActivityRes.get({ cp_no: newItem }, function (data) {
                $scope.Data.CarePlanActivityList = data.Data;
            });
        }
        //保存社工服务记录
        $scope.saveCarersvr = function (item) {
            $scope.currentItem.RegNo = $scope.currentResident.RegNo;
            $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
            if ($scope.crForm.$valid) {
                carersvrRes.save(item, function (data) {

                    if (angular.isDefined(item.Id)) {
                        utility.message("资料更新成功！");
                        $scope.currentItem = {};
                    }
                    else {
                        $scope.getCarerSvrByNo($scope.currentResident.FeeNo);
                        // $scope.carersvrs.push(data.Data);
                        utility.message("资料保存成功！");
                        $scope.currentItem = {};
                    }
                });
            } else {
                //验证没有通过
                $scope.getErrorMessage($scope.crForm.$error);
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
        //删除社工记录
        $scope.carersvrDelete = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                carersvrRes.delete({ id: item.Id }, function (data) {
                    if (data.$resolved) {
                        var whatIndex = null;
                        angular.forEach($scope.carersvrs, function (cb, index) {
                            if (cb.id = item.Id) whatIndex = index;
                        });

                        if (data.ResultCode == 0)
                            utility.message("资料删除成功！");
                        //$scope.carersvrs.splice(whatIndex, 1);
                        $scope.options.pageInfo.CurrentPage = 1;
                        //$scope.options.params.feeNo = feeNo;
                        $scope.options.search();
                    }
                });
            }
        }
        $scope.editOrCreate = function (item) {

            carePlanRes.get({ category: "001", levelPR: item.QuestionLevel }, function (data) {

                $scope.Data.Diag = data.Data;
            });
            carePlanActivityRes.get({ cp_no: item.QuestionFocus }, function (data) {
                $scope.Data.CarePlanActivityList = data.Data;
            });
            $scope.currentItem = item;
        }
        //选择填写人员
        $scope.staffSelected = function (item) {
            $scope.currentItem.CarerName = item.EmpName;
            $scope.currentItem.Carer = item.EmpNo;
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

