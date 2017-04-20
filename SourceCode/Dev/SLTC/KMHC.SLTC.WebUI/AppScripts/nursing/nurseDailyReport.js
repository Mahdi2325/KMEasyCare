angular.module("sltcApp").controller("nurseDailyReportListCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'nurseDailyReportList', 'nurseDailyReportpipe', 'utility', 'inValueRes', 'outValueRes', 'vitalSignRes', 'pipelineevalRes',
    function ($scope, $http, $location, $state, dictionary, nurseDailyReportList, nurseDailyReportpipe, utility, inValueRes, outValueRes, vitalSignRes, pipelineevalRes) {
        $scope.FeeNo = $state.params.FeeNo;
        //首次加载页面执行的方法
        $scope.init = function () {
            getname();
            $scope.Data = {};
            $scope.filter = { REGNO: null };
            $scope.CurrentPage = 1;
            $scope.record = {};
            $scope.btn_cc = false;
            $scope.div_cc = false;


            $scope.show = [

             { "value": true, "text": "有" },

             { "value": false, "text": "无" },
            ]

            $scope.showOx = [

                 { "value": "1", "text": "有" },

                 { "value": "0", "text": "无" },
            ]
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: nurseDailyReportList,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.item = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    REGNO: -1,
                    FEENO: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        };
        $scope.search = function () {

            $scope.options.params.FEENO = $scope.filter.FEENO;
            $scope.options.params.REGNO = $scope.filter.REGNO;
            $scope.options.search();
        };
        function getname() {

            nurseDailyReportList.get(function (obj) {
                $scope.names = obj.Data;

            });
        }
        function fristselect() {
            $scope.record.NASOGASTRIC = false;

            $scope.record.CATHETER = false;

            $scope.record.TRACHEOSTOMY = false;

            $scope.record.STOMAFISTULA = false;

            $scope.record.WOUNDSKINCARE = false;
            $scope.record.SPRAYINHALATION = false;

            $scope.record.OXYGENUSE = "0";
        }

        function resetPipeline() {
            $scope.record.NASOGASTRIC = false;
            $scope.record.CATHETER = false;
            $scope.record.TRACHEOSTOMY = false;
        };

        $scope.residentSelected = function (resident) {

            $scope.record = {};

            //这边获取几个值，
            $scope.filter.REGNO = resident.RegNo;

            //这边添加新的东西
            $scope.filter.FEENO = resident.FeeNo;

            $scope.curUser = utility.getUserInfo();

            $scope.record.RECORDBY = $scope.curUser.EmpNo;

            //这边按键的控制

            $scope.btn_cc = true;
            fristselect();

            $scope.search();
        }

        $scope.LoadBODY = function () {
            if ($scope.filter.FEENO == null) {

                alert("住院咨询不能为空");
            }
            else {
                //加载生命体

                if ($scope.record.RECDATE == null || $scope.record.CLASSTYPE == null) {

                    alert("日期与班别不能为空");

                }
                else {
                    resetPipeline();

                    vitalSignRes.get({ FEENO: $scope.filter.FEENO, CLASSTYPE: $scope.record.CLASSTYPE, RECDATE: $scope.record.RECDATE }, function (data) {
                        if (data.Data) {
                            $scope.record.SBP = data.Data.SBP
                            $scope.record.BODYTEMP = data.Data.Bodytemp;
                            $scope.record.DBP = data.Data.DBP;
                            $scope.record.BREATH = data.Data.Breathe;
                            $scope.record.PULSE = data.Data.Pulse;
                            $scope.record.OXYGEN = data.Data.Oxygen;
                            var str1 = "001002003004005";
                            var str2 = "006007008";
                            if (str1.indexOf(data.Data.BSType) != -1) {
                                $scope.record.FPG = data.Data.BloodSugar;
                            } else if (str2.indexOf(data.Data.BSType) != -1) {
                                $scope.record.PPBS = data.Data.BloodSugar;
                            }
                        } else {
                            $scope.record.SBP = undefined
                            $scope.record.BODYTEMP = undefined;
                            $scope.record.DBP = undefined;
                            $scope.record.BREATH = undefined;
                            $scope.record.PULSE = undefined;
                            $scope.record.OXYGEN = undefined;
                            $scope.record.FPG = undefined;
                            $scope.record.PPBS = undefined;

                        }
                    });

                    outValueRes.get({ FEENO: $scope.filter.FEENO, CLASSTYPE: $scope.record.CLASSTYPE, RECDATE: $scope.record.RECDATE }, function (data) {
                        if (data.Data) {
                            $scope.record.OUTVALUE = data.Data.OutValue;
                        } else {
                            $scope.record.OUTVALUE = undefined;
                        }


                        //$scope.record.INVALUE = data.Data[0].intvalue;

                    });

                    inValueRes.get({ FEENO: $scope.filter.FEENO, CLASSTYPE: $scope.record.CLASSTYPE, RECDATE: $scope.record.RECDATE }, function (data) {
                        if (data.Data) {
                            $scope.record.INVALUE = data.Data.InValue;
                        } else {
                            $scope.record.INVALUE = undefined;
                        }

                        //$scope.record.INVALUE = data.Data[0].intvalue;

                    });

                    pipelineevalRes.get({ feeNo: $scope.filter.FEENO, pipeLineName: '003', recDate: $scope.record.RECDATE }, function (data4) {
                        if (data4.ResultCode == 1001) {
                            $scope.record.CATHETER = true;
                        } if (data4.ResultCode == 1002) {
                            $scope.record.CATHETER == false;
                        }
                    });

                    //鼻胃管
                    pipelineevalRes.get({ feeNo: $scope.filter.FEENO, pipeLineName: '005', recDate: $scope.record.RECDATE }, function (data1) {
                        if (data1.ResultCode == 1001) {
                            $scope.record.NASOGASTRIC = true;
                        } if (data1.ResultCode == 1002) {
                            $scope.record.NASOGASTRIC == false;
                        }
                    });
                    pipelineevalRes.get({ feeNo: $scope.filter.FEENO, pipeLineName: '004', recDate: $scope.record.RECDATE }, function (data2) {
                        if (data2.ResultCode == 1001) {
                            $scope.record.TRACHEOSTOMY = true;
                        } if (data2.ResultCode == 1002) {
                            $scope.record.TRACHEOSTOMY == false;
                        }
                    });
                    pipelineevalRes.get({ feeNo: $scope.filter.FEENO, pipeLineName: '002', recDate: $scope.record.RECDATE }, function (data3) {
                        if (data3.ResultCode == 1001) {
                            $scope.record.CATHETER = true;
                        } if (data3.ResultCode == 1002) {
                            $scope.record.CATHETER == false;
                        }
                    });
                }
            }
        }

        // 这边复制拷贝的密码
        $scope.recordModify = function (item) {
            $scope.record = angular.copy(item);
        }
        //这边是删除的
        $scope.delete = function (id) {
            if (confirm("确定删除该信息吗?")) {
                nurseDailyReportList.delete({ id: id }, function (data) {
                    utility.message("删除成功");
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.record = {};
                    $scope.search();
                });
            }
        };
        $scope.checkall = function () {
            if ($scope.record.PAINFLAG == false) {
                $scope.div_cc = false;
            }
            else {
                $scope.div_cc = true;
            }
        };
        $scope.staffSelected = function (item, flag) {
            switch (flag) {
                case "RECORDBY": {
                    $scope.record.RECORDBY = item.EmpNo;
                    break;
                }
                case "NEXTCHECKBY": {
                    $scope.record.NEXTRECORDBY = item.EmpNo;
                    break;
                }
            }
        }


        // 插入新的
        $scope.insertinto = function () {

            if ($scope.lifeRecForm.$valid) {

                if ($scope.filter.REGNO != null) {
                    //这边设置一个数组,这边添加的一个id，这样就可以达到编辑的效果了,加入新的东西
                    var para = {

                        REGNO: $scope.filter.REGNO,

                        FEENO: $scope.filter.FEENO,

                        ID: $scope.record.ID,

                        RECDATE: $scope.record.RECDATE,

                        CLASSTYPE: $scope.record.CLASSTYPE,

                        RECORDBY: $scope.record.RECORDBY,

                        BODYTEMP: $scope.record.BODYTEMP,

                        SBP: $scope.record.SBP,

                        DBP: $scope.record.DBP,

                        BREATH: $scope.record.BREATH,

                        PULSE: $scope.record.PULSE,

                        OXYGEN: $scope.record.OXYGEN,

                        OUTVALUE: $scope.record.OUTVALUE,

                        INVALUE: $scope.record.INVALUE,

                        EDEMA: $scope.record.EDEMA,

                        OTHERDESC: $scope.record.OTHERDESC,

                        NOISEI1: $scope.record.NOISEI1,

                        NOISEI2: $scope.record.NOISEI2,

                        NOISEI3: $scope.record.NOISEI3,

                        FPG: $scope.record.FPG,

                        PPBS: $scope.record.PPBS,

                        NASOGASTRIC: $scope.record.NASOGASTRIC,

                        CATHETER: $scope.record.CATHETER,

                        TRACHEOSTOMY: $scope.record.TRACHEOSTOMY,

                        STOMAFISTULA: $scope.record.STOMAFISTULA,

                        WOUNDSKINCARE: $scope.record.WOUNDSKINCARE,

                        SPRAYINHALATION: $scope.record.SPRAYINHALATION,

                        OXYGENUSE: $scope.record.OXYGENUSE,

                        APPETITE: $scope.record.APPETITE,

                        VOMITINGTIMES: $scope.record.VOMITINGTIMES,

                        SLEEPHOURS: $scope.record.SLEEPHOURS,

                        SLEEPSTATE: $scope.record.SLEEPSTATE,
                        //是否是生理期
                        MENSTRUALCYCLE: $scope.record.MENSTRUALCYCLE,

                        DEFECATIONWAY: $scope.record.DEFECATIONWAY,

                        INTESTINALPERISTALSIS: $scope.record.INTESTINALPERISTALSIS,

                        STOOLNATURE: $scope.record.STOOLNATURE,

                        STOOLTIMES: $scope.record.STOOLTIMES,

                        REHABILITATION: $scope.record.REHABILITATION,

                        OUTBEDNUMBER: $scope.record.OUTBEDNUMBER,

                        CONSTRAINTSEVAL: $scope.record.CONSTRAINTSEVAL,

                        SKININTEGRITY: $scope.record.SKININTEGRITY,

                        DOCDIAGFLAG: $scope.record.DOCDIAGFLAG,

                        PAINFLAG: $scope.record.PAINFLAG,

                        PAINPART: $scope.record.PAINPART,

                        PAINLEVEL: $scope.record.PAINLEVEL,

                        PAINPRESCRIPTION: $scope.record.PAINPRESCRIPTION,

                        SIDEEFFECT: $scope.record.SIDEEFFECT,

                        PAINCARE: $scope.record.PAINCARE,

                        THERAPYRESPONSE: $scope.record.THERAPYRESPONSE,

                        PULMONARYMURMUR: $scope.record.PULMONARYMURMUR,

                        SPUTUMCOLOR: $scope.record.SPUTUMCOLOR,

                        REGTALKFLAG: $scope.record.REGTALKFLAG,

                        FAMILYTALKFLAG: $scope.record.FAMILYTALKFLAG,

                        CARERECFLAG: $scope.record.CARERECFLAG
                    };
                    nurseDailyReportList.save(para, function (obj) {

                        utility.message("保存成功");
                        //这边方法是多余的。
                        $scope.BtnReset();
                        $scope.search();
                    });
                }
                else {

                    alert("请先选中住民咨询");
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

        $scope.BtnReset = function () {

            $scope.record = {};

            $scope.record.NASOGASTRIC = false;

            $scope.record.CATHETER = false;

            $scope.record.TRACHEOSTOMY = false;

            $scope.record.STOMAFISTULA = false;

            $scope.record.WOUNDSKINCARE = false;
            $scope.record.SPRAYINHALATION = false;
            $scope.record.OXYGENUSE = "0";



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
