/*
创建人:郝元彦
创建日期:2016-02-29
说明:自定义住民录入控件
*/
angular.module("extentComponent")
.directive("inputResident", ['residentBriefRes', function (residentBriefRes) {
    return {
        resctict: "E",
        templateUrl: "/AppScripts/components/inputResident/inputResident.html",
        scope: {
            value: "@value",
            callbackFn: "&callback",
            required: "@required"
        },
        controller: ['$scope', function ($scope) {
            $scope.searchWords = "";
            $scope.IpdFlag = "I";
            $scope.change = function () {
                
                $scope.showList = (angular.isDefined($scope.searchWords) && $scope.searchWords != "");
                if ($scope.showList) {
                    residentBriefRes.get({ keyWord: $scope.searchWords, ipdFlag: $scope.IpdFlag, currentPage: 1, pageSize: 10 }, function (data) {
                    
                        $scope.residents = data.Data;
                    });
                }
            }


            $scope.rowCick = function (item) {
                $scope.value = this.FeeNo;
                $scope.searchWords = item.Name;//输入框显示选择的编码
                $scope.callbackFn({ item: item });//回调函数
                $scope.showList = false;//隐藏列表
            };


            //根据关键字过滤结果
            //$scope.filterItems = function (item) {

            //    return (angular.isDefined(item.Name) && item.Name.indexOf($scope.searchWords) >= 0)
            //          ||
            //          (angular.isDefined(item.Bunk) && item.Bunk.indexOf($scope.searchWords) >= 0)
            //          ||
            //          (angular.isDefined(item.id) && item.id.indexOf($scope.searchWords) >= 0)
                

            //}
            //监控传入值的改变,同步关键字显示
            //$scope.$watch("value", function (newValue) {
                //$scope.searchWords = newValue;
            //});

            //按键盘
            var rowNo= -1;
            document.onkeydown = function (event)
            {
                var tElement = $("#tb tbody tr"), indis = -1, len = tElement.length;
                
                //Up,down事件的标识代码
                if ((event.keyCode == 38 && (rowNo = ((rowNo == 0 || rowNo == -1) ? len : rowNo))>-2) || (event.keyCode == 40 && (indis = 1) && (rowNo = (rowNo == len - 1 ? -1 : rowNo)) > -2)) {

                    for (var k = 0; k < len; k++) {
                        tElement[k].style.backgroundColor = "lightgray";
                    }
                    tElement[rowNo += indis].style.backgroundColor = "#FFFFFF";
                }

                //Enter事件的标识代码
                if (event.keyCode == 13 && rowNo!= -1) {
                    for (var k = 0; k < len; k++) {
                        if (rowNo == k) {
                            tElement[k].click();
                        }
                    }
                }
            }
        }]
    }
}]);

