

angular.module("sltcApp")
    .controller("visitorInOutListCtrl", ['$scope', '$http', '$state', '$location', 'utility', 'visitorInOutRecRes', '$compile', function ($scope, $http, $state, $location, utility, visitorInOutRecRes, $compile) {

        //开始时间和结束时间
        var preMonthDate = new Date();
        preMonthDate.setDate(1);
        preMonthDate.setMonth(preMonthDate.getMonth());
        var preYear = preMonthDate.getFullYear();
        var preMonth = preMonthDate.getMonth();
        $scope.StartDate = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
        $scope.EndDate = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");

        //获得某月的天数
        function getMonthDays(nowYear, myMonth) {
            var monthStartDate = new Date(nowYear, myMonth, 1);
            var monthEndDate = new Date(nowYear, myMonth + 1, 1);
            var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
            return days;
        }


        //初始化数据源
        $scope.Data = {};
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: visitorInOutRecRes,//异步请求的res
            params: { sDate: $scope.StartDate, eDate: $scope.EndDate, keyWord: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Visitors = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }

        $scope.checksDate = function () {
            if (angular.isDefined($scope.StartDate) && angular.isDefined($scope.EndDate)) {
                if (!checkDate($scope.StartDate, $scope.EndDate)) {
                    utility.msgwarning("来访时间开始日期应该在来访时间截止日期之前");
                    $scope.EndDate = "";
                    return;
                }
            }
        }

        $scope.checkeDate = function () {
            if ($scope.StartDate != "" && $scope.EndDate != "") {
                if (angular.isDefined($scope.StartDate) && angular.isDefined($scope.EndDate)) {
                    if (!checkDate($scope.StartDate, $scope.EndDate)) {
                        utility.msgwarning("来访时间截止日期应该在来访时间开始日期之后");
                        $scope.EndDate = "";
                        return;
                    }
                }
            }
            else {
                utility.msgwarning("来访时间开始日期或截止日期不能为空");
                return;
            }
        }

        $scope.edit = function (item) {

            if (!item.IsRegVisit) {
                var params = { Id: item.Id, StartDate: item.StartDate, VisitorName: item.VisitorName, VisitorSex: item.VisitorSex, VisitorIdNo: item.VisitorIdNo, VisitorCompany: item.VisitorCompany, Interviewee: item.Interviewee, Description: item.Description, EndDate: item.EndDate, Remark: item.Remark };
                var html = '<div km-include km-template="Views/ReceptionManagement/VisitorInOutRecEdit.html" km-controller="VisitorInOutRecEditCtrl"  km-include-params=\'' + JSON.stringify(params) + '\'}" ></div>';
                $scope.dialog = BootstrapDialog.show({
                    title: '<label class=" control-label">参观来宾登记</label>',
                    cssClass: 'pop-dialog',
                    type: BootstrapDialog.TYPE_DEFAULT,
                    message: html,
                    size: BootstrapDialog.SIZE_WIDE,
                    onshow: function (dialog) {
                        var obj = dialog.getModalBody().contents();
                        $compile(obj)($scope);
                    }
                });
            }
            else {
                //window.open("/Resident/RegVisitRec.html?feeNo={0}".format(4), "_blank");
                $location.url('/angular/RegVisitRec/{0}/{1}'.format(item.FeeNo, item.IpdFlag));
            }
        }

        $scope.delete = function (id) {
            if (confirm("确定删除该来宾的出入信息吗?")) {
                visitorInOutRecRes.delete({ Id: id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            }
        };

        $scope.searchInfo = function () {
            $scope.options.params.sDate = $scope.StartDate;
            $scope.options.params.eDate = $scope.EndDate;
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
        };

        $scope.Hide = function () {
            $scope.dialog.close();
        }

        $scope.visitorRegister = function () {
            var params = {};
            var html = '<div km-include km-template="Views/ReceptionManagement/VisitorInOutRecEdit.html" km-controller="VisitorInOutRecEditCtrl"  km-include-params=\'' + JSON.stringify(params) + '\'}" ></div>';
            $scope.dialog = BootstrapDialog.show({
                title: '<label class=" control-label">参观来宾登记</label>',
                cssClass: 'pop-dialog',
                type: BootstrapDialog.TYPE_DEFAULT,
                message: html,
                size: BootstrapDialog.SIZE_WIDE,
                onshow: function (dialog) {
                    var obj = dialog.getModalBody().contents();
                    $compile(obj)($scope);
                }
            });

        };
        $scope.visitResident = function () {
            $location.url('/angular/RegVisitRec/0/I');
        };
    }])


    .controller("VisitorInOutRecEditCtrl", ['$scope', 'visitorInOutRecRes', '$http', '$stateParams', 'utility', '$compile','$location', function ($scope, visitorInOutRecRes, $http, $stateParams, utility, $compile,$location) {

        $scope.currentItem = {};
        console.log($scope.kmIncludeParams);
        if ($scope.kmIncludeParams != null) {
            $scope.currentItem.Id = $scope.kmIncludeParams.Id;
            $scope.currentItem.StartDate = $scope.kmIncludeParams.StartDate;
            $scope.currentItem.VisitorName = $scope.kmIncludeParams.VisitorName;
            $scope.currentItem.VisitorSex = $scope.kmIncludeParams.VisitorSex;
            $scope.currentItem.VisitorIdNo = $scope.kmIncludeParams.VisitorIdNo;
            $scope.currentItem.VisitorCompany = $scope.kmIncludeParams.VisitorCompany;
            $scope.currentItem.Interviewee = $scope.kmIncludeParams.Interviewee;
            $scope.currentItem.Description = $scope.kmIncludeParams.Description;
            $scope.currentItem.EndDate = $scope.kmIncludeParams.EndDate;
            $scope.currentItem.Remark = $scope.kmIncludeParams.Remark;
        }

        $scope.ChangeNextEvalDate = function () {

            if ($scope.currentItem.StartDate != "" && $scope.currentItem.EndDate != "") {
                //var days = DateDiff($scope.currentItem.EndDate, $scope.currentItem.StartDate)
                if (!checkDate($scope.currentItem.StartDate, $scope.currentItem.EndDate)) {
                    utility.message("离去时间不能小于来访时间");
                    $scope.currentItem.EndDate = "";

                } else {

                }
            };
        }

 
        $scope.SaveVisitRec = function (item) {
            visitorInOutRecRes.save(item, function (data) {
                if (data.ResultCode == -1) {
                    utility.message("保存失败！" + data.ResultMessage);
                }
                else {
                    $scope.$parent.searchInfo();
                    $scope.$parent.Hide();
                    utility.message("保存成功！");
                }


            });
        };
        $scope.backList = function () {

            $location.url('/angular/VisitorInOutRec');
        };
        //$scope.init();

    }]);
