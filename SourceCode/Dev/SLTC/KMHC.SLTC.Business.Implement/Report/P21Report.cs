using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class P21Report:BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            //入住72小时内肠胃道出血而非计划性转到急性医院住院人次
            //因肠胃道出血而非计划性转到急性医院住院人次
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            DateTime now = StartDate;
            doc.ReplaceText("year", (now.Year).ToString());
            doc.ReplaceText("month", now.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var dt72 = ToDataTable(reportManageService.GetUnPlanEdipd(now, true));
            doc.FillTable(0, dt72, "而非计划转至急性医院住院人数", "入住72小时内发生因");
            doc.FillChartData(0, dt72, 6);

            var dt = ToDataTable(reportManageService.GetUnPlanEdipd(now, false));
            doc.FillTable(1, dt, "而非计划转至急性医院住院人数");
            doc.FillChartData(1, dt, 6);
        }
    }
}

