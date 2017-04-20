/*
创建人:BobDu
创建日期:2017-02-08
说明: 前台控制台
*/

angular.module("sltcApp")
.controller("frontConsoleCtrl", ['$scope', '$state', '$http', '$compile', 'utility', function ($scope, $state, $http, $compile, utility) {
   
    $scope.getTypeData = function (type) {
        switch (type) {
            case "day":
                $scope.eDate = $scope.sDate = moment().format("YYYY-MM-DD");
                break;
            case "week":
                $scope.sDate = moment().subtract(7, 'days').format("YYYY-MM-DD");
                $scope.eDate = moment().format("YYYY-MM-DD");
                break;
            case "month":
                $scope.sDate = moment().subtract(30, 'days').format("YYYY-MM-DD");
                $scope.eDate = moment().format("YYYY-MM-DD");
                break;
        }
        $scope.init();
    }
    $scope.init = function () {
        if (!$scope.sDate || !$scope.eDate) {
            utility.msgwarning("请选择开始日期和结束日期");
            return;
        }
        $http({
            url: 'api/FrontConsole?beginTime=' +
                (typeof ($scope.sDate) == "undefined" ? "" : $scope.sDate) +
                '&endTime=' +
                (typeof ($scope.eDate) == "undefined" ? "" : $scope.eDate),
            method: 'GET'
        }).success(function (data, header, config, status) {
            $scope.data = data;
            var dayArr = _.map($scope.data, 'day');
            var ipdArr = [];
            var leaArr = [];
            var famArr = [];
            var bedPreArr = [];
            var bedNoUseArr = [];
            var resInHospArr = [];
            for (var i = 0; i < dayArr.length; i++) {
                ipdArr.push($scope.data[i].ipdFee.length);
                leaArr.push($scope.data[i].leaFee.length);
                famArr.push($scope.data[i].famFee.length);
                bedPreArr.push($scope.data[i].bedPreFee.length);
                bedNoUseArr.push($scope.data[i].bedNoUseFee.length);
                resInHospArr.push($scope.data[i].resInHospFee.length);
            }
            initChart("fcChart", dayArr, leaArr, famArr, bedPreArr, bedNoUseArr, resInHospArr);
        }).error(function (data, header, config, status) {
            //处理响应失败
            alert("Error:数据获取出错异常！");
        });
    }
    $scope.sDate = moment().subtract(7, 'days').format("YYYY-MM-DD");
    $scope.eDate = moment().format("YYYY-MM-DD");
    $scope.init();
    $scope.clickDetail = function (feeNoArr) {
        var html = '<div km-include km-template="Views/Home/Person.html" km-controller="personExtendCtrl"  km-include-params="{feeNoArr:\'' + JSON.stringify(feeNoArr) + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">住民</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
    function initChart(id, dayArr, leaArr, famArr, bedPreArr, bedNoUseArr, resInHospArr) {
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            tooltip: {
                trigger: 'axis'
            },
            toolbox: {
                feature: {
                    //dataView: { show: true, readOnly: false },
                    //magicType: { show: true, type: ['line', 'bar'] },
                    //restore: { show: true },
                    //saveAsImage: { show: true }
                }
            },
            legend: {
                data: ['外出人数', '参观人数', '床位预定人数', '入住数']
            },
            xAxis: [
                {
                    type: 'category',
                    data: dayArr
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    interval:10,
                    splitLine: {
                        show: false
                    },
                    axisLabel: {
                        formatter: '{value}人'
                    }
                }
            ],
            series: []
        };
        option.series.push({
            name: '外出人数',
            type: 'line',
            smooth: true,
            symbol: 'circle',
            symbolSize: 8,
            data: leaArr
        });
        option.series.push({
            name: '参观人数',
            type: 'line',
            smooth: true,
            symbol: 'circle',
            symbolSize: 8,
            data: famArr
        });
        option.series.push({
            name: '床位预定人数',
            type: 'line',
            smooth: true,
            symbol: 'circle',
            symbolSize: 8,
            data: bedPreArr
        });
        //option.series.push({
        //    name: '空床数',
        //    type: 'line',
        //    smooth: true,
        //    symbol: 'circle',
        //    symbolSize: 8,
        //    data: bedNoUseArr
        //});
        option.series.push({
            name: '入住数',
            type: 'line',
            smooth: true,
            symbol: 'circle',
            symbolSize:8,
            data: resInHospArr
        });
        myChart.setOption(option);
    }
}]);