/*
创建人:张凯
创建日期:2016-03-16
说明: 指标-感染
*/
angular.module("sltcApp")
.controller("infectionCtrl",['$scope', '$state', 'dictionary', 'utility', 'infectionRes', 'infectionSympotmRes', 'labExamRes', 'infectionItemRes', 'symptomItemRes', function ($scope, $state, dictionary, utility, infectionRes, infectionSympotmRes, labExamRes, infectionItemRes, symptomItemRes) {   
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.Data.infections = [];
    $scope.infectionsOptions = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: infectionRes,//异步请求的res
        params: { regNo: 0, feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo },
        success: function (data) {//请求成功时执行函数
            $scope.Data.infections = data.Data;
            if ($scope.Data.infections.length > 0) {
                if (angular.isDefined($scope.currentItem.SeqNo)) {
                    $scope.rowSelect($scope.currentItem);
                } else {
                    $scope.rowSelect($scope.Data.infections[0]);
                }
            } else {
                $scope.rowSelect($scope.currentItem);
            }
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }


    $scope.curUser = utility.getUserInfo();
    $scope.currentItem = {
        RecordBy: $scope.curUser.EmpNo
    };
    $scope.Dic = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;
    $scope.newButtonShow = false;
    //$scope.SaveListShow = false;
    //加载感染项目选择项
    $scope.initDic = function () {
        infectionItemRes.get({}, function (data) {
            $scope.Dic.InfectionList = data.Data;
        });

        symptomItemRes.get({ ItemCode:""}, function (data) {
            $scope.Dic.SymptomListAll = data.Data;
        });
    };
    $scope.initDic();
    //当感染项目改变，加载对应的症状
    $scope.ItemChange = function (item) {
        if (angular.isDefined(item.ItemNo) && item.ItemNo !== null) {
            $.each($scope.Dic.InfectionList, function (n, list) {
                if (list.ItemCode == item.ItemNo) {
                    $scope.currentItem.SymInfo.ItemName = list.ItemName;
                }
            });
            $scope.Dic.SymptomList = $.grep($scope.Dic.SymptomListAll, function (bp) {
                return bp.ItemCode == item.ItemNo;
            });
        }
    };


    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.RegNo, $scope.currentResident.FeeNo);
        $scope.currentItem = {
            FeeNo: $scope.currentResident.FeeNo,
            RegNo: $scope.currentResident.RegNo
        };
        //清空编辑项
        //$scope.initPage($scope.currentResident.RegNo, $scope.currentResident.FeeNo);//加载当前住民的感染页面信息
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }


    $scope.listItem = function (regNo, feeNo) {
        $scope.infectionsOptions.params.regNo = regNo;
        $scope.infectionsOptions.params.feeNo = feeNo;
        $scope.infectionsOptions.search();
        //infectionRes.get({ regNo: regNo, feeNo: feeNo }, function (data) {
        //    if (angular.isDefined(data.Data)) {
        //        $scope.Data.infections = data.Data;
        //    }
        //});
    }

    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的感染记录吗?")) {
            infectionRes.delete({ id: item.SeqNo }, function () {
                $scope.Data.infections.splice($scope.Data.infections.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的感染信息删除成功！");
            });
        }
    };

    $scope.createItem = function (item) {
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        infectionRes.save($scope.currentItem, function (data) {
            $scope.Data.infections.push(data.Data);
        });
        $scope.currentItem = {};
    };

    $scope.updateItem = function (item) {
        infectionRes.save(item, function (data) {
            //angular.copy(data.Data, item);
        });
        $scope.currentItem = {};
    };

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
        $scope.newButtonShow = true;
        initPage(item.SeqNo);
    };

    $scope.saveEdit = function (item) {

        if (angular.isDefined($scope.unPlanfrom.$error.required)) {
            for (var i = 0; i < $scope.unPlanfrom.$error.required.length; i++) {
                utility.msgwarning($scope.unPlanfrom.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.unPlanfrom.$error.maxlength)) {
            for (var i = 0; i < $scope.unPlanfrom.$error.maxlength.length; i++) {
                utility.msgwarning($scope.unPlanfrom.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.unPlanfrom2.$error.maxlength)) {
            for (var i = 0; i < $scope.unPlanfrom2.$error.maxlength.length; i++) {
                utility.msgwarning($scope.unPlanfrom2.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined(item.FeeNo)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的感染信息保存成功！");
    };

    $scope.currentItem.symInfoList = [];
    $scope.symInfoListOptions = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: infectionSympotmRes,//异步请求的res
        params: { seqNo: 0 },
        success: function (data) {//请求成功时执行函数
            $scope.currentItem.symInfoList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }

    $scope.currentItem.labExamList = [];
    $scope.labExamListOptions = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: labExamRes,//异步请求的res
        params: { seqNo: 0 },
        success: function (data) {//请求成功时执行函数
            $scope.currentItem.labExamList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }

    //加载感染页面信息
    var initPage = function (seqNo) {
        if (seqNo != "") {
            $scope.symInfoListOptions.params.seqNo = seqNo;
            $scope.symInfoListOptions.search();
            $scope.labExamListOptions.params.seqNo = seqNo;
            $scope.labExamListOptions.search();
            ////$scope.currentItem = {};
            //$scope.currentItem.symInfoList = [];
            //$scope.currentItem.labExamList = [];
            ////感染
            ////infectionRes.get({ regNo: regNo, feeNo: feeNo,  }, function (data) {
            ////    if (data.Data.length > 0) {
            ////        $scope.currentItem = data.Data[0];
            ////    } else {
            ////        $scope.currentItem = {};
            ////    }
            ////});
            ////感染症状列表
            //infectionSympotmRes.get({ seqNo: seqNo }, function (data) {
            //    $scope.currentItem.symInfoList = data.Data;
            //});
            ////检体送检情形列表
            //labExamRes.get({ seqNo: seqNo }, function (data) {
            //    $scope.currentItem.labExamList = data.Data;
            //});
        }
    }
    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.RecordBy = item.EmpNo;
        $scope.currentItem.RecordNameBy = item.EmpName;
    }

    $scope.saveIfcSym = function () {
        if (angular.isDefined($scope.ifcform.$error.required)) {
            for (var i = 0; i < $scope.ifcform.$error.required.length; i++) {
                utility.msgwarning($scope.ifcform.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.ifcform.$error.maxlength)) {
            for (var i = 0; i < $scope.ifcform.$error.maxlength.length; i++) {
                utility.msgwarning($scope.ifcform.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        $scope.currentItem.SymInfo.SeqNo = $scope.currentItem.SeqNo;
        infectionSympotmRes.save($scope.currentItem.SymInfo, function (data) {
            if (!$scope.currentItem.SymInfo.Id) {
                $scope.currentItem.symInfoList.push(data.Data);
            }
            $('#IfcSymModal').modal('hide');
            utility.message($scope.currentResident.Name + "的感染项目信息保存成功！");
        });
    }

    $scope.saveLabExam = function () {
        $scope.currentItem.ExamInfo.SeqNo = $scope.currentItem.SeqNo;
        labExamRes.save($scope.currentItem.ExamInfo, function (data) {
            if(!$scope.currentItem.ExamInfo.Id) {
                $scope.currentItem.labExamList.push(data.Data);
            }
            $('#LabExamModal').modal('hide');
            utility.message($scope.currentResident.Name + "的送检情形信息保存成功！");
        });
    }

    //删除感染项目
    $scope.deleteIfcSym = function (item) {
        if (confirm("您确定要删除该住民的感染项目吗?")) {
            infectionSympotmRes.delete({ id: item.Id }, function () {
                $scope.currentItem.symInfoList.splice($scope.currentItem.symInfoList.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的感染项目信息删除成功！");
            });
        }
    };

    //删除感染项目
    $scope.deleteLabExam = function (item) {
        if (confirm("您确定要删除该住民的送检情形吗?")) {
            labExamRes.delete({ id: item.Id }, function () {
                $scope.currentItem.labExamList.splice($scope.currentItem.labExamList.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的送检情形信息删除成功！");
            });
        }
    };

    $scope.infectOpen = function (data) {
        $scope.currentItem.SymInfo = data;
        $scope.ItemChange($scope.currentItem.SymInfo);
        openIncSymModal();
    };

    $scope.examOpen = function (data) {
        $scope.currentItem.ExamInfo = data;
        openExamModal();
    };
    
    var openIncSymModal = function () {
        $('#IfcSymModal').modal({
            backdrop: true,
            keyboard: true,
            show: true
        });
    }

    var openExamModal = function () {
        $('#LabExamModal').modal({
            backdrop: true,
            keyboard: true,
            show: true
        });
    }

    $scope.showSympotm = function (SympotmId) {
        var name = "";
        if (angular.isDefined(SympotmId) && SympotmId !== null) {
            $.each($scope.Dic.SymptomListAll, function (n, list) {
                if (list.Id === SympotmId) {
                    name= list.SympotmName;
                }
            });
        }
        return name;

    };

}]);
