/*
创建人:陈磊
创建日期:2016-03-29
说明:用於入住登记模块 校验入住用户姓名
*/
angular.module("extentComponent")
.directive("inputResidentCheck", ['personRes', function (personRes) {
    return {
        resctict: "E",
        templateUrl: "/AppScripts/components/inputResidentCheck/inputResidentCheck.html",
        scope: {
            searchWords: "=value",
            callbackFn: "&callback",
            required: "@required"
        },
        controller: ['$scope', function ($scope) {
            $scope.searchWords = "";

            $scope.keySearch = function () {
                $scope.showList = false;
                if (angular.isDefined($scope.searchWords) && $scope.searchWords != "") {
                    personRes.get({ residentName: $scope.searchWords, ipdFlag: '' }, function (data) {
                        if (data.Data.length > 0) {
                            $scope.showList = true;
                            $scope.residents = getResidentCheckList(data.Data);
                        }
                    });
                }
            }

            $scope.mouseleave = function () {
                if (angular.isDefined($scope.searchWords) && $scope.searchWords != "") {
                    personRes.get({ residentName: $scope.searchWords, ipdFlag: '', IdNo: '' }, function (data) {
                        if (data.Data.length > 0) {
                            $scope.callbackFn({ IsExistsName: true })
                        } else {
                            $scope.callbackFn({ IsExistsName: false })
                        }
                    });
                }
                $scope.showList = false;
            }

            $scope.KeyPress = function ($event) {
                if (window.event && window.event.keyCode == 13) {
                    window.event.returnValue = false;
                }
            }


            $scope.rowClick = function (item) {
                $scope.value = item.Name;
                $scope.searchWords = item.Name;//输入框显示选择的编码
                $scope.callbackFn({ item: item });//回调函数
                $scope.showList = false;//隐藏列表
            };

            //按键盘
            var rowNo= -1;
            document.onkeydown = function (event)
            {
                var tElement = $("#tb tbody tr"), indis = 1;
                
                //Up事件的标识代码
                if (event.keyCode == 38 || (event.keyCode == 40&&(indis=-1)))
                {
                    for (var k = 0; k < tElement.length; k++) {
                        tElement[k].style.backgroundColor = "lightgray";
                    }
                    if (rowNo == 0) {
                        rowNo = tElement.length;
                    }
                    if (tElement[rowNo + indis]) {
                        tElement[rowNo += indis].style.backgroundColor = "#FFFFFF";
                    }
                }
            

                //Enter事件的标识代码
                if (event.keyCode == 13) {
                    for (var k = 0; k < tElement.length; k++) {
                        if (rowNo == k) {
                            tElement[k].click();
                        }
                    }
                }
            }
        }]
    }
}]);

function getResidentCheckList(arr) {
    var map = {}, dest = [], disDest=[]; 
    for (var i = 0; i < arr.length; i++) {
        var ai = arr[i];
        if (!map[ai.RegNo]) {
            dest.push({ RegNo: ai.RegNo, data: [ai] });
            map[ai.RegNo] = ai;
        } else {
            for (var j = 0; j < dest.length; j++)
            { var dj = dest[j]; if (dj.RegNo == ai.RegNo) { dj.data.push(ai); break; } }
        }
    }
    for (var j = 0; j < dest.length; j++) {
        disDest.push(dest[j].data[0]);
        if (dest[j].data.length > 1) {
            for (var k = 0; k < dest[j].data.length; k++) {
                if (dest[j].data[k].IpdFlag == "I" || dest[j].data[k].IpdFlag == "N") {
                    dest[j].data[0].IpdFlag = dest[j].data[k].IpdFlag;
                }
            }
        } 
    }
    return disDest;
}

