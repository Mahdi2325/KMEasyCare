using AutoMapper;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Implement.Base;
using KMHC.SLTC.Business.Interface.SocialWorkerManage;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KMHC.SLTC.Business.Implement.SocialWorkerManage
{
    public class ResourceLinkService:BaseService,IResourceLinkService
    {
        public BaseResponse<ResourceLinkModel> SaveResourceLink(ResourceLinkModel request)
        {
            return base.Save<LTC_RESOURCELINKREC,ResourceLinkModel>(request,(q)=>q.ID==request.Id);
        }
        public BaseResponse<ResourceLinkModel> GetResourceLink(long regNo)
        {
            return base.Get<LTC_RESOURCELINKREC, ResourceLinkModel>((q) => q.REGNO == regNo);
        }
        public BaseResponse DeleteResourceLink(long id)
        {
            return base.Delete<LTC_RESOURCELINKREC>(id);
        }

        public BaseResponse<IList<ResourceLinkModel>> QueryResourceLink(BaseRequest<ResourceLinkFilter> request)
        {
            var response = base.Query<LTC_RESOURCELINKREC, ResourceLinkModel>(request, (q) => {
                q = q.OrderBy(m => m.ID);
                return q;
            });
            return response;
        }
    }
}
