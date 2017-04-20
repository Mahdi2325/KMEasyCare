using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/CommWord")]
    public class CommWordController : BaseController
    {
        ICommfileService codeService = IOCContainer.Instance.Resolve<ICommfileService>();

        [Route(""), HttpPost]
        public IHttpActionResult Query(CommonUseWordFilter request)
        {
       
            BaseResponse<Dictionary<string, List<DC_CommUseWord>>> response = new BaseResponse<Dictionary<string, List<DC_CommUseWord>>>();
            if (request != null)
            {
                if (request.TypeName != null)
                {
                    var typeNames = request.TypeName.Split(',');
                    if (typeNames.Length > 1)
                    {
                        request.TypeName = string.Empty;
                        request.TypeNames = typeNames;
                    }
                }
                var commonUseWordList = codeService.QueryCommonFile(request);
                response.Data = request.TypeNames.ToDictionary(typeName => typeName, no => commonUseWordList.Data.Where(o => o.ItemType == no).ToList());
            }
            else
            {
                response.Data = new Dictionary<string, List<DC_CommUseWord>>();
            }
            return Ok(response);
        }
    }
}
