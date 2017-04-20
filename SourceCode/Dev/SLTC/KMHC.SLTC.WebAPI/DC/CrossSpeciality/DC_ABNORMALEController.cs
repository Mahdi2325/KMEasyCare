using KM.Common;
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
    [RoutePrefix("api/DCProblemBehaviorList")]
    public class DC_ABNORMALEController : BaseController
    {

        IDC_CrossDayLife service = IOCContainer.Instance.Resolve<IDC_CrossDayLife>();
        [Route("")]
        public IHttpActionResult Post(AbNormaleMotionRec baseRequest)
        {
            //baseRequest.DayLifeRec.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            var response = service.SaveAB(baseRequest);

            return Ok(response);
        }
        //查询的东西
        [Route("")]
        public IHttpActionResult Get(int FeeNo, int year, int month)
        {
            var response = service.QueryAB(FeeNo, year, month);

            return Ok(response);

        }

        // 批量修改状态
        // DELETE api/syteminfo/5
        [Route("")]
        public IHttpActionResult Delete(int regno, int year, int month)
        {
            var response = service.DeleteAB(regno, year, month);
            return Ok(response);
        }
    }
}
