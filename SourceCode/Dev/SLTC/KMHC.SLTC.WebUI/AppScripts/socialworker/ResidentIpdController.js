///创建人:杨金高
///创建日期:2016-03-30
///说明:院民主页信息

angular.module("sltcApp")
.controller("employeeCtrl", ['$scope', '$http', '$location', '$rootScope', '$state', '$compile', 'empFileExtRes', 'roleRes', 'DCDataDicListRes', 'utility', function ($scope, $http, $location, $rootScope, $state, $compile, empFileExtRes, roleRes, DCDataDicListRes, utility) {
    $scope.Data = {};
    var empGroup = $scope.kmIncludeParams.empGroup;

    $scope.loadEmpFile = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: empFileExtRes,//异步请求的res
            params: { empName: "", empGroup: empGroup },
            success: function (data) {//请求成功时执行函数
                $scope.Data.empFiles = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }

    $scope.StaffSelect = function (item) {
        $scope.$parent.StaffSelect(item, empGroup);
    }

    $scope.showResDistribution = function (item) {
        if (item.FeeNoArr == null) {
            utility.msgwarning("还未分配给任何住民");
            return;
        }
        var html = '<div km-include km-template="Views/Home/Person.html" km-controller="personExtendCtrl"  km-include-params="{feeNoArr:\'[' + item.FeeNoArr + ']\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">住民</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            cssClass: 'staff-dialog',
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
    $scope.loadEmpFile();
}])

.controller("residentV2Ctrl", ['$scope', '$http', '$state', '$location', '$compile', 'utility', 'cloudAdminUi', '$filter', 'residentV2Res', 'empFileRes', 'deptRes',
    function ($scope, $http, $state, $location,$compile, utility, cloudAdminUi, $filter, residentV2Res, empFileRes, deptRes) {
        var id = $state.params.id;
        $scope.currentItem = {};
        $scope.hide = function () {
            $scope.dialog.close();
        }
        $scope.StaffSelect = function (item, empGroup) {
            switch (empGroup) {
                case $scope.enum.careerType.Nurse:
                    $scope.currentItem.NurseNo = { id: item.EmpNo, name: item.EmpName };
                    break;
                case $scope.enum.careerType.Carer:
                    $scope.currentItem.Carer = { id: item.EmpNo, name: item.EmpName };
                    break;
                case $scope.enum.careerType.Doctor:
                    $scope.currentItem.Doctor = { id: item.EmpNo, name: item.EmpName };
                    break;
                case $scope.enum.careerType.Physiotherapist:
                    $scope.currentItem.Physiotherapist = { id: item.EmpNo, name: item.EmpName };
                    break;
            }

            $scope.hide();
        }
        $scope.showNurse = function (empGroup) {
            var html = '<div km-include km-template="Views/Socialworker/Employee.html"' +
            ' km-controller="employeeCtrl"  km-include-params="{empGroup:\'' + empGroup + '\'}"></div>';
                $scope.dialog= BootstrapDialog.show({
                title: '<label>员工信息列表</label>',
                type: BootstrapDialog.TYPE_DEFAULT,
                message: html,
                cssClass: 'staff-dialog',
                size: BootstrapDialog.SIZE_WIDE,
                onshow: function (dialog) {
                    var obj = dialog.getModalBody().contents();
                    $compile(obj)($scope);
                }
               
            });
        }

        $scope.init = function () {
            residentV2Res.get({ id: id }, function (response) {
                if (response.Data[0] != null) {
                    $scope.currentItem = response.Data[0];
                    $scope.currentItem.OldBedNo = response.Data[0].BedNo;
                    $scope.currentItem.OldRoomNo = response.Data[0].RoomName;
                    $scope.currentItem.OldFloor = response.Data[0].FloorName;
                    $scope.currentItem.BedNo = { id: response.Data[0].BedNo, name: response.Data[0].BedNo };
                    $scope.currentItem.NurseNo = { id: response.Data[0].NurseNo, name: response.Data[0].NurseName };
                    $scope.currentItem.Carer = { id: response.Data[0].Carer, name: response.Data[0].CarerName };
                    $scope.currentItem.Doctor = { id: response.Data[0].Doctor, name: response.Data[0].DoctorName };
                    $scope.currentItem.Physiotherapist = { id: response.Data[0].Physiotherapist, name: response.Data[0].PhysiotherapistName };
                    $scope.currentItem.Age = (new Date().getFullYear() - newDate(response.Data[0].BirthDay).getFullYear());
                    if ($scope.currentItem.CtrlFlag == false) {
                        $scope.cflg = true;
                    }

                    if ($scope.currentItem.InDate == "" && $scope.currentItem.InDate == null) {
                        $scope.currentItem.InDate = new Date().format("yyyy-MM-dd");
                    }
                    if ($scope.currentItem.BirthDay == "" && $scope.currentItem.BirthDay == null) {
                        $scope.currentItem.BirthDay = (new Date().getFullYear() - newDate($scope.currentItem.BirthDay).getFullYear());
                    }
                }
            });
            //获取下拉字典数据
            //$scope.GetEmpMember = function () {
            empFileRes.get({ empNo: '', empName: '', empGroup: '', currentPage: 0, pageSize: 0 }, function (response) {
                var dest = getEmpMemberByGroup(response.Data);
                if (!isEmpty(dest)) {
                    for (var i = 0; i < dest.length; i++) {
                        if (dest[i].EmpGroup == "001") {
                            $scope.NurseNoData = dest[i].data;
                        } else if (dest[i].EmpGroup == "002") {
                            $scope.CarerData = dest[i].data;
                        } else if (dest[i].EmpGroup == "003") {
                            $scope.NutritionistData = dest[i].data;
                        } else if (dest[i].EmpGroup == "005") {
                            $scope.PhysiotherapistData = dest[i].data;
                        } else if (dest[i].EmpGroup == "006") {
                            $scope.DoctorData = dest[i].data;
                        }
                    }
                }
            });
            //}
        }

        $scope.changCF = function (item) {
            if (item == true) $scope.cflg = false;
            else {
                $scope.cflg = true;
                currentItem.CtrlReason = '';
            }
        }

        $scope.updateResident = function (newItem) {
            if (angular.isDefined($scope.dataForm.$error.required)) {
                for (var i = 0; i < $scope.dataForm.$error.required.length; i++) {
                    utility.msgwarning($scope.dataForm.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.dataForm.$error.kmRequired)) {
                for (var i = 0; i < $scope.dataForm.$error.kmRequired.length; i++) {
                    utility.msgwarning($scope.dataForm.$error.kmRequired[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.dataForm.$error.maxlength)) {
                for (var i = 0; i < $scope.dataForm.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.dataForm.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.dataForm.$error.number)) {
                for (var i = 0; i < $scope.dataForm.$error.number.length; i++) {
                    utility.msgwarning($scope.dataForm.$error.number[i].$name + "格式不正确！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            newItem.BedNo = newItem.BedNo.id;
            newItem.NurseNo = newItem.NurseNo.id;
            newItem.Carer = newItem.Carer.id;
            newItem.Doctor = newItem.Doctor.id;
            newItem.Physiotherapist = newItem.Physiotherapist.id;
            residentV2Res.save(newItem, function (data) {
                if (angular.isDefined(newItem.FeeNo)) {

                    utility.message("资料更新成功！");
                }
                else {
                    utility.message("资料更新失败！");
                }
            });
            // $location.url("/angular/ResidentListV2/");
            //$state.go('ResidentListV2');
            $state.go('ResidentListV2', null, {
                reload: true
            });
            //$('#modalDtl').modal('hide');
        }
        //重新安排床位
        $scope.chooseBed = function () {
            $('#modalBed').modal({
                backdrop: true,
                keyboard: true,
                show: true
            });
        };
        $scope.selectBed = function (item) {
            $scope.currentItem.FloorName = item.FloorName;
            $scope.currentItem.RoomName = item.RoomName;
            $scope.currentItem.Floor = item.Floor;
            $scope.currentItem.RoomNo = item.RoomNo;
            $scope.currentItem.BedKind = item.BedKind;
            $scope.currentItem.BedClass = item.BedClass;
            $scope.currentItem.BedType = item.BedType;
            $scope.currentItem.Area = item.Area;
            $scope.currentItem.RoomType = item.RoomType;
            $scope.currentItem.SexType = item.SexType;
            $scope.currentItem.DeptNo = item.DeptNo;
        }
        $scope.$on("chooseBed", function (event, data) {

            if (data.BedStatus == "Empty") {
                $scope.currentItem.FloorName = data.FloorName;
                $scope.currentItem.RoomName = data.RoomName;
                $scope.currentItem.Floor = data.Floor;
                $scope.currentItem.RoomNo = data.RoomNo;
                $scope.currentItem.BedNo = data.BedNo;
                $scope.currentItem.BedKind = data.BedKind;
                $scope.currentItem.BedClass = data.BedClass;
                $scope.currentItem.BedType = data.BedType;
                $scope.currentItem.Area = data.Area;
                $scope.currentItem.RoomType = data.RoomType;
                $scope.currentItem.SexType = data.SexType;
                $scope.currentItem.DeptNo = data.DeptNo;
                $('#modalBed').modal('hide');
            }

        });
        $scope.cancelEdit = function () {
            //if ($scope.currentItem && $scope.currentItem.$get) {
            //    $scope.currentItem.$get();
            //}

            //$scope.currentItem = {};
            //$("#modalDtl").modal("toggle");
            $state.go('ResidentListV2', null, {
                reload: true
            });
        };
        $scope.init();
    }]);


