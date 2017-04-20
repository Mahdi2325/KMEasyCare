using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface ICarePlansManageService : IBaseService
    {
        #region 照护计划制定
        BaseResponse<List<CodeValue>> GetCategory();
        BaseResponse<List<CodeValue>> GetLevelPRCategory(string category);
        BaseResponse<List<CodeValue>> GetDiaPR(string category, string levelPR);
        BaseResponse<List<string>> GetCP_Reason(int cp_no);
        BaseResponse<List<string>> GetCP_Data(int cp_no);
        BaseResponse<NSCPL> SaveNSCPL(NSCPL request);


        #endregion

        #region 照护目标
        BaseResponse<IList<NSCPLGOAL>> GetCareGoalList(long seqNo);
        BaseResponse<NSCPLGOAL> SaveGoal(NSCPLGOAL request);
        BaseResponse DeleteGoal(long id);
        BaseResponse<List<CodeValue>> GetPlanGoalsLP(int cp_no);
        BaseResponse<List<CodeValue>> GetPlanActivityLP(int cp_no);
        LTC_NSCPLActivity GetActivity(int id);
        #endregion

        #region 照护措施
        BaseResponse<IList<LTC_NSCPLActivity>> GetCareActivityList(long seqNo);
        BaseResponse<LTC_NSCPLActivity> SaveActivity(LTC_NSCPLActivity request);
        BaseResponse DeleteActivity(long id);
        #endregion

        #region 照护列表
        BaseResponse<IList<NSCPLView>> QueryCarePlanList(CarePlansFilter request);
        BaseResponse<NSCPL> GetCarePlan(long seqNo);
        #endregion
        #region 照护计划明细
        BaseResponse<IList<NSCPL>> QueryCarePlanDetail(BaseRequest<CarePlanDetailFilter> request);
        BaseResponse DeleteCarePlan(long id);
        object QueryAssTask(BaseRequest<AssignTaskFilterByBobDu> request);
        #endregion

        #region 照护评值
        BaseResponse<IList<ASSESSVALUE>> GetCareAssessList(long seqNo);
        BaseResponse<ASSESSVALUE> SaveAssess(ASSESSVALUE request);
        BaseResponse DeleteAssess(long id);
        BaseResponse<List<CodeValue>> GetPlanAssessLP(int cp_no);
        #endregion

        BaseResponse<object> GetCarePlanForExtApi(BaseRequest<CarePlanRecFilter> request);

    }
}

