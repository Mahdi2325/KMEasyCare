using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/visitorInOutRec")]
    public class VisitorInOutRecController : BaseController
    {
        IVisitorInOutRecService service = IOCContainer.Instance.Resolve<IVisitorInOutRecService>();

        [Route(""), HttpGet]

        public IHttpActionResult Get(string keyWord, DateTime sDate, DateTime eDate, int currentPage, int pageSize)
        {
            var request = new BaseRequest<VisitorInOutFilter>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = new VisitorInOutFilter()
                {
                    StartDate = sDate,
                    EndDate = eDate,
                    keyWord = keyWord
                }
            };
            var response = service.QueryVisitorInOutList(request);
            return Ok(response);
        }


        [Route("")]
        public IHttpActionResult Delete(int Id)
        {
            var response = service.DeleteVisitorInOutByID(Id);
            return Ok(response);
        }


        [Route("")]
        public IHttpActionResult Post(VisitorInOut request)
        {
            var response = new BaseResponse<VisitorInOut>();
            response = service.SaveVisitorInOut(request);
            return Ok(response);
        }

    }
}