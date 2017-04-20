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
    public class NCIDrugService : BaseService, INCIDrugService
    {
        public BaseResponse<IList<NCIDrug>> Query(BaseRequest<NCIDrugFilter> request)
        {
            var response = base.Query<NCI_DRUG, NCIDrug>(request, (q) =>
            {
                if (request.Data.KeyWord != "")
                {
                    q = q.Where(m => m.CNNAME.Contains(request.Data.KeyWord) || m.ENNAME.Contains(request.Data.KeyWord) || m.DRUGCODE.Contains(request.Data.KeyWord) || m.PINYIN.Contains(request.Data.KeyWord));
                }
                q = q.Where(m => m.STATUS == 0);
                q = q.OrderByDescending(m => m.CREATETIME);
                return q;
            });

            return response;
        }

        public BaseResponse<NCIDrug> Get(string drugCode)
        {
            return base.Get<NCI_DRUG, NCIDrug>((q) => q.DRUGCODE == drugCode);
        }
    }
}
