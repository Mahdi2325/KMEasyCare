using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/Code")]
    public class CodeController : BaseController
    {
        IDictManageService codeService = IOCContainer.Instance.Resolve<IDictManageService>();

        [Route(""), HttpPost]
        public IHttpActionResult Query(CodeFilter request)
        {
            BaseResponse<Dictionary<string, List<CodeValue>>> response = new BaseResponse<Dictionary<string, List<CodeValue>>>();
            if (request != null)
            {
                if (request.ItemType != null)
                {
                    var itemTypes = request.ItemType.Split(',');
                    if (itemTypes.Length > 1)
                    {
                        request.ItemType = string.Empty;
                        request.ItemTypes = itemTypes;
                    }
                }
                var codeList = codeService.QueryCode(request);
                response.Data = request.ItemTypes.ToDictionary(itemType => itemType, no => codeList.Data.Where(o => o.ItemType == no).ToList());
            }
            else
            {
                response.Data = new Dictionary<string, List<CodeValue>>();
            }
            return Ok(response);
        }
    }
}
