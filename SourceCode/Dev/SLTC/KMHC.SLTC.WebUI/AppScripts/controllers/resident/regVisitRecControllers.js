/*
创建人:张凯
创建日期:2016-03-10
说明: 家庭亲友访视
*/
angular.module("sltcApp")
.controller("regVisitRecCtrl", ['$scope', 'dictionary', 'utility', 'familyDiscussrecRes', 'relationDtlRes', '$state','$location', function ($scope, dictionary, utility, familyDiscussrecRes, relationDtlRes,$state,$location) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.ipd = $state.params.IpdFlag;
    $scope.Data = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;
    $scope.curUser = utility.getUserInfo();
    if (typeof($scope.curUser) != 'undefined') {
        $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
    }

    var divwidth = $("#ContNamediv").width();

    $scope.autowidth = divwidth;

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: familyDiscussrecRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.RegVisitRecs = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                OrgId: ''
            }
        }

        var ss = $("#ContNamediv").width();
        $("#selVisitorName").css('width', ss);
        $("#spanwidth").css("margin-left", ss - 20);
        $("#selVisitorName").css('margin-left', -(ss - 20));
        $("#inputwidth").css('width', ss - 20);
    }
    
    //获取住民的亲友访视
    $scope.listItem = function (FeeNo, OrgId) {
        $scope.Data.RegVisitRecs = {};

        $scope.options.pageInfo.currentPage = 1;
        $scope.options.params.feeNo = FeeNo;
        $scope.options.params.OrgId = OrgId;
        $scope.options.search();

        //familyDiscussrecRes.query({ currentPage: 1, pageSize: 10, FeeNo: FeeNo, OrgId: OrgId }, function (data) {
        //    $scope.Data.RegVisitRecs = data;
        //});
    }


    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//加载当前住民的亲友访视记录
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
        }
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
        relationDtlRes.get({ FeeNo: $scope.currentResident.FeeNo, currentPage: 1, pageSize: 100 }, function (data) {
            $scope.Data.ContactList = data.Data;
        });
    }
    $scope.change = function () {
        var o = $("#selVisitorName").find("option:selected");
        if (o.length > 0) {
            $scope.currentItem.BloodRelationship = o.attr("kinship");
            $scope.currentItem.Appellation = o.attr("contrel");
        }
    }

    //删除亲友访视记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的亲友访视记录吗?")) {
            familyDiscussrecRes.delete({ id: item.Id }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                //$scope.Data.RegVisitRecs.splice($scope.Data.RegVisitRecs.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的亲友访视记录信息删除成功！");
            });
        }
    };

     $scope.createItem = function (item) {
        //新增亲友访视记录，得到住民ID
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentItem.Interviewee = $scope.currentResident.Name;
        $scope.checkBodyTemp();
        if ($scope.visitfrom.$valid) {//判断验证通过后才可以保存
            familyDiscussrecRes.save($scope.currentItem, function (data) {
                //$scope.Data.RegVisitRecs.push(data.Data);
                // $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//加载当前住民的亲友访视记录
                $scope.curUser = utility.getUserInfo();
                if (typeof ($scope.curUser) != 'undefined') {
                    $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
                }
                $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//加载当前住民的亲友访视记录
                utility.message($scope.currentResident.Name + "的亲友访视信息保存成功！");
            });
        }
        else {
            //验证没有通过
            $scope.getErrorMessage($scope.visitfrom.$error);
            $scope.errs = $scope.errArr.reverse();
            for (var n = 0; n < $scope.errs.length; n++) {
                if (n != 3) {
                    utility.msgwarning($scope.errs[n]);
                }
                //if (n > 2) break;
            }
        }
       
    };

    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.RecordBy = item.EmpNo;
    }

    $scope.updateItem = function (item) {
        $scope.checkBodyTemp();
        item.Interviewee = $scope.currentResident.Name;
        familyDiscussrecRes.save(item, function (data) {
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
            }
            $scope.listItem($scope.currentResident.FeeNo, $scope.currentResident.OrgId);//加载当前住民的亲友访视记录
            utility.message($scope.currentResident.Name + "的亲友访视信息保存成功！");
        });
    };

    $scope.rowSelect = function (item) {
        $scope.currentItem = item;
    };

    $scope.saveEdit = function (item) {
        $scope.checkBodyTemp();
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
    };

    $scope.changeGoAbroadPlace = function ()
    {
        if (!$scope.currentItem.IsGoAbroad)
        {
            $scope.currentItem.GoAbroadPlace = "";
        }
    }

    $scope.ChangeNextEvalDate = function () {

        if ($scope.currentItem.StartDate != "" && $scope.currentItem.EndDate != "") {
            //var days = DateDiff($scope.currentItem.EndDate, $scope.currentItem.StartDate)
            if (!checkDate($scope.currentItem.StartDate,$scope.currentItem.EndDate)) {
                utility.message("探视结束时间不能小于探视开始时间");
                $scope.currentItem.EndDate = "";

            } else {

            }
        };
    }

    $scope.checkBodyTemp = function () {
        //if (angular.isDefined($scope.currentItem.BodyTemp))
        //{
        //    var bodyTemp = $scope.currentItem.BodyTemp;
        //    if (bodyTemp < 0 || bodyTemp > 100) {
        //        utility.message("输入的体温不符合规范");
        //        $scope.currentItem.BodyTemp = "";
              
        //    } else {
        //        if (bodyTemp.toString().split(".").length > 1)
        //        {
        //            if (bodyTemp.toString().split(".")[1].length > 2) {
        //                utility.message("输入的体温不符合规范");
        //                $scope.currentItem.BodyTemp = "";
                      
        //            }
        //        }
        //    }
        //}
    };

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
                    $scope.errArr.push(value.$name + "邮箱验证失败");
                });
            }
            if (error.number, function (n, value) {
                $scope.errArr.push(value.$name + "只能录入数字");
            });
            if (error.pattern) {debugger
                $.each(error.pattern, function (n, value) {
                    $scope.errArr.push(value.$name + "只能录入正整数！");

                });
            }
            if (error.maxlength) {
                $.each(error.maxlength, function (n, value) {
                    $scope.errArr.push(value.$name + "录入已超过最大设定长度！");

                });
            }
            //return errorMsg;
        }
    }

    $scope.hrefVisitorInOutRec = function () {
        $location.url('/angular/VisitorInOutRec');
    }


    $scope.init();

}]);
