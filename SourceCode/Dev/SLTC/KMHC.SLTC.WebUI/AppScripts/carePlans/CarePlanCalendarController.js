/*
创建人:Bob Du
创建日期:2016-08-31
说明:　新桌面控制器
*/

angular.module("sltcApp").controller("calendarCtrl", ['$scope', '$state', '$http', '$compile', 'dictionary', 'webUploader', 'utility', 'carePlanDetailRes', 'carePlanActivityRes', function ($scope, $state, $http, $compile, dictionary, webUploader, utility, carePlanDetailRes, carePlanActivityRes) {
    var feeNo = $state.params.feeNo;
    var regName = $state.params.regName;
    var regNo = $state.params.regNo;
    $scope.Data = {};
    $scope.Data.RegNo = regNo;
    $scope.Data.feeNo = feeNo;
    $scope.Data.RegName = regName;

    $scope.currentItem = {};

    $scope.data_In = {};
    $scope.currentResident = {};
    $scope.buttonShow = false;
    $scope.Notices = [];
    var colors = ['#0066CC', '#336600', '#FF3300'];
    $scope.aClick = function () {
        $state.go("AssignTask");
    }
    $scope.$on('renderEvent', function (evt, next, current) {
        evt.stopPropagation();
        var schdata2 = { id: next.id, title: next.title, start: next.start, color: colors[Math.ceil(Math.random() * 3) - 1] };
        $('#calendar').fullCalendar('renderEvent', schdata2, false);
        $scope.dialog.close();
    });
    $scope.$on('updateEvent', function (evt, next, current) {
        evt.stopPropagation();
        $scope.event.title = $scope.event.regName == null ? next.title : "(" + $scope.event.regName + ")" + next.title;
        var schdata2 = { title: evt.targetScope.Data.Content, start: evt.targetScope.Data.AssignDate };
        $('#calendar').fullCalendar('updateEvent', $scope.event);
        $scope.dialog.close();
    });
    $scope.$on('removeEvents', function (evt, next, current) {
        evt.stopPropagation();
        $('#calendar').fullCalendar('removeEvents', $scope.event.id);
        $scope.dialog.close();
    });
    $scope.hide = function () {
        $scope.dialog.close();
    }
    $scope.click = function (date, id) {
        var html = '<div km-include km-template="Views/CarePlans/Remote.html" km-controller="remoteCtrl2"  km-include-params="{date:\'' + date + '\',id:\'' + id + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">查看日历</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
    angular.element(document).ready(function () {
        $('#calendar').fullCalendar({
          
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek'
            },
            eventClick: function (event) {
                $scope.event = event;
                $scope.click(event.start.format(), event.id);
            },
            firstDay: 1,
            editable: true,
            timeFormat: 'H(:mm)',
            eventLimit: true, // allow "more" link when too many events
            //events: {
            //    url: 'api/myDesk',
            //    error: function () {
            //        $('#script-warning').show();
            //    }
            //},
            events: function (start, end, timezone, callback) {
                $.ajax({
                    url: 'api/carePlanDetail',
                    dataType: 'json',
                    data: {
                        // our hypothetical feed requires UNIX timestamps
                        start: start.format(),
                        end: end.format(),
                        feeno: feeNo
                    },
                    success: function (doc) {
                        var events = [];
                        //$(doc).find('event').each(function() {
                        //    events.push({
                        //        title: $(this).attr('title'),
                        //        start: $(this).attr('start') // will be parsed
                        //    });
                        //});

                        for (var i = 0; i < doc.length; i++) {
                            events.push({
                                id: doc[i].id,
                                title: doc[i].title,
                                end: moment(doc[i].end).format("HH:mm:ss") == "00:00:00" ? moment(doc[i].end).add(1, 'days').format("YYYY-MM-DD") : doc[i].end, // will be parsed
                                color: colors[Math.ceil(Math.random() * 3) - 1],
                                start: moment(doc[i].start).format("HH:mm:ss") == "00:00:00" ? moment(doc[i].start).format("YYYY-MM-DD") : doc[i].start // will be parsed
                            });
                        }
                        callback(events);
                    }
                });
            },
            loading: function (bool) {
                $('#loading').toggle(bool);
            }
        });
    });
   

}]);















