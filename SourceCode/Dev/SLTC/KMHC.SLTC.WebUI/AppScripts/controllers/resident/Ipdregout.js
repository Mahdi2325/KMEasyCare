/*
创建人:张祥
创建日期:2016-04-01
说明: 出院办理
*/
angular.module("sltcApp")
.controller("IpdregoutCtrl", ['$scope', '$http','IpdregoutRes', 'regNCIInfoRes', 'utility', '$state', function ($scope,$http, IpdregoutRes, regNCIInfoRes,utility, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.currentItem = {};
    $scope.regnciInfo = {};
    $scope.InDate = "";
    // 当前住民
    $scope.currentResident = {};
    $scope.buttonShow = false;
    $scope.IsHasCert = false;
    $scope.setdate = "";

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息

        $scope.init($scope.currentResident.FeeNo);//加载当前住民的跌倒记录
        $scope.InDate = $scope.currentResident.InDate != null ? new Date($scope.currentResident.InDate).format("yyyy-MM-dd") : "";
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            if ($scope.currentResident.IsFinancialClose) {
                $scope.setdate = $scope.currentResident.FinancialCloseTime;
                $scope.buttonShow = true;
            }
            else {
                utility.message("请先到关账作业中设置关账时间后，进行结案操作！");
                $scope.setdate = "";
                $scope.buttonShow = false;
                return;
            }
        }
    }

    //获取住民出院信息
    $scope.init = function (FeeNo) {
        IpdregoutRes.get({ FeeNo: FeeNo }, function (data) {
            $scope.currentItem = data.Data;
        });

        regNCIInfoRes.get({ feeNo: FeeNo }, function (data) {
            if (data.ResultCode == -1) {
                $scope.IsHasCert = false;
            }
            else {
                $scope.IsHasCert = true;
                $scope.regnciInfo = data.Data;
            }
        });
    }

    $scope.checkIpdregout = function (item) {
        item.FeeNo = $scope.currentResident.FeeNo;
        if (confirm("结案作业后，不能进行费用录入或生成账单操作，您是否确认做结案结业?")) {
            IpdregoutRes.CheckBillInfo({ feeNo: item.FeeNo, ipdoutTime: item.CloseDate }, function (data) {
                if (data.ResultCode == -1) {
                    utility.msgwarning(data.ResultMessage);
                    return;
                }
                else {
                    $scope.saveIpdregout(item);
                }
            });
        }
    };


    //保存数据
    $scope.saveIpdregout = function (item) {
        item.FeeNo = $scope.currentResident.FeeNo;
        //查询有无护理险信息
        if ($scope.IsHasCert) {
            $scope.UpdateNCICert(item);
        }
        else {
            $scope.saveIpdOut(item);
        }
    }

    $scope.UpdateNCICert = function (item)
    {
        $http({
            url: 'api/IpdregOut/RegCert?feeNo=' + item.FeeNo + '&ipdoutTime=' + item.CloseDate,
            method: 'GET'
        }).success(function (data, header, config, status) {
            if (data.ResultCode == 0) { //护理险入院资格已取消
                $scope.saveIpdOut(item);
            }
        }).error(function (data, header, config, status) {
            utility.msgwarning("护理险平台无法连接，请联系管理员后修复系统后进行离院！");
        })
    }

    $scope.saveIpdOut = function (item)
    {
        IpdregoutRes.save(item, function () {
            $scope.$broadcast('refreshResidentList', '');
            utility.message($scope.currentResident.Name + "的出院信息保存成功！");
        })
    }

    $scope.GetTotalDay = function () {
        if ($scope.currentItem.CloseDate != "" && $scope.InDate != "") {
            if ($scope.InDate > $scope.currentItem.CloseDate) {
                utility.message("结案日期不能小于入住日期");
                $scope.currentItem.CloseDate = "";
            }
            else {
                var days = GetDateDiff($scope.currentItem.CloseDate, $scope.InDate)
                $scope.currentItem.TotalDay = days;
            }
        };
    }
    $scope.selCloseDate = function (val) {
        if (val) {
            $scope.currentItem.CloseDate = $scope.setdate == "" ? CurentTime() : $scope.setdate.slice(0, 10)
            $scope.GetTotalDay();
        }
        else {
            $scope.currentItem.CloseDate = "";
        }



    }
    $scope.selDeadDate = function (val) {
        if (val) {
            $scope.currentItem.DeadDate = CurentTime();
        }
        else {
            $scope.currentItem.DeadDate = "";
        }


    }

    //得到当前时间字符串，格式为：YYYY-MM-DD HH:MM:SS  
    function CurentTime() {
        var now = new Date();

        var year = now.getFullYear();       //年
        var month = now.getMonth() + 1;     //月
        var day = now.getDate();            //日

        //var hh = now.getHours();            //时
        //var mm = now.getMinutes();          //分
        //var ss = now.getSeconds();			//秒

        var clock = year + "-";

        if (month < 10) clock += "0";
        clock += month + "-";

        if (day < 10) clock += "0";
        clock += day + " ";

        //if (hh < 10) clock += "0";
        //clock += hh + ":";

        //if (mm < 10) clock += '0';
        //clock += mm + ":";

        //if (ss < 10) clock += '0';
        //clock += ss;

        return (clock);
    }

    function GetDateDiff(startDate, endDate) {

        var startTime = new Date(Date.parse(startDate.replace(/-/g, "/"))).getTime();

        var endTime = new Date(Date.parse(endDate.replace(/-/g, "/"))).getTime();

        var dates = Math.abs((startTime - endTime)) / (1000 * 60 * 60 * 24);

        return dates;

    }


}]);


