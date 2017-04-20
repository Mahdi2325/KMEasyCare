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
using System.Net.Http;

namespace KMHC.SLTC.WebAPI.NCIP
{
     [RoutePrefix("api/ServiceDepositGrantList")]
    public class ServiceDepositGrantListController : BaseController
    {
        IServiceDepositGrantList service = IOCContainer.Instance.Resolve<IServiceDepositGrantList>();
        [Route(""), HttpGet]
         public async Task<object> Get( int CurrentPage, int PageSize)
         {
             string nsNo = service.GetNsno();
             var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/ServiceDepositGrantList/GetServiceDepositGrantByNsNo?NSNO=" + nsNo + "&CurrentPage=" + CurrentPage + "&PageSize=" + PageSize);
             object resultContent = await result.Content.ReadAsAsync<object>();
             return resultContent;
         }
    }
}
