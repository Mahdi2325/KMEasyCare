angular.module('sltcApp')
.controller('lifeRecordCtrl',['$scope', 'utility', 'dictionary', 'liferecordsRes', 'empFileRes', 'floorRes', 'roomRes',
function ($scope, utility, dictionary, liferecordsRes, empFileRes, floorRes, roomRes) {
    $scope.btnShow = true;
    //定义当前页码
    $scope.currentPage = 1;
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

    //获取当前时间
    $scope.Data.RecordDate = new Date().format("yyyy-MM-dd hh:mm:ss");
    $scope.currentDate = new Date().format("yyyy-MM-dd hh:mm:ss");
    $scope.loadLifeRecord = function () {
        var floorName = !$scope.Data.FloorName ? "" : $scope.Data.FloorName;
        var roomName = !$scope.Data.RoomName ? "" : $scope.Data.RoomName;
        var recordDate = $scope.Data.RecordDate == "" ? null : new Date($scope.Data.RecordDate);
        liferecordsRes.get({ floorName: floorName, roomName: roomName, currentPage: $scope.currentPage, pageSize: 10 }, function (data) {
            $scope.RecordBy = utility.getUserInfo();
            $scope.Data.List = data.Data;
            _.forEach($scope.Data.List, function (name,index) {
                $scope.Data.List[index].RecordBy = $scope.RecordBy.EmpNo;
                $scope.Data.List[index].RecordDate = moment().format("YYYY-MM-DD HH:mm:ss");
                console.log(name);
            });
            $.each(data.Data, function (index, item) {
                // item.RecordDate = $scope.Data.RecordDate;
            });
            var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {
                $scope.currentPage = curPage;
                $scope.loadLifeRecord();
            });
          
        });

    }
    
    $scope.saveLifeRecord = function () {
        
        var list = [];
   
        if ($scope.Data.List != undefined) {
            $.each($scope.Data.List, function (index, item) {
                if (item.CheckType) {
                    list.push(item);
                }
            })
            if (list.length > 0) {
                
                liferecordsRes.save(list, function (data) {
                    if (data.ResultCode == 0) {
                        utility.message("保存成功！");
                        $scope.loadLifeRecord();
                    }
                    else {
                        utility.message(data.ResultMessage);
                    }
                })
            }
            else {
                utility.message("数据录入不完整,请依次从日期开始填写！");
            }
        }
    }
    $scope.checkNum = function (input) {
       
        var nubmer = parseInt(input.BodyTemp);
        if (typeof (input.BodyTemp) == "undefined" || input.BodyTemp == "" || input.BodyTemp==null) {
            return;
        }
        if (isNaN(nubmer) || nubmer <= 0 || !(/^(0|\+?[1-9][0-9]*)$/.test(nubmer))) {
            {
                alert("请输入正确的数据，只允许输入正整数!");
                input.BodyTemp = "";
                return false;
            }
        }
    }
    $scope.setDate = function () {
        //currentDate
        if ($scope.Data.List != null) {
            $.each($scope.Data.List, function () {
                this.RecordDate = $scope.currentDate;
            });
        }
    }
    $scope.staffSelected = function (item, info) {
        info.RecordBy = item.EmpNo;
        info.RecordByName = item.EmpName;
    }
}]);
