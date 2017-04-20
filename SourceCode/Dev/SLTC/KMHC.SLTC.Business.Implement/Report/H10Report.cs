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
    /// <summary>
    /// 护理计划 预览打印
    /// </summary>
    public class H10Report : BaseReport
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
        }
    }
}
