/*
创建人: 张正泉
创建日期:2016-03-08
说明: 
*/
angular.module("sltcApp")
    .controller("nursingRecordMainCtrl", ['$scope','$state',
function ($scope,$state) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.curResident = { RegNo: 0, FeeNo: 0 };

        $scope.residentSelected = function (resident) {
            $scope.curResident = resident;
            $scope.$broadcast("residentChange", resident);
        }
    }])
    .controller("nursingRecordCtrl", ['$scope', '$location', '$filter', 'nursingRecordRes', 'utility',

    function ($scope, $location, $filter, nursingRecordRes, utility) {
        $scope.init = function () {
            $scope.loadInfo();
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: nursingRecordRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.nursingRecords = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.curResident.FeeNo,
                    regNo: 0
                }
            }
        }
        $scope.Print = function (item) {
            if (angular.isDefined(item.Id)) {
                if (item.Id == 0) {
                    utility.message("无打印数据！");
                    return;
                }
                window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("P18ReportS", item.Id, "", ""), "_blank");

            } else {
                utility.message("无打印数据！");
            }
        }
        $scope.$watch('record.RecordDate',
         function (newVal, oldVal, scope) {
             if (newVal === oldVal) {
                 // 只会在监控器初始化阶段运行
             } else {
                 // 初始化之后发生的变化
                 var h = moment(newVal).hour();
                 if (h >= 8 && h < 20) {
                     $scope.record.ClassType = "D";
                 }
                 else if (h >= 0 && h < 8) {
                     $scope.record.ClassType = "N";
                 }
                 else if (h >= 20 && h < 24) {
                     $scope.record.ClassType = "E";
                 }
             }
         });
        //加载数据
        $scope.loadInfo = function () {
            var d = new Date(), classType = utility.getClassType(d);
            $scope.curUser = utility.getUserInfo();
            $scope.currentPage = 1;
            $scope.record = { IsShow: true, PrintFlag: true, RecordBy: $scope.curUser.EmpNo, RecordNameBy: $scope.curUser.EmpName, ClassType: classType, RecordDate: $filter("date")(d, "yyyy-MM-dd HH:mm:ss"), Content: "" };
        };

        $scope.ShortCuts = [
               "℃", "cc", "cm", "mm", "mg", "kg", "ml", "%", "SPO2", "mmHg", "mg/dl", "R/O",
               "F/u", "GCS", "BW", "BL", "BP", "BS", "AC", "PC", "TPR", "OBS", "BT", "HR",
               "RR", "#", "Supp", "st", "po", "Recheck", "PRN", "Insulin", "I/O", "的", "，", "。",
               "、", "："
        ];
        $scope.recordModify = function (item) {
            $scope.record = angular.copy(item);
            $scope.record.IsShow = false;
        }

        //护理记录列表删除操作
        $scope.delete = function (item) {
            if (confirm("确定删除该交班记录信息吗?")) {
                nursingRecordRes.delete({ id: item.Id }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                    }
                });
            }
        }

        $scope.submit = function () {
            $scope.record.FeeNo = $scope.curResident.FeeNo;
            $scope.record.RegNo = $scope.curResident.RegNo;
            nursingRecordRes.save($scope.record, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                $scope.loadInfo();
            });
        }

        $scope.staffSelected = function (staff) {
            $scope.record.RecordBy = staff.EmpNo;
            $scope.record.RecordNameBy = staff.EmpName;
        }
        $scope.$on("residentChange", function (e, data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = $scope.curResident.FeeNo;
            $scope.options.params.regNo = 0;
            $scope.options.search();
        });

        $scope.btnShortClick = function (e) {
            if (e.target != e.currentTarget) {
                $scope.record.Content += e.target.innerText;
            }
        }
        $scope.PrintPreview = function (order) {
            if ($scope.nursingRecords.length == 0) {
                utility.message("无打印数据！");
                return;
            }

            nursingRecordRes.get({ feeNo: $scope.curResident.FeeNo, regNo: 0, PrintFlag: true }, function (data3) {
                if (data3.Data != null && data3.Data.length != 0) {
                    if ($scope.curResident.FeeNo > 0) {
                        window.open("/Report/Preview?templateName={0}&key={1}&order={2}"
                            .format("P18Report", $scope.curResident.FeeNo,order), "_blank");
                    } else {
                        utility.message("请先选择住民！");
                    }
                }
                else {
                    utility.message("无打印数据！");
                    return;
                }
            });
        };
        $scope.init();
    }]).controller("nursingSingleShiftCtrl", ['$scope', '$http', '$location', '$filter', '$state', '$parse', 'nursingHandoverRes', 'utility',
    function ($scope, $http, $location, $filter, $state, $parse, nursingHandoverRes, utility) {
        $scope.init = function () {
            $scope.loadInfo();
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: nursingHandoverRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.nursingRecords = data.Data;
                    $scope.nursingRecords.forEach(function (item) {
                        item.ResidentName = $scope.curResident.Name;
                    });
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.curResident.FeeNo,
                    regNo: $scope.curResident.RegNo
                }
            }
        }

        $scope.loadInfo = function () {

            var d = new Date(), starDate = $filter("date")(d, "yyyy-MM-dd HH:mm:ss"),
            classType = utility.getClassType(d), txts = $(".txtInput").focusin(function () {
                $scope.curInput = this;
            });
            $scope.currentPage = 1;
            $scope.curUser = utility.getUserInfo();
            switch (classType) {
                case "D":
                    $scope.record = {
                        Recordby_D: $scope.curUser.EmpNo,
                        Recdate_D: starDate
                    };
                    txts.eq(0).focusin();
                    break;
                case "E":
                    $scope.record = {
                        Recordby_E: $scope.curUser.EmpNo,
                        Recdate_E: starDate
                    };
                    txts.eq(1).focusin();
                    break;
                case "N":
                    $scope.record = {
                        Recordby_N: $scope.curUser.EmpNo,
                        Recdate_N: starDate
                    };
                    txts.eq(2).focusin();
                    break;
            }

            angular.extend($scope.record, {
                Content_D: "",
                Content_N: "",
                Content_E: ""
            });
        };

        $scope.ShortCuts = [
                 "℃", "cc", "cm", "mm", "mg", "kg", "ml", "%", "SPO2", "mmHg", "mg/dl", "R/O",
                 "F/u", "GCS", "BW", "BL", "BP", "BS", "AC", "PC", "TPR", "OBS", "BT", "HR",
                 "RR", "#", "Supp", "st", "po", "Recheck", "PRN", "Insulin", "I/O", "的", "，", "。",
                 "、", "："
        ];

        $scope.staffSelectedD = function (staff) {
            $scope.record.Recordby_D = staff.EmpNo;
        }
        $scope.staffSelectedE = function (staff) {
            $scope.record.Recordby_E = staff.EmpNo;
        }
        $scope.staffSelectedN = function (staff) {
            $scope.record.Recordby_N = staff.EmpNo;
        }

        $scope.submit = function () {
            $scope.record.FeeNo = $scope.curResident.FeeNo;
            $scope.record.RegNo = $scope.curResident.RegNo;
            nursingHandoverRes.save($scope.record, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                $scope.record = {};
            });
        }

        $scope.recordModify = function (item) {
            $scope.record = angular.copy(item);
        }

        $scope.btnShortClick = function (e) {
            if (e.target != e.currentTarget) {
                var nm = $($scope.curInput).attr("ng-model"),
                    model = $scope.$eval(nm),
                    getter = $parse(nm);
                getter.assign($scope, model + e.target.innerText);
            }
        }

        $scope.delete = function (Item) {
            if (confirm("确定删除该交班记录信息吗?")) {
                nursingHandoverRes.delete({ id: Item.Id }, function (data) {
                    if (data.ResultCode == 0) {
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                        //$scope.nursingRecords.splice($scope.nursingRecords.indexOf(Item), 1);
                    }
                });
            }
        }

        $scope.$on("residentChange", function (e, data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = $scope.curResident.FeeNo;
            $scope.options.params.regNo = $scope.curResident.RegNo;
            $scope.options.search();
        });

        $scope.init();

    }]).controller("nursingMultiShiftCtrl", ['$scope', '$http', '$location', '$state', 'nursingRecordRes', 'utility',
    function ($scope, $http, $location, $state, nursingRecordRes, utility) {
        $scope.init = function () {

        }

        $scope.ShortCuts = {
            a1: ["℃", "cc", "cm", "mm", "mg", "kg", "ml", "%", "SPO2", "mmHg", "mg/dl", "R/O"],
            a2: ["F/u", "GCS", "BW", "BL", "BP", "BS", "AC", "PC", "TPR", "OBS", "BT", "HR"],
            a3: ["RR", "#", "Supp", "st", "po", "Recheck", "PRN", "Insulin", "I/O", "的", "，", "。"],
            a4: ["、", "："]
        };
        $scope.init();

    }]);
