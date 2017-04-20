/*
创建人:肖国栋
创建日期:2016-03-10
说明:自定义员工录入控件
001	E00.013	护理
002	E00.013	社工
003	E00.013	营养
004	E00.013	职能治疗
005	E00.013	物理治疗
006	E00.013	医师
007	E00.013	心理
008	E00.013	共同
009	E00.013	其他
*/
angular.module("extentComponent")
.directive("inputStaff", ['empFileRes', function (empFileRes) {
    return {
        resctict: "AE",
        templateUrl: "/AppScripts/components/inputStaff/inputStaff.html",
        scope: {
            value: "@value",
            cleardata: "&clearitem",//add by Duke on20160816
            callbackFn: "&callback",
            required: "@required",
            orgid: "@orgid",
            name:"@name"

        },
        link: function (scope, element, attrs) {
            scope.isSelect = false;
            var empGroup = attrs["empGroup"];
            var readonly = attrs["readonly"];
            if (!empGroup)
            {
                empGroup = "";
            }
            if (readonly == "readonly") {
                element.find("input").attr("readonly", "readonly");
            }
            
            scope.focus = function () {
                scope.showList = true;
            }

            //scope.keydown = function (e) {
            //    if (e.keyCode == 9) {
            //        scope.showList = false;
            //    }
            //}

            scope.mouseleave = function () {
                scope.showList = false;
            };

            scope.change = function () {
                search();
                scope.showList = (angular.isDefined(scope.searchWords) && scope.searchWords != "");
                scope.isSelect = false;
                //add by Duke on 20160816
                //如果输入框清空 则同时回调clearitem函数
                if (!angular.isDefined(scope.searchWords) || scope.searchWords == "")
                {
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

            //scope.$watch("orgid", function (orgid) {
                
            //    if (angular.isDefined(orgid) && orgid != null && orgid != "") {
            //        empFileRes.get({ empNo: scope.searchWords, empName: scope.searchWords, empGroup: empGroup,orgid:orgid, currentPage: 1, pageSize: 10 }, function (response) {
            //            scope.empFiles = response.Data;
            //        });
            //    }
            //});

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
            //根据关键字过滤结果
            //scope.filterItems = function (item) {
            //    return (!angular.isDefined(empGroup) || (angular.isDefined(empGroup) && empGroup == item.empGroup)) && (
            //              (angular.isDefined(item.EmpNO) && item.EmpNO.indexOf(scope.searchWords) >= 0)
            //                ||
            //              (angular.isDefined(item.EmpName) && item.EmpName.indexOf(scope.searchWords) >= 0)
            //                ||
            //               !angular.isDefined(scope.searchWords)
            //            )
            //};

            //监控传入值的改变,同步关键字显示
            scope.$watch("value", function (newValue) {
                if (angular.isDefined(newValue) && newValue != "") {
                    empFileRes.get({ empNo: newValue, orgid: scope.orgid }, function (data) {
                        if (data.Data.length > 0) {
                            scope.searchWords = data.Data[0].EmpName;
                            scope.isSelect = true;
                        }
                        else
                        {
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
                var tElement = $("#tb tbody tr"), indis = -1, len=tElement.length;
                
                //Up,Down事件的标识代码
                if ((e.keyCode == 38 && (rowNo = ((rowNo == 0 || rowNo == -1) ? len : rowNo))>-2) || (e.keyCode == 40 && (indis = 1) && (rowNo = (rowNo == len - 1 ? -1 : rowNo))>-2))
                {
                    
                    for (var k = 0; k < len; k++) {
                        tElement[k].style.backgroundColor = "lightgray";
                    }
                    //console.log(e.keyCode + "***" + (rowNo == len - 1).toString() + "***" + rowNo + "***" + indis);
                    tElement[rowNo += indis].style.backgroundColor = "#FFFFFF";
                    //if (rowNo == 0) {
                    //    rowNo = tElement.length;
                    //}
                    
                    //if (tElement[rowNo + indis]) {
                    //    tElement[rowNo += indis].style.backgroundColor = "#FFFFFF";
                    //}
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

