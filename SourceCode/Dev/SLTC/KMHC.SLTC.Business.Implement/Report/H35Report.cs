using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.Report
{
   public class H35Report :BaseReport
    {
         protected override void Operation(WordDocument doc)
        {
            int id = (int)ParamId;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var caredemandevalprivew = reportManageService.GetCareDemandHis(id, SecurityHelper.CurrentPrincipal.OrgId);
            if (caredemandevalprivew == null)
            {
                InitData(typeof(CareDemandEvalPrivew), doc);
                return;
            }
            BindData(caredemandevalprivew, doc);
            
        }
    }
}
