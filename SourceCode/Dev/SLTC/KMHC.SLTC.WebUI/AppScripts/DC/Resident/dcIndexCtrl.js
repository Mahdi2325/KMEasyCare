angular.module("sltcApp")
    .controller("dc_msgListCtr", ['$rootScope', '$scope', '$filter', '$compile', '$state', 'adminHandoversRes', 'dc_AssignJobsRes', '$cookieStore', function ($rootScope, $scope, $filter, $compile,$state, adminHandoversRes, dc_AssignJobsRes, $cookieStore) {
        $scope.init = function() {
            $scope.Data = {
                Msgs: []
            }
            adminHandoversRes.query({}, function(data) {
                var tData = [], length = data.length;
                for (var i = 0; i < length; i++) {
                    if (data[i].AssignName == "李湘") {
                        tData.push(data[i]);
                    }
                }
                $scope.Data.Msgs = tData;
            });
            $scope.loginUser = "李湘";

            var global = $cookieStore.get('Global');
            if (isEmpty(global) || global.CurrentLoginSys != 'DC') {
                dc_AssignJobsRes.get({}, function (data) {
                    $rootScope.Global = data.Data;
                    $cookieStore.put('Global', data.Data);
                })
            } else {
                $rootScope.Global = global;
            }           
        }

        $scope.logout = function ()
        {
            $cookieStore.remove('Global');
            location.href = "/Home/Logout";
        }
        $scope.hide = function () {
            $scope.dialog.close();
        }
        $scope.changePass = function () {
            var html = '<div km-include ' +
               'km-template="Views/Home/changeMyPass.html" ' +
               'km-controller="changeMyPassController" </div>';

            $scope.dialog = BootstrapDialog.show({
                title: '修改密码',
                message: html,
                onshow: function (dialog) {
                    dialog.getModalHeader().css('background-color', '#08c');
                    dialog.getModalHeader().css('color', '#fff');
                    dialog.getModalFooter().hide();
                    var obj = dialog.getModalBody().contents();
                    $compile(obj)($scope);
                }
            });
        };
        $scope.changeAccount = function (id) {
            $state.go("MyAccount", { id: id });
        };
        $scope.init();

    }])
.controller("dc_menuCtr", ['$scope', '$state', '$location', 'cloudAdminUi', 'roleModuleRes', function ($scope, $state, $location, cloudAdminUi, roleModuleRes) {
        $scope.init = function () {
            $scope.menus = [];
            roleModuleRes.query(function (data) {
                $scope.menus = data;
                $scope.selectMent = null;
                var arr = $.grep($scope.menus, function (item, i) {
                    if (item.Target == "2") {
                        if (item.Url != null) {
                            var url = encodeURI(item.Url);
                            return $location.$$url.indexOf(url) > -1;
                        } else {
                            return false;
                        }
                    }
                    else {
                        var url = "";
                        var stateName = "";
                        if (item.Url != null) {
                            var stateArr = item.Url.split(".");
                            $.each(stateArr, function (i, item) {
                                stateName = stateName != "" ? stateName + "." + item : item;
                                var p = $state.get(stateName);
                                if (p != null) {
                                    url = url + p.url;
                                }
                            });
                        }
                        if (url != "") {
                            url = encodeURI(url.replace(":id", ""));
                            return url != null && $location.$$url.indexOf(url) > -1;
                        }
                        else {
                            return false;
                        }
                    }
                });
                if (arr.length > 0) {
                    $scope.selectMent = arr[0];
                }
                cloudAdminUi.handleSidebar();
            });
        }
        $scope.displayblock = {
            "display": "block"
        }
        $scope.displaynone = {
            "display": "none"
        }
        var getTarget = function (t) {
            if (t.tagName === "A") {
                return t;
            } else if (t.tagName === "IMG") {
                return $(t).parent().parent();
            } else {
                return $(t).parent();
            }
        }
        $scope.init();
        $scope.menuClick = function ($event, url) {
            if (angular.isDefined(url) && url != ""&& url != null) {
                $location.url(url);
            } else {
                var target = getTarget($event.target);//$event.target.closest("a");
                var last = $('.has-sub.open', $('.sidebar-menu'));
                last.removeClass("open");
                $('.arrow', last).removeClass("open");
                $('.sub', last).slideUp(200);
                var thisElement = $(target);
                var slideOffeset = -200;
                var slideSpeed = 200;

                var sub = $(target).next();
                if (sub.is(":visible")) {
                    $('.arrow', $(target)).removeClass("open");
                    $(target).parent().removeClass("open");
                    sub.slideUp(slideSpeed, function () {
                        if ($('#sidebar').hasClass('sidebar-fixed') === false) {
                            App.scrollTo(thisElement, slideOffeset);
                        }
                        cloudAdminUi.handleSidebarAndContentHeight();
                    });
                } else {
                    $('.arrow', jQuery(target)).addClass("open");
                    $(target).parent().addClass("open");
                    sub.slideDown(slideSpeed, function () {
                        if ($('#sidebar').hasClass('sidebar-fixed') === false) {
                            App.scrollTo(thisElement, slideOffeset);
                        }
                        cloudAdminUi.handleSidebarAndContentHeight();
                    });
                }
            }
         
        }
        $scope.subMenuClick = function ($event, url, way) {
            var last = $('.has-sub', $('.sidebar-menu'));
            last.children(".sub").children(".active").removeClass("active");
            var target = getTarget($event.target);
            var id = $('.selectedCor td').eq(0).text();
           // jQuery(target).parent().parent().children(".active").removeClass("active");
            jQuery(target).parent().addClass("active");
            if (way==='1') {
                $state.go(url, { id: 0 });
            } else if (way === '2') {
                if (id != '') {
                    url = url + id;
                }
                $location.url(url);
            } else {
                $state.go(url);
            }
        }

        $scope.active = function (menu) {
            if ($scope.selectMent != null && ($scope.selectMent.Url == menu.Url || $scope.selectMent.SuperModuleId == menu.ModuleId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
}]);

