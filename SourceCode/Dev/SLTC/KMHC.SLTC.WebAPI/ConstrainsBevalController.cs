/*
 * 描述:ConstrainsBevalController
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/28/2016 2:29:09 PM    张正泉            15986707042@163.com    创建 
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
    [RoutePrefix("api/restrictdetails")]
    public class ConstrainsBevalController:BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(long seqNo = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<ConstrainsBevalFilter> request = new BaseRequest<ConstrainsBevalFilter>()
            {
                Data = new ConstrainsBevalFilter()
                {
                    SeqNo = seqNo
                },
                CurrentPage = currentPage,
                PageSize = pageSize
            };
            var response = service.QueryConstrainsBeval(request);
            return Ok(response);
        }

        // GET
        [Route("{FeeNo}"), HttpGet]
        public IHttpActionResult Get(long feeNo)
        {
            var response = service.GetConstrainsBevalExtend(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(ConstrainsBeval request)
        {
            //request.EvaluateBy = SecurityHelper.CurrentPrincipal.LoginName;
            //request.EvalDate = DateTime.Now;
            var response = service.SaveConstrainsBeval(request);
            return Ok(response);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteConstrainsBeval(id);
            return Ok(response);
        }
    }
}
