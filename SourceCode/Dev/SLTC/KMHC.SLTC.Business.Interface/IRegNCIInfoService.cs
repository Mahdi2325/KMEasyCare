using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
   public interface IRegNCIInfoService :IBaseService
    {
       BaseResponse SaveRegNCIInfo(RegNCIInfo baseRequest);

       BaseResponse<RegNCIInfo> GetLTCRegInfo(int feeNo);

       BaseResponse UpdateRegInfo(CareInsInfo baserequest);
    }
}
