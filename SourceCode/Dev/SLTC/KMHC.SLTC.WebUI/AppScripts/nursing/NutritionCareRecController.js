//营养照顾记录单
angular.module("sltcApp").controller("NutritionCareRecCtrl", ['$scope', '$state', 'dictionary', 'NutritionCareRecRes', 'utility',
function ($scope, $state, dictionary, NutritionCareRecRes, utility) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    // 当前住民
    $scope.currentResident = {};
    $scope.curUser = utility.getUserInfo();
    if (typeof ($scope.curUser) != 'undefined') {
        $scope.staffName = $scope.curUser.EmpName;
    }
    $scope.init = function () {
        var myDate = new Date().format("yyyy-MM-dd");
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NutritionCareRecRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.items = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
            ,
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }
    }

    $scope.residentSelected = function (resident) {
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.staffName = $scope.curUser.EmpName;
        }
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo);//加载
        $scope.clear();//清空编辑项
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    };
    $scope.listItem = function (FeeNo) {
        $scope.Data.items = {};
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.feeNo = FeeNo;
        $scope.options.search();
    }
    $scope.init();

    $scope.saveNutritionCateRecEdit = function (item) {
        if (angular.isDefined($scope.staffName)) {
            item.Dietitian = $scope.staffName;
        }
        if (angular.isDefined($scope.NutritionCareForm.$error.required)) {
            for (var i = 0; i < $scope.NutritionCareForm.$error.required.length; i++) {
                if ($scope.NutritionCareForm.$error.required[i].$name == "") {
                    utility.msgwarning("营养师为必填项！");
                }
                else {
                    utility.msgwarning($scope.NutritionCareForm.$error.required[i].$name + "为必填项！");
                }
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.NutritionCareForm.$error.maxlength)) {
            for (var i = 0; i < $scope.NutritionCareForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.NutritionCareForm.$error.maxlength[i].$name + "超过设定长度！");
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
        utility.message($scope.currentResident.Name + "的营养记录单保存成功！");
    };


    $scope.staffSelected = function (item) {
        $scope.staffName = item.EmpName;
        //$scope.currentItem.Dietitian = item.EmpNo;
    }
    $scope.clearStaff = function () {
        if (angular.isDefined($scope.currentItem)) {
            $scope.currentItem.Dietitian = "";
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.staffName = $scope.curUser.EmpName;
            }
        }
    }
    $scope.createItem = function (item) {
        $scope.currentItem.DinnerFreq = ($scope.DinnerFreqPartOne == null ? "" : $scope.DinnerFreqPartOne) + "," + ($scope.DinnerFreqPartTwo == null ? "" : $scope.DinnerFreqPartTwo) + "," + ($scope.DinnerFreqPartOther == null ? "" : $scope.DinnerFreqPartOther);
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;//住院序号
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;//病例号
        NutritionCareRecRes.save($scope.currentItem, function (data) {
            //$scope.Data.items.push(data);
            //Modfied by Duke on 20160815
            //保存成功后刷新历史记录倒叙排列 默认第1页 每页显示10条记录
            $scope.options.pageInfo.CurrentPage = 1;//默认第1页
            $scope.options.pageInfo.PageSize = 10;//每页显示10条记录
            $scope.options.search();//刷新数据
        });
        $scope.clear();
    };

    $scope.updateItem = function (item) {
        item.DinnerFreq = ($scope.DinnerFreqPartOne == null ? "" : $scope.DinnerFreqPartOne) + "," + ($scope.DinnerFreqPartTwo == null ? "" : $scope.DinnerFreqPartTwo) + "," + ($scope.DinnerFreqPartOther == null ? "" : $scope.DinnerFreqPartOther);
        NutritionCareRecRes.save(item, function (data) {
            $scope.clear();
        });
    };

    $scope.clear = function () {
        $scope.currentItem = {}
        $scope.staffName = "";
        //$scope.currentItem.DietPattern = "";
        //$scope.currentItem.salinity = "";
        $scope.DinnerFreqPartOne = "";
        $scope.DinnerFreqPartTwo = "";
        $scope.DinnerFreqPartOther = "";
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.staffName = $scope.curUser.EmpName;
        }
    }

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
        $scope.staffName = $scope.currentItem.Dietitian;
        $scope.DinnerFreqPartOne = $scope.currentItem.DinnerFreq.split(",")[0];
        $scope.DinnerFreqPartTwo = $scope.currentItem.DinnerFreq.split(",")[1];
        $scope.DinnerFreqPartOther = $scope.currentItem.DinnerFreq.split(",")[2];
    };

    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的营养记录单吗?")) {
            NutritionCareRecRes.delete({ id: item.Id }, function () {
                $scope.Data.items.splice($scope.Data.items.indexOf(item), 1);
            });
        }
    };

    $scope.PrintPreview = function () {
        window.open('/DC_Report/PreviewLTC_SocialReport?templateName=DLN1.2&feeNo=' + $scope.currentResident.FeeNo);
    }

    //*********************验证*********************
    //*********************Start*********************
    //正餐
    $scope.$watch('DinnerFreqPartOne', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.DinnerFreqPartOne = "";
            }
            else {
                if (newValue < 0) {
                    $scope.DinnerFreqPartOne = "";
                }
            }
        }
    });
    //点心
    $scope.$watch('DinnerFreqPartTwo', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.DinnerFreqPartTwo = "";
            }
            else {
                if (newValue < 0) {
                    $scope.DinnerFreqPartTwo = "";
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
    //BMI
    $scope.$watch('currentItem.Bmi', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Bmi = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Bmi = "";
                }
            }
        }
    });
    //能量需求
    $scope.$watch('currentItem.Kcal', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Kcal = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Kcal = "";
                }
            }
        }
    });
    //主食类
    $scope.$watch('currentItem.KcalFood', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.KcalFood = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.KcalFood = "";
                }
            }
        }
    });
    //肉鱼豆蛋
    $scope.$watch('currentItem.KcalFish', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.KcalFish = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.KcalFish = "";
                }
            }
        }
    });
    //蔬菜
    $scope.$watch('currentItem.KcalVegetables', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.KcalVegetables = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.KcalVegetables = "";
                }
            }
        }
    });
    //水果类
    $scope.$watch('currentItem.KcalFruit', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.KcalFruit = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.KcalFruit = "";
                }
            }
        }
    });
    //油脂类
    $scope.$watch('currentItem.KcalGrease', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.KcalGrease = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.KcalGrease = "";
                }
            }
        }
    });
    //蛋白质需求
    $scope.$watch('currentItem.Protein', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.Protein = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.Protein = "";
                }
            }
        }
    });
    //管灌需求
    $scope.$watch('currentItem.PipeKcal', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.PipeKcal = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.PipeKcal = "";
                }
            }
        }
    });
    //蛋白质
    $scope.$watch('currentItem.PipeProtein', function (newValue) {
        if (angular.isDefined(newValue)) {
            if (isNaN(newValue)) {
                $scope.currentItem.PipeProtein = "";
            }
            else {
                if (newValue < 0) {
                    $scope.currentItem.PipeProtein = "";
                }
            }
        }
    });
    //*********************End*********************
}]);
