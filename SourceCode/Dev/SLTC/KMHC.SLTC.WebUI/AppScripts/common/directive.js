///修改人:肖国栋
///修改日期:2016-03-01
///说明:修改成匿名函数方式
laydate.skin("danlan");
(function () {
    var app = angular.module("extentDirective", []);

    app.directive("caReload", function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.click(function () {
                    var el = element.parents(".box");
                    jQuery(el).block({
                        message: '<img src="../Content/CloudAdmin/img/loaders/12.gif" align="absmiddle">',
                        css: {
                            border: 'none',
                            padding: '2px',
                            backgroundColor: 'none'
                        },
                        overlayCSS: {
                            backgroundColor: '#000',
                            opacity: 0.05,
                            cursor: 'wait'
                        }
                    });
                    window.setTimeout(function () {
                        jQuery(el).unblock({
                            onUnblock: function () {
                                jQuery(el).removeAttr("style");
                            }
                        });
                    }, 1000);
                });
            }
        };
    }).directive("caRemove", function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.click(function () {
                    var removable = jQuery(this).parents(".box");
                    if (removable.next().hasClass('box') || removable.prev().hasClass('box')) {
                        jQuery(this).parents(".box").remove();
                    } else {
                        jQuery(this).parents(".box").parent().remove();
                    }
                });
            }
        };
    }).directive("caCollapse", function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.click(function () {
                    var el = jQuery(this).parents(".box").children(".box-body");
                    if (jQuery(this).hasClass("collapse")) {
                        jQuery(this).removeClass("collapse").addClass("expand");
                        var i = jQuery(this).children(".fa-chevron-up");
                        i.removeClass("fa-chevron-up").addClass("fa-chevron-down");
                        el.slideUp(200);
                    } else {
                        jQuery(this).removeClass("expand").addClass("collapse");
                        var i = jQuery(this).children(".fa-chevron-down");
                        i.removeClass("fa-chevron-down").addClass("fa-chevron-up");
                        el.slideDown(200);
                    }
                });
            }
        };
    }).directive("caBtnModal", function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.click(function () {
                    var el = jQuery(this).attr("href");
                    if (el.indexOf("#") < 0) {
                        el = jQuery(this).attr("data-target");
                    }
                    $(el).modal();
                });
            }
        };
    }).directive('caDateTimePicker', function () {
        return {
            restrict: 'A',
            require: ['ngModel'],
            scope: {
                ngModel: '='
            },
            link: function (scope, element, attrs, ctrls) {
                var ngModel = ctrls[0];
                var option = {
                    format: 'YYYY-MM-DD hh:mm:ss', //日期格式
                    istoday: true, //是否显示今天
                    istime: true,
                    issure: true, //是否显示确认
                    zIndex: 99999999, //css z-index
                    choose: function (dates) { //选择好日期的回调
                        element.val(dates);
                        ngModel.$setViewValue(dates);
                    }
                };
                element.bind('click', function () {
                    var number_r = /^\d+$/;
                    var date_r = /^((\d{4})-(\d{2})-(\d{2}))|(\s{1}(\d{2}):(\d{2}):(\d{2}))$/;
                    var min = attrs["min"];
                    if (typeof (min) != "undefined") {
                        if (number_r.test(min)) {
                            option.min = laydate.now(Number(min));
                        }
                        else if (date_r.test(min)) {
                            option.min = min;
                        }
                    }
                    var max = attrs["max"];
                    if (typeof (max) != "undefined") {
                        if (number_r.test(max)) {
                            option.max = laydate.now(Number(max));
                        }
                        else if (date_r.test(max)) {
                            option.max = max;
                        }
                    }
                    var start = attrs["start"];
                    if (typeof (start) != "undefined") {
                        var sv = scope.$parent.$eval(start);
                        if (typeof (sv) != "undefined" && date_r.test(sv)) {
                            option.min = sv;
                        }
                    }
                    var end = attrs["end"];
                    if (typeof (end) != "undefined" ) {
                        var ev = scope.$parent.$eval(end);
                        if (typeof (ev) != "undefined" && date_r.test(ev)) {
                            option.max = ev;
                        }
                    }
                    laydate(option);
                });
                //element.attr("readonly", "readonly");
                //element.datetimepicker({
                //    dateFormat: "yy-mm-dd",
                //    changeMonth: true,
                //    changeYear: true,
                //    timeFormat: "HH:mm:ss",
                //    dateFormat: "yy-mm-dd",
                //    dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六", "周日"],
                //    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                //    daysMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                //    monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                //    monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                //    prevText: '向前',
                //    nextText: '向后',
                //    closeText: "清空",
                //    currentText: "现在",
                //    timeText: "时间",
                //    hourText: "时",
                //    minuteText: "分",
                //    secondText: "秒"
                //});
                //if (!$("#ui-datepicker-div").data("click")) {
                //    $("#ui-datepicker-div").data("click", true);
                //    $("#ui-datepicker-div").on("click", function (e) {
                //        var $this = $(e.target);
                //        if ($this.hasClass("ui-datepicker-close")) {
                //            var $input = $($.datepicker._curInst.input[0]);
                //            $input.val('');
                //            $input.trigger('change');
                //        }
                //    });
                //}
                scope.$watch('ngModel', function (newValue, oldValue, scope) {
                    if (angular.isDefined(newValue) && newValue != "" && newValue != null && isDate(newValue)) {
                        element.val(newDate(newValue).format("yyyy-MM-dd hh:mm:ss"));
                    }
                });
            }
        };
    }).directive('kmcaDateTimePicker', function () {//Bob Du 新增指令为了解决数据联动
        return {
            restrict: 'A',
            require: ['ngModel'],
            scope: {
                ngModel: '=',
                checktype: '='
            },
            link: function (scope, element, attrs, ctrls) {
                var ngModel = ctrls[0];
                var option = {
                    format: 'YYYY-MM-DD hh:mm:ss', //日期格式
                    istoday: true, //是否显示今天
                    istime: true,
                    issure: true, //是否显示确认
                    zIndex: 99999999, //css z-index
                    choose: function (dates) { //选择好日期的回调
                        element.val(dates);
                        ngModel.$setViewValue(dates);
                        console.log(dates);
                        scope.checktype = dates?true:false;
                    }
                };
                element.bind('click', function () {
                    var number_r = /^\d+$/;
                    var date_r = /^((\d{4})-(\d{2})-(\d{2}))|(\s{1}(\d{2}):(\d{2}):(\d{2}))$/;
                    var min = attrs["min"];
                    if (typeof (min) != "undefined") {
                        if (number_r.test(min)) {
                            option.min = laydate.now(Number(min));
                        }
                        else if (date_r.test(min)) {
                            option.min = min;
                        }
                    }
                    var max = attrs["max"];
                    if (typeof (max) != "undefined") {
                        if (number_r.test(max)) {
                            option.max = laydate.now(Number(max));
                        }
                        else if (date_r.test(max)) {
                            option.max = max;
                        }
                    }
                    var start = attrs["start"];
                    if (typeof (start) != "undefined") {
                        var sv = scope.$parent.$eval(start);
                        if (typeof (sv) != "undefined" && date_r.test(sv)) {
                            option.min = sv;
                        }
                    }
                    var end = attrs["end"];
                    if (typeof (end) != "undefined" ) {
                        var ev = scope.$parent.$eval(end);
                        if (typeof (ev) != "undefined" && date_r.test(ev)) {
                            option.max = ev;
                        }
                    }
                    laydate(option);
                });
                //element.attr("readonly", "readonly");
                //element.datetimepicker({
                //    dateFormat: "yy-mm-dd",
                //    changeMonth: true,
                //    changeYear: true,
                //    timeFormat: "HH:mm:ss",
                //    dateFormat: "yy-mm-dd",
                //    dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六", "周日"],
                //    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                //    daysMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                //    monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                //    monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                //    prevText: '向前',
                //    nextText: '向后',
                //    closeText: "清空",
                //    currentText: "现在",
                //    timeText: "时间",
                //    hourText: "时",
                //    minuteText: "分",
                //    secondText: "秒"
                //});
                //if (!$("#ui-datepicker-div").data("click")) {
                //    $("#ui-datepicker-div").data("click", true);
                //    $("#ui-datepicker-div").on("click", function (e) {
                //        var $this = $(e.target);
                //        if ($this.hasClass("ui-datepicker-close")) {
                //            var $input = $($.datepicker._curInst.input[0]);
                //            $input.val('');
                //            $input.trigger('change');
                //        }
                //    });
                //}
                scope.$watch('ngModel', function (newValue, oldValue, scope) {
                    if (angular.isDefined(newValue) && newValue != "" && newValue != null && isDate(newValue)) {
                        element.val(newDate(newValue).format("yyyy-MM-dd hh:mm:ss"));
                    }
                });
            }
        };
    }) .directive('caDatePicker', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            require: ['ngModel'],
            scope: {
                ngModel: '='
            },
            link: function (scope, element, attrs, ctrls) {
                var ngModel = ctrls[0];
                var option = {
                    format: 'YYYY-MM-DD', //日期格式
                    istoday: true, //是否显示今天
                    issure: false, //是否显示确认
                    zIndex: 99999999, //css z-index
                    choose: function (dates) { //选择好日期的回调
                        element.val(dates);
                        ngModel.$setViewValue(dates);
                    }
                };
                element.bind('click', function () {
                    var number_r = /^\d+$/;
                    var date_r = /^((\d{4})-(\d{2})-(\d{2}))|(\s{1}(\d{2}):(\d{2}):(\d{2}))$/;
                    var min = attrs["min"];
                    if( typeof (min) != "undefined")
                    {
                        if (number_r.test(min)) {
                            option.min = laydate.now(Number(min));
                        }
                        else if (date_r.test(min))
                        {
                            option.min = min;
                        }
                    }
                    var max = attrs["max"];
                    if (typeof (max) != "undefined") {
                        if (number_r.test(max)) {
                            option.max = laydate.now(Number(max));
                        }
                        else if (date_r.test(max)) {
                            option.max = max;
                        }
                    }
                    var start = attrs["start"];
                    if (typeof (start) != "undefined") {
                        var sv = scope.$parent.$eval(start);
                        if (typeof (sv) != "undefined" && date_r.test(sv)) {
                            option.min = sv;
                        }
                    }
                    var end = attrs["end"];
                    if (typeof (end) != "undefined") {
                        var ev = scope.$parent.$eval(end);
                        if (typeof (ev) != "undefined" && date_r.test(ev)) {
                            option.max = ev;
                        }
                    }
                    laydate(option);
                });
                element.bind('change', function () {
                    var v = element.val();
                    var p = /\d{8}/g;
                    var r = new RegExp(p);
                    if (r.test(v)) {
                        var dates = v.toDateFormat();
                        element.val(dates);
                        ngModel.$setViewValue(dates);
                    }
                });
                //element.attr("readonly", "readonly");
                //element.datepicker({
                //    showButtonPanel: true,
                //    closeText: "清空",
                //    currentText: "今天",
                //    dateFormat: "yy-mm-dd",
                //    dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六", "周日"],
                //    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                //    daysMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                //    monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                //    monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                //    prevText: '向前',
                //    nextText: '向后',
                //    changeMonth: true,
                //    changeYear: true
                //});
                //if (!$("#ui-datepicker-div").data("click")) {
                //    $("#ui-datepicker-div").data("click", true);
                //    $("#ui-datepicker-div").on("click", function (e) {
                //        var $this = $(e.target);
                //        if ($this.hasClass("ui-datepicker-close"))
                //        {
                //            var $input = $($.datepicker._curInst.input[0]);
                //            $input.val('');
                //            $input.trigger('change');
                //        }
                //    });
                //}
                scope.$watch('ngModel', function (newValue, oldValue, scope) {
                    if (angular.isDefined(newValue) && newValue != "" && newValue != null && isDate(newValue)) {
                        element.val(newDate(newValue).format("yyyy-MM-dd"));
                    }
                });
            }
        };
    }])
        .directive('kmCaDatePicker', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                require: ['ngModel'],
                scope: {
                    ngModel: '='
                },
                link: function (scope, element, attrs, ctrls) {
                    var ngModel = ctrls[0];
                    var option = {
                        format: 'YYYY-MM', //日期格式
                        istoday: true, //是否显示今天
                        issure: false, //是否显示确认
                        zIndex: 99999999, //css z-index
                        choose: function (dates) { //选择好日期的回调
                            element.val(dates);
                            ngModel.$setViewValue(dates);
                        }
                    };
                    element.bind('click', function () {
                        var number_r = /^\d+$/;
                        var date_r = /^((\d{4})-(\d{2})-(\d{2}))|(\s{1}(\d{2}):(\d{2}):(\d{2}))$/;
                        var min = attrs["min"];
                        if (typeof (min) != "undefined") {
                            if (number_r.test(min)) {
                                option.min = laydate.now(Number(min));
                            }
                            else if (date_r.test(min)) {
                                option.min = min;
                            }
                        }
                        var max = attrs["max"];
                        if (typeof (max) != "undefined") {
                            if (number_r.test(max)) {
                                option.max = laydate.now(Number(max));
                            }
                            else if (date_r.test(max)) {
                                option.max = max;
                            }
                        }
                        var start = attrs["start"];
                        if (typeof (start) != "undefined") {
                            var sv = scope.$parent.$eval(start);
                            if (typeof (sv) != "undefined" && date_r.test(sv)) {
                                option.min = sv;
                            }
                        }
                        var end = attrs["end"];
                        if (typeof (end) != "undefined") {
                            var ev = scope.$parent.$eval(end);
                            if (typeof (ev) != "undefined" && date_r.test(ev)) {
                                option.max = ev;
                            }
                        }
                        laydate(option);
                    });
                    element.bind('change', function () {
                        var v = element.val();
                        var p = /\d{8}/g;
                        var r = new RegExp(p);
                        if (r.test(v)) {
                            var dates = v.toDateFormat();
                            element.val(dates);
                            ngModel.$setViewValue(dates);
                        }
                    });
                    //element.attr("readonly", "readonly");
                    //element.datepicker({
                    //    showButtonPanel: true,
                    //    closeText: "清空",
                    //    currentText: "今天",
                    //    dateFormat: "yy-mm-dd",
                    //    dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六", "周日"],
                    //    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                    //    daysMin: ["日", "一", "二", "三", "四", "五", "六", "七"],
                    //    monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                    //    monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                    //    prevText: '向前',
                    //    nextText: '向后',
                    //    changeMonth: true,
                    //    changeYear: true
                    //});
                    //if (!$("#ui-datepicker-div").data("click")) {
                    //    $("#ui-datepicker-div").data("click", true);
                    //    $("#ui-datepicker-div").on("click", function (e) {
                    //        var $this = $(e.target);
                    //        if ($this.hasClass("ui-datepicker-close"))
                    //        {
                    //            var $input = $($.datepicker._curInst.input[0]);
                    //            $input.val('');
                    //            $input.trigger('change');
                    //        }
                    //    });
                    //}
                    scope.$watch('ngModel', function (newValue, oldValue, scope) {
                        if (angular.isDefined(newValue) && newValue != "" && newValue != null && isDate(newValue)) {
                            element.val(newDate(newValue).format("yyyy-MM-dd"));
                        }
                    });
                }
            };
        }]).directive('onFinishRepeatRender', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                if (scope.$last === true) {
                    scope.$emit('repeatFinished');
                }
            }
        };
    }]).directive('caSelect', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {

                function initAjaxSelect2() {
                    var url = element.attr("ajax-url");
                    element.select2({
                        ajax: {
                            url: url,
                            dataType: 'json',
                            delay: 1000,
                            data: function (params) {
                                return {
                                    code: params
                                };
                            },
                            results: function (data, params) {
                                var results = [];
                                $.each(data, function (index, item) {
                                    results.push({
                                        id: item.id,
                                        text: item.Code
                                    });
                                });
                                return {
                                    results: results
                                };
                            },
                            cache: true
                        }
                    });
                }

                function initSelect2() {
                    var elm = element.select2();
                    elm.on('select2:select', function (v) {
                        var model = this.attr("ngModel");
                        if (model) {
                            scope.$parent[model] = value;
                        }
                    });
                }

                if (element[0].nodeName == "SELECT") {
                    scope.$on("repeatFinished", function () {
                        initSelect2();
                    });
                } else {
                    initAjaxSelect2();
                }
            }
        };
    }])
        .directive('uiDataTable', ['$http', '$parse', function ($http, $parse) {
            return {
                restrict: 'EA',
                transclude: true,
                /*
                template: function (tElement, tAttrs) {
                    return "<div><div class='panel clearfix'><div class='col-md-4'><div class='col-md-12'><label>显示<select size='1' ng-model='options.pageInfo.PageSize' ng-options='t.Value as t.Text for t in options.selectRows.opt' aria-controls='datatable2' class='input-sm'>"
                        + "</select>行</label></div></div><div class='col-md-8' id='btnsArea' ng-click='options.buttonsClick($event)'></div></div><div ng-transclude>"
                        + "</div><div class='row'><div class='col-sm-12'> <div class='pull-left'><div class='dataTables_info' id='datatable1_info'>显示第 {{options.pageIndexRender.start}} 到 {{options.pageIndexRender.end}} 条 总共 {{options.pageIndexRender.sum}} 条</div></div>"
                        + "<div class='pull-right'><div class='dataTables_paginate paging_bs_full' id='datatable1_paginate'>"
                        + "<ul class='pagination' >"
                        + "<li ng-repeat='item in options.pageIndexRender.indexAry' class='{{item.cls.pl}}'><a href='#' class='{{item.cls.al}}' ng-click='changePage(item.params)'>{{item.Name}}</a></li>"
                        + "</ul>"
                        + "</div></div><div class='clearfix'></div></div></div><div>";
                },
                */
                scope: {
                    options: '='
                },
                template: function (tElement, tAttrs) {
                    //return "<div><div class='panel clearfix'><div class='col-md-4'><div class='col-md-12'><label>显示<select size='1' ng-model='options.pageInfo.PageSize' ng-options='t.Value as t.Text for t in options.selectRows.opt' aria-controls='datatable2' class='input-sm' ng-change='changePageSize()' >"
                    //    + "</select>行</label></div></div><div class='col-md-8 btnsArea' ng-click='options.buttonsClick($event)'></div></div><div ng-transclude>"
                    //    + "</div><div class='panel clearfix'><div class='col-sm-12'> <div class='pull-left'><div class='dataTables_info' id='datatable1_info'>显示第 {{options.pageIndexRender.start}} 到 {{options.pageIndexRender.end}} 条 总共 {{options.pageIndexRender.sum}} 条</div></div>"
                    //    + "<div class='pull-right'><div class='dataTables_paginate paging_bs_full' id='datatable1_paginate'>"
                    //    + "</div></div></div></div><div>";
                    return "<div ng-transclude></div>" +
                        "<div id='pageFooter' class='panel clearfix'>" +
                        "<div class='col-sm-2'><div class='dataTables_info'><label>显示<select size='1' ng-model='options.pageInfo.PageSize' ng-options='t.Value as t.Text for t in options.selectRows.opt' aria-controls='datatable2' class='input-sm' ng-change='changePageSize()' > </select>行</label></div></div>" +
                        "<div class='col-sm-4'><div class='dataTables_info' id='datatable1_info'>显示第 {{options.pageIndexRender.start}} 到 {{options.pageIndexRender.end}} 条 总共 {{options.pageIndexRender.sum}} 条</div></div>" +
                       "<div class='col-sm-6'><div class='dataTables_paginate paging_bs_full' id='datatable1_paginate'></div></div>" +
                        "</div>";
                },
                link: function ($scope, $elem, attrs) {
                    var opt = $scope.options,
                        pageSize = opt.pageInfo.PageSize;

                    var buttons = opt.buttons,
                        buttonsParent = $elem.find(".btnsArea");


                    function createBtn(btn) {
                        return "<a class='btn btn-default'><span>" + btn.btnType
                            + "</span><div><embed id='movie" + btn.btnType + "'"
                            + btn.btnType + "' src='/Content/CloudAdmin/js/datatables/extras/TableTools/media/swf/copy_csv_xls_pdf.swf' "
                            + "loop='false' menu='false' quality='best' bgcolor='#ffffff' width='39' height='29' name='movie" + btn.btnType + "'"
                            + "align='middle' allowscriptaccess='always' allowfullscreen='false' type='application/x-shockwave-flash' pluginspage='http://www.macromedia.com/go/getflashplayer' "
                            + "flashvars='id=2&amp;width=39&amp;height=29' wmode='transparent'></div></a>";
                    }

                    for (var i = buttons.length - 1; i > -1; i--) {
                        buttonsParent.append(createBtn(buttons[i]));
                    }

                    // 查询按钮事件
                    opt.search = function () {
                        $scope.search();
                    };

                    var render = function () {
                        pageSize = opt.pageInfo.PageSize;
                        var rows = opt.pageInfo.CurrentPage * pageSize,
                        start = (rows - pageSize) + 1,
                        sum = $scope.recordsCount,
                        end = rows > sum ? sum : rows;
                        opt.pageIndexRender.start = start;
                        opt.pageIndexRender.end = end;
                        opt.pageIndexRender.sum = sum;
                    }

                    $scope.search = function () {
                        if (opt.ajaxObject == null) {
                            return;
                        }
                        pageSize = opt.pageInfo.PageSize;
                        var params = {};
                        $.extend(params, { currentPage: opt.pageInfo.CurrentPage, pageSize: pageSize }, opt.params);
                        opt.ajaxObject.get(params, function (response) {
                            opt.success(response);
                            //debugger;
                            //if ((response.RecordsCount % opt.pageInfo.PageSize) === 0 && opt.pageInfo.CurrentPage > 1) {
                            //    opt.pageInfo.CurrentPage--;
                            //    $scope.search();
                            //    return;
                            //}
                            if (response.RecordsCount <= opt.pageInfo.PageSize) {
                                $elem.find("#pageFooter").hide();
                            } else {
                                $elem.find("#pageFooter").show();
                            }
                            $scope.recordsCount = response.RecordsCount;
                            var pager = new Pager($elem.find("#datatable1_paginate"), opt.pageInfo.CurrentPage, response.PagesCount, function (currentPage) {
                                opt.pageInfo.CurrentPage = currentPage;
                                render();
                                $scope.search();
                            });
                            render();
                        }, function (error) {//BobDu扩展请求失败的回调函数
                            opt.error(error);
                        });
                    };

                    $scope.changePageSize = function () {
                        opt.pageInfo.CurrentPage = 1;
                        $scope.search();
                    }

                    $scope.search();
                },
                controller: ['$scope', '$element', function ($scope, $element) {
                    $scope.options = angular.extend({
                        pageInfo: {
                            CurrentPage: 1, PageSize: 10
                        },
                        ajaxObject: null,
                        success: function () { },
                        selectRows: {
                            opt: [{ Value: 10, Text: "10" }, { Value: 20, Text: "20" }, { Value: 30, Text: "30" }]
                        },
                        buttons: [
                            { btnType: 'copy' },
                            { btnType: 'print' },
                            { btnType: 'pdf' },
                            { btnType: 'excel' }
                        ],
                        //buttonsClick: function (e) {

                        //},
                        renderPage: 0,
                        pageIndexRender: { start: 1, end: 10, indexAry: [] },
                        sumInfo: {}
                    }, $scope.options);
                }]
            }
        }]).directive("onlyNumber", function () {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    element.blur(function () {
                        if (!/^(-?\d+)(\.\d+)?$/.test($(this).val())) { //替换非数字字符(只能输入正负整数、正负小数)  
                            $(element).val("");
                        }
                    });
                }
            };
        }).directive("onlyInt", function () {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    element.blur(function () {
                        if (!/^-?\d+$/.test($(this).val())) { //替换非数字字符(只能输入正负整数)  
                            $(element).val("");
                        }
                    });
                }
            };
        }).directive("fex", function () {
            return {
                require: 'ngModel',
                restrict: 'A',
                link: function (scope, element, attrs, ctrl) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue, scope) {
                        if (newValue != null && typeof (newValue) != "undefined") {
                            var b = /(^$)|(^0\d{2,3}-\d{7,8}$)/.test(newValue);
                            ctrl.$setValidity(attrs.ngModel, b);
                        }
                    });
                }
            };
        }).directive("phone", function () {
            return {
                require: 'ngModel',
                restrict: 'A',
                link: function (scope, element, attrs, ctrl) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue, scope) {
                        if (newValue != null && typeof (newValue) != "undefined") {
                            var b = /^((0\d{2,3}-\d{7,8})|((\d|-){8,15})|(^$)|(1[3584]\d{9})|(([-_－—\s\(]?)([\(]?)((((0?)|((00)?))(((\s){0,2})|([-_－—\s]?)))|(([\)]?)[+]?))(886)?([\)]?)([-_－—\s]?)([\(]?)[0]?[1-9]{1}([-_－—\s\)]?)[1-9]{2}[-_－—]?[0-9]{3}[-_－—]?[0-9]{3}))$/.test(newValue);
                            ctrl.$setValidity(attrs.ngModel, b);
                        }
                    });
                }
            };
        }).directive("idcard", function () {
            return {
                require: 'ngModel',
                restrict: 'A',
                link: function (scope, element, attrs, ctrl) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue, scope) {
                        if (newValue != null && typeof (newValue) != "undefined") {
                            var b = /^(^$)|(^[a-zA-Z][0-9]{9}$)|(^[a-zA-Z]{2}[0-9]{8}$)|(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$/.test(newValue);
                            ctrl.$setValidity(attrs.ngModel, b);
                        }
                    });
                }
            };
        }).directive("email", function () {
            return {
                require: 'ngModel',
                restrict: 'A',
                link: function (scope, element, attrs, ctrl) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue, scope) {
                        if (newValue != null && typeof (newValue) != "undefined") {
                            var b = /^(^$)|(((\w)+(\.\w+)*@(\w)+((\.\w+)+))$)$/.test(newValue);
                            ctrl.$setValidity(attrs.ngModel, b);
                        }
                    });
                }
            };
        }).directive("twTimeFormat", function () {
            return {
                restrict: 'A',
                scope: {
                    inValue: '='
                },
                link: function (scope, element, attrs) {
                    element.change(function () {
                        var datetime = element.val().toTwDateFormat();
                        var arr = datetime.split('-');
                        if (arr.length == 3) {
                            var year = (parseInt(arr[0]) + 1911);
                            var month = arr[1].length == 1 ? "0" + arr[1] : arr[1];
                            var day = arr[2].length == 1 ? "0" + arr[2] : arr[2];
                            scope.inValue = year + "-" + month + "-" + day;
                        }
                        else {
                            scope.inValue = "";
                        }
                        scope.$apply();
                    });
                    scope.$watch('inValue', function (newValue, oldValue, scope) {
                        if (typeof (newValue) != "undefined" && newValue != null) {
                            element.val(newValue.toTwDate());
                        }
                    });
                }
            };
        }).directive("activeTr", function () {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    element.click(function () {
                        var tr = $(element);//.parent();
                        tr.parent().children("tr").each(function () {
                            $(this).attr("class", "ng-scope");
                        });
                        tr.attr("class", "ng-scope active");
                    });
                }
            };
        }).directive('tabs', function () {
            return {
                restrict: 'AE',
                transclude: true,
                scope: { gotopane: "@gotopane", fixedpane: "@fixedpane" },
                controller: ["$scope", function ($scope) {
                    var panes = $scope.panes = [];
                   $scope.select = function (pane) {
                       if (angular.isDefined($scope.fixedpane) && $scope.fixedpane=="true")
                       { } else {
                           angular.forEach(panes, function (pane) {
                               pane.selected = false;
                           });
                           pane.selected = true;
                       }
                    }
                    $scope.goto = function (num) {
                        angular.forEach(panes, function (pane) {
                            pane.selected = false;
                        });
                        if (panes.length > num) {
                            panes[num].selected = true;
                        }
                    }

                    this.addPane = function (pane) {
                        if (panes.length == 0) $scope.select(pane);
                        panes.push(pane);
                    }
                    $scope.$watch("fixedpane", function () {
                        $scope.fixed = $scope.fixedpane=="true";
                    }
                 );
                    $scope.$watch("gotopane", function (num) {
                        if (angular.isDefined(num)) {
                            if (num < 0) { num = 0 };
                            $scope.goto(num);
                        }}
                    );
                }],
                template:
                  '<div class="tabbable" >' +
                    '<ul class="nav nav-tabs">' +
                      '<li ng-repeat="pane in panes" ng-class="{active:pane.selected}">' +
                        '<a href="" ng-class="{tab_disable:fixed}" ng-click="select(pane)">{{pane.title}}</a>' +
                      '</li>' +
                    '</ul>' +
                    '<div class="tab-content" ng-transclude></div>' +
                  '</div>',
                replace: true
            };
        }).directive('pane', function () {
            return {
                require: '^tabs',
                restrict: 'E',
                transclude: true,
                scope: { title: '@' },
                link: function (scope, element, attrs, tabsCtrl) {
                    tabsCtrl.addPane(scope);
                },
                template:
                  '<div class="tab-pane" ng-class="{active: selected }" ng-transclude>' +
                  '</div>',
                replace: true
            };
        }).directive('unique', ['$http', '$timeout', function ($http, $timeout) {
            return {
                require: 'ngModel',
                restrict: 'A',
                link: function (scope, elem, attr, ctrl) {
                    var key = attr["uniqueKey"];
                    var type = attr["uniqueType"];
                    var param = attr["uniqueParam"];
                    var cacheQuery = [];
                    var completeTimeout;
                    var vaild = function () {
                        var value = scope.$eval(attr.ngModel);
                        if (angular.isDefined(value) && value != "" && value != null) {
                            var cacheValue = null;
                            //$.each(cacheQuery, function (i, v) {
                            //    if (v.checkValue == value) {
                            //        cacheValue = v.vaild;
                            //        return false;
                            //    }
                            //});  //添加此段代码会误判Lei
                            if (cacheValue != null) {
                                //ctrl.$setValidity(type, cacheValue);
                                //scope.$apply();
                            } else {
                                $timeout.cancel(completeTimeout);

                                // Attempt to aggregate any start/complete calls within 500ms:
                                completeTimeout = $timeout(function () {
                                    var keyValue = scope.$eval(key);
                                    var url = '/api/common/{0}/{1}/{2}'.format(type, keyValue, value);
                                    if (typeof (param) != 'undefined') {
                                        var ps = param.split(',');
                                        $.each(ps, function (i, name) {
                                            var value = scope.$eval(name);
                                            var c = "&";
                                            if (i == 0) {
                                                c = "?";
                                            }
                                            url += '{0}p{1}={2}'.format(c, i + 1, value)
                                        });
                                    }
                                    ctrl.$setValidity(attr.ngModel + "wait", false);
                                    $.ajax({
                                        type: "POST",
                                        url: url,
                                        dataType: "json",
                                        async: false,
                                        success: function (data) {
                                            ctrl.$setValidity(type, !data);
                                            //cacheQuery.push({ checkValue: value, vaild: !data });
                                        },
                                        complete: function (XMLHttpRequest, textStatus) {
                                            ctrl.$setValidity(attr.ngModel + "wait", true);
                                            scope.$apply();
                                        }
                                    });
                                }, 500);
                            }
                        }
                        else {
                            ctrl.$setValidity(attr.ngModel + "wait", true);
                        }
                    };
                   // elem.keyup(function () { vaild(); });
                    elem.blur(function () { vaild(); });
                }
            }
        }]).directive('customValid', [function () {
            return {
                require: "ngModel",
                restrict: 'A',
                scope: { valid: '&' },
                link: function (scope, elem, attrs, ctrl) {
                    var callFunction = function () {
                        scope.valid({
                            validity: function (name, v) {
                                ctrl.$setValidity(name, !v);
                            }
                        });
                    };
                    if (typeof (attrs.customValid) != "undefined") {
                        scope.$parent.$watch(attrs.customValid, function () {
                            callFunction();
                        });
                    }
                    scope.$parent.$watch(attrs.ngModel, function () {
                        callFunction();
                    });
               }
            };
        }]).directive("kmInclude", function () {//Bob Du扩展的把任何页面作为dialog打开的指令
            return ({
                controller: "@",
                name: "kmController",
                scope: {
                    kmIncludeParams: '=',
                    kmResult: '='
                },
                restrict: "A",
                templateUrl: function (elem, attrs) {
                    return attrs.kmTemplate;
                }
            });
        })
        .directive("backToTop", function () {//Bob Du扩展的回到顶部指令
            return ({
                restrict: "A",
                link: function (scope, element, attr) {
                    var e = $(element);
                    /*点击回到顶部*/
                    e.click(function () {
                        $('html, body').animate({                 //添加animate动画效果  
                            scrollTop: 0
                        }, 500);
                    });
                }
            });
        })
        .controller('kmLookupMulController', ['$scope', '$compile', function ($scope, $compile) {
            $scope.empty=function() {
                $scope.value = null;
            }
            $scope.showLookupDialog = function (colList, factoryName, searchParams, searchOptionsParams, title, itemProperty, single, isCanChoose) {
                var preOpenMessage = $scope.preOpen();
                if (preOpenMessage == false) {
                    return;
                }
                var params = { colList: colList, factoryName: factoryName, searchParams: searchParams, searchOptionsParams: searchOptionsParams, itemProperty: itemProperty, isCanChoose: isCanChoose };
                if (single) {
                    params.selectType = 'single';
                }
                var html = '<div km-include km-template="AppScripts/components/LookUp/LookUpView.html"' +
                                ' km-controller="LookUpController" km-include-params=\'' + JSON.stringify(params) + '\'></div>';
                BootstrapDialog.show({
                    title: "<label>"+title+"</label>",
                    type: BootstrapDialog.TYPE_DEFAULT,
                    message: html,
                    cssClass: 'staff-dialog',
                    size: BootstrapDialog.SIZE_WIDE,
                    onshow: function (dialog) {
                        var obj = dialog.getModalBody().contents();
                        $compile(obj)($scope);
                    },
                    onshown: function (dialog) {
                        var listener = $scope.$on('km-on-lookup-dialog-close-click', function (event, value) {
                            dialog.close();
                            event.stopPropagation();
                            listener();
                        });
                    }
                });
            };
        }])
         .filter("filterLookupName", function () {
             return function (input) {
                 if (input === undefined || input === null) {
                     return '';
                 }

                 if (_.isArray(input)) {
                     return (_.map(input, 'name')).join();
                 }
                 else if (_.isObject(input)) {
                     return input.name;
                 }

                 return ''
             };
         })
         .directive("kmLookUp", function () {//Bob Du扩展的通用的弹出框选择指令
             return ({
                 restrict: "E",
                 template: '<div class="input-group input-group-sm">'
                     + ' <input class="form-control" readonly  type="text" value={{value|filterLookupName}} ng-show="inputShow" />'
                     + '<span class="input-group-btn">'
                 + '<button class="btn btn-default" ng-click="showLookupDialog(colList,factoryName,searchParams,searchOptionsParams,title,itemProperty,single,isCanChoose)">'
                 + ' {{buttonName}}<span class="fa fa-search"></span>'
                 + '</button>'
                  + '<button ng-if="emptyButtonShow" class="btn btn-default" ng-click="empty()">'
                 + ' <span class="fa fa-times"></span>'
                 + '</button>'
                 + '</span>'
                 + '</div>',
                 require: ['?ngModel'],
                 scope: {
                     value:'=ngModel',
                     callbackFn: "&callback",
                     preOpen: "&preOpen",
                     colList: '=',
                     factoryName: '@',
                     searchParams: '=',
                     searchOptionsParams: '=',
                     title: '@',
                     itemProperty: '=',
                     buttonName: '@',
                     single: '=',
                     inputShow: '=',
                     isCanChoose: '=',
                     emptyButtonShow: '=',
                     required:'='
                 },
                 controller: 'kmLookupMulController',
                 link: function (scope, element, attr, ctrls) {
                     scope.$on('km-on-lookup-confirm-click', function (event, value) {
                         scope.value = value;
                         if (_.isArray(value)) {
                             scope.callbackFn({ item: _.map(value, 'data') });
                         }
                         else if (_.isObject(value)) {
                             scope.callbackFn({ item: value.data });
                         }
                         event.stopPropagation();
                     });
                     if (scope.required) {
                         var ngModel = ctrls[0];
                         ngModel.$setValidity('kmRequired', true);
                         scope.$watch('value', function (newVal, oldVal, scope) {
                             if (newVal === oldVal) {
                                 // 只会在监控器初始化阶段运行
                             } else {
                                 // 初始化之后发生的变化
                                 if (!newVal.id) {
                                     ngModel.$setValidity('kmRequired', false);
                                 } else {
                                     ngModel.$setValidity('kmRequired', true);
                                 }
                             }
                         });
                     }
                    
                 }
             });
         })
        .directive('checkItem', [function () {//bobDu扩展的指令用于福利信息
            return {
                require: ['ngModel'],
                restrict: 'A',
                scope: { item: '=' },
                link: function (scope, elem, attrs, ctrl) {
                    var ngModel = ctrl[0];
                    scope.$watch('item', function (newVal, oldVal, scope) {
                       if (newVal === oldVal) {
                             // 只会在监控器初始化阶段运行
                       } else {
                           // 初始化之后发生的变化
                           if (!newVal) {
                               ngModel.$setViewValue("");
                               ngModel.$render();
                           }
                       }
                    });
                   
                   
                }
            };
        }])
          .directive('checkDate', [function () {//bobDu扩展的指令用于校验日期手动输入规则
              return {
                  restrict: 'A',
                  link: function (scope, elem, attrs) {
                      $(elem).mask('0000-00-00');
                  }
              };
          }])
        .directive('mSelectMultiple', function () {
        return {
            restrict: 'A',
            require: ['ngModel'],
            link: function (scope, element, attrs, ctrls) {
                var ngModel = ctrls[0];
                var id = attrs['id'];
                var jsonData = attrs['selectData'];
                if (angular.isDefined(jsonData) && jsonData != "") {
                    var data = JSON.parse(jsonData);
                  var ms= $('#' + id).magicSuggest({
                        data: data,
                        //sortOrder: 'ItemCode',
                        displayField: 'ItemName',
                        valueField: 'ItemName',
                        //selectionPosition: 'bottom',
                        maxSelection: 1
                    });

                  $(ms).on('selectionchange', function (e, m) {
                      var arrayVals = this.getValue();
                      if (arrayVals.length > 0) {
                          var old = "",newValue="";
                          if(angular.isDefined(ngModel.$viewValue)){
                              old = ngModel.$viewValue;
                              newValue = old + '\n' + arrayVals[0];
                          } else {
                              newValue = arrayVals[0];
                          }
                          ngModel.$setViewValue(newValue);
                        } 
                    });

                }
            }
        };
        })
         .directive('datePicker', function () {
             return {
                 restrict: 'A',
                 require: 'ngModel',
                 scope: { dateFmt: '@' },
                 link: function (scope, element, attr, ngModel) {
                     element.val(ngModel.$viewValue);
                     function onpicking(dp) {
                         var date = dp.cal.getNewDateStr();
                         scope.$apply(function () {
                             ngModel.$setViewValue(date);
                         });
                     }
                     function onclearing() {
                         scope.$apply(function () {
                             ngModel.$setViewValue(null);
                         });
                     }
                     element.bind('click', function () {
                         WdatePicker({
                             onpicking: onpicking,
                             onclearing: onclearing,
                             dateFmt: scope.dateFmt ? scope.dateFmt : 'yyyy-MM-dd',
                         })
                     });
                 }
             }
         })
        .directive('bedStatus', function () {
            return {
                restrict: 'E',
                link: function (scope, element, attr) {
                    //TODO 研究如何在指令里引用枚举
                    switch (attr.status) {
                        case "Empty": element.addClass('bedStatus-empty'); break;
                        case "Subscribe": element.addClass('bedStatus-subscribe'); break;
                        case "Used": element.addClass('bedStatus-used'); break;
                        default:;
                    }
                },
                template: "<i class='fa fa-circle bedStatus '></i>"
            };
        }).directive('keditor', function ($rootScope, $sce) {

            var linkFn = function (scope, elm, attr, ctrl) {

                if (typeof window.KindEditor === 'undefined') {
                    throw new Error('Please import the local resources of kindeditor!');
                }
                var editor, isReady,
                    editorId = elm[0],
                    _config = {
                        width: '100%',
                        autoHeightMode: false,
                        afterCreate: function () {
                            this.loadPlugin('autoheight');
                        },
                        afterChange: function () {
                            var content = this.html();
                            if (isReady && editor) {
                                scope.$evalAsync(function () {
                                    ctrl.$setViewValue(content);
                                })

                            }
                        }

                    },
                    isReady = false,
                    editorConfig = angular.extend(_config, scope.config);

                editor = new KindEditor.create(editorId, editorConfig);
                KindEditor.ready(function () {
                    isReady = true;
                    var _content = ctrl.$isEmpty(ctrl.$viewValue) ? "" : ctrl.$viewValue;
                    editor.html(_content);
                })

                // 匹配正则
                var regObj = scope.pattern ? new RegExp(scope.pattern) : false;
                if (regObj) {
                    ctrl.$parsers.push(function (value) {
                        if (regObj.test(value)) {
                            ctrl.$setValidity(attr.ngModel, true)
                        } else {
                            ctrl.$setValidity(attr.ngModel, false)
                        }

                    })
                }

                scope.$watch(function () {
                    return  ctrl.$viewValue;
                }, function () {
                    if (!isReady) {
                        isReady = true;
                        var _content = ctrl.$viewValue;
                        editor.html(_content);
                    }
                });

                ctrl.$render = function () {
                    if (isReady && editor) {
                        //isReady = true;
                        var _content = ctrl.$isEmpty(ctrl.$viewValue) ? "" : ctrl.$viewValue;
                        editor.html(_content);
                    }
                }

            };

            return {
                restrict: 'AC',
                require: '^ngModel',
                scope: {
                    config: '=',
                    pattern: '='
                },
                link: linkFn
            };
        });

   
})();
