using KM.Common;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.DC.CrossSpeciality
{
   
    [RoutePrefix("api/NurseCareLifeService")]
    public class NurseingLifeController : BaseController
    {
        IDC_CrossDayLife service = IOCContainer.Instance.Resolve<IDC_CrossDayLife>();
        //[Route("")]
        //public IHttpActionResult Post()
        //{
        //    //baseRequest.DayLifeRec.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
        //    //var response = service.SaveDayLife();

        //    return  null;
        //}

        //查询历史记录的一起的
       [Route("")]
        public IHttpActionResult Get(int FeeNo, int year, int num)
       {
           var response = service.QueryNurseingLife(FeeNo, year, num);

           return Ok(response);

       }
      //保存的
       [Route("")]
       public IHttpActionResult Post(NurseingLife1 baseRequest)
       {
           //baseRequest.DayLifeRec.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
           var response = service.SaveNurseingLife(baseRequest);

           return Ok(response);
       }

       [Route("")]
       public IHttpActionResult Get()
       {
           var response = service.yearweek();

           return Ok(response);
       }

       // 删除列表
       // DELETE api/syteminfo/5
       [Route("{id}")]
       public IHttpActionResult Delete(int id)
       {
           var response = service.DeleteNuring(id);
           return Ok(response);
       }


    }
}

