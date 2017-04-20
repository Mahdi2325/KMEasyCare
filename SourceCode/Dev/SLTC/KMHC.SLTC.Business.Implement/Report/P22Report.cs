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
    public class P22Report:BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            DateTime now = StartDate;
            //doc.ReplaceText("year", (now.Year - 1911).ToString());
            doc.ReplaceText("year", now.Year.ToString());    
            doc.ReplaceText("month", now.Month.ToString());
           
            decimal total = reportManageService.GetResidentTotal(now);
            if (total == 0)
            {
                doc.ReplaceText("New", "0");
                doc.ReplaceText("N72", "0");
                doc.ReplaceText("R72", "0");
                doc.ReplaceText("CTotal", "0");
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
                doc.ReplaceText("N0", "0");
                doc.ReplaceText("N1", "0");
                doc.ReplaceText("N2", "0");
                doc.ReplaceText("N3", "0");
                doc.ReplaceText("N4", "0");
                doc.ReplaceText("R0", "0");
                doc.ReplaceText("R1", "0");
                doc.ReplaceText("R2", "0");
                doc.ReplaceText("R3", "0");
                doc.ReplaceText("R4", "0");
                doc.ReplaceText("CompareNum", "");
                doc.ReplaceText("ReasonInfo", "");
                return;
            }
            doc.ReplaceText("CTotal", total.ToString("#0"));
            decimal newTotal = reportManageService.GetNewResidentTotal(now);
            if (newTotal == 0)
            {
                doc.ReplaceText("New", "0");
                doc.ReplaceText("R72", "0");
                doc.ReplaceText("N72", "0");
            }
            else
            {
                doc.ReplaceText("New", newTotal.ToString("#0"));
                decimal n72 = reportManageService.GetUnPlanEdipdH72Total(now);
                if (n72 != 0)
                {
                    doc.ReplaceText("R72", (n72 / newTotal * 100).ToString("#0.0"));
                    doc.ReplaceText("N72", n72.ToString("#0"));
                }
                else
                {
                    doc.ReplaceText("N72", "0");
                    doc.ReplaceText("R72", "0");
                }
            }
            var list = reportManageService.GetUnPlanEdipd(now, false);
            decimal nTotal = list.Sum(o => o.Total);

            var lastList = reportManageService.GetUnPlanEdipd(now.AddMonths(-1), false);
            decimal lastNTotal = lastList.Sum(o => o.Total);

            if (nTotal == 0)
            {
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
            }
            else
            {
                doc.ReplaceText("NTotal", nTotal.ToString("#0"));
                doc.ReplaceText("RTotal", (nTotal / total * 100).ToString("#0.0"));
            }

            if (nTotal < lastNTotal)
            {
                doc.ReplaceText("CompareNum", "较上月减少" + (lastNTotal - nTotal).ToString("#0") + "，");
            }
            else if (nTotal == lastNTotal)
            {
                doc.ReplaceText("CompareNum", "较上月相同，");
            }
            else if (nTotal > lastNTotal)
            {
                doc.ReplaceText("CompareNum", "较上月增加" + (nTotal - lastNTotal).ToString("#0") + "人，"); 
            }

 
            var keys = new[] { "心血管代偿机能减退", "骨折之治疗或评估", "肠胃道出血", "感染", "其他内外科原因" };
            var msgInfo = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                if (nTotal == 0)
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                    msgInfo += "";
                    continue;
                }
                var obj = list.FirstOrDefault(o => o.Type == keys[i]);
                if (obj != null)
                {
                    doc.ReplaceText("N" + i, obj.Total.ToString());
                    doc.ReplaceText("R" + i, (obj.Total / nTotal * 100).ToString("#0.0"));
                    msgInfo += obj.Total == 0 ? "" : (",因" + keys[i] + "住院" + obj.Total.ToString() + "位");
                }
                else
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                    msgInfo += "";
                }
            }
            doc.ReplaceText("ReasonInfo", msgInfo);
        }
    }
}

