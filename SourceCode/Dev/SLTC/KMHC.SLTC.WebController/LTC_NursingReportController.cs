using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.DC.Report;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController
{
    public partial class DC_ReportController : ReportBaseController
    {
        public ActionResult PreviewLTC_NursingReport()
        {
            string templateName = Request["templateName"];
            string feeNo = Request["feeNo"];
            string id = Request["id"];
            ReportRequest request = new ReportRequest();
            if (templateName != null)
            {
                switch (templateName)
                {
                    case "ADLReport":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.type = "ADLReport";
                            this.GeneratePDF("ADLReport", this.Operation, request);
                        }
                        break;
                    case "MMSEReport":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.type = "MMSEReport";
                            this.GeneratePDF("MMSEReport", this.Operation, request);
                        }
                        break;
                    case "SPMSQReport":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.type = "SPMSQReport";
                            this.GeneratePDF("SPMSQReport", this.Operation, request);
                        }
                        break;
                    case "IADLReport":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.type = "IADLReport";
                            this.GeneratePDF("IADLReport", this.Operation, request);
                        }
                        break;
                    case "ColeScaleReport":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.type = "ColeScaleReport";
                            this.GeneratePDF("ColeScaleReport", this.Operation, request);
                        }
                        break;
                    case "PrsSoreReport":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.type = "PrsSoreReport";
                            this.GeneratePDF("PrsSoreReport", this.Operation, request);
                        }
                        break;
                    case "CareEvalReport":
                        if (!string.IsNullOrEmpty(id))
                        {
                            request.id = int.Parse(id);
                            request.type = "CareEvalReport";
                            this.GeneratePDF("CareEvalReport", this.Operation, request);
                        }
                        break;
                }
            }
            return View("Preview");
        }

        protected void Operation(WordDocument doc, ReportRequest request)
        {
            long id = request.id;
            var codeType = request.type;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var question = reportManageService.GetQuestion(id);
            if (question.Id == 0)
            {
                InitData(typeof(Question), doc);
                switch (codeType)
                {
                    case "ADLReport":
                        InitValue(31, 40, doc);
                        break;
                    case "MMSEReport":
                        InitValue(41, 47, doc);
                        break;
                    case "SPMSQReport":
                        InitValue(48, 57, doc);
                        break;
                    case "IADLReport":
                        InitValue(58, 65, doc);
                        break;
                    case "ColeScaleReport":
                        InitValue(66, 66, doc);
                        break;
                    case "PrsSoreReport":
                        InitValue(100, 105, doc);
                        break;
                }

                return;
            }
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            var dict = new Dictionary<string, string>();
            BindData(question, doc, dict);
            var answers = reportManageService.GetAnswers(question.Id).ToList();
            switch (codeType)
            {
                case "ADLReport":
                    for (var i = 31; i <= 40; i++)
                    {
                        var answer = answers.Find(o => o.Id == i);
                        dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                    }
                    break;
                case "MMSEReport":
                    for (var i = 41; i <= 51; i++)
                    {
                        var answer = answers.Find(o => o.Id == i);
                        doc.ReplaceText("Value" + i, answer != null ? answer.Value : "未填");
                    }
                    break;
                case "SPMSQReport":
                    for (var i = 52; i <= 61; i++)
                    {
                        var answer = answers.Find(o => o.Id == i);
                        dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                    }
                    break;
                case "IADLReport":
                    for (var i = 62; i <= 69; i++)
                    {
                        var answer = answers.Find(o => o.Id == i);
                        dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                    }
                    break;
                case "ColeScaleReport":
                    for (var i = 70; i <= 70; i++)
                    {
                        var answer = answers.Find(o => o.Id == i);
                        dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                    }
                    break;
                case "PrsSoreReport":
                    for (var i = 104; i <= 109; i++)
                    {
                        var answer = answers.Find(o => o.Id == i);
                        dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                    }
                    break;
            }
                

            list.Add(dict);
            doc.FillTable(0, list, doc,codeType);
        }

    }
}
