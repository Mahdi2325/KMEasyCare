/*
创建人:刘美方
创建日期:2016-03-11
说明:自定义客户录入控件
*/
angular.module("extentComponent")
.directive("inputPerson", ['personRes', function (personRes) {
    return {
        resctict: "E",
        templateUrl: "/AppScripts/components/inputPerson/inputPerson.html",
        scope: {
            value: "@value",
            callbackFn: "&callback",
            required: "@required"
        },
        link: function (scope, element, attrs) {
            scope.items = personRes.query();
            scope.focus = function () {
                scope.showList = true;
            }

            scope.keydown = function () {
                if (event.keyCode == 9) {
                    scope.showList = false;
                }
            }

            scope.mouseleave = function () {
                scope.showList = false;
            };

            scope.change = function () {
                scope.showList = (angular.isDefined(scope.searchWords) && scope.searchWords != "");
            }

            scope.rowClick = function (item) {
                scope.searchWords = item.Name;//输入框显示选择的姓名
                scope.value = item.id;
                scope.callbackFn({ person: item });//回调函数
                scope.showList = false;//隐藏列表
            };
            //根据关键字过滤结果
            scope.filterItems = function (item) {
                return ((angular.isDefined(item.IdNo) && item.IdNo.indexOf(scope.searchWords) >= 0) ||
                        (angular.isDefined(item.Name) && item.Name.indexOf(scope.searchWords) >= 0) ||
                        !angular.isDefined(scope.searchWords)
                );
            };

            //监控传入值的改变,同步关键字显示
            scope.$watch("value", function (newValue) {
                if (angular.isDefined(newValue) && newValue != "") {
                    personRes.query({ Id: newValue }).$promise.then(function (data) {
                        if (data.length > 0) {
                            scope.searchWords = data[0].Name;
                        }
                    });
                }
            });
        }
    }
}]);
