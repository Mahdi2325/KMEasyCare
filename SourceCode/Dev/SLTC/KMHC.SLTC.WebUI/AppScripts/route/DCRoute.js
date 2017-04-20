/// <reference path="../../Scripts/timeline/index.html" />
/// <reference path="../../Scripts/timeline/index.html" />
angular.module('sltcApp')
.config([
        '$stateProvider', '$urlRouterProvider', '$locationProvider', 'cfpLoadingBarProvider', function ($stateProvider, $urlRouterProvider, $locationProvider, cfpLoadingBarProvider) {

            cfpLoadingBarProvider.spinnerTemplate = '<div class="loading"><span class="fa fa-spinner">加载中...</span></div>';
            cfpLoadingBarProvider.latencyThreshold = 800;


            $urlRouterProvider.when("/", "/dc/DCIndex").otherwise('/');
            //报表
            $stateProvider.state('ReportIndex', { url: '/dc/ReportIndex', templateUrl: '/Views/Report/ReportIndex.html', controller: "reportIndexCtrl" });
            $stateProvider.state('ActivityAssessment', { url: '/dc/Report/ActivityAssessment', templateUrl: '/Views/NursingForm/ActivityAssessment.html' });

            //集团管理
            $stateProvider.state('GroupList', { url: '/dc/GroupList', templateUrl: '/Views/OrganizationManage/GroupList.html', controller: 'groupListCtrl' });
            $stateProvider.state('GroupAdd', { url: '/dc/GroupEdit', templateUrl: '/Views/OrganizationManage/GroupEdit.html', controller: 'groupEditCtrl' });
            $stateProvider.state('GroupEdit', { url: '/dc/GroupEdit/:id', templateUrl: '/Views/OrganizationManage/GroupEdit.html', controller: 'groupEditCtrl' });
            //机构管理
            $stateProvider.state('OrgList', { url: '/dc/OrgList', templateUrl: '/Views/OrganizationManage/OrgList.html', controller: 'orgListCtrl' });
            $stateProvider.state('OrgAdd', { url: '/dc/OrgDtl', templateUrl: '/Views/OrganizationManage/OrgDtl.html', controller: 'orgDtlCtrl' });
            $stateProvider.state('OrgEdit', { url: '/dc/OrgDtl/:id', templateUrl: '/Views/OrganizationManage/OrgDtl.html', controller: 'orgDtlCtrl' });
            //$stateProvider.state('CodeList', { url: '/angular/CodeList', templateUrl: '/Views/OrganizationManage/CodeManage.html', controller: 'codeListCtrl' });
            //员工管理
            $stateProvider.state('StaffList', { url: '/dc/StaffList', templateUrl: '/Views/OrganizationManage/StaffList.html', controller: 'staffListCtrl' });
            $stateProvider.state('StaffAdd', { url: '/dc/StaffEdit', templateUrl: '/Views/OrganizationManage/StaffEdit.html', controller: 'staffEditCtrl' });
            $stateProvider.state('StaffEdit', { url: '/dc/StaffEdit/:id', templateUrl: '/Views/OrganizationManage/StaffEdit.html', controller: 'staffEditCtrl' });
            $stateProvider.state('MyAccount', { url: '/dc/MyAccount/:id', templateUrl: '/Views/Home/MyAccount.html', controller: 'myAccountCtrl' });
            //用户管理
            $stateProvider.state('UserList', { url: '/dc/UserList', templateUrl: '/Views/OrganizationManage/UserList.html', controller: 'userListCtrl' });
            $stateProvider.state('UserAdd', { url: '/dc/UserEdit', templateUrl: '/Views/OrganizationManage/UserEdit.html', controller: 'userEditCtrl' });
            $stateProvider.state('UserEdit', { url: '/dc/UserEdit/:id', templateUrl: '/Views/OrganizationManage/UserEdit.html', controller: 'userEditCtrl' });
            //角色管理
            $stateProvider.state('RoleList', { url: '/dc/RoleList', templateUrl: '/Views/OrganizationManage/RoleList.html', controller: 'roleListCtrl' });
            $stateProvider.state('RoleAdd', { url: '/dc/RoleEdit', templateUrl: '/Views/OrganizationManage/RoleEdit.html', controller: 'roleEditCtrl' });
            $stateProvider.state('RoleEdit', { url: '/dc/RoleEdit/:id', templateUrl: '/Views/OrganizationManage/RoleEdit.html', controller: 'roleEditCtrl' });
            //模块管理
            $stateProvider.state('ModuleEdit', { url: '/dc/OrganizationManage/ModuleEdit', templateUrl: '/Views/OrganizationManage/ModuleEdit.html' });
            //部门管理
            $stateProvider.state('DeptList', { url: '/dc/deptList', templateUrl: '/Views/OrganizationManage/DeptList.html', controller: 'deptListCtrl' });
            $stateProvider.state('DeptAdd', { url: '/dc/deptEdit', templateUrl: '/Views/OrganizationManage/DeptEdit.html', controller: 'deptEditCtrl' });
            $stateProvider.state('DeptEdit', { url: '/dc/deptEdit/:id', templateUrl: '/Views/OrganizationManage/DeptEdit.html', controller: 'deptEditCtrl' });
            //-------系统管理zhongyh--------------
            $stateProvider.state('DCActivity', { url: '/dc/DCActivity/', templateUrl: '/Views/DC/SysAdmin/DCActivity.html', controller: 'DCActivityCtrl' });//活动类型维护
            $stateProvider.state('DCDataDicList', { url: '/dc/DCDataDicList/', templateUrl: '/Views/DC/SysAdmin/DCDataDicList.html', controller: 'DCDataDicListCtrl' });//数据字典列表
            $stateProvider.state('DCDataDicAdd', { url: '/dc/DCDataDicEdit/', templateUrl: '/Views/DC/SysAdmin/DCDataDicEdit.html', controller: 'DCDataDicEditCtrl' });//数据字典增加
            $stateProvider.state('DCDataDicEdit', { url: '/dc/DCDataDicEdit/:id', templateUrl: '/Views/DC/SysAdmin/DCDataDicEdit.html', controller: 'DCDataDicEditCtrl' });//编辑
            //-----------------------------------
            $stateProvider.state('DCregMedcine', { url: '/dc/DCregMedcine/:FeeNo', templateUrl: '/Views/DC/NurseCare/DCregMedcine.html', controller: 'DCregMedcineCtrl' });//个案药品管理表单
            $stateProvider.state('DCNurseRequirementEval', { url: '/dc/DCNurseRequirementEval/:FeeNo', templateUrl: '/Views/DC/NurseCare/DCNurseRequirementEval.html', controller: 'DCNurseRequirementEvalCtrl' });//护理需求评估及照顾计画表
            $stateProvider.state('DCAdjuvantTherapy', { url: '/dc/DCAdjuvantTherapy/:FeeNo', templateUrl: '/Views/DC/NurseCare/DCAdjuvantTherapy.html', controller: 'DCAdjuvantTherapyCtrl' });//辅疗性活动-个别化活动需求评估及计画表
            $stateProvider.state('DCRegNursingDiag', { url: '/dc/DCRegNursingDiag/:FeeNo', templateUrl: '/Views/DC/NurseCare/DCRegNursingDiag.html', controller: 'DCRegCPLCtrl' });
            $stateProvider.state('DCBasicDataArrangement', { url: '/dc/DCBasicDataArrangement/:FeeNo', templateUrl: '/Views/DC/NurseCare/DCBasicDataArrangement.html', controller: 'DCBasicDataArrangementCtrl' });//个案基本资料汇整表
            $stateProvider.state('DCNursePlan', { url: '/dc/DCNursePlan/:FeeNo', templateUrl: '/Views/DC/NurseCare/DCNursePlan.html', controller: 'DCNursePlanCtrl' });
            //----------------------------------------------------------------------------------日照----------------------------------------------------------------------------------------
            $stateProvider.state('DCIndex', { url: '/dc/DCIndex', templateUrl: '/Views/DC/Resident/ResidentNavigate.html', controller: 'residentNavigateCtrl' });//residentNavigateCtrl
            //$stateProvider.state('AssignTaskModal', { url: '/dc/AssignTaskModal', templateUrl: '/Views/DC/Resident/AssignTaskEmpModal.html', controller: 'assignTaskEmpModalCtr' });
            $stateProvider.state('ResidentNavigate', { url: '/dc/ResidentNavigate/:FeeNo', templateUrl: '/Views/DC/Resident/ResidentNavigate.html', controller: 'residentNavigateCtrl' });
            $stateProvider.state('AssignWorkNote', { url: '/dc/AssignWorkNote', templateUrl: '/Views/DC/Resident/AssignWorkNote.html', controller: 'assignWorkNoteCtrl' });//
            $stateProvider.state('SocialNavigate', { url: '/dc/SocialNavigate/:FeeNo', templateUrl: '/Views/DC/SocialWorker/SocialNavigate.html', controller: 'socialNavigateCtrl' });//
            $stateProvider.state('NurseCareNavigate', { url: '/dc/NurseCareNavigate/:FeeNo', templateUrl: '/Views/DC/NurseCare/NurseCareNavigate.html', controller: 'nursingNavigateCtrl' });//
            $stateProvider.state('CrossSpecialNavigate', { url: '/dc/CrossSpecialNavigate/:FeeNo', templateUrl: '/Views/DC/CrossSpeciality/CrossSpecialNavigate.html', controller: 'crossNavigateCtrl' });
            $stateProvider.state('CarePlanTimeline', { url: '/dc/CarePlanTimeline', templateUrl: '/Views/DC/Resident/CarePlanTimeline.html', controller: 'residentNavigateCtrl' });//
            $stateProvider.state('CasesTimeline', { url: '/dc/DCTransdisciplinaryTimeline/:FeeNo', templateUrl: '/Views/DC/CasesWorkStation/TransdisciplinaryTimeline.html', controller: 'transdisciplinaryTimelineCtrl' });
            $stateProvider.state('demo1', { url: '/dc/demo1/:FeeNo', templateUrl: '/Scripts/timeline/Resident/Demo1.html', controller: 'demo1Ctrl' });

            //-------------------yao--------------------
            $stateProvider.state('DCTransdisciplinaryPlan', { url: '/dc/DCTransdisciplinaryPlan/:FeeNo', templateUrl: '/Views/DC/CrossSpeciality/DCTransdisciplinaryPlan.html', controller: 'dcTransdisciplinaryPlanCtrl' });//照顾计划表

            $stateProvider.state('DCNurseCareLifeService', { url: '/dc/NurseCareLifeService/:FeeNo', templateUrl: '/Views/DC/CrossSpeciality/DCNurseCareLifeService.html', controller: 'NurseCareLifeServiceCtrl' });//护理及生活照顾服务纪录表

            $stateProvider.state('DCDayLifeCare', { url: '/dc/DCDayLifeCare/:FeeNo', templateUrl: '/Views/DC/CrossSpeciality/DCDayLifeCare.html', controller: 'DCDayLifeCareCtrl' });//日常生活照顾记录表

            $stateProvider.state('DCProblemBehavior', { url: '/dc/DCProblemBehavior/:FeeNo', templateUrl: '/Views/DC/CrossSpeciality/DCProblemBehaviorAbEm.html', controller: 'DCProblemBehaviorCtrl' });//问题行为与异常情绪记录表

            

            // 家庭医生
            $stateProvider.state('RegCheckRecordList', { url: '/dc/FamilyDoctor/RegCheckRecordList', templateUrl: '/Views/DC/FamilyDoctor/RegCheckRecordList.html' });
            $stateProvider.state('RegCheckRecordDtl', { url: '/dc/FamilyDoctor/RegCheckRecordDtl/:RegNo', templateUrl: '/Views/DC/FamilyDoctor/RegCheckRecordDtl.html' });
            $stateProvider.state('RegNoteRecord', { url: '/dc/FamilyDoctor/RegNoteRecord/:RegNo', templateUrl: '/Views/DC/FamilyDoctor/RegNoteRecord.html' });
            $stateProvider.state('RegVisitRecord', { url: '/dc/FamilyDoctor/RegVisitRecord/:RegNo', templateUrl: '/Views/DC/FamilyDoctor/RegVisitRecord.html' });


            //----------------------------------------------------------------------------------日照  杨金高---------------------------------------------------------------------------------------
            $stateProvider.state('IpdRegIn', { url: '/dc/IpdregIn/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_IpdRegIn.html', controller: 'ipdRegCtrl' });//个案收案
            $stateProvider.state('PersonBasicInfor', { url: '/dc/PersonBasicInfor/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_PersonBasicInfor.html', controller: 'personbasicCtrl' });//个案基本资料
            $stateProvider.state('PersonLifeHistory', { url: '/dc/PersonLifeHistory/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_PersonLifeHistory.html', controller: 'lifehistoryCtrl' });//个案生活史
            $stateProvider.state('OneDayLife', { url: '/dc/OneDayLife/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_OneDayLife.html', controller: 'ondaylifeCtrl' });//一天生活
            $stateProvider.state('SocialEval', { url: '/dc/SocialEval/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_SocialEval.html', controller: 'swregevalplanCtrl' });//社工个案评估及处遇计画表
            $stateProvider.state('PersonReferral', { url: '/dc/PersonReferral/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_PersonReferral.html', controller: 'personReferralCtrl' });//个案基本资料
            $stateProvider.state('IpdregOut', { url: '/dc/IpdregOut/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_IpdRegOut.html', controller: 'ipdRegOutCtrl' });//个案结案表dc_RegLifeQualityEval
            $stateProvider.state('RegQuestionEvalRec', { url: '/dc/RegQuestionEvalRec/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_RegQuestionEvalRec.html', controller: 'RegQuestionEvalRecCtrl' });//受托长辈适应程度评估表
            $stateProvider.state('RegLifeQualityEval', { url: '/dc/RegLifeQualityEval/:FeeNo', templateUrl: '/Views/DC/SocialWorker/dc_RegLifeQualityEval.html', controller: 'regLifeQualityEval' });//dc_RegLifeQualityEval
            $stateProvider.state('Tsg', { url: '/angular/Tsg', templateUrl: '/Views/TSG/Tsg.html', controller: 'TsgCtrl' });//支持与帮助
            $locationProvider.html5Mode(true);
           
            
        }
]);

