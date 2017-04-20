/*****************************************************************************
 * Filename: injection
 * Creator:	Lei Chen
 * Create Date: 2016-03-01 13:34:19
 * Modifier:
 * Modify Date:
 * Description:针剂
 ******************************************************************************/
//angular.module("sltcApp").controller("injectionCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'injectionRes','empFileRes',
//    function ($scope, $http, $location, $state, dictionary, injectionRes, empFileRes) {
//        var id = $state.params.id;
//        var regNo = $state.params.regId;
//        $scope.Data = {};
//        $scope.Data.Injection = {};

//        $scope.init = function () {

//            empFileRes.get({ empNo: '' }, function (data) {
//                $scope.EmpList = data.Data;
//            });

//            $scope.Data.E00 = [{ value: "", name: "" }, { value: "001", name: "H1N1流感" }, { value: "002", name: "肺炎疫苗" }, { value: "003", name: "流行性感冒疫苗" }];
//            $scope.Data.Gap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];

//            if (id> 0) {
//                injectionRes.get({ id: id }, function (data) {
//                    $scope.Data.Injection = data.Data;
//                    $scope.Data.Injection.ID = id;
//                    $scope.Data.Injection.INJECTDATE = transferDate($scope.Data.Injection.INJECTDATE);
//                });

//            } else {
//                $scope.Data.Injection.REGNO = regNo;
//            }

//        }
//        $scope.init();

//        $scope.Delete = function (currentItem) {
//            $scope.Data.pipelineEvalList.splice(0, 1)
//        }

//        $scope.Save = function () {
//            injectionRes.save($scope.Data.Injection, function (data) {
//                if (data.ResultCode == 0) {
//                    alert("保存成功");
//                    $location.url('/angular/InjectionList');
//                } else {
//                    alert(data.ResultMessage);
//                }
//            });
//        }

//        $scope.setNextValDate = function () {
//            var gap = $scope.Data.Injection.INTERVAL;
//            var currentDate = $scope.Data.Injection.INJECTDATE;
//            var t = GetNextEvalDate(currentDate, gap);
//            $scope.Data.Injection.NEXTINJECTDATE = GetNextEvalDate(currentDate, gap);
//        }


//    }
//]);

