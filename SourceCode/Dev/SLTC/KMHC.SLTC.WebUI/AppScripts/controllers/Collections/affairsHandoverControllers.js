/*
创建人:张凯
创建日期:2016-03-20
说明: 行政交班  affairsHandoverListCtrl，affairsHandoverEditCtrl
*/
angular.module("sltcApp")
.controller("affairsHandoverListCtrl", ['$scope', '$http', '$location', '$state', 'affairsHandoverRes', 'utility', function ($scope, $http, $location, $state, affairsHandoverRes, utility) {
    $scope.Data = {};
    $scope.searchWords = "";
    $scope.loadPages = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: affairsHandoverRes,//异步请求的res
            params: { RecorderName: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.HandoversList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }

        //affairsHandoverRes.query({ currentPage: 1, pageSize: 10, RecorderName: $scope.searchWords }, function (data) {
        //    $scope.Data.HandoversList = data;
        //});
    }

    $scope.AffairsHandoverDelete = function (item) {
        if (confirm("您确定删除该角色信息吗?")) {
            affairsHandoverRes.delete({ id: item.Id }, function (data) {
                //$scope.options.search();
                $scope.Data.HandoversList.splice($scope.Data.HandoversList.indexOf(item), 1);
                utility.message("删除成功");
            });
        }
    }
    $scope.loadPages();
}])
.controller("affairsHandoverEditCtrl", ['$scope', 'utility', '$http', '$location', '$stateParams', 'affairsHandoverRes', function ($scope, utility,$http, $location, $stateParams, affairsHandoverRes) {
    var empInfo = utility.getUserInfo();
    $scope.Data = {
        RecordBy: empInfo.EmpNo,
        RecorderName: empInfo.EmpName
    }
    if ($stateParams.id) {
        affairsHandoverRes.get({ id: $stateParams.id }, function (data) {
            $scope.Data = data;
        });
    }

    //选择人员
    $scope.staffSelected = function (item, type) {
        if (type === "RecordBy") {
            $scope.Data.RecorderName = item.EmpName;
            $scope.Data.RecordBy = item.EmpNo;
        } else if (type === "ExecuteBy") {
            $scope.Data.ExecutiveName = item.EmpName;
            $scope.Data.ExecuteBy = item.EmpNo;
        }
    }


    $scope.save = function () {
        console.log($scope.Data);
        affairsHandoverRes.save($scope.Data, function (data) {
            $location.url('/angular/AffairsHandoverList');
        });
        var empInfo = utility.getUserInfo();
        $scope.Data = {
            RecordBy: empInfo.EmpNo,
            RecorderName: empInfo.EmpName
        }
    }


}]);
