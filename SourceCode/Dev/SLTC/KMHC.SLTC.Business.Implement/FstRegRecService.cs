using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Implement
{
    public class FstRegRecService : BaseService, IFstRegRecService
    {
        public BaseResponse<FstRegRec> SaveFstRegRec(FstRegRec request)
        {
            BaseResponse<FstRegRec> response = new BaseResponse<FstRegRec>();
            try
            {
                request.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.UpdateTime = DateTime.Now;
                return base.Save<LTC_FSTREGREC, FstRegRec>(request, (q) => q.REGNO == request.RegNo);
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = "保存失败，请联系管理员！";
                return response;
                throw ex;
            };
        }

        BaseResponse<FstRegRec> IFstRegRecService.GetFstRegRec(int regNo)
        {
            return base.Get<LTC_FSTREGREC, FstRegRec>((q) => q.REGNO == regNo && q.ISDELETE == false);
        }
    }
}
