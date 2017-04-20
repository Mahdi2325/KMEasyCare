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
    public class CareEvalReport : BaseReport
    {
        protected override void Operation(WordDocument doc)
        {
            INursingCareEvalService service = IOCContainer.Instance.Resolve<INursingCareEvalService>();
            int id = (int)ParamId;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            var response　=　service.GetNursingCareEvalQues("VL", -1, id);
            doc.ReplaceText("Name", response.Data.Name == null ? "" : response.Data.Name);
            doc.ReplaceText("Ssno", response.Data.Ssno == null ? "" : response.Data.Ssno);
            doc.ReplaceText("Starttime", response.Data.Starttime.Value.ToString("yyyy-MM-dd"));
            doc.ReplaceText("Residentno", response.Data.Residentno == null ? "" : response.Data.Residentno);
            doc.ReplaceText("Bedno", response.Data.Bedno == null ? "" : response.Data.Bedno);
            if (response != null && response.Data.nursingEval != null && response.Data.nursingEval.Count > 0)
            {
                var sumScore = 0;
                foreach (var nurEval in response.Data.nursingEval)
                {
                    var subtotal = 0;
                    foreach (var quesitem in nurEval.QuesItem)
                    {
                        string replacename = "sub";
                        if (quesitem.Id < 10)
                        {
                            replacename += "0" + quesitem.Id;
                        }
                        else
                        {
                            replacename += quesitem.Id;
                        }
                        subtotal += quesitem.ncievaluatedtl.Score == null ? 0 : quesitem.ncievaluatedtl.Score.Value;
                        doc.ReplaceText(replacename, quesitem.ncievaluatedtl.Score == null ? "0" : quesitem.ncievaluatedtl.Score.Value.ToString());
                    }
                    sumScore += subtotal;
                    doc.ReplaceText("sum" + nurEval.Order.Value, subtotal.ToString());
                }
                doc.ReplaceText("sum5", sumScore.ToString());
            }
        }
    }
}
