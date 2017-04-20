
angular.module("sltcApp").controller("ReferralListCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'ReferralLists', 'utility', 'empFileResGet', 'vitalSignRes',
    function ($scope, $http, $location, $state, dictionary, ReferralLists, utility, empFileResGet, vitalSignRes) {
        $scope.FeeNo = $state.params.FeeNo;
        //首次加载页面执行的方法
        $scope.init = function () {
            getname();

            $scope.Data = {};
            $scope.filter = { FEENO: null };
            $scope.CurrentPage = 1;
            $scope.btn_cc = false;
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.record = { ASSESSBY: $scope.curUser.EmpNo };
            }

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: ReferralLists,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.item = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    FEENO: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        };

        function getname() {

            ReferralLists.get(function (obj) {
                $scope.names = obj.Data;

            });
        }

        // 这边复制拷贝的密码
        $scope.recordModify = function (item) {
            $scope.record = angular.copy(item);
        }

        //这边是获取
        $scope.residentSelected = function (resident) {
            if (angular.isDefined($scope.lifeRecForm.$error.pattern)) {
                var errorLength = $scope.lifeRecForm.$error.pattern.length;
                for (var i = errorLength - 1; i >= 0; i--) {
                    $scope.lifeRecForm.$error.pattern[i].$setViewValue(undefined);
                }
            }
            $scope.lifeRecForm.$setPristine();
            $scope.lifeRecForm.$setUntouched();
            $scope.lifeRecForm.$rollbackViewValue();


            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.record = { ASSESSBY: $scope.curUser.EmpNo };
            }
            $scope.btn_cc = true;


            //这边获取几个值，
            $scope.filter.REGNO = resident.RegNo;
            //这边添加新的东西
            $scope.filter.FEENO = resident.FeeNo;

            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.FEENO = resident.FeeNo;
            $scope.options.search();

            vitalSignRes.get({ regNo: resident.RegNo, feeNo: resident.FeeNo }, function (data) {
                if (angular.isDefined(data.Data)) {
                    if (data.Data.length > 0) {
                        $scope.record.BODYTEMP = data.Data[0].Bodytemp;
                        $scope.record.PULSE = data.Data[0].Pulse;
                        //$scope.record.BREATHE = data.Data[0].Breathe;
                        //$scope.record.SBP = data.Data[0].SBP;
                        //$scope.record.DBP = data.Data[0].DBP;
                    }
                }
            });
            $scope.record.DIAGNOSIS = resident.DiseaseDiag;
            $scope.record.HEIGHT = resident.Height;
            $scope.record.WEIGHT = resident.Weight;
            if (angular.isNumber($scope.record.HEIGHT) && angular.isNumber($scope.record.WEIGHT)) {
                if ($scope.record.HEIGHT != 0) {
                    $scope.record.BMI = ($scope.record.WEIGHT / ($scope.record.HEIGHT * $scope.record.HEIGHT) * 10000).toFixed(1);
                    var idWeight = Math.round((22 * $scope.record.HEIGHT * $scope.record.HEIGHT) / 10000);
                    $scope.record.WEIGHT_S = idWeight;
                }
            }
            //empFileResGet.get({ regon: resident.RegNo }, function (data) {
            //    if (data.Data != null) {
            //        $scope.record.HEIGHT = data.Data.Height;
            //        $scope.record.WEIGHT = data.Data.Weight;
            //        if (angular.isNumber($scope.record.HEIGHT) && angular.isNumber($scope.record.WEIGHT)) {
            //            $scope.record.BMI = ($scope.record.WEIGHT / ($scope.record.HEIGHT * $scope.record.HEIGHT) * 10000).toFixed(1);
            //        }
            //    }

            //});

        }

        //这边是更新的一个方法
        $scope.insertinto = function () {
            if ($scope.lifeRecForm.$valid) {
                if ($scope.filter.REGNO != null) {
                    //这边设置一个数组,这边添加的一个id，这样就可以达到编辑的效果了,加入新的东西

                    var para = {
                        REGNO: $scope.filter.REGNO,
                        FEENO: $scope.filter.FEENO,
                        ID: $scope.record.ID,
                        TRANSDATE: $scope.record.TRANSDATE,
                        DEPTNO: $scope.record.DEPTNO,
                        DISEASECOUNT: $scope.record.DISEASECOUNT,
                        DIAGNOSIS: $scope.record.DIAGNOSIS,
                        CURRENTISSUE: $scope.record.CURRENTISSUE,
                        HEIGHT: $scope.record.HEIGHT,
                        KNEELENGTH: $scope.record.KNEELENGTH,
                        WEIGHT: $scope.record.WEIGHT,
                        WEIGHT_S: $scope.record.WEIGHT_S,
                        BMI: $scope.record.BMI,
                        BODYTEMP: $scope.record.BODYTEMP,
                        PULSE: $scope.record.PULSE,
                        BP: $scope.record.BP,
                        BLOODOXYGEN: $scope.record.BLOODOXYGEN,
                        BS: $scope.record.BS,
                        EKG: $scope.record.EKG,
                        EAT: $scope.record.EAT,
                        SWALLOW: $scope.record.SWALLOW,
                        DIETMODE: $scope.record.DIETMODE,
                        STOMACH: $scope.record.STOMACH,
                        FOODTABOO: $scope.record.FOODTABOO,
                        ACTIVITY: $scope.record.ACTIVITY,
                        CURRENTDIET: $scope.record.CURRENTDIET,
                        FOODINTAKE: $scope.record.FOODINTAKE,
                        WATER: $scope.record.WATER,
                        NUTRITION: $scope.record.NUTRITION,
                        SNACKS: $scope.record.SNACKS,
                        PIPLELINE: $scope.record.PIPLELINE,
                        MEDICINE: $scope.record.MEDICINE,
                        CHECKDESC: $scope.record.CHECKDESC,
                        ASSESSDATE: $scope.record.ASSESSDATE,
                        ASSESSBY: $scope.record.ASSESSBY,
                        REPLYDATE: $scope.record.REPLYDATE,
                        SUPERVISOR: $scope.record.SUPERVISOR,
                        SUGGESTSUMMARY: $scope.record.SUGGESTSUMMARY,
                        CURRENTDIETSTATE: $scope.record.CURRENTDIETSTATE

                    };
                    ReferralLists.save(para, function (obj) {
                        $scope.curUser = utility.getUserInfo();
                        if (typeof ($scope.curUser) != 'undefined') {
                            $scope.record = { ASSESSBY: $scope.curUser.EmpNo };
                        }
                        utility.message("保存成功");
                        //这边方法是多余的。
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                    });
                }
                else {

                    alert("请选择人员后，再添加转诊信息");

                }
            }
            else {
                $scope.getErrorMessage($scope.lifeRecForm.$error);
                $scope.errs = $scope.errArr.reverse();
                for (var n = 0; n < $scope.errs.length; n++) {
                    if (n != 3) {
                        utility.msgwarning($scope.errs[n]);
                    }
                }

            }
        };
        //这边是删除的
        $scope.delete = function (id) {
            if (confirm("确定删除该信息吗?")) {
                ReferralLists.delete({ id: id }, function (data) {

                    utility.message("删除成功");
                    $scope.CurrentPage = 1;
                    $scope.curUser = utility.getUserInfo();
                    if (typeof ($scope.curUser) != 'undefined') {
                        $scope.record = { ASSESSBY: $scope.curUser.EmpNo };
                    }
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                });
            }
        };

        //这边选择过来的值

        $scope.staffSelected = function (item, flag) {
            //加载人员

            switch (flag) {
                case "SUPERVISOR": {
                    $scope.record.SUPERVISOR = item.EmpNo;
                    break;
                }
                case "ASSESSBY": {
                    $scope.record.ASSESSBY = item.EmpNo;
                    break;
                }

            }

        }

        //验证信息
        $scope.getErrorMessage = function (error) {
            $scope.errArr = new Array();
            //var errorMsg = '';
            if (angular.isDefined(error)) {
                if (error.required) {
                    $.each(error.required, function (n, value) {
                        $scope.errArr.push(value.$name + "不能为空");
                    });
                }
                //if (error.maxlength) {
                //    $.each(error.maxlength, function (n, value) {
                //        $scope.errArr.push(value.$name + "邮箱验证失败");
                //    });
                //}
                //if (error.number, function (n, value) {
                //    $scope.errArr.push(value.$name + "只能录入数字");
                //});
                if (error.number) {
                    $.each(error.number, function (n, value) {
                        $scope.errArr.push(value.$name + "只能录入数字！");
                    });

                }


                if (error.maxlength) {
                    $.each(error.maxlength, function (n, value) {
                        $scope.errArr.push(value.$name + "输入过长！");
                    });

                }

                //return errorMsg;
            }

        };

        $scope.init();
    }
]);












