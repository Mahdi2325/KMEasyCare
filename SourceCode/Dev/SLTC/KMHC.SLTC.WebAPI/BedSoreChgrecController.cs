/*
 * 描述:BedSoreChgrecController
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/26/2016 3:36:20 PM    张正泉            15986707042@163.com    创建 
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
    [RoutePrefix("api/sorechg")]
    public class BedSoreChgrecController:BaseController
    {
        IIndexManageService service = IOCContainer.Instance.Resolve<IIndexManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(long seq = 0, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<BedSoreChgrecFilter> request = new BaseRequest<BedSoreChgrecFilter>()
            {
                Data = new BedSoreChgrecFilter()
                {
                    Seq = seq
                }
            };
            var response = service.QueryBedSoreChgrec(request);
            return Ok(response);
        }

        // GET
        [Route("{seq}"), HttpGet]
        public IHttpActionResult Get(long seq)
        {
            var response = service.GetBedSoreChgrecExtend(seq);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(BedSoreChgrec request)
        {
            request.CreateBy = SecurityHelper.CurrentPrincipal.LoginName;
            request.CreateDate = DateTime.Now;
            var response = service.SaveBedSoreChgrec(request);
            return Ok(response);
        }

        [Route("{id:long}")]
        public IHttpActionResult Delete(long id)
        {
            var response = service.DeleteBedSoreChgrec(id);
            return Ok(response);
        }
    }
}
