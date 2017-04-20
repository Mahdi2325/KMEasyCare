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

namespace KMHC.SLTC.WebAPI.ResidentManage
{
    [RoutePrefix("api/familydiscussrec")]
    public class FamilyDiscussController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult get(int CurrentPage, int PageSize, int FeeNo, string OrgId)
        {
            BaseRequest<FamilyDiscussFilter> request = new BaseRequest<FamilyDiscussFilter>
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = { FeeNo = FeeNo, OrgId = OrgId }
            };
            var response = service.QueryFamilyDiscussExtend(request);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Get(int Id)
        {
            var response = service.GetFamilyDiscuss(Id);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(FamilyDiscuss baseRequest)
        {
            var response = service.SaveFamilyDiscuss(baseRequest);
            return Ok(response);
        }

        [Route("{Id}")]
        public IHttpActionResult Delete(int Id)
        {
            var response = service.DeleteFamilyDiscuss(Id);
            return Ok(response);
        }

    }
}

