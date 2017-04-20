/*
创建人:刘美方
创建日期:2016-04-28
说明:标准字典多选控件
*/
angular.module("extentComponent")
.directive("selectMultipleCode", ['$rootScope', '$timeout', '$http', function ($rootScope, $timeout, $http) {
    return {
        resctict: "A",
        require: '?ngModel',
        link: function (scope, element, attrs, ngModel) {
            var id = attrs["id"];
            if (!ngModel || angular.isUndefined(id)) return;
            var tmpDics = $rootScope.TmpDics;
            var dics = $rootScope.Dics;//缩写
            var codeId = attrs["selectMultipleCode"];
            var defaultFirst = attrs["defaultFirst"];
            var ms = $('#' + id).magicSuggest({
                placeholder: '请选择',
                displayField: 'ItemName',
                //style: 'height: 32px;',
                valueField: 'ItemCode',
                editable: false,
                maxSelection:100
            });
            $(ms).on('selectionchange', function (e, m) {
                var arrayVals = this.getValue();
                if (arrayVals.length > 0) {
                    ngModel.$setViewValue(arrayVals.join(','));
                } else {
                    ngModel.$setViewValue("");
                }
            });
            ngModel.$render = function () {
                if (angular.isString(ngModel.$viewValue)) {
                    var oldVal = ms.getValue();
                    var newVal = ngModel.$viewValue.split(',');
                    if (oldVal.sort().toString() != newVal.sort().toString()) {
                        ms.clear();
                        ms.setValue(newVal);
                    }
                    //ms.setValue(ngModel.$viewValue.split(','));
                } else {
                    ms.clear();
                }
                ms.collapse();
            };

            var initControl = function () {
                ms.setData(dics[codeId]);
                if (angular.isDefined(scope.$parent.$eval(defaultFirst))) {
                    if (dics[codeId].length > 0) {
                        ms.setValue(dics[codeId][0].ItemCode.split(','));
                    }
                }
            }
            if (!angular.isDefined(dics[codeId])) {
                dics[codeId] = {};
                if (tmpDics.length === 0) {
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
}]);
