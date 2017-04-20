/*
创建人:张祥
创建日期:2016-03-21
说明: 医师巡诊
*/
angular.module("sltcApp")
.controller("PhysicianRoundsCtrl",['$scope', '$state', 'dictionary', 'PhysicianRoundsRes', 'utility',
function ($scope, $state, dictionary, PhysicianRoundsRes, utility) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.currentItem = {};
    // 当前住民
    $scope.currentResident = {};
    $scope.buttonShow = false;

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: PhysicianRoundsRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.docCheckList = data.Data;
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
            $scope.currentItem = { DocNo: $scope.curUser.EmpNo };
        }

        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    $scope.listItem = function (FeeNo) {
        $scope.Data.docCheckList = {};
        $scope.options.params.feeNo = FeeNo;
        $scope.options.search();
    }

    $scope.staffSelected = function (item) {
        $scope.currentItem.DocNo = item.EmpNo;
        $scope.currentItem.DocName = item.EmpName;
    }
    $scope.PrintPreview = function (item) {
        window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("PhReport", item.Id, item.CheckDate, ''), "_blank");
    };


    //删除医师巡诊记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的医师的查房记录吗?")) {
            PhysicianRoundsRes.delete({ id: item.Id }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
            });
        }
    };




    $scope.createItem = function (item) {
      
        //新增医师巡诊记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;//住院序号
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;//病例号

        PhysicianRoundsRes.save($scope.currentItem, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
        });
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { DocNo: $scope.curUser.EmpNo };
        }
    };

    $scope.updateItem = function (item) {
        //item.$save();
        PhysicianRoundsRes.save(item, function (data) {
        });
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { DocNo: $scope.curUser.EmpNo };
        }
    };


    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    };

    $scope.saveDocCheckEdit = function (item) {
        if (angular.isDefined($scope.docCheckfrom.$error.required)) {
            for (var i = 0; i < $scope.docCheckfrom.$error.required.length; i++) {
                utility.msgwarning($scope.docCheckfrom.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.docCheckfrom.$error.maxlength)) {
            for (var i = 0; i < $scope.docCheckfrom.$error.maxlength.length; i++) {
                utility.msgwarning($scope.docCheckfrom.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        if (angular.isDefined($scope.docCheckfrom.$error.pattern)) {
            for (var i = 0; i < $scope.docCheckfrom.$error.pattern.length; i++) {        
                utility.msgwarning($scope.docCheckfrom.$error.pattern[i].$name + "输入格式不对！");
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
        utility.message($scope.currentResident.Name + "的医师查房记录保存成功！");
    };
    //*********************验证*********************
    //*********************Start*********************

    //体温
    $scope.$watch('currentItem.BodyTemp', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.BodyTemp = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.BodyTemp = "";
                }
            }
        }
    });
    //脉搏
    $scope.$watch('currentItem.Pulse', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Pulse = "";
            }
            else {
                if (newValue < 0 ) {
                    $scope.currentItem.Pulse = "";
                }
            }
        }
    });
    //血压
    $scope.$watch('currentItem.Bp', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Bp = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Bp = "";
                }
            }
        }
    });
    //血氧
    $scope.$watch('currentItem.Oxygen', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Oxygen = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Oxygen = "";
                }
            }
        }
    });

    //血糖
    $scope.$watch('currentItem.Bs', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Bs = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Bs = "";
                }
            }
        }
    });
    //*********************End*********************
    $scope.init();
}])
