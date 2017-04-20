/*
创建人:Bob Du
创建日期:2016-11-07
说明:　工作日历控制器
*/

angular.module("sltcApp").controller("workCalendarCtrl", ['$scope', '$state', '$http', '$compile', 'dictionary', 'webUploader', 'utility', 'dc_AssignJobsRes', function ($scope, $state, $http, $compile, dictionary, webUploader, utility, dc_AssignJobsRes) {

    var colors = ['#0066CC', '#336600', '#FF3300'];
   
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
            eventLimit: true, 
            events: function (start, end, timezone, callback) {
                $.ajax({
                    url: 'api/AssignJobs',
                    dataType: 'json',
                    data: {
                        start: start.format(),
                        end: end.format()
                    },
                    success: function (doc) {
                        var events = [];
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















