angular.module("sltcApp")
.controller("bedManagementListCtrl", ['$scope', 'roomRes', function ($scope, roomRes) {
    $scope.Data = {};
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: roomRes,//异步请求的res
            params: { roomName: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.rooms = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }
    $scope.delete = function (item) {
        if (confirm("确定删除该房间信息吗?")) {
            roomRes.delete({ id: item.RoomNo }, function (data) {
                if (data.$resolved) {
                    $scope.Data.rooms.splice($scope.Data.rooms.indexOf(item), 1);
                    utility.message("删除成功");
                }
            });
        }
    };
    $scope.init();

}])

.controller("bedManagementEditCtrl", ['$scope', 'roomRes', 'floorRes', '$stateParams', 'utility', function ($scope, roomRes, floorRes, $stateParams, utility) {

    $scope.BedBasic = {}
    $scope.OrgRoom = {}
    $scope.BedBasicList = []
    $scope.currentItem = {}
    $scope.init = function () {
        $scope.Data = {};
        floorRes.get({ CurrentPage: 1, PageSize: 10 }, function (data) {
            $scope.Data.floors = data.Data;
        });
        if ($stateParams.id) {
            roomRes.get({ id: $stateParams.id }, function (data) {
                if (data.Data != null) {
                    $scope.currentItem = data.Data;
                    if ($scope.currentItem.Bedes != null && $scope.currentItem.Bedes.length > 0) {
                        $scope.BedBasicList = $scope.currentItem.Bedes;
                        $scope.currentItem.TotalBedNumber = $scope.currentItem.Bedes.length;
                    }
                }

            });
            $scope.isAdd = false;
        } else {
            $scope.isAdd = true;
        }
    };

    $scope.init();

    //加载床位列表
    $scope.loadBedList = function (val) {
        $scope.IsReLoadBedList = false;
        if (angular.isNumber(val)) {

            if ($scope.BedBasicList.length > 0) {
                if ($scope.BedBasicList.length == val) {
                    utility.msgwarning("已经加载床位列表");
                    return;
                }
                else {
                    if (confirm("您确定要重新加载覆盖现在床位列表吗?")) {
                        $scope.IsReLoadBedList = true;
                    }
                }
            }

            if ($scope.BedBasicList.length == 0 || $scope.IsReLoadBedList) {
                $scope.BedBasicList = []
                for (var i = 0; i < val; i++) {
                    $scope.BedBasicList.push(angular.copy($scope.BedBasic));
                }
            }

        }
    }
    //添加一床位
    $scope.addOneBed = function () {
        if (!angular.isDefined($scope.currentItem.TotalBedNumber) || $scope.currentItem.TotalBedNumber == "") {
            $scope.currentItem.TotalBedNumber = 0;
        }
        if ($scope.currentItem.TotalBedNumber == $scope.BedBasicList.length) {
            $scope.currentItem.TotalBedNumber = $scope.currentItem.TotalBedNumber + 1;
        }
        else {
            $scope.currentItem.TotalBedNumber = $scope.BedBasicList.length;
            $scope.currentItem.TotalBedNumber = $scope.currentItem.TotalBedNumber + 1;
        }
        $scope.BedBasicList.push(angular.copy($scope.BedBasic));
    }
    //删除一床位
    $scope.delete = function (item) {
        $scope.BedBasicList.splice($scope.BedBasicList.indexOf(item), 1);
        $scope.currentItem.TotalBedNumber = $scope.currentItem.TotalBedNumber - 1;
    }
    //监控床位总数不能为0
    $scope.$watch('currentItem.TotalBedNumber', function (newValue) {
        if (newValue < 0) {
            $scope.currentItem.TotalBedNumber = 0;
            utility.msgwarning("床位总数不能小于0");
        }

    })
    //保存房间床位信息
    $scope.save = function () {
        //校验
        if (angular.isDefined($scope.bedManagementForm.$error.required)) {
            for (var i = 0; i < $scope.bedManagementForm.$error.required.length; i++) {
                utility.msgwarning($scope.bedManagementForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.bedManagementForm.$error.number)) {
            for (var i = 0; i < $scope.bedManagementForm.$error.number.length; i++) {
                utility.msgwarning($scope.bedManagementForm.$error.number[i].$name + "格式错误！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.bedManagementForm.$error.maxlength)) {
            for (var i = 0; i < $scope.bedManagementForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.bedManagementForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if ($scope.BedBasicList.length > 0) {
            for (var i = 0; i < $scope.BedBasicList.length; i++) {
                if (!angular.isDefined($scope.BedBasicList[i].BedNo) || $scope.BedBasicList[i].BedNo == "") {
                    utility.msgwarning("序号为" + (i + 1).toString() + "的床号不能为空！");
                    return;
                }
            }
        }

        if ($scope.BedBasicList.length > 0) {
            for (var i = 0; i < $scope.BedBasicList.length; i++) {
                for (var j = i + 1; j < $scope.BedBasicList.length; j++) {
                    if ($scope.BedBasicList[i].BedNo == $scope.BedBasicList[j].BedNo) {
                        utility.msgwarning("序号为" + (j + 1).toString() + "的床号不能重复！");
                        return;
                    }
                }

            }
        }

        if ($scope.BedBasicList.length != $scope.currentItem.TotalBedNumber) {
            utility.msgwarning("床位总数和床位列表的床位总数不一致");
            return;
        }

        $scope.OrgRoom = $scope.currentItem;
        $scope.OrgRoom.Bedes = $scope.BedBasicList;
        roomRes.saveRoom($scope.OrgRoom, function (data) {
            if (data.ResultCode != -1) {
                utility.message("保存成功！");
                window.location.href = "/angular/BedManagementList";
            }
            else {
                utility.msgwarning(data.ResultMessage);
            }
        })

    }
}])