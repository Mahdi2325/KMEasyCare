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
    public class NCIMedicalMaterialService : BaseService, INCIMedicalMaterialService
    {
        public BaseResponse<IList<NCIMedicalMaterial>> Query(BaseRequest<NCIMedicalMaterialFilter> request)
        {
            var response = base.Query<NCI_MEDICALMATERIAL, NCIMedicalMaterial>(request, (q) =>
            {
                if (!string.IsNullOrWhiteSpace(request.Data.KeyWord))
                {
                    q = q.Where(m => m.MATERIALNAME.ToUpper().Contains(request.Data.KeyWord.ToUpper()) || m.MATERIALCODE.ToUpper().Contains(request.Data.KeyWord.ToUpper()) || m.PINYIN.ToUpper().Contains(request.Data.KeyWord.ToUpper()));
                }
                q = q.Where(m => m.STATUS == 0);
                q = q.OrderByDescending(m => m.CREATETIME);
                return q;
            });
            return response;
        }

        public BaseResponse<NCIMedicalMaterial> Get(string medicalMaterialCode)
        {
            return base.Get<NCI_MEDICALMATERIAL, NCIMedicalMaterial>((q) => q.MATERIALCODE == medicalMaterialCode);
        }
    }
}
