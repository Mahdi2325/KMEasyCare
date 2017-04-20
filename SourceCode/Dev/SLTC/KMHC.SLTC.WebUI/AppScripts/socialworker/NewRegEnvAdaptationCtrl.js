angular.module('sltcApp')
.controller('NewRegEnvAdaptationCtrl', ['$scope', '$rootScope', 'utility', '$filter', 'dictionary', 'NewRegEnvAdaptationRes', '$state', function ($scope, $rootScope, utility, filter, dictionary, NewRegEnvAdaptationRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.btnShow = true;
    $scope.currentPage = 1; 
    $scope.Data = {};
    $scope.currentResident = {};
    var isEdit = false;
    //获取当前时间
    $scope.titlename = "(新增)追踪记录-追踪评估";
    $scope.availableOptions = [];
    $scope.namestr = "";


    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NewRegEnvAdaptationRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                    $scope.Data.items = data.Data;
                    $scope.Name = $scope.namestr;
                    $scope.RecordsCount = data.RecordsCount;
                  //$scope.WEEK = $scope.currentItem.WEEK;
                    $scope.availableOptions = [];
                    for (var i = 1; i <= $scope.RecordsCount + 4; i++) {
                        $scope.availableOptions.push(i);
                    }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                FEENO: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                orgid: '' 
            }
        }
    };
    

    $scope.loadInfo = function (feeNo) {
        $scope.Data.items = {};

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.params.FEENO = feeNo;
        $scope.options.params.orgid = "";
        $scope.options.search();

    };

    $scope.PrintPreview = function (item) {
         
        window.open('/DC_Report/PreviewLTC_SocialReport?templateName=LTC_NEWEnvironmentTutor&id=' + item.ID + '&feeName=' + $scope.namestr);
    }

    $scope.resetData = function ()
    {
        $scope.currentItem = {};
        $scope.currentItem.Name = $scope.currentResident.Name;
        $scope.currentItem.INDATE = $scope.currentResident.InDate;
        $scope.currentItem.FEENO = $scope.currentResident.FeeNo;
        $scope.currentItem.REGNO == $scope.currentResident.RegNo;
    }

    $scope.checkINDATE = function () {
        if (!checkDate($scope.currentItem.INDATE, $scope.currentItem.W1EVALDATE)) {
            $scope.currentItem.INDATE = "";
            utility.msgwarning("入住日期不能大于评估日期");
        }
    }
    $scope.checkW1EVALDATE = function () {
        if (!checkDate($scope.currentItem.INDATE, $scope.currentItem.W1EVALDATE)) {
            $scope.currentItem.W1EVALDATE = "";
            utility.msgwarning("评估日期不能小于入住日期");
        }
    }


    $scope.save = function (item) {
        var weekCount = parseInt(item.WEEK);
        var result1 = _.where($scope.Data.items, { WEEK: weekCount });
        var result2 = _.find($scope.Data.items, { WEEK: weekCount - 1 });
        var result3 = _.find($scope.Data.items, { WEEK: weekCount + 1 });
        if (isEdit) {
            if (result1.length>1) {
                utility.message("该周数据已存在");
                return;
            }
        } else {
            if (result1.length > 0) {
                utility.message("该周数据已存在");
                return;
            }
        }
       
        if (result2 != null && result3 != null) {
            if (item.W1EVALDATE > result3.W1EVALDATE || item.W1EVALDATE < result2.W1EVALDATE) {
                utility.message("评估日期应该大于上次评估日期且小于下次评估日期");
                return;
            }
           
        }
        if (result2 != null) {
            if (item.W1EVALDATE < result2.W1EVALDATE) {
                utility.message("评估日期应该大于上次评估日期");
                return;
            }
           
        }
        if (result3 != null) {
            if (item.W1EVALDATE > result3.W1EVALDATE) {
                utility.message("评估日期应该小于下次评估日期");
                return;
            }
          
        }
        NewRegEnvAdaptationRes.save(item, function (data) {
            
                if (data.ResultCode == 0) {
                    utility.message("保存成功！");
                    $scope.titlename = "(新增)追踪记录-追踪评估";

                    $scope.loadInfo($scope.currentResident.FeeNo);
                     
                    var lsresidengno = $scope.currentItem.FEENNO;
                    var lsname =$scope.currentItem.Name  ;
                 
                    var recorddate=$scope.currentItem.RECORDDATE = new Date().format("yyyy-MM-dd");
                    $scope.currentItem= {};
                   
                    $scope.currentItem.FEENO = $scope.currentResident.FeeNo;;
                    $scope.currentItem.Name = lsname;
                    $scope.currentItem.INDATE = $scope.currentResident.InDate;
                   
                    $scope.namestr = lsname;
                } 
          } )  
    }
 
    //选中住民
    $scope.residentSelected = function (resident) {
         
        $scope.currentResident = resident;
 
        $scope.currentItem = {};//清空编辑项
        if (angular.isDefined($scope.currentItem)) {
            $scope.buttonShow = true;
        }
        $scope.currentItem.Name = resident.Name; 
        $scope.currentItem.INDATE = resident.InDate; 
        $scope.currentItem.FEENO = resident.FeeNo;
        $scope.currentItem.REGNO == resident.RegNo;
        $scope.namestr = resident.Name;
        $scope.CREATEBY = utility.getUserInfo();
        $scope.currentItem.CREATEBY = $scope.CREATEBY.EmpNo;
        $scope.titlename = "(新增)追踪记录-追踪评估";
        if (!(resident.FeeNo == null || resident.FeeNo == "undefind"))
        {
            $scope.loadInfo(resident.FeeNo);
        }
         else
        { $scope.Data.items = {}} 
    }

    
    //选择填写人员
    $scope.staffSelected = function (item) 
    {
          //$scope.currentItem.Name = item.EmpName;
        $scope.currentItem.CREATEBY = item.EmpNo;
          
    }

    $scope.rowSelect = function (item) 
    {
        var lsname = $scope.currentItem.Name;
       // $scope.currentItem = item;0
        $scope.currentItem.Name = lsname;
        $scope.titlename = "(修改)追踪记录-追踪评估";
        isEdit = true;
        NewRegEnvAdaptationRes.get({ id: item.ID }, function (data) {
            $scope.currentItem = data.Data ;
            $scope.currentItem.Name = $scope.namestr;
            $scope.currentItem.WEEK = $scope.currentItem.WEEK + '';
        });
    }

    $scope.Delete = function (item) {
        var residenno =  item.FeeNo;
        if (confirm("您确定要删除新住民环境适应及辅导记录吗?")) {
            NewRegEnvAdaptationRes.delete({ id: item.ID }, function () {

                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();

                //$scope.Data.items.splice($scope.Data.items.indexOf(item), 1);
                //$scope.init(residenno, ""); //注释 By Duke On 2016-07-22 
              }
            ) 
        }
    }
    $scope.init();
         
}]);
