using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC
{
    public interface IDC_ResidentManageService
    {
        #region ResidentList
        BaseResponse<IList<DC_Resident>> QueryDCResident(BaseRequest<DC_ResidentFilter> request);
        BaseResponse<DC_Resident> GetDCResident(string regNo);
        #endregion
        /// <summary>
        /// 查询个案药品信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DcRegMedicine>> QueryMedicine(BaseRequest<DcRegMedicine> request);

        /// <summary>
        /// 保存个案药品信息
        /// </summary>
        /// <param name="redMed"></param>
        /// <returns></returns>
        BaseResponse<DcRegMedicine> saveMedicine(BaseResponse<DcRegMedicine> redMed);

        /// <summary>
        /// 删除药品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteMedicine(long id);
        /// <summary>
        /// 批量保存个案药品信息
        /// </summary>
        /// <param name="redMed"></param>
        /// <returns></returns>
        BaseResponse<List<DcRegMedicine>> saveMedicineList(BaseResponse<List<DcRegMedicine>> redMed);
        /// <summary>
        /// 获取住民最近一笔需求评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DcNurseingPlanEval> QueryCurrentDcNurseingPlanEval(BaseRequest<DcNurseingPlanEvalFilter> request);

        /// <summary>
        /// 获取住民所有需求评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DcNurseingPlanEval>> QueryDcNurseingPlanEval(BaseRequest<DcNurseingPlanEvalFilter> request);


        /// <summary>
        /// 保存需求评估数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DcNurseingPlanEval> saveNurseingPlanEval(DcNurseingPlanEval request);


        /// <summary>
        /// 获取量表评估所有数据
        /// </summary>
        /// <param name="qId"></param>
        /// <param name="regNo"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        BaseResponse<QUESTION> GetQuetion(int qId, long? recordId);

        /// <summary>
        /// 根据Code 与ORGID获取ID 再获取评估数据
        /// </summary>
        /// <param name="qId"></param>
        /// <param name="regNo"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        BaseResponse<QUESTION> GetQuetionByCode(string Code, long? recordId);

        /// <summary>
        /// 保存量表数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse SaveQuetion(EVALQUESTION request);

        /// <summary>
        /// 获取量表评估主表数据
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        BaseResponse<EVALQUESTION> GetREGQuetion(long recordId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        BaseResponse<IList<EVALQUESTION>> GetREGQuetionList(long Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        string GetREGQuetionScore(List<int> l);
        /// <summary>
        /// 获取住民最近一笔个别化活动需求评估及计画数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegActivityRequestEval> QueryCurrentRegActivityRequestEval(BaseRequest<DC_RegActivityRequestEvalFilter> request);

        /// <summary>
        /// 获取住民个别化活动需求评估及计画部分数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegActivityRequestEval> QueryPartRegActivityRequestEval(BaseRequest<DC_RegActivityRequestEvalFilter> request);

        /// <summary>
        /// 获取住民所有个别化活动需求评估及计画数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegActivityRequestEval>> QueryRegActivityRequestEval(BaseRequest<DC_RegActivityRequestEvalFilter> request);


        /// <summary>
        /// 保存个别化活动需求评估及计画数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegActivityRequestEval> saveRegActivityRequestEval(DC_RegActivityRequestEval request);

        /// <summary>
        /// 获取住民个案护理诊断、问题数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegCpl>> QueryRegCpl(BaseRequest<DC_RegCplFilter> request);

        /// <summary>
        /// 查询最近一笔照顾计划是否执行成效
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        string QueryDCCplAction(long Id, long feeNo);

        /// <summary>
        /// 获取照顾计划需求问题
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_CarePlanProblem>> QueryCarePlanProblem(BaseRequest<DC_CarePlanProblemFilter> request);

        /// <summary>
        /// 获取照顾计划需求问题细项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_CarePlanDia>> QueryCarePlanDia(BaseRequest<DC_CarePlanDiaFilter> request);

        /// <summary>
        /// 获取照顾计划需求问题措施
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_CarePlanActivity>> QueryCarePlanActivity(BaseRequest<DC_CarePlanActivityFilter> request);

        /// <summary>
        /// 获取住民最近一笔照顾计画数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegCpl>> QueryCurrentRegCpl(BaseRequest<DC_RegCplFilter> request);

        /// <summary>
        /// 保存照顾计划数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegCpl> saveRegCpl(DC_RegCpl request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestList"></param>
        /// <returns></returns>
        BaseResponse<List<DC_RegCpl>> saveRegCplEval(List<DC_RegCpl> requestList);

        /// <summary>
        /// 获取个案基本整理所有资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegBaseInfoList>> QueryAllRegBaseInfoList(BaseRequest<DC_RegBaseInfoListFilter> request);

        /// <summary>
        /// 获取最近一笔个案基本资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegBaseInfoList> QueryRegBaseInfoList(BaseRequest<DC_RegBaseInfoListFilter> request);

        /// <summary>
        /// 获取个案基本资料住民带入信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegBaseInfoList> QueryRegBaseInfo(BaseRequest<DC_RegBaseInfoListFilter> request);

        /// <summary>
        /// 保存个案基本资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegBaseInfoList> saveRegBaseInfoList(DC_RegBaseInfoList request);


        /// <summary>
        /// 获取问题层面数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CAREPLANPROBLEM>> QueryLevelPr(string categoryType);
        /// <summary>
        /// 获取问题
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CAREPLANPROBLEM>> QueryDiaPr(string levelPr, string categoryType);
        /// <summary>
        /// 获取问题导因数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CAREPLANREASON>> QueryCausep(int? cpNo);
        /// <summary>
        ///  获取问题特征数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CAREPLANDATA>> QueryPrData(int? cpNo);
        /// <summary>
        /// 获取目标数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CAREPLANGOAL>> QueryGoalp(string dirPr);
        /// <summary>
        /// 获取措施数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CAREPLANACTIVITY>> QueryActivity(string dirPr);
        /// <summary>
        /// 获取评值数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CAREPLANEVAL>> QueryAssessvalue(string dirPr);

        /// <summary>
        /// 查询护理目标List数据
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL>> QueryNsCplGoalp(long seqNo);

        /// <summary>
        /// 保存护理目标数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL> saveNsCplGoalp(KMHC.SLTC.Business.Entity.DC.Model.NSCPLGOAL request);

        /// <summary>
        /// 查询护理措施List数据
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        BaseResponse<IList<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY>> QueryNsCplActivity(long seqNo);

        /// <summary>
        /// 保存护理措施数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY> saveNsCplActivity(KMHC.SLTC.Business.Entity.DC.Model.NSCPLACTIVITY request);

        /// <summary>
        /// 获取护理评值数据
        /// </summary>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE> QueryAssessValue(long seqNo);

        /// <summary>
        /// 保存护理评值数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE> saveAssessValue(KMHC.SLTC.Business.Entity.DC.Model.ASSESSVALUE request);
    }
}

