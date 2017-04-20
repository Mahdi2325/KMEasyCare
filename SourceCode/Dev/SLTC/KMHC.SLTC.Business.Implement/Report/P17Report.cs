using System;
using System.Collections.Generic;
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
    public class P17Report:BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            int seqNo = (int)ParamId;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var nscpl = reportManageService.GetNSCPLReportView(seqNo);
            if (nscpl == null)
            {
                InitData(typeof(NSCPLReportView), doc);
                return;
            }
            BindData(nscpl, doc);
            //doc.ReplaceText("Org", nscpl.Org);
            //doc.ReplaceText("SeqNo", nscpl.SeqNo.ToString());
            //doc.ReplaceText("StartDate", nscpl.StartDate);
            //doc.ReplaceText("EndDate", nscpl.EndDate);
            //doc.ReplaceText("RegName", nscpl.RegName);
            //doc.ReplaceText("FeeNo", nscpl.FeeNo.ToString());
            //doc.ReplaceText("Sex", nscpl.Sex);
            //doc.ReplaceText("Age", nscpl.Age);
            //doc.ReplaceText("EmpName", nscpl.EmpName);
            //doc.ReplaceText("CpLevel", nscpl.CpLevel);
            //doc.ReplaceText("CpDiag", nscpl.CpDiag);
            //doc.ReplaceText("CpReason", nscpl.CpReason);
            //doc.ReplaceText("NsDesc", nscpl.NsDesc);
            //doc.ReplaceText("CpResult", nscpl.CpResult);
            //doc.ReplaceText("TotalDays", nscpl.TotalDays);
            //doc.ReplaceText("NscplGoal", nscpl.NscplGoal);
            //doc.ReplaceText("NscplActivity", nscpl.NscplActivity);
            //doc.ReplaceText("AssessValue", nscpl.AssessValue);
        }
    }
}
