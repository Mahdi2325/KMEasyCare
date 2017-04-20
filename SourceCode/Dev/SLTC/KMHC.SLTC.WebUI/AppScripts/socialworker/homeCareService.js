//
angular.module('sltcApp')
.controller("homeCareSvrrecCtrl", ['$scope', '$http', '$state', 'dictionary', 'homeCareSvrrecRes',
    function ($scope, $http, $state, dictionary, homeCareSvrrecRes) {
        var id = $state.params.id;
        $scope.Data = {};
        $scope.init = function () {

            //加载数据
            //$scope.homecaresvrrecList = homeCareSvrrecRes.query();
            homeCareSvrrecRes.query({}, function (data) {
          
                $scope.homecaresvrrecList = data;
            });

            //加载字典数据
            var dicType = ["Socialworkers", "ExcreteHelp", "TrafficMode", "AMActivity", "Participate", "Concentration", "Equipmentuse"];
            dictionary.dictionary(dicType, function (dic) {
                $scope.Dic = dic;
                for (var name in dic) {
                    if ($scope.Dic.hasOwnProperty(name)) {
                        if ($scope.Dic[name][0] != undefined) {
                            $scope.Detail[name] = $scope.Dic[name][0].Value;
                        }
                    }
                }
            });

            $scope.Data.nomalData = [
                { value: '1', text: '1' },
                { value: '2', text: '2' },
                { value: '3', text: '3' },
                { value: '4', text: '4' },
                { value: '5', text: '5' },
                { value: '6', text: '6' },
                { value: '7', text: '7' },
                { value: '8', text: '8' },
                { value: '9', text: '9' },
                { value: '10', text: '10' }
            ];
            $scope.Data.AttendType = [
                { value: '1', text: '出席' },
                { value: '2', text: '请假' }
            ];
            $scope.Data.LeaveReason = [
                { value: '1', text: '请假原因１' },
                { value: '2', text: '请假原因２' }
            ];
            
            $scope.Data.WoundCareReason = [
                { value: '1', text: '伤口原因１' },
                { value: '2', text: '伤口原因２' }
            ];
            
            $scope.Data.LunchMeat = [
                { value: '1', text: '荤食' },
                { value: '2', text: '素食' }
            ]; 
            $scope.Data.VisitHospital = [
                { value: '1', text: '医院一' },
                { value: '2', text: '医院二' }
            ]; 
            $scope.Data.ExcreteHelp = [
                { value: '1', text: '可自理' },
                { value: '2', text: '部分协助' },
                { value: '3', text: '完全协助' }
            ];
            $scope.Data.NoonBreak = [
                { value: '1', text: '有' },
                { value: '2', text: '无' }
            ];
        }

        $scope.currentItem = null;

        $scope.createItem = function (item) {
           
            new homeCareSvrrecRes(item).$save().then(function (newItem) {
                $scope.homecaresvrrecList.push(newItem);
            });

        };
        //删除一条记录 
        $scope.deleteItem = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
           
                item.$delete().then(function () {
                    $scope.homecaresvrrecList.splice($scope.homecaresvrrecList.indexOf(item), 1);

                });
                alert("删除成功！");

            }
        };
        //更新数据一条记录
        $scope.updateItem = function (item) {
            item.$save();
        };
        //新增或编辑窗口的数据保存函数
        $scope.saveEdit = function (item) {
            
            if (angular.isDefined(item.id)) {

                $scope.updateItem(item);
            }
            else {
                $scope.createItem(item);
            }
            alert('存档成功!');
            $("#modalHomeCareService").modal("toggle");
        }
        ///编辑或新增时弹出窗
        $scope.editOrCreate = function (item) {
            
            $scope.currentItem = item ? item : {};
            $("#modalHomeCareService").modal("toggle");
        };
        //窗口关闭操作
        $scope.cancelEdit = function () {
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }

            $scope.currentItem = {};
            $("#modalHomeCareService").modal("toggle");
        };

        $scope.init();
    }]);

