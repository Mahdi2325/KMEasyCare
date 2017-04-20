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
    public interface IFeeRecordService : IBaseService
    {
        BaseResponse<IList<FeeRecordBaseInfo>> QueryNotGenerateBillRecord(BaseRequest<FeeRecordFilter> request);

        BaseResponse<List<FeeRecordBaseInfo>> SaveFeeRecord(BillV2Info request);
    }
}
