//个案转介
angular.module('sltcApp')
.controller('personReferralCtrl', ['$rootScope', '$scope', '$state', '$location', 'utility', 'dictionary', 'dc_PersonBasicRes', 'dc_ReferralRes',
    function ($rootScope, $scope, $state, $location, utility, dictionary, dc_PersonBasicRes, dc_ReferralRes) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.currentItem = {};
    $scope.currentItemReff = {};
    $scope.HistoryList = {};
    $scope.buttonShow = true;
    $scope.init = function () {
        $scope.Org = $rootScope.Global.Organization;
        $scope.sarch();
    };
    //选中住民
    $scope.residentSelected = function (resident) {
        
        $scope.FeeNo = resident.FeeNo;
        $scope.currentResident = resident;
        $scope.currentItem = {};//清空编辑项
        $scope.currentItemReff = {};
        $scope.sarch();
        if (angular.isDefined($scope.currentResident)) {
            $scope.buttonShow = false;
        }

        $scope.currentItem.OrgId = resident.OrgId;
        $scope.currentItem.RegNo = resident.RegNo;
        
        //获得当前登陆用户信息
        $scope.curUser = utility.getUserInfo();
        $scope.currentItem.CreateBy = $scope.curUser.EmpNo;
    }
    $scope.sarch = function () {

        if (angular.isDefined($scope.FeeNo) && $scope.FeeNo!='') {
            //加载基本资料表信息
            dc_PersonBasicRes.get({ orgId: '', feeNo: $scope.FeeNo, idNo: '', name: '', currentPage: 1, pageSize: 10 }, function (data) {
                $scope.currentItem = data.Data[0]; 
                $scope.ORGNAME = $scope.currentItem.OrgName;
                $scope.currentItem.BirthDate = new Date($scope.currentItem.BirthDate).format("yyyy-MM-dd");
                $scope.currentItem.InDate = new Date($scope.currentItem.InDate).format("yyyy-MM-dd");
                $scope.currentItem.Age = (new Date().getFullYear() - new Date($scope.currentItem.BirthDate).getFullYear());
            });
            //加载转介表信息,最新一条记录
            dc_ReferralRes.get({ feeNo: $scope.FeeNo, currentPage: 1, pageSize: 10 }, function (data) {
                $scope.currentItemReff = data.Data[0];
               // $scope.currentItemReff.Id = data.Data[0].Id;
                //if(data.Data!=null)
                    //$scope.currentItemReff.FeeNo = $scope.FeeNo;
            });
            
        }

    }
    //保存表单数据
    $scope.saveForm = function (item) {
        
        if (window.confirm("您确定要为" + $scope.currentResident.Name + "进行转介作业吗?")) {
            if ($scope.rfrForm.$valid) {//判断验证通过后才可以保存
                item.FeeNo = $scope.currentResident.FeeNo;
  
                dc_ReferralRes.save(item, function (data) {
                    if (angular.isDefined(item.FeeNo)) {
                        utility.message($scope.currentResident.Name + "转介成功！");
                        $scope.currentItemReff.Id = data.Data.Id;
                       // $location.url("/dc/PersonReferral/");
                    }
                    else {
                    }
                });
            }
            else {
                $scope.buttonShow = true;
                //验证没有通过
                $scope.getErrorMessage($scope.rfrForm.$error);
                $scope.errs = $scope.errArr.reverse();
                var count = 0;
                for (var n = $scope.errs.length; n--;) {
                    utility.msgwarning($scope.errs[n]);
                    count++;
                    if (count > 2) break;
                }
            }
        }
        
    };
    //编辑／修改表单数据
    $scope.editReferral = function (item) {
        if (angular.isDefined($scope.currentResident.RegNo)) {
            //$scope.currentItem = {};
            //$scope.currentItemReff = {};
            $("#historyModal").modal("toggle");
            //加载基本资料表信息
            dc_PersonBasicRes.get({ orgId: $scope.currentResident.OrgId, regNo: $scope.currentResident.RegNo, idNo: '', name: '', currentPage: 1, pageSize: 10 }, function (data) {
                $scope.currentItem = data.Data[0];
                $scope.currentItem.BirthDate = new Date($scope.currentItem.BirthDate).format("yyyy-MM-dd");
                $scope.currentItem.InDate = new Date($scope.currentItem.InDate).format("yyyy-MM-dd");
                
                $scope.currentItem.Age = (new Date().getFullYear() - new Date($scope.currentItem.BirthDate).getFullYear());
            });
         
            //加载转介表信息
            dc_ReferralRes.get({ id:item.Id }, function (data) {
                $scope.currentItemReff = data.Data;
               // $scope.currentItemReff.FeeNo = $scope.currentResident.FeeNo;
            });
            
        }
        
    };
    $scope.deleteReferral = function (item) {
        if (confirm("您确定要删除该条记录吗?")) {
            dc_ReferralRes.delete({ id: item.Id }, function (data) {
                if (data.$resolved) {
                    var whatIndex = null;
                    angular.forEach($scope.HistoryList, function (cb, index) {
                        if (cb.id = item.Id) whatIndex = index;
                    });

                    if (data.ResultCode == 0)
                        utility.message("资料删除成功！");
                    $scope.HistoryList.splice(whatIndex, 1);
                    $("#historyModal").modal("toggle");
                }
            });
        }
    }
    //查看历史记录
    $scope.showHistory = function () {
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            
            //加载转介表信息,历史记录
            dc_ReferralRes.get({ feeNo: $scope.currentResident.FeeNo, currentPage: 1, pageSize: 10 }, function (data) {
            
                $scope.HistoryList = data.Data;
            });

            
            $("#historyModal").modal("toggle");
        }
        else
            utility.message("您未选择住民，请选择要查询的住民信息后再点击查看！");
    }
    $scope.PrintPreview = function (id) {
 
        if (angular.isDefined(id)) {
            window.open('/DC_Report/Preview?templateName=DCS1.2&recId=' + id);
        } else {
            utility.message("未获取到个案转介记录,无法打印！");
        }

    }
    $scope.DownloadWord = function (id) {

        if (angular.isDefined(id)) {
            window.open('/DC_Report/DownLoadSocialReport?templateName=DCS1.2&recId=' + id);
        } else {
            utility.message("未获取到个案转介记录,无法导出！");
        }

    }
    //验证信息提示
    $scope.getErrorMessage = function (error) {
        $scope.errArr = new Array();
        if (angular.isDefined(error)) {
      
            if (error.required) {
                $.each(error.required, function (n, value) {
                    $scope.errArr.push(value.$name + "不能为空");
                });
            }
            if (error.email) {
                $.each(error.email, function (n, value) {
                    $scope.errArr.push(value.$name + "邮箱验证失败");
                });
            }
            if (error.number, function (n, value) {
                $scope.errArr.push(value.$name + "只能录入数字");
            });
            if (error.minlength) {
                $.each(error.minlength, function (n, value) {
                    $scope.errArr.push(value.$name + "录入数据的长度过短！");
                });

            }
            if (error.maxlength) {
                $.each(error.maxlength, function (n, value) {
                    $scope.errArr.push(value.$name + "录入数据的长度过长！");
                });
            }
            if (error.pattern) {
                $.each(error.pattern, function (n, value) {
                    $scope.errArr.push(value.$name + "验证失败！");
                });
            }
        }

    };
    $scope.init();
}]);

