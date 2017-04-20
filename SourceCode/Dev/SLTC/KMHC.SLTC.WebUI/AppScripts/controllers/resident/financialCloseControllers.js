angular.module("sltcApp")
    .controller("financialCloseCtrl", ['$scope', '$state', 'FinancialCloseRes', '$http', 'residentBriefRes', 'dictionary', 'utility', function ($scope, $state, FinancialCloseRes, $http,residentBriefRes, dictionary, utility) {
    $scope.residentInfo = {};
    $scope.FeeNo = $state.params.FeeNo == "underfind" ? "-1" : $state.params.FeeNo;
    //初始化
    $scope.init = function () {
        $scope.residentInfo = {};
        $scope.isclosebtn = false;
        $scope.iscanclebtn = false;
        $scope.getResident($scope.FeeNo);
    };

    ///选择住民
    $scope.residentSelected = function (resident) {
        $scope.FeeNo = resident.FeeNo;
        $scope.getResident($scope.FeeNo);
    };

    $scope.getResident = function (feeno) {
        if (feeno == "" || feeno == null || feeno == undefined || feeno == "-1")
        {
            utility.message("请选择住民！");
            return;
        }

        residentBriefRes.get({ feeNo: feeno, currentPage: 1, pageSize: 1000 }, function (data) {
            $scope.residentInfo = data.Data[0];
            if ($scope.residentInfo.IsFinancialClose) {
                $scope.isclosebtn = false;
                $scope.iscanclebtn = true;
            }
            else {
                $scope.isclosebtn = true;
                $scope.iscanclebtn = false;
                $scope.residentInfo.FinancialCloseTime = getNowFormatData();
            }
        });
    }

    //获取当前时间
    function getNowFormatData() {
        var date = new Date();
        var sep = "-";
        var sep2 = ":";
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var strdate = date.getDate();

        var strhour = date.getHours();
        var strminutes = date.getMinutes();
        var strsecond = date.getSeconds();

        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strdate >= 1 && strdate <= 9) {
            strdate = "0" + strdate;
        }

        if (strhour >= 1 && strhour <= 9) {
            strhour = "0" + strhour;
        }

        if (strminutes >= 1 && strminutes <= 9) {
            strminutes = "0" + strminutes;
        }

        if (strsecond >= 1 && strsecond <= 9) {
            strsecond = "0" + strsecond;
        }

        var currentdate = year + sep + month + sep + strdate + " " + strhour + sep2 + strminutes + sep2 + strsecond;
        return currentdate;
    };

    $scope.closeData = function () {
        $scope.CloseFinancialFilter = {};
        if ($scope.residentInfo.FeeNo == null) {
            utility.message("请选择住民！");
            return;
        }

        if ($scope.residentInfo.FinancialCloseTime == null) {
            utility.message("关账日期不能为空，请重新选择！");
            return;
        }
        FinancialCloseRes.get({ feeNo: $scope.FeeNo, financialCloseTime: $scope.residentInfo.FinancialCloseTime, type: "close" }, function (data) {
            if (data.ResultCode == -1) {
                utility.message(data.ResultMessage);
            }
            else if (data.ResultCode == 20)
            {
                $http({
                    url: 'api/FinancialClose?yearMonth=' + data.ResultMessage,
                    method: 'GET'
                }).success(function (data, header, config, status) {
                    $scope.CloseFinancialFilter.feeNo = $scope.FeeNo;
                    $scope.CloseFinancialFilter.financialCloseTime = $scope.residentInfo.FinancialCloseTime;
                    $scope.CloseFinancialFilter.type = "close";
                    if (data != null) {
                        if (data.Status == 20) {
                            FinancialCloseRes.save($scope.CloseFinancialFilter, function () {
                                $scope.getResident($scope.FeeNo);
                                utility.message("关账成功！");
                            })
                        }
                        else if (data.Status == 30) {
                            FinancialCloseRes.save($scope.CloseFinancialFilter, function () {
                                $scope.getResident($scope.FeeNo);
                                utility.message("关账成功！");
                            })
                        }
                        else {
                            utility.message("存在关账日期时的账单，请先此月份上传明细撤回后，再进行关账操作");
                            return;
                        }
                    }
                    // 设置个人信息状态
                }).error(function (data, header, config, status) {
                    utility.msgwarning("护理险平台无法连接，请联系管理员！");
                    return;
                })
            }
            else {
                $scope.getResident($scope.FeeNo);
                utility.message("关账成功！");
            }
        });
    };

    $scope.cancleData = function () {
        $scope.CloseFinancialFilter = {};
        FinancialCloseRes.get({ feeNo: $scope.FeeNo, financialCloseTime: $scope.residentInfo.FinancialCloseTime, type: "Cancle" }, function (data) {
            if (data.ResultCode == -1) {
                utility.message(data.ResultMessage);
            }
            else if (data.ResultCode == 20)
            {
                $http({
                    url: 'api/FinancialClose?yearMonth=' + data.ResultMessage,
                    method: 'GET'
                }).success(function (data, header, config, status) {
                    $scope.CloseFinancialFilter.feeNo = $scope.FeeNo;
                    $scope.CloseFinancialFilter.financialCloseTime = $scope.residentInfo.FinancialCloseTime;
                    $scope.CloseFinancialFilter.type = "Cancle";
                    if (data != null) {
                        if (data.Status == 20) {
                            FinancialCloseRes.save($scope.CloseFinancialFilter, function () {
                                $scope.getResident($scope.FeeNo);
                                utility.message("关账成功！");
                            })
                        }
                        else if (data.Status == 30) {
                            FinancialCloseRes.save($scope.CloseFinancialFilter, function () {
                                $scope.getResident($scope.FeeNo);
                                utility.message("关账成功！");
                            })
                        }
                        else
                        {
                            utility.message("存在关账日期时的账单，请先此月份上传明细撤回后，再进行关账操作");
                            return;
                        }
                    }
                    // 设置个人信息状态
                }).error(function (data, header, config, status) {
                    utility.msgwarning("护理险平台无法连接，请联系管理员！");
                    return;
                })
            }
            else {
                $scope.getResident($scope.FeeNo);
                utility.message("取消关账成功！");
            }
        });
    };


    $scope.init();
}]);