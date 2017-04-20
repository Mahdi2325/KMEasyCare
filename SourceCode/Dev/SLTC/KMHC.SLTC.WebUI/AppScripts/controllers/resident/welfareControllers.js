///创建人:刘美方
///创建日期:2016-02-22
///说明:福利信息

angular.module("sltcApp")
.controller('welfareCtrl', ['$scope', 'utility', '$http', 'welfareRes', '$state', 'regNCIInfoRes', function ($scope, utility,$http, welfareRes, $state, regNCIInfoRes) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.reginsdtl = {};
    $scope.buttonShow = false;
    $scope.isShowNCI = false;
    $scope.isShowLTC = false;
    $scope.isShowbtn = false;

    $scope.maxErrorTips = 3;
    //cloudAdminUi.initDatepicker();
    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.init();//加载当前住民
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }


    $scope.init = function () {
        $scope.isShowbtn = false;
        $scope.hasCertInfo = false;
        $scope.norCertInfo = false;
        $scope.needCertInfo = false;

        if ($scope.currentResident.FeeNo) {
            welfareRes.get({ id: $scope.currentResident.FeeNo }, function (data) {
                $scope.reginsdtl = data;
                if ($scope.reginsdtl.BookNo == null) {
                    $scope.reginsdtl.BookNo = $scope.currentResident.IdNo;
                }
            });

            $scope.AppCert = {};

            $http({
                url: 'api/welfare/?idNo=' + $scope.currentResident.IdNo,
                method: 'GET'
            }).success(function (data, header, config, status) {
                $scope.iserror = false;
                if (data.ResultCode == -1) {
                    $scope.isShowNCI = false;
                }
                else {
                    $scope.isShowNCI = true;
                    $scope.AppCert = data.Data;
                }
                $scope.loadLTCData();

            }).error(function (data, header, config, status) {
                utility.msgwarning("护理险平台无法连接，请联系管理员！");
                $scope.isShowNCI = false;
                $scope.iserror = true;
                $scope.loadLTCData();
            })
        }
    }

    $scope.loadLTCData = function ()
    {
        $scope.LTCCertInfo = {};
        //查询LTC现有资格信息
        regNCIInfoRes.get({ feeNo: $scope.currentResident.FeeNo }, function (data1) {
            if (data1.ResultCode == -1) {
                $scope.isShowLTC = false;
                $scope.hasCertInfo = false;
                $scope.norCertInfo = true;
                $scope.needCertInfo = false;
                if ($scope.isShowNCI && !$scope.iserror) {
                    $scope.isShowbtn = true;
                    $scope.hasCertInfo = true;
                    $scope.norCertInfo = false;
                    $scope.needCertInfo = false;
                }
            }
            else {
                $scope.isShowLTC = true;
                $scope.LTCCertInfo = data1.Data;
                if ($scope.isShowNCI) {
                    if ($scope.AppCert.CertNo == $scope.LTCCertInfo.Certno && $scope.AppCert.InHospDate == $scope.LTCCertInfo.ApplyHosTime && $scope.AppCert.Name == $scope.LTCCertInfo.RegName && $scope.AppCert.Gender == $scope.LTCCertInfo.Sex && $scope.AppCert.DiseaseTxt == $scope.LTCCertInfo.Diseasediag) {
                        $scope.isShowbtn = false;
                        $scope.hasCertInfo = true;
                        $scope.norCertInfo = false;
                        $scope.needCertInfo = false;
                    }
                    else {
                        $scope.isShowbtn = true;
                        $scope.hasCertInfo = false;
                        $scope.norCertInfo = false;
                        $scope.needCertInfo = true;
                    }
                }
                else {
                    $scope.iserror ? $scope.isShowbtn = false : $scope.isShowbtn = true;
                    if ($scope.iserror)
                    {
                        $scope.hasCertInfo = false;
                        $scope.norCertInfo = false;
                        $scope.needCertInfo = false;
                    }
                    else
                    {
                        $scope.hasCertInfo = false;
                        $scope.norCertInfo = false;
                        $scope.needCertInfo = true;
                    }
                }
            }

            if ($scope.isShowLTC) {
                $scope.CertTxt = "是";
            }
            else {
                $scope.CertTxt = "否";
                $scope.LTCCertInfo.RegName = "无";
                $scope.LTCCertInfo.IdNo = "无";
                $scope.LTCCertInfo.SsNo = "无";
                $scope.LTCCertInfo.Certno = "无";
                $scope.LTCCertInfo.Caretypeid = 9;
                $scope.LTCCertInfo.NCIPaylevel = "无";
                $scope.LTCCertInfo.NCIPayscale = "无";
            }
        });
    }

    $scope.SyncData = function () {
        $scope.CareInsInfo = {};
        $scope.CareInsInfo.FeeNo = $scope.currentResident.FeeNo;
        $scope.CareInsInfo.regNCIInfo = $scope.LTCCertInfo;
        $scope.CareInsInfo.appCertInfo = $scope.AppCert;

        regNCIInfoRes.welfareSave($scope.CareInsInfo, function (data) {
            $scope.init();
            utility.message("长期护理保险待遇资格信息同步成功！");
        });
    }

    $scope.saveItem = function () {
        if (!$scope.Validation()) {
            return;
        }
        $scope.reginsdtl.FeeNo = $scope.currentResident.FeeNo;
        $scope.reginsdtl.RegNo = $scope.currentResident.RegNo;
        $scope.reginsdtl.OrgId = $scope.currentResident.OrgId;
        welfareRes.save($scope.reginsdtl, function (data) {
            utility.message("保存成功");
        });
    };
    $scope.checkItem = function (item) {
        if (item) {
            $scope.reginsdtl.DISABILITYREEVALDATE = "";
        }
    }
    $scope.checkItem2 = function (item) {
        if (!item) {
            $scope.reginsdtl.INMIGRATIONDATE1 = "";
        }
    }
    $scope.checkItem3 = function (item) {
        if (!item) {
            $scope.reginsdtl.INMIGRATIONDATE2 = "";
        }
    }

    $scope.Validation = function () {
        var errorTips = 0;
        if (angular.isDefined($scope.myForm.$error.required)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.required.length; i++) {
                msg = $scope.myForm.$error.required[i].$name + " 为必填项";
                utility.msgwarning(msg);
                errorTips++;
                if (errorTips >= $scope.maxErrorTips) {
                    return false;
                }
            }
        }

        if (angular.isDefined($scope.myForm.$error.maxlength)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.maxlength.length; i++) {
                msg = $scope.myForm.$error.maxlength[i].$name + "超过设定长度 ";
                utility.msgwarning(msg);
                errorTips++;
                if (errorTips >= $scope.maxErrorTips) {
                    return false;
                }
            }
        }

        if (angular.isDefined($scope.myForm.$error.pattern)) {
            var msg = "";
            for (var i = 0; i < $scope.myForm.$error.pattern.length; i++) {
                msg = $scope.myForm.$error.pattern[i].$name + getValidateMsg($scope.myForm.$error.pattern[i].$name);
                utility.msgwarning(msg);
                errorTips++;
                if (errorTips >= $scope.maxErrorTips) {
                    return false;
                }
            }
        }
        if (errorTips > 0) { return false; }
        return true;
    }

}]);
