/*
创建人:张凯
创建日期:2016-03-14
说明: 交付照会  adminHandoversCtrl
*/
angular.module("sltcApp")
.controller("adminHandoversCtrl", ['$scope', '$filter', 'utility', 'adminHandoversRes', '$state', function ($scope, $filter, utility, adminHandoversRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    var empInfo = utility.getUserInfo();
    $scope.currentItem = {
        AssignedBy: empInfo.EmpNo,
        AssignedName: empInfo.EmpName
    }
    // 当前住民
    $scope.curResident = {}
    $scope.buttonShow = false;
    var d = new Date(), dateTime = $filter("date")(d, "yyyy-MM-dd HH:mm:ss"), classType = utility.getClassType(d);
    $scope.assignDate = "";//交付日期当前值 Add by Duke on 20160809
    $scope.performDate = "";//执行日期当前值 Add by Duke on 20160809
    //选中住民 
    $scope.residentSelected = function (resident) {
        $scope.curResident = resident;//获取当前住民信息
        $scope.listItem($scope.curResident.FeeNo, $scope.curResident.OrgId);//加载当前住民的交付照会记录
        var empInfo = utility.getUserInfo();
        $scope.currentItem = {
            AssignedBy: empInfo.EmpNo,
            AssignedName: empInfo.EmpName
        }
        if (angular.isDefined($scope.curResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    //获取住民的交付照会
    $scope.listItem = function (FeeNo, OrgId) {
        $scope.Data.AssignList = {};

        $scope.options.params.feeNo = FeeNo;
        $scope.options.params.OrgId = OrgId;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    }

    //加载交付照会历史数据
    $scope.init = function () {
        $scope.Data.AssignList = {};

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: adminHandoversRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.AssignList = data.Data;
                $scope.currentItem.AssignDate = dateTime;
                $scope.currentItem.PerformDate = dateTime;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }
    }


    $scope.choseDate = function () {
        if ($scope.currentItem.RecStatus == true) {
            $scope.currentItem.FinishDate = dateTime;
        }
        else {
            $scope.currentItem.FinishDate = "";
        }

    }

    //删除交付照会记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的交付照会记录吗?")) {
            adminHandoversRes.delete({ id: item.Id }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                utility.message($scope.curResident.Name + "的交付照会删除成功！");
            });
        }
    };

    //选择人员
    $scope.staffSelected = function (item, type) {
        if (type === "AssignedBy") {
            $scope.currentItem.AssignedName = item.EmpName;
            $scope.currentItem.AssignedBy = item.EmpNo;
        } else if (type === "Assignee") {
            $scope.currentItem.AssignName = item.EmpName;
            $scope.currentItem.Assignee = item.EmpNo;
        }

    }

    $scope.createItem = function (item) {
        //新增交付照会记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.curResident.FeeNo;
        if ($scope.currentItem.RecStatus == null || $scope.currentItem.RecStatus == "") {
            $scope.currentItem.RecStatus = false;
        }
        if ($scope.currentItem.NewrecFlag == null || $scope.currentItem.NewrecFlag == "") {
            $scope.currentItem.NewrecFlag = true;
        }

        adminHandoversRes.save($scope.currentItem, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();

            //$scope.Data.AssignList.push(data.Data);
        });
        var empInfo = utility.getUserInfo();
        $scope.currentItem = {
            AssignedBy: empInfo.EmpNo,
            AssignedName: empInfo.EmpName
        }
        $scope.currentItem.AssignDate = dateTime;
        $scope.currentItem.PerformDate = dateTime;
        //$scope.currentItem.FinishDate = dateTime;
    };

    $scope.updateItem = function (item) {
        adminHandoversRes.save(item, function (data) {
            var empInfo = utility.getUserInfo();

        });
        $scope.currentItem = {
            AssignedBy: empInfo.EmpNo,
            AssignedName: empInfo.EmpName
        }
        $scope.currentItem.AssignDate = dateTime;
        $scope.currentItem.PerformDate = dateTime;
        //$scope.currentItem.FinishDate = dateTime;
    };

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
        //交付日期当前值赋值 Add by Duke on 20160809
        if (angular.isDefined($scope.currentItem.AssignDate)) {
            $scope.assignDate = $scope.currentItem.AssignDate;
        }
        //执行日期当前值赋值 Add by Duke on 20160809
        if (angular.isDefined($scope.currentItem.AssignDate)) {
            $scope.assignDate = $scope.currentItem.AssignDate;
        }
    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.curResident.Name + "的交付照会信息保存成功！");
    };

    //检查交付日期是否在执行日期之前 Add by Duke on 20160809
    $scope.checkAssignDate = function () {
        if (angular.isDefined($scope.currentItem.AssignDate) && angular.isDefined($scope.currentItem.PerformDate)) {
            if (!checkDate($scope.currentItem.AssignDate, $scope.currentItem.PerformDate)) {
                utility.msgwarning("交付日期应在执行日期之前");
                $scope.currentItem.AssignDate = $scope.assignDate;
            }
            else
            {
                $scope.assignDate = $scope.currentItem.AssignDate;
            }
        }

    }

    //检查执行日期是否在交付日期之后 Add by Duke on 20160809
    $scope.checkPerformDate = function () {
        if (angular.isDefined($scope.currentItem.AssignDate) && angular.isDefined($scope.currentItem.PerformDate)) {
            if (!checkDate($scope.currentItem.AssignDate, $scope.currentItem.PerformDate)) {
                utility.msgwarning("执行日期应在交付日期之后");
                $scope.currentItem.PerformDate = $scope.performDate;
            }
            else
            {
                $scope.performDate = $scope.currentItem.PerformDate;
            }
        }

    }

    $scope.init();
}])
    .controller("shiftWorkCtrl", ['$scope', '$filter', 'utility', 'empFileRes', 'adminHandoversRes', 'adminHandoversMultiRes', 'nursingHandoverRes', 'affairsHandoverRes','$state',
        function ($scope, $filter, utility, empFileRes, adminHandoversRes, adminHandoversMultiRes, nursingHandoverRes, affairsHandoverRes, $state) {
            $scope.FeeNo = $state.params.FeeNo;
            $scope.recordDate = "";//交付日期当前值 Add by Duke on 20160811
            $scope.excuteDate = "";//执行日期当前值 Add by Duke on 20160811
            $scope.init = function () {
                var empInfo = utility.getUserInfo(), d = new Date(),
                    classType = utility.getClassType(d);
                $scope.record = {
                    RecordDate: $filter("date")(d, "yyyy-MM-dd HH:mm:ss"),
                    ExcuteDate: $filter("date")(d.dateAdd("d", 1), "yyyy-MM-dd HH:mm:ss"),
                    ClassType: classType,
                    AssignedBy: empInfo.EmpNo,
                    AssignedName: empInfo.EmpName
                }
                $scope.recordDate = $scope.record.RecordDate;
                $scope.excuteDate = $scope.record.ExcuteDate;;
                $scope.empFilesSelected = [];
                $scope.empFiles = [];
                $scope.curResident = $scope.curResident || {};
            }

            //员工选择框回调函数
            $scope.staffSelected = function (item, tp) {
                switch (tp) {
                    case 1:
                        $scope.record.AssignedBy = item.EmpNo;
                        $scope.record.AssignedName = item.EmpName;
                        break;
                    case 2:
                        $scope.record.AssignBy = item.EmpNo;
                        $scope.record.AssignName = item.EmpName;
                        break;
                }
            }

            //检查交付日期是否在执行日期之前 Add by Duke on 20160811
            $scope.checkRecordDate = function () {
                if (angular.isDefined($scope.record.RecordDate) && angular.isDefined($scope.record.ExcuteDate)) {
                    if (!checkDate($scope.record.RecordDate, $scope.record.ExcuteDate)) {
                        utility.msgwarning("交付日期应在执行日期之前");
                        $scope.record.RecordDate = $scope.recordDate;
                    }
                    else {
                        $scope.recordDate = $scope.record.RecordDate;
                    }
                }

            }

            //检查执行日期是否在交付日期之后 Add by Duke on 20160811
            $scope.checkExcuteDate = function () {
                if (angular.isDefined($scope.record.RecordDate) && angular.isDefined($scope.record.ExcuteDate)) {
                    if (!checkDate($scope.record.RecordDate, $scope.record.ExcuteDate)) {
                        utility.msgwarning("执行日期应在交付日期之后");
                        $scope.record.ExcuteDate = $scope.excuteDate;
                    }
                    else {
                        $scope.excuteDate = $scope.record.ExcuteDate;
                    }
                }

            }
            //读取员工时操作
            $scope.readEmp = function () {
                if ($scope.IncludeFlag) {
                    empFileRes.get({ currentPage: 1, pageSize: 100, needEmp: 1, needShort: 1 }, function (response) {
                        $scope.empFiles = response.Data;
                    });
                } else {
                    empFileRes.get({ currentPage: 1, pageSize: 100, needShort: 1 }, function (response) {
                        $scope.empFiles = response.Data;
                    });
                }
                $scope.empFilesSelected = [];
            }

            //员工多选框操作
            $scope.addEmp = function (type) {
                if (type == "right") {//添加员工
                    var temp = [];
                    $("#sltFrom option:selected").each(function () {
                        temp.push({ EmpNo: this.value, EmpName: this.text });
                    });
                    $scope.empFilesSelected = $scope.empFilesSelected.concat(temp);
                    for (var i = temp.length - 1; i > -1; i--) {
                        var j = $scope.empFiles.length;
                        while ($scope.empFiles[--j].EmpNo != temp[i].EmpNo);
                        $scope.empFiles.splice(j, 1);
                    }
                } else {//减少员工
                    var temp = [];
                    $("#sltTo option:selected").each(function () {
                        temp.push({ EmpNo: this.value, EmpName: this.text });
                    });
                    $scope.empFiles = $scope.empFiles.concat(temp);
                    for (var i = temp.length - 1; i > -1; i--) {
                        var j = $scope.empFilesSelected.length;
                        while ($scope.empFilesSelected[--j].EmpNo != temp[i].EmpNo);
                        $scope.empFilesSelected.splice(j, 1);
                    }
                }

            }

            //保存工作照会
            $scope.saveAssign = function () {
                if (!angular.isDefined($scope.record.Content) || $scope.record.Content=="")
                {
                    utility.msgwarning("交付内容不能为空");
                    return;
                }
                var record = $scope.record, rcdList = [], saveObj = null, emps = $scope.empFilesSelected;
                if (record) {
                    if (!record.FeeNo) {
                        record.FeeNo = $scope.curResident.FeeNo;
                    }
                    if (emps.length > 0) {
                        for (var i = emps.length - 1; i > -1; i--) {
                            saveObj = {
                                AssignBy: emps[i].EmpNo,
                                AssignName: emps[i].EmpName,
                                AssignDate: record.RecordDate,
                                AssignedBy: record.AssignedBy,
                                AssignedName: record.AssignedName,
                                Assignee: emps[i].EmpNo,
                                ClassType: record.ClassType,
                                PerformDate: record.ExcuteDate,
                                Content: record.Content,
                                FeeNo: record.FeeNo
                            };
                            rcdList.push(saveObj);
                        }
                        adminHandoversMultiRes.save(rcdList, function (data) {
                            //$scope.init();
                            utility.message($scope.record.AssignedName + "的交付照会信息保存成功！");
                        });
                    } else {
                        saveObj = {
                            AssignBy: record.AssignBy,
                            AssignName: record.AssignName,
                            AssignDate: record.RecordDate,
                            AssignedBy: record.AssignedBy,
                            AssignedName: record.AssignedName,
                            Assignee: record.AssignBy,
                            ClassType: record.ClassType,
                            PerformDate: record.ExcuteDate,
                            Content: record.Content,
                            FeeNo: record.FeeNo
                        };
                        adminHandoversRes.save(saveObj, function (data) {
                            //$scope.init();
                            utility.message($scope.record.AssignedName + "的交付照会信息保存成功！");
                        });
                    }
                }
            }

            $scope.saveNursing = function () {
                var record = $scope.record, tempObj = null,nrcdList = [], emps = $scope.empFilesSelected;
                if (record) {
                    if (emps.length > 0) {
                        for(var i=emps.length-1;i>=0;i--){
                            var saveObj = { FeeNo: record.FeeNo, RegNo: record.RegNo };
                            switch (record.ClassType) {
                                case "D":
                                    tempObj = { Content_D: record.Content, RecDate_D: record.RecordDate, RecordBy_D: emps[i].EmpNo, Nurse_D: emps[i].EmpName };
                                    break;
                                case "E":
                                    tempObj = { Content_E: record.Content, RecDate_E: record.RecordDate, RecordBy_E: emps[i].EmpNo, Nurse_E: emps[i].EmpName };
                                    break;
                                case "N":
                                    tempObj = { Content_N: record.Content, RecDate_N: record.RecordDate, RecordBy_N: emps[i].EmpNo, Nurse_N: emps[i].EmpName };
                                    break;
                            }
                            angular.extend(saveObj, tempObj);
                            nrcdList.push(saveObj);
                        }
                    }
                   
                    if (!record.FeeNo && !record.RegNo) {
                        utility.message("请先选择住民！");
                        return;
                    };
                    nursingHandoverRes.mulSave(nrcdList, function () {
                        //$scope.init();
                        utility.message($scope.record.AssignedName + "的护理交班信息保存成功！");
                    });
                }
            }

            $scope.saveAffair = function () {
                var record = $scope.record, atempObj = null,arcdList = [], emps = $scope.empFilesSelected;
                if (record) {
                    var saveObj = {
                        RecordDate: record.RecordDate,
                        RecordBy: record.AssignedBy,
                        RecorderName: record.AssignedName,
                        Content: record.Content,
                        ClassType: record.ClassType,
                       
                        //ExecutiveName: record.AssignName,
                        //ExecuteBy: record.AssignBy,
                        ExecuteDate: record.ExcuteDate
                    };
                    if (emps.length > 0) {
                        for (var i = emps.length - 1; i >= 0; i--) {
                            var aobj = {
                                //RecordDate: record.RecordDate,
                                //RecordBy: emps[i].EmpNo,
                                //RecorderName:emps[i].EmpName,
                                //Content: record.Content,
                                //ClassType: record.ClassType,
                                //ExecutiveName: record.AssignName,
                                //ExecuteBy: record.AssignBy,
                                //ExecuteDate: record.ExcuteDate

                                RecordDate: record.RecordDate,
                                RecordBy: record.AssignedBy,
                                RecorderName: record.AssignedName,
                                Content: record.Content,
                                ClassType: record.ClassType,
                                ExecutiveName: emps[i].EmpName,
                                ExecuteBy: emps[i].EmpNo,
                                ExecuteDate: record.ExcuteDate
                            };
                           
                           
                            arcdList.push(aobj);
                        }
                      
                    } else {
                        arcdList.push(saveObj);
                    }
                   
                    affairsHandoverRes.mulSave(arcdList, function (data) {
                        //$scope.init();
                        utility.message($scope.record.AssignedName + "的行政交班信息保存成功！");
                    });
                }
            }
            $scope.txtResidentIDChange = function (resident) {
                $scope.record.FeeNo = resident.FeeNo;
                $scope.record.RegNo = resident.RegNo;
                $scope.curResident = resident;
            }


            $scope.init();
        }]);
