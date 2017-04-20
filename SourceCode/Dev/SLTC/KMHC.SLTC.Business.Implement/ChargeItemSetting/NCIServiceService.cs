using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class NCIServiceService : BaseService, INCIServiceService
    {
        public BaseResponse<IList<NCIService>> Query(BaseRequest<NCIServiceFilter> request)
        {
            var response = base.Query<NCI_SERVICE, NCIService>(request, (q) =>
            {
                if (!string.IsNullOrWhiteSpace(request.Data.KeyWord))
                {
                    q = q.Where(m => m.SERVICENAME.ToUpper().Contains(request.Data.KeyWord.ToUpper()) || m.SERVICECODE.ToUpper().Contains(request.Data.KeyWord.ToUpper()) || m.PINYIN.ToUpper().Contains(request.Data.KeyWord.ToUpper()));
                }
                q = q.Where(m => m.STATUS == 0);
                q = q.OrderByDescending(m => m.CREATETIME);
                return q;
            });
            return response;
        }

        public BaseResponse<NCIService> Get(string serviceCode)
        {
            return base.Get<NCI_SERVICE, NCIService>((q) => q.SERVICECODE == serviceCode);
        }
    }
}
