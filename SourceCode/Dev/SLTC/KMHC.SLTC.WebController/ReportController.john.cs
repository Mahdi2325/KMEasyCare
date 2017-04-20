using KM.Common;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Report;

namespace KMHC.SLTC.WebController
{
    public partial class ReportController : Controller
    {
        /// <summary>
        /// 失智量表
        /// </summary>
        /// <param name="doc"></param>
        private void MMSEOperation(WordDocument doc)
        {
            int recordId = 31;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var question = reportManageService.GetQuestion(recordId);
            if (question == null)
            {
                InitData(typeof(Question), doc);
                InitValue(41, 70, doc);
                return;
            }
            BindData(question, doc);
            var answers = reportManageService.GetAnswers(question.Id).ToList();
            for (var i = 41; i <= 70; i++)
            {
                var answer = answers.Find(o => o.Id == i);
                doc.ReplaceText("Value" + i, answer != null ? answer.Value : "未填");
            }
        }

        /// <summary>
        /// 压疮风险评估
        /// </summary>
        /// <param name="doc"></param>
        private void PrsSoreOperation(WordDocument doc)
        {
            int recordId = 14;
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

        /// <summary>
        /// 柯氏评估
        /// </summary>
        /// <param name="doc"></param>
        private void ColeScaleOperation(WordDocument doc)
        {
            int recordId = 10;
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

        /// <summary>
        /// SPMSQ简易心智量表
        /// </summary>
        /// <param name="doc"></param>
        private void SPMSQOperation(WordDocument doc)
        {
            int feeNo = 7;
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            doc.ReplaceText("Org", org.Data.OrgName);
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var questionList = reportManageService.GetQuestionList(feeNo, 6);
            if (questionList.Count == 0)
            {
                InitData(typeof(Question), doc);
                InitValue(71, 80, doc);
                return;
            }
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (var question in questionList)
            {
                var dict = new Dictionary<string, string>();
                BindData(question, doc, dict);
                var answers = reportManageService.GetAnswers(question.Id).ToList();
                for (var i = 71; i <= 80; i++)
                {
                    var answer = answers.Find(o => o.Id == i);
                    dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                }
                list.Add(dict);
            }
            doc.FillTable(0, list);
        }

        /// <summary>
        /// ADL巴氏量表
        /// </summary>
        /// <param name="doc"></param>
        private void ADLOperation(WordDocument doc)
        {
            int feeNo = 7;
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            doc.ReplaceText("Org", org.Data.OrgName);
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var questionList = reportManageService.GetQuestionList(feeNo, 4);
            if (questionList.Count == 0)
            {
                InitData(typeof(Question), doc);
                InitValue(31, 40, doc);
                return;
            }
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (var question in questionList)
            {
                var dict = new Dictionary<string, string>();
                BindData(question, doc, dict);
                var answers = reportManageService.GetAnswers(question.Id).ToList();
                for (var i = 31; i <= 40; i++)
                {
                    var answer = answers.Find(o => o.Id == i);
                    dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                }
                list.Add(dict);
            }
            doc.FillTable(0, list);
        }

        /// <summary>
        /// IADL表
        /// </summary>
        /// <param name="doc"></param>
        private void IADLOperation(WordDocument doc)
        {
            int feeNo = 7;
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            doc.ReplaceText("Org", org.Data.OrgName);
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

        /// <summary>
        /// P13社工定期处遇评估表
        /// </summary>
        /// <param name="doc"></param>
        private void P13Operation(WordDocument doc)
        {
            int recordId = 4;
            ISocialWorkerManageService reportManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IResidentManageService residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
            //ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            var question = reportManageService.GetRegEvaluateById(recordId).Data;
            if (question == null) return;
            if (!question.FeeNo.HasValue) return;
            var resident = residentManageService.GetResident(question.FeeNo.Value);
            var person = residentManageService.GetPerson(resident.Data.RegNo ?? 0);

            var emp = organizationManageService.GetEmployee(question.EvaluateBy);
            CodeFilter request = new CodeFilter
            {
                ItemTypes = new string[]
                    {
                        "E00.014", "E00.005", "E00.008", "E00.009", "E00.015", "E00.016", "E00.017", "E00.018",
                        "E00.019","E00.206", "E00.207", "E00.208", "E00.209", "E00.210", "E00.211"
                    }
            };
            var org = organizationManageService.GetOrg(resident.Data.OrgId);
            var dict = (List<CodeValue>)dictManageService.QueryCode(request).Data;
            doc.ReplaceText("MindState", dict.Find(it => it.ItemType == "E00.005" && it.ItemCode == question.MindState).ItemName);
            doc.ReplaceText("ExpressionState", dict.Find(it => it.ItemType == "E00.008" && it.ItemCode == question.ExpressionState).ItemName);
            doc.ReplaceText("LanguageState", dict.Find(it => it.ItemType == "E00.015" && it.ItemCode == question.LanguageState).ItemName);
            doc.ReplaceText("NonexpressionState", dict.Find(it => it.ItemType == "E00.009" && it.ItemCode == question.NonexpressionState).ItemName);
            doc.ReplaceText("EmotionState", dict.Find(it => it.ItemType == "E00.016" && it.ItemCode == question.EmotionState).ItemName);
            doc.ReplaceText("Personality", dict.Find(it => it.ItemType == "E00.017" && it.ItemCode == question.Personality).ItemName);
            doc.ReplaceText("Attention", dict.Find(it => it.ItemType == "E00.018" && it.ItemCode == question.Attention).ItemName);
            doc.ReplaceText("Realisticsense", dict.Find(it => it.ItemType == "E00.019" && it.ItemCode == question.Realisticsense).ItemName);
            doc.ReplaceText("SocialParticipation", dict.Find(it => it.ItemType == "E00.206" && it.ItemCode == question.SocialParticipation).ItemName);
            doc.ReplaceText("SocialAttitude", dict.Find(it => it.ItemType == "E00.207" && it.ItemCode == question.SocialAttitude).ItemName);
            doc.ReplaceText("SocialSkills", dict.Find(it => it.ItemType == "E00.208" && it.ItemCode == question.SocialSkills).ItemName);
            doc.ReplaceText("CommSkills", dict.Find(it => it.ItemType == "E00.209" && it.ItemCode == question.CommSkills).ItemName);
            doc.ReplaceText("ResponseSkills", dict.Find(it => it.ItemType == "E00.210" && it.ItemCode == question.ResponseSkills).ItemName);
            doc.ReplaceText("FixissueSkills", dict.Find(it => it.ItemType == "E00.211" && it.ItemCode == question.FixissueSkills).ItemName);
            doc.ReplaceText("BookDegree", dict.Find(it => it.ItemType == "E00.014" && it.ItemCode == question.BookDegree).ItemName);
            doc.ReplaceText("EmpName", emp == null ? "" : emp.Data.EmpName);
            doc.ReplaceText("Org", org == null ? "" : org.Data.OrgName);
            doc.ReplaceText("Name", person == null ? "" : person.Data.Name);
            if (person != null && person.Data.Age != null)
            {
                doc.ReplaceText("Age", person.Data.Age.ToString());
            }
            else
            {
                doc.ReplaceText("Age", "");
            }
            doc.ReplaceText("BedNo", resident.Data.BedNo);
            doc.ReplaceText("Floor", resident.Data.Floor);
            doc.ReplaceText("IllCard", question.IllCardName);
            doc.ReplaceText("Service", question.ServiceName);
            doc.ReplaceText("NextDate", question.NextEvalDate == null ? "" : ((DateTime)question.NextEvalDate).ToString("yyyy-MM-dd"));
            BindData(question, doc);
        }

        /// <summary>
        /// P21非计画性转至怠性医院住院月统计表
        /// </summary>
        /// <param name="doc"></param>
        private void P21Operation(WordDocument doc)
        {
            //入住72小时内肠胃道出血而非计划性转到急性医院住院人次
            //因肠胃道出血而非计划性转到急性医院住院人次
            DateTime now = DateTime.Now;
            doc.ReplaceText("year", now.Year.ToString());
            doc.ReplaceText("month", now.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var dt72 = ToDataTable(reportManageService.GetUnPlanEdipd(now,true));
            doc.FillTable(0, dt72, "而非计划转至急性医院住院人数", "入住72小时内发生因");
            doc.FillChartData(0, dt72, 6);

            var dt = ToDataTable(reportManageService.GetUnPlanEdipd(now, false));
            doc.FillTable(1, dt, "而非计划转至急性医院住院人数");
            doc.FillChartData(1, dt, 6);

            }

        /// <summary>
        /// P22非计画性转至怠性医院住院月统计表
        /// </summary>
        /// <param name="doc"></param>
        private void P22Operation(WordDocument doc)
        {
            DateTime now = DateTime.Now;
            doc.ReplaceText("year", now.Year.ToString());
            doc.ReplaceText("month", now.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            decimal total = reportManageService.GetResidentTotal(now);
            if (total == 0)
            {
                doc.ReplaceText("New", "0");
                doc.ReplaceText("N72", "0");
                doc.ReplaceText("R72", "0");
                doc.ReplaceText("CTotal", "0");
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
                doc.ReplaceText("N0", "0");
                doc.ReplaceText("N1", "0");
                doc.ReplaceText("N2", "0");
                doc.ReplaceText("N3", "0");
                doc.ReplaceText("N4", "0");
                doc.ReplaceText("R0", "0");
                doc.ReplaceText("R1", "0");
                doc.ReplaceText("R2", "0");
                doc.ReplaceText("R3", "0");
                doc.ReplaceText("R4", "0");
                return;
            }
            doc.ReplaceText("CTotal", total.ToString("#0"));
            decimal newTotal = reportManageService.GetNewResidentTotal(now);
            if (newTotal == 0)
            {
                doc.ReplaceText("New", "0");
                doc.ReplaceText("R72", "0");
                doc.ReplaceText("N72", "0");
            }
            else
            {
                decimal n72 = reportManageService.GetUnPlanEdipdH72Total(now);
                if (n72 != 0)
                {
                    doc.ReplaceText("R72", (n72/newTotal*100).ToString("#0.0"));
                    doc.ReplaceText("N72", n72.ToString("#0"));
                }
                else
                {
                    doc.ReplaceText("N72", "0"); 
                }

            }
            var list = reportManageService.GetUnPlanEdipd(now, false);
            decimal nTotal = list.Sum(o=>o.Total);
            if (nTotal == 0)
            {
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
            }
            else
            {
                doc.ReplaceText("NTotal", nTotal.ToString("#0"));
                doc.ReplaceText("RTotal", (nTotal / total * 100).ToString("#0.0"));
            }
            var keys = new[] { "心血管代偿机能减退", "骨折之治疗或评估", "肠胃道出血", "感染", "其他内外科原因" };
            for (int i = 0; i < 5; i++)
            {
                if (nTotal == 0)
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                    continue;
                }
                var obj = list.FirstOrDefault(o => o.Type == keys[i]);
                if (obj != null)
                {
                    doc.ReplaceText("N" + i, obj.Total.ToString());
                    doc.ReplaceText("R" + i, (obj.Total / nTotal * 100).ToString("#0.0"));
                }
                else
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                }
            }
        }

        /// <summary>
        /// P23医疗照顾相关感染指标月统计表
        /// </summary>
        /// <param name="doc"></param>
        private void P23Operation(WordDocument doc)
        {
            DateTime now = DateTime.Now;
            doc.ReplaceText("year", now.Year.ToString());
            doc.ReplaceText("month", now.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            decimal total = reportManageService.GetResidentTotal(now);
            if (total == 0)
            {
                doc.ReplaceText("UTotal", "0");
                doc.ReplaceText("STotal", "0");
                doc.ReplaceText("CTotal", "0");
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
                doc.ReplaceText("N0", "0");
                doc.ReplaceText("N1", "0");
                doc.ReplaceText("N2", "0");
                doc.ReplaceText("N3", "0");
                doc.ReplaceText("N4", "0");
                doc.ReplaceText("N5", "0");
                doc.ReplaceText("R0", "0");
                doc.ReplaceText("R1", "0");
                doc.ReplaceText("R2", "0");
                doc.ReplaceText("R3", "0");
                doc.ReplaceText("R4", "0");
                doc.ReplaceText("R5", "0");
                return;
            }
            doc.ReplaceText("CTotal", total.ToString("#0"));

            var list = reportManageService.GetInfection(now);
            decimal nTotal = list.Sum(o => o.Total);
            if (nTotal == 0)
            {
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
            }
            else
            {
                doc.ReplaceText("NTotal", nTotal.ToString("#0"));
                doc.ReplaceText("RTotal", (nTotal / total * 100).ToString("#0.0"));
            }
            decimal sTotal = reportManageService.GetUsedPipeTotal(now);
            decimal uTotal = total - sTotal;
            doc.ReplaceText("STotal", sTotal.ToString("#0"));
            doc.ReplaceText("UTotal", uTotal.ToString("#0"));
            var keys = new[] { "001", "002", "003", "004", "005" };
            for (int i = 0; i < 5; i++)
            {
                if (nTotal == 0)
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                    continue;
                }
                var obj = list.FirstOrDefault(o => o.Type == keys[i]);
                if (obj != null)
                {
                    if (i == 2)
                    {
                        doc.ReplaceText("N" + i, obj.Total.ToString());
                        doc.ReplaceText("R" + i, sTotal != 0 ? (obj.Total/sTotal*100).ToString("#0.0") : "0");
                    }
                    else if (i == 3)
                    {
                        doc.ReplaceText("N" + i, obj.Total.ToString());
                        doc.ReplaceText("R" + i, uTotal != 0 ? (obj.Total / uTotal * 100).ToString("#0.0") : "0");
                    }
                    else
                    {
                        doc.ReplaceText("N" + i, obj.Total.ToString());
                        doc.ReplaceText("R" + i, (obj.Total / total * 100).ToString("#0.0"));
                    }
                   
                }
                else
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                }
            }

            var n5 = list.Where(o => o.Type == "003" || o.Type == "004").Sum(o => o.Total);
            doc.ReplaceText("N5", n5.ToString());
            doc.ReplaceText("R5", n5 != 0 ? (n5 / total * 100).ToString("#0.0") : "0");

        }

        /// <summary>
        /// 院内感染指标统计表
        /// </summary>
        /// <param name="doc"></param>
        private void Infectionperation(WordDocument doc)
        {
            DateTime date = DateTime.Parse("2016-05-01");
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            doc.ReplaceText("Org", org.Data.OrgName);
            doc.ReplaceText("Year", date.Year.ToString());
            doc.ReplaceText("Month", date.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var list = reportManageService.GetInfection(date);
            //doc.ReplaceText("totalB",list.Sum(o=>o.Total).ToString());
            //var keys = new[] { "001", "002", "003", "004", "005" };
            //for (int i = 0; i < keys.Length; i++)
            //{
            //    var obj = list.FirstOrDefault(o => o.Type == keys[i]);
            //    doc.ReplaceText(i.ToString(), obj == null ? "" : obj.Total.ToString());
            //}
            //doc.ReplaceText("totalC", list.Where(o=>o.Type==keys[0] || o.Type==keys[1]).Sum(o => o.Total).ToString());
            //doc.ReplaceText("totalD", list.Where(o => o.Type == keys[2] || o.Type == keys[3]).Sum(o => o.Total).ToString());
            //var constraintList = reportManageService.GetInfectionIndList(date);
            //string key = string.Empty;

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

        private void BindData(object obj, WordDocument doc)
        {
            var t = obj.GetType();
            foreach (var field in t.GetProperties())
            {
                if (field.PropertyType == typeof(DateTime?) || field.PropertyType == typeof(DateTime))
                {
                    doc.ReplaceText(field.Name, field.GetValue(obj) == null ? "" : ((DateTime)field.GetValue(obj)).ToString("yyyy-MM-dd"));
                }
                else
                {
                    doc.ReplaceText(field.Name, field.GetValue(obj) == null ? "" : field.GetValue(obj).ToString());
                }

            }
        }

        private  DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        private void BindData(object obj, WordDocument doc, Dictionary<string, string> dict)
        {
            var t = obj.GetType();
            foreach (var field in t.GetProperties())
            {
                if (field.PropertyType == typeof(DateTime?) || field.PropertyType == typeof(DateTime))
                {
                    dict.Add(field.Name, field.GetValue(obj) == null ? "" : ((DateTime)field.GetValue(obj)).ToString("yyyy-MM-dd"));
                }
                else
                {
                    dict.Add(field.Name, field.GetValue(obj) == null ? "" : field.GetValue(obj).ToString());
                }
            }
        }

        private void InitData(Type type, WordDocument doc)
        {
            foreach (var field in type.GetProperties())
            {
                doc.ReplaceText(field.Name, "");
            }
        }

        private void InitValue(int start,int end,WordDocument doc)
        {
            for (var i = start; i <= end; i++)
            {
                doc.ReplaceText("Value" + i, "");
            }
        }
    }
}

