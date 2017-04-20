/*
创建人:张祥
创建日期:2016-03-21
说明: 医师评估
*/
angular.module("sltcApp")
.controller("PhysicianAssCtrl", ['$scope', '$state', 'dictionary', 'PhysicianAssRes', 'utility', 'empFileResGet',
function ($scope, $state, dictionary, PhysicianAssRes, utility, empFileResGet) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.curUser = utility.getUserInfo();
    if (typeof ($scope.curUser) != 'undefined') {
        $scope.currentItem = { DocName: $scope.curUser.EmpNo };
    }

    // 当前住民
    $scope.currentResident = {};
    $scope.buttonShow = false;

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: PhysicianAssRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.docRevList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }
    }

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo);//加载
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { DocName: $scope.curUser.EmpNo };
        }
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }

        empFileResGet.get({ regon: resident.RegNo }, function (data) {
            if (data.Data != null) {
                $scope.currentItem.Height = data.Data.Height;
                $scope.currentItem.Weight = data.Data.Weight;
            }

        });
    }

    $scope.listItem = function (FeeNo) {
        $scope.Data.docRevList = {};
        $scope.options.params.feeNo = FeeNo;
        $scope.options.search();
    }


    //删除医师评估记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的医师评估记录吗?")) {
            PhysicianAssRes.delete({ id: item.Id }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
            });
        }
    };

    $scope.staffSelected = function (item) {
        $scope.currentItem.DocName = item.EmpNo;
        $scope.currentItem.DocActName = item.EmpName;
    }

    $scope.createItem = function (item) {
        if (item.SleepPills == "1") {
            $scope.currentItem.SleepPills = true;
        }
        else if (item.SleepPills == "0") {
            $scope.currentItem.SleepPills = false;
        }
        else {
            $scope.currentItem.SleepPills = "";
        }

        if (item.WoundFlag == "1") {
            $scope.currentItem.WoundFlag = true;
        }
        else if (item.WoundFlag == "0") {
            $scope.currentItem.WoundFlag = false;
        }
        else {
            $scope.currentItem.WoundFlag = "";
        }

        //新增医师评估记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;//住院序号
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;//病例号
        PhysicianAssRes.save($scope.currentItem, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
        });
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { DocName: $scope.curUser.EmpNo };
        }
    };

    $scope.updateItem = function (item) {
        if (item.SleepPills == "1")
        {
            $scope.currentItem.SleepPills = true;
        }
        else if (item.SleepPills == "0") {
            $scope.currentItem.SleepPills = false;
        }
        else {
            $scope.currentItem.SleepPills = "";
        }

        if (item.WoundFlag == "1") {
            $scope.currentItem.WoundFlag = true;
        }
        else if (item.WoundFlag == "0") {
            $scope.currentItem.WoundFlag = false;
        }
        else {
            $scope.currentItem.WoundFlag = "";
        }


        //item.$save();
        PhysicianAssRes.save(item, function (data) {
        });
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { DocName: $scope.curUser.EmpNo };
        }
    };


    $scope.rowSelect = function (item) {
        $scope.currentItem = item;

        if ($scope.currentItem.SleepPills == true) {
            $scope.currentItem.SleepPills = "1";
        }
        else if ($scope.currentItem.SleepPills == false) {
            $scope.currentItem.SleepPills = "0";
        }
        else {
            $scope.currentItem.SleepPills = "";
        }

        if ($scope.currentItem.WoundFlag == true) {
            $scope.currentItem.WoundFlag = "1";
        }
        else if ($scope.currentItem.WoundFlag == false) {
            $scope.currentItem.WoundFlag = "0";
        }
        else {
            $scope.currentItem.WoundFlag = "";
        }
    };

    $scope.savePhysicianAssEdit = function (item) {


        if (angular.isDefined($scope.PhysicianAssForm.$error.required)) {
            for (var i = 0; i < $scope.PhysicianAssForm.$error.required.length; i++) {
                utility.msgwarning($scope.PhysicianAssForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.PhysicianAssForm.$error.maxlength)) {
            for (var i = 0; i < $scope.PhysicianAssForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.PhysicianAssForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }


        if (angular.isDefined($scope.PhysicianAssForm.$error.number)) {
            for (var i = 0; i < $scope.PhysicianAssForm.$error.number.length; i++) {
                utility.msgwarning($scope.PhysicianAssForm.$error.number[i].$name + "格式不正确！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的医师评估信息保存成功！");
    };


    //*********************验证*********************
    //*********************Start*********************

    //身高
    $scope.$watch('currentItem.Height', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Height = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Height = "";
                }
            }
        }
    });
    //体重
    $scope.$watch('currentItem.Weight', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Weight = "";
            }
            else {
                if (newValue < 0 || newValue > 500) {
                    $scope.currentItem.Weight = "";
                }
            }
        }
    });
    //腰围
    $scope.$watch('currentItem.Waist', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Waist = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Waist = "";
                }
            }
        }
    });
    //臀围
    $scope.$watch('currentItem.Hipline', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Hipline = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Hipline = "";
                }
            }
        }
    });

    //中臂环围
    $scope.$watch('currentItem.ArmSaRound', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.ArmSaRound = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.ArmSaRound = "";
                }
            }
        }
    });
    //*********************End*********************
    $scope.init();
}])


