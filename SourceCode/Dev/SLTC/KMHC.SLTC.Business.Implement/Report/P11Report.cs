using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class P11Report:BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            DateTime date = StartDate;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            //doc.ReplaceText("Year", (date.Year - 1911).ToString());
            doc.ReplaceText("Year", date.Year.ToString());
            doc.ReplaceText("Month", date.Month.ToString());
            var constraintList = reportManageService.GetConstraintList(date);
            string key = string.Empty;

            dynamic[] tags = new dynamic[] { 
                new { Type = "ExecReason", Value = "001", Text = "因预防跌倒而使用身体约束人数（bl)" },
                new { Type = "ExecReason", Value = "002", Text = "因预防自拔管路而使用身体约束人数（b2)" }, 
                new { Type = "ExecReason", Value = "003", Text = "因预防自伤而使用身体约束人数（b3)" }, 
                new { Type = "ExecReason", Value = "004", Text = "因行为紊乱而使用身体约束人数（b4)" },
                new { Type = "ExecReason", Value = "005", Text = "因协助治疗而使用身体约束人数（b5)" },
                new { Type = "ExecReason", Value = "006", Text = "因其他因素而使用身体约束人数（b6)" },
                new { Type = "Duration", Value = "001", Text = "约束持续小於等於4小时人数（c1)" },
                new { Type = "Duration", Value = "002", Text = "约束持续大於4小时小於等於8小时人数（c2)" }, 
                new { Type = "Duration", Value = "003", Text = "约束持续大於8小时小於等於16小时人数（c3)" }, 
                new { Type = "Duration", Value = "004", Text = "约束持续大於16小时小於等於24小时人数（c4)" },
                new { Type = "Duration", Value = "005", Text = "约束持续大於24小时人数（c5)" },
                new { Type = "ConstraintWay", Value = "002", Text = "受身体约束二种以上住民人数(d)" },
                new { Type = "Cancel", Value = "24Flag", Text = "当月移除身体约束至少维持24小时以上之住民人数(e)" },
            };
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("name");
            dt.Columns.Add("total");
            foreach (dynamic item in tags)
            {
                dr = dt.NewRow();
                dr["name"] = item.Text;
                switch ((string)item.Type)
                {
                    case "ExecReason":
                        dr["total"] = constraintList.Count(it => it.ExecReason == item.Value);
                        break;
                    case "Duration":
                        dr["total"] = constraintList.Count(it => it.Duration == item.Value);
                        break;
                    case "ConstraintWay":
                        dr["total"] = constraintList.Count(it => it.ConstraintWayCnt == "002");
                        break;
                    case "Cancel":
                        dr["total"] = constraintList.Count(it => it.Cancel24Flag);
                        break;
                }
                dt.Rows.Add(dr);
                doc.ReplaceText(string.Format("{0}{1}", item.Type, item.Value), dr["total"].ToString());
            }
            doc.FillChartData(0, dt, 10);
            dt.Dispose();
        }
    }
}

