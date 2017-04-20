using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
namespace KMHC.SLTC.Business.Interface
{
    public interface IReceptionManagementService : IBaseService
    {
        object GetFrontConsole(DateTime startTime, DateTime endTime);
        BaseResponse<IList<ConsultRec>> QueryAdvisoryReg(BaseRequest<AdvisoryRegFilter> request);
        BaseResponse<ConsultRec> SaveConsultRec(ConsultRec request);
        BaseResponse DeleteConsultRec(long id);
        BaseResponse<ConsultRec> GetConsultRec(long id);
        BaseResponse<IList<ConsultCallBack>> QueryConsultCallBack(BaseRequest<ConsultCallBackFilter> request);
        BaseResponse<ConsultCallBack> SaveConsultCallBack(ConsultCallBack request);
        BaseResponse DeleteConsultCallBack(long id);
        BaseResponse<ConsultCallBack> GetConsultCallBack(long id);
    }
}
