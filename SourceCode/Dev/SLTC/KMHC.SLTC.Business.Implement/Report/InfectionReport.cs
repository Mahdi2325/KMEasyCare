using System;
using System.Collections.Generic;
using System.Data;
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
    public class InfectionReport : BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            DateTime date = StartDate;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            doc.ReplaceText("Year", (date.Year).ToString());
            doc.ReplaceText("Month", date.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var list = reportManageService.GetInfection(date);
            dynamic[] tags = new dynamic[] { 
                new { Type = "totalB", Value = "001,002,003,004,005", Text = "总感染人次(b)" },
                new { Type = "totalC", Value = "001,002", Text = "呼吸道感染人次(c)" }, 
                new { Type = "001", Value = "001", Text = "上呼吸道感染人次(cl)" }, 
                new { Type = "002", Value = "002", Text = "下呼吸道感染人次(c2)" },
                new { Type = "totalD", Value = "003,004", Text = "泌尿道感染人次(d)" },
                new { Type = "003", Value = "003", Text = "当月使用存留导尿管泌尿道感染人次(dl)" },
                new { Type = "004", Value = "004", Text = "当月未使用存留导尿管泌尿道感染人次(d2)" }, 
                new { Type = "005", Value = "005", Text = "疥疮感染人次(g)" }
            };
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("total");
            foreach (dynamic item in tags)
            {
                var dr = dt.NewRow();
                dr["name"] = item.Text;
                string[] types = item.Value.Split(',');
                dr["total"] = list.Where(o => types.Contains(o.Type)).Sum(o => o.Total);
                dt.Rows.Add(dr);
                doc.ReplaceText(item.Type, dr["total"].ToString());
            }
            doc.FillChartData(0, dt, 10);
            dt.Dispose();
        }
    }
}

