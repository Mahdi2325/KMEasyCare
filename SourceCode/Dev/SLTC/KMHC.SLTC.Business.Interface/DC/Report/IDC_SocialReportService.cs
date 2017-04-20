using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC.Report
{
    public interface IDC_SocialReportService
    {
        #region 个案生活史
            BaseResponse<DC_LifeHistoryModel> GetLifeHistoryById(int id);
        #endregion

        #region 个案转介
            BaseResponse<DC_ReferrallistsModel> GetReferralById(int id);
        #endregion

        #region 受托长辈适应程度评估表
            BaseResponse<List<DC_RegQuestionDataModel>> GetRegQuestionEvalRec(int id,int qId);
        #endregion
       
        #region 收案
        BaseResponse<DC_IpdRegModel> GetIpdRegInById(int id);
        #endregion

        #region 一天生活
        BaseResponse<DC_RegDayLifeModel> GetOneDayLifeById(int id);
        #endregion

        #region 基本资料
        BaseResponse<DC_RegFileModel> GetBasicInfoById(long id);
        #endregion

        #region 社工个案评估与处遇计划表
        //BaseResponse<DC_SwRegEvalPlanModel> GetSwRegEvalPlan(int evalPlanId, int feeNo);
        #endregion
    }
}

