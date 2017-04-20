angular.module('sltcApp')
.config([
        '$stateProvider', '$urlRouterProvider', '$locationProvider', 'cfpLoadingBarProvider', function ($stateProvider, $urlRouterProvider, $locationProvider, cfpLoadingBarProvider) {

            cfpLoadingBarProvider.spinnerTemplate = '<div class="loading"><span class="fa fa-spinner fa-spin" style="color: #FFF;"></span> 加载中...</div>';
            cfpLoadingBarProvider.latencyThreshold = 800;
             //$urlRouterProvider.when("/", "/angular/ResidentList").otherwise('/');
            $urlRouterProvider.when("/", "/angular/MyDesk").otherwise('/');
           
            $stateProvider.state('AssignTask', { url: '/angular/AssignTask', templateUrl: '/Views/Message/AssignTask.html', controller: 'assignTaskCtrl' });
            $stateProvider.state('NoticeDetail', { url: '/angular/Notice/:id', templateUrl: '/Views/Message/NoticeDetail.html', controller: 'noticeDetailCtrl' });
            $stateProvider.state('Notice', { url: '/angular/Notice', templateUrl: '/Views/Message/Notice.html', controller: 'noticeCtrl' });
            $stateProvider.state('NoticeAdd', { url: '/angular/NoticeEdit', templateUrl: '/Views/Message/NoticeEdit.html', controller: 'noticeEditCtrl' });
            $stateProvider.state('NoticeEdit', { url: '/angular/NoticeEdit/:id', templateUrl: '/Views/Message/NoticeEdit.html', controller: 'noticeEditCtrl' });

            //个人基本资料 张正泉
            $stateProvider.state('PersonList', { url: '/angular/PersonList', templateUrl: '/Views/Resident/PersonList.html', controller: 'personListCtrl' });
            $stateProvider.state('Person', { url: '/angular/Person/:id/:mode', templateUrl: '/Views/Resident/Person.html', controller: 'personCtrl' });
            $stateProvider.state('Person.BasicInfo', { url: '/BasicInfo', templateUrl: '/Views/Resident/BasicInfo.html' });
            $stateProvider.state('Person.HisDisease', { url: '/HisDisease', templateUrl: '/Views/Resident/HisDisease.html' });
            $stateProvider.state('Person.HomePhoto', { url: '/HomePhoto', templateUrl: '/Views/Resident/HomePhoto.html' });
            $stateProvider.state('Person.AttachArchives', { url: '/AttachArchives', templateUrl: '/Views/Resident/AttachArchiveList.html' });
            $stateProvider.state('Person.Address', { url: '/Address', templateUrl: '/Views/Resident/Address.html' });
            $stateProvider.state('Person.Contact', { url: '/Contact', templateUrl: '/Views/Resident/Contact.html' });

            //健康档案
            $stateProvider.state('HealthRecords', { url: '/angular/HealthRecords/:FeeNo', templateUrl: '/Views/Resident/HealthRecords.html', controller: 'healthRecordsCtrl' });
            //首次入住记录
            $stateProvider.state('FstRegRec', { url: '/angular/FstRegRec/:id', templateUrl: '/Views/Resident/FstRegRec.html', controller: 'fstRegRecCtrl' });
            $stateProvider.state('ServiceResidentList', { url: '/angular/ResidentList/:mode', templateUrl: '/Views/Resident/ServiceResidentList.html' });
            $stateProvider.state('Resident', { url: '/angular/Resident', templateUrl: '/Views/Resident/Residents.html', controller: 'residentCtrl' });
            //$stateProvider.state('Resident.BasicInfo', { url: '/BasicInfo', templateUrl: '/Views/Resident/BasicInfo.html', controller: 'basicInfoCtrl' });
            $stateProvider.state('Resident.Welfare', { url: '/Welfare', templateUrl: '/Views/Resident/Welfare.html', controller: 'welfareCtrl' });
            $stateProvider.state('Resident.Contact', { url: '/Contact', templateUrl: '/Views/Resident/Contact.html', controller: 'contactCtrl' });

            //$stateProvider.state('Resident.HealthManage', { url: '/HealthManage', templateUrl: '/Views/Resident/HealthManage.html', controller: 'healthManageCtrl' });
            //$stateProvider.state('Resident.RequirementManage', { url: '/RequirementManage', templateUrl: '/Views/Resident/RequirementManage.html', controller: 'requirementManageCtrl' });
            //$stateProvider.state('Resident.CaseClosedManage', { url: '/CaseClosedManage', templateUrl: '/Views/Resident/CaseClosedManage.html', controller: 'caseClosedManageCtrl' });
            //$stateProvider.state('Resident.ChargeCheck', { url: '/ChargeCheck', templateUrl: '/Views/Resident/ChargeCheck.html', controller: 'chargeCheckCtrl' });
            $stateProvider.state('Resident.AttachArchive', { url: '/AttachArchives', templateUrl: '/Views/Resident/AttachArchiveList.html' });
            $stateProvider.state('Resident.HomePhoto', { url: '/HomePhoto', templateUrl: '/Views/Resident/HomePhoto.html', controller: 'homePhotoCtrl' });
            //预收款 update by zhangkai 20160311
            $stateProvider.state('PrePayment', { url: '/angular/PrePayment/:FeeNo', templateUrl: '/Views/Resident/PrePaymentEdit.html', controller: 'prePaymentEditCtrl' });


            //$stateProvider.state('Wizard', { url: '/angular/Wizard/:id', templateUrl: '/Views/Resident/Wizard.html', controller: 'wizardCtrl' });
            //$stateProvider.state('Wizard.Resident', { url: '/Resident', templateUrl: '/Views/Resident/Resident.html', controller: 'residentCtrl' });
            //$stateProvider.state('Wizard.Resident.BasicInfo', { url: '/BasicInfo', templateUrl: '/Views/Resident/BasicInfo.html', controller: 'basicInfoCtrl' });
            //$stateProvider.state('Wizard.Resident.Welfare', { url: '/Welfare', templateUrl: '/Views/Resident/Welfare.html', controller: 'welfareCtrl' });

            //入住登记-分步
            $stateProvider.state('regResident', { url: '/angular/regResident', templateUrl: '/Views/Resident/regResident.html', controller: 'regResidentCtrl' });


      

            //请假
            $stateProvider.state('LeaveHosp', { url: '/angular/LeaveHosp/:FeeNo/:IpdFlag', templateUrl: '/Views/Resident/LeaveHosp.html', controller: "leaveHospCtrl" });
            //零用金
            $stateProvider.state('PinMoney', { url: '/angular/PinMoney/:FeeNo', templateUrl: '/Views/Resident/PinMoney.html', controller: "pinMoneyCtrl" });
            //亲友访视
            $stateProvider.state('RegVisitRec', { url: '/angular/RegVisitRec/:FeeNo/:IpdFlag', templateUrl: '/Views/Resident/RegVisitRec.html' });
            //入住审核 update by zhangkai 20160311
            $stateProvider.state('ChargeCheck', { url: '/angular/ChargeCheck/:FeeNo', templateUrl: '/Views/Resident/ChargeSingleCheck.html'});

            $stateProvider.state('Welfare', { url: '/angular/Welfare/:FeeNo', templateUrl: '/Views/Resident/Welfare.html', controller: 'welfareCtrl' });
            //健康管理 
            $stateProvider.state('HealthManage', { url: '/angular/HealthManage/:FeeNo', templateUrl: '/Views/Resident/HealthManage.html', controller: 'healthManageCtrl' });
            //新需求管理
            $stateProvider.state('RequirementManage', { url: '/angular/RequirementManage/:FeeNo', templateUrl: '/Views/Resident/RequirementManage.html', controller: 'requirementManageCtrl' });
            $stateProvider.state('Ipdregout', { url: '/angular/Ipdregout/:FeeNo', templateUrl: '/Views/Resident/Ipdregout.html' });//出院办理
            //新结案 
            $stateProvider.state('CaseClose', { url: '/angular/CaseClose/:FeeNo', templateUrl: '/Views/Resident/CaseClose.html' });

            //营养筛查zhongyh
            $stateProvider.state('NutrtionEval', { url: '/angular/NutrtionEval/:FeeNo', templateUrl: '/Views/Resident/NutrtionEval.html', controller: "NutrtionEvalCtrl" });
            //团体活动 GroupActivity
            $stateProvider.state('GroupActivityList', { url: '/angular/GroupActivityList', templateUrl: '/Views/NurseStation/GroupActivityList.html' });
            $stateProvider.state('GroupActivityAdd', { url: '/angular/GroupActivityAdd', templateUrl: '/Views/NurseStation/GroupActivityEdit.html', controller: 'groupActivityEditCtrl' });
            $stateProvider.state('GroupActivityEdit', { url: '/angular/GroupActivityEdit/:id', templateUrl: '/Views/NurseStation/GroupActivityEdit.html', controller: 'groupActivityEditCtrl' });
            //活动成员个别化评估 memberAssessRes
            $stateProvider.state('MemberAssess', { url: '/angular/MemberAssess/:id', templateUrl: '/Views/NurseStation/MemberAssess.html', controller: 'memberAssessCtrl' });
            //活动照片
            $stateProvider.state('ActivityPhoto', { url: '/angular/ActivityPhoto/:id', templateUrl: '/Views/NurseStation/ActivityPhoto.html', controller: 'activityPhotoCtrl' });

            $stateProvider.state('CompanyList', { url: '/angular/companyList', templateUrl: '/Views/OrganizationManage/CompanyList.html', controller: 'companyListCtrl' });
            $stateProvider.state('CompanyAdd', { url: '/angular/companyEdit', templateUrl: '/Views/OrganizationManage/CompanyEdit.html', controller: 'companyEditCtrl' });
            $stateProvider.state('CompanyEdit', { url: '/angular/companyEdit/:id', templateUrl: '/Views/OrganizationManage/CompanyEdit.html', controller: 'companyEditCtrl' });
            $stateProvider.state('FloorList', { url: '/angular/floorList', templateUrl: '/Views/OrganizationManage/FloorList.html', controller: 'floorListCtrl' });
            $stateProvider.state('FloorAdd', { url: '/angular/floorEdit', templateUrl: '/Views/OrganizationManage/FloorEdit.html', controller: 'floorEditCtrl' });
            $stateProvider.state('FloorEdit', { url: '/angular/floorEdit/:id', templateUrl: '/Views/OrganizationManage/FloorEdit.html', controller: 'floorEditCtrl' });
            $stateProvider.state('DeptList', { url: '/angular/deptList', templateUrl: '/Views/OrganizationManage/DeptList.html', controller: 'deptListCtrl' });
            $stateProvider.state('DeptAdd', { url: '/angular/deptEdit', templateUrl: '/Views/OrganizationManage/DeptEdit.html', controller: 'deptEditCtrl' });
            $stateProvider.state('DeptEdit', { url: '/angular/deptEdit/:id', templateUrl: '/Views/OrganizationManage/DeptEdit.html', controller: 'deptEditCtrl' });
            $stateProvider.state('RoomList', { url: '/angular/roomList', templateUrl: '/Views/OrganizationManage/RoomList.html', controller: 'roomListCtrl' });
            $stateProvider.state('RoomAdd', { url: '/angular/roomEdit', templateUrl: '/Views/OrganizationManage/RoomEdit.html', controller: 'roomEditCtrl' });
            $stateProvider.state('RoomEdit', { url: '/angular/roomEdit/:id', templateUrl: '/Views/OrganizationManage/RoomEdit.html', controller: 'roomEditCtrl' });
            $stateProvider.state('BedList', { url: '/angular/bedList', templateUrl: '/Views/OrganizationManage/BedList.html', controller: 'bedListCtrl' });
            $stateProvider.state('BedAdd', { url: '/angular/bedEdit', templateUrl: '/Views/OrganizationManage/BedEdit.html', controller: 'bedEditCtrl' });
            $stateProvider.state('BedEdit', { url: '/angular/bedEdit/:id', templateUrl: '/Views/OrganizationManage/BedEdit.html', controller: 'bedEditCtrl' });
            //RoomBedV2
            $stateProvider.state('BedOv', { url: '/angular/bedOv', templateUrl: '/Views/OrganizationManage/BedOverView.html', controller: 'bedOverViewCtrl' });

            //集团管理
            $stateProvider.state('GroupList', { url: '/angular/GroupList', templateUrl: '/Views/OrganizationManage/GroupList.html', controller: 'groupListCtrl' });
            $stateProvider.state('GroupAdd', { url: '/angular/GroupEdit', templateUrl: '/Views/OrganizationManage/GroupEdit.html', controller: 'groupEditCtrl' });
            $stateProvider.state('GroupEdit', { url: '/angular/GroupEdit/:id', templateUrl: '/Views/OrganizationManage/GroupEdit.html', controller: 'groupEditCtrl' });
            //机构管理
            $stateProvider.state('OrgList', { url: '/angular/OrgList', templateUrl: '/Views/OrganizationManage/OrgList.html', controller: 'orgListCtrl' });
            $stateProvider.state('OrgAdd', { url: '/angular/OrgDtl', templateUrl: '/Views/OrganizationManage/OrgDtl.html', controller: 'orgDtlCtrl' });
            $stateProvider.state('OrgEdit', { url: '/angular/OrgDtl/:id', templateUrl: '/Views/OrganizationManage/OrgDtl.html', controller: 'orgDtlCtrl' });
            //$stateProvider.state('CodeList', { url: '/angular/CodeList', templateUrl: '/Views/OrganizationManage/CodeManage.html', controller: 'codeListCtrl' });
            //员工管理
            $stateProvider.state('StaffList', { url: '/angular/StaffList', templateUrl: '/Views/OrganizationManage/StaffList.html', controller: 'staffListCtrl' });
            $stateProvider.state('StaffAdd', { url: '/angular/StaffEdit', templateUrl: '/Views/OrganizationManage/StaffEdit.html', controller: 'staffEditCtrl' });
            $stateProvider.state('StaffEdit', { url: '/angular/StaffEdit/:id', templateUrl: '/Views/OrganizationManage/StaffEdit.html', controller: 'staffEditCtrl' });
            $stateProvider.state('MyAccount', { url: '/angular/MyAccount/:id', templateUrl: '/Views/Home/MyAccount.html', controller: 'myAccountCtrl' });
            //用户管理
            $stateProvider.state('UserList', { url: '/angular/UserList', templateUrl: '/Views/OrganizationManage/UserList.html', controller: 'userListCtrl' });
            $stateProvider.state('UserAdd', { url: '/angular/UserEdit', templateUrl: '/Views/OrganizationManage/UserEdit.html', controller: 'userEditCtrl' });
            $stateProvider.state('UserEdit', { url: '/angular/UserEdit/:id', templateUrl: '/Views/OrganizationManage/UserEdit.html', controller: 'userEditCtrl' });
            //角色管理
            $stateProvider.state('RoleList', { url: '/angular/RoleList', templateUrl: '/Views/OrganizationManage/RoleList.html', controller: 'roleListCtrl' });
            $stateProvider.state('RoleAdd', { url: '/angular/RoleEdit', templateUrl: '/Views/OrganizationManage/RoleEdit.html', controller: 'roleEditCtrl' });
            $stateProvider.state('RoleEdit', { url: '/angular/RoleEdit/:id', templateUrl: '/Views/OrganizationManage/RoleEdit.html', controller: 'roleEditCtrl' });
            //模块管理
            $stateProvider.state('ModuleEdit', { url: '/angular/OrganizationManage/ModuleEdit', templateUrl: '/Views/OrganizationManage/ModuleEdit.html' });

            //字典管理
            $stateProvider.state('CodeList', { url: '/angular/CodeList', templateUrl: '/Views/OrganizationManage/CodeList.html', controller: 'codeListCtrl' });
            $stateProvider.state('CodeAdd', { url: '/angular/CodeEdit', templateUrl: '/Views/OrganizationManage/CodeEdit.html', controller: 'codeEditCtrl' });
            $stateProvider.state('CodeEdit', { url: '/angular/CodeEdit/:id', templateUrl: '/Views/OrganizationManage/CodeEdit.html', controller: 'codeEditCtrl' });

            //厂商管理
            $stateProvider.state('manufactureList', { url: '/angular/ManufactureList', templateUrl: '/Views/Goods/ManufactureList.html', controller: 'manufactureListCtrl' });
            $stateProvider.state('manufactureAdd', { url: '/angular/ManufactureEdit', templateUrl: '/Views/Goods/ManufactureEdit.html', controller: 'manufactureEditCtrl' });
            $stateProvider.state('manufactureEdit', { url: '/angular/ManufactureEdit/:id', templateUrl: '/Views/Goods/ManufactureEdit.html', controller: 'manufactureEditCtrl' });

            //物品管理
            $stateProvider.state('goodsList', { url: '/angular/goodsList', templateUrl: '/Views/Goods/GoodsList.html', controller: 'goodsListCtrl' });
            //$stateProvider.state('goodsAdd', { url: '/angular/goodsEdit', templateUrl: '/Views/OrganizationManage/GoodsEdit.html', controller: 'goodsEditCtrl' });
            $stateProvider.state('goodsEdit', { url: '/angular/goodsEdit/:id', templateUrl: '/Views/Goods/GoodsEdit.html', controller: 'goodsEditCtrl' });
            $stateProvider.state('goodsEdit.Sale', { url: '/Sale', templateUrl: '/Views/Goods/Sale.html', controller: 'saleCtrl' });
            $stateProvider.state('goodsEdit.Loan', { url: '/Loan', templateUrl: '/Views/Goods/Loan.html', controller: 'loanCtrl' });

            $stateProvider.state('storeList', { url: '/angular/storeList', templateUrl: '/Views/Goods/StoreList.html', controller: 'storeListCtrl' });
            $stateProvider.state('storeAdd', { url: '/angular/storeEdit', templateUrl: '/Views/Goods/StoreEdit.html', controller: 'storeEditCtrl' });
            $stateProvider.state('storeEdit', { url: '/angular/storeEdit/:id', templateUrl: '/Views/Goods/StoreEdit.html', controller: 'storeEditCtrl' });

            
            //controller: 'regResidentCtrl'

            //费用管理
            $stateProvider.state('ChargeItem', { url: '/angular/charge/chargeitem', templateUrl: '/Views/Charges/ChargeItemList.html', controller: "chargeItemCtrl" });
            $stateProvider.state('ChargeItemAdd', { url: '/angular/charge/chargeitemedit', templateUrl: '/Views/Charges/ChargeItemEdit.html', controller: "chargeItemEditCtrl" });
            $stateProvider.state('ChargeItemEdit', { url: '/angular/charge/chargeitemedit/:id', templateUrl: '/Views/Charges/ChargeItemEdit.html', controller: "chargeItemEditCtrl" });
            $stateProvider.state('ChargeGroup', { url: '/angular/charge/chargegroup', templateUrl: '/Views/Charges/ChargeGroupList.html', controller: "chargeGroupCtrl" });
            $stateProvider.state('ChargeGroupAdd', { url: '/angular/charge/chargegroupadd', templateUrl: '/Views/Charges/ChargeGroupEdit.html', controller: "chargeGroupEditCtrl" });
            $stateProvider.state('ChargeGroupEdit', { url: '/angular/charge/chargegroupadd/:id', templateUrl: '/Views/Charges/ChargeGroupEdit.html', controller: "chargeGroupEditCtrl" });
            $stateProvider.state('ChargeInput', { url: '/angular/charge/chargeinput/:FeeNo', templateUrl: '/Views/Charges/ChargeInput.html', controller: "chargeInputCtrl" });
            $stateProvider.state('BillList', { url: '/angular/charge/billList/:FeeNo', templateUrl: '/Views/Charges/BillList.html', controller: "billListCtrl" });
            $stateProvider.state('FixChargeSetting', { url: '/angular/charge/fixedchargesetting/:FeeNo', templateUrl: '/Views/Charges/FixedChargeSettings.html', controller: "fixedChargeCtrl" });
            //帐单缴费
            $stateProvider.state('PayBills', { url: '/angular/PayBills/:FeeNo', templateUrl: '/Views/Charges/PayBills.html', controller: "payBillsCtrl" });
            $stateProvider.state('BillPay', { url: '/angular/BillPay', templateUrl: '/Views/Charges/BillPay.html', controller: "billPayCtrl" });
            $stateProvider.state('BillPayList', { url: '/angular/BillPayList/:id', templateUrl: '/Views/Charges/BillPayList.html', controller: "billPayListCtrl" });
            $stateProvider.state('BillPayAdd', { url: '/angular/BillPayDtl/:id', templateUrl: '/Views/Charges/BillPayDtl.html', controller: "billPayDtlCtrl" });
            $stateProvider.state('BillPayDtl', { url: '/angular/BillPayDtl/:id', templateUrl: '/Views/Charges/BillPayDtl.html', controller: "billPayDtlCtrl" });

            //生命体征批量录入
            $stateProvider.state('VitalSignsRecord', { url: '/angular/VitalSigns', templateUrl: '/Views/Nursing/VitalSigns.html', controller: "vitalSignsCtrl" });
            //出入量批量录入
            $stateProvider.state('OutInValueRecord', { url: '/angular/OutInValue', templateUrl: '/Views/Nursing/OutInValue.html', controller: "outInValueCtrl" });

            //生命体征列表
            $stateProvider.state('VitalSignList', { url: '/angular/VitalSignList/:feeNo', templateUrl: '/Views/Nursing/VitalSignList.html', controller: "vitalSignListCtrl" });
            //出入量列表
            $stateProvider.state('OutInValueList', { url: '/angular/OutInValueList/:feeNo', templateUrl: '/Views/Nursing/OutInValueList.html', controller: "outInValueListCtrl" });
            //生命体征
            //$stateProvider.state('VitalSignsRecord', { url: '/angular/VitalSignsRecord', templateUrl: '/Views/NurseStation/VitalSignRecord.html'});
            //出入量
            //$stateProvider.state('OutInValueRecord', { url: '/angular/OutInValueRecord', templateUrl: '/Views/NurseStation/OutInValueRecord.html'});
            //护理评估
            $stateProvider.state('EvaluationList', { url: '/angular/EvaluationList/:code/:qName', templateUrl: '/Views/Nursing/EvaluationList.html', controller: 'evaluationListCtr' });
            $stateProvider.state('EvalSheetTemp', { url: '/angular/EvalSheetTemp/:qCode/:qName/:FeeNo', templateUrl: '/Views/Nursing/EvalSheetTemp.html', controller: 'evaluationSheetCtr' });
            $stateProvider.state('EvalSheetHistory', { url: '/angular/EvalSheetHistory/:qId/:feeNo/:regName/:qName', templateUrl: '/Views/Nursing/EvalSheetHistory.html', controller: 'evalSheetHistoryCtr' });
            $stateProvider.state('NursingDemandList', { url: '/angular/NursingDemandList', templateUrl: '/Views/Nursing/NursingDemandList.html', controller: 'nurDemandListCtr' });
            $stateProvider.state('NursingDemand', { url: '/angular/NursingDemandSheet/:feeNo/:regNo/:id', templateUrl: '/Views/Nursing/NursingDemandSheet.html', controller: 'nurDemandSheetCtr' });
            $stateProvider.state('NursingDemandHis', { url: '/angular/NursingDemandHis/:feeNo/:regNo', templateUrl: '/Views/Nursing/NursingDemandHis.html', controller: 'nursingDemandHisCtr' });
            $stateProvider.state('PipelineRec', { url: '/angular/PipelineRec/:FeeNo', templateUrl: '/Views/Nursing/PipelineRec.html', controller: 'pipelineCtrl' });
            $stateProvider.state('Pipeline', { url: '/angular/Pipeline', templateUrl: '/Views/Nursing/Pipeline.html', controller: 'pipelineCtr' });

            $stateProvider.state('BiochemistryList', { url: '/angular/BiochemistryList/:FeeNo', templateUrl: '/Views/Nursing/BiochemistryList.html', controller: 'biochemistryListCtr' });
            //第二个conbin
           // $stateProvider.state('BiochemistryLists', { url: '/angular/BiochemistryList', templateUrl: '/Views/Nursing/BiochemistryList.html', controller: 'biochemistryCtr' });

            $stateProvider.state('Biochemistry', { url: '/angular/Biochemistry/:id', templateUrl: '/Views/Nursing/Biochemistry.html', controller: 'biochemistryCtr' });
            $stateProvider.state('InjectionList', { url: '/angular/InjectionList/:FeeNo', templateUrl: '/Views/Nursing/InjectionList.html', controller: 'injectionListCtr' });
            $stateProvider.state('Injection', { url: '/angular/Injection/:regId/:id', templateUrl: '/Views/Nursing/Injection.html', controller: 'injectionCtr' });
            $stateProvider.state('InjectionHistory', { url: '/angular/InjectionHistory/:regNo', templateUrl: '/Views/Nursing/InjectionHistory.html', controller: 'injectionHisCtr' });
            $stateProvider.state('RehabilitationList', { url: '/angular/RehabilitationList/:FeeNo', templateUrl: '/Views/Nursing/RehabilitationList.html', controller: 'rehabilitationCtr' });
            $stateProvider.state('Rehabilitation', { url: '/angular/Rehabilitation/:id', templateUrl: '/Views/Nursing/Rehabilitation.html', controller: 'rehabilitationCtr' });
            //复健 姚丙慧 
            $stateProvider.state('referral', { url: '/angular/referral', templateUrl: '/Views/Nursing/RehabilitationList.html', controller: 'rehabilitationCtr' });
            //转诊 姚丙慧
            $stateProvider.state('referralList', { url: '/angular/ReferralList/:FeeNo', templateUrl: '/Views/Nursing/ReferralList.html', controller: 'ReferralListCtr' });
            // 护士日报表界面
            $stateProvider.state('nurseDailyReportList', { url: '/angular/NurseDailyReportList/:FeeNo', templateUrl: '/Views/Nursing/NurseDailyReportList.html', controller: 'nurseDailyReportListCtr' });
            // 姚丙慧 常用语管理
            $stateProvider.state('Commfilelists', { url: '/angular/Commfilelists', templateUrl: '/Views/OrganizationManage/CommFileManage.html', controller: 'CommfilelistsCtr' });
            
            $stateProvider.state('Referral', { url: '/angular/Referral/:id', templateUrl: '/Views/Nursing/Referral.html', controller: 'referralCtr' });
            //常用语添加 
            $stateProvider.state('COMMFILEAdd', { url: '/angular/COMMFILEAdd', templateUrl: '/Views/OrganizationManage/CommFileAdd.html', controller: 'COMMFILEAddCtrl' });

            //常用语修改
            $stateProvider.state('COMMFILEEdit', { url: '/angular/COMMFILEedit/:id', templateUrl: '/Views/OrganizationManage/CommFileEdit.html', controller: 'COMMFILEeditCtrl' });

            //照护计划
            $stateProvider.state('MultidisciplinaryCarePlan', { url: '/angular/MultidisciplinaryCarePlan/:feeNo/:seqNo/:regNo/:regName', templateUrl: '/Views/CarePlans/MultidisciplinaryCarePlan.html', controller: 'multidisciplinaryCarePlanCtr' });
            $stateProvider.state('AddCarePlanGoal', { url: '/angular/AddCarePlanGoal/:seqNo/:feeNo/:regNo/:cpNo/:regName', templateUrl: '/Views/CarePlans/AddCarePlanGoal.html', controller: 'addCarePlanGoalCtr' });
            $stateProvider.state('AddCarePlanActivity', { url: '/angular/AddCarePlanActivity/:seqNo/:feeNo/:regNo/:cpNo/:regName', templateUrl: '/Views/CarePlans/AddCarePlanActivity.html', controller: 'addCarePlanActivityCtr' });
            $stateProvider.state('CarePlanList', { url: '/angular/CarePlanList/:FeeNo', templateUrl: '/Views/CarePlans/CarePlanList.html', controller: 'carePlanListCtr' });
            $stateProvider.state('CarePlanDetail', { url: '/angular/CarePlanDetail/:feeNo/:regNo/:regName', templateUrl: '/Views/CarePlans/CarePlanDetail.html', controller: 'carePlanDetailCtr' });
            $stateProvider.state('AddCarePlanAssess', { url: '/angular/AddCarePlanAssess/:seqNo/:feeNo/:regNo/:cpNo/:regName', templateUrl: '/Views/CarePlans/AddCarePlanAssess.html', controller: 'carePlanAssessCtr' });
            $stateProvider.state('CarePlanCalendar', { url: '/angular/CarePlanCalendar/:feeNo/:regNo/:regName', templateUrl: '/Views/CarePlans/CarePlanCalendar.html', controller: 'calendarCtrl' });
            //交付照会Add by zhangkai 20160314
            $stateProvider.state('AdminHandovers', { url: '/angular/AdminHandovers/:FeeNo', templateUrl: '/Views/OrganizationManage/AdminHandovers.html', controller: 'adminHandoversCtrl' });
            $stateProvider.state('ShiftWork', { url: '/angular/ShiftWork/:FeeNo', templateUrl: '/Views/NurseStation/ShiftWork.html', controller: 'shiftWorkCtrl' });
            //行政交班
            $stateProvider.state('AffairsHandoverList', { url: '/angular/AffairsHandoverList', templateUrl: '/Views/OrganizationManage/AffairsHandoverList.html', controller: 'affairsHandoverListCtrl' });
            $stateProvider.state('AffairsHandoverAdd', { url: '/angular/AffairsHandoverEdit', templateUrl: '/Views/OrganizationManage/AffairsHandoverEdit.html', controller: 'affairsHandoverEditCtrl' });
            $stateProvider.state('AffairsHandoverEdit', { url: '/angular/AffairsHandoverEdit/:id', templateUrl: '/Views/OrganizationManage/AffairsHandoverEdit.html', controller: 'affairsHandoverEditCtrl' });
            //行政交班v2
            $stateProvider.state('HandoverV2', { url: '/angular/HandoverV2', templateUrl: '/Views/NurseStation/HandoverV2.html', controller: 'handoverV2Ctrl' });

            //行政交班v2
            $stateProvider.state('LookOver', { url: '/angular/LookOver', templateUrl: '/Views/NurseStation/LookOver.html', controller: 'LookOverCtrl' });
            $stateProvider.state('LookOverAdd', { url: '/angular/LookOverEdit', templateUrl: '/Views/NurseStation/LookOverEdit.html', controller: 'LookOverEditCtrl' });
            $stateProvider.state('LookOverEdit', { url: '/angular/LookOverEdit/:id', templateUrl: '/Views/NurseStation/LookOverEdit.html', controller: 'LookOverEditCtrl' });

            //住院记录
            $stateProvider.state('HspRecordList', { url: '/angular/HspRecordList', templateUrl: '/Views/Resident/HspRecordList.html', controller: 'hspRecordListCtrl' });
            $stateProvider.state('hspRecordAdd', { url: '/angular/hspRecord', templateUrl: '/Views/Resident/HspRecord.html', controller: 'hspRecordEditCtrl' });
            $stateProvider.state('hspRecordEdit', { url: '/angular/hspRecord/:id', templateUrl: '/Views/Resident/HspRecord.html', controller: 'hspRecordEditCtrl' });

            //护理记录及交班
            $stateProvider.state('NursingRecord', { url: '/angular/NursingRecord', templateUrl: '/Views/Nursing/NursingRecordMain.html', controller: 'nursingRecordMainCtrl' });
            $stateProvider.state('NursingRecord.Record', { url: '/Record/:FeeNo', templateUrl: '/Views/Nursing/NursingRecord.html', controller: 'nursingRecordCtrl' });
            $stateProvider.state('NursingRecord.SingleShift', { url: '/SingleShift', templateUrl: '/Views/Nursing/NursingSingleShift.html', controller: 'nursingSingleShiftCtrl' });
            $stateProvider.state('NursingRecord.MultiShift', { url: '/MultiShift', templateUrl: '/Views/Nursing/NursingMultiShift.html', controller: 'nursingMultiShiftCtrl' });
            //社工-行政  杨金高
            //社工评估
            $stateProvider.state('EvaluateList', { url: '/angular/EvaluateList', templateUrl: '/Views/Socialworker/EvaluateList.html', controller: 'evalPersonListCtrl' });
            $stateProvider.state('EvaluateDetail', { url: '/angular/EvaluateDtl/:id', templateUrl: '/Views/Socialworker/EvaluateDtl.html', controller: 'evalPersonCtrl' });
            $stateProvider.state('EvaluateAdd', { url: '/angular/EvaluateAdd/:FeeNo', templateUrl: '/Views/Socialworker/EvaluateAdd.html', controller: 'regEvalueateCtrl' });
            $stateProvider.state('EvaluateAdd.PsychologyInfo', { url: '/PsychologyInfo', templateUrl: '/Views/Socialworker/tpls/PsychologyInfo.html', controller: 'regEvalueateCtrl' });
            $stateProvider.state('EvaluateAdd.HomeEvaluate', { url: '/HomeEvaluate', templateUrl: '/Views/Socialworker/tpls/HomeEvaluate.html', controller: 'regEvalueateCtrl' });
            $stateProvider.state('EvaluateAdd.SocietyResEvaluate', { url: '/SocietyResEvaluate', templateUrl: '/Views/Socialworker/tpls/SocialRelEvaluate.html', controller: 'regEvalueateCtrl' });
            $stateProvider.state('EvaluateAdd.QuestionEvaluate', { url: '/QuestionEvaluate', templateUrl: '/Views/Socialworker/tpls/QuestionEvaluate.html', controller: 'regEvalueateCtrl' });
            $stateProvider.state('EvaluateAdd.SocialRelEvaluate', { url: '/SocialRelEvaluate', templateUrl: '/Views/Socialworker/tpls/SocialRelEvaluate.html', controller: 'regEvalueateCtrl' });
            $stateProvider.state('EvaluateAdd.RelativeSdiscussEvaluate', { url: '/RelativeSdiscussEvaluate', templateUrl: '/Views/Socialworker/tpls/RelativeSdiscussEvaluate.html', controller: 'regEvalueateCtrl' });
            $stateProvider.state('EvaluateHistory', { url: '/EvaluateHistory', templateUrl: '/Views/Socialworker/EvaluateHistory.html', controller: 'evalPersonListCtrl' });
            //生活记录  杨金高
            $stateProvider.state('LifeRecordsOld', { url: '/angular/LifeRecordsOld/', templateUrl: '/Views/Socialworker/LifeRecordList.html', controller: 'lifeRecordListCtrl' });
            $stateProvider.state('LifeRecordsHistory', { url: '/angular/LifeRecordsHistory/:id', templateUrl: '/Views/Socialworker/LifeRecordsHistory.html', controller: 'lifeListCtrl' });
            //营养评估单2016-7-7
            $stateProvider.state('NutritionEvalList', { url: '/angular/NutritionEvalList/:FeeNo', templateUrl: '/Views/Socialworker/NutritionEvalList.html', controller: 'NutritionEvalListCtrl' });
            //新进住民环境介绍记录表zhongyh
            $stateProvider.state('NewResideEntenvRec', { url: '/angular/NewResideEntenvRec/:FeeNo', templateUrl: '/Views/SocialWorker/NewResideEntenvRec.html', controller: 'NewResideEntenvRecCtrl' });
            //新进住民环境适应及辅导记录zhongyh
            $stateProvider.state('NewRegEnvAdaptation', { url: '/angular/NewRegEnvAdaptation/:FeeNo', templateUrl: '/Views/SocialWorker/NewRegEnvAdaptation.html', controller: 'NewRegEnvAdaptationCtrl' });

            //生活记录 史垚祎
            $stateProvider.state('LifeRecords', {
                url: '/angular/LifeRecords/', templateUrl: '/Views/Socialworker/LifeRecord.html', controller: 'lifeRecordCtrl'
            });

            //$stateProvider.state('AddLifeRecord', { url: '/angular/AddLifeRecord', templateUrl: '/Views/Socialworker/AddLifeRecord.html', controller: 'lifeRecordsCtrl' });
            //$stateProvider.state('EditLifeRecord', { url: '/angular/EditLifeRecord/:id', templateUrl: '/Views/Socialworker/EditLifeRecord.html', controller: 'lifeRecordsCtrl' });
            //院民清册
            $stateProvider.state('ResidentListV2', { url: '/angular/ResidentListV2/', templateUrl: '/Views/Socialworker/ResidentV2.html', controller: "personListV2Ctrl" });
            $stateProvider.state('ResidentRegDtl', { url: '/angular/ResidentRegDtl/:id', templateUrl: '/Views/Socialworker/ResidentRegDtl.html', controller: "residentV2Ctrl" });
            //补助申请 杨金高
            $stateProvider.state('SubsidyRec', { url: '/angular/SubsidyRec/:FeeNo', templateUrl: '/Views/Socialworker/SubsidyRec.html', controller: 'subsidyListCtrl' });
            //资源连接 杨金高
            $stateProvider.state('ResourceLink', { url: '/angular/ResourceLink/:FeeNo', templateUrl: '/Views/Socialworker/ResourceLink.html', controller: 'resourceLinkCtrl' });
            $stateProvider.state('ResourceLinkHistory', { url: '/angular/ResourceLinkHistory/:id', templateUrl: '/Views/Socialworker/ResourceLinkHistory.html', controller: 'resourceLinkCtrl' });
            //申诉-权益维护  杨金高
            $stateProvider.state('ComplainRec', { url: '/angular/ComplainRec/:FeeNo', templateUrl: '/Views/Socialworker/ComplainRec.html', controller: 'complainRecCtrl' });
            //跨专业提案讨论　杨金高
            $stateProvider.state('ProposalDisscuss', { url: '/angular/ProposalDisscuss', templateUrl: '/Views/Socialworker/ProposalDisscuss.html', controller: 'evalPersonListCtrl' });
            $stateProvider.state('ProposalDisscussHistory', { url: '/angular/ProposalDisscussHistory/:id', templateUrl: '/Views/Socialworker/ProposalDisscussHistory.html', controller: 'proposalDisscussCtrl' });
            //个案研讨会议记录　杨金高
            $stateProvider.state('ProposalDisscussrec', { url: '/angular/ProposalDisscussrec', templateUrl: '/Views/Socialworker/ProposalDisscussrec.html', controller: 'evalPersonListCtrl' });
            $stateProvider.state('ProposalDisscussrecHistory', { url: '/angular/ProposalDisscussrecHistory/:id', templateUrl: '/Views/Socialworker/ProposalDisscussrecHistory.html', controller: 'proposalDisscussrecCtrl' });
            //服务中心日志　杨金高
            $stateProvider.state('HomeCareSvrrec', { url: '/angular/HomeCareSvrrec', templateUrl: '/Views/Socialworker/HomeCareSvrrec.html', controller: 'evalPersonListCtrl' });
            $stateProvider.state('HomeCareSvrrecHistory', { url: '/angular/HomeCareSvrrecHistory/:id', templateUrl: '/Views/Socialworker/HomeCareSvrrecHistory.html', controller: 'homeCareSvrrecCtrl' });
            //居家服务督导记录　杨金高
            $stateProvider.state('HomeCareSupervise', { url: '/angular/HomeCareSupervise', templateUrl: '/Views/Socialworker/HomeCareSupervise.html', controller: 'homeCareSuperviseCtrl' });
            $stateProvider.state('HomeCareSuperviseHistory', { url: '/angular/HomeCareSuperviseHistory/:id', templateUrl: '/Views/Socialworker/HomeCareSuperviseHistory.html', controller: 'homeCareSuperviseCtrl' });
            //转介　杨金高
            $stateProvider.state('ReferralRec', { url: '/angular/ReferralRec/:FeeNo', templateUrl: '/Views/Socialworker/ReferralRec.html', controller: 'referralRecCtrl' });
            //社工服务记录
            $stateProvider.state('CarersvrRec', { url: '/angular/CarersvrRec/:FeeNo', templateUrl: '/Views/Socialworker/CarersvrRec.html', controller: 'carersvrCtrl' });
            //家庭支持　杨金高
            $stateProvider.state('FamilySupport', { url: '/angular/FamilySupport', templateUrl: '/Views/Socialworker/FamilySupport.html', controller: 'evalPersonListCtrl' });
            $stateProvider.state('FamilySupportHistory', { url: '/angular/FamilySupportHistory/:id', templateUrl: '/Views/Socialworker/FamilySupportHistory.html', controller: 'familyDiscussrecCtrl' });
            //药品管理　杨金高
            $stateProvider.state('DrugList', { url: '/angular/DrugList', templateUrl: '/Views/Nursing/DrugList.html', controller: 'drugCtrl' });
            // $stateProvider.state('FamilySupportHistory', { url: '/angular/FamilySupportHistory/:id', templateUrl: '/Views/Socialworker/FamilySupportHistory.html', controller: 'familyDiscussrecCtrl' });
            //就医管理  杨金高
            $stateProvider.state('VisitDocRecords', { url: '/angular/VisitDocRecords/:FeeNo', templateUrl: '/Views/Nursing/VisitDocRecords.html', controller: 'visitDocRecordCtrl' });
            $stateProvider.state('AddVisitDocRecord', { url: '/angular/AddVisitDocRecord', templateUrl: '/Views/Nursing/AddVisitDocRecord.html', controller: 'visitDocRecordCtrl' });
            $stateProvider.state('OwnDrugRec', { url: '/angular/OwnDrugRec/:FeeNo', templateUrl: '/Views/MedicalWork/OwnDrugRec.html', controller: 'ownDrugRecCtrl' });
            $stateProvider.state('InsulinInject', { url: '/angular/InsulinInject', templateUrl: '/Views/Nursing/InsulinInjects.html', controller: 'insulinInjectCtrl' });
            //指标管理 D
            $stateProvider.state('Fall', { url: '/angular/Fall/:FeeNo', templateUrl: '/Views/KPI/Fall.html', controller: 'fallingCtrl' }); //跌到
            $stateProvider.state('PrsSore', { url: '/angular/PrsSore/:FeeNo', templateUrl: '/Views/KPI/PrsSoreNew.html', controller: 'prsSoreNewCtrl' }); //压疮
            $stateProvider.state('Infection', { url: '/angular/Infection/:FeeNo', templateUrl: '/Views/KPI/InfectionNew.html', controller: 'infectionNewCtrl' });//感染
            $stateProvider.state('Restrict', { url: '/angular/Restrict/:FeeNo', templateUrl: '/Views/KPI/RestrictNew.html', controller: 'restrictNewCtrl' });
            $stateProvider.state('Pain', { url: '/angular/Pain/:FeeNo', templateUrl: '/Views/KPI/PainNew.html', controller: 'painNewCtrl' });
            $stateProvider.state('UnPlanWeight', { url: '/angular/UnPlanWeight/:FeeNo', templateUrl: '/Views/KPI/UnPlanWeight.html', controller: 'unPlanWeightCtrl' });//非计划减重改变
            $stateProvider.state('UnPlanWeightList', { url: '/angular/UnPlanWeightList', templateUrl: '/Views/KPI/UnPlanWeightList.html', controller: 'unPlanWeightListCtrl' });//非计划减重改变
            $stateProvider.state('UnPlanWeightHistory', { url: '/angular/UnPlanWeightHistory/:id/:regNo', templateUrl: '/Views/KPI/UnPlanWeightHistory.html', controller: 'unPlanWeightHistoryCtrl' });
            $stateProvider.state('UnPlanIpd', { url: '/angular/UnPlanIpd/:FeeNo', templateUrl: '/Views/KPI/UnPlanIpd.html', controller: 'unPlanIpdCtrl' });//非计划住院
            $stateProvider.state('Pharmacist', { url: '/angular/Pharmacist/:FeeNo', templateUrl: '/Views/KPI/Pharmacist.html', controller: 'pharmacistEvalCtrl' });
            $stateProvider.state('DashBoard', { url: '/angular/DashBoard/:FeeNo', templateUrl: '/Views/Home/DashBoard.html', controller: 'dashBoardCtrl' });
            
            //个人推送信息
            $stateProvider.state('MsgList', { url: '/angular/MsgList', templateUrl: '/Views/Nursing/MsgList.html', controller: 'msgListCtrl' });
            //报表
            $stateProvider.state('ReportIndex', { url: '/angular/ReportIndex', templateUrl: '/Views/Report/ReportIndex.html', controller: "reportIndexCtrl" });
            $stateProvider.state('ReportManage', { url: '/angular/ReportManage/:FeeNo', templateUrl: '/Views/Report/ReportManage.html' });
            $stateProvider.state('ReportSet', { url: '/angular/ReportSet', templateUrl: '/Views/Report/ReportSet.html' });
            $stateProvider.state('ActivityAssessment', { url: '/angular/Report/ActivityAssessment', templateUrl: '/Views/NursingForm/ActivityAssessment.html' });

            $stateProvider.state('PhysicianAss', { url: '/angular/PhysicianAss/:FeeNo', templateUrl: '/Views/NurseStation/PhysicianAss.html', controller: 'PhysicianAssCtrl' });//医师评估
            $stateProvider.state('PhysicianRounds', { url: '/angular/PhysicianRounds/:FeeNo', templateUrl: '/Views/NurseStation/PhysicianRounds.html', controller: 'PhysicianRoundsCtrl' });//医师巡诊
            $stateProvider.state('MedicationRecord', { url: '/angular/MedicationRecord/:FeeNo', templateUrl: '/Views/NurseStation/MedicationRecord.html', controller: 'MedicationRecordCtrl' });//用药记录
            $stateProvider.state('Preipd', { url: '/angular/Preipd', templateUrl: '/Views/Resident/Preipd.html', controller: 'PreipdCtrl' });//预约登记
  
            $stateProvider.state('LeaveNursing', { url: '/angular/LeaveNursing', templateUrl: '/Views/Resident/LeaveNursing.html', controller: 'LeaveNursingCtrl' });//退住院
            $stateProvider.state('RegActivityRequEval', { url: '/angular/RegActivityRequEval/:FeeNo', templateUrl: '/Views/Socialworker/RegActivityRequEval.html', controller: 'RegActivityRequEvalCtrl' });//个别化
            $stateProvider.state('NutritionCareRec', { url: '/angular/NutritionCareRec/:FeeNo', templateUrl: '/Views/Nursing/NutritionCareRec.html', controller: 'NutritionCareRecCtrl' });//

            $stateProvider.state('MyDesk', { url: '/angular/MyDesk', templateUrl: '/Views/Home/MyDesk.html', controller: 'myDeskCtrl' }); //我的新桌面  孙伟 add
            $stateProvider.state('EvalTemplateManage', { url: '/angular/EvalTemplateManage', templateUrl: '/Views/EVM/EvalTemplateManage.html', controller: 'EvalTemplateCtrl' });//评估模板管理
            $stateProvider.state('EditEvalTemplateManage', { url: '/angular/EditEvalTemplateManage/:id', templateUrl: '/Views/EVM/EditEvalTemplateManage.html', controller: 'EditEvalTemplateCtrl' });//编辑评估模板管理
            $stateProvider.state('EvalTemplateManageSet', { url: '/angular/EvalTemplateManageSet', templateUrl: '/Views/EVM/EvalTemplateManageSet.html', controller: 'EvalTemplateSetCtrl' });
            $stateProvider.state('Tsg', { url: '/angular/Tsg', templateUrl: '/Views/TSG/Tsg.html', controller: 'TsgCtrl' });//支持与帮助
            $stateProvider.state('TsgQuestionList', { url: '/angular/TsgQuestionList', templateUrl: '/Views/TSG/TsgQuestionList.html', controller: 'TsgQuestionCtrl' });//帮助问题
            $stateProvider.state('EditTsgQuestion', { url: '/angular/EditTsgQuestion/:id', templateUrl: '/Views/TSG/EditTsgQuestion.html', controller: 'EditTsgQuestionCtrl' });
            $stateProvider.state('kindEditorDemo', { url: '/angular/KindEditorDemo', templateUrl: '/Views/TSG/KindEditorDemo.html', controller: 'kindeditorCtrl' });
            $stateProvider.state('exportReport', { url: '/angular/ExportReport/:FeeNo', templateUrl: '/Views/EvalReport/ExportReport.html', controller: 'exportReportCtrl' });
            $stateProvider.state('exportPeopleFeeReport', { url: '/angular/exportPeopleFeeReport', templateUrl: '/Views/EvalReport/exportPeopleFeeReport.html', controller: 'exportPeopleFeeReportCtrl' });
            $stateProvider.state('serviceDepositGrantList', { url: '/angular/ServiceDepositGrantList', templateUrl: '/Views/ServiceDeposit/ServiceDepositGrantList.html', controller: 'serviceDepositGrantListCtrl' });

            //缴费管理 Bob wu 201701091349
            //账单缴费
            $stateProvider.state('Payment', { url: '/angular/Payment', templateUrl: '/Views/FinancialManagement/Payment.html', controller: 'PaymentCtrl' });
            $stateProvider.state('Payment.BillPayment', { url: '/BillPayment/:FeeNo', templateUrl: '/Views/FinancialManagement/BillPayment.html', controller: "billPaymentCtrl" });
            $stateProvider.state('Payment.BillPaymentRec', { url: '/BillPaymentRec/:FeeNo', templateUrl: '/Views/FinancialManagement/BillPaymentRec.html', controller: "billPaymentRecCtrl" });
            $stateProvider.state('Payment.AdvanceCharge', { url: '/AdvanceCharge/:FeeNo', templateUrl: '/Views/FinancialManagement/AdvanceCharge.html', controller: "advanceChargeCtrl" });
            $stateProvider.state('Payment.RegAccountInfo', { url: '/RegAccountInfo/:FeeNo', templateUrl: '/Views/FinancialManagement/RegAccountInfo.html', controller: "RegAccountInfoCtrl" });

            //缴费退费
            $stateProvider.state('Refund', { url: '/angular/Refund', templateUrl: '/Views/FinancialManagement/Refund.html', controller: 'RefundCtrl' });
            $stateProvider.state('Refund.BillRefund', { url: '/BillRefund/:FeeNo', templateUrl: '/Views/FinancialManagement/BillRefund.html', controller: "billRefundCtrl" });
            $stateProvider.state('Refund.BillRefundRec', { url: '/BillRefundRec/:FeeNo', templateUrl: '/Views/FinancialManagement/BillRefundRec.html', controller: "billRefundRecCtrl" });
            $stateProvider.state('Refund.AdvanceChargeRefund', { url: '/AdvanceChargeRefund/:FeeNo', templateUrl: '/Views/FinancialManagement/AdvanceChargeRefund.html', controller: "advanceChargeRefundCtrl" });

            //结算单打印
            $stateProvider.state('PrintSettleAccount', { url: '/angular/PrintSettleAccount/:FeeNo', templateUrl: '/Views/FinancialManagement/PrintSettleAccount.html', controller: "printSettleAccountCtrl" });
            //费用录入 耗材&服务
            $stateProvider.state('costEntry', { url: '/angular/costEntry/:FeeNo', templateUrl: '/Views/ChargeInput/CostEntry.html', controller: 'costEntryCtrl' });

            //套餐费用录入
            $stateProvider.state('chargeGroupEntry', { url: '/angular/chargeGroupEntry/:FeeNo', templateUrl: '/Views/PackageRelated/ChargeGroupEntry.html', controller: 'chargeGroupEntryCtrl' });

            // 账单管理
            $stateProvider.state('DocOrder', { url: '/angular/DocOrder/:FeeNo', templateUrl: '/Views/BillManagement/DocOrder.html', controller: "docOrderCtrl" });
            $stateProvider.state('NurSendOrder', { url: '/angular/NurSendOrder/:FeeNo', templateUrl: '/Views/BillManagement/NurSendOrder.html', controller: "nurSendOrderCtrl" });
            $stateProvider.state('BillV2', { url: '/angular/BillV2/:FeeNo', templateUrl: '/Views/BillManagement/BillV2.html', controller: "BillV2Ctrl" });
            $stateProvider.state('FeeRecord', { url: '/angular/FeeRecord/:FeeNo', templateUrl: '/Views/BillManagement/FeeRecord.html', controller: "FeeRecordCtrl" });
            $stateProvider.state('BillV2Manger', { url: '/angular/BillV2Manger/:FeeNo', templateUrl: '/Views/BillManagement/BillV2Manger.html', controller: "BillV2MangerCtrl" });
            $stateProvider.state('PacMaintain', { url: '/angular/PacMaintain/:id', templateUrl: '/Views/PackageRelated/PacMaintain.html', controller: 'PacMaintainCtrl' });
            $stateProvider.state('chargeGroList', { url: '/angular/chargeGroList', templateUrl: '/Views/PackageRelated/chargeGroList.html', controller: 'chargeGroListCtrl' });
            $stateProvider.state('ResChargeGro', { url: '/angular/ResChargeGro', templateUrl: '/Views/PackageRelated/ResChargeGro.html', controller: 'ResChargeGroCtrl' });
            $stateProvider.state('MonFeeUpload', { url: '/angular/MonFeeUpload/:date', templateUrl: '/Views/PackageRelated/MonFeeUpload.html', controller: 'MonFeeUploadCtrl' });
            $stateProvider.state('RsMonFeeDtl', { url: '/angular/rsMonFeeDtl/:id/:rsMonFeeId/:YearMon', templateUrl: '/Views/BillManagement/RSMonFeeDtl.html', controller: "rsMonFeeDtlCtrl" });
            $stateProvider.state('LocalRsMonFeeDtl', { url: '/angular/LocalRsMonFeeDtl/:FeeNo/:YearMon', templateUrl: '/Views/PackageRelated/LocalRsMonFeeDtl.html', controller: "localRsMonFeeDtlCtrl" });
            $stateProvider.state('MonFeeUploadEdit', { url: '/angular/MonFeeUploadEdit/:NSMonFeeID', templateUrl: '/Views/PackageRelated/MonFeeUploadEdit.html', controller: 'MonFeeUploadEditCtrl' });
            $stateProvider.state('MonFeeList', { url: '/angular/MonFeeList', templateUrl: '/Views/PackageRelated/MonFeeList.html', controller: 'MonFeeListCtrl' });
            $stateProvider.state('PayGrant', { url: '/angular/PayGrant', templateUrl: '/Views/BillManagement/PayGrant.html', controller: "payGrantCtrl" });


            // 收费项目管理
            $stateProvider.state('ChargeItemDrug', { url: '/angular/ChargeItemSetting/DrugList', templateUrl: '/Views/ChargeItemSetting/DrugList.html', controller: "chargeItemNSDrugCtrl" });
            $stateProvider.state('ChargeItemDrugAdd', { url: '/angular/ChargeItemSetting/DrugEdit', templateUrl: '/Views/ChargeItemSetting/DrugEdit.html', controller: "chargeItemNSDrugEditCtrl" });
            $stateProvider.state('ChargeItemDrugEidt', { url: '/angular/ChargeItemSetting/DrugEdit/:id', templateUrl: '/Views/ChargeItemSetting/DrugEdit.html', controller: "chargeItemNSDrugEditCtrl" });
            $stateProvider.state('ChargeItemMaterial', { url: '/angular/ChargeItemSetting/MaterialList', templateUrl: '/Views/ChargeItemSetting/MaterialList.html', controller: "chargeItemNSMaterialCtrl" });
            $stateProvider.state('ChargeItemMaterialAdd', { url: '/angular/ChargeItemSetting/MaterialEdit', templateUrl: '/Views/ChargeItemSetting/MaterialEdit.html', controller: "chargeItemNSMaterialEditCtrl" });
            $stateProvider.state('ChargeItemMaterialEidt', { url: '/angular/ChargeItemSetting/MaterialEdit/:id', templateUrl: '/Views/ChargeItemSetting/MaterialEdit.html', controller: "chargeItemNSMaterialEditCtrl" });
            $stateProvider.state('ChargeItemService', { url: '/angular/ChargeItemSetting/ServiceList', templateUrl: '/Views/ChargeItemSetting/ServiceList.html', controller: "chargeItemNSServiceCtrl" });
            $stateProvider.state('ChargeItemServiceAdd', { url: '/angular/ChargeItemSetting/ServiceEdit', templateUrl: '/Views/ChargeItemSetting/ServiceEdit.html', controller: "chargeItemNSServiceEditCtrl" });
            $stateProvider.state('ChargeItemServiceEidt', { url: '/angular/ChargeItemSetting/ServiceEdit/:id', templateUrl: '/Views/ChargeItemSetting/ServiceEdit.html', controller: "chargeItemNSServiceEditCtrl" });

            //接待管理
            //前台控制台
            $stateProvider.state('FrontConsole', { url: '/angular/ReceptionManagement/FrontConsole', templateUrl: '/Views/ReceptionManagement/FrontConsole.html', controller: "frontConsoleCtrl" });
            //咨询登记
            $stateProvider.state('AdvisoryReg', { url: '/angular/ReceptionManagement/AdvisoryReg', templateUrl: '/Views/ReceptionManagement/AdvisoryReg.html', controller: "advisoryRegCtrl" });
            $stateProvider.state('AdvisoryRegAdd', { url: '/angular/AdvisoryRegEdit', templateUrl: '/Views/ReceptionManagement/AdvisoryRegEdit.html', controller: 'advisoryRegEditCtrl' });
            $stateProvider.state('AdvisoryRegEdit', { url: '/angular/AdvisoryRegEdit/:id', templateUrl: '/Views/ReceptionManagement/AdvisoryRegEdit.html', controller: 'advisoryRegEditCtrl' });
            $stateProvider.state('AdvisoryRegCallBack', { url: '/angular/ReceptionManagement/AdvisoryRegCallBack/:ConsultRecId', templateUrl: '/Views/ReceptionManagement/AdvisoryRegCallBack.html', controller: "advisoryRegCallBackCtrl" });
            $stateProvider.state('AdvisoryRegCallBackAdd', { url: '/angular/AdvisoryRegCallBackEdit/:ConsultRecId', templateUrl: '/Views/ReceptionManagement/AdvisoryRegCallBackEdit.html', controller: 'advisoryRegCallBackEditCtrl' });
            $stateProvider.state('AdvisoryRegCallBackEdit', { url: '/angular/AdvisoryRegCallBackEdit/:id/:ConsultRecId', templateUrl: '/Views/ReceptionManagement/AdvisoryRegCallBackEdit.html', controller: 'advisoryRegCallBackEditCtrl' });

            //房间床位管理
            $stateProvider.state('BedManagementList', { url: '/angular/BedManagementList', templateUrl: '/Views/OrganizationManage/BedManagementList.html', controller: 'bedManagementListCtrl' });
            $stateProvider.state('BedManagementEdit', { url: '/angular/BedManagementEdit/:id', templateUrl: '/Views/OrganizationManage/BedManagementEdit.html', controller: 'bedManagementEditCtrl' });

            $stateProvider.state('VisitorInOutRec', { url: '/angular/VisitorInOutRec', templateUrl: '/Views/ReceptionManagement/VisitorInOutRec.html', controller: 'visitorInOutListCtrl' });
            $stateProvider.state('ResBirthDayList', { url: '/angular/ResBirthDayList', templateUrl: '/Views/ReceptionManagement/ResBirthDayList.html', controller: 'resBirthDayListCtrl' });
            $stateProvider.state('LeaveHospList', { url: '/angular/LeaveHospList', templateUrl: '/Views/ReceptionManagement/LeaveHospList.html', controller: 'leaveHospListCtrl' });

            //长期护理保险评价
            $stateProvider.state('NursingEval', { url: '/angular/NursingEval', templateUrl: '/Views/NurseStation/NursingEval.html', controller: 'NursingEvalCtrl' });

            //报表管理
            $stateProvider.state('reportTempManage', { url: '/angular/reportTempManage', templateUrl: '/Views/Report/ReportTempManage.html', controller: 'reportTempManageCtrl' });

            //报表管理
            $stateProvider.state('FinancialClose', { url: '/angular/FinancialClose', templateUrl: '/Views/Resident/FinancialClose.html', controller: 'financialCloseCtrl' });

            //护理险三目查询
            //$stateProvider.state('QueryNCIDrug', { url: '/angular/NCIDrug/Query', templateUrl: '/Views/ChargeItem/DrugList.html', controller: "chargeItemNSDrugCtrl" });
            $locationProvider.html5Mode(true); 
        }
]);

