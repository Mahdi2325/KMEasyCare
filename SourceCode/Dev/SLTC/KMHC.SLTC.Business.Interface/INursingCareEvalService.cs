using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface INursingCareEvalService : IBaseService
    {
        BaseResponse<ResBasicInfo> GetNursingCareEvalQues(string itemType, int feeNo, int evaluateid);

        object SaveResBasicInfo(ResBasicInfo baserequest);

        object QueryCareEvalInfo(BaseRequest<NursingFilter> request);
        BaseResponse DeleteEvalCare(long id);
    }
}