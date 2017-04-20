//'use strict';
var homeApp = angular.module('sltcApp', [
        'ui.router',
        'ngResource',
        'ngCookies',
        'Utility',
        'extentDirective',
        'extentFilter',
        'extentComponent',
        'extentService',
        'angular-loading-bar',
        'ngAnimate'
])
     .constant("resourceBase", "http://120.25.225.5:5501/")
     .factory("empFileRes", ['$resource', function ($resource) {   //员工管理
         return $resource("api/empfiles/:id", { id: "@id" });
         //return $resource(resourceBase + "empfiles/:id", { id: "@id" });
     }])
    .factory("adminHandoversRes", ['$resource', function ($resource) { // 交付照会
        return $resource('api/AssignTask/:id', { id: '@id' });
        //return $resource(resourceBase + 'affairshandover/:id', { id: '@id' });
    }])
    .factory("groupRes", ['$resource', function ($resource) { //集团管理
        return $resource('api/group/:id', { id: '@id' });
    }])
    .factory("commonRes", ['$resource', function ($resource) { //获取一些公共的信息
        return $resource("api/Common/:id", { id: "@id" });
    }])
    .factory("roleModuleRes", ['$resource', function ($resource) {
        return $resource('api/module/:id', { id: '@id' });
    }])

  
    //需求护理评估
    .factory("DCNurseRequirementEvalRes", ['$resource', function ($resource) { //
        return $resource('api/DCNurseRequirementEval/:id', { id: '@id' });
    }])
    .factory('DCevalsheetRes', ['$resource', function ($resource, resourceBase) {
        return $resource("api/nursingSheet/:id", { id: "@id" });
    }])
    //个别化需求评估
    .factory("DCAdjuvantTherapyRes", ['$resource', function ($resource) { //
        return $resource('api/DCAdjuvantTherapy/:id', { id: '@id' });
    }])
    .factory("DCResidentRes", ['$resource', function ($resource, resourceBase) {
        return $resource('api/DCResidentRes/:id', { id: '@id' });
    }])
    //个案药品管理评估
    .factory("DrugManageRes", ['$resource', function ($resource, resourceBase) {
        return $resource('api/DrugManage/:id', { id: '@id' });
    }])
    //
    .factory("DCRegCPLRes", ['$resource', function ($resource, resourceBase) {
        return $resource('api/DCRegCPL/:id', { id: '@id' });
    }])
   .factory("DCNsCplGoalRes", ['$resource', function ($resource, resourceBase) {
       return $resource('api/DC_NSCPLGOAL/:id', { id: '@id' });
   }])
   .factory("DCNsCplActivityRes", ['$resource', function ($resource, resourceBase) {
       return $resource('api/DC_NSCPLACTIVITY/:id', { id: '@id' });
   }])
   .factory("DCAssessValueRes", ['$resource', function ($resource, resourceBase) {
       return $resource('api/DC_ASSESSVALUE/:id', { id: '@id' });
   }])
     .factory("deptRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //部门管理
         return $resource('api/Depts/:id', { id: '@id' });
     }])
    .factory("userRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //用户管理
        return $resource("api/users/:id", { id: "@id" }, {//bobdu扩展了方法
            ChangePassWord: {     //自定义的方法  
                method: 'POST',
                url: 'api/users/ChangePassWord',
            }
        });
        //return $resource(resourceBase + "users/:id", { id: "@id" });
    }])
    .factory("roleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //角色管理
        return $resource("api/roles/:id", { id: "@id" });
        //return $resource(resourceBase + "roles/:id", { id: "@id" });
    }])
    .factory("moduleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //模块管理
        return $resource("api/module/:id", { id: "@id" });
        //return $resource(resourceBase + "roles/:id", { id: "@id" });
    }])
    .factory("DCNursePlanRes", ['$resource', function ($resource, resourceBase) {
        return $resource('api/DC_REGCPL/:id', { id: '@id' });
    }])
    .factory("DCNursePlanRes2", ['$resource', function ($resource, resourceBase) {
        return $resource('api/DC_REGCPL2/:id', { id: '@id' });
    }])
    .factory("DCBasicDataArrangementRes", ['$resource', function ($resource, resourceBase) {
        return $resource('api/DCRegBaseInfoList/:id', { id: '@id' });
    }])
    //照顾计划
  .factory("DCCarePlanRes", ['$resource', function ($resource) { //
      return $resource('api/DCCarePlanRes/:id', { id: '@id' });
  }])
    .factory("DCCarePlanRes", ['$resource', function ($resource) { //
        return $resource('api/DCCarePlanRes/:id', { id: '@id' });
    }])
    .factory("dc_AssignJobsRes", ['$resource', function ($resource) {
        return $resource('api/AssignJobs/:id', { id: '@id' });
    }])
    .factory("dc_AssignTaskRes", ['$resource', function ($resource) {
        return $resource('api/DC_AssignTask/:id', { id: '@id' });
    }])
    .factory("dc_AssignWorkNoteRes", ['$resource', function ($resource) { 
        return $resource('api/dc_AssignWordNote/:id', { id: '@id' });
    }])
 .factory("DCNurseCareLifeServiceRes", ['$resource', function ($resource) { //
     return $resource('api/NurseCareLifeService/:id', { id: '@id' });
 }])
     .factory("commWordRes", ['$resource', function ($resource) {
         return $resource('api/CommWord/:id', { id: '@id' });
     }])

 .factory("DCDayLifeCareRes", ['$resource', function ($resource) { //

     return $resource('api/DCDayLifeCareRes/:id', { id: '@id' });
 }])

  .factory("DCDayLifeCareList", ['$resource', function ($resource) { //

      return $resource('api/DCDayLifeCarelist/:id', { id: '@id' });
  }])

  .factory("DCDayLifeCareListA", ['$resource', function ($resource) { //

      return $resource('api/DCDayLifeCarelistA/:id', { id: '@id' });
  }])
    .factory("CasesTimelineRes", ['$resource', function ($resource) { 

        return $resource('api/casesTimeline/:id', { id: '@id' });
    }])
   .factory("DCNurseCareLifeList", ['$resource', function ($resource) { //

       return $resource('api/DCNurseCareLifeList/:id', { id: '@id' });
   }])

    .factory("DCNurseCareLifeEdit", ['$resource', function ($resource) { //

        return $resource('api/DCNurseCareLifeEdit/:id', { id: '@id' });
    }])
    //问题行为异常情绪记录表
  .factory("DCProblemBehaviorList", ['$resource', function ($resource) { //

      return $resource('api/DCProblemBehaviorList/:id', { id: '@id' });
  }])

  .factory("DCProblemBehaviorHisList", ['$resource', function ($resource) { //

      return $resource('api/DCProblemBehaviorHisList/:id', { id: '@id' });

  }])

 //跨专业团队服务计划表

  .factory("DCProfessionalteamRes", ['$resource', function ($resource) { //
      return $resource('api/DCProfessionalteamList/:id', { id: '@id' });
  }])
  .factory("orgRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {  //机构管理
        return $resource("api/organizations/:id", { id: "@id" });
        //return $resource(resourceBase + "organizations/:id", { id: "@id" });
  }])
 .factory("dc_TaskgoalsstrategyRes", ['$resource', function ($resource) { //社工个案评估及处遇计划表
     return $resource('api/Taskgoalsstrategy/:id', { id: '@id' });
 }])

    .factory("DCProfessionalteamHisRes", ['$resource', function ($resource) { 
        return $resource('api/DCProfessionalteamHis/:id', { id: '@id' });
    }])

        //跨专业团队服务计划表2
     .factory("DCProfessionalteamExtRes", ['$resource', function ($resource) { //
         return $resource('api/DCProfessionalteamExt/:id', { id: '@id' });
     }])


    //活动管理 zhongyh
  .factory("DCActivityRes", ['$resource', function ($resource) {
      return $resource('api/DCActivity/:id', { id: '@id' });
  }])
   //字典管理list zhongyh
  .factory("DCDataDicListRes", ['$resource', function ($resource) {
      return $resource('api/DCDataDicList/:id', { id: '@id' });
  }])
      //字典管理edit zhongyh
  .factory("DCDataDicEditRes", ['$resource', function ($resource) {
      return $resource('api/DCDataDicEdit/:id', { id: '@id' });
  }])
   //字典管理子项处理 zhongyh
  .factory("DCDataDicEditDtlRes", ['$resource', function ($resource) {
      return $resource('api/DCDataDicEditDtl/:id', { id: '@id' });
  }])
