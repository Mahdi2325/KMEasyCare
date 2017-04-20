/*
创建人: 张正泉
创建日期:2016-02-20
说明:房间管理
*/

angular.module("sltcApp")
    .controller("roomListCtrl", ['$scope', '$http', '$location', '$state', 'dictionary', 'roomRes', 'utility', function ($scope, $http, $location, $state, dictionary, roomRes, utility) {
        $scope.init = function() {
            $scope.Data = {};
           
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
        };
        
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
    .controller("roomEditCtrl", ['$scope', '$http', '$location', '$stateParams', 'dictionary', 'floorRes', 'roomRes',
        function ($scope, $http, $location, $stateParams, dictionary,floorRes, roomRes) {
        $scope.init = function() {
            $scope.Data = {};
            $scope.RoomNo = "null"
            //var dicType = ["RoomTypes"];
            //dictionary.get(dicType, function (dic) {
            //    $scope.Dic = dic;
            //});
            //orgRes.get({ CurrentPage: 1, PageSize: 10 }, function (data) {
            //    $scope.Data.orgs = data.Data;
            //});
            floorRes.get({ CurrentPage: 1, PageSize: 10 }, function (data) {
                $scope.Data.floors = data.Data;
            });

            if ($stateParams.id) {
                
                roomRes.get({ id: $stateParams.id }, function(data) {
                    $scope.Data.room = data;
                    $scope.RoomNo = data.RoomNo;
                });
               
                $scope.isAdd = false;
            } else {
                //$scope.Data.room = {
                //    bunkCount: 3,
                //    enableCount: 3,
                //    roomType: "普通"
                //};
                //$scope.$watch("Data.room.roomType", function(newValue) {
                //    if (newValue == "高级") {
                //        $scope.Data.room.bunkCount = 3;
                //        $scope.Data.room.enableCount = 3;
                //    } else {
                //        $scope.Data.room.bunkCount = 6;
                //        $scope.Data.room.enableCount = 6;
                //    }
                //});
                $scope.isAdd = true;
            }
        };

        $scope.submit = function () {
            roomRes.save($scope.Data.room, function (data) {
                $location.url('/angular/roomList');
            });
        };

        $scope.init();
    }]);
