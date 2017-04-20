///创建人:Amaya
///创建日期:2017-02-08
///说明:健康档案
angular.module("sltcApp")
.controller("healthRecordsCtrl", ['$rootScope', '$scope', '$filter', '$state', 'utility', 'HealthRecordsRes', 'unPlanWeightRes', 'vitalSignRes', 'MedicationRecordRes', 'biochemistryRes', 'biochemistrylist', 'biochemistry', 'evaluationHisRes',
    function ($rootScope, $scope, $filter, $state, utility, HealthRecordsRes, unPlanWeightRes, vitalSignRes, MedicationRecordRes, biochemistryRes, biochemistrylist, biochemistry, evaluationHisRes) {
        $scope.FeeNo = $state.params.FeeNo;
        var d = new Date(), nowDate = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss")
        $scope.RegNo = -1;
        $scope.type = 1;
        $scope.divshow = false;
        $scope.Data = {};
        $scope.data = [];
        $scope.data1 = [];
        $scope.data2 = [];
        $scope.data3 = [];
        $scope.HealthRecordList = {};
        $scope.UnPlans = {};
        $scope.curUser = utility.getUserInfo();

        $scope.btshow0 = true;
        $scope.btshow1 = true;
        $scope.btshow2 = true;
        $scope.btshow3 = true;
        $scope.btshow4 = true;
        $scope.btshow5 = true;
        $scope.btshow6 = true;
        $scope.btshow7 = false;
        $scope.nullValue = "未知";
        $scope.Description = "";
        $scope.codeName = "";
        $scope.unit = "";
        $scope.BSType = "001";
        $scope.MeaSuredRecord = {};
        $scope.tt = [];
        $scope.QuestionList = {};
        $scope.MedReccordList = {};

        //开始时间和结束时间
        var preMonthDate = new Date();
        preMonthDate.setDate(1);
        preMonthDate.setMonth(preMonthDate.getMonth());
        var preYear = preMonthDate.getFullYear();
        var preMonth = preMonthDate.getMonth();
        $scope.StartDate = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
        $scope.EndDate = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");
        $scope.recStartDate = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
        $scope.recEndDate = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");
        //获得某月的天数
        function getMonthDays(nowYear, myMonth) {
            var monthStartDate = new Date(nowYear, myMonth, 1);
            var monthEndDate = new Date(nowYear, myMonth + 1, 1);
            var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
            return days;
        }

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.currentResident = resident;//获取当前住民信息
            $scope.FeeNo = $scope.currentResident.FeeNo;
            $scope.RegNo = $scope.currentResident.RegNo;
            $scope.show(1);
            $scope.show(2);
            $scope.show(3);
            $scope.show(4);
            $scope.show(5);
            $scope.divshow = true;
        }

        $scope.init = function () {
            if ($scope.FeeNo == "" || $scope.FeeNo == undefined) {
                $scope.divshow = false;
            }
            else {
                $scope.divShow = true;
                $scope.show($scope.type);
            }

            biochemistry.get(function (obj) {
                $scope.produceitem = obj.Data;
            });

            showcodes();

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: HealthRecordsRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.HealthRecordList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                    genre: "-1",
                }
            }

            var myDate = new Date().format("yyyy-MM-dd");
            $scope.medoptions = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: MedicationRecordRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.MedicationRecordList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                    StartDate: $scope.StartDate,
                    EndDate: $scope.EndDate
                }
            }

            $scope.medrecoptions = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: HealthRecordsRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.MedReccordList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                    recStartDate: $scope.recStartDate,
                    recEndDate: $scope.recEndDate
                }
            }
           
            $scope.unoptions = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: unPlanWeightRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.UnPlans = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    regNo: 0,
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }

            $scope.bioptions = {
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

            $scope.evoptions = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: evaluationHisRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.QuestionList = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    QuestionId: -1,
                    FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
        };

        $scope.show = function (type) {
            $scope.type = type;
            $scope.bodyTemp = {};
            $scope.pulse = {};
            $scope.breathe = {};
            $scope.sbp = {};
            $scope.dbp = {};
            $scope.weight = {};
            $scope.bloodSugar = {};

            HealthRecordsRes.get({ feeNo: $scope.FeeNo, type: type }, function (data) {
                if (type == 1) {
                    $scope.data = data;
                    $scope.bodyTemp = $scope.data.MeasurementList[0];
                    $scope.nullModelInfo($scope.bodyTemp, 0);

                    $scope.pulse = $scope.data.MeasurementList[1];
                    $scope.nullModelInfo($scope.pulse, 1);

                    $scope.breathe = $scope.data.MeasurementList[2];
                    $scope.nullModelInfo($scope.breathe, 2);

                    $scope.sbp = $scope.data.MeasurementList[3];
                    $scope.nullModelInfo($scope.sbp, 3);
                    $scope.dbp = $scope.data.MeasurementList[4];
                    $scope.nullModelInfo($scope.dbp, 4);

                    $scope.weight = $scope.data.MeasurementList[5];
                    $scope.nullModelInfo($scope.weight, 5);

                    $scope.bloodSugar = $scope.data.MeasurementList[6];
                    $scope.nullModelInfo($scope.bloodSugar, 6);
                }

                if (type == 2) {
                    $scope.data1 = data;
                    $scope.medsearch();
                }

                if (type == 3) {
                    $scope.data2 = data;
                    $scope.bisearch();
                }

                if (type == 4) {
                    $scope.data3 = data;
                    $scope.data3.ADLEvaluation = data.ADLEvaluation;
                    $scope.data3.MMSEEvaluation = data.MMSEEvaluation;
                    $scope.data3.IADLEvaluation = data.IADLEvaluation;
                    $scope.data3.SoreEvaluation = data.SoreEvaluation;
                    $scope.data3.FallEvaluation = data.FallEvaluation;
                }

                if (type == 5)
                {
                    $scope.data5 = data;
                    $scope.medrecsearch();
               
                }
            });
        }
        $scope.lookEvalDetail = function (qid) {
            $scope.QuestionList = {};
            $scope.evoptions.pageInfo.CurrentPage = 1;
            $scope.evoptions.params.FeeNo = $scope.FeeNo;
            $scope.evoptions.params.QuestionId = qid;
            $scope.evoptions.search();
            $("#EvModalDetail").modal("toggle");
        }

        $scope.bisearch = function () {
            $scope.bioptions.pageInfo.CurrentPage = 1;
            $scope.bioptions.params.FEENO = $scope.FeeNo;
            $scope.bioptions.search();
        }

        $scope.medsearch = function () {
            $scope.medoptions.pageInfo.CurrentPage = 1;
            $scope.medoptions.params.feeNo = $scope.FeeNo;
            $scope.medoptions.params.StartDate = $scope.StartDate;
            $scope.medoptions.params.EndDate = $scope.EndDate;
            $scope.medoptions.search();
        }

        $scope.medrecsearch = function () {
            $scope.medrecoptions.pageInfo.CurrentPage = 1;
            $scope.medrecoptions.params.feeNo = $scope.FeeNo;
            $scope.medrecoptions.params.recStartDate = $scope.recStartDate;
            $scope.medrecoptions.params.recEndDate = $scope.recEndDate;
            $scope.medrecoptions.search();
        }

        $scope.nullModelInfo = function (bt, type) {
            if (type == 0) {
                if (bt != null) {
                    $scope.btshow0 = true;
                }
                else {
                    $scope.btshow0 = false;
                }
            }
            if (type == 1) {
                if (bt != null) {
                    $scope.btshow1 = true;
                }
                else {
                    $scope.btshow1 = false;
                }
            }
            if (type == 2) {
                if (bt != null) {
                    $scope.btshow2 = true;
                }
                else {
                    $scope.btshow2 = false;
                }
            }
            if (type == 3) {
                if (bt != null) {
                    $scope.btshow3 = true;
                    $scope.btshow7 = true;
                    $scope.Description = bt.Description;
                }
                else {
                    $scope.btshow3 = false;
                }
            }
            if (type == 4) {
                if (bt != null) {
                    $scope.btshow4 = true;
                    $scope.btshow7 = true;
                    if ($scope.Description != "超出正常值范围") {
                        $scope.Description = bt.Description;
                    }
                }
                else {
                    $scope.btshow4 = false;
                }
            }
            if (type == 5) {
                if (bt != null) {
                    $scope.btshow5 = true;
                }
                else {
                    $scope.btshow5 = false;
                }
            }
            if (type == 6) {
                if (bt != null) {
                    $scope.btshow6 = true;
                }
                else {
                    $scope.btshow6 = false;
                }
            }

        }

        $scope.lookDetail = function (genre, codename, unit) {
            $scope.HealthRecordList = {};
            $("#lookDetail").modal("toggle");
            $scope.codeName = codename;
            $scope.unit = unit;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = $scope.FeeNo;
            $scope.options.params.genre = genre;
            $scope.options.search();
        };
        $scope.lookunDetail = function (codename) {
            $scope.UnPlans = {};
            $("#lookunDetail").modal("toggle");
            $scope.codeName = codename;
            $scope.unoptions.params.regNo = $scope.RegNo;
            $scope.unoptions.pageInfo.CurrentPage = 1;
            $scope.unoptions.params.feeNo = $scope.FeeNo;

            $scope.unoptions.search();

        }
        //体温  呼吸  脉搏
        $scope.insertRecord = function (genre, codename, unit) {
            $scope.MeasureItem = {};
            $scope.MeaSuredRecord = {};
            $scope.MeaSuredRecord.MeaSureTime = nowDate;
            $scope.MeaSuredRecord.MeasureItemCode = genre;
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.MeaSuredRecord.MeaSuredPerson = $scope.curUser.EmpNo;
            }

            $("#insertRecord").modal("toggle");
            $scope.codeName = codename;
            vitalSignRes.get({ itemCode: genre }, function (data) {
                $scope.MeasureItem = data;
            });
        }
        $scope.checkValue = function () {
            if ($scope.MeaSuredRecord.MeaSuredValue != null) {
                if ($scope.MeaSuredRecord.MeaSuredValue >= $scope.MeasureItem.Lower && $scope.MeaSuredRecord.MeaSuredValue <= $scope.MeasureItem.Upper) {
                    $scope.MeaSuredRecord.Description = "正常";
                }
                else {
                    $scope.MeaSuredRecord.Description = "超出正常值范围";
                }
            }
            else {
                utility.message("请填写量测数据！");
            }
        }
        $scope.save = function () {
            $scope.checkValue();
            if (angular.isDefined($scope.formRecord.$error.required)) {
                for (var i = 0; i < $scope.formRecord.$error.required.length; i++) {
                    utility.msgwarning($scope.formRecord.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.formRecord.$error.maxlength)) {
                for (var i = 0; i < $scope.formRecord.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.formRecord.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            $scope.MeaSuredRecord.FeeNo = $scope.FeeNo;
            $scope.MeaSuredRecord.Source = "PC";
            HealthRecordsRes.save($scope.MeaSuredRecord, function () {
                utility.message($scope.codeName + "量测数据保存成功！");
                $scope.show(1);
                $("#insertRecord").modal("hide");
            })
        }
        //血压
        $scope.insertRecord1 = function (genre, codename, unit) {
            $scope.MeasureItem1 = {};
            $scope.MeasureItem2 = {};
            $scope.MeaSuredRecord = {};
            $scope.MeaSuredRecord.MeaSureTime = nowDate;
            $scope.MeaSuredRecord.MeasureItemCode = genre;
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.MeaSuredRecord.MeaSuredPerson = $scope.curUser.EmpNo;
            }

            $("#insertxueyaRecord").modal("toggle");
            $scope.codeName = codename;

            vitalSignRes.get({ itemCode: "004" }, function (data) {
                $scope.MeasureItem1 = data;
            });

            vitalSignRes.get({ itemCode: "005" }, function (data) {
                $scope.MeasureItem2 = data;
            });
        }
        $scope.checkValue1 = function () {
            if ($scope.MeaSuredRecord.MeaSuredValue1 != null) {
                if ($scope.MeaSuredRecord.MeaSuredValue1 >= $scope.MeasureItem1.Lower && $scope.MeaSuredRecord.MeaSuredValue1 <= $scope.MeasureItem1.Upper) {
                    $scope.MeaSuredRecord.Description1 = "正常";
                }
                else {
                    $scope.MeaSuredRecord.Description1 = "超出正常值范围";
                }
            }
            else {
                utility.message("请填写量测数据！");
            }
        }
        $scope.checkValue2 = function () {
            if ($scope.MeaSuredRecord.MeaSuredValue2 != null) {
                if ($scope.MeaSuredRecord.MeaSuredValue2 >= $scope.MeasureItem2.Lower && $scope.MeaSuredRecord.MeaSuredValue2 <= $scope.MeasureItem2.Upper) {
                    $scope.MeaSuredRecord.Description2 = "正常";
                }
                else {
                    $scope.MeaSuredRecord.Description2 = "超出正常值范围";
                }
            }
            else {
                utility.message("请填写量测数据！");
            }
        }
        $scope.savexueya = function () {
            $scope.checkValue1();
            $scope.checkValue2();
            if (angular.isDefined($scope.formxyRecord.$error.required)) {
                for (var i = 0; i < $scope.formxyRecord.$error.required.length; i++) {
                    utility.msgwarning($scope.formxyRecord.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.formxyRecord.$error.maxlength)) {
                for (var i = 0; i < $scope.formxyRecord.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.formxyRecord.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            $scope.MeaSuredRecord.FeeNo = $scope.FeeNo;
            $scope.MeaSuredRecord.Source = "PC";
            $scope.MeaSuredRecord.MeasureItemCode = "004";
            $scope.MeaSuredRecord.MeaSuredValue = $scope.MeaSuredRecord.MeaSuredValue1;
            $scope.MeaSuredRecord.Description = $scope.MeaSuredRecord.Description1;
            HealthRecordsRes.save($scope.MeaSuredRecord, function () {
                $scope.MeaSuredRecord.MeasureItemCode = "005";
                $scope.MeaSuredRecord.MeaSuredValue = $scope.MeaSuredRecord.MeaSuredValue2;
                $scope.MeaSuredRecord.Description = $scope.MeaSuredRecord.Description2;
                HealthRecordsRes.save($scope.MeaSuredRecord, function () {
                    utility.message($scope.codeName + "量测数据保存成功！");
                    $scope.show(1);
                    $("#insertxueyaRecord").modal("hide");
                })
            })
        }
        //血糖
        $scope.insetxt = function (codename) {
            $scope.MeaSuredRecord = {};
            $scope.MeasureItem = {};
            $scope.MeaSuredRecord.MeaSureTime = nowDate;
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.MeaSuredRecord.MeaSuredPerson = $scope.curUser.EmpNo;
            }

            var itemCode = "S006" + $scope.BSType;
            vitalSignRes.get({ itemCode: itemCode }, function (data) {
                $scope.MeasureItem = data;
            });
            $("#insertxuetangRecord").modal("toggle");
            $scope.codeName = codename;
        }
        $scope.checkValue3 = function () {
            if ($scope.MeaSuredRecord.MeaSuredValue != null) {
                if ($scope.MeaSuredRecord.MeaSuredValue >= $scope.MeasureItem.Lower && $scope.MeaSuredRecord.MeaSuredValue <= $scope.MeasureItem.Upper) {
                    $scope.MeaSuredRecord.Description = "正常";
                }
                else {
                    $scope.MeaSuredRecord.Description = "超出正常值范围";
                }
            }
            else {
                utility.message("请填写量测数据！");
            }
        }
        $scope.changebs = function () {
            $scope.MeasureItem = {};
            if ($scope.BSType == "") {
                $scope.BSType = "009";
            }
            var itemCode = "S006" + $scope.BSType;
            vitalSignRes.get({ itemCode: itemCode }, function (data) {
                $scope.MeasureItem = data;
            });

            if ($scope.MeaSuredRecord.MeaSuredValue != null) {
                if ($scope.MeaSuredRecord.MeaSuredValue >= $scope.MeasureItem.Lower && $scope.MeaSuredRecord.MeaSuredValue <= $scope.MeasureItem.Upper) {
                    $scope.MeaSuredRecord.Description = "正常";
                }
                else {
                    $scope.MeaSuredRecord.Description = "超出正常值范围";
                }
            }
            else {
                utility.message("请填写量测数据！");
            }
        }
        $scope.savexuetang = function () {
            $scope.checkValue3();
            if (angular.isDefined($scope.formxuetangRecord.$error.required)) {
                for (var i = 0; i < $scope.formxuetangRecord.$error.required.length; i++) {
                    utility.msgwarning($scope.formxuetangRecord.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.formxuetangRecord.$error.maxlength)) {
                for (var i = 0; i < $scope.formxuetangRecord.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.formxuetangRecord.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            $scope.MeaSuredRecord.FeeNo = $scope.FeeNo;
            $scope.MeaSuredRecord.Source = "PC";
            $scope.MeaSuredRecord.MeasureItemCode = "S006" + $scope.BSType;
            HealthRecordsRes.save($scope.MeaSuredRecord, function () {
                utility.message($scope.codeName + "量测数据保存成功！");
                $scope.show(1);
                $("#insertxuetangRecord").modal("hide");
            })
        }

        //体重
        $scope.insertweightRecord = function (codename) {
            $scope.currentItem = {};
            $scope.currentItem.ThisRecDate = nowDate;
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem.RecordBy = $scope.curUser.EmpNo;
            }
            $("#insertweightRecord").modal("toggle");
            $scope.codeName = codename;
        }
        $scope.getBMI = function () {
            $scope.currentItem.BMI = utility.BMI($scope.currentItem.ThisWeight, $scope.currentItem.ThisHeight);
            $scope.currentItem.BMIResults = utility.BMIResult($scope.currentItem.BMI);
        }
        $scope.staffSelected = function (item, type) {
            if (type == 0) {
                $scope.currentItem.RecordBy = item.EmpNo;
                $scope.currentItem.RecordNameBy = item.EmpName;
            }
            else if (type == 1) {
                $scope.MeaSuredRecord.MeaSuredPerson = item.EmpNo;
            }
        }
        $scope.saveweight = function () {
            if (angular.isDefined($scope.formweightRecord.$error.required)) {
                for (var i = 0; i < $scope.formweightRecord.$error.required.length; i++) {
                    utility.msgwarning($scope.formweightRecord.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.formweightRecord.$error.maxlength)) {
                for (var i = 0; i < $scope.formweightRecord.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.formweightRecord.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            $scope.currentItem.RegNo = $scope.currentResident.RegNo;
            $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
            unPlanWeightRes.save($scope.currentItem, function (data) {
                utility.message($scope.codeName + "量测数据保存成功！");
                $scope.show(1);
                $("#insertweightRecord").modal("hide");
            });
        }

        $scope.lookBiochemistryList = function (CheckRecdtl) {
            $scope.tt = angular.copy(CheckRecdtl);
            $("#CheckRecdtlRecord").modal("toggle");
        }

        //生成项目组 
        function showcodes(code) {
            biochemistrylist.get(function (obj) {
                $scope.CheckCode = obj.Data;
            });
        }

        $scope.init();
    }]);
