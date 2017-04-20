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
    public class IADLReport:BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            long feeNo = ParamId;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var questionList = reportManageService.GetQuestionList(feeNo, 7);
            if (questionList.Count == 0)
            {
                InitData(typeof(Question), doc);
                InitValue(81, 88, doc);
                return;
            }
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (var question in questionList)
            {
                var dict = new Dictionary<string, string>();
                BindData(question, doc, dict);
                var answers = reportManageService.GetAnswers(question.Id).ToList();
                for (var i = 81; i <= 88; i++)
                {
                    var answer = answers.Find(o => o.Id == i);
                    dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                }
                list.Add(dict);
            }
            doc.FillTable(0, list);
        }
    }
}
