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
    public class PrsSoreReport : BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            int recordId = (int)ParamId;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var question = reportManageService.GetQuestion(recordId);
            if (question == null)
            {
                InitData(typeof(Question), doc);
                InitValue(123, 128, doc);
                return;
            }

            BindData(question, doc);
            var answers = reportManageService.GetAnswers(question.Id).ToList();
            for (var i = 123; i <= 128; i++)
            {
                var answer = answers.Find(o => o.Id == i);
                doc.ReplaceText("Value" + i, answer != null ? answer.Value : "未填");
            }
        }
    }
}
