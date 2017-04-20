using KM.Common;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Interface.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController
{
    public class DC_ChartsController : Controller
    {
        IDC_ChartsService svc = IOCContainer.Instance.Resolve<IDC_ChartsService>();
        public JsonResult GetDailyUUTrend()
        {
            List<Chart_AccumulateUU> list = svc.GetAccumulateUU();
            return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiseaseDistribution()
        {
            List<Chart_DiseaseDistribution> list = svc.GetDiseaseDistribution();
            return Json(new { result = true, data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}
