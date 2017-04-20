/*
创建人:Bob Du
创建日期:2016-08-31
说明:　新桌面控制器
*/

angular.module("sltcApp").controller("myDeskCtrl", ['$scope', '$state', '$http', '$compile', '$q', 'dictionary', 'webUploader', 'myDeskRes', 'utility', 'assignTaskRes', 'noticeRes', function($scope, $state, $http, $compile, $q, dictionary, webUploader, myDeskRes, utility, assignTaskRes, noticeRes) {

    $scope.Data = [];
    $scope.currentItem = {};
    $scope.data_In = {};
    $scope.currentResident = {};
    $scope.buttonShow = false;
    $scope.Notices = [];
    var res;
    var colors = ['#0066CC', '#336600', '#FF3300'];
    $scope.aClick = function() {
        $state.go("AssignTask");
    }
    $scope.$on('renderEvent', function(evt, next, current) {
        evt.stopPropagation();
        var schdata2 = {
            id: next.id,
            title: next.title,
            start: next.start,
            color: colors[Math.ceil(Math.random() * 3) - 1]
        };
        $('#calendar').fullCalendar('renderEvent', schdata2, false);
        $scope.dialog.close();
    });
    $scope.$on('updateEvent', function(evt, next, current) {
        evt.stopPropagation();
        $scope.event.title = $scope.event.regName == null ? next.title : "(" + $scope.event.regName + ")" + next.title;
        var schdata2 = {
            title: evt.targetScope.Data.Content,
            start: evt.targetScope.Data.AssignDate
        };
        $('#calendar').fullCalendar('updateEvent', $scope.event);
        $scope.dialog.close();
    });
    $scope.$on('removeEvents', function(evt, next, current) {
        evt.stopPropagation();
        $('#calendar').fullCalendar('removeEvents', $scope.event.id);
        $scope.dialog.close();
    });
    $scope.hide = function() {
        $scope.dialog.close();
    }
    $scope.click = function(date, id) {
        var html = '<div km-include km-template="Views/Home/Remote.html" km-controller="remoteCtrl"  km-include-params="{date:\'' + date + '\',id:\'' + id + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">编辑日历</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function(dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
    $scope.clickDetail = function (type) {
        var feeNoArr;
        switch(type)
        {
            case "L":
                feeNoArr= _.map(_.where(res, { type: 'L' }), 'FEENO');
                break;
            case "U":
                feeNoArr= _.map(_.where(res, { type: 'U' }), 'FEENO');
                break;
            case "F":
                feeNoArr = _.map(_.where(res, { type: 'F' }), 'FEENO');
                break;
            case "B":
                feeNoArr = _.map(_.where(res, { type: 'B' }), 'FEENO');
                break;
        }
      
        var html = '<div km-include km-template="Views/Home/Person.html" km-controller="personExtendCtrl"  km-include-params="{type:\'' + type + '\',feeNoArr:\'' + JSON.stringify(feeNoArr) + '\'}" ></div>';
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
    angular.element(document).ready(function() {
        $('#calendar').fullCalendar({
            customButtons: {
                myCustomButton: {
                    text: '新增提醒',
                    click: function() {
                        $scope.click(moment().format("YYYY-MM-DD HH:mm:ss"), "");
                    }
                }
            },
            header: {
                left: 'prev,next today,myCustomButton',
                center: 'title',
                right: 'month,agendaWeek'
            },
            eventClick: function(event) {
                $scope.event = event;
                $scope.click(event.start.format(), event.id);
            },
            firstDay: 1,
            editable: true,
            timeFormat: 'H:mm',
            eventLimit: true,
            events: function(start, end, timezone, callback) {
                $.ajax({
                    url: 'api/myDesk',
                    dataType: 'json',
                    data: {
                        start: start.format(),
                        end: end.format()
                    },
                    success: function(doc) {
                        var events = [];
                        for (var i = 0; i < doc.length; i++) {
                            events.push({
                                id: doc[i].id,
                                title: doc[i].title,
                                regName: doc[i].regName,
                                color: colors[Math.ceil(Math.random() * 3) - 1],
                                start: moment(doc[i].start).format("HH:mm:ss") == "00:00:00" ? moment(doc[i].start).format("YYYY-MM-DD") : doc[i].start // will be parsed
                            });
                        }
                        callback(events);
                    }
                });
            },
            loading: function(bool) {
                $('#loading').toggle(bool);
            }
        });
    });
    $scope.init = function() {
        $scope.options = {
            buttons: [],
            ajaxObject: noticeRes,
            params: {
                sDate: "",
                eDate: ""
            },
            success: function(data) {
                $scope.Notices = data.Data;
            },
            pageInfo: {
                CurrentPage: 1,
                PageSize: 10
            }
        }
        $q.all([$http({
            url: 'api/myDesk/QueryKPI',
            method: 'GET'
        }), $http({
            url: 'api/myDesk/DashboardDataIn',
            method: 'GET'
        }), $http({
            url: 'api/myDesk/DashboardDataOut',
            method: 'GET'
        }), $http({
            url: 'api/myDesk/DashboardDataBed',
            method: 'GET'
        }), $http({
            url: 'api/myDesk/DashboardDataBedSore',
            method: 'GET'
        })]).then(function (results) {
            res = results[0].data;
            $scope.Data.push(_.where(res, { type: 'I' }).length);
            $scope.Data.push(_.where(res, { type: 'L' }).length);
            $scope.Data.push(_.where(res, { type: 'U' }).length);
            $scope.Data.push(_.where(res, { type: 'F' }).length);
            $scope.Data.push(_.where(res, { type: 'B' }).length);
            $scope.Data.push(_.where(res, { type: 'A' }).length);
            var data_In = eval("(" + results[1].data + ")");
            initChart("container", data_In, 'column', "本年度入院人数");
            var data_Out = eval("(" + results[2].data + ")");
            initChart("container1", data_Out, 'column', "本年度结案人数");
            initChart2(results[3].data);
            var data_BedSore = eval("(" + results[4].data + ")");
            initChart("container3", data_BedSore, 'column', "压疮");
        });

    }

    function initChart(id, data, type, title) {
        var chart;
        chart = new Highcharts.Chart({
            chart: {
                renderTo: id, //放置图表的容器
                plotBackgroundColor: null,
                plotBorderWidth: null,
                defaultSeriesType: type //图表类型line, spline, area, areaspline, column, bar, pie , scatter
            },
            title: {
                text: title
            },
            xAxis: { //X轴数据
                categories: ['01月', '02月', '03月', '04月', '05月', '06月', '07月', '08月', '09月', '10月', '11月', '12月'],
                labels: {
                    rotation: -40, //字体倾斜
                    align: 'right',
                    style: {
                        font: 'normal 8px 宋体'
                    }
                }
            },
            yAxis: { //Y轴显示文字
                title: {
                    text: ''
                },
                min: 0,
                minRange: 1
            },
            exporting: {
                enabled: false
            },
            tooltip: {
                enabled: true,
                formatter: function() {
                    return '<b>' + this.x + '</b><br/>' + this.series.name + ': ' + Highcharts.numberFormat(this.y, 0) + "个人";
                }
            },
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: true //是否显示title
                }
            },
            series: [{
                name: '女性',
                data: data[1]

            }, {
                name: '男性',
                data: data[0],
                color: '#FB9337'
            }]
        });
    }

    function initChart2(data) {
        var oValue = 0;
        var eValue = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i].BEDSTATUS == "Used") {
                oValue = data[i].Num;
            }
            if (data[i].BEDSTATUS == "Empty") {
                eValue = data[i].Num;
            }
        }
        // initChart("container2");
        $('#container2').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            title: {
                text: '床位占用比例'
            },
            tooltip: {
                pointFormat: ' <b>{point.percentage:.1f}%<br/>床位数：{point.y}</b>'
            },
            exporting: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            series: [{
                type: 'pie',

                data: [

                    ['床位空闲比例', eValue], {
                        name: '床位占用比例',
                        y: oValue,
                        sliced: true,
                        selected: true,
                        color: '#FB9337'
                    }
                ]
            }]
        });
    }

    $scope.init();

}]);
