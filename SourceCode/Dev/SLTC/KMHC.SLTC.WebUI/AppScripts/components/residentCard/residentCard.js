angular.module("extentComponent")
.directive("residentCard", ['residentBriefRes', 'floorRes', function (residentBriefRes, floorRes) {
    return {
        resctict: "EA",
        templateUrl: "/AppScripts/components/residentCard/residentCard.html",
        scope: {
            //value: "@value",
            iflag: "@ipdFlag",
            callbackFn: "&callback"
        },
        controller: ['$scope', '$attrs', function ($scope, $attrs) {
            $scope.Name = "";
            if ($scope.iflag != undefined) {
                if ($scope.iflag == "O")
                { $scope.IpdFlag = "O"; }
                else
                { $scope.IpdFlag = "I"; }
            }
            else {
                $scope.IpdFlag = "I";
            }
            //$scope.IpdFlag = "O";
            $scope.$watch('FloorId', function (newValue, oldValue, scope) {
                $scope.FilterFloorId = newValue == null ? undefined : newValue;
            });
            $scope.currentResident = {
                Name: "---",
                ImgUrl: "/Images/defaultavatar.png"
            };
            floorRes.get({ floorName: "" }, function (data) {
                $scope.floors = data.Data;
            });
            var readList = function () {
                residentBriefRes.get({ keyWord: "", ipdFlag: "", currentPage: 1, pageSize: 1000 }, function (data) {
                    $scope.residents = data.Data;
                    var FeeNo = $attrs['feeno'];
                    var item = {}; var isExist = false;
                    if (angular.isDefined(FeeNo) && FeeNo != "") {
                        for (var i = 0; $scope.residents.length; i++) {
                            if ($scope.residents[i].FeeNo == FeeNo) {
                                item = $scope.residents[i]; isExist = true;
                                $scope.currentResident = item;
                                $scope.colorId = FeeNo;
                                if (!$scope.currentResident.ImgUrl) {
                                    if (item.ImgUrl != "" && item.ImgUrl != null)
                                        $scope.currentResident.ImgUrl = item.ImgUrl;
                                    else
                                        $scope.currentResident.ImgUrl = "/Images/defaultavatar.png";
                                }
                                break;
                            }
                        }
                        if (isExist) {
                            $scope.callbackFn({ resident: item });
                        }
                    }
                });
            };
            $scope.afterSelected = function (item) {
                $scope.currentResident = item;//设置ResidentCard的currentResident
                if (!$scope.currentResident.ImgUrl) {
                    if (item.ImgUrl != "" && item.ImgUrl!=null)
                        $scope.currentResident.ImgUrl = item.ImgUrl;//PhotoPath.PhotoPath;
                    else
                        $scope.currentResident.ImgUrl = "/Images/defaultavatar.png";
                }
                $scope.callbackFn({ resident: item });//回调
            }
            $scope.$on('refreshResidentList', function () {
                readList();
            });
            readList();
        }],
        link: function (scope, element, attrs) {
            if (attrs.layoutDirection) {
                if (attrs.layoutDirection == "horizontal") {
                    element.find("#cardImgArea").removeClass("col-sm-12").addClass("col-sm-4");
                    element.find("#cardInfoArea").removeClass("col-sm-12").addClass("col-sm-8");
                } else if (attrs.layoutDirection == "auto") {
                    element.find("#cardImgArea").addClass("modal-card");
                    element.find("#cardInfoArea").addClass("modal-card");
                }
            }
        }
    }
}]);
