angular.module('sltcApp')
.controller("familyDiscussrecCtrl", function ($scope, $state, dictionary, familyDiscussrecRes) {
    
    var id = $state.params.id;
    $scope.Data = {};
    $scope.init = function () {
        //查询数据库数据
        familyDiscussrecRes.query({}, function (data) {
            $scope.familyDiscussrecs = data;
        });
        
        //加载字典数据
        //var dicType = ["Socialworkers"];
        //dictionary.dictionary(dicType, function (dic) {
        //    $scope.Dic = dic;
        //    for (var name in dic) {
        //        if ($scope.Dic.hasOwnProperty(name)) {
        //            if ($scope.Dic[name][0] != undefined) {
        //                $scope.Detail[name] = $scope.Dic[name][0].Value;
        //            }
        //        }
        //    }
        //});
        $scope.Data.VisitType = [
               { value: '1', text: '到院探视' },
               { value: '2', text: '通信方式' },
               { value: '3', text: '电话探访' }
        ];
        $scope.Data.BloodRelationShip = [
            { value: '1', text: '其他血亲' },
               { value: '2', text: '朋友' },
               { value: '3', text: '社政人员' },
               { value: '4', text: '原生家庭' }
        ];
        $scope.Data.Appellation = [
            { value: '1', text: '女' },
               { value: '2', text: '女婿' },
               { value: '3', text: '子' },
               { value: '4', text: '工作人员' },
               { value: '5', text: '夫或妻' },
               { value: '6', text: '兄弟姐妹' },
               { value: '7', text: '其他' },
               { value: '8', text: '其他亲属' },
               { value: '9', text: '朋友' },
               { value: '10', text: '侄女或外甥女' },
               { value: '11', text: '侄子或外甥' },
               { value: '12', text: '个案之父亲' },
               { value: '13', text: '个案之母亲' },
               { value: '14', text: '孙女或曾孙女' },
               { value: '15', text: '孙子或曾孙子' },
               { value: '16', text: '媳妇' },
               { value: '17', text: '监护人' },
               { value: '18', text: '养女' },
               { value: '19', text: '养子' }
        ];
    };
    $scope.currentItem = null;

    $scope.createItem = function (item) {

        new familyDiscussrecRes(item).$save().then(function (newItem) {
            $scope.familyDiscussrecs.push(newItem);
        });

    };
    //删除一条记录 
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该条记录吗?")) {

            item.$delete().then(function () {
                $scope.familyDiscussrecs.splice($scope.familyDiscussrecs.indexOf(item), 1);

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
        $("#modalFamilySupport").modal("toggle");
    }
    ///编辑或新增时弹出窗
    $scope.openWin = function (item) {
        
        $scope.currentItem = item ? item : {};
        $("#modalFamilySupport").modal("toggle");
    };

    //窗口关闭操作
    $scope.cancelEdit = function () {
        if ($scope.currentItem && $scope.currentItem.$get) {
            $scope.currentItem.$get();
        }

        $scope.currentItem = {};
        $("#modalFamilySupport").modal("toggle");
    };
    $scope.init();
});
