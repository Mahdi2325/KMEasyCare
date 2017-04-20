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
    public interface IDeductionService : IBaseService
    {
        BaseResponse<IList<NCIDeductionModel>> QueryDeductionList(BaseRequest<DeductionFilter> request);
        BaseResponse SaveDeduction(NCIDeductionModel request);
        object GetDeductionList(BaseRequest request,string month);
    }
}
