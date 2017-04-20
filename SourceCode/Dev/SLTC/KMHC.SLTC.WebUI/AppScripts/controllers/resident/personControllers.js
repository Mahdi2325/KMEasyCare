///创建人:张正泉
///创建日期:2016-03-09
///说明:人员信息列表

angular.module("sltcApp")
    .controller('personCtrl', ['$scope', '$location', '$state', '$filter', 'utility', 'personRes', function ($scope, $location, $state, $filter, utility, personRes) {
        $scope.scenarioList = {};
        $scope.chooseItem = {};
        $scope.currentItem = {};
        $scope.regdiseasehisDtl = {};
        $scope.otherItems = [];
        $scope.editItem = {};
        var id = $state.params.id;
        $scope.mode = $state.params.mode;
        $scope.serviceResidentListUrl = "/angular/ResidentList";

        if ($scope.mode != undefined & $scope.mode != null && $scope.mode != "") {
            $scope.serviceResidentListUrl = $scope.serviceResidentListUrl + "/" + $scope.mode;
        };
        

        $scope.init = function () {
            $scope.person = {};
            $scope.RegNo = "0";
            if (id && id != "0") {
                $scope.isAdd = false;
                personRes.get({ id: id }, function (data) {
                    // angular.copy(data.Data, $scope.person);
                    $scope.person = data.Data;
                    $scope.$broadcast('LoadTabData', null);
                    if (data.Data != null) {
                        personRes.get({ sNo: 1 }, function (data) {
                            $scope.scenarioList = data.Data;
                            if (data.Data != null) {
                                //初始化界面
                                angular.forEach($scope.scenarioList, function (item) {
                                    angular.forEach(item.ScenarioItem, function (subItem) {

                                        subItem.IsShowInput = true;
                                        if (subItem.ScenarioItemOption.length > 0) {
                                            subItem.IsShowOption = true;
                                        }
                                    });
                                });
                                //
                                if ($scope.person.RegDisData.Regdiseasehis != null) {
                                    $scope.currentItem = $scope.person.RegDisData.Regdiseasehis;

                                    if ($scope.currentItem.HaveOperation) {
                                        $scope.chooseItem.tHaveOperation = true;
                                        $scope.chooseItem.fHaveOperation = false;
                                    }
                                    else if (!$scope.currentItem.HaveOperation) {
                                        $scope.chooseItem.tHaveOperation = false;
                                        $scope.chooseItem.fHaveOperation = true;
                                    }
                                    else {
                                        $scope.chooseItem.tHaveOperation = false;
                                        $scope.chooseItem.fHaveOperation = false;
                                    }

                                    if ($scope.currentItem.HaveDrugAllergy) {
                                        $scope.chooseItem.tHaveDrugAllergy = true;
                                        $scope.chooseItem.fHaveDrugAllergy = false;
                                    }
                                    else if (!$scope.currentItem.HaveDrugAllergy) {
                                        $scope.chooseItem.tHaveDrugAllergy = false;
                                        $scope.chooseItem.fHaveDrugAllergy = true;
                                    }
                                    else {
                                        $scope.chooseItem.tHaveDrugAllergy = false;
                                        $scope.chooseItem.fHaveDrugAllergy = false;
                                    }

                                    if ($scope.currentItem.HaveFoodAllergy) {
                                        $scope.chooseItem.tHaveFoodAllergy = true;
                                        $scope.chooseItem.fHaveFoodAllergy = false;
                                    }
                                    else if (!$scope.currentItem.HaveFoodAllergy) {
                                        $scope.chooseItem.tHaveFoodAllergy = false;
                                        $scope.chooseItem.fHaveFoodAllergy = true;
                                    }
                                    else {
                                        $scope.chooseItem.tHaveFoodAllergy = false;
                                        $scope.chooseItem.fHaveFoodAllergy = false;
                                    }

                                    if ($scope.currentItem.HaveTransfusion) {
                                        $scope.chooseItem.tHaveTransfusion = true;
                                        $scope.chooseItem.fHaveTransfusion = false;
                                    }
                                    else if (!$scope.currentItem.HaveTransfusion) {
                                        $scope.chooseItem.tHaveTransfusion = false;
                                        $scope.chooseItem.fHaveTransfusion = true;
                                    }
                                    else {
                                        $scope.chooseItem.tHaveTransfusion = false;
                                        $scope.chooseItem.fHaveTransfusion = false;
                                    }

                                    if ($scope.currentItem.IsAgreeTransfer) {
                                        $scope.chooseItem.tIsAgreeTransfer = true;
                                        $scope.chooseItem.fIsAgreeTransfer = false;
                                    }
                                    else if (!$scope.currentItem.IsAgreeTransfer) {
                                        $scope.chooseItem.tIsAgreeTransfer = false;
                                        $scope.chooseItem.fIsAgreeTransfer = true;
                                    }
                                    else {
                                        $scope.chooseItem.tIsAgreeTransfer = false;
                                        $scope.chooseItem.fIsAgreeTransfer = false;
                                    }

                                    if (angular.isDefined($scope.currentItem.MedicalHis) && $scope.currentItem.MedicalHis != null) {
                                        var medicalHis = $scope.currentItem.MedicalHis.split(',');
                                        for (var i = 0; i < medicalHis.length; i++) {
                                            if (medicalHis[i] == "中药") {
                                                $scope.chooseItem.cMedicalHis = true;
                                            }

                                            if (medicalHis[i] == "西药") {
                                                $scope.chooseItem.eMedicalHis = true;
                                            }

                                            if (medicalHis[i] == "按时服药") {
                                                $scope.chooseItem.onTime = true;
                                            }

                                            if (medicalHis[i] == "未依剂量服用") {
                                                $scope.chooseItem.notByDose = true;
                                            }

                                            if (medicalHis[i] == "断续服用") {
                                                $scope.chooseItem.goOn = true;
                                            }

                                        }
                                    }


                                    angular.forEach($scope.person.RegDisData.RegdiseasehisDtl, function (regDtl) {
                                        if (regDtl.ItemId == 0 || regDtl.ItemId == null) {
                                            $scope.editItem.OtherItemName = regDtl.OtherItemName;
                                            $scope.editItem.SickTime = regDtl.SickTime;
                                            $scope.editItem.OrgiTreatmentHos = regDtl.OrgiTreatmentHos;
                                            $scope.editItem.ExpectTransferTo = regDtl.ExpectTransferTo;
                                            $scope.otherItems.push($scope.editItem);
                                            $scope.editItem = {};
                                        }
                                        angular.forEach($scope.scenarioList, function (item) {
                                            if (regDtl.CategoryId == item.CategoryId) {
                                                angular.forEach(item.ScenarioItem, function (subItem) {
                                                    if (regDtl.ItemId == subItem.Id) {
                                                        subItem.RegdiseasehisDtlId = regDtl.Id;
                                                        subItem.IsCheck = true;
                                                        if (subItem.GroupId == 0) {
                                                            subItem.SickTime = regDtl.SickTime;
                                                            subItem.OrgiTreatmentHos = regDtl.OrgiTreatmentHos;
                                                            subItem.ExpectTransferTo = regDtl.ExpectTransferTo;
                                                            subItem.HaveCure = regDtl.HaveCure;
                                                            subItem.IsShowInput = false;
                                                        }
                                                        var options = regDtl.ScenarioOptionIds.split(',');
                                                        if (options.length > 0) {
                                                            subItem.IsShowOption = false;
                                                            angular.forEach(options, function (option) {
                                                                angular.forEach(subItem.ScenarioItemOption, function (subOption) {
                                                                    if (option == subOption.Id.toString()) {
                                                                        subOption.IsCheck = true;
                                                                        return;
                                                                    }
                                                                });
                                                            });
                                                        }
                                                    }
                                                    else {
                                                        if (!subItem.IsCheck) {
                                                            subItem.IsShowInput = true;
                                                        }
                                                    }

                                                });
                                                return;
                                            }

                                        });

                                    });
                                }
                                //else {

                                //    angular.forEach($scope.scenarioList, function (item) {
                                //        angular.forEach(item.ScenarioItem, function (subItem) {

                                //            subItem.IsShowInput = true;
                                //            if (subItem.ScenarioItemOption.length > 0) {
                                //                subItem.IsShowOption = true;
                                //            }
                                //        });
                                //    });
                                //}
                            }
                        });
                    }
                });

            } else {
                $scope.isAdd = true;
                $scope.person = {
                    Gender: "001",
                    BirthDay: "1956-01-01",
                    CreateDate: $filter("date")(new Date(), "yyyy-MM-dd"),
                    ImgUrl: "/Images/0.png",
                    DiseaseDiag: "",
                };

                personRes.get({ sNo: 1 }, function (data) {
                    $scope.scenarioList = data.Data;
                    if (data.Data != null) {
                        angular.forEach($scope.scenarioList, function (item) {
                            angular.forEach(item.ScenarioItem, function (subItem) {
                                subItem.IsShowInput = true;
                                if (subItem.ScenarioItemOption.length > 0) {
                                    angular.forEach(subItem.ScenarioItemOption, function (option) {
                                        option.IsShowOption = false;
                                    });
                                }
                            });
                        });
                    }
                });



            }
        }
        $scope.checkEvent = function (item, subItem) {
            if ($scope.person.DiseaseDiag == null) $scope.person.DiseaseDiag = "";
            //如果选中某个疾病复选框
            if (subItem.IsCheck) {
                //如果选中正常的复选框
                if (subItem.GroupId == 1) {
                    angular.forEach(item.ScenarioItem, function (val) {
                        //不正常的选项全部变更为不选中
                        if (val.GroupId == 0) {
                            val.IsCheck = false;
                            val.IsShowInput = true;
                            val.SickTime = "";
                            val.OrgiTreatmentHos = "";
                            val.ExpectTransferTo = "";
                            val.HaveCure = false;
                            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(val.ItemName + "、", "");
                            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(val.ItemName, "");
                            if (val.ScenarioItemOption.length > 0) {
                                angular.forEach(val.ScenarioItemOption, function (option) {
                                    $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(option.OptionName + "、", "");
                                    $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("(" + option.OptionName, "(");
                                    $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(option.OptionName + ")", ")");
                                    option.IsCheck = false;
                                });
                                val.IsShowOption = true;
                            }
                        }
                    });
                    subItem.IsShowInput = true;
                }
                    //如果选中不正常的复选框
                else if (subItem.GroupId == 0) {
                    angular.forEach(item.ScenarioItem, function (val) {
                        //正常的选项变更为不选中
                        if (val.GroupId == 1) {
                            val.IsCheck = false;
                        }
                    });
                    if ($scope.person.DiseaseDiag.indexOf(subItem.ItemName) > 0) {
                        return;
                    }
                    else {
                        if ($scope.person.DiseaseDiag == "") {
                            $scope.person.DiseaseDiag = subItem.ItemName
                        }
                        else {
                            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag + "、" + subItem.ItemName
                        }
                    }
                    subItem.IsShowInput = false;
                    if (subItem.ScenarioItemOption.length > 0) {
                        subItem.IsShowOption = false;
                    }
                }
            }
            else {
                subItem.IsShowInput = true;
                subItem.SickTime = "";
                subItem.OrgiTreatmentHos = "";
                subItem.ExpectTransferTo = "";
                subItem.HaveCure = false;
                $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(subItem.ItemName + "、", "");
                $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(subItem.ItemName, "");
                if (subItem.ScenarioItemOption.length > 0) {
                    angular.forEach(subItem.ScenarioItemOption, function (option) {
                        $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(option.OptionName + "、", "");
                              $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("(" +option.OptionName, "(");
                $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(option.OptionName + ")", ")");
                        option.IsCheck = false;
                    });
                    subItem.IsShowOption = true;
                }
            }

            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("()", "");
            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("、、", "、");
        };

        $scope.checkOptionEvent = function (subItem, option) {

            if (option.IsCheck) {
                $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(subItem.ItemName, subItem.ItemName + "(" + option.OptionName + ")");
            }
            else {
                $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(option.OptionName + "、", "");
                $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("(" + option.OptionName, "(");
                $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(option.OptionName + ")", ")");
            }
            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace(")(", "、");
            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("(、", "(");
            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("、)", ")");
            $scope.person.DiseaseDiag = $scope.person.DiseaseDiag.replace("()", "");
        };
        $scope.checkedChange = function (key, value) {
            switch (key) {
                case 'Operation':
                    if (value) {
                        if ($scope.chooseItem.tHaveOperation) {
                            if (angular.isDefined($scope.chooseItem.fHaveOperation)) {
                                if ($scope.chooseItem.fHaveOperation) {
                                    $scope.chooseItem.fHaveOperation = false;
                                }
                            }
                        }
                    }
                    else {
                        if ($scope.chooseItem.fHaveOperation) {
                            if (angular.isDefined($scope.chooseItem.tHaveOperation)) {
                                if ($scope.chooseItem.tHaveOperation) {
                                    $scope.chooseItem.tHaveOperation = false;
                                }
                            }
                        }
                        $scope.currentItem.Operation = "";
                    }
                    $scope.currentItem.HaveOperation = value;
                    break;
                case 'DrugAllergy':
                    if (value) {
                        if ($scope.chooseItem.tHaveDrugAllergy) {
                            if (angular.isDefined($scope.chooseItem.fHaveDrugAllergy)) {
                                if ($scope.chooseItem.fHaveDrugAllergy) {
                                    $scope.chooseItem.fHaveDrugAllergy = false;
                                }
                            }
                        }
                    }
                    else {
                        if ($scope.chooseItem.fHaveDrugAllergy) {
                            if (angular.isDefined($scope.chooseItem.tHaveDrugAllergy)) {
                                if ($scope.chooseItem.tHaveDrugAllergy) {
                                    $scope.chooseItem.tHaveDrugAllergy = false;
                                }
                            }
                        }
                        $scope.currentItem.DrugAllergy = "";
                    }
                    $scope.currentItem.HaveDrugAllergy = value;
                    break;
                case 'FoodAllergy':
                    if (value) {
                        if ($scope.chooseItem.tHaveFoodAllergy) {
                            if (angular.isDefined($scope.chooseItem.fHaveFoodAllergy)) {
                                if ($scope.chooseItem.fHaveFoodAllergy) {
                                    $scope.chooseItem.fHaveFoodAllergy = false;
                                }
                            }
                        }
                    }
                    else {
                        if ($scope.chooseItem.fHaveFoodAllergy) {
                            if (angular.isDefined($scope.chooseItem.tHaveFoodAllergy)) {
                                if ($scope.chooseItem.tHaveFoodAllergy) {
                                    $scope.chooseItem.tHaveFoodAllergy = false;
                                }
                            }
                        }
                        $scope.currentItem.FoodAllergy = "";
                    }
                    $scope.currentItem.HaveFoodAllergy = value;
                    break;
                case 'Transfusion':
                    if (value) {
                        if ($scope.chooseItem.tHaveTransfusion) {
                            if (angular.isDefined($scope.chooseItem.fHaveTransfusion)) {
                                if ($scope.chooseItem.fHaveTransfusion) {
                                    $scope.chooseItem.fHaveTransfusion = false;
                                }
                            }
                        }
                    }
                    else {
                        if ($scope.chooseItem.fHaveTransfusion) {
                            if (angular.isDefined($scope.chooseItem.tHaveTransfusion)) {
                                if ($scope.chooseItem.tHaveTransfusion) {
                                    $scope.chooseItem.tHaveTransfusion = false;
                                }
                            }
                        }
                    }
                    $scope.currentItem.HaveTransfusion = value;
                    break;
                case 'IsAgreeTransfer':
                    if (value) {
                        if ($scope.chooseItem.tIsAgreeTransfer) {
                            if (angular.isDefined($scope.chooseItem.fIsAgreeTransfer)) {
                                if ($scope.chooseItem.fIsAgreeTransfer) {
                                    $scope.chooseItem.fIsAgreeTransfer = false;
                                }
                            }
                        }
                    }
                    else {
                        if ($scope.chooseItem.fIsAgreeTransfer) {
                            if (angular.isDefined($scope.chooseItem.tIsAgreeTransfer)) {
                                if ($scope.chooseItem.tIsAgreeTransfer) {
                                    $scope.chooseItem.tIsAgreeTransfer = false;
                                }
                            }
                        }
                    }
                    $scope.currentItem.IsAgreeTransfer = value;
                    break;
            }
        };
        $scope.editOtherItem = function (item) {
            $scope.editItem = item;
        }
        $scope.saveOtherItem = function (item) {
            if (!angular.isDefined(item.OtherItemName) || item.OtherItemName == null) {
                utility.msgwarning("疾病名称为必填项！");
                return;
            }
            $scope.otherItems.push(item);
            $scope.editItem = {};
        }
        $scope.deleteOtherItem = function (item) {
            $scope.otherItems.splice($scope.otherItems.indexOf(item), 1);
        }

        $scope.init();
        $scope.save = function () {
            //处理疾病史的数据
            //********* Start *********
            $scope.currentItem.MedicalHis = "";
            $scope.MedicalHis = [];
            if ($scope.chooseItem.cMedicalHis) {
                $scope.MedicalHis.push("中药");
            }
            if ($scope.chooseItem.eMedicalHis) {
                $scope.MedicalHis.push("西药");
            }
            if ($scope.chooseItem.onTime) {
                $scope.MedicalHis.push("按时服药");
            }
            if ($scope.chooseItem.notByDose) {
                $scope.MedicalHis.push("未依剂量服用");
            }
            if ($scope.chooseItem.goOn) {
                $scope.MedicalHis.push("断续服用");
            }
            if ($scope.MedicalHis.length > 0) {
                for (var i = 0; i < $scope.MedicalHis.length; i++) {
                    if (i == $scope.MedicalHis.length - 1) {
                        $scope.currentItem.MedicalHis = $scope.currentItem.MedicalHis + $scope.MedicalHis[i];
                    }
                    else {
                        $scope.currentItem.MedicalHis = $scope.currentItem.MedicalHis + $scope.MedicalHis[i] + ",";
                    }

                }
            }
            $scope.person.RegDisData = {};
            $scope.person.RegDisData.RegdiseasehisDtl = [];
            angular.forEach($scope.scenarioList, function (item) {
                angular.forEach(item.ScenarioItem, function (subItem) {
                    if (subItem.IsCheck) {
                        $scope.regdiseasehisDtl = {};
                        $scope.regdiseasehisDtl.Id = subItem.RegdiseasehisDtlId;
                        $scope.regdiseasehisDtl.CategoryId = item.CategoryId;
                        $scope.regdiseasehisDtl.ItemId = subItem.Id;
                        $scope.regdiseasehisDtl.SickTime = subItem.SickTime;
                        $scope.regdiseasehisDtl.OrgiTreatmentHos = subItem.OrgiTreatmentHos;
                        $scope.regdiseasehisDtl.ExpectTransferTo = subItem.ExpectTransferTo;
                        $scope.regdiseasehisDtl.HaveCure = subItem.HaveCure;
                        $scope.regdiseasehisDtl.ScenarioOptionIds = "";
                        $scope.scenarioOptionIds = [];
                        angular.forEach(subItem.ScenarioItemOption, function (option) {
                            if (option.IsCheck) {
                                $scope.scenarioOptionIds.push(option);
                            }
                        });
                        if ($scope.scenarioOptionIds.length > 0) {
                            for (var i = 0; i < $scope.scenarioOptionIds.length; i++) {
                                if (i == $scope.scenarioOptionIds.length - 1) {
                                    $scope.regdiseasehisDtl.ScenarioOptionIds = $scope.regdiseasehisDtl.ScenarioOptionIds + $scope.scenarioOptionIds[i].Id;
                                }
                                else {
                                    $scope.regdiseasehisDtl.ScenarioOptionIds = $scope.regdiseasehisDtl.ScenarioOptionIds + $scope.scenarioOptionIds[i].Id + ",";
                                }
                            }
                        }
                        $scope.person.RegDisData.RegdiseasehisDtl.push($scope.regdiseasehisDtl);
                    }
                });
            });
            angular.forEach($scope.otherItems, function (item) {
                $scope.regdiseasehisDtl = {};
                $scope.regdiseasehisDtl.CategoryId = 0;
                $scope.regdiseasehisDtl.ItemId = 0;
                $scope.regdiseasehisDtl.OtherItemName = item.OtherItemName;
                $scope.regdiseasehisDtl.SickTime = item.SickTime;
                $scope.regdiseasehisDtl.OrgiTreatmentHos = item.OrgiTreatmentHos;
                $scope.regdiseasehisDtl.ExpectTransferTo = item.ExpectTransferTo;
                $scope.regdiseasehisDtl.HaveCure = item.HaveCure;
                $scope.person.RegDisData.RegdiseasehisDtl.push($scope.regdiseasehisDtl);
            });
            $scope.person.RegDisData.Regdiseasehis = $scope.currentItem;
            //*********  End  *********
            personRes.save($scope.person, function (data) {
                if (data.ResultMessage) {
                    utility.message(data.ResultMessage);
                    return;
                }
                utility.message($scope.person.Name + "的信息保存成功！");
                if (angular.isDefined($state.stateName)) {
                    $state.go($state.stateName);
                }
                else {
                    $location.url('/angular/PersonList');
                }
                //$window.history.back();
            });
        };

        $scope.cancel = function () {
            $location.url($scope.serviceResidentListUrl);
        };
        $scope.person.Age = function () {
            return $filter("ageFormat")(person.Brithdate);
        }
    }])
    .controller("personListCtrl", ['$scope', '$http', '$state', '$location', 'utility', 'personRes', 'floorRes', 'roomRes', function ($scope, $http, $state, $location, utility, personRes, floorRes, roomRes) {
        //$scope.currentPage = 1;
        //$scope.search = $scope.reload = function () {
        //    var keyWord = "";
        //    if (angular.isDefined($scope.keyword)) {
        //        keyWord = $scope.keyword;
        //    }
        //    personRes.get({ keyWord: keyWord, currentPage: $scope.currentPage, pageSize: 10 }, function (response) {
        //        $scope.Persons = response.Data;
        //        var pager = new Pager('pager', $scope.currentPage, response.PagesCount, function (curPage) {
        //            $scope.currentPage = curPage;
        //            $scope.search();
        //        });
        //    });
        //};

        //初始化数据源
        $scope.Data = {};
        //获取所有楼层信息
        floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {

            $scope.Data.floors = data.Data;
        });
        //获取所有房间信息
        roomRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
            $scope.Data.rooms = data.Data;
        });
        $scope.$watch('options.params.FloorName',
    function (newVal, oldVal, scope) {
        if (newVal === oldVal) {
            // 只会在监控器初始化阶段运行
        } else {
            // 初始化之后发生的变化
            roomRes.get({ CurrentPage: 1, PageSize: 100, floorName: $scope.options.params.FloorName }, function (data) {
                $scope.Data.rooms = data.Data;
            });
        }
    });
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: personRes,//异步请求的res
            params: { keyWord: "", FloorName: "", RoomName: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Persons = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
        $scope.Print = function (item) {
            if (angular.isDefined(item.RegNo)) {
                if (item.RegNo == 0) {
                    utility.message("无打印数据！");
                    return;
                }
                window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("PA002", item.RegNo, "", ""), "_blank");

            } else {
                utility.message("无打印数据！");
            }
        }
        //根据关键字过滤结果
        $scope.filterItems = function (item) {
            if ($scope.keyword) {
                return (angular.isDefined(item.IdNo) && item.IdNo.indexOf($scope.keyword) >= 0)
                 ||
                 (angular.isDefined(item.Name) && item.Name.indexOf($scope.keyword) >= 0)
            }
            return true;
        };

        $scope.edit = function (item) {
            $state.go('Person.BasicInfo', { id: item.RegNo });
            $state.stateName = "PersonList";
        }

        $scope.delete = function (id) {
            if (confirm("确定删除该住民信息吗?")) {
                personRes.delete({ id: id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            }
        };

        $scope.searchInfo = function () {
            $scope.options.params.FloorName = !$scope.options.params.FloorName ? "" : $scope.options.params.FloorName;
            $scope.options.params.RoomName = !$scope.options.params.RoomName ? "" : $scope.options.params.RoomName;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
        };
    }]);
