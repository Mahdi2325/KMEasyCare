﻿using KM.Common;
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
    [RoutePrefix("api/DCDataDicEdit")]
    public class DC_DataDicEditDtlController : BaseController
    {
        IDC_SysAdminService service = IOCContainer.Instance.Resolve<IDC_SysAdminService>();
        IOrganizationManageService orgserver = IOCContainer.Instance.Resolve<IOrganizationManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(string flag, int staus )
        {
            BaseRequest<OrganizationFilter> request = new BaseRequest<OrganizationFilter>
            {
                CurrentPage = 1,
                PageSize = 100,
                Data = { OrgName = "" }
            };
            var response = orgserver.QueryOrg(request);
            var responseObject = new BaseResponse<IList<Object>>();
            Array.ForEach(response.Data.ToArray(), (item) =>
            {
                responseObject.Data.Add(new
                {
                    orgid = item.OrgId,
                    orgname = item.OrgName,

                });

            });
            return Ok(responseObject);
        }

        

        [Route("")]
        public IHttpActionResult Post(DC_COMMFILEModel baseRequest)
        {
            baseRequest.UPDATEDATE = System.DateTime.Now;
            //baseRequest.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
             var response = service.SaveDCCOMMFILE(baseRequest);
            return Ok(response);
            
            
        }
        [Route("")  ]
        public IHttpActionResult Delete(string itemtype)
        {

            var response = service.DeleteDCcommfile(itemtype);
            return Ok(response);
            
            
        }


        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {

            var response = service.GetDCcommfile(id);
            return Ok(response);


        }

    }
}