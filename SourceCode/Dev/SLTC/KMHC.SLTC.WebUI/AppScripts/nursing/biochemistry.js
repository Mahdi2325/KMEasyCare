
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

angular.module("sltcApp")
    .controller("biochemistryListCtr", ['$scope', '$http', '$filter', '$location', '$state', 'dictionary', 'biochemistryRes', 'biochemistry', 'biochemistrylist', 'nursingRecordRes', 'utility',
    function ($scope, $http, $filter, $location, $state, dictionary, biochemistryRes, biochemistry, biochemistrylist, nursingRecordRes, utility) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.tbEdit = "ui-inline-hide";
        $scope.classEdit = "ui-inline-hide";
        $scope.tt = [];
        //$scope.day_show = true;
        //$scope.Data.item = [];
        $scope.filter = { REGNO: null, FEENO: null };
        //首次加载页面执行的方法
        $scope.init = function () {
            biochemistry.get(function (obj) {
                $scope.produceitem = obj.Data;
            });        //生成项目
            $scope.Data = {};
            $scope.SyncNurRec = true;
            $scope.CurrentPage = 1;
            $scope.btn_s = false;
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: biochemistryRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.item = data.Data;
                    biochemistryRes.get(function (obj) {
                        $scope.names = obj.Data;
                    });
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    FEENO: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
            //分页的
        };
        //生成项目组 
        function showcodes(code) {
            biochemistrylist.get(function (obj) {
                $scope.CheckCode = obj.Data;
            });
        }
        $scope.$watch('day', function (newValue) {

            if (newValue) {
                if (angular.isDefined($scope.record)) {
                    // var tt = addtime($scope.record.CHECKDATE);
                    //$scope.record.NEXTCHECKDATE = addByTransDate(tt, parseInt(newValue));

                    $scope.record.NEXTCHECKDATE = addByTransDate($scope.record.CHECKDATE, parseInt(newValue));

                }

            }

        });

        //$scope.$watch('record.CHECKDATE', function (newValue) {

        //    if (newValue) {
        //        if (angular.isDefined($scope.day)) {
        //            if ($scope.day != "" && $scope.day != null)
        //            {
        //               $scope.record.NEXTCHECKDATE = addByTransDate(newValue, parseInt($scope.day));
        //            }
        //        }
        //    }
        //});
        function addByTransDate(dateParameter, num) {

            var translateDate = "", dateString = "", monthString = "", dayString = "";
            // translateDate = dateParameter.replace("-", "/").replace("-", "/");
            var newDate = new Date(dateParameter);
            newDate = newDate.valueOf();
            newDate = newDate + num * 24 * 60 * 60 * 1000;
            newDate = new Date(newDate);
            //如果月份长度少于2，则前加 0 补位     
            if ((newDate.getMonth() + 1).toString().length == 1) {
                monthString = 0 + "" + (newDate.getMonth() + 1).toString();
            } else {
                monthString = (newDate.getMonth() + 1).toString();
            }
            //如果天数长度少于2，则前加 0 补位     
            if (newDate.getDate().toString().length == 1) {
                dayString = 0 + "" + newDate.getDate().toString();
            } else {
                dayString = newDate.getDate().toString();
            }
            dateString = newDate.getFullYear() + "-" + monthString + "-" + dayString;
            return dateString;
        };
        //$scope.chageint = function () 
        //{

        //    return fasle;
        //}

        //$scope.$watch('record.CHECKDATE', function (newValue) {

        //    if (angular.isDefined(newValue)) {
        //        $scope.day_show = false;
        //    }
        //    else {
        //        $scope.day = "";
        //        $scope.day_show = true;

        //    }
        //});


        //function getname() {

        //    biochemistryRes.get(function (obj) {
        //        $scope.names = obj.Data;
        //    });
        //}
        $scope.staffSelected = function (item, flag) {

            switch (flag) {
                case "RECORDBY": {
                    $scope.record.RECORDBY = item.EmpNo;
                    break;
                }
                case "NEXTCHECKBY": {
                    $scope.record.NEXTCHECKBY = item.EmpNo;
                    break;
                }
            }
        }
        //确认的时候
        $scope.produceCode = function (code) {
            if (code != null) {
                biochemistrylist.get({ code: code }, function (obj) {
                    $scope.Checkgrouplist = obj.Data;

                    for (var t = 0; t < $scope.Checkgrouplist.length; t++) {

                        var cd = {
                            CHECKTYPE: $scope.Checkgrouplist[t].TYPECODE, CHECKITEM: $scope.Checkgrouplist[t].ITEMCODE, DESCRIPTION: $scope.Checkgrouplist[t].DESCRIPTION,
                            RECORDID: $scope.Checkid, LOWBOUND: $scope.Checkgrouplist[t].LOWBOUND, UPBOUND: $scope.Checkgrouplist[t].UPBOUND
                        };
                        $scope.tt.push(cd);

                    }

                    $scope.search();

                });
            }
            else {

                utility.message("请选择套餐类型");
            }
        }

        $scope.search = function () {

            $scope.options.params.FEENO = $scope.filter.FEENO;
            $scope.options.search();
        };

        $scope.DeleteItem = function (currentItem) {

            if (confirm("确定删除该信息吗?")) {

                $scope.tt.splice($scope.tt.indexOf(currentItem), 1);
            }
        }
        $scope.change = function (code) {


            if (code != "" && code != null) {
                biochemistryRes.get({ code: code }, function (obj) {

                    $scope.Itemcode = obj.Data;
                });
            }
            else {

                $scope.Itemcode = null;
                $scope.CheckRecdtl = {};
            }

            //调用函数


        }

        // 第二次更新
        $scope.changes = function (code) {

            //调用函
            biochemistryRes.get({ code: code }, function (obj) {

                $scope.xxx = obj.Data;

            });
        }
        $scope.changeitem = function (code) {

            //调用函
            biochemistry.get({ code: code }, function (obj) {

                $scope.CheckRecdtl.LOWBOUND = obj.Data[0].LOWBOUND;
                $scope.CheckRecdtl.UPBOUND = obj.Data[0].UPBOUND;

            });

        }
        function tt() {

            return false;
        }


        $scope.updateItem = function () {
            var Rc = {
                RECORDID: $scope.Checkid,
                REGNO: $scope.filter.REGNO,
                FEENO: $scope.filter.FEENO,
                RECORDBY: $scope.record.RECORDBY,
                CHECKDATE: $scope.record.CHECKDATE,
                NEXTCHECKDATE: $scope.record.NEXTCHECKDATE,
                NEXTCHECKBY: $scope.record.NEXTCHECKBY,
                HOSPNAME: $scope.record.HOSPNAME,
                CHECKRESULTS: $scope.record.CHECKRESULTS,
                TRACESTATE: $scope.record.TRACESTATE,
                DISEASEDESC: $scope.record.DISEASEDESC,
                DESCRIPTION: $scope.record.DESCRIPTION,
                XRAYFLAG: $scope.record.XRAYFLAG,
                NORMALFLAG: $scope.record.NORMALFLAG
            };
            //这边设置一个数组,这边添加的一个id，这样就可以达到编辑的效果了,加入新的东西
            var para = {

                CheckRec: Rc,
                CheckRecdtl: $scope.tt

            };
            //全部添加的方法
            biochemistrylist.save(para, function (obj) {

                $scope.day = "";
                $scope.CheckRecdtl = {};
                // 这边是删除的东西

                //这边的id是取到的
                //items.RECORDID = bj.ResultCode;
                utility.message("保存成功");
                $scope.search();
                //这边方法是多余的。
            });

            $scope.Data = {};
            $scope.tt = [];
            $scope.record = {};
        }

        $scope.createItem = function () {
            var Rc = {
                RECORDID: $scope.Checkid,
                REGNO: $scope.filter.REGNO,
                FEENO: $scope.filter.FEENO,
                RECORDBY: $scope.record.RECORDBY,
                CHECKDATE: $scope.record.CHECKDATE,
                NEXTCHECKDATE: $scope.record.NEXTCHECKDATE,
                NEXTCHECKBY: $scope.record.NEXTCHECKBY,
                HOSPNAME: $scope.record.HOSPNAME,
                CHECKRESULTS: $scope.record.CHECKRESULTS,
                TRACESTATE: $scope.record.TRACESTATE,
                DISEASEDESC: $scope.record.DISEASEDESC,
                DESCRIPTION: $scope.record.DESCRIPTION,
                XRAYFLAG: $scope.record.XRAYFLAG,
                NORMALFLAG: $scope.record.NORMALFLAG
            };
            //这边设置一个数组,这边添加的一个id，这样就可以达到编辑的效果了,加入新的东西
            var para = {

                CheckRec: Rc,
                CheckRecdtl: $scope.tt

            };

            //全部添加的方法
            biochemistrylist.save(para, function (obj) {

                if (obj.ResultCode == 1001) {
                    if ($scope.SyncNurRec) {
                        $scope.careRecord = {};
                        $scope.careRecord.FeeNo = $scope.filter.FEENO;
                        $scope.careRecord.RegNo = $scope.filter.REGNO;
                        //$scope.record.RecordDate = data.Data.RECDATE;
                        var d = new Date();
                        $scope.careRecord.RecordDate = $filter("date")(d, "yyyy-MM-dd HH:mm:ss");
                        var recNameBy = "";
                        var recName = "";
                        var recDate = "";

                        $scope.curUser = utility.getUserInfo();
                        if (typeof ($scope.curUser) != 'undefined') {
                            recName = $scope.curUser.EmpName;
                            recNameBy = $scope.curUser.EmpNo;
                        }

                        if (angular.isDefined($scope.record.CHECKDATE)) {
                            recDate = $scope.record.CHECKDATE.substring(0, 4) + "年" + $scope.record.CHECKDATE.substring(5, 7) + "月" + $scope.record.CHECKDATE.substring(8, 10) + "日";
                        }

                        $scope.careRecord.RecordNameBy = recName;
                        $scope.careRecord.RecordBy = recNameBy;
                        var myDate = new Date();
                        var h = myDate.getHours();
                        if (h >= 0 && h < 8) {
                            $scope.careRecord.ClassType = "N";
                        }
                        if (h >= 8 && h < 16) {
                            $scope.careRecord.ClassType = "D";
                        }
                        if (h >= 16 && h < 24) {
                            $scope.careRecord.ClassType = "E";
                        }
                        $scope.careRecord.Content = "";
                        if (recName != "") {
                            $scope.careRecord.Content += "填写人员：" + recName + ";";
                        }
                        if (recDate != "") {
                            $scope.careRecord.Content += "检查日期：" + recDate + ";";
                        }
                        if (angular.isDefined($scope.record.HOSPNAME)) {
                            $scope.careRecord.Content += "检查单位：" + $scope.record.HOSPNAME + ";";
                        }
                        if (angular.isDefined($scope.record.CHECKRESULTS)) {
                            $scope.careRecord.Content += "检查结果：" + $scope.record.CHECKRESULTS + ";";
                        }

                        if ($scope.tt.length > 0) {
                            for (var i = 0; i < $scope.tt.length; i++) {
                                if ($scope.produceitem.length > 0) {
                                    for (var j = 0; j < $scope.produceitem.length; j++) {
                                        if (angular.isDefined($scope.tt[i].CHECKITEM))
                                        {
                                            if ($scope.produceitem[j].ITEMCODE == $scope.tt[i].CHECKITEM) {
                                                $scope.careRecord.Content += "检查项目：" + $scope.produceitem[j].ITEMNAME + "，";
                                                if (angular.isDefined($scope.tt[i].CHECKRESULTS)) {
                                                    $scope.careRecord.Content += "检查结果：" + $scope.tt[i].CHECKRESULTS + "。";
                                                }
                                                else {
                                                    $scope.careRecord.Content += "检查结果：" + "  ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        nursingRecordRes.save($scope.careRecord, function () {
                            utility.message("已同步写入护理记录成功，如需完善，请到护理记录及交班模块操作");
                        });
                    }
                    $scope.day = "";
                    $scope.CheckRecdtl = {};
                    // 这边是删除的东西

                    //这边的id是取到的
                    //items.RECORDID = bj.ResultCode;
                    utility.message("保存成功");
                    $scope.search();
                    //这边方法是多余的。
                    $scope.Data = {};
                    $scope.tt = [];
                    $scope.record = {};
                }
                else {
                    utility.message("保存失败");
                }
            });
        }

        //存储的按钮
        $scope.insertinto = function () {
            if ($scope.filter.REGNO > 0) {

                if (angular.isDefined($scope.record.RECORDID)) {
                    $scope.updateItem();
                } else {
                    $scope.createItem();
                }
            }
            else {
                utility.message("请选择住民信息");
            }

        };

        $scope.SaveAll = function (currentItem) {
            $scope.tbEdit = "ui-inline-hide";
            $scope.tbCancle = "ui-inline-show";
            $scope.classEdit = "ui-inline-hide";
            $scope.classEditAll = "ui-inline-show";
            $scope.btn_s = true;

        }
        $scope.SaveItem = function (currentItem) {

            var items = {
                CHECKTYPE: currentItem.GROUPCODE, CHECKITEM: currentItem.ITEMCODE, CHECKRESULTS: currentItem.CHECKRESULTS, DESCRIPTION: currentItem.DESCRIPTION,
                RECORDID: $scope.Checkid, LOWBOUND: currentItem.LOWBOUND, UPBOUND: currentItem.UPBOUND, NORMALVALUE: currentItem.LOWBOUND + "~" + currentItem.UPBOUND

            };

            if (items.RECORDID > 0) {

                $scope.tt.push(items);
            }
            else {
                var itemlist = {
                    CHECKTYPE: currentItem.GROUPCODE, CHECKITEM: currentItem.ITEMCODE, CHECKRESULTS: currentItem.CHECKRESULTS, DESCRIPTION: currentItem.DESCRIPTION,
                    RECORDID: $scope.Checkid, LOWBOUND: currentItem.LOWBOUND, UPBOUND: currentItem.UPBOUND, NORMALVALUE: currentItem.LOWBOUND + "~" + currentItem.UPBOUND
                };

                $scope.tt.push(itemlist);

            }

            $scope.CheckRecdtl = null;

        }
        // 这边复制拷贝的密码
        $scope.recordModify = function (item) {
            //日期差 取时间出来

            var now = new Date(item.CHECKDATE);

            var date = new Date(item.NEXTCHECKDATE);

            var diff = date.valueOf() - now.valueOf();

            var cday = parseInt(diff / (1000 * 60 * 60 * 24));

            $scope.day = parseInt(cday.toString());

            $scope.record = angular.copy(item);

            $scope.tt = angular.copy(item.CheckRecdtl);

            $scope.Checkid = item.RECORDID;
        }
        //这边是删除的操作
        $scope.delete = function (id) {
            if (confirm("确定删除该信息吗?")) {
                biochemistryRes.delete({ id: id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    utility.message("删除成功");
                    $scope.tt = null;
                    $scope.record = {};
                    $scope.day = null;

                    $scope.search();

                });
            }
        };
        $scope.EditAll = function () {
            $scope.btn_s = false;
            $scope.tbEdit = "ui-inline-show";
            $scope.tbCancle = "ui-inline-hide";
            $scope.classEdit = "ui-inline-show";
            $scope.classEditAll = "ui-inline-hide";
        }
        $scope.CancleEdit = function () {
            $scope.btn_s = true;
            $scope.tbEdit = "ui-inline-hide";
            $scope.tbCancle = "ui-inline-show";
            $scope.classEdit = "ui-inline-hide";
            $scope.classEditAll = "ui-inline-show";
        }
        //这边是获取
        $scope.residentSelected = function (resident) {
            showcodes();
            //设置select的默认值

            $scope.day = null;
            $scope.record = {};
            $scope.tt = [];

            $scope.btn_s = true;
            //这边获取几个值，
            $scope.filter.REGNO = resident.RegNo;

            //这边添加新的东西
            $scope.filter.FEENO = resident.FeeNo;

            $scope.curUser = utility.getUserInfo();

            $scope.record.RECORDBY = $scope.curUser.EmpNo;
            $scope.record.CHECKDATE = getNowFormatDate();

            $scope.code = "";

            $scope.search();
        };
        //这边是起始的方法
        $scope.init();
    }
    ])


