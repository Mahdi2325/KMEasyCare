 //新住民入住72小时营养筛查表
angular.module("sltcApp")
.controller("NutrtionEvalCtrl", ['$scope', 'dictionary', 'utility', 'NutrtionEvalRes', '$state', function ($scope, dictionary, utility, NutrtionEvalRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;
    $scope.curUser = utility.getUserInfo();
    if (typeof($scope.curUser) != 'undefined') {
        $scope.currentItem = { RECORDBY: $scope.curUser.EmpNo };
    }
    //Add by Duke on 20160815 加分页
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NutrtionEvalRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.RegVisitRecs = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                FeeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                OrgId:-1,
            }
        }
    }


    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前信息
        $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//
        $scope.clear();
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
         
    }
    $scope.change = function () {

      
    }
    $scope.clear = function () {

        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { RECORDBY: $scope.curUser.EmpNo };
        }
    }
    $scope.listItem = function (FeeNo, OrgId) {
        $scope.Data.RegVisitRecs = {};
        //Modfied by Duke on 20160815
        //***Start***
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.FeeNo = FeeNo;
        $scope.options.params.OrgId = OrgId;
        $scope.options.search();

        //NutrtionEvalRes.query({ currentPage: 1, pageSize: 10, FeeNo: FeeNo, OrgId: OrgId }, function (data) {
        //    $scope.Data.RegVisitRecs = data;
        //});
        //***End***
    }
   
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该营养筛查记录吗?")) {
            NutrtionEvalRes.delete({ id: item.ID }, function () {
                $scope.Data.RegVisitRecs.splice($scope.Data.RegVisitRecs.indexOf(item), 1);
                $scope.options.search();
                utility.message($scope.currentResident.Name + "的营养筛查信息删除成功！");
            });
        }
    };

    $scope.CalBMI = function(currentItem)
    {
        $scope.currentItem.BMI = "";
        var heightcalval = (currentItem.HEIGHT / 100);
        var BMIval = currentItem.CURRENTWEIGHT / (heightcalval * heightcalval);
        var fs = BMIval.toString();
        var fp = fs.indexOf('.');
        if (fp > 0) { 
            BMIval=fs.substring(0, fp + 3);
        }
        $scope.currentItem.BMI = (BMIval == "Infinity" ? "" : BMIval);
        if( $scope.currentItem.BMI>100)
        {
            $scope.currentItem.BMI = "";
        }
    }
    $scope.createItem = function (item) {
        //新增记录，得到住民ID
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        NutrtionEvalRes.save($scope.currentItem, function (data) {
            //Modfied by Duke on 20160815
            //***Start***
            //$scope.Data.RegVisitRecs.push(data.Data);
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.search();
            //***End***
        });
        //$scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//
        $scope.clear();
    };

    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.RECORDBY = item.EmpNo;
       // $scope.staffName = item.EmpName;
    }

    $scope.updateItem = function (item) {
        NutrtionEvalRes.save(item, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.search();
            //$scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//
            $scope.clear();
        });
    };

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
        //$scope.staffName = $scope.currentItem.RECORDBY;
    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.ID)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的信息保存成功！");
    };


    $scope.init();

}]);
