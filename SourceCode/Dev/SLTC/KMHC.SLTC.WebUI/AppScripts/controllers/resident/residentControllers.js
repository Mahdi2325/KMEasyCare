///创建人:刘美方
///创建日期:2016-02-22
///说明:住民主页信息

///修改人:肖国栋
///修改日期:2016-03-05
///说明:拆分成入住登记和住民信息
angular.module("sltcApp")
    .controller('residentCtrl', ['$scope', '$http', '$state', '$location', 'dictionary', 'utility', 'residentBriefRes', function ($scope, $http, $state, $location, dictionary, utility, residentBriefRes) {
        var id = $state.params.id;

        $scope.init = function () {
            //seajs.use(['/Content/CloudAdmin/js/jquery-ui-1.10.3.custom/css/custom-theme/jquery-ui-1.10.3.custom.min.css'
            //    , '/Content/CloudAdmin/js/bootstrap-daterangepicker/daterangepicker-bs3.css'
            //    , '/Content/CloudAdmin/js/datepicker/picker'
            //    , '/Content/CloudAdmin/js/datepicker/picker.date'
            //    , '/Content/CloudAdmin/js/datepicker/picker.time'], function () {
                    //$(".datepicker").datepicker({
                    //    dateFormat: "yy-mm-dd",
                    //    changeMonth: true,
                    //    changeYear: true
                    //    //maxDate: "0d"
                    //});
                //});
            //生日改变
            $scope.birthDayChange = function (birthDay) {
                $scope.Data.AGE = utility.calculateAge(birthDay);
            };

            $scope.listItem();
        };

        $scope.displayMode = "list";
        $scope.currentItem = {};

        $scope.Keyword = "";

        $scope.listItem = function () {
            $scope.residents = residentBriefRes.query();
        }

        $scope.deleteItem = function (item) {
            item.$delete().then(function () {
                $scope.residents.splice($scope.residents.indexOf(item), 1);
            });
            $scope.displayMode = "list";
        };

        $scope.createItem = function (item) {
            new residents(item).$save().then(function (newItem) {
                $scope.residents.push(newItem);
                $scope.displayMode = "list";
            });
        };

        $scope.updateItem = function (item) {
            item.$save();
            $scope.displayMode = "list";
        };


        $scope.editOrCreate = function (item) {
            $scope.currentItem = item ? item : {};
            $scope.displayMode = "edit";
            $scope.disabled = angular.isDefined($scope.currentItem.id);
        };

        $scope.cancelEdit = function () {
            //还原
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }
            $scope.currentItem = {};

            $scope.displayMode = "list";
        };

        $scope.saveEdit = function (item) {
            if (angular.isDefined(item.id)) {
                $scope.updateItem(item);
            } else {
                $scope.createItem(item);
            }
            $scope.currentItem = {};
        };

        $scope.selectBed = function (data) {
            $scope.currentItem.Floor = data.Floor;
            $scope.currentItem.BedKind = data.BedKind;
            $scope.currentItem.RoomNO = data.RoomNo;
            $scope.currentItem.BedNO = data.Code;
            $scope.currentItem.BedClass = data.BedClass;
            //$scope.SickFlag = data.SickFlag;
            //$scope.RoomFlag = data.RoomFlag;
            //$scope.Protflaf = data.Protflaf;
        };

        $scope.init();
    }])
    .controller("serviceResidentListCtrl", ['$scope', '$http', '$state', '$location', 'personRes', 'floorRes', 'roomRes', function ($scope, $http, $state, $location, personRes, floorRes, roomRes) {
        $scope.mode = $state.params.mode;
        $scope.HealthRecordsUrl = "/angular/HealthRecords";

        if ($scope.mode=="G") {
            $scope.displayModeName = "列表模式";
            $scope.displayMode = "grid";
        } else if ($scope.mode=="L") {
            $scope.displayModeName = "网格模式";
            $scope.displayMode = "list";
        } else {
            $scope.displayModeName = "列表模式";
            $scope.displayMode = "grid";
        }
        
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
        $scope.$watch('Data.FloorName',
       function (newVal, oldVal, scope) {
           if (newVal === oldVal) {
               // 只会在监控器初始化阶段运行
           } else {
               // 初始化之后发生的变化
               roomRes.get({ CurrentPage: 1, PageSize: 100, floorName: $scope.Data.FloorName }, function (data) {
                   $scope.Data.rooms = data.Data;
               });
           }
       });

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: personRes,//异步请求的res
            params: { keyWord: "", FloorName: "", RoomName: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Persons = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };

        $scope.Print = function (item) {
            if (angular.isDefined(item.RegNo)) {
                if (item.RegNo == 0) {
                    utility.message("无打印数据！");
                    return;
                }
                window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("PA002", item.RegNo, "", ""), "_blank");

            } else {
                utility.message("无打印数据！");
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

        $scope.delete = function (id) {
            if (confirm("确定删除该住民信息吗?")) {
                personRes.delete({ id: id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            };
        };

        $scope.init = $scope.reload = function () {
            var keyWord = "";
            if (angular.isDefined($scope.keyword)) {
                keyWord = $scope.keyword;
            }
            var today = new Date();
            var floorName = !$scope.Data.FloorName ? "" : $scope.Data.FloorName;
            var roomName = !$scope.Data.RoomName ? "" : $scope.Data.RoomName;
            personRes.get({ floorName: floorName, roomName: roomName, keyWord: keyWord, currentPage: $scope.currentPage, pageSize: 20 }, function (data) {
                $scope.residents = data.Data;
                $.each($scope.residents, function () {
                    if (this.Birthdate != null) {
                        var birthdate = new Date(this.Birthdate);
                        this.Age = today.getFullYear() - birthdate.getFullYear();
                    }
                    else {
                        this.Age = "";
                    }
                });
                var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {
                    $scope.currentPage = curPage;
                    $scope.search();
                });
            });
        };

        $scope.search = function () {
            var keyWord = "";
            if (angular.isDefined($scope.keyword)) {
                keyWord = $scope.keyword;
            }
            var today = new Date();
            var floorName = !$scope.Data.FloorName ? "" : $scope.Data.FloorName;
            var roomName = !$scope.Data.RoomName ? "" : $scope.Data.RoomName;
            personRes.get({ floorName: floorName, roomName: roomName, keyWord: keyWord, currentPage: $scope.currentPage, pageSize: 20 }, function (data) {
                $scope.residents = data.Data;

                $scope.options.params.FloorName = floorName;
                $scope.options.params.RoomName = roomName;

                if ($scope.keyword != undefined && $scope.keyword != null && $scope.keyword != "") {
                    $scope.options.params.keyword = $scope.keyword;
                }
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();

                $.each($scope.residents, function () {
                    if (this.Birthdate != null) {
                        var birthdate = new Date(this.Birthdate);
                        this.Age = today.getFullYear() - birthdate.getFullYear();
                    }
                    else {
                        this.Age = "";
                    }
                });
                var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {
                    $scope.currentPage = curPage;
                    $scope.search();
                });
            });
        };

        $scope.mode = "G";
        $scope.edit = function (item) {
            $state.go('Person.BasicInfo', { id: item.RegNo, mode: $scope.mode });
            $state.stateName = "Person.BasicInfo";
        };


        $scope.HealthRec = function (item) {
            $location.url($scope.HealthRecordsUrl + "/" + item.FeeNo);
        };

        $scope.ChangeDisplayMode = function () {
            if ($scope.displayModeName == "列表模式") {
                $scope.displayModeName = "网格模式";
                $scope.displayMode = "list";
                $scope.mode="L";
            } else {
                $scope.displayModeName = "列表模式";
                $scope.displayMode = "grid";
                $scope.mode="G";
            };
        };
        
        $scope.init();
    }]);
