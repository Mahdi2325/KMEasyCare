/*
 * Author : Dennis yang(杨金高)
 * Date   : 2016-03-04
 * Desc   : VisitDocRecords(就医管理)
 */

/*
修改人:张祥
修改日期:2016-04-06
说明: 就医用药
*/

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
angular.module("sltcApp").controller("visitDocRecordCtrl", ['$scope', '$compile', '$http', '$filter', '$state', '$location', 'webUploader', 'dictionary', 'visitHospitalRes', 'visitDeptRes', 'visitDoctorRes', 'icd9Res', 'visitdocrecordsRes', 'visitprescriptionsRes', 'medicineRes', 'utility', 'nursingRecordRes', 'costEntryRes',
    function ($scope, $compile, $http, $filter, $state, $location, webUploader, dictionary, visitHospitalRes, visitDeptRes, visitDoctorRes, icd9Res, visitdocrecordsRes, visitprescriptionsRes, medicineRes, utility, nursingRecordRes, costEntryRes) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.Data = {};
        //初始化药品信息
        $scope.pillItem = {};
        //初始化处方信息
        $scope.PrescItem = {};
        //初始化就医信息
        $scope.currentItem = {};
        $scope.Data.visitDocRecords = {}
        $scope.Data.medicines = {};
        $scope.displayMode = "list";
        $scope.buttonShow = false;
        $scope.Info = {};
        var tempVisitDoctorList = [];
        $scope.isAdd = true;
        //根据所选院民获取其相关就医/用药记录信息
        $scope.init = function () {
            $scope.SyncNurRec = true;
            $scope.curUser = utility.getUserInfo();
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: visitdocrecordsRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.visitDocRecords = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
                }
            }
            $scope.options2 = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: costEntryRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.medicines = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    keyWord: ""
                }
            }

            visitHospitalRes.get({}, function (data) {
                $scope.Info.VisitHosp = data.Data;
                $scope.Info.VisitDept = [];
                $scope.Info.VisitDoctor = [];
            });

            var ss = $(".ContNamediv").width();
            $(".selContName").css('width', ss);
            $(".spanwidth").css("margin-left", ss - 20);
            $(".selContName").css('margin-left', -(ss - 20));
            $(".inputwidth").css('width', ss - 20);

            var ss = $("#ContNamediv2").width();
            $("#selContName2").css('width', ss);
            $("#spanwidth2").css("margin-left", ss - 20);
            $("#selContName2").css('margin-left', -(ss - 20));
            $("#inputwidth2").css('width', ss - 20);
            var key = "K00.008";
            var ItemTypes = [];
            ItemTypes.push(key);
            $http.post('/api/Code', { ItemTypes: ItemTypes }).success(function (data) {
                for (var name in data.Data) {
                    if (data.Data.hasOwnProperty(name) && name == key) {
                        $scope.VisitReasonList = data.Data[name];
                        break;
                    }
                }
            });
        }

        $scope.changeHospital = function (value) {
            $scope.Info.VisitHosp.forEach(function (item) {
                if (item.HospName == value) {
                    if (item.VisitDept.length == 0) {
                        $scope.Info.VisitDept = [];
                    }
                    else {
                        $scope.Info.VisitDept = item.VisitDept;
                    }

                    if (item.VisitDoctor.length == 0) {
                        $scope.Info.VisitDoctor = [];
                    }
                    else {
                        $scope.Info.VisitDoctor = item.VisitDoctor;
                        tempVisitDoctorList = item.VisitDoctor;
                    }
                    $scope.currentItem.VisitHospName = item.HospName;
                    $scope.changeDept($scope.currentItem.VisitDept);
                }
            });
        }

        $scope.changeDept = function (value) {
            var deptNo = "";
            var tempVisitDoctor = [];
            $scope.Info.VisitDept.forEach(function (item) {
                if (item.DeptName == value) {
                    $scope.currentItem.VisitDept = item.DeptName;
                    deptNo = item.DeptNo;
                };
            });

            tempVisitDoctorList.forEach(function (item) {
                if (item.DeptNo == deptNo) {
                    tempVisitDoctor.push(item);
                }
            });

            $scope.Info.VisitDoctor = tempVisitDoctor;
        }

        $scope.changeDoctor = function (value) {

            $scope.Info.VisitDoctor.forEach(function (item) {
                if ($scope.Info.VisitDoctor.DocId == value) {
                    $scope.currentItem.VisitDoctorName = item.DocName;
                };
            });
        }

        $scope.getDisease = function (value) {

        };
        $scope.AddVisitDocRecord = function () {
            $scope.displayMode = "edit";
            //加载处方记录数据
            visitprescriptionsRes.get({ feeNo: resident.FeeNo }, function (data) {
                $scope.Data.visitpresList = data.Data;
            });

        }

        $scope.staffSelected = function (item, type) {
            if (type === "RecordBy") {
                $scope.currentItem.RecordBy = item.EmpNo;
                $scope.currentItem.RecordNameBy = item.EmpName;
            } else if (type === "VisitDoctor") {
                $scope.currentItem.VisitDoctor = item.EmpNo;
                $scope.currentItem.VisitDoctorName = item.EmpName;
            }
        }

        $scope.ChangeNextvisitdate = function () {
            if ($scope.currentItem.VisitDate != "" && $scope.currentItem.Nextvisitdate != "") {
                var days = DateDiff($scope.currentItem.Nextvisitdate, $scope.currentItem.VisitDate)
                if (days < 0) {
                    utility.message("下次就医日期不能小于就医日期");
                    $scope.currentItem.Nextvisitdate = "";
                    $scope.currentItem.Intervalday = '';
                } else {
                    $scope.currentItem.Intervalday = days;
                }
            };
        }


        $scope.ICD9Selected = function (item) {
            if (angular.isDefined($scope.currentItem.DiseaseName)) {
                if ($scope.currentItem.DiseaseName == "" || $scope.currentItem.DiseaseName == null) {
                    $scope.currentItem.DiseaseName = item.EngName;
                }
                else {
                    $scope.currentItem.DiseaseName = $scope.currentItem.DiseaseName + "," + item.EngName;
                }

            }
            else {
                $scope.currentItem.DiseaseName = item.EngName;
            }
            if (angular.isDefined($scope.currentItem.DiseaseType)) {
                if ($scope.currentItem.DiseaseType == "" || $scope.currentItem.DiseaseType == null) {
                    $scope.currentItem.DiseaseType = item.IcdCode;
                }
                else {
                    $scope.currentItem.DiseaseType = $scope.currentItem.DiseaseType + "," + item.IcdCode;
                }

            }
            else {
                $scope.currentItem.DiseaseType = item.IcdCode;

            }
        }

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.currentResident = resident;
            //加载就医记录数据
            $scope.options.params.feeNo = resident.FeeNo;
            $scope.options.search();
            if (angular.isDefined($scope.currentResident.FeeNo)) {
                $scope.buttonShow = true;
            }
        };

        //删除一条记录
        $scope.deleteItem = function (item) {
            if (confirm('您确定要删除该条记录吗?')) {
                visitdocrecordsRes.delete({ id: item.SeqNo }, function () {
                    $scope.Data.visitDocRecords.splice($scope.Data.visitDocRecords.indexOf(item), 1);
                });
            }
            $scope.displayMode = "list";
        };

        //打开就医信息界面
        $scope.openWin = function (item) {
            var deptNo = "";
            if (item) {
                $scope.Info.VisitHosp.forEach(function (item2) {
                    if (item2.HospName == item.VisitHosp) {
                        if (item2.VisitDept.length == 0) {
                            $scope.Info.VisitDept = [];
                        }
                        else {
                            $scope.Info.VisitDept = item2.VisitDept;
                        }

                        if (item2.VisitDoctor.length == 0) {
                            $scope.Info.VisitDoctor = [];
                        }
                        else {
                            $scope.Info.VisitDoctor = item2.VisitDoctor;
                            tempVisitDoctorList = item2.VisitDoctor;
                        }
                    }
                });


                $scope.Info.VisitDept.forEach(function (item4) {
                    if (item4.DeptName == item.VisitDept) {
                        deptNo = item4.DeptNo;
                    }
                });


                var tempVisitDoctor = [];

                tempVisitDoctorList.forEach(function (item3) {
                    if (item3.DeptNo == deptNo) {
                        tempVisitDoctor.push(item3);
                    }
                });
                $scope.Info.VisitDoctor = tempVisitDoctor;
                $scope.currentItem = item;
                $scope.add = false;
            }
            else {
                $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
                $scope.currentItem.VisitDate = getNowFormatDate();
                $scope.Info.VisitDoctor = [];
                $scope.Info.VisitDept = [];
                $scope.add = true;
            }
            $scope.displayMode = "edit";
            visitprescriptionsRes.get({ seqNo: angular.isDefined(item) ? item.SeqNo : -1 }, function (data) {
                $scope.Data.visitpresList = data.Data;
            });
        };
        $scope.copyItem = function (item) {
            $scope.currentItem = item;
            $scope.currentItem.RecordBy = $scope.curUser.EmpNo;
            $scope.currentItem.VisitDate = getNowFormatDate();
            $scope.currentItem.TakeDays = "";
            $scope.currentItem.StartDate = "";
            $scope.currentItem.EndDate = "";
            $scope.currentItem.Nextvisitdate = "";
            $scope.currentItem.Intervalday = "";
            $scope.Data.visitpresList = [];
            visitprescriptionsRes.get({ seqNo: angular.isDefined(item) ? item.SeqNo : -1 }, function (data) {
                data.Data.forEach(function (visit) {
                    visit.StartDate = "";
                    visit.EndDate = "";
                    $scope.Data.visitpresList.push(visit);
                })
            });
            $scope.currentItem.SeqNo = "";
        }
        $scope.cancelInfectVisitReason = function (value) {
            var prereginfo = $scope.currentItem.Prereginfo;
            var intervalday = $scope.currentItem.Intervalday;
            var nextvisitdate = $scope.currentItem.Nextvisitdate;
            var nextvisittype = $scope.currentItem.Nextvisittype;
            var infectvisitreason = $scope.currentItem.Infectvisitreason;
            var nextvisithint = $scope.currentItem.Nextvisithint;

            if (value) {
                $scope.currentItem.Prereginfo = "";
                $scope.currentItem.Intervalday = "";
                $scope.currentItem.Nextvisitdate = "";
                $scope.currentItem.Nextvisittype = "";
                $scope.currentItem.Nextvisithint = "";
                $scope.currentItem.Infectvisitreason = infectvisitreason;

            }
            else {
                $scope.currentItem.Prereginfo = prereginfo;
                $scope.currentItem.Intervalday = intervalday;
                $scope.currentItem.Nextvisitdate = nextvisitdate;
                $scope.currentItem.Nextvisittype = nextvisittype;
                $scope.currentItem.Nextvisithint = nextvisithint;
                $scope.currentItem.Infectvisitreason = "";


            }

        };

        //保存就医信息包括处方信息
        $scope.saveVisitDocRec = function (item) {
            if (angular.isDefined($scope.vrForm.$error.required)) {
                for (var i = 0; i < $scope.vrForm.$error.required.length; i++) {
                    utility.msgwarning($scope.vrForm.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.vrForm.$error.maxlength)) {
                for (var i = 0; i < $scope.vrForm.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.vrForm.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            if (angular.isDefined($scope.vrForm.$error.pattern)) {
                for (var i = 0; i < $scope.vrForm.$error.pattern.length; i++) {
                    utility.msgwarning($scope.vrForm.$error.pattern[i].$name + "格式错误！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            item.FeeNo = $scope.currentResident.FeeNo;//住院序号
            item.RegNo = $scope.currentResident.RegNo;//病例号
            item.VisitPrescription = $scope.Data.visitpresList;
            if (angular.isDefined(item.SeqNo)) {
                visitdocrecordsRes.save(item, function (data) {
                    utility.message($scope.currentResident.Name + "就诊信息存档成功！");
                    $scope.options.search();
                });
            } else {
                visitdocrecordsRes.save(item, function (data) {
                    if (data.ResultCode == 0) {
                        if ($scope.SyncNurRec) {
                            $scope.record = {};
                            $scope.record.FeeNo = $scope.currentResident.FeeNo;
                            $scope.record.RegNo = $scope.currentResident.RegNo;
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

                            if (angular.isDefined($scope.currentItem.VisitDate)) {
                                recDate = $scope.currentItem.VisitDate.substring(0, 4) + "年" + $scope.currentItem.VisitDate.substring(5, 7) + "月" + $scope.currentItem.VisitDate.substring(8, 10) + "日";
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

                            $scope.record.Content += "";

                            if (recName != "") {
                                $scope.record.Content += "填写人员：" + recName + ";";
                            }
                            if (recDate != "") {
                                $scope.record.Content += "就医日期：" + recDate + ";";
                            }
                            if (angular.isDefined($scope.currentItem.VisitType)) {
                                $scope.record.Content += "就医类型：" + $scope.currentItem.VisitType + ";";
                            }

                            nursingRecordRes.save($scope.record, function () {
                                utility.message("已同步写入护理记录成功，如需完善，请到护理记录及交班模块操作");
                            });
                        }

                        $scope.options.search();
                        utility.message($scope.currentResident.Name + "就诊信息存档成功！");
                    }
                });
            }
            $scope.displayMode = "list";
        };

        //取消操作
        $scope.cancelVisitDocRec = function () {
            $scope.currentItem = {};
            $scope.displayMode = "list";
            $scope.options.search();
        };


        //弹出添加或编辑药物窗体
        $scope.openWin_vdr = function (item) {
            $scope.PrescItem = {};
            $scope.pillItem = {};
            $scope.PrescItem.StartDate = $scope.currentItem.StartDate;
            $scope.PrescItem.EndDate = $scope.currentItem.EndDate;
            $scope.PrescItem.UseFlag = true;
            $("#visitDocModal").modal("toggle");
            $scope.options2.pageInfo.CurrentPage = 1;
            $scope.options2.pageInfo.PageSize = 10;
            $scope.displayMode = "edit";
            $scope.options2.search();
        };



        //查询药物
        $scope.searchMedicines = function () {
            $scope.options2.pageInfo.CurrentPage = 1;
            $scope.options2.pageInfo.PageSize = 10;
            if (!angular.isDefined($scope.keyword))
            {
                $scope.keyword = "";
            }
            $scope.options2.params.keyWord = $scope.keyword;
            $scope.options2.search();
        };

        $scope.medicinesManagement = function () {
            $('#visitDocModal').on('hidden.bs.modal', function (e) {
                $location.path('/angular/DrugList');
                $scope.$apply();
                $(e.currentTarget).unbind();
            })
            $("#visitDocModal").modal("toggle");
        };

        //$scope.addMedicines = function () {
        //    //$location.path('/angular/DrugList');
        //    //$state.go('DrugList');
        //    //$state.stateName = "DrugList";
        //};

        $scope.GetNextvisitdate = function () {
            if (angular.isDefined($scope.currentItem.VisitDate) && angular.isDefined($scope.currentItem.Intervalday)) {
                $scope.currentItem.Nextvisitdate = addByTransDate($scope.currentItem.VisitDate.replace('T', ' '), $scope.currentItem.Intervalday);
            }
        }
        //
        $scope.getVisitDate = function (val) {
            if (val != "") {
                $scope.currentItem.Nextvisitdate = addByTransDate(val, 1);
            }
        }
        //计算日期
        $scope.calculation = function () {
            var beginDate;
            var EndDate;
            if ($scope.currentItem.StartDate != "" && $scope.currentItem.TakeDays != "") {
                if (typeof ($scope.currentItem.TakeDays) == "undefined") {
                    $scope.currentItem.EndDate = "";
                    $scope.currentItem.Nextvisitdate = "";
                }
                else {
                    beginDate = new Date($scope.currentItem.StartDate);
                    $scope.currentItem.EndDate = addByTransDate($scope.currentItem.StartDate, $scope.currentItem.TakeDays);
                    $scope.currentItem.Nextvisitdate = GetNextEvalDate($scope.currentItem.EndDate, 1);
                    $scope.ChangeNextvisitdate();
                }
            }
            else {
                $scope.currentItem.EndDate = "";
            };
        };

        //日期增加天数
        function addByTransDate(dateParameter, num) {
            var translateDate = "", dateString = "", monthString = "", dayString = ""; hourString = ""; minString = ""; secString = "";
            translateDate = dateParameter.replace("-", "/").replace("-", "/");
            var newDate = new Date(translateDate);
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

            //如果小时数长度少于2，则前加 0 补位   
            if (newDate.getHours().toString().length == 1) {
                hourString = 0 + "" + newDate.getHours().toString();
            } else {
                hourString = newDate.getHours().toString();
            }

            //如果分钟数长度少于2，则前加 0 补位   
            if (newDate.getMinutes().toString().length == 1) {
                minString = 0 + "" + newDate.getMinutes().toString();
            } else {
                minString = newDate.getMinutes().toString();
            }

            //如果秒数长度少于2，则前加 0 补位   
            if (newDate.getSeconds().toString().length == 1) {
                secString = 0 + "" + newDate.getSeconds().toString();
            } else {
                secString = newDate.getSeconds().toString();
            }

            dateString = newDate.getFullYear() + "-" + monthString + "-" + dayString + "  " + hourString + ":" + minString + ":" + secString;
            return dateString;
        };

        //日期减少天数
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

        //得到日期  主方法  
        function showTime(pdVal) {
            var trans_day = "";
            var cur_date = new Date();
            var cur_year = new Date().format("yyyy");
            var cur_month = cur_date.getMonth() + 1;
            var real_date = cur_date.getDate();
            cur_month = cur_month > 9 ? cur_month : ("0" + cur_month);
            real_date = real_date > 9 ? real_date : ("0" + real_date);
            eT = cur_year + "-" + cur_month + "-" + real_date;
            if (pdVal == 1) {
                trans_day = addByTransDate(eT, 1);
            }
            else if (pdVal == -1) {
                trans_day = reduceByTransDate(eT, 1);
            }
            else {
                trans_day = eT;
            }
            //处理  
            return trans_day;
        };

        //选择药品
        $scope.rowSelect = function (item) {
            $scope.pillItem = item;
            $scope.PrescItem.CnName = item.CNName;
            $scope.PrescItem.ENName = item.ENName;
            $scope.PrescItem.FeeNo = $scope.currentResident.FeeNo;
            $scope.PrescItem.Dosage = item.Form;
            $scope.PrescItem.DrugId = item.DrugId;
            $scope.PrescItem.Units = item.Units;
            $scope.PrescItem.NsId = item.NSId;
            $scope.PrescItem.IsNciItem = item.IsNciItem;
            $scope.PrescItem.UnitPrice = item.UnitPrice;
            $scope.PrescItem.Qty = item.StandardUsage;
            $scope.PrescItem.TakeQty = item.MinPackage;
            $scope.PrescItem.TakeWay = item.DrugUsageMode;
        };

        $scope.getCost = function (item) {
            if (angular.isNumber(item.UnitPrice)) {
                item.Cost = item.UnitPrice * item.TotalQty;
            }
        }
        $scope.FreqSelected = function (item) {
            $scope.PrescItem.Freq = item.FREQNO;
            $scope.PrescItem.FreqName = item.FREQNAME;
            $scope.PrescItem.Freqtime = item.FREQTIME;
            $scope.PrescItem.Freqday = item.FREQDAY;
            $scope.PrescItem.Freqqty = item.FREQQTY;
        }
        $scope.editVisitpres = function (item) {
            $scope.isAdd = false;
            $scope.editName = "确定修改";
            $scope.PrescItem = item;
        };
        //添加保存处方的一条药品信息
        $scope.addVisitDocRec = function (item) {

            var isExit = false;
            angular.forEach($scope.Data.visitpresList, function (visitpres) {
                if(visitpres.CnName==item.CnName)
                {
                    isExit = true;
                }
            })
            if (isExit)
            {
                if (!confirm('已存在名称为' + item.CnName+"的药品,确定添加吗？")) {
                    return;
                }
            }

            if (angular.isDefined($scope.medForm.$error.required)) {
                for (var i = 0; i < $scope.medForm.$error.required.length; i++) {
                    utility.msgwarning($scope.medForm.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.medForm.$error.maxlength)) {
                for (var i = 0; i < $scope.medForm.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.medForm.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }


            if ($scope.isAdd)
            {
                $scope.Data.visitpresList.push(item);
                $scope.editName = "确定添加";
            }
            else
            {
                $scope.isAdd = true;
                $scope.editName == "确定修改";
            }
            $scope.PrescItem = {};
            $scope.pillItem = {};
         
        };

        //删除处方里的药品
        $scope.deleteVisitpres = function (item) {
            $scope.Data.visitpresList.splice($scope.Data.visitpresList.indexOf(item), 1);
        };

        //关闭弹出窗体
        $scope.closeWin = function () {
            $scope.PrescItem = {};
            $scope.Data.medicines = {};
            $scope.pillItem = {};
            $scope.keyword = "";
            $("#visitDocModal").modal("toggle");
            $scope.displayMode = "edit";
        };
        $scope.copy = function (feeNo) {
            var html = '<div km-include km-template="Views/Nursing/VisitDocPerRecords.html" km-controller="visitDocPerRecordCtrl"  km-include-params="{feeNo:\'' + feeNo + '\'}" ></div>';
            $scope.dialog = BootstrapDialog.show({
                title: '<label class=" control-label">就医记录</label>',
                type: BootstrapDialog.TYPE_DEFAULT,
                message: html,
                cssClass: 'visit-dialog',
                size: BootstrapDialog.SIZE_WIDE,
                onshow: function (dialog) {
                    var obj = dialog.getModalBody().contents();
                    $compile(obj)($scope);
                }
            });
        }
        $scope.hide = function () {
            $scope.dialog.close();
        }
        $scope.init();
    }])
 .controller("visitDocPerRecordCtrl", ['$scope', '$compile', '$http', '$state', '$stateParams', '$rootScope', 'visitdocrecordsRes', function ($scope, $compile, $http, $state, $stateParams, $rootScope, visitdocrecordsRes) {
     $scope.Data = {};
     $scope.hide = function () {
         $scope.$parent.hide();
     }
     $scope.select = function (item) {
         $scope.$parent.copyItem(item);
         $scope.$parent.hide();
     }
     $scope.init = function () {
         $scope.options = {
             buttons: [],//需要打印按钮时设置
             ajaxObject: visitdocrecordsRes,//异步请求的res
             success: function (data) {//请求成功时执行函数
                 $scope.Data.visitDocRecords = data.Data;
             },
             pageInfo: {//分页信息
                 CurrentPage: 1, PageSize: 10
             },
             params: {
                 feeNo: $scope.kmIncludeParams.feeNo == "" ? -1 : $scope.kmIncludeParams.feeNo
             }
         }
     };

     $scope.init();
 }]);



