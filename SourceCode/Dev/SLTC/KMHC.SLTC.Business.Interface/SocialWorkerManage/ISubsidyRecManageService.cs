using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;
namespace KMHC.SLTC.Business.Interface.SocialWorkerManage
{
    public interface ISubsidyRecManageService : IBaseService
    {
        BaseResponse<IList<SubsidyView>> Query(BaseRequest<SubsidyFilter> request);

        BaseResponse<SubsidyView> Get(long id);

        BaseResponse<SubsidyView> Save(SubsidyView request);

        BaseResponse Delete(long id);
    }
}
