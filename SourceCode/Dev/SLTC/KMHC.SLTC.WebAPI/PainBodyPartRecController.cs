/*
 * 描述:PainBodyPartController
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/26/2016 7:31:15 PM    张正泉            15986707042@163.com    创建 
 *  
 */
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/paindetails")]
    public class PainBodyPartRecController:BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(long seqNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<PainBodyPartRecFilter> request = new BaseRequest<PainBodyPartRecFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = new PainBodyPartRecFilter()
                {
                    SeqNo = seqNo
                }
            };
            var response = service.QueryPainBodyPartRec(request);
            return Ok(response);
        }

        // GET
        [Route("{FeeNo}"), HttpGet]
        public IHttpActionResult Get(long feeNo)
        {
            var response = service.GetPainBodyPartRecExtend(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(PainBodyPartRec request)
        {
            request.CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            request.CreateDate = DateTime.Now;
            var response = service.SavePainBodyPartRec(request);
            return Ok(response);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeletePainBodyPartRec(id);
            return Ok(response);
        }
    }
}
