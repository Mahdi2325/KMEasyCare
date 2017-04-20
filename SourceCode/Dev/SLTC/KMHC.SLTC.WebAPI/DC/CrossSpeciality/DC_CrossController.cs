using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.CrossSpeciality
{
   [RoutePrefix("api/DCDayLifeCareRes")]
   public class DC_CrossController: BaseController
    {
       IDC_CrossDayLife service = IOCContainer.Instance.Resolve<IDC_CrossDayLife>();
       [Route("")]
       public IHttpActionResult Post(DayLife baseRequest)
       {
           //baseRequest.DayLifeRec.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
           var response = service.SaveDayLife(baseRequest);

           return Ok(response);
       }

       [Route("")]
       public IHttpActionResult Get(int FeeNo, int year, int num)
       {
           var response = service.QueryDayLife(FeeNo, year, num);

           return Ok(response);

       }

       [Route("")]
       public IHttpActionResult Get()
       {
         
           var response = service.yearweek();

           return Ok(response);

       }

       public static int WeekOfYear(DateTime dt, CultureInfo ci)
       {
           return ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
       }


       // 删除列表
       // DELETE api/syteminfo/5
       [Route("{id}")]
       public IHttpActionResult Delete(int id)
       {
           var response = service.DeleteDayLife(id);
           return Ok(response);
       }

    }
}
