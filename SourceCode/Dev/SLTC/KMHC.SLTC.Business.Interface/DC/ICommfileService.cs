using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC
{
     public interface ICommfileService
    {

         [Caching(CachingMethod.Get)]
         BaseResponse<IList<DC_CommUseWord>> QueryCommonFile(CommonUseWordFilter request);
    }
}
