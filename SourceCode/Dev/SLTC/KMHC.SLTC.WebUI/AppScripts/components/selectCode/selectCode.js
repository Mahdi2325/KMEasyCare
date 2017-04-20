/*
创建人:郝元彦
创建日期:2016-03-8
说明:标准字典加载

修改人:肖国栋
修改日期:2016-03-16
说明:添加selectValuer的绑定值改变时，联动显示对应的text功能
修改日期:2016-03-20
说明:联接数据库
*/
angular.module("extentComponent")
.directive("selectCode", ['$rootScope', '$timeout', '$http', function ($rootScope, $timeout, $http) {
    return {
        resctict: "A",
        scope: {
            selectValue: "@selectValue"
        },
        link: function (scope, element, attrs) {

            var tmpDics = $rootScope.TmpDics;
            var dics = $rootScope.Dics;//缩写
            var codeId = attrs["selectCode"];
            var codeValue = scope.selectValue;
            var ngModel = attrs["ngModel"];
            var defaultFirst = attrs["defaultFirst"];
            var notNull = attrs["notNull"];

            var setText = function (element)
            {
                if (angular.isDefined(dics[codeId])) {
                    var codeName = "";
                    angular.forEach(dics[codeId], function (e) {
                        if (e.ItemCode === codeValue) {
                            codeName = e.ItemName;
                        }
                    });
                    element.text(codeName === "" ? codeValue : codeName);
                }
            }

            //跟随ngModel值的变化,改变下来菜单选项
            if (angular.isDefined(ngModel)) {
                scope.$watch(function () {
                    return scope.$parent.$eval(ngModel);
                }, function () {
                    if (element.find("OPTION").first().text() === "") {
                        element.find("OPTION").first().remove();
                    }
                    $(element).val(scope.$parent.$eval(ngModel));
                });
            }
            //根据codeValue值变化，改变下来选项
            if (angular.isDefined(codeValue)) {
                scope.$watch("selectValue", function () {
                    codeValue = scope.selectValue;
                    setText(element);
                });
            }
            var initControl = function ()
            {
                if (element[0].nodeName === "SELECT") {//如果是select注入,填充下拉列表
                    element.empty();
                    if (!angular.isDefined(scope.$parent.$eval(notNull)))
                    {
                        var defaultOption = angular.element("<option>").attr("value", "").text("-- 请选择 --");
                        element.append(defaultOption);
                    }
                    angular.forEach(dics[codeId], function (e) {
                        var option = angular.element("<option>").attr("value", e.ItemCode).text(e.ItemName);
                        element.append(option);
                    });
                   
                    $(element).val("");
                    //判断ngModel是否有值,如果已有值,设置option
                    if (angular.isDefined(scope.$parent.$eval(ngModel))) {
                        $(element).val(scope.$parent.$eval(ngModel));
                    } else if (angular.isDefined(scope.$parent.$eval(defaultFirst))) {
                        if (dics[codeId].length > 0) {
                            $(element).val(dics[codeId][0].ItemCode);
                        }
                    }
                } else {//如果是label,span注入,填充数据值对应的文字
                    if (angular.isDefined(codeValue)) {
                        setText(element);
                    }

                }
            }
            if (!angular.isDefined(dics[codeId])) {
                dics[codeId] = {};
                if (tmpDics.length == 0) {
                    $timeout(function () {
                        $http.post("api/Code", { ItemTypes: tmpDics }).success(function (response, status, headers, config) {
                            $.each(response.Data, function (key, value) {
                                dics[key] = response.Data[key];
                            });
                            tmpDics.splice(0, tmpDics.length);
                        });
                    }, 100);
                }
                tmpDics.push(codeId);
            }
            scope.$watch(function () {
                return dics[codeId].length;
            }, function () {
                if (dics[codeId].length > 0) {
                    initControl();
                }
            });
        }
    }
}]).run(['$rootScope', function ($rootScope) {
    $rootScope.Dics = {};
    $rootScope.Global = {};
    $rootScope.TmpDics = [];
    $rootScope.TmpWord = [];
    $rootScope.TmpLCWord = [];
    $rootScope.DicsWord = [];
}]);

