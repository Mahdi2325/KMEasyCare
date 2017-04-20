using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using KMHC.SLTC.Business.Entity.Model;

namespace KMHC.SLTC.WebAPI.DC.SysAdmin
{
    [RoutePrefix("api/DCActivity")]
    public class DC_ActivityController : BaseController
    {
        IDC_SysAdminService service = IOCContainer.Instance.Resolve<IDC_SysAdminService>();
        IOrganizationManageService orgserver = IOCContainer.Instance.Resolve<IOrganizationManageService>();
        [Route(""), HttpGet]
        public IHttpActionResult Query(string keyWord, string Activecode, string orgid, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<DC_TeamActivitydtlFilter> request = new BaseRequest<DC_TeamActivitydtlFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            if (!string.IsNullOrEmpty(Activecode))
            { request.Data.ACTIVITYCODE = Activecode; }


            request.Data.TITLENAME = keyWord;

            if (!string.IsNullOrEmpty(orgid))
            { request.Data.ORGID = orgid; }
            else
            { request.Data.ORGID = SecurityHelper.CurrentPrincipal.OrgId; }


            var orginfo = orgserver.GetOrg(request.Data.ORGID);

            var responseByQuery = service.QueryTeamActivitydtl(request);
            var response = new BaseResponse<List<object>>(new List<object>());
            response.CurrentPage = responseByQuery.CurrentPage;
            response.PagesCount = responseByQuery.PagesCount;
            response.RecordsCount = responseByQuery.RecordsCount;
            Array.ForEach(responseByQuery.Data.ToArray(), (item) =>
            {
                if (!(string.IsNullOrEmpty(item.TITLENAME) && string.IsNullOrEmpty(item.ITEMNAME)))
                {
                    response.Data.Add(new
                    {
                        ID = item.ID,
                        SEQNO = item.SEQNO,//item.SEQNO,
                        TITLENAME = item.TITLENAME,
                        ACTIVITYCODE = item.ACTIVITYCODE,
                        ITEMNAME = item.ITEMNAME,
                        ORGID = orginfo.Data.OrgName
                    }
                );
                }
               
            });
            return Ok(response);

        }


        [Route("")]
        public IHttpActionResult Post(DC_TeamActivitydtlModel baseRequest)
        {
            baseRequest.ORGID = SecurityHelper.CurrentPrincipal.OrgId;

            var response = service.SaveTeamActivitydtl(baseRequest);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {

            var response = service.DeleteTeamActivitydtl(id);
            return Ok(response);


        }



    }
}