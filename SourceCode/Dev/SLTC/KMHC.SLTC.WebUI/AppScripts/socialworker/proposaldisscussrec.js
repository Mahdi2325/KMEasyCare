angular.module('sltcApp')
.controller("proposalDisscussrecCtrl", ['$scope', '$http', '$state', 'dictionary', 'proposaldisscussrecsRes',
    function ($scope, $http, $state, dictionary, proposaldisscussrecsRes) {
        var id = $state.params.id;
        $scope.Data = {};
        $scope.init = function () {
            
            //加载数据
            $scope.proposaldisscussrecsList = proposaldisscussrecsRes.query();
            //加载字典数据
            var dicType = ["Socialworkers", "MeetingAddr", "RecordBy", "MeetChairman", "GuidTeacher"];
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


        }
        $scope.currentItem = null;

        $scope.createItem = function (item) {
            new proposaldisscussrecsRes(item).$save().then(function (newItem) {
                $scope.proposaldisscussrecsList.push(newItem);
            });

        };
        //删除一条记录
        $scope.deleteItem = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                item.$delete().then(function () {
                    $scope.proposaldisscussrecsList.splice($scope.proposaldisscussrecsList.indexOf(item), 1);

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
            $("#modalProposalDisscussrecs").modal("toggle");
        }
        ///编辑或新增时弹出窗
        $scope.editOrCreate = function (item) {
            
            $scope.currentItem = item ? item : {};
            $("#modalProposalDisscussrecs").modal("toggle");
        };
        //窗口关闭操作
        $scope.cancelEdit = function () {
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }

            $scope.currentItem = {};
            $("#modalProposalDisscussrecs").modal("toggle");
        };

        $scope.init();
    }]);

