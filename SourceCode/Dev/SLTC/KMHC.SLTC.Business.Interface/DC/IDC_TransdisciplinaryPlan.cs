using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC
{
    public interface IDC_TransdisciplinaryPlan
    {
        BaseResponse<DC_MultiteamCarePlanRecModel> QueryMultiCarePlanRec(long seqNo);
        BaseResponse<DC_MultiteamCarePlanRecModel> QueryLatestMultiCarePlanRec(long feeNo);
        BaseResponse<DC_MultiteamCarePlanRecModel> QueryTransdisciplinaryRef(long feeNo);
        BaseResponse<DC_MultiteamCarePlanRecModel> SaveTransdisciplinary(DC_MultiteamCarePlanRecModel carePlanRec);
        BaseResponse<List<DC_MultiteaMcarePlanEvalModel>> QueryTransdisciplinaryHistory(long feeNo);
        BaseResponse DeleteTransdisciplinaryHis(long seqNo);
        #region Timeline
        BaseResponse<CasesTimeline> QueryCasesTimeline(long feeNo, string startDate, string endDate,int tag);
        #endregion
    }
}
