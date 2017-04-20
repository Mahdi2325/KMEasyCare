/*
创建人:BobDu
创建日期:2017-02-09
说明: 咨询登记
*/

angular.module("sltcApp")
.controller("advisoryRegCtrl", ['$scope', '$state', '$http', '$compile', 'advisoryRegRes', 'utility', function ($scope, $state, $http, $compile, advisoryRegRes, utility) {
    $scope.Data = {};
    $scope.searchWords = "";
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: advisoryRegRes,//异步请求的res
            params: { keywords: "", sDate: "", eDate: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.advisoryRegList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }
    $scope.AdvisoryRegDelete = function (item) {
        if (item.CallBackTime || item.ReservationBed) {
            utility.msgwarning("已预订床位或已有回访记录不能删除");
            return;
        }
        if (confirm("您确定删除该信息吗?")) {
            advisoryRegRes.delete({ id: item.Id }, function (data) {
                $scope.Data.advisoryRegList.splice($scope.Data.advisoryRegList.indexOf(item), 1);
                utility.message("删除成功");
            });
        }
    }
    $scope.hide = function () {
        $scope.dialog.close();
        $scope.options.search();
    }
    $scope.deposit = function (id) {
        var html = '<div km-include km-template="Views/ReceptionManagement/Deposit.html" km-controller="depositCtrl"  km-include-params="{id:\'' + id + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">订金</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
    $scope.search = function () {
        if ($scope.options.params.sDate && $scope.options.params.eDate) {
            if ($scope.options.params.eDate < $scope.options.params.sDate) {
                utility.msgwarning("咨询开始日期不能大于咨询结束日期");
                return;
            }
        }
        $scope.options.search();
    }
    $scope.init();
}])
.controller("depositCtrl", ['$scope', '$http', '$state', '$stateParams', '$rootScope', '$compile', 'advisoryRegRes', function ($scope, $http, $state, $stateParams, $rootScope,$compile, advisoryRegRes) {
    $scope.hide = function () {
        $scope.$parent.hide();
    }
    $scope.init = function () {
        $scope.Data = {};
        if ($scope.kmIncludeParams.id != "") {
            advisoryRegRes.get({ id: $scope.kmIncludeParams.id }, function (data) {
                $scope.Data = data.Data;
                $scope.Data.ReservationBed = { id: $scope.Data.ReservationBed, name: $scope.Data.ReservationBed };
            });
        }
    };
    $scope.submit = function () {
        $scope.Data.ReservationBed = $scope.Data.ReservationBed ? $scope.Data.ReservationBed.id : $scope.Data.ReservationBed;
        advisoryRegRes.save($scope.Data, function (data) {
            $scope.$parent.hide();
        });
    };
    $scope.init();
}])
.controller("advisoryRegEditCtrl", ['$scope', '$state', '$http', '$stateParams', '$location', 'advisoryRegRes', 'utility', function ($scope, $state, $http, $stateParams, $location, advisoryRegRes, utility) {
    $scope.Data = {};
    var empInfo = utility.getUserInfo();
    $scope.Data.RecordBy = empInfo.EmpNo;
    if ($stateParams.id) {
        advisoryRegRes.get({ id: $stateParams.id }, function (data) {
            $scope.Data = data.Data;
        });
    }

    $scope.save = function (item) {
        if (angular.isDefined($scope.advisoryReg.$error.required)) {
            for (var i = 0; i < $scope.advisoryReg.$error.required.length; i++) {
                utility.msgwarning($scope.advisoryReg.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.advisoryReg.$error.maxlength)) {
            for (var i = 0; i < $scope.advisoryReg.$error.maxlength.length; i++) {
                utility.msgwarning($scope.advisoryReg.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.advisoryReg.$error.number)) {
            for (var i = 0; i < $scope.advisoryReg.$error.number.length; i++) {
                utility.msgwarning($scope.advisoryReg.$error.number[i].$name + "格式不正确！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        advisoryRegRes.save(item, function () {
            utility.message("咨询登记信息保存成功！");
            $location.url('/angular/ReceptionManagement/AdvisoryReg');
        });

    };
}])
.controller("advisoryRegCallBackCtrl", ['$scope', '$state', '$http', '$stateParams', 'advisoryRegCallBackRes', 'utility', function ($scope, $state, $http, $stateParams, advisoryRegCallBackRes, utility) {
    $scope.Data = {};
    $scope.searchWords = "";
    $scope.ConsultRecId = $stateParams.ConsultRecId;
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: advisoryRegCallBackRes,//异步请求的res
            params: { startTime: "", endTime: "", consultRecId: $scope.ConsultRecId },
            success: function (data) {//请求成功时执行函数
                $scope.Data.advisoryRegCallBackList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    }
    $scope.AdvisoryRegCallBackDelete = function (item) {
        if (confirm("您确定删除该信息吗?")) {
            advisoryRegCallBackRes.delete({ id: item.Id }, function (data) {
                $scope.Data.advisoryRegCallBackList.splice($scope.Data.advisoryRegCallBackList.indexOf(item), 1);
                utility.message("删除成功");
            });
        }
    }
    $scope.init();
}])
.controller("advisoryRegCallBackEditCtrl", ['$scope', '$state', '$http', '$stateParams', '$location', 'advisoryRegCallBackRes', 'utility', 'advisoryRegRes', function ($scope, $state, $http, $stateParams, $location, advisoryRegCallBackRes, utility, advisoryRegRes) {
    $scope.Data = {};
    var advisoryRegData = {};
    $scope.ConsultRecId = $stateParams.ConsultRecId;
    if ($scope.ConsultRecId) {
        advisoryRegRes.get({ id: $scope.ConsultRecId }, function (data) {
            advisoryRegData = data.Data;
        });
    }
    if ($stateParams.id) {
        advisoryRegCallBackRes.get({ id: $stateParams.id }, function (data) {
            $scope.Data = data.Data;
        });
    }

    $scope.save = function (item) {
        if (item.CallBackTime < advisoryRegData.ConsultTime) {
            utility.msgwarning("回访时间不能早于咨询登记时间！");
            return;
        }
        if (angular.isDefined($scope.advisoryRegCallBack.$error.required)) {
            for (var i = 0; i < $scope.advisoryRegCallBack.$error.required.length; i++) {
                utility.msgwarning($scope.advisoryRegCallBack.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.advisoryRegCallBack.$error.maxlength)) {
            for (var i = 0; i < $scope.advisoryRegCallBack.$error.maxlength.length; i++) {
                utility.msgwarning($scope.advisoryRegCallBack.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.advisoryRegCallBack.$error.number)) {
            for (var i = 0; i < $scope.advisoryRegCallBack.$error.number.length; i++) {
                utility.msgwarning($scope.advisoryRegCallBack.$error.number[i].$name + "格式不正确！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
      
        item.ConsultRecId = $scope.ConsultRecId;
        advisoryRegCallBackRes.save(item, function () {
            utility.message("咨询登记信息保存成功！");
            $location.url('/angular/ReceptionManagement/AdvisoryRegCallBack/' + item.ConsultRecId);
        });

    };
}]);