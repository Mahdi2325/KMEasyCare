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
    [RoutePrefix("api/resBirthDayList")]
    public class ResBirthDayListController : BaseController
    {
        IResBirthDayListService service = IOCContainer.Instance.Resolve<IResBirthDayListService>();


        [Route(""), HttpGet]
        public IHttpActionResult Query(DateTime sDate, DateTime eDate, string keyWord,int currentPage, int pageSize)
        {
            var response = service.QueryResBirthDayList(sDate, eDate, keyWord, currentPage, pageSize);
            return Ok(response);
        }

    }
}

