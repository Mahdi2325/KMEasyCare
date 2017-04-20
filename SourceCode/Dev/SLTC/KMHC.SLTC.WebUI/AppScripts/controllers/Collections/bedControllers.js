/*
创建人: 张正泉
创建日期:2016-02-20
说明:床位管理
*/
angular.module("sltcApp")
    .controller("bedListCtrl", ['$scope', '$http', '$location', '$state', 'bedRes', 'utility', function ($scope, $http, $location, $state, bedRes, utility) {
        $scope.init = function () {
            $scope.Data = {};

            //$scope.$watch("keyword", function (newValue) {
            //    if (newValue) {
            //        bedRes.query({}, function (data) {
            //            $scope.Data.beds = data;
            //        });
            //    }
            //});

            $scope.options = {
                //buttons: [],//需要打印按钮时设置
                ajaxObject: bedRes,//异步请求的res,
                params: { keyWords: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.beds = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }
        };

        $scope.delete = function (item) {
            if (item.ResidentName == null || item.ResidentName == "") {
                if (confirm("确定删除该床位信息吗?")) {
                    bedRes.delete({ id: item.BedNo }, function (data) {
                        //$scope.options.search();
                        $scope.Data.beds.splice($scope.Data.beds.indexOf(item), 1);
                        utility.message("删除成功");
                    });
                }
            }
            else {
                utility.message("该床位已有住民无法删除");
            }
        };


        $scope.search = $scope.reload = function () {
            bedRes.get($scope.options.pageInfo, function (req) {
                $scope.Data.beds = req.Data;
                $scope.options.sumInfo = { RecordsCount: req.RecordsCount, PagesCount: req.PagesCount };
                $scope.options.renderPage = $scope.options.pageInfo.CurrentPage;
            });
        };

        $scope.goResident = function (id) {
            $state.go('Person.BasicInfo', { id: id });
            $state.stateName = "ServiceResidentList";
        }

        $scope.init();
    }])
    .controller("bedEditCtrl", ['$scope', '$http', '$location', '$stateParams', 'bedRes', 'floorRes', 'roomRes', 'deptRes', 'utility',
function ($scope, $http, $location, $stateParams, bedRes, floorRes, roomRes, deptRes, utility) {
    $scope.Data = {};
    $scope.BedNo = "null";
    $scope.Data.bed = {};
    $scope.Data.bed.BedKind = "001";
    $scope.Data.bed.BedType = "001";
    $scope.Data.bed.Prestatus = "N";
    $scope.Data.bed.InsbedFlag = "N";
    $scope.Data.bed.SexType = "001";
    deptRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
        $scope.Data.depts = data.Data;
        $scope.Data.bed.DeptNo = $scope.Data.depts[0].DeptNo
    });
    floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
        $scope.Data.floors = data.Data;
        $scope.Data.bed.Floor = $scope.Data.floors[0].FloorId
    });

    $scope.init = function () {

        if ($stateParams.id) {
            bedRes.get({ id: $stateParams.id }, function (data) {
                $scope.Data.bed = data;
            });
            $scope.isAdd = false;
        } else {
            $scope.isAdd = true;

        }

        $scope.$watch('Data.bed.Floor', function (newVal, oldVal, scope) {
            if (newVal === oldVal) {
                // 只会在监控器初始化阶段运行
            } else {
                roomRes.get({ CurrentPage: 1, PageSize: 100, floorName: newVal }, function (data) {
                    $scope.Data.rooms = data.Data;
                    //$scope.Data.bed.RoomNo = $scope.Data.rooms[0].RoomNo
                });
            }
        });

        /*roomRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
            $scope.Data.rooms = data.Data;
            //$scope.Data.bed.RoomNo = $scope.Data.rooms[0].RoomNo
        });*/

    };

    $scope.submit = function () {
        if ($scope.isAdd == true) {
            $scope.Data.bed.bedStatus = "Empty";
        }
        bedRes.save($scope.Data.bed, function (data) {
            utility.message("保存成功！");
            $location.url("/angular/bedList");
        });
    };

    $scope.init();

}])
.controller("bedOverViewCtrl", ['$scope', '$http', '$location', '$state', '$compile', 'dictionary', 'roomRes', 'bedRes', 'floorRes', 'utility', 'changeBedService', function ($scope, $http, $location, $state, $compile, dictionary, roomRes, bedRes, floorRes, utility, changeBedService) {
    $scope.filter = { floorId: "", emptyBedNum: "0", sex: "" };
    $scope.rooms = {};
    $scope.floors = {};
    $scope.selectedRoom = {};
    $scope.changeBedModel = {};
    $scope.currentRoomNo = "";
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: roomRes,//异步请求的res
            params: { roomName: "", floorId: $scope.filter.floorId, emptyBedNum: $scope.filter.emptyBedNum, sex: $scope.filter.sex },
            success: function (data) {//请求成功时执行函数
                $scope.rooms = data.Data;
                if ($scope.currentRoomNo != "") {
                    angular.forEach($scope.rooms, function (room) {
                        if (room.RoomNo == $scope.currentRoomNo) {
                            $scope.selectedRoom = room;
                        }
                    })
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 20
            }
        }
    };

    floorRes.get({ CurrentPage: 1, PageSize: 100, floorName: "" }, function (data) {
        $scope.floors = data.Data;
    });

    $scope.selectFloor = function (floorId) {
        $scope.filter.floorId = floorId;
        $scope.query();
    };
    $scope.selectRoom = function (room) {
        $scope.selectedRoom = room;
        $scope.currentRoomNo = room.RoomNo;
    };
    $scope.query = function () {
        $scope.options.params["floorId"] = $scope.filter.floorId;
        $scope.options.params["emptyBedNum"] = $scope.filter.emptyBedNum;
        $scope.options.search();
    };
    $scope.changeBed = function (bed) {
        //utility.message("此功能尚未实现！");
        //add by Duke
        var html = '<div km-include km-template="Views/OrganizationManage/ChangeBed.html" km-controller="changeBedCtrl"></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">更换床位</label>',
            type: BootstrapDialog.TYPE_INFO,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            closable: false,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            },
            buttons: [{
                label: '保存',
                cssClass: 'btn btn-success',
                action: function (dialogRef) {
                    if (changeBedService.BedNo == "") {
                        utility.msgwarning("请先选择更换的床位");
                        return;
                    }

                    $scope.changeBedModel = {};
                    $scope.changeBedModel.OldBedNo = bed.BedNo;
                    $scope.changeBedModel.FeeNo = bed.FEENO;
                    $scope.changeBedModel.SexType = bed.SexType;
                    $scope.changeBedModel.NewBedNo = changeBedService.BedNo;
                    bedRes.changeBed($scope.changeBedModel, function (data) {
                        changeBedService.BedNo = "";
                        $scope.options.search();
                        utility.message("更换成功！");
                        dialogRef.close();
                    })

                }
            }, {
                label: '取消',
                action: function (dialogRef) {
                    changeBedService.BedNo = "";
                    dialogRef.close();
                }
            }]
        });


    };
    //$scope.initFloor();
    $scope.init();

}])

.controller("changeBedCtrl", ['$scope', '$state', '$http', '$stateParams', '$location', 'changeBedService', 'utility', function ($scope, $state, $http, $stateParams, $location, changeBedService, utility) {
    $scope.Data = {};
    $scope.$watch('Data.ReservationBed', function (newValue) {
        if (angular.isDefined(newValue)) {
            changeBedService.BedNo = newValue.id;
        }
    })
}])

.factory("changeBedService", [function () {
    var Data = {};
    Data.BedNo = "";
    return Data;
}])
