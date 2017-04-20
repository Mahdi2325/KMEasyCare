/*
 * Author : Dennis yang(杨金高)
 * Date   : 2016-03-06
 * Desc   : InsulinInject(胰岛素注射记录)
 */
angular.module("sltcApp")
.controller("insulinInjectCtrl",['$scope', '$state', 'dictionary', 'insulininjectsRes',
    , function ($scope, $state, dictionary, insulininjectsRes) {
        $scope.Data = {};

        $scope.init = function () {
            //加载数据
            insulininjectsRes.query({}, function (data) {
                $scope.Data.insulininjects = data;
            });
            //加载字典数据
            $scope.init = function () {
                var dicType = ["Socialworkers"];
                dictionary.get(dicType, function (dic) {
                    $scope.Dic = dic;
                    for (var name in dic) {
                        if ($scope.Dic.hasOwnProperty(name)) {
                            if ($scope.Dic[name][0] != undefined) {
                                $scope.Data[name] = $scope.Dic[name][0].Value;
                            }
                        }
                    }
                    $scope.Data.insulininjects = insulininjectsRes.query();
                });
                //$scope.$emit('tabChange', '.PrePayment');

            }
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd",
                changeMonth: true,
                changeYear: true
            });

        }

        //当前操作对象(编辑/删除时传入的item参数)
        $scope.currentItem = null;
        $scope.displayMode = "list";
        var firstLoader = false;

        //添加一条就诊记录
        $scope.createItem = function (item) {
            new insulininjectsRes(item).$save().then(function (newItem) {
                $scope.Data.insulininjects.push(newItem);
            });
            $scope.displayMode = "list";
        };

        //删除一条记录 
        $scope.deleteItem = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
                item.$delete().then(function () {
                    $scope.Data.insulininjects.splice($scope.Data.insulininjects.indexOf(item), 1);

                });
                 alert("删除成功！");
                $scope.displayMode = "list";
            }
        };
        //更新数据一条记录
        $scope.updateItem = function (item) {
            item.$save(); $scope.displayMode = "list";
        };


        //弹出添加或编辑窗体
        $scope.openWin = function (item) {
            $scope.currentItem = item ? item : {};
            $("#insulinInjectModal").modal("toggle");
            $scope.displayMode = "edit";
        };
        //关闭弹出窗体
        $scope.closeWin = function () {
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }
            $scope.currentItem = {};
            $("#insulinInjectModal").modal("toggle");
            $scope.displayMode = "list";
        };
        //提交表单数据
        $scope.submit = function (item) {
            if (angular.isDefined(item.id)) {
                $scope.updateItem(item);
            } else {
                $scope.createItem(item);
            }
            alert("存档成功!");
            $("#insulinInjectModal").modal("toggle");
        };
        //加载页面字典数据
        $scope.loadDict = function () {
            $scope.Data.BoseValue = [
                { value: '1', text: '1' },
                { value: '2', text: '2' },
                { value: '3', text: '3' },
                { value: '4', text: '4' }
            ];
            $scope.Data.BodyPart = [
                { value: '1', text: 'A' },
                { value: '2', text: 'B' },
                { value: '3', text: 'C' },
                { value: '4', text: 'D' },
                { value: '5', text: 'E' },
                { value: '6', text: 'F' },
                { value: '7', text: 'G' },
                { value: '8', text: 'H' }
            ];
            $scope.Data.BodyPart_Num = [
                { value: '1', text: '1' },
                { value: '2', text: '2' },
                { value: '3', text: '3' },
                { value: '4', text: '4' },
                { value: '5', text: '5' },
                { value: '6', text: '6' },
                { value: '7', text: '7' },
                { value: '8', text: '8' }
            ];
        };
        $scope.init();
        $scope.loadDict();
    }]);

//angular.module("sltcApp")
//.controller("insulinInjectCtrl", function ($scope, $state, dictionary, insulininjectsRes) {
//    $scope.Data = {};
//    $scope.init = function () {
//        var dicType = ["PaymentType", "PaymentWay", "AssistInput"];
//        dictionary.get(dicType, function (dic) {
//            $scope.Dic = dic;
//            for (var name in dic) {
//                if ($scope.Dic.hasOwnProperty(name)) {
//                    if ($scope.Dic[name][0] != undefined) {
//                        $scope.Data[name] = $scope.Dic[name][0].Value;
//                    }
//                }
//            }
//            $scope.Data.insulininjects = insulininjectsRes.query();
//        });
//        //$scope.$emit('tabChange', '.PrePayment');

//    }

//    $scope.displayMode = "list";
//    $scope.currentItem = null;

//    $scope.Keyword = "";

//    $scope.deleteItem = function (item) {
//        item.$delete().then(function () {
//            $scope.Data.insulininjects.splice($scope.Data.insulininjects.indexOf(item), 1);
//        });
//        $scope.displayMode = "list";
//    };

//    $scope.createItem = function (item) {
//        new insulininjectsRes(item).$save().then(function (newItem) {
//            $scope.Data.insulininjects.push(newItem);
//            $scope.displayMode = "list";
//        });
//    };

//    $scope.updateItem = function (item) {
//        item.$save();
//        $scope.displayMode = "list";
//    };


//    $scope.openWin = function (item) {
//        $scope.currentItem = item ? item : {};
//        $scope.displayMode = "edit";
//    };

//    $scope.closeWin = function () {
//        //还原
//        if ($scope.currentItem && $scope.currentItem.$get) {
//            $scope.currentItem.$get();
//        }
//        $scope.currentItem = {};

//        $scope.displayMode = "list";
//    };

//    $scope.submit = function (item) {
//        if (angular.isDefined(item.id)) {
//            $scope.updateItem(item);
//        } else {
//           // item.ResidentId = residentId;
//            //item.ResidentName = residentId;
//            $scope.createItem(item);
//        }
//    };
//    $scope.init();

//});

