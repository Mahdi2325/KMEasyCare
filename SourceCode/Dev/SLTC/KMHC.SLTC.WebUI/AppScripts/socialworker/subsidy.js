angular.module('sltcApp')
.controller('subsidyCtrl', ['$scope', '$http', '$location', '$state', 'dictionary', 'subsidyrecsRes',
    function ( $scope, $http, $location, $state, dictionary, subsidyrecsRes) {
        var id = $state.params.id;
        $scope.Data = {};
        $scope.nextDate = null;
        $scope.init = function () {

        $scope.subsidys = subsidyrecsRes.query();
        var dicType = ["EvalDateGap", "Socialworkers"];
        dictionary.get(dicType, function (dic) {
            $scope.Dic = dic;
            for (var name in dic) {
                if ($scope.Dic.hasOwnProperty(name)) {
                    if ($scope.Dic[name][0] != undefined) {
                        $scope.Data[name] = $scope.Dic[name][0].Value;
                    }
                }
            }
        });
    };

        $scope.currentItem = null;
        //监听间隔天数变化,以通知下一次申请日期变化
        $scope.$watch('days', function (newValue) {
          
            if (newValue) {
                var d = new Date(Date.parse($scope.currentItem.ApplyDate.replace(/-/g,"/")));
                d.setDate(d.getDate() + newValue.Value * 1);
                $scope.currentItem.NextApplyDate = d.format("yyyy-MM-dd");
            }
        });
    
    $scope.deleteItem = function (item) {
        if(confirm('您确定要删除该条记录吗?')){
            item.$delete().then(function () {
                $scope.subsidys.splice($scope.subsidys.indexOf(item), 1);
            });
        }
    };

    $scope.createItem = function (item) {
        
        new subsidyrecsRes(item).$save().then(function (newItem) {

            $scope.subsidys.push(newItem);
        });
    };
    
    $scope.updateItem = function (item) {
        item.$save();
    };

    $scope.editOrCreate = function (item) {
        
        $scope.currentItem = item ? item : {};
        $("#modalSubsidy").modal("toggle");
    };

    $scope.cancelEdit = function () {
        if ($scope.currentItem && $scope.currentItem.$get) {
            $scope.currentItem.$get();
        }

        $scope.currentItem = {};
        $("#modalSubsidy").modal("toggle");
    };
    $scope.saveEdit = function (item) {

        if (angular.isDefined(item.id)) {
            
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        $('#modalSubsidy').modal('toggle');
    };
    Date.prototype.format = function (format) {
        var date = {
            "M+": this.getMonth() + 1,
            "d+": this.getDate(),
            "h+": this.getHours(),
            "m+": this.getMinutes(),
            "s+": this.getSeconds(),
            "q+": Math.floor((this.getMonth() + 3) / 3),
            "S+": this.getMilliseconds()
        };
        if (/(y+)/i.test(format)) {
            format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
        }
        for (var k in date) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1
                       ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
            }
        }
        return format;
    }
    $scope.init();
}]);

