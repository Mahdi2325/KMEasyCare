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
    public class P13Report:BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            int recordId = (int)ParamId;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            ISocialWorkerManageService reportManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IResidentManageService residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            var question = reportManageService.GetRegEvaluateById(recordId).Data;
            if (question == null || !question.FeeNo.HasValue)
            {
                InitData(typeof(RegEvaluateModel), doc);
                doc.ReplaceText("BedNo", "");
                doc.ReplaceText("Floor", "");
                doc.ReplaceText("Name", "");
                doc.ReplaceText("Age", "");
                doc.ReplaceText("IllCard", "");
                doc.ReplaceText("Service",  "");
                doc.ReplaceText("NextDate", "");
                return;
            }
            var emp = organizationManageService.GetEmployee(question.EvaluateBy);
            doc.ReplaceText("EmpName", emp == null ? "" : emp.Data.EmpName);
            var request = new CodeFilter
            {
                ItemTypes = new[]
                    {
                        "E00.014", "E00.005", "E00.008", "E00.009", "E00.015", "E00.016", "E00.017", "E00.018",
                        "E00.019","E00.206", "E00.207", "E00.208", "E00.209", "E00.210", "E00.211"
                    }
            };
            var dict = (List<CodeValue>)dictManageService.QueryCode(request).Data;
            if (question.MindState != null)
            {
                doc.ReplaceText("MindState", dict.Find(it => it.ItemType == "E00.005" && it.ItemCode == question.MindState).ItemName);
            }
            if (question.ExpressionState != null)
            {
                doc.ReplaceText("ExpressionState", dict.Find(it => it.ItemType == "E00.008" && it.ItemCode == question.ExpressionState).ItemName);
            }
            if (question.LanguageState != null)
            {
                doc.ReplaceText("LanguageState", dict.Find(it => it.ItemType == "E00.015" && it.ItemCode == question.LanguageState).ItemName);
            }
            if (question.NonexpressionState != null)
            {
                doc.ReplaceText("NonexpressionState", dict.Find(it => it.ItemType == "E00.009" && it.ItemCode == question.NonexpressionState).ItemName);
            }
            if (question.EmotionState != null)
            {
                doc.ReplaceText("EmotionState", dict.Find(it => it.ItemType == "E00.016" && it.ItemCode == question.EmotionState).ItemName);
            }

            if (question.Personality != null)
            {
                doc.ReplaceText("Personality", dict.Find(it => it.ItemType == "E00.017" && it.ItemCode == question.Personality).ItemName);
            }
            if (question.Attention != null)
            {
                doc.ReplaceText("Attention", dict.Find(it => it.ItemType == "E00.018" && it.ItemCode == question.Attention).ItemName);
            }
            if (question.Realisticsense != null)
            {
                doc.ReplaceText("Realisticsense", dict.Find(it => it.ItemType == "E00.019" && it.ItemCode == question.Realisticsense).ItemName);
            }
            if (question.SocialParticipation != null)
            {
                doc.ReplaceText("SocialParticipation", dict.Find(it => it.ItemType == "E00.206" && it.ItemCode == question.SocialParticipation).ItemName);
            }
            if (question.SocialAttitude != null)
            {
                doc.ReplaceText("SocialAttitude", dict.Find(it => it.ItemType == "E00.207" && it.ItemCode == question.SocialAttitude).ItemName);
            }

            if (question.SocialSkills != null)
            {
                doc.ReplaceText("SocialSkills", dict.Find(it => it.ItemType == "E00.208" && it.ItemCode == question.SocialSkills).ItemName);
            }
            if (question.CommSkills != null)
            {
                doc.ReplaceText("CommSkills", dict.Find(it => it.ItemType == "E00.209" && it.ItemCode == question.CommSkills).ItemName);
            }
            if (question.ResponseSkills != null)
            {
                doc.ReplaceText("ResponseSkills", dict.Find(it => it.ItemType == "E00.210" && it.ItemCode == question.ResponseSkills).ItemName);
            }
            if (question.FixissueSkills != null)
            {
                doc.ReplaceText("FixissueSkills", dict.Find(it => it.ItemType == "E00.211" && it.ItemCode == question.FixissueSkills).ItemName);
            }
            if (question.BookDegree != null)
            {
                doc.ReplaceText("BookDegree", dict.Find(it => it.ItemType == "E00.014" && it.ItemCode == question.BookDegree).ItemName);
            }
            doc.ReplaceText("IllCard", question.IllCardName ?? "");
            doc.ReplaceText("Service", question.ServiceName ?? "");
            doc.ReplaceText("NextDate", question.NextEvalDate == null ? "" : ((DateTime)question.NextEvalDate).ToString("yyyy-MM-dd"));
            BindData(question, doc);

            var resident = residentManageService.GetResident(question.FeeNo.Value);
            if (resident != null && resident.Data != null)
            {
                doc.ReplaceText("BedNo", resident.Data.BedNo);
                doc.ReplaceText("Floor", resident.Data.Floor);
            }
            else
            {
                doc.ReplaceText("BedNo", "");
                doc.ReplaceText("Floor", "");
            }

            var person = residentManageService.GetPerson(question.RegNo ?? 0);
            if (person != null && person.Data != null)
            {
                doc.ReplaceText("Name", person.Data.Name);
                doc.ReplaceText("Age", person.Data.Age.ToString());
            }
            else
            {
                doc.ReplaceText("Name", "");
                doc.ReplaceText("Age", "");
            }
            
        }
    }
}
