using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using KM.Common;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/NutrtionEval")]
    public class NutrtionEvalController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int FeeNo, string OrgId)
        {
            BaseRequest<NutrtionEvalFilter> request = new BaseRequest<NutrtionEvalFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeNo = FeeNo, OrgId = OrgId }
            };
            var response = service.QueryNutrtionEvalExtend(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Query(int FeeNo, string startDate, string endDate)
        {
            BaseRequest<NutrtionEvalFilter> request = new BaseRequest<NutrtionEvalFilter>
            {
                PageSize = 0,
                Data =
                {
                    FeeNo = FeeNo,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                    StartDate=Convert.ToDateTime(startDate),
                    EndDate = Convert.ToDateTime(endDate),
                }
            };
            var response = service.QueryNutrtionEvalExtend(request);
            return Ok(response);
        }
        [Route("{Id}")]
        public IHttpActionResult Get(int Id)
        {
            var response = service.GetNutrtionEval(Id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(LTCNUTRTION72EVAL baseRequest)
        {
            var response = service.SaveNutrtionEval(baseRequest);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = service.DeleteNutrtionEval(Id);
            return Ok(response);
        }

    }
}

