//'use strict';


// Declare app level module which depends on filters, and services
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
        'ngAnimate',
        'angularFileUpload',
        'extentEnum'
])
    .constant("resourceBase", "http://120.25.225.5:5501/")
    .factory("commonRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //获取一些公共的信息
        return $resource("api/Common/:id", { id: "@id" });
    }])
    .factory("personRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //人员信息
        //return $resource(resourceBase + "persons/:id", { id: "@id" });
        return $resource("api/Person/:id", { id: "@id" });
    }])
     .factory("personExtendRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //人员信息
         //return $resource(resourceBase + "persons/:id", { id: "@id" });
         return $resource("api/PersonExtend/:id", { id: "@id" });
     }])
    .factory("residentBriefRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //住民信息
        //return $resource(resourceBase + 'residents/:id', { id: '@id' });
        return $resource('api/Resident/:id', { id: '@id' });
    }])
    .factory("relationRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //联系信息
        return $resource("api/Relation/:id", { id: "@id" });
    }])
    .factory("relationDtlRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //亲属联系信息
        //return $resource(resourceBase + "contact/:id", { id: "@id" });
        return $resource("api/RelationDtl/:id", { id: "@id" });
    }])
    .factory("healthManageRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/healthManageRes/:id", { id: "@id" });
        //return $resource(resourceBase + "health-manage/:id", { id: "@id" });

    }])
    .factory("dc_AssignJobsRes", ['$resource', function ($resource) {
        return $resource('api/AssignJobs/:id', { id: '@id' });
    }])
    .factory("orgRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {  //机构管理
        return $resource("api/organizations/:id", { id: "@id" });
        //return $resource(resourceBase + "organizations/:id", { id: "@id" });
    }])
    .factory("relativeRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + "relatives/:id", { id: "@id" });
    }])
    .factory("homePhotoRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + "home-photo/:id", { id: "@id" });
    }])
    .factory("attachArchiveRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //附加文件
        //return $resource(resourceBase + "attach-archives/:id", { id: "@id" });
        return $resource("api/AttachFile/:id", { id: "@id" });
    }])
    .factory("codeRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        //return $resource(resourceBase + "codes/:id", { id: "@id" });
        return $resource("api/Code/:id", { id: "@id" });
    }])
        .factory("codeFileRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
            //return $resource(resourceBase + "codes/:id", { id: "@id" });
            return $resource("api/CodeFile/:id", { id: "@id" });
        }])
            .factory("codeDtlRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
                //return $resource(resourceBase + "codes/:id", { id: "@id" });
                return $resource("api/codedtl/:id", { id: "@id" });
            }])
    .factory("residentDetailRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + "residentdetails/:id", { id: "@id" });
    }])
    .factory("prePaymentRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //预付款
        return $resource("api/Receipts/:id", { id: "@id" });
        //return $resource(resourceBase + "prepayments/:id", { id: "@id" });
    }])
    .factory("welfareRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/welfare/:id", { id: "@id" });
        //return $resource(resourceBase + "welfares/:id", { id: "@id" });
    }])
    .factory("residentRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + "resident/:id", { id: "@id" });
    }])
    .factory("requirementRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/requirement/:id", { id: "@id" });
        //return $resource(resourceBase + "requirement/:id", { id: "@id" });
    }])
    //.factory("chargeCheckRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
    //    return $resource(resourceBase + "charge-check/:id", { id: "@id" });
    //}])
    .factory("companyRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + "companys/:id", { id: "@id" });
    }])
    .factory("floorRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/floors/:id", { id: "@id" });
    }])
    .factory("roomRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/rooms/:id", { id: "@id" }, {
            saveRoom: {     //自定义的方法  
                method: 'POST',
                url: 'api/rooms/saveRoom',
            }
        });
    }])
    //.factory("chargeCheckRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
    //    return $resource(resourceBase + "floors/:id", { id: "@id" });
    //}])
    .factory("bedRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/beds/:id", { id: "@id" }, {
            changeBed: {     //自定义的方法  
                method: 'POST',
                url: 'api/beds/changeBed',
            }
        });
    }])
    .factory("chargeDetailRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {  //费用录入
        return $resource("api/Costdtl/:id", { id: "@id" });
        //return $resource(resourceBase + "chargedetails/:id", { id: "@id" });
    }])
    .factory('injectionRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/Injection/:id", { id: "@id" });
    }])
    .factory('carePlanRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/carePlanRes/:id", { id: "@id" });
    }])
    .factory('nurDemandEvalRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/nurDemandEval/:id", { id: "@id" });
    }])
    .factory('carePlanGoalRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/carePlanGoalRes/:id", { id: "@id" });
    }])
    .factory('evaluationRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/evaluation/:id", { id: "@id" });
    }])
    .factory('evaluationHisRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/evaluationHis/:id", { id: "@id" });
    }])
    .factory('carePlanActivityRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/carePlanActivityRes/:id", { id: "@id" }, {//bobdu扩展了批量存储方法
            GetById: {     //自定义的方法  
                method: 'GET',
                url: 'api/carePlanActivityRes/GetById',
            }
        });
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
    .factory('carePlanListRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/carePlanList/:id", { id: "@id" });
    }])
    .factory('carePlanDetailRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/carePlanDetail/:id", { id: "@id" });
    }])
    .factory('carePlanAssessRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/carePlanAssess/:id", { id: "@id" });
    }])
    .factory('evalsheetRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/nursing/:id", { id: "@id" });
    }])
    .factory('nurDemandHisRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/nurDemandHis/:id", { id: "@id" });
    }])
      .factory('bedStatusRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
          return $resource("api/bedStatusRes/:id", { id: "@id" });
      }])
    .factory('rehabilitationRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + "rehabilitation/:id", { id: "@id" });
    }])
    .factory('referralRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + "referral/:id", { id: "@id" });
    }])

   .factory('nurseDailyReportList', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧 护士日报表
       return $resource("api/NurseDailyReportList/:id", { id: "@id" });
   }])
 .factory('nurseDailyReportpipe', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧 护士日报表
     return $resource("api/NurseDailyReportPipe/:id", { id: "@id" });
 }])

    .factory('rehabilitationRes', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧
        return $resource("api/referral/:id", { id: "@id" });
    }])
    .factory('ReferralLists', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧
        return $resource("api/referralListZ/:id", { id: "@id" })
    }])

    .factory('Commfilelists', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧 常用语管理
        return $resource("api/Commfilelist/:id", { id: "@id" }, {//bobdu扩展了批量存储方法
            mulSave: {     //自定义的方法  
                method: 'POST',
                url: 'api/Commfilelist/mulDelete',
            }
        })
    }])


    //姚丙慧 生化检查
    .factory('biochemistryRes', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧
        return $resource("api/Biochemistry/:id", { id: "@id" });
    }])
    .factory('biochemistry', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧
        return $resource("api/Biochemistrys/:id", { id: "@id" });
    }])
    .factory('biochemistrylist', ['$resource', 'resourceBase', function ($resource, resourceBase) { // 姚丙慧
        return $resource("api/Biochemistrylist/:id", { id: "@id" });
    }])
    .factory("billRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/Bill/:id", { id: "@id" });
        //return $resource(resourceBase + "bills/:id", { id: "@id" });
    }])
    .factory("chargeCheckRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {    //入住审核 
        return $resource("api/Verify/:id", { id: "@id" });
        //return $resource(resourceBase + "charge-check/:id", { id: "@id" });
    }])
    .factory("caseClosedRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {  //结案管理
        return $resource("api/caseClosed/:id", { id: "@id" });
        //return $resource("api/ipdregout/:id", { id: "@id" });   
    }])
    //.factory("requirementRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
    //    return $resource(resourceBase + "requirement/:id", { id: "@id" });
    //}])
    .factory("empFileRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {   //员工管理
        return $resource("api/empfiles/:id", { id: "@id" });
        //return $resource(resourceBase + "empfiles/:id", { id: "@id" });
    }])
     .factory("empFileExtRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {   //员工管理
         return $resource("api/empfileExt/:id", { id: "@id" });
         //return $resource(resourceBase + "empfiles/:id", { id: "@id" });
     }])

      .factory("empFileResGet", ['$resource', 'resourceBase', function ($resource, resourceBase) {   //员工管理
          return $resource("api/empfilesgetprosn/:id", { id: "@id" });
          //return $resource(resourceBase + "empfiles/:id", { id: "@id" });
      }])

     //字典管理list zhongyh
  .factory("DCDataDicListRes", ['$resource', function ($resource) {
      return $resource('api/DCDataDicList/:id', { id: '@id' });
  }])

    .factory("roleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //角色管理
        return $resource("api/roles/:id", { id: "@id" });
        //return $resource(resourceBase + "roles/:id", { id: "@id" });
    }])
    .factory("moduleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //模块管理
        return $resource("api/module/:id", { id: "@id" });
        //return $resource(resourceBase + "roles/:id", { id: "@id" });
    }])
    //.factory("BillRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
    //    return $resource(resourceBase + "bills/:id", { id: "@id" });
    //}])
    .factory("BillPayRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {    //帐单交费明细
        return $resource("api/PayBill/:id", { id: "@id" });
        //return $resource(resourceBase + "bill-pay/:id", { id: "@id" });
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
    .factory("manufactureRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/manufactures/:id", { id: "@id" });
        //return $resource(resourceBase + "manufactures/:id", { id: "@id" });
    }])
    .factory("goodsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/goods/:id", { id: "@id" });
        //return $resource(resourceBase + "goods/:id", { id: "@id" });
    }])
    .factory("chargeItemRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //收费项目
        //return $resource(resourceBase + "chargeitems/:id", { id: "@id" });
        return $resource("api/CostItem/:id", { id: "@id" });
    }])
    .factory("chargeGroupRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //收费组合
        //return $resource(resourceBase + "chargegroups/:id", { id: "@id" });
        return $resource("api/CostGroup/:id", { id: "@id" });
    }])
    .factory("chargeGroupDelRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //收费组合
        //return $resource(resourceBase + "chargegroups/:id", { id: "@id" });
        return $resource("api/CostGroupDtl/:id", { id: "@id" });
    }])
    .factory("fixedChargeRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //固定费用设定
        //return $resource(resourceBase + "fixedcharges/:id", { id: "@id" });
        return $resource("api/FixedCost/:id", { id: "@id" });
    }])
    .factory("fixedChargeChargeGroupRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //固定费用设定
        //return $resource(resourceBase + "fixedcharges/:id", { id: "@id" });
        return $resource("api/FixedCost/ChargeGroup");
    }])
    .factory("familyDiscussrecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //家庭支持//亲友访视add by zk.
        return $resource("api/familydiscussrec/:id", { id: "@id" });
        //return $resource(resourceBase + 'familydiscussrec/:id', { id: '@id' });
    }])
     .factory("NutrtionEvalRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //营养筛查  zhongyh
         return $resource("api/NutrtionEval/:id", { id: "@id" });
         //return $resource(resourceBase + 'familydiscussrec/:id', { id: '@id' });
     }])
    .factory("proposaldisscussrecsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //个案研讨会议记录
        return $resource(resourceBase + 'proposaldisscussrecs/:id', { id: '@id' });
    }])
    .factory("proposaldisscussRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //跨专业讨论
        return $resource(resourceBase + 'proposaldisscuss/:id', { id: '@id' });
    }])
    .factory("complainrecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //申诉/权益维护
        //return $resource(resourceBase + 'complainrecs/:id', { id: '@id' });
        return $resource('api/complainrecs/:id', { id: '@id' });
    }])
    .factory("resourcelinkRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //资源连接
        return $resource('api/resourcelink/:id', { id: '@id' });
        //return $resource(resourceBase + "resourcelink/:id", { id: "@id" });
    }])
    .factory("subsidyRecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //申请补助
        return $resource("api/subsidyrec/:id", { id: "@id" });
        //return $resource(resourceBase + "subsidyrecs/:id", { id: "@id" });
    }])
    .factory('liferecordsRes', ['$resource', 'resourceBase', function ($resource, resourceBase) { //生活记录
        return $resource("api/liferecords/:id", { id: "@id" });
        //return $resource(resourceBase + "liferecords/:id", { id: "@id" });
    }])
    .factory('regevaluateRes', ['$resource', 'resourceBase', function ($resource, resourceBase) { //社工评估
        return $resource("api/regevaluate/:id", { id: "@id" });
    }])
    .factory('NewResideEntenvRecRes', ['$resource', 'resourceBase', function ($resource, resourceBase) { //新进住民环境介绍记录表zhongyh
        return $resource("api/NewResideEntenvRec/:id", { id: "@id" });
    }])
   .factory('NewRegEnvAdaptationRes', ['$resource', 'resourceBase', function ($resource, resourceBase) { //新进住民环境介绍适应入辅导记录表zhongyh
       return $resource("api/NewRegEnvAdaptation/:id", { id: "@id" });
   }])
    .factory("carersvrRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //社工服务记录
        return $resource('api/carersvrs/:id', { id: '@id' });
    }])
    .factory("homeCareSvrrecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource(resourceBase + 'homecaresvrrec/:id', { id: '@id' });
    }])
    .factory("homeCareSuperviseRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {//居家服务督导记录
        return $resource('api/homecaresupervise/:id', { id: '@id' });
    }])
    .factory("residentV2Res", ['$resource', 'resourceBase', function ($resource, resourceBase) {//住民清册
        return $resource('api/residentV2/:id', { id: '@id' });
    }])
    .factory("referralRecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {//转介
        return $resource('api/referralrecs/:id', { id: '@id' });
    }])
    .factory("medicineRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //药品管理
        return $resource('medicines/:id', { id: '@id' });
    }])
    .factory("visitdocrecordsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //就医管理
        return $resource('visitdocrecords/:id', { id: '@id' });
    }])
    .factory("ownDrugRecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //自带药品
        return $resource("api/ownDrugRec/:id", { id: "@id" }, {
            SaveOwnDrugDtl: {     //自定义的方法  
                method: 'POST',
                url: 'api/ownDrugRec/saveOwnDrugDtl',
            }
        });
    }])
    .factory("visitprescriptionsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //处方管理
        return $resource('visitprescriptions/:id', { id: '@id' });
    }])
    .factory("insulininjectsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //胰岛素注射记录
        return $resource(resourceBase + 'insulininjects/:id', { id: '@id' });
    }])
    .factory('pipelinerecRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {//管路
        //return $resource(resourceBase + "pipelinerec/:id", { id: "@id" });
        return $resource("api/pipelinerec/:id", { id: "@id" });
    }])
    .factory('pipelineevalRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {//管路
        return $resource("api/pipelineeval/:id", { id: "@id" });
    }])
    .factory("vitalSignRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //生命体征
        //return $resource(resourceBase + 'vital-sign/:id', { id: '@id' });
        return $resource('api/Vitalsign/:id', { id: '@id' });
    }])
    .factory("nursingRecordRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {  //护理记录
        //return $resource(resourceBase + 'nursingrecords/:id', { id: '@id' });
        return $resource('api/NursingRecord/:id', { id: '@id' }, {
            saveBatch: {
                method: 'POST',
                url: 'api/NursingRecord/batchSave'
            }
        });
    }])
    .factory("nursingHandoverRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {  //单笔交班记录
        //return $resource(resourceBase + 'nursinghandovers/:id', { id: '@id' });
        return $resource('api/NursingSingleShift/:id', { id: '@id' }, {//bobdu扩展了批量存储方法
            mulSave: {     //自定义的方法  
                method: 'POST',
                url: 'api/NursingSingleShift/multipSave',
            }
        });
    }])
    .factory("hspRecordRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {  //护理记录
        return $resource(resourceBase + 'hsprecords/:id', { id: '@id' });
    }])
    .factory("nursingPlanRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource(resourceBase + 'nursingplans/:id', { id: '@id' });
    }])
    .factory("loanrecordsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //进贷
        return $resource("api/goodsLoan/:id", { id: "@id" });
        //return $resource(resourceBase + 'loanrecords/:id', { id: '@id' });
    }])
    .factory("salerecordsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //领贷
        return $resource("api/goodsSale/:id", { id: "@id" });
        //return $resource(resourceBase + 'salerecords/:id', { id: '@id' });
    }])
    .factory("inValueRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //输入量
        //return $resource(resourceBase + 'in-value/:id', { id: '@id' });
        return $resource('api/InValue/:id', { id: '@id' });
    }])
    .factory("outValueRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //输出量
        //return $resource(resourceBase + 'out-value/:id', { id: '@id' });
        return $resource('api/OutValue/:id', { id: '@id' });
    }])
    .factory("leaveHospRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //请假
        return $resource('api/leaveHosp/:id', { id: '@id' });
        //return $resource(resourceBase + 'leave-hosp/:id', { id: '@id' });
    }])
    .factory("pinMoneyRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //零用金
        return $resource('api/PinMoney/:id', { id: '@id' });
        //return $resource(resourceBase + 'pin-money/:id', { id: '@id' });
    }])
    //.factory("regVisitRecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //亲友访视 作废,改用familyDiscussrecRes
    //    return $resource(resourceBase + 'reg-visit-rec/:id', { id: '@id' });
    //}])
    .factory("reginsdtlRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //入住登记福利 ipdreg
        return $resource(resourceBase + 'reginsdtl/:id', { id: '@id' });
    }])
    .factory("ipdregRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //入住登记福利 ipdreg 
        return $resource(resourceBase + 'ipdreg/:id', { id: '@id' });
    }])
    .factory("fallRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 跌倒
        return $resource('api/fallincidentevent/:id', { id: '@id' });
    }])
    .factory("pharmacistevalsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 多重用药
        return $resource('api/pharmacistevals/:id', { id: '@id' });
    }])
    .factory("prsSoreRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 压疮
        return $resource('api/prssores/:id', { id: '@id' });
        //return $resource(resourceBase + 'prssores/:id', { id: '@id' });
    }])
    .factory("sorechgRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 压疮
        return $resource('api/sorechg/:id', { id: '@id' });
        //return $resource(resourceBase + 'prssores/:id', { id: '@id' });
    }])
    .factory("restrictRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 约束
        return $resource('api/restricts/:id/:feeno', { id: '@id', feeno: '@feeno' });
        //return $resource(resourceBase + 'restricts/:id', { id: '@id' });
    }])
    .factory("restrictDetailRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 约束
        return $resource('api/restrictdetails/:id', { id: '@id' });
        //return $resource(resourceBase + 'restricts/:id', { id: '@id' });
    }])
    .factory("painRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 疼痛
        return $resource('api/pains/:id/:feeno', { id: '@id', feeno: '@feeno' });
        //return $resource(resourceBase + 'pains/:id', { id: '@id' });
    }])
    .factory("painDetailRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 疼痛
        return $resource('api/paindetails/:id', { id: '@id' });
        //return $resource(resourceBase + 'pains/:id', { id: '@id' });
    }])
    .factory("adminHandoversRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 交付照会
        return $resource('api/AssignTask/:id', { id: '@id' }, {//bobdu扩展了方法
            assSave: {     //自定义的方法  
                method: 'POST',
                url: 'api/AssignTask/assSave',
            }
        });
        //return $resource(resourceBase + 'affairshandover/:id', { id: '@id' });
    }])
     .factory("adminHandoversMultiRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 多批次交付照会提交
         return $resource('api/AssignTask/multisave', { id: '@id' });
         //return $resource(resourceBase + 'affairshandover/:id', { id: '@id' });
     }])
    .factory("affairsHandoverRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //行政交班 
        return $resource('api/affairshandover/:id', { id: '@id' }, {//bobdu扩展了批量存储方法
            mulSave: {     //自定义的方法  
                method: 'POST',
                url: 'api/affairshandover/multipSave',
            }
        });
    }])
    .factory("groupActivityRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //团体活动 groupActivityRes
        //return $resource(resourceBase + 'group-activity/:id', { id: '@id' });
        return $resource('api/GroupActivityRec/:id', { id: '@id' });
    }])
    .factory("groupActivityEvalRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //团体活动 groupActivityRes
        return $resource('api/GroupActivityEval/:id', { id: '@id' });
    }])
    .factory("memberAssessRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //成员个别化活动评估 member-assess
        return $resource(resourceBase + 'member-assess/:id', { id: '@id' });
    }])
    .factory("infectionSympotmRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //感染症状  
        //return $resource(resourceBase + 'infection-sympotm/:id', { id: '@id' });
        return $resource('api/InfectionSympotm/:id', { id: '@id' });
    }])
    .factory("infectionRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //感染
        //return $resource(resourceBase + 'infection/:id', { id: '@id' });
        return $resource('api/Infection/:id', { id: '@id' });
    }])
    .factory("unPlanWeightRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //非计划减重改变
        //return $resource(resourceBase + 'unplanweightind/:id', { id: '@id' });
        return $resource('api/UnPlanWeightInd/:id', { id: '@id' }, {//bobdu扩展了批量存储方法
            MultiSave: {     //自定义的方法  
                method: 'POST',
                url: 'api/UnPlanWeightInd/MultiSave',
            },
            MultiQuery: {     //自定义的方法  
                method: 'GET',
                isArray: false,
                url: 'api/UnPlanWeightInd/MultiQuery',
            }
        });
    }])
    .factory("unPlanIpdRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //非计划住院
        //return $resource(resourceBase + 'unplanedipd/:id', { id: '@id' });
        return $resource('api/UnPlanEdipd/:id', { id: '@id' });
    }])
    .factory("labExamRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //检体送检情形
        //return $resource(resourceBase + 'lab-exam-rec/:id', { id: '@id' });
        return $resource('api/LabExamRec/:id', { id: '@id' });
    }]).factory('commonUserWordRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/CommonUseWord/:id", { id: "@id" });
    }])
    .factory("PhysicianAssRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //医师评估
        return $resource('api/doctorEvalRec/:id', { id: '@id' });
    }])
    .factory("PhysicianRoundsRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //医师巡诊
        return $resource('api/doctorCheckRec/:id', { id: '@id' });
    }])
    .factory("MedicationRecordRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //用药记录
        return $resource('api/medicationRecord/:id', { id: '@id' });
    }])
    .factory("groupRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //集团管理
        return $resource('api/group/:id', { id: '@id' });
    }])
    .factory("deptRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //部门管理
        return $resource('api/Depts/:id', { id: '@id' });
    }])
    .factory("PreipdRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //预约登记
        return $resource('api/Preipd/:id', { id: '@id' });
    }])
    .factory("IpdregoutRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //结案作业
        return $resource('api/Ipdregout/:id', { id: '@id' }, {
            CheckBillInfo: {     //自定义的方法  
                method: 'GET',
                url: 'api/Ipdregout/CheckBillInfo'
            }
        });
    }])
    .factory("roleModuleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //出院办理
        return $resource('api/module/:id', { id: '@id' });
    }])
    .factory("LeaveNursingRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //退住院
        return $resource('api/LeaveNursing/:id', { id: '@id' });
    }])
    .factory("infectionItemRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //感染项目
        return $resource('api/InfectionItem/:id', { id: '@id' });
    }])
    .factory("symptomItemRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //感染症状
        return $resource('api/SymptomItem/:id', { id: '@id' });
    }])
    .factory("visitHospitalRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //就诊医院
        return $resource('api/visitHospital/:id', { id: '@id' });
    }])
    .factory("visitDeptRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //就诊科室
        return $resource('api/visitDept/:id', { id: '@id' });
    }])
    .factory("visitDoctorRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //就诊医师
        return $resource('api/visitDoctor/:id', { id: '@id' });
    }])
    .factory("icd9Res", ['$resource', 'resourceBase', function ($resource, resourceBase) { //国际分类
        return $resource('api/icd9/:id', { id: '@id' });
    }])
    .factory("freqRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource('api/Freq/:id', { id: '@id' });
    }])
   .factory("RegActivityRequEvalRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //个别化活动需求
       return $resource('api/RegActivityRequEval/:id', { id: '@id' });
   }])
   .factory("reportManageRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //报表管理
       return $resource('api/ReportManage/:id', { id: '@id' });
   }])
   .factory("reportManageSetRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //报表设置
       return $resource('api/ReportManage/Set/:id', { id: '@id' });
   }])
   .factory("assignTaskRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //消息提醒
       return $resource('api/assignTaskNew/:id', { id: '@id' });
   }])
     .factory("reAssignTaskRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 重新分配工作日照
         return $resource('api/assignTaskNew/multisave', { id: '@id' });
     }])
     .factory("noticeRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { // 院内公告
         return $resource('api/Notice', { id: '@id' });
     }])
     .factory("NutritionCareRecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/NutritionCareRec', { id: '@id' });
     }])
     .factory("postCodeRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/post', { id: '@id' });
     }])

     .factory("myDeskRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //新桌面  孙伟 新增
         return $resource('api/myDesk/', { id: '@id' });
     }])

     .factory("NutritionRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //营养评估单
         return $resource('api/nutrition', { id: '@id' });

     }])
     .factory("evalTemplateRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/evalTemplate', { id: '@id' });

     }])
     .factory("exportEvalRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/exportEval', { id: '@id' });

     }])
     .factory("evalQuestionResultRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/evalQuestionResult', { id: '@id' });

     }])
     .factory("evalAnswerRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/evalAnswer', { id: '@id' });

     }])
     .factory("evalQuestionRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/evalQuestion', { id: '@id' });

     }])
     .factory("exportQueResRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/exportQueRes', { id: '@id' });

     }])
     .factory("evalTempSetRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/evalTempSet', { id: '@id' });

     }])
     .factory("categoryRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/category', { id: '@id' });
     }])

    //费用管理
     .factory("billV2Res", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/billv2', { id: '@id' });
     }])

     .factory("docOrderRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //费用管理-医嘱开立 Bob wu 201704061819
         return $resource('api/docOrder', { id: '@id' }, {
             saveOrders: {     //自定义的方法  
                 method: 'POST',
                 url: 'api/docOrder/saveOrders'
             },
             deleteSentOrders: {     //自定义的方法  
                 method: 'POST',
                 url: 'api/docOrder/deleteSentOrders'
             },
             saveSendOrders: {     //自定义的方法  
                 method: 'POST',
                 url: 'api/docOrder/saveSendOrders'
             }
         });
     }])
     .factory("billPaymentRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费-账单缴费 Bob wu 201701091355
         return $resource('api/billPayment/:id', { id: '@id' });
     }])

     .factory("rsChargeGroupRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费-账单缴费-住民绑定套餐详细信息 Bob wu 201702241234
         return $resource('api/rsChargeGroup/:id', { id: '@id' });
     }])

     .factory("billPaymentRecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-历史缴费账单 Bob wu 201701121055
         return $resource('api/billPaymentRec/:id', { id: '@id' });
     }])
     .factory("advanceChargeRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-预收款缴费 Bob wu 201701101915
         return $resource('api/advanceCharge/:id', { id: '@id' });
     }])
     .factory("residentBalanceRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-住民账户 Bob wu 201701161208
         return $resource('api/residentBalanceRes/:id', { id: '@id' });
     }])
     .factory("billV2PayRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-缴费记录 Bob wu 201701161208
         return $resource('api/billV2PayRes/:id', { id: '@id' });
     }])
     .factory("billRefundRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费-账单退费 Bob wu 201701191606
         return $resource('api/billRefund/:id', { id: '@id' });
     }])
     .factory("billRefunRecRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-缴费退费 Bob wu 201701091355
         return $resource('api/billRefundRec/:id', { id: '@id' });
     }])
     .factory("advanceChargeRefundRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-费用退费 Bob wu 201701190954
         return $resource('api/advanceChargeRefund/:id', { id: '@id' });
     }])
     .factory("residentBalanceRefundRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-住民账户退费 Bob wu 201701191128
         return $resource('api/residentBalanceRefund/:id', { id: '@id' });
     }])
     .factory("RefundRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //缴费管理-退费记录 Bob wu 201701191355
         return $resource('api/RefundRes/:id', { id: '@id' });
     }])
     .factory("feeRecordRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/feeRecord', { id: '@id' });
     }])

.factory("nsDrugRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
    return $resource('api/nsdrug', { id: '@id' });
}])
 .factory("nsServiceRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
     return $resource('api/nsservice', { id: '@id' });
 }])
 .factory("nsMedicalMaterialRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
     return $resource('api/nsmaterial', { id: '@id' });
 }])
    .factory("pacMaintainRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource('api/PacMaintain/:id', { id: '@id' }, {
            DeleteChargeItem: {     //自定义的方法  
                method: 'Delete',
                url: 'api/PacMaintain/DeleteChargeItem/:id',
            }
        });
    }])
    .factory("resChargeGroRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource('api/ResChargeGro/:id', { id: '@id' });
    }])
     .factory("costEntryRes", ['$resource', 'resourceBase', function ($resource, resourceBase) {
         return $resource('api/CostEntry', { id: '@id' }, {
             updateRecord: {     //自定义的方法  
                 method: 'POST',
                 url: 'api/CostEntry/updateRecord'
             }
         });
     }])
    //收费项目管理
     .factory("chargeItemNSDrugRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/nsdrugmgr/:id', { id: '@id' }, {
             getByMCDrugCode: {     //自定义的方法  
                 method: 'GET',
                 url: 'api/nsdrugmgr/getbymcdrugcode'
             }
         });
     }])
     .factory("nciDrugRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/ncidrug/:drugcode', { drugcode: '@drugcode' });
     }])
     .factory("chargeItemNSMaterialRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/nsmaterialmgr/:id', { id: '@id' }, {
             getByMCMaterialCode: {     //自定义的方法  
                 method: 'GET',
                 url: 'api/nsmaterialmgr/getbymcmaterialcode'
             }
         });
     }])
     .factory("nciMaterialRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/ncimaterial/:materialcode', { materialcode: '@materialcode' });
     }])
     .factory("chargeItemNSServiceRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/nsservicemgr/:id', { id: '@id' }, {
             getByMCServiceCode: {     //自定义的方法  
                 method: 'GET',
                 url: 'api/nsservicemgr/getbymcservicecode'
             }
         });
     }])
     .factory("nciServiceRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/nciservice/:servicecode', { servicecode: '@servicecode' });
     }])
     .factory("chargeGroupEntryRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/chargeGroupRec/:id', { servicecode: '@id' });
     }])
     .factory("chargeGroupDrugRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/chargeGroupDrug/:id', { servicecode: '@id' });
     }])
     .factory("chargeGroupMaterialRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/chargeGroupMaterial/:id', { servicecode: '@id' });
     }])
     .factory("chargeGroupServiceRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/chargeGroupService/:id', { servicecode: '@id' });
     }])

     .factory("payGrantServiceRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
         return $resource('api/appropriation/:id', { servicecode: '@id' });
     }])

     .factory("rsFeeMonDtlRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //住民月度申报费用明细
         return $resource('api/rsFeeMonDtl/:id/:YearMon', { servicecode: '@id' });
     }])

    .factory("NCIPServiceDepositGrantListRes", ['$resource', function ($resource) {
        return $resource('api/ServiceDepositGrantList/:id', { id: '@id' });
    }])
    // 健康档案
   .factory("HealthRecordsRes", ['$resource', function ($resource) {
       return $resource('api/HealthRecords/:id', { id: '@id' });
   }])
   // 首次入住记录
   .factory("FstRegRecRes", ['$resource', function ($resource) {
       return $resource('api/FstRegRec/:id', { id: '@id' });
   }])
  .factory("advisoryRegRes", ['$resource', function ($resource) {
      return $resource('api/advisoryReg/:id', { id: '@id' });
  }])
  .factory("advisoryRegCallBackRes", ['$resource', function ($resource) {
      return $resource('api/advisoryRegCallBack/:id', { id: '@id' });
  }])
    //交班记录
    .factory("workItemRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource('api/workItem/:itemType', { itemType: '@itemType' });
    }])
    .factory("handoverRecordRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource('api/HandoverRecord/:date', { date: '@date' },
             {
                 SaveHandoverRecord: {     //自定义的方法  
                     method: 'POST',
                     url: 'api/HandoverRecord/saveHandoverDtl'
                 }
             });
    }])

    //交班记录
    .factory("lookOverRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource('api/lookOver/:id', { id: '@id' });
    }])
    //长期护理保险评价
    .factory("NursingCareEvalRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //
        return $resource('api/nursingCareEval/:id', { id: '@id' });
    }])
    .factory("visitorInOutRecRes", ['$resource', function ($resource) {
        return $resource('api/visitorInOutRec/:id', { id: '@id' });
    }])

    .factory("resBirthDayListRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //老人生日查询
        return $resource('api/resBirthDayList/:id', { id: '@id' });
    }])
     .factory("DeductionRes", ['$resource', function ($resource) {
         return $resource('api/DeductionInfo/:id', { id: '@id' });
     }])
    .factory('regNCIInfoRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/regNCIInfo/:id", { id: "@id" },
            {
                welfareSave: {     //自定义的方法  
                    method: 'POST',
                    url: 'api/regNCIInfo/welfareInfo',
                }
            });
    }])
    .factory("ReportTempManageRes", ['$resource', function ($resource) {
        return $resource('api/ReportTempManage/:id', { id: '@id' });
    }])
    //  关账作业
    .factory("FinancialCloseRes", ['$resource', function ($resource) {
        return $resource('api/FinancialClose/:id', { id: '@id' });
    }])
;