function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = year + seperator1 + month + seperator1 + strDate;
    return currentdate;
}
//界面中的初始化中的东西
angular.module("sltcApp").controller("injectionListCtr", ['$scope', '$http','$filter', '$location', '$state', '$timeout', 'dictionary', 'injectionRes', 'empFileRes', 'biochemistryRes','nursingRecordRes', 'utility',
    function ($scope, $http,$filter, $location, $state, $timeout, dictionary, injectionRes, empFileRes, biochemistryRes, nursingRecordRes, utility) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.filter = { REGNO: null, FEENO: null, Name: null };
        $scope.Data = {};
        $scope.Data.Injection = {};


        $scope.init = function () {

            // 大饼

            //empFileRes.get({ empNo: '' }, function (data) {
            //    $scope.EmpList = data.Data;
            //});
            $scope.Data.SyncNurRec = true;
            $scope.CurrentPage = 1;

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: injectionRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.InjectionList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    REGNO: -1
                }
            }
            //大饼

            getname();

            $scope.Data.Filter =
                {
                    "Id": 1, "ItemType": '', "RegNO": null, "Date": '', 'Name': '', 'CurrentPage': 0, 'PageSize': 0, 'TotalRecords': 0
                };

            $scope.Data.Gap = [{ value: "7", name: "7" }, { value: "30", name: "30" }, { value: "60", name: "60" }, { value: "90", name: "90" }];
            $scope.Data.E00 = [{ value: "", name: "" }, { value: "001", name: "H1N1流感" }, { value: "002", name: "肺炎疫苗" }, { value: "003", name: "流行性感冒疫苗" }];
            $timeout(function () {
                $http.post("api/Code", { ItemTypes: ["E00.324"] }).success(function (response, status, headers, config) {
                    $.each(response.Data, function (key, value) {
                        $scope.codeE = response.Data[key];

                    });

                });
            }, 100);
        }

        function getname() {

            biochemistryRes.get(function (obj) {
                $scope.names = obj.Data;
            });
        }

        $scope.search = function () {

            $scope.options.params.REGNO = $scope.filter.REGNO;
            $scope.options.search();
        };

        $scope.residentSelected = function (resident) {

            $scope.Data.Injection = {};
            //这边获取几个值，
            $scope.filter.REGNO = resident.RegNo;

            //这边添加新的东西
            $scope.filter.FEENO = resident.FeeNo;

            $scope.filter.Name = resident.Name;

            $scope.Data.InjectionList = [];

            $scope.curUser = utility.getUserInfo();

            $scope.Data.Injection.OPERATOR = $scope.curUser.EmpNo;
            $scope.Data.Injection.INJECTDATE = getNowFormatDate();
            $scope.search();

        };

        $scope.Edit = function (currentItem) {
            $scope.Data.pipelineEval = currentItem;

        }
        $scope.Delete = function (currentItem) {
            $scope.Data.pipelineEvalList.splice(0, 1)
        }

        //下次日期
        $scope.setNextValDate = function () {
            var gap = $scope.Data.Injection.INTERVAL;
            var currentDate = $scope.Data.Injection.INJECTDATE;
            var t = GetNextEvalDate(currentDate, gap);
            $scope.Data.Injection.NEXTINJECTDATE = GetNextEvalDate(currentDate, gap);
        }


        $scope.createItem = function () {

            $scope.Data.Injection.NAME = $scope.filter.Name;

            $scope.Data.Injection.REGNO = $scope.filter.REGNO;

            injectionRes.save($scope.Data.Injection, function (data) {
                if (data.ResultCode == 0) {

                    $scope.CurrentPage = 1;

                    if ($scope.Data.SyncNurRec) {
                        $scope.record = {};
                        $scope.record.FeeNo = $scope.filter.FEENO;
                        $scope.record.RegNo = $scope.filter.REGNO;
                        //$scope.record.RecordDate = data.Data.RECDATE;
                        var d = new Date();
                        $scope.record.RecordDate = $filter("date")(d, "yyyy-MM-dd HH:mm:ss");
                        var recNameBy = "";
                        var recName = "";
                        var recDate = "";

                        $scope.curUser = utility.getUserInfo();
                        if (typeof ($scope.curUser) != 'undefined') {
                            recName = $scope.curUser.EmpName;
                            recNameBy = $scope.curUser.EmpNo;
                        }

                        if (angular.isDefined($scope.Data.Injection.INJECTDATE)) {
                            recDate = $scope.Data.Injection.INJECTDATE.substring(0, 4) + "年" + $scope.Data.Injection.INJECTDATE.substring(5, 7) + "月" + $scope.Data.Injection.INJECTDATE.substring(8, 10) + "日";
                        }

                        $scope.record.RecordNameBy = recName;
                        $scope.record.RecordBy = recNameBy;
                        var myDate = new Date();
                        var h = myDate.getHours();
                        if (h >= 0 && h < 8) {
                            $scope.record.ClassType = "N";
                        }
                        if (h >= 8 && h < 16) {
                            $scope.record.ClassType = "D";
                        }
                        if (h >= 16 && h < 24) {
                            $scope.record.ClassType = "E";
                        }
                        $scope.record.Content = "";
                        if (recName != "") {
                            $scope.record.Content += "负责人员：" + recName + ";";
                        }
                        if (recDate != "") {
                            $scope.record.Content += "注射日期：" + recDate + ";";
                        }
                        if (angular.isDefined($scope.Data.Injection.ITEMTYPE)) {
                            $scope.record.Content += "注射疫苗种类：" + $scope.Data.Injection.ITEMTYPE + ";";
                        }

                        nursingRecordRes.save($scope.record, function () {
                            utility.message("已同步写入护理记录成功，如需完善，请到护理记录及交班模块操作");
                        });
                    }

                    $scope.Data.Injection = {};
                    $scope.Data.Injection.INJECTDATE = getNowFormatDate();
                    utility.message("保存成功");
                    $scope.search();

                } else {

                    alert(data.ResultMessage);
                }
            });
        }

        $scope.updateItem = function ()
        {
            $scope.Data.Injection.NAME = $scope.filter.Name;

            $scope.Data.Injection.REGNO = $scope.filter.REGNO;

            injectionRes.save($scope.Data.Injection, function (data) {
                if (data.ResultCode == 0) {

                    $scope.CurrentPage = 1;
                    $scope.Data.Injection = {};
                    utility.message("保存成功");
                    $scope.search();
                }
            });
        }


        $scope.Save = function () {

            if ($scope.lifeRecForm.$valid) {

                if (angular.isDefined($scope.Data.Injection.ID)) {
                    $scope.updateItem();
                } else {
                    $scope.createItem();
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

        }

        $scope.staffSelected = function (item, flag) {

            switch (flag) {
                case "NEXTOPERATEBY": {
                    $scope.Data.Injection.NEXTOPERATEBY = item.EmpNo;
                    break;
                }
                case "tt": {
                    $scope.Data.Injection.OPERATOR = item.EmpNo;
                    break;
                }


            }
        }

        $scope.recordModify = function (item) {

            var now = new Date(item.INJECTDATE);

            var date = new Date(item.NEXTINJECTDATE);

            var diff = date.valueOf() - now.valueOf();

            var cday = parseInt(diff / (1000 * 60 * 60 * 24));

            item.INTERVAL = cday.toString();

            $scope.Data.Injection = angular.copy(item);
        }

        //删除id的这块
        $scope.delete = function (id) {
            //这边是id的感觉.
            injectionRes.delete({ id: id }, function (data) {
                $scope.Data.Injection = {};
                $scope.Data.InjectionList = {};
                $scope.options.pageInfo.CurrentPage = 1;
                alert("删除成功");
                $scope.search();
            });
        }

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
                if (error.maxlength) {
                    $.each(error.maxlength, function (n, value) {
                        $scope.errArr.push(value.$name + "输入过长！");
                    });

                }

                //return errorMsg;
            }

        };
        $scope.init();//初始化

    }
]);


//angular.module("sltcApp").controller("injectionHisCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'injectionRes',
//    function ($scope, $http, $location, $state, dictionary, injectionRes) {
//        $scope.Data = {};
//        var regNo = $state.params.regNo;
//        $scope.init = function () {
//            $scope.Data.Filter =
//                {
//                    "Id": 1, "ItemType": '', "RegNO": null, "Date": '', 'Name': '', 'CurrentPage': 0, 'PageSize': 0, 'TotalRecords': 0
//                };

//            $scope.Data.Gap = [{ value: "7", name: "7" }, { value: "30", name: "30" }, { value: "60", name: "60" }, { value: "90", name: "90" }];
//            $scope.Data.E00 = [{ value: "", name: "" }, { value: "001", name: "H1N1流感" }, { value: "002", name: "肺炎疫苗" }, { value: "003", name: "流行性感冒疫苗" }];
//        }

//        $scope.init();//初始化

//        injectionRes.get({ regNo: regNo }, function (data) {
//            $scope.Data.InjectionHisList = data.Data;
//        });

//    }
//]);






