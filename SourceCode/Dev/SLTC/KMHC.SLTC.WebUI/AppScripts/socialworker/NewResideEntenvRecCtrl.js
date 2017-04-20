angular.module('sltcApp')
.controller('NewResideEntenvRecCtrl', ['$scope', 'utility', '$filter', 'dictionary', 'utility', 'NewResideEntenvRecRes', 'empFileRes', '$state', function ($scope, utility, filter, dictionary, utility, NewResideEntenvRecRes, empFileRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.btnShow = true;
    $scope.currentPage = 1;
    $scope.Data = {};
    $scope.EmpList = {};
    $scope.currentResident = {};
    //获取当前时间
    $scope.titlename = "(新增)专案内容-承办人员";
    $scope.Data.RecordDate = new Date().format("yyyy-MM-dd hh:mm:ss");
    $scope.currentDate = new Date().format("yyyy-MM-dd hh:mm:ss");
    $scope.namestr = "";
    $scope.FamilyFun = function (type) {
        if (type == true) {

            return "是";

        } else if (type == false) {

            return "否";

        }
        return "";
    }
    $scope.init = function (ResidengNo, orgid) {

        empFileRes.get({ empNo: '', currentPage: 1, pageSize: 100 }, function (data) {
            $scope.EmpList = data.Data;
        })

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NewResideEntenvRecRes,//异步请求的res
            params: {
                feeNo: ResidengNo,
                orgid: "",
            },
            success: function (data) {//请求成功时执行函数
                $scope.Data.items = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }


        var ss = $(".col-md-3").width();
        $(".selVisitorName").css('width', ss);
        $(".spanwidth").css("margin-left", ss - 20);
        $(".selVisitorName").css('margin-left', -(ss - 20));
        $(".inputwidth").css('width', ss - 20);
    }

    $scope.PrintPreview = function (item) {

        window.open('/DC_Report/PreviewLTC_SocialReport?templateName=LTC_NEWEnvironmentRec&id=' + item.ID + '&feeName=' + $scope.namestr);
    }

    $scope.save = function (item) {
        item.FAMILYPARTICIPATION = $scope.FAMILYPARTICIPATION;
        NewResideEntenvRecRes.save(item, function (data) {

            if (data.ResultCode == 0) {
                utility.message("保存成功！");
                $scope.titlename = "(新增)专案内容-承办人员";
                if (angular.isDefined(item.FEENO)) {
                    //$scope.init(item.FEENO, "");
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.pageInfo.PageSize = 10;
                    $scope.options.params.feeNo = item.FEENO;
                    $scope.options.params.orgid = "";
                    $scope.options.search();
                }
                var lsBEDNO = $scope.currentItem.BEDNO;
                var lsresidengno = $scope.currentItem.RESIDENGNO;
                var lsname = $scope.currentItem.Name;
                var birthdate = $scope.currentItem.BIRTHDATE;
                var recorddate = $scope.currentItem.RECORDDATE = new Date().format("yyyy-MM-dd");
                $scope.currentItem = {};
                $scope.currentItem.BEDNO = lsBEDNO;
                $scope.currentItem.RESIDENGNO = lsresidengno;
                $scope.currentItem.Name = lsname;
                $scope.currentItem.BIRTHDATE = birthdate;
                $scope.currentItem.RECORDDATE = new Date().format("yyyy-MM-dd");
                $scope.currentItem.SEX = $scope.currentResident.Sex;
                $scope.currentItem.Age = new Date().getFullYear() - new Date($scope.currentResident.BirthDay).getFullYear();
                $scope.currentItem.INDATE = $scope.currentResident.InDate;
                $scope.currentItem.FEENO = $scope.currentResident.FeeNo;
                $scope.currentItem.REGNO == $scope.currentResident.RegNo;
                $scope.namestr = lsname;

                if (data.FAMILYPARTICIPATION == true) {
                    $scope.FAMILYPARTICIPATION = "true";
                }
                else if (data.FAMILYPARTICIPATION == false) {
                    $scope.FAMILYPARTICIPATION = "false";
                }
                else {
                    $scope.FAMILYPARTICIPATION = null;
                }

            }
        })
    }

    $scope.changefacility = function (value) {
        if (value) {
            $scope.currentItem.COMMUNITYFACILITIES = true;
        }
    };

    //选中住民
    $scope.residentSelected = function (resident) {

        $scope.currentResident = resident;
        $scope.currentItem = {};//清空编辑项
        if (angular.isDefined($scope.currentItem)) {
            $scope.buttonShow = true;
        }
        $scope.currentItem.BEDNO = resident.BedNo;
        $scope.currentItem.RESIDENGNO = resident.ResidengNo;
        $scope.currentItem.Name = resident.Name;
        $scope.currentItem.BIRTHDATE = new Date(resident.BirthDay).format("yyyy-MM-dd");
        $scope.currentItem.RECORDDATE = new Date().format("yyyy-MM-dd");
        $scope.currentItem.INDATE = resident.InDate;
        $scope.currentItem.SEX = resident.Sex;
        $scope.currentItem.FEENO = resident.FeeNo;
        $scope.currentItem.REGNO == resident.RegNo;
        $scope.currentItem.CONTRACTFLAG = true;
        $scope.currentItem.LIFEFLAG = true;
        $scope.currentItem.REGULARACTIVITY = true;
        $scope.currentItem.NOTREGULARACTIVITY = true;
        $scope.currentItem.BELLFLAG = true;
        $scope.currentItem.LAMPFLAG = true;
        $scope.currentItem.TVFLAG = true;
        $scope.currentItem.LIGHTSWITCH = true;
        $scope.currentItem.ESCAPEDEVICE = true;
        $scope.currentItem.ENVIRONMENT = true;
        $scope.currentItem.COMMUNITYFACILITIES = true;
        $scope.currentItem.POSTOFFICE = true;
        $scope.currentItem.SCHOOL = true;
        $scope.currentItem.BANK = true;
        $scope.currentItem.STATION = true;
        $scope.currentItem.PARK = true;
        $scope.currentItem.TEMPLE = true;
        $scope.currentItem.HOSPITAL = true;
        $scope.currentItem.OTHERFACILITIES = true;
        $scope.currentItem.CLEANLINESS = true;
        $scope.currentItem.MEDICALCARE = true;
        $scope.currentItem.MEALSERVICE = true;
        $scope.currentItem.WORKACTIVITIES = true;
        $scope.currentItem.PERSONINCHARGE = true;
        $scope.currentItem.DIRECTOR = true;
        $scope.currentItem.NURSE = true;
        $scope.currentItem.NURSEAIDES = true;
        $scope.currentItem.RESIDENT = true;
        $scope.currentItem.DOCTOR = true;
        $scope.currentItem.SOCIALWORKER = true;
        $scope.currentItem.DIETITIAN = true;
        $scope.currentItem.OTHERPEOPLE = true;
        var curUser = utility.getUserInfo();
        $scope.currentItem.STAFF1 = curUser.EmpName;
        $scope.currentItem.STAFF2 = curUser.EmpName;
        $scope.currentItem.STAFF3 = curUser.EmpName;
        $scope.currentItem.STAFF4 = curUser.EmpName;
        $scope.currentItem.STAFF5 = curUser.EmpName;
        $scope.FAMILYPARTICIPATION = null;
        $scope.namestr = resident.Name;
        $scope.currentItem.Age = new Date().getFullYear() - new Date(resident.BirthDay).getFullYear();
        $scope.titlename = "(新增)专案内容-承办人员";
        if (!(resident.FeeNo == null || resident.FeeNo == "undefind")) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.pageInfo.PageSize = 10;
            $scope.options.params.feeNo = resident.FeeNo;
            $scope.options.params.orgid = "";
            $scope.options.search();
        }
        else { $scope.Data.items = {} }
    }

    //$scope.BirthDayChange = function (currentItem) {
    //    $scope.currentItem.Age = (new Date().getFullYear() - new Date(currentItem.BIRTHDATE).getFullYear());
    //}


    $scope.resetData = function () {
        $scope.currentItem = {};
        $scope.currentItem.BEDNO = $scope.currentResident.BedNo;
        $scope.currentItem.RESIDENGNO = $scope.currentResident.ResidengNo;
        $scope.currentItem.Name = $scope.currentResident.Name;
        $scope.currentItem.BIRTHDATE = new Date($scope.currentResident.BirthDay).format("yyyy-MM-dd");
        $scope.currentItem.RECORDDATE = new Date().format("yyyy-MM-dd");
        $scope.currentItem.INDATE = $scope.currentResident.InDate;
        $scope.currentItem.SEX = $scope.currentResident.Sex;
        $scope.currentItem.FEENO = $scope.currentResident.FeeNo;
        $scope.currentItem.REGNO == $scope.currentResident.RegNo;
        $scope.currentItem.Age = new Date().getFullYear() - new Date($scope.currentResident.BirthDay).getFullYear();
    }

    //选择填写人员
    $scope.staffSelected = function (item) {
        $scope.currentItem.Name = item.EmpName;
        $scope.currentItem.RESIDENGNO = item.EmpNo;
    }

    $scope.rowSelect = function (item) {
        var lsname = $scope.currentItem.Name;
        $scope.currentItem = {};
        $scope.currentItem = item;
        $scope.currentItem.BIRTHDATE = new Date($scope.currentResident.BirthDay).format("yyyy-MM-dd");
        $scope.currentItem.Name = lsname;
        $scope.titlename = "(修改)专案内容-承办人员";
        $scope.currentItem.Name = $scope.namestr;
        //$scope.BirthDayChange(item); //注释 by Duke on 2016-07-21
        //NewResideEntenvRecRes.get({ id: item.ID }, function (data) {
        //    $scope.currentItem = data.Data ;

        //});
        $scope.currentItem.Age = new Date().getFullYear() - new Date($scope.currentResident.BirthDay).getFullYear();
        if (item.FAMILYPARTICIPATION == true) {
            $scope.FAMILYPARTICIPATION = "true";
        }
        else if (item.FAMILYPARTICIPATION == false) {
            $scope.FAMILYPARTICIPATION = "false";
        } else {
            $scope.FAMILYPARTICIPATION = null;
        }
    }

    $scope.Delete = function (item) {
        var residenno = item.FEENO;
        if (confirm("您确定要删除新住民环境介绍记录吗?")) {
            NewResideEntenvRecRes.delete({ id: item.ID }, function () {
                //$scope.init(residenno, "");
                $scope.Data.items.splice($scope.Data.items.indexOf(item), 1);
            }
            )
        }
    }
    $scope.init("-1", "")
}]);
