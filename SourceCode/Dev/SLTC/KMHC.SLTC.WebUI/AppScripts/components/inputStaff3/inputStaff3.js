/*
创建人:BobDu
创建日期:2016-11-02
说明:自定义员工录入控件
*/
angular.module("extentComponent")
.controller('kmLookupController', ['$scope', '$compile', function ($scope, $compile) {
    $scope.showLookupDialog = function () {
       var html = '<div km-include km-template="AppScripts/components/inputStaff2/inputStaff2.html"' +
       ' km-controller="inputStaff2Controller"  km-include-params="{orgid:\'' + (typeof ($scope.orgid) == "undefined" ? "" : $scope.orgid) + '\',empgroup:\'' + (typeof ($scope.empgroup) == "undefined" ? "" : $scope.empgroup) + '\'}"></div>';
       BootstrapDialog.show({
          title: '<label>员工信息列表</label>',
          type: BootstrapDialog.TYPE_DEFAULT,
          message: html,
          cssClass: 'staff-dialog',
          size: BootstrapDialog.SIZE_WIDE,
          onshow: function (dialog) {
            var obj = dialog.getModalBody().contents();
            $compile(obj)($scope);
          },
          onshown: function (dialog) {
            var listener = $scope.$on('km-on-dialog-close-click', function (event, value) {
              dialog.close();
              event.stopPropagation();
              listener();
            });
          }
      });
   };
}])
.directive("inputStaff", ['empFileRes', function (empFileRes) {
    return {
        resctict: "AE",
        templateUrl: "/AppScripts/components/inputStaff3/inputStaff3.html",
        scope: {
            value: "@value",
            cleardata: "&clearitem",
            callbackFn: "&callback",
            required: "@required",
            orgid: "@orgid",
            empgroup: "@empgroup",
            name: "@name"

        },
        controller: 'kmLookupController',
        link: function (scope, element, attrs) {
            scope.$on('km-on-staff-confirm-click', function (event, value) {
                scope.searchWords = value.EmpName;
                scope.value = value.EmpNO;
                scope.callbackFn({ item: value });
                event.stopPropagation();
            });
            scope.isSelect = false;
            var empGroup = attrs["empGroup"];
            var readonly = attrs["readonly"];
            if (!empGroup) {
                empGroup = "";
            }
            if (readonly == "readonly") {
                element.find("input").attr("readonly", "readonly");
            }

            scope.focus = function () {
                scope.showList = true;
            }

            scope.mouseleave = function () {
                scope.showList = false;
            };

            scope.change = function () {
                search();
                scope.showList = (angular.isDefined(scope.searchWords) && scope.searchWords != "");
                scope.isSelect = false;
                if (!angular.isDefined(scope.searchWords) || scope.searchWords == "") {
                    scope.cleardata();
                }
            }

            scope.blur = function () {
                if (!scope.isSelect) {
                    scope.searchWords = "";
                }
            }

            scope.rowClick = function (item) {
                scope.searchWords = item.EmpName;//输入框显示选择的姓名
                scope.value = item.EmpNO;
                scope.callbackFn({ item: item });//回调函数
                scope.showList = false;//隐藏列表
                scope.isSelect = true;
            };

            function search() {
                if (scope.searchWords != null && scope.searchWords != "") {
                    if (!angular.isDefined(scope.orgid) || scope.orgid == null) {
                        scope.orgid = "";
                    }
                    empFileRes.get({ empNo: scope.searchWords, empName: scope.searchWords, empGroup: empGroup, orgid: scope.orgid, currentPage: 1, pageSize: 10 }, function (response) {
                        scope.empFiles = response.Data;
                    });
                };
            }
           
            //监控传入值的改变,同步关键字显示
            scope.$watch("value", function (newValue) {
                if (angular.isDefined(newValue) && newValue != "") {
                    empFileRes.get({ empNo: newValue, orgid: scope.orgid }, function (data) {
                        if (data.Data.length > 0) {
                            scope.searchWords = data.Data[0].EmpName;
                            scope.isSelect = true;
                        }
                        else {
                            scope.searchWords = newValue;
                            scope.isSelect = true;
                        }
                    });
                } else {
                    if (newValue != undefined) {
                        scope.searchWords = newValue;
                    }
                }
            });

            var rowNo = -1;
            scope.keydown = function (e) {
                if (e.keyCode == 9) {
                    scope.showList = false;
                }
                var tElement = $("#tb tbody tr"), indis = -1, len = tElement.length;

                //Up,Down事件的标识代码
                if ((e.keyCode == 38 && (rowNo = ((rowNo == 0 || rowNo == -1) ? len : rowNo)) > -2) || (e.keyCode == 40 && (indis = 1) && (rowNo = (rowNo == len - 1 ? -1 : rowNo)) > -2)) {

                    for (var k = 0; k < len; k++) {
                        tElement[k].style.backgroundColor = "lightgray";
                    }
                    tElement[rowNo += indis].style.backgroundColor = "#FFFFFF";  
                }
                //Enter事件的标识代码
                if (e.keyCode == 13 && rowNo != -1) {
                    for (var k = 0; k < len; k++) {
                        if (rowNo == k) {
                            tElement[k].click();
                        }
                    }
                }
            }


        }
    }
}]);

