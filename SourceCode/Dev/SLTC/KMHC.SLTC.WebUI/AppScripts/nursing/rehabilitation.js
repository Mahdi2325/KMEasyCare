/*****************************************************************************
 * Filename: rehabilitation
 * Creator:	 yaobinghui
 * Create Date: 2016-03-01 13:34:19
 * Modifier:
 * Modify Date:
 * Description:复健
 ******************************************************************************/
angular.module("sltcApp").controller("rehabilitationCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'rehabilitationRes', 'utility',
    function ($scope, $http, $location, $state, dictionary, rehabilitationRes, utility) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.recDate = "";//Add by Duke on 20160809
        $scope.nextRecDate = "";//Add by Duke on 20160809
        //首次加载页面执行的方法
        $scope.init = function () {

            getname();
            $scope.Data = {};
            $scope.filter = { REGNO: null };
            $scope.CurrentPage = 1;
            //加载中的事件
            //$scope.search();
            $scope.btn_cc = false;

            $scope.Data.Gap = [{ value: "0", name: "0" }, { value: "1", name: "1" }, { value: "30", name: "30" }, { value: "31", name: "31" }, { value: "60", name: "60" }, { value: "90", name: "90" }];

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: rehabilitationRes,//异步请求的res
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
        //
        $scope.search = function (FEENO) {
            $scope.options.params.FEENO = $scope.filter.FEENO;
            $scope.options.search();
        };

        function getname() {
            rehabilitationRes.get(function (obj) {
                $scope.names = obj.Data;
            });
        }

        //检查日期是否在下次日期之前 Add by Duke on 20160809
        $scope.checkRecDate = function () {
            if (angular.isDefined($scope.record.RECDATE) && angular.isDefined($scope.record.NEXTRECDATE)) {
                if (!checkDate($scope.record.RECDATE, $scope.record.NEXTRECDATE)) {
                    utility.msgwarning("日期应在下次日期之前");
                    $scope.record.RECDATE = $scope.recDate;
                }
                else {
                    $scope.recDate = $scope.record.RECDATE;
                    $scope.record.INTERVALDAY = DateDiff($scope.record.NEXTRECDATE, $scope.record.RECDATE);//计算出间隔天数
                }
            }
            else if(angular.isDefined($scope.record.RECDATE) && angular.isDefined($scope.record.INTERVALDAY))
            {
                if($scope.record.INTERVALDAY!="")
                {
                    $scope.record.NEXTRECDATE = addByTransDate($scope.record.RECDATE, parseInt($scope.record.INTERVALDAY));
                }
            }

        }

        //检查下次日期是否在日期之后 Add by Duke on 20160809
        $scope.checkNextRecDate = function () {
            if (angular.isDefined($scope.record.RECDATE) && angular.isDefined($scope.record.NEXTRECDATE)) {
                if (!checkDate($scope.record.RECDATE, $scope.record.NEXTRECDATE)) {
                    utility.msgwarning("下次日期应在日期之后");
                    $scope.record.NEXTRECDATE = $scope.nextRecDate;
                }
                else {
                    $scope.nextRecDate = $scope.record.NEXTRECDATE;
                    $scope.record.INTERVALDAY = DateDiff($scope.record.NEXTRECDATE, $scope.record.RECDATE);//计算出间隔天数
                }
            }
            else if (angular.isDefined($scope.record.NEXTRECDATE) && angular.isDefined($scope.record.INTERVALDAY)) {
                if ($scope.record.INTERVALDAY != "") {
                    $scope.record.RECDATE = reduceByTransDate($scope.record.NEXTRECDATE, parseInt($scope.record.INTERVALDAY));
                }
            }

        }


        // 这边复制拷贝的密码
        $scope.recordModify = function (item) {


            var now = new Date(item.RECDATE);

            var date = new Date(item.NEXTRECDATE);

            var diff = date.valueOf() - now.valueOf();

            var cday = parseInt(diff / (1000 * 60 * 60 * 24));




            $scope.record = angular.copy(item);
            $scope.record.INTERVALDAY = cday.toString();

        }

        //这边是获取
        $scope.residentSelected = function (resident) {

            $scope.record = {};
            $scope.btn_cc = true;


            //这边获取几个值，
            $scope.filter.REGNO = resident.RegNo;
            //这边添加新的东西
            $scope.filter.FEENO = resident.FeeNo;
            $scope.curUser = utility.getUserInfo();
            $scope.record.RECORDBY = $scope.curUser.EmpNo;

            //$scope.record.RecordBy = staff.EmpNO;
            $scope.search();


        }
        //复健的检查
        $scope.$watch('record.INTERVALDAY', function (newValue,oldValue) {
            if (newValue) {
                if (!isNaN(newValue)) {
                    if (newValue > 0) {
                        if (angular.isDefined($scope.record.RECDATE)) {
                            $scope.record.NEXTRECDATE = addByTransDate($scope.record.RECDATE, parseInt(newValue));
                        }
                    }
                }
                else {
                    utility.msgwarning("间隔天数输入格式不正确");
                    $scope.record.INTERVALDAY = oldValue;
                }
            }
        });


        //$scope.$watch('record.RECDATE', function (newValue) {

        //    if (angular.isDefined(newValue)) {
        //        $scope.day_show = false;
        //    }
        //    else {
        //        $scope.day = "";
        //        $scope.day_show = true;

        //    }
        //});

        function addByTransDate(dateParameter, num) {

            var translateDate = "", dateString = "", monthString = "", dayString = "";

            //translateDate = dateParameter.replace("-", "/").replace("-", "/");

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

        function reduceByTransDate(dateParameter, num) {
            var translateDate = "", dateString = "", monthString = "", dayString = "";
            translateDate = dateParameter.replace("-", "/").replace("-", "/");
            var newDate = new Date(translateDate);
            newDate = newDate.valueOf();
            newDate = newDate - num * 24 * 60 * 60 * 1000;
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

        //这边是更新的一个方法
        $scope.insertinto = function () {



            if ($scope.filter.REGNO != null) {

                //这边设置一个数组,这边添加的一个id，这样就可以达到编辑的效果了,加入新的东西
                var para = { RECORDBY: $scope.record.RECORDBY, RECDATE: $scope.record.RECDATE, INTERVALDAY: $scope.day, NEXTRECDATE: $scope.record.NEXTRECDATE, NEXTRECORDBY: $scope.record.NEXTRECORDBY, HOSPNAME: $scope.record.HOSPNAME, ITEMNAME: $scope.record.ITEMNAME, ASSESSMENT: $scope.record.ASSESSMENT, DESCRIPTION: $scope.record.DESCRIPTION, ID: $scope.record.ID, REGNO: $scope.filter.REGNO, FEENO: $scope.filter.FEENO };

                rehabilitationRes.save(para, function (obj) {

                    utility.message("操作成功");
                    //这边方法是多余的。
                    $scope.record = {};
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.search();
                });
            }
            else {

                alert("请选择人员后，再添加康健信息");

            }
        };

        //这边是删除的
        $scope.delete = function (id) {
            if (confirm("确定删除该信息吗?")) {
                rehabilitationRes.delete({ id: id }, function (data) {
                    utility.message("删除成功");
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.search();
                    $scope.CurrentPage = 1;
                    $scope.record = {}
                });
            }
        };



        $scope.staffSelected = function (item, flag) {

            switch (flag) {
                case "RECORDBY": {
                    $scope.record.RECORDBY = item.EmpNo;
                    break;
                }
                case "NEXTRECORDBY": {
                    $scope.record.NEXTRECORDBY = item.EmpNo;
                    break;
                }

            }

        }

        $scope.init();

    }
]);






