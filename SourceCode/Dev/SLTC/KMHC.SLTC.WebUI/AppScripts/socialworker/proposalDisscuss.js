angular.module('sltcApp')
.controller("proposalDisscussCtrl", ['$scope', '$http', '$state', 'dictionary', 'proposaldisscussRes',
    function ($scope, $http, $state, dictionary, proposaldisscussRes) {
        var id = $state.params.id;
        $scope.Data = {};
        $scope.init = function () {
         
            
            //加载字典数据
            var dicType = ["Socialworkers"];
            dictionary.get(dicType, function (dic) {
                $scope.Dic = dic;
                for (var name in dic) {
                    if ($scope.Dic.hasOwnProperty(name)) {
                        if ($scope.Dic[name][0] != undefined) {
                            $scope.Detail[name] = $scope.Dic[name][0].Value;
                        }
                    }
                }
            });
            //加载数据
            //$scope.proposaldisscussList = proposaldisscussRes.query();
            proposaldisscussRes.query({}, function (data) {
                $scope.proposaldisscussList = data;
            });
        }
        $scope.currentItem = null;

        $scope.createItem = function (item) {
            new proposaldisscussRes(item).$save().then(function (newItem) {
                $scope.proposaldisscussList.push(newItem);
            });

        };
        //删除一条记录
        $scope.deleteItem = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                item.$delete().then(function () {
                    $scope.proposaldisscussList.splice($scope.proposaldisscussList.indexOf(item), 1);

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
            $("#modalProposal").modal("toggle");
        }
        ///编辑或新增时弹出窗
        $scope.editOrCreate = function (item) {
    
            $scope.currentItem = item ? item : {};
            $("#modalProposal").modal("toggle");
        };
        //窗口关闭操作
        $scope.cancelEdit = function () {
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }

            $scope.currentItem = {};
            $("#modalProposal").modal("toggle");
        };

        $scope.init();
    }]);




