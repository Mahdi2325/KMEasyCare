//收案Or结案
angular.module('sltcApp')
.controller('ipdRegCtrl', ['$rootScope', '$scope', '$location', 'utility', '$state', 'dc_PersonBasicRes', 'dc_IpdRegRes', 'empFileRes',
    function ($rootScope, $scope, $location, utility, $state,dc_PersonBasicRes,  dc_IpdRegRes, empFileRes) {
    $scope.FeeNo = $state.params.FeeNo; 
    $scope.currentItem = {};
    $scope.currentBasic = {};
    $scope.editOrAdd = "add";
    $scope.ipdFlag = {};
    $scope.currentItem.IdNo = '';
    $scope.buttonShow = true;
    $scope.currentResident = {};
    $scope.IsDisabled = true;
    $scope.resetSave = false;
    $scope.Org = $rootScope.Global.Organization;
    $scope.init = function () {
        
     
        $scope.IsInvalid = false;
        if ($scope.FeeNo != "")
            $scope.sarch();
        //获取下拉字典数据
    
        empFileRes.get({ empNo: '', empName: '', empGroup: '', currentPage: 0, pageSize: 0 }, function (response) {
            var dest = getEmpMemberByGroup(response.Data);
            if (!isEmpty(dest)) {
                for (var i = 0; i < dest.length; i++) {
                    if (dest[i].EmpGroup == "001") {
                        $scope.NurseNoData = dest[i].data;
                    } else if (dest[i].EmpGroup == "002") {
                        $scope.CarerData = dest[i].data;
                    } else if (dest[i].EmpGroup == "003") {
                        $scope.NutritionistData = dest[i].data;
                    } else if (dest[i].EmpGroup == "005") {
                        $scope.PhysiotherapistData = dest[i].data;
                    } else if (dest[i].EmpGroup == "006") {
                        $scope.DoctorData = dest[i].data;
                    } else if (dest[i].EmpGroup == "009") {//9对应照服员
                        $scope.NurseAides = dest[i].data;
                    }
                }
            }
        });
        $scope.currentItem.OutDate = new Date().format("yyyy-MM-dd");
    };
    
    function getEmpMemberByGroup(arr) {
        var map = {}, dest = [];
        for (var i = 0; i < arr.length; i++) {
            var ai = arr[i];
            if (!map[ai.EmpGroup]) {
                dest.push({ EmpGroup: ai.EmpGroup, data: [ai] });
                map[ai.EmpGroup] = ai;
            } else {
                for (var j = 0; j < dest.length; j++)
                { var dj = dest[j]; if (dj.EmpGroup == ai.EmpGroup) { dj.data.push(ai); break; } }
            }
        }
        return dest;
    };
    function isEmpty(value) {
        if (value == null || value == "" || value == "undefined" || value == undefined || value == "null") { return true; } else {  //value = value.replace(/\s/g, "");
            if (value == "") {
                return true;
            }
            return false;
        }
    }
    

    $scope.checkIdNo = function () {
        if (angular.isDefined($scope.currentItem.IdNo)) {
            dc_IpdRegRes.get({ idNo: $scope.currentItem.IdNo, type: 1 }, function (data) {
             
                if (data.Data != null) {
                    if (data.Data.IpdFlag == "I") {

                        alert("已存在个案信息,无须再次收案！");
                        $scope.resetSave = false;
                        $scope.currentItem = data.Data;
                        $scope.IsDisabled = true;

                        
                    }
                    else {
                        alert("已存在个案信息,建议直接重新开案！");
                        $scope.resetSave = true;
                        $scope.currentItem = data.Data;
                        $scope.IsDisabled = true;
                        $scope.currentItem.InDate = new Date().format("yyyy-MM-dd");
                    }
                    //debugger;
                    //$scope.FeeNo = data.Data.FeeNo;
                    //$scope.sarch();
                }
            });
        }
       
    }
    //查询用户基本资料
    $scope.sarch = function () {
        
        if (angular.isDefined($scope.FeeNo)) {
            $scope.buttonShow = false;
            dc_IpdRegRes.get({ feeNo: $scope.FeeNo, currentPage: 1, pageSize: 10 }, function (data) {
                
                if ($state.current.name != "IpdregOut") {
                    $scope.currentItem = data.Data[0];
                    $scope.currentItem.BirthDate = new Date($scope.currentResident.BirthDate).format("yyyy-MM-dd");
                    $scope.currentItem.InDate = new Date($scope.currentResident.InDate).format("yyyy-MM-dd");
                   // $scope.currentItem.BirthDate = $scope.currentItem.BirthDate.substr(0, $scope.currentItem.BirthDate.indexOf(T));
                    $scope.ipdFlag = data.Data[0].IpdFlag;
                }
                else {
                    $scope.currentItem.RegName = $scope.currentResident.Name;
                    $scope.currentItem.BirthDate = new Date($scope.currentResident.BirthDate).format("yyyy-MM-dd");
                    $scope.currentItem.IdNo = $scope.currentResident.IdNo;
                    $scope.currentItem.MerryState = $scope.currentResident.MerryState;
                    $scope.currentItem.Phone = $scope.currentResident.Phone;
                    $scope.currentItem.PermanentAddress = $scope.currentResident.PermanentAddress;
                    $scope.currentItem.LivingAddress = $scope.currentResident.LivingAddress;
                    $scope.currentItem.InDate = $scope.currentResident.InDate;
                }
                if ($scope.ipdFlag == "O")
                    $scope.resetSave = true;
            });
        }

    }
   
    //选中住民
    $scope.residentSelected = function (resident) {
   
        $scope.FeeNo = resident.FeeNo;
        $scope.currentResident = resident; 
        $scope.currentItem = {};//清空编辑项
        $scope.sarch();
        $scope.editOrAdd = "edit";
        if (angular.isDefined($scope.currentResident)) {
            $scope.buttonShow = false;
        }
        //获得当前登陆用户信息
        $scope.curUser = utility.getUserInfo();
        $scope.currentItem.CreateBy = $scope.curUser.EmpNo;
        if ($scope.currentResident.IpdFlag == "I")
        {
            $scope.resetSave = false;
        }
        $scope.IsDisabled = true;
    }
    //保存收案
    $scope.saveIn = function (item, editOrAdd) {
     
        if ($scope.inForm.$valid) {//判断验证通过后才可以保存
            if (!angular.isDefined(item.Sex)) {
                utility.message("请选择个案性别！");
                return;
            }
            
            if ($scope.resetSave == true) {
                $scope.currentItem.isAdd = "1";
            }
            else {
                $scope.currentItem.isAdd = $scope.editOrAdd == "add" ? "1" : "3";
            }
            
            var tpiMsg = $scope.currentItem.isAdd == "1" ? "您确定要进行收案作业吗?" : "您确定要修改资料吗？";
            
            if (window.confirm(tpiMsg)) {
                dc_IpdRegRes.save(item, function (data) {
                    
                    if (data.ResultCode == -1) {
                        utility.message(data.ResultMessage);
                    }
                    else if (data.ResultCode == 0) {
                        if ($scope.editOrAdd == "edit" && !$scope.resetSave) {
                            var cDate = new Date().format("yyyy-MM-dd");
                            utility.message(item.RegName + " 于  " + cDate + "  资料修改成功！");
                            $state.go('IpdRegIn', null, {
                                reload: true
                            });
                        }
                        else {
                            utility.message(item.RegName + "  收案成功！");
                            
                            $scope.currentItem.FeeNo = data.Data.FeeNo;
                            $scope.editOrAdd = "edit";
                            $state.go('IpdRegIn', null, {
                                reload: true
                            });
                        }
                        $scope.FeeNo = data.Data.FeeNo;
                    }
                });
            }
            //$location.url("/dc/IpdregIn/");
           
        }
        else {
            //验证没有通过
            $scope.getErrorMessage($scope.inForm.$error);
            $scope.errs = $scope.errArr.reverse();
            for (var n = 0; n < $scope.errs.length; n++) {
                if (n != 3) {
                    utility.msgwarning($scope.errs[n]);
                }
                //if (n > 2) break;
            }
        }
        
    };

    //身份证输入完成后回车校验事件
    $scope.myKeyup = function (e) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) {
            $scope.checkPerson();
        }
    };
    //根据姓名＋身份证字号验证重复
    //$scope.checkPerson = function () {
    //    //加载基本资料表信息
    //    dc_PersonBasicRes.get({ orgId: '', regNo: '', idNo: $scope.currentItem.IdNo, name: $scope.currentItem.RegName, currentPage: 1, pageSize: 10 }, function (data) {
    //        if (data.Data[0] != null && $scope.ipdFlag=="I") {
    //            utility.message("个案已存在！");
    //        }
    //        else {
                
    //            utility.message("恭喜，该个案可以进行收案！");
    //        }
    //    });
    //}
    $scope.addResident = function () {
        $scope.currentItem = {};
        $scope.editOrAdd = "add";
        $scope.FeeNo = 0;
        $scope.resetSave = false;
        $scope.IsDisabled = false;
        $scope.buttonShow = true;
    }
    //验证信息提示
    $scope.getErrorMessage = function (error) {
        $scope.errArr = new Array();
        //var errorMsg = '';
        if (angular.isDefined(error)) {
            if (error.required) {
                $.each(error.required, function (n, value) {
                    $scope.errArr.push(value.$name + "不能为空");
                });
            }
            if (error.email) {
                $.each(error.email, function (n, value) {
                    $scope.errArr.push(value.$name+"邮箱验证失败");
                });
            }
            if (error.number, function (n, value) {
                $scope.errArr.push(value.$name+"只能录入数字");
            });
            if (error.minlength) {
                $.each(error.minlength, function (n, value) {
                    $scope.errArr.push(value.$name + "格式： 身份证格式错误 ！");
                });
                
            }
            if (error.maxlength) {
                $.each(error.maxlength, function (n, value) {
                    if (value.$name == "日字号") {
                        $scope.errArr.push(value.$name + "录入非法,已超过最大设定长度！");
                    }
                    else if (value.$name == "简易服务摘要") {
                        $scope.errArr.push(value.$name + "录入非法,已超过最大设定长度！");
                    } else {
                        $scope.errArr.push(value.$name + "格式： 身份证格式错误！");
                    }
                    
                });
            }
            //return errorMsg;
        }
        
    };
    $scope.PrintPreview = function (type) {
        
        if (angular.isDefined($scope.FeeNo)) {
            if(type==1)
                window.open('/DC_Report/Preview?templateName=DCS1.7&recId=' + $scope.FeeNo);
            else 
                window.open('/DC_Report/Preview?templateName=DCS1.5&recId=' + $scope.FeeNo);
        } else {
            utility.message("未获取到个案生活史记录,无法打印！");
        }

    }

    $scope.DownloadWord = function (type) {

        if (angular.isDefined($scope.FeeNo)) {
            if (type == 1)
                window.open('/DC_Report/DownLoadSocialReport?templateName=DCS1.7&recId=' + $scope.FeeNo);
            else
                window.open('/DC_Report/DownLoadSocialReport?templateName=DCS1.5&recId=' + $scope.FeeNo);
        } else {
            utility.message("未获取到个案生活史记录,无法导出！");
        }

    }

    $scope.init();
}]);

