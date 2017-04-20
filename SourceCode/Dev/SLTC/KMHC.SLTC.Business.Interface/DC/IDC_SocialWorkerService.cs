using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KMHC.SLTC.Business.Interface
{
    public interface IDC_SocialWorkerService
    {
        #region***********************日照部分--个案基本资料*********************
        /// <summary>
        /// 获取个案基本资料列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegFileModel>> QueryPersonBasic(BaseRequest<DC_RegFileFilter> request);

        /// <summary>
        /// 获取个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<DC_RegFileModel> GetPersonBasicById(string id);

        /// <summary>
        /// 保存个案基本资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegFileModel> SavePersonBasic(DC_RegFileModel request);

        /// <summary>
        /// 删除个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse DeletePersonBasicById(string id);
        #endregion#

        #region***************************日照部分--个案转介*********************
        /// <summary>
        /// 获取个案转介列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_ReferrallistsModel>> QueryReferrallist(BaseRequest<DC_ReferrallistsFilter> request);

        /// <summary>
        /// 获取个案转介
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<DC_ReferrallistsModel> GetReferralById(int id);
        /// <summary>
        /// 保存个案转介
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_ReferrallistsModel> SaveReferral(DC_ReferrallistsModel request);
        /// <summary>
        /// 删除个案转介
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse DeleteReferralById(int id);
        #endregion#

        #region***************************日照部分--1天生活**********************

        /// <summary>
        /// 获取个案1天生活记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegDayLifeModel>> QueryDayLife(BaseRequest<DC_RegDayLifeFilter> request);

        /// <summary>
        /// 获取个案单笔一天生活
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<DC_RegDayLifeModel> GetDayLifeById(int feeNo);
        /// <summary>
        /// 保存个案1天生活
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegDayLifeModel> SaveDayLife(DC_RegDayLifeModel request);
        /// <summary>
        /// 删除个案1天生活
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse DeleteDayLifeById(int feeNo);

        #endregion

        #region**************************日照部分--个案生活史********************

        /// <summary>
        /// 获取个案生活史记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_LifeHistoryModel>> QueryLifeHistory(BaseRequest<DC_LifeHistoryFilter> request);

        /// <summary>
        /// 获取个案单笔生活史
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<DC_LifeHistoryModel> GetLifeHistoryById(int feeNo);
        /// <summary>
        /// 保存个案生活史
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_LifeHistoryModel> SaveLifeHistory(DC_LifeHistoryModel request);
        /// <summary>
        /// 删除个案生活史
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse DeleteLifeHistoryById(int feeNo);

        #endregion

        #region****************************日照部分--结案表**********************

        /// <summary>
        /// 获取个案结案列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_IpdRegModel>> QueryIpdRegOut(BaseRequest<DC_IpdRegFilter> request);

        /// <summary>
        /// 获取结案单笔记录
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse<DC_IpdRegModel> GetIpdRegOutById(int feeNo);
		/// <summary>
		/// 校验身份证
		/// </summary>
		/// <param name="idNo"></param>
		/// <returns></returns>
		BaseResponse<DC_RegFileModel> GetIpdInfo(string idNo);
        /// <summary>
        /// 保存结案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_IpdRegModel> SaveIpdRegOut(DC_IpdRegModel request);

        BaseResponse<DC_IpdRegModel> SaveIpdRegIn(DC_IpdRegModel request);

        BaseResponse<DC_IpdRegModel> SaveUpdateIpdRegIn(DC_IpdRegModel request);
        /// <summary>
        /// 删除个案结案
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        BaseResponse DeleteIpdRegOutById(int feeNo);

        #endregion

        #region*******************日照部分--社工个案评估及处遇计划表*************

        /// <summary>
        /// 获取社工个案评估及处遇计划列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_SwRegEvalPlanModel>> QuerySwRegEvalPlan(BaseRequest<DC_SwRegEvalPlanFilter> request);

        BaseResponse<EvalPlan> QuerySwRegEvalPlan(int evalPlanId, int feeNo);


        /// <summary>
        /// 获取社工个案评估及处遇计划单笔记录
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        BaseResponse<DC_SwRegEvalPlanModel> GetSwRegEvalPlanById(int evalPlanId);
        /// <summary>
        /// 保存社工个案评估及处遇计划
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_SwRegEvalPlanModel> SaveSwRegEvalPlan(DC_SwRegEvalPlanModel request);


        BaseResponse<IList<DC_TaskGoalsStrategyModel>> GetTaskGoalyssTrategyById(int evalplanId, int feeNo);

        /// <summary>
        /// 保存社工个案计划评值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_TaskGoalsStrategyModel> SaveTaskGoalssTrategy(DC_TaskGoalsStrategyModel request);
 
        /// <summary>
        /// 删除社工个案评估及处遇计划
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        BaseResponse DeleteSwRegEvalPlanById(int evalPlanId);
		/// <summary>
		/// 删除社工填写的计划
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		BaseResponse DeleteTaskGoalyssTrategyById(int id);

        BaseResponse<DC_EvalQeustionModel> GetEvalQuestionVal(int feeNo, int number, string type);

        BaseResponse<IList<DC_TaskGoalsStrategyModel>> CheckAddRec(int feeNo, int number);

        int? GetMaxNumber(long? feeNo, string orgId);

        #endregion

        #region*****************日照部分--家庭照顾者生活品质评估问卷*************

        /// <summary>
        /// 获取家庭照顾者生活品质评估问卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_RegLifeQualityEvalModel>> QueryRegLifeQualityEval(BaseRequest<DC_RegLifeQualityEvalFilter> request);

        /// <summary>
        /// 获取家庭照顾者生活品质评估问卷单笔记录
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        BaseResponse<DC_RegLifeQualityEvalModel> GetRegLifeQualityEvalById(int id);
        /// <summary>
        /// 保存家庭照顾者生活品质评估问卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DC_RegLifeQualityEvalModel> SaveRegLifeQualityEval(DC_RegLifeQualityEvalModel request);


        /// <summary>
        /// 删除家庭照顾者生活品质评估问卷
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        BaseResponse DeleteRegLifeQualityEvalById(int id);

        #endregion

        #region*******************日照部分--受托长辈适应程度评估表***************

        /// <summary>
        /// 获取所有问题列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //BaseResponse<IList<DC_QuestionModel>> QueryQuestion(BaseRequest<DC_QuestionFilter> request);

        BaseResponse<IList<DC_QuestionModel>> QueryQuestion(string orgId);

        BaseResponse<IList<DC_QuestionModel>> QueryQuestion(int feeNo, string orgId);
        BaseResponse<IList<DC_QuestionModel>> QueryQuestion(int feeNo, string orgId, int? evalRecId);

        BaseResponse<IList<DC_QuestionValueModel>> QueryQuestionValue(BaseRequest<DC_QuestionValueFilter> request);
        //历史记录
        BaseResponse<IList<DC_RegQuestionEvalRecModel>> QueryRegQuestionHistory(int feeNo);

        /// <summary>
        /// 社工个案评估历史记录
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        BaseResponse<IList<DC_SwRegEvalPlanModel>> QueryRegEvalHistory(int feeNo, int pageIndex, int pageSize);
        /// <summary>
        /// 获取指定questionId问题
        /// </summary>
        /// <param name="evalPlanId"></param>
        /// <returns></returns>
        BaseResponse<DC_QuestionModel> GetQuestionById(int questionId);
        BaseResponse<DC_RegQuestionEvalRecModel> SaveRegQuestionItem(List<DC_QuestionModel> request);
        #endregion
    }
}

