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
    public class ColeScaleReport : BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            int recordId = (int)ParamId;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var question = reportManageService.GetQuestion(recordId);
            if (question == null)
            {
                InitData(typeof(Question), doc);
                doc.ReplaceText("Value89", "");
                return;
            }
            BindData(question, doc);
            var answers = reportManageService.GetAnswers(question.Id).ToList();
            var answer = answers.Find(o => o.Id == 89);
            doc.ReplaceText("Value89", answer != null ? answer.Value : "未填");
        }
    }
}
