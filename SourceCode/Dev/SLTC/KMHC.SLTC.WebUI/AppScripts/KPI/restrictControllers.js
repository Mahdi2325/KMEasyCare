/*
创建人:张正泉
创建日期:2016-03-15
说明: 压疮
*/
angular.module("sltcApp")
.controller("restrictCtrl",['$scope', '$state', '$filter','$http', 'restrictRes', 'restrictDetailRes', 'utility', function ($scope, $state, $filter,$http, restrictRes, restrictDetailRes, utility) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.init = function () {
        var d=new Date(), nowDate = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss"), classType = utility.getClassType(d);
        $scope.curUser = utility.getUserInfo();
        $scope.curResident = $scope.curResident||{};
        $scope.curItem = {
            StartDate: nowDate, RecordBy: $scope.curUser.EmpNo, ClassType: classType,
            Days: 30, RegNo: $scope.curResident.RegNo,
            FeeNo: $scope.curResident.FeeNo
        };
        $scope.initDel();
    }

    $scope.initDel = function () {
        $scope.curDelItem = { EvalDate: $filter("date")(new Date(), "yyyy-MM-dd"), EvaluateBy: $scope.curUser.EmpNo };
        $scope.isAdd = true;
    }

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.curResident = resident;//获取当前住民信息
        angular.extend($scope.curItem, { FeeNo: resident.FeeNo, RegNo: resident.RegNo });
    }

    $scope.$watch("curItem.CancelFlag", function (newValue) {
        if (newValue) {
            $scope.curItem.CancelDate = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss");
        } else {
            $scope.curItem.CancelDate = null;
        }
    });
    $scope.$watch("curItem.FeeNo", function (newValue) {
        $scope.search(newValue);
    });
    $scope.staffSelected = function (item, t) {
        if (t) {
            $scope.curItem[t] = item.EmpNo;
        } else {
            $scope.curDelItem.EvaluateBy = item.EmpNo;
        }
    }

    $scope.search = function (feeNo) {
        if (feeNo) {
            restrictRes.get({ feeno: feeNo }, function (data) {
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
        restrictDetailRes.query({ SeqNo: item.SeqNo }, function (data) {
            $scope.curItem.Detail = data;
        });
        $("#modalAll").modal("toggle");
    }


    //删除疼痛记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的疼痛记录吗?")) {
            $scope.curItem.Detail.splice($scope.curItem.Detail.indexOf(item), 1);
            restrictDetailRes.delete({ id: item.Id });
        }
    };


    $scope.editOrCreate = function (item) {
        if (item) {
            $scope.curDelItem = angular.copy(item);
            $scope.isAdd = false;
        } else {
            $scope.isAdd = true;
            $scope.curDelItem = { SeqNo: $scope.curItem.SeqNo };
        }
    }

    $scope.searchAll = function () {
        var feeNo = $scope.curItem.FeeNo;
        if (feeNo) {
            restrictRes.query({ CurrentPage: 1, PageSize: 10, FeeNo: feeNo }, function (data) {
                if (data.length > 0) {
                    $scope.restrictList = data;
                }
            });
        }
    }

    $scope.save = function () {
        restrictRes.save($scope.curItem);
        utility.message("主文档保存成功！");
    }

    $scope.mEditOrAdd = function (type) {
        if (type == "add") {
            $scope.curItem.SeqNo = 0;
            restrictRes.save($scope.curItem, function (data) {
                $scope.curItem = data.Data;
                $scope.curDelItem = {};
                utility.message("文档保存成功！");
            });

        } else {
            if ($scope.curItem.SeqNo) {
                var detail = $scope.curItem.Detail;
                $scope.curItem.Detail = [];//只保存主文档时,传到后端数据过滤掉detail
                restrictRes.save($scope.curItem, function (data) {
                    $scope.curItem = data.Data;
                    utility.message("文档保存成功！");
                });
                $scope.curItem.Detail = detail;
            } else {
                restrictRes.save($scope.curItem, function (data) {
                    $scope.curItem = data.Data;
                    $scope.curDelItem = {};
                    utility.message("文档保存成功！");
                });
            }
        }
    }

    $scope.mAdd = function () {
        restrictRes.save($scope.curItem, function (data) {
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
            restrictDetailRes.save($scope.curDelItem, function (data) {
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
        $scope.isAdd = true;
    }

    $scope.saveEdit = function () {
        restrictRes.save($scope.curItem, function () {
            $scope.isAdd = true;
            $scope.curDelItem = {};
            utility.message("修改压疮信息保存成功！");
        });
    };

    $scope.init();
}]);





