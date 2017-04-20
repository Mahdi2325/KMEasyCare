/*
创建人:张祥
创建日期:2016-04-05
说明: 退住院
*/
angular.module("sltcApp").controller("LeaveNursingCtrl", ['$scope', 'LeaveNursingRes', 'utility', function ($scope, LeaveNursingRes, utility) {
    $scope.Data = {};

    $scope.currentItem = {};
    // 当前住民
    $scope.currentResident = {};
    $scope.buttonShow = false;

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.currentItem = {};
        $scope.listItem($scope.currentResident.FeeNo);//加载当前住民的退住院信息
    }

    $scope.staffSelected = function (item, type) {

        if (type === "Carer") {
            $scope.currentItem.Carer = item.EmpNo;
            $scope.currentItem.CarerName = item.EmpName;
        } else if (type === "NurseNo") {
            $scope.currentItem.NurseNo = item.EmpNo;
            $scope.currentItem.NurseName = item.EmpName;
        }
    }

    $scope.listItem = function (FeeNo) {
        LeaveNursingRes.get({ feeNo: FeeNo }, function (data) {
            $scope.currentItem = data.Data;
            $scope.currentItem.InDate = new Date($scope.currentItem.InDate).format("yyyy-MM-dd");
            $scope.currentItem.OutDate = new Date($scope.currentItem.OutDate).format("yyyy-MM-dd");
            if (data.Data.BedStatus != null) {
                $scope.currentItem.BedStatus = data.Data.BedStatus.trim() == "Disable" ? "结案" : (data.Data.BedStatus.trim() == "Used" ? "入院" : (data.Data.BedStatus.trim() == "Empty" ? "空床" : ""));

            };
            $scope.currentItem.IpdFlagName = data.Data.IpdFlag.trim() == "Disable" ? "结案" : (data.Data.IpdFlag.trim() == "Used" ? "入院" : (data.Data.IpdFlag.trim() == "Empty" ? "退住院" : (data.Data.IpdFlag.trim() == "Empty" ? "结账" : "")));
            if ($scope.currentItem.IpdFlagName == "退住院") {
                $scope.buttonShow = false;
            }
            else {
                $scope.buttonShow = true;
            }
        });

    }

    $scope.saveLeaveNursing = function (item) {
        item.FeeNo = $scope.currentResident.FeeNo;
        LeaveNursingRes.save(item, function (data) {
            $scope.currentItem = {};
            $scope.currentItem = data.Data;
            $scope.currentItem.InDate = new Date($scope.currentItem.InDate).format("yyyy-MM-dd");
            $scope.currentItem.OutDate = new Date($scope.currentItem.OutDate).format("yyyy-MM-dd");
            if (data.Data.BedStatus != null) {
                $scope.currentItem.BedStatus = data.Data.BedStatus.trim() == "Empty" ? "结案" : (data.Data.BedStatus.trim() == "N" ? "入院" : (data.Data.BedStatus.trim() == "Empty" ? "空床" : ""));
            }
            $scope.currentItem.IpdFlagName = data.Data.IpdFlag.trim() == "Empty" ? "结案" : (data.Data.IpdFlag.trim() == "Used" ? "入院" : (data.Data.IpdFlag.trim() == "D" ? "退住院" : (data.Data.IpdFlag.trim() == "N" ? "结账" : "")));
            $scope.buttonShow = false;
            utility.message($scope.currentResident.Name + "退住院成功！");
        });
    }

}]);
