/*
创建人: 肖国栋
创建日期:2016-03-09
说明:住民管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System;

namespace KMHC.SLTC.Business.Interface
{
    public interface IResidentManageService
    {
        /// <summary>
        /// 获取住民信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Person>> QueryPerson(BaseRequest<PersonFilter> request);
        /// <summary>
        /// 获取住民扩展信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Person>> QueryPersonExtend(BaseRequest<PersonFilter> request);
        BaseResponse<IList<Person>> QueryPersonExtend2(BaseRequest<PersonExtendFilter> request);
        /// <summary>
        /// 获取住民信息
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<Person> GetPerson(int regNo);
        BaseResponse<PersonPreview> GetPersonExtend(int regNo);
        /// <summary>
        /// 获取疾病史信息
        /// </summary>
        /// <param name="sNo"></param>
        /// <returns></returns>
        BaseResponse<IList<ScenarioMain>> GetScenario(int sNo);
        /// <summary>
        /// 获取病人疾病史数据
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<Regdiseasehis> QueryRegdiseasehis(int regNo);
        /// <summary>
        /// 保存住民信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Person> SavePerson(Person request);
        /// <summary>
        /// 删除住民信息
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse DeletePerson(int regNo);
        /// <summary>
        /// 获取入住信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Resident>> QueryResident(BaseRequest<ResidentFilter> request);
        /// <summary>
        /// 获取入住扩展信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Resident>> QueryResidentExtend(BaseRequest<ResidentFilter> request);

        BaseResponse<Resident> QueryLocaResInfo(string idNo);
        /// <summary>
        /// 根据住民姓名获得同名住民列表（姓名完全匹配）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Resident>> QueryResidentByName(BaseRequest<ResidentFilter> request);
        /// <summary>
        /// 根据住民楼层或房间号住民列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Resident>> QueryResidentByFloorAndRoom(BaseRequest<ResidentFilter> request);        
        /// <summary>
        /// 获取入住信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Resident> GetResident(long feeNo);

        BaseResponse<object> GetResidentsForExtApiByIdNoList(List<string> idNoList);
        /// <summary>
        /// 保存入住信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Resident> SaveResident(Resident request);
        /// <summary>
        /// 删除入住信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteResident(long feeNo);
        /// <summary>
        /// 获取入住审核信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Verify>> QueryVerify(BaseRequest<VerifyFilter> request);
        /// <summary>
        /// 获取出入住审核信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Verify> GetVerify(long feeNo);
        /// <summary>
        /// 保存入住审核信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Verify> SaveVerify(Verify request);
        /// <summary>
        /// 删除入住审核信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteVerify(long feeNo);
        /// <summary>
        /// 获取请假记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LeaveHosp>> QueryLeaveHosp(BaseRequest<LeaveHospFilter> request);

        BaseResponse<IList<LeaveHosp>> QueryLeaveHospList(DateTime sDate, DateTime eDate, string keyWord, int levStatus, int CurrentPage, int PageSize);


        BaseResponse<IList<RegInHosStatusListEntity>> QueryRegInHosStatusList();


        /// <summary>
        /// 查询住民最新一笔请假记录
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>离院记录</returns>
        BaseResponse<IList<LeaveHosp>> GetNewLeaveHosp(BaseRequest<LeaveHospFilter> request);
        /// <summary>
        /// 获取请假记录信息
        /// </summary>
        /// <param name="leaveHospId"></param>
        /// <returns></returns>
        BaseResponse<LeaveHosp> GetLeaveHosp(long leaveHospId);
        /// <summary>
        /// 保存请假记录信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<LeaveHosp> SaveLeaveHosp(LeaveHosp request);
        /// <summary>
        /// 删除请假记录信息
        /// </summary>
        /// <param name="leaveHospId"></param>
        BaseResponse DeleteLeaveHosp(long leaveHospId);
        /// <summary>
        /// 获取零用金列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Deposit>> QueryDeposit(BaseRequest<DepositFilter> request);
        /// <summary>
        /// 获取零用金信息
        /// </summary>
        /// <param name="deptNo"></param>
        /// <returns></returns>
        BaseResponse<Deposit> GetDeposit(string deptNo);
        /// <summary>
        /// 保存零用金信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Deposit> SaveDeposit(Deposit request);
        /// <summary>
        /// 删除零用金信息
        /// </summary>
        /// <param name="deptNo"></param>
        BaseResponse DeleteDeposit(string deptNo);
        /// <summary>
        /// 获取出院结案信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CloseCase>> QueryCloseCase(BaseRequest<CloseCaseFilter> request);
        /// <summary>
        /// 获取出院结案信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<CloseCase> GetCloseCase(long feeNo);
        /// <summary>
        /// 保存出院结案信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CloseCase> SaveCloseCase(CloseCase request);
        /// <summary>
        /// 删除出院结案信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteCloseCase(long feeNo);
        /// <summary>
        /// 获取社会福利信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ResidentDtl>> QueryResidentDtl(BaseRequest<ResidentDtlFilter> request);
        /// <summary>
        /// 获取社会福利信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<ResidentDtl> GetResidentDtl(long feeNo);
        /// <summary>
        /// 保存社会福利信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<ResidentDtl> SaveResidentDtl(ResidentDtl request);
        /// <summary>
        /// 删除社会福利信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteResidentDtl(long feeNo);

        BaseResponse UpdateYearCert(AuditYearCertModel baseRequest);
        /// <summary>
        /// 获取需求管理信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Demand>> QueryDemand(BaseRequest<DemandFilter> request);
        /// <summary>
        /// 获取需求管理信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Demand> GetDemand(long id);
        /// <summary>
        /// 保存需求管理信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Demand> SaveDemand(Demand request);
        /// <summary>
        /// 删除需求管理信息
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteDemand(long id);
        /// <summary>
        /// 获取健康管理信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Health>> QueryHealth(BaseRequest<HealthFilter> request);
        /// <summary>
        /// 获取	健康管理信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Health> GetHealth(long feeNo);
        /// <summary>
        /// 保存健康管理信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Health> SaveHealth(Health request);
        /// <summary>
        /// 删除健康管理信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteHealth(long feeNo);
        /// <summary>
        /// 获取	附加文件信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<AttachFile>> QueryAttachFile(BaseRequest<AttachFileFilter> request);
        /// <summary>
        /// 获取附加文件信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<AttachFile> GetAttachFile(long feeNo);
        /// <summary>
        /// 保存附加文件信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<AttachFile> SaveAttachFile(AttachFile request);
        /// <summary>
        /// 批量保存附加文件信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="request"></param>
        BaseResponse<List<AttachFile>> SaveAttachFile(long feeNo, List<AttachFile> request);
        /// <summary>
        /// 删除附加文件信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteAttachFile(long Id);
        /// <summary>
        /// 获取通信录住民地址信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Relation>> QueryRelation(BaseRequest<RelationFilter> request);
        /// <summary>
        /// 获取通信录住民地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Relation> GetRelation(long feeNo);
        /// <summary>
        /// 保存通信录住民地址信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Relation> SaveRelation(Relation request);
        /// <summary>
        /// 删除通信录住民地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteRelation(long feeNo);
        /// <summary>
        /// 获取通信录亲属地址信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<RelationDtl>> QueryRelationDtl(BaseRequest<RelationDtlFilter> request);
        /// <summary>
        /// 获取通信录亲属地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<RelationDtl> GetRelationDtl(long feeNo);
        /// <summary>
        /// 保存通信录亲属地址信息
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<RelationDtl> SaveRelationDtl(RelationDtl request);
        /// <summary>
        /// 批量保存通信录亲属地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="request"></param>
        BaseResponse<List<RelationDtl>> SaveRelationDtl(long feeNo, List<RelationDtl> request);
        /// <summary>
        /// 删除通信录亲属地址信息
        /// </summary>
        /// <param name="feeNo"></param>
        BaseResponse DeleteRelationDtl(long feeNo);

        /// <summary>
        /// 是否住民已登记过
        /// </summary>
        /// <param name="regNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool ExistResident(long regNo,string[] status);

        /// <summary>
        /// 获取住民访视列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscuss(BaseRequest<FamilyDiscussFilter> request);
        /// <summary>
        /// 获取住民访视列表拓展
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<FamilyDiscuss>> QueryFamilyDiscussExtend(BaseRequest<FamilyDiscussFilter> request);
        /// <summary>
        /// 获取营养筛查
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTCNUTRTION72EVAL>> QueryNutrtionEvalExtend(BaseRequest<NutrtionEvalFilter> request);
 
        /// <summary>
        /// 保存营养筛查
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTCNUTRTION72EVAL> SaveNutrtionEval(LTCNUTRTION72EVAL request);
        /// <summary>
        /// 取某条营养筛查记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        BaseResponse<LTCNUTRTION72EVAL> GetNutrtionEval(int Id);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        BaseResponse DeleteNutrtionEval(int Id); 
        /// <summary>
        /// 获取住民访视
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<FamilyDiscuss> GetFamilyDiscuss(int Id);
        /// <summary>
        /// 保存住民访视
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<FamilyDiscuss> SaveFamilyDiscuss(FamilyDiscuss request);
        /// <summary>
        /// 删除住民访视
        /// </summary>
        /// <param name="regNo"></param>
        BaseResponse DeleteFamilyDiscuss(int Id);
        /// <summary>
        /// 获取预约登记信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Preipd>> QueryPreipd(BaseRequest<PreipdFilter> request);
        /// <summary>
        /// 保存预约登记信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<Preipd> SavePreipd(Preipd request);
        /// <summary>
        /// 删除预约登记信息
        /// </summary>
        /// <param name="PreFeeNo"></param>
        /// <returns></returns>
        BaseResponse DeletePreipd(long PreFeeNo);
        /// <summary>
        /// 获取出院办理信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Ipdregout> GetIpdregout(long feeNo);
        /// <summary>
        /// 保存出院办理信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<Ipdregout> SaveIpdregout(Ipdregout request);

        /// <summary>
        /// 查询护理险账单相关信息
        /// </summary>
        /// <param name="feeNo">员工编号</param>
        /// <param name="ipdoutTime">结案时间</param>
        /// <returns>账单数据信息</returns>
        BaseResponse QueryIpdBillInfo(int feeNo, DateTime? ipdoutTime);
        /// <summary>
        /// 获取住民退住院信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<Resident> GetLeaveNursing(long feeNo);
        /// <summary>
        /// 获取住民退住院床位信息
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<BedBasic> GetLeaveNursingBedInfo(long feeNo);
        /// <summary>
        /// 保存住民退住院信息
        /// </summary>
        /// <param name="resident"></param>
        /// <returns></returns>
        BaseResponse<Resident> SaveLeaveNursing(Resident resident);
        /// <summary>
        /// 获取Post数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ZipFile>> QueryPost(BaseRequest<ZipFileFilter> request);
        /// <summary>
        /// 获取住民在院证明数据
        /// </summary>
        /// <param name="feeNo"></param>
        /// <returns></returns>
        BaseResponse<RegHosProof> QueryRegHosProof(long feeNo);

        /// <summary>
        /// 关帐作业处理流程
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="financialCloseTime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        BaseResponse CloseOrCnacleBill(long feeNo, DateTime financialCloseTime, string type);
        BaseResponse SaveIpdregInfo(string type, DateTime financialCloseTime, long feeNo);
    }
}
