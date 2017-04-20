/*
创建人:张正泉
创建日期:2016-03-17
说明: 疼痛
*/
angular.module("sltcApp")
.controller("painCtrl", ['$scope', '$state', '$filter', 'painRes', 'painDetailRes', 'utility', function ($scope, $state, $filter, painRes, painDetailRes, utility) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.buttonShow = false;

    $scope.init = function () {
        var nowDate = $filter("date")(new Date(), "yyyy-MM-dd");
        $scope.curUser = utility.getUserInfo();
        $scope.curResident = $scope.curResident || {};
        $scope.curItem = {
            EvalDate: nowDate, NextEvalDate: nowDate, NextEvaluateBy: $scope.curUser.EmpNo, ClassType: "D",
            Days: 30, RegNo: $scope.curResident.RegNo,
            FeeNo: $scope.curResident.FeeNo
        };
        $scope.initDel();
    }

    $scope.initDel = function () {
        $scope.curDelItem = {};
    }


    $scope.itemChange = function () {
        var newValue = $(event.srcElement).val();
        if (newValue && $scope.curItem.CancelDate == null) {
            $scope.curItem.CancelDate = $filter("date")(new Date(), "yyyy-MM-dd");
        } else {
            $scope.curItem.CancelDate = null;
        }
    }
    $scope.itemDtlChange = function () {
        var newValue = $(event.srcElement).val();
        if (newValue && $scope.curDelItem.CancelDate == null) {
            $scope.curDelItem.CancelDate = $filter("date")(new Date(), "yyyy-MM-dd");
        } else {
            $scope.curDelItem.CancelDate = null;
        }
    }
    $scope.$watch("curDelItem.PainPart", function (newValue) {
        if (newValue) {
            $scope.curDelItem.Picture = "/Images/" + newValue + ".jpg"
        } else {
            $scope.curDelItem.Picture = null;
        }
    });
    $scope.$watch("curItem.FeeNo", function (newValue) {
        $scope.search(newValue);
    });


    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.curResident = resident;//获取当前住民信息
        angular.extend($scope.curItem, { FeeNo: resident.FeeNo, RegNo: resident.RegNo });
        if (angular.isDefined($scope.curResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    $scope.staffSelected = function (item, t) {
        if (t) {
            $scope.curItem[t] = item.EmpNo;
        } else {
            $scope.curDelItem.EvaluateBy = item.EmpNo;
        }

    }

    $scope.search = function (feeNo) {
        if (feeNo) {
            painRes.get({ feeno: feeNo }, function (data) {
                if (data.Data) {
                    $scope.curItem = data.Data;
                    $scope.initDel();
                } else {
                    $scope.init();
                }
            });
        }
    }

    $scope.modify = function (item) {
        $scope.curItem = item;
        painDetailRes.query({ SeqNo: item.SeqNo }, function (data) {
            $scope.curItem.Detail = data;
        });
        $("#modalAll").modal("toggle");
    }


    //删除疼痛记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的疼痛记录吗?")) {
            $scope.curItem.Detail.splice($scope.curItem.Detail.indexOf(item), 1);
            painDetailRes.delete({ id: item.Id });
        }
    };


    $scope.editOrCreate = function (item) {
        if (item) {
            $scope.curDelItem = angular.copy(item);
        } else {
            $scope.curDelItem = { SeqNo: $scope.curItem.SeqNo };
        }
    }

    $scope.searchAll = function () {
        var feeNo = $scope.curItem.FeeNo;
        if (feeNo) {
            painRes.query({ CurrentPage: 1, PageSize: 10, FeeNo: feeNo }, function (data) {
                if (data.length > 0) {
                    $scope.painList = data;
                }
            });
        }
    }

    $scope.save = function () {
        painRes.save($scope.curItem);
        utility.message("主文档保存成功！");
    }



    $scope.mEditOrAdd = function (type) {
        if (type == "add") {
            $scope.curItem.SeqNo = 0
            painRes.save($scope.curItem, function (data) {
                $scope.curItem = data.Data;
                $scope.curDelItem = {};
                utility.message("文档保存成功！");
            });
        } else {
            if ($scope.curItem.SeqNo) {
                var detail = $scope.curItem.Detail;
                $scope.curItem.Detail = [];//只保存主文档时,传到后端数据过滤掉detail
                painRes.save($scope.curItem, function (data) {
                    $scope.curItem = data.Data;
                    utility.message("文档保存成功！");
                });
                $scope.curItem.Detail = detail;
            } else {
                painRes.save($scope.curItem, function (data) {
                    $scope.curItem = data.Data;
                    $scope.curDelItem = {};
                    utility.message("文档保存成功！");
                });
            }
        }
    }

    $scope.mAdd = function () {
        painRes.save($scope.curItem, function (data) {
            $scope.curItem = data.Data;
            $scope.curDelItem = {};
            utility.message("文档保存成功！");
        });
    }


    $scope.dEditOrAdd = function () {
        if (!$scope.curItem.SeqNo) {
            $scope.curItem.Detail = [];
            if (!$scope.curDelItem.Id) {
                $scope.curItem.Detail.push($scope.curDelItem);
            }
            $scope.mEditOrAdd();
        } else {
            $scope.curDelItem.SeqNo = $scope.curItem.SeqNo;
            painDetailRes.save($scope.curDelItem, function (data) {
                if (!$scope.curDelItem.Id) {
                    $scope.curItem.Detail.push(data.Data);
                } else {
                    for (var i = $scope.curItem.Detail.length - 1; i > -1; i--) {
                        if ($scope.curItem.Detail[i].Id == data.Data.Id) {
                            $scope.curItem.Detail[i] = data.Data;
                        }
                    }
                }
                $scope.curDelItem = {};
            });
        }
    }


    $scope.init();
}]);