//字典管理子项处理取机构信息 zhongyh
  .factory("DCDataDicOrglistRes", ['$resource', function ($resource) {
      return $resource('api/DCDataDicOrglist/:id', { id: '@id' });
  }])

 .factory("dc_SwRegEvalPlan", ['$resource', function ($resource) { //社工个案评估及处遇计划表
     return $resource('api/SwRegEvalPlan/:id', { id: '@id' });
 }])
 .factory("dc_PersonBasicRes", ['$resource', function ($resource) { //
     return $resource('api/PersonBasic/:id', { id: '@id' });
 }])
 .factory("dc_ReferralRes", ['$resource', function ($resource) { //个案转介
     return $resource('api/PersonReferral/:id', { id: '@id' });
 }])
 .factory("dc_PersonDayLifeRes", ['$resource', function ($resource) { //个案一天生活
     return $resource('api/PersonDayLife/:id', { id: '@id' });
 }])
 .factory("dc_LifeHistoryRes", ['$resource', function ($resource) { //个案生活史
     return $resource('api/LifeHistory/:id', { id: '@id' });
 }])
 .factory("dc_IpdRegRes", ['$resource', function ($resource) { //结案 
     return $resource('api/IpdReg/:id', { id: '@id' });
 }])
 .factory("dc_RegLifeQualityEvalRes", ['$resource', function ($resource) { //家庭照顾者生活品质评估问卷 　
     return $resource('api/RegLifeQualityEval/:id', { id: '@id' });
 }])
 .factory("dc_RegQuestionEvalRecRes", ['$resource', function ($resource) { //受托长辈适应程度评估表 　
     return $resource('api/RegQuestionEvalRec/:id', { id: '@id' });
 }])
 .factory("regCheckRecordRes", ['$resource', function ($resource) { //健康记录追踪
     return $resource('api/RegCheckRecord/:id', { id: '@id' });
 }])
 .factory("regCheckRecordDataRes", ['$resource', function ($resource) { //健康记录数据
     return $resource('api/RegCheckRecordData/:id', { id: '@id' });
 }])
 .factory("regNoteRecordRes", ['$resource', function ($resource) { //关怀记录数据
     return $resource('api/RegNoteRecord/:id', { id: '@id' });
 }])
 .factory("noteRes", ['$resource', function ($resource) { //关怀内容
     return $resource('api/Note/:id', { id: '@id' });
 }])
 .factory("regVisitRecordRes", ['$resource', function ($resource) { //访谈记录数据
     return $resource('api/RegVisitRecord/:id', { id: '@id' });
 }])
 .factory("checkTemplateRes", ['$resource', function ($resource) { //下拉框数据
     return $resource('api/CheckTemplate/:id', { id: '@id' });
 }])
     .factory("categoryRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/category', { id: '@id' });
     }])
    .factory('dictManageRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/DictManage/:id", { id: "@id" }, {
            GetFeeIntervalByMonth: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByMonth/:month',
            },
            GetFeeIntervalByDateStr: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByDateStr/:date'
            },
            GetFeeIntervalByDateTime: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByDateTime/:date',
            },
            GetFeeIntervalByYearMonth: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByYearMonth/:yearMonth',
            }
        });
    }])
;
