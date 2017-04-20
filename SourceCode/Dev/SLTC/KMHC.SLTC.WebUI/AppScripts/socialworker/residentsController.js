///创建人:杨金高
///创建日期:2016-03-30
///说明:院民主页信息

angular.module("sltcApp")
.controller("personListV2Ctrl", ['$scope', '$http', '$state', '$location', 'utility', 'cloudAdminUi', '$filter', 'residentV2Res', 'empFileRes', 'deptRes', 'floorRes', 'roomRes',
function ($scope, $http, $state, $location, utility, cloudAdminUi, $filter, residentV2Res, empFileRes, deptRes, floorRes, roomRes) {

    //初始化数据源
    $scope.Data = {};
    //获取所有楼层信息
    floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {

        $scope.Data.floors = data.Data;
    });
    //获取所有房间信息
    roomRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
        $scope.Data.rooms = data.Data;
    });
    $scope.$watch('options.params.FloorName',
     function (newVal, oldVal, scope) {
         if (newVal === oldVal) {
             // 只会在监控器初始化阶段运行
         } else {
             // 初始化之后发生的变化
             roomRes.get({ CurrentPage: 1, PageSize: 100, floorName: $scope.options.params.FloorName }, function (data) {
                 $scope.Data.rooms = data.Data;
             });
         }
     });
    $scope.init = function () {
        $(".uniform").uniform();
        cloudAdminUi.initFormWizard();
    }
    $scope.Info = {};
    $scope.IpdFlag = 'I';
    //管制原因
    $scope.cflg = false;
    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: residentV2Res,//异步请求的res
        params: { keyword: "", ipdFlag: $scope.IpdFlag, FloorName: "", RoomName:"" },
        success: function (data) {//请求成功时执行函数
            $scope.Persons = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }
    //根据关键字过滤结果
    $scope.filterItems = function (item) {
        if ($scope.keyword) {
            return (angular.isDefined(item.IdNo) && item.IdNo.indexOf($scope.keyword) >= 0)
             ||
             (angular.isDefined(item.Name) && item.Name.indexOf($scope.keyword) >= 0)
        }
        return true;
    };
    //$scope.search = $scope.reload = function () {

    //    var keyword = "";
    //    if (angular.isdefined($scope.keyword)) {
    //        keyword = $scope.keyword;
    //    }

    //    residentv2res.get({ keyword: keyword, ipdflag: $scope.ipdflag, currentpage: $scope.currentpage, pagesize: 10 }, function (response) {
    //        $scope.persons = response.data;
            
    //        var pager = new pager('pager', $scope.currentpage, response.pagescount, function (curpage) {
    //            $scope.currentpage = curpage;
    //            $scope.search();
    //        });
    //    });
        
    //};
    

    $scope.delete = function (id) {
        if (confirm("确定删除该房间信息吗?")) {
            personRes.delete({ id: id }, function (data) {
                if (data.$resolved) {
                    var whatIndex = null;
                    angular.forEach($scope.Persons, function (cb, index) {
                        if (cb.id === id) {
                            whatIndex = index;
                        }
                    });
                    $scope.Persons.splice(whatIndex, 1);
                    utility.message("删除成功");
                }
            });
        }
    };
    $scope.openDtl = function (item) {

        //if (item.InDate != "" && item.InDate != null) item.InDate = new Date().format("yyyy-MM-dd");
        //if (item.BirthDay != "" && item.BirthDay!=null) item.BirthDay = (new Date().getFullYear() - newDate(item.BirthDay).getFullYear());
        $scope.currentItem = item ? item : {};
        $scope.currentItem.OldBedNo = item.BedNo;
        $scope.currentItem.OldRoomNo = item.RoomNo;
        $scope.currentItem.OldFloor = item.Floor;
        if (item.CtrlFlag = true) $scope.cflg = false;
        $state.go("ResidentRegDtl", { id: item.FeeNo });
        $state.stateName = "ResidentIPD";
        //加载部门
        //deptRes.get({}, function (data1) {    
        //    $scope.Info.Depts = data1.Data;
        //});

       // $("#modalDtl").modal("toggle");
    };
    $scope.searchInfo = function () {
        $scope.options.params.FloorName = !$scope.options.params.FloorName ? "" : $scope.options.params.FloorName;
        $scope.options.params.RoomName = !$scope.options.params.RoomName ? "" : $scope.options.params.RoomName;
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

   


    
   
    //保存当前数据时
    //$scope.init();
    //$scope.GetEmpMember();
}]);
