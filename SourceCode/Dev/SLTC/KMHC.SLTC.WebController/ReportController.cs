using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using KMHC.SLTC.Business.Implement.Report;

namespace KMHC.SLTC.WebController
{
    public partial class ReportController : Controller
    {
        public ActionResult Export()
        {
            string templateName = Request["templateName"];
            string keyStr = Request["key"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            if (templateName == null) return View();

            long key = 0;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            long.TryParse(keyStr, out key);
            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);
            var re = ReportFactory.CreateReport(templateName, key, startDate, endDate);
            re.Download();
            return View();
        }
        public ActionResult Preview()
        {
            string templateName = Request["templateName"];
            string keyStr = Request["key"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            string order = Request["order"];
            if (templateName == null) return View();

            long key = 0;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            long.TryParse(keyStr, out key);
            DateTime.TryParse(startDateStr, out startDate);
            DateTime.TryParse(endDateStr, out endDate);
            var re = ReportFactory.CreateReport(templateName, key, startDate, endDate,order);
            ViewBag.StartDocument = re.Preview();

            //switch (templateName)
            //{
            //    case "000_demo":
            //        this.GeneratePDF("000_demo", this.DemoDocOperation);
            //        break;
            //    case "H32社工服务记录表PS011":
            //        this.GeneratePDF("H32社工服务记录表PS011", this.H32Operation);
            //        break;
            //    case "MMSE失智量表":
            //        this.GeneratePDF("MMSE失智量表", this.MMSEOperation);
            //        break;
            //    case "ADL巴氏量表":
            //        //var re = ReportFactory.CreateReport("ADL巴氏量表", 1);
            //        //re.Preview();
            //        ViewBag.StartDocument=re.Preview();
            //        this.GeneratePDF("ADL巴氏量表", this.ADLOperation);             
            //        break;
            //    case "IADL表":
            //        this.GeneratePDF("IADL表", this.IADLOperation);
            //        break;
            //    case "P21非计画性转至怠性医院住院月统计表":
            //        this.GeneratePDF("P21非计画性转至怠性医院住院月统计表", this.P21Operation);
            //        break;
            //    case "P22非计画性转至怠性医院住院月统计表":
            //        this.GeneratePDF("P22非计画性转至怠性医院住院月统计表", this.P22Operation);
            //        break;
            //    case "P23医疗照顾相关感染指标月统计表":
            //        this.GeneratePDF("P23医疗照顾相关感染指标月统计表", this.P23Operation);
            //        break;
            //    case "P13社工定期处遇评估表":
            //        this.GeneratePDF("P13社工定期处遇评估表", this.P13Operation);
            //        break;
            //    case "P10跌倒危险因子评估":
            //        this.GeneratePDF(templateName, this.P10Operation);
            //        break;
            //    case "P11约束月统计表":
            //        this.GeneratePDF(templateName, this.P11Operation);
            //        break;
            //    case "P12忧郁量表":
            //        this.GeneratePDF(templateName, this.P12Operation);
            //        break;
            //    case "P14社工服务记录":
            //        this.GeneratePDF(templateName, this.P14Operation);
            //        break;
            //    case "P15个案日常生活记录":
            //        this.GeneratePDF(templateName, this.P15Operation);
            //        break;
            //    case "SPMSQ简易心智量表":
            //        this.GeneratePDF(templateName, this.SPMSQOperation);
            //        break;
            //    case "P17护理计划":
            //        this.GeneratePDF(templateName, this.P17Operation);
            //        break;
            //    case "压疮风险评估":
            //        this.GeneratePDF(templateName, this.PrsSoreOperation);
            //        break;
            //    case "柯氏评估":
            //        this.GeneratePDF(templateName, this.ColeScaleOperation);
            //        break;
            //    case "院内感染指标统计表":
            //        this.GeneratePDF(templateName, this.Infectionperation);
            //        break;
            //}
            return View();
        }

        private void GeneratePDF(string templateName, Action<WordDocument> docOperation)
        {
            using (WordDocument doc = new WordDocument())
            {
                doc.Load(templateName);
                docOperation(doc);
                ViewBag.StartDocument = doc.SavePDF();
            }
        }

        /// <summary>
        /// 演示word操作
        /// </summary>
        /// <param name="doc"></param>
        private void DemoDocOperation(WordDocument doc)
        {
            //入住72小时内肠胃道出血而非计划性转到急性医院住院人次
            //因肠胃道出血而非计划性转到急性医院住院人次
            DateTime now = DateTime.Now;
            doc.ReplaceText("year", now.Year.ToString());
            doc.ReplaceText("month", now.Month.ToString());
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("total");
            for (int i = 0; i < 5; i++)
            {
                var dr = dt.NewRow();
                dr["name"] = i.ToString() + "_入住72小时内肠胃道出血而非计划性转到急性医院住院人次";
                dr["total"] = i.ToString();
                dt.Rows.Add(dr);
            }
            doc.FillTable(0, dt);
            doc.FillChartData(0, dt, 6);
            dt.Clear();
            for (int i = 0; i < 5; i++)
            {
                var dr = dt.NewRow();
                dr["name"] = "因肠胃道出血而非计划性转到急性医院住院人次";
                dr["total"] = i.ToString();
                dt.Rows.Add(dr);
            }
            doc.FillTable(1, dt);
            string mapPath = Server.MapPath(VirtualPathUtility.GetDirectory("~"));
            string imagePath = string.Format(@"{0}Images\{1}", mapPath, "002.jpg");
            doc.InsertImage("InsertImage", imagePath, 300, 600);
        }

        /// <summary>
        /// H32社工服务记录表PS011
        /// </summary>
        /// <param name="doc"></param>
        private void H32Operation(WordDocument doc)
        {
            long feeNo = 7;
            int careSvrId = 18;
            IResidentManageService residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
            ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            ICarePlansManageService carePlansManageService = IOCContainer.Instance.Resolve<ICarePlansManageService>();
            var resident = residentManageService.GetResident(feeNo);
            if (resident.Data == null)
                return;
            var person = residentManageService.GetPerson(resident.Data.RegNo??0);
            var careSvr = socialWorkerManageService.GetCareSvrById(careSvrId);
            var emp = organizationManageService.GetEmployee(careSvr.Data.Carer);
            var diaPR = carePlansManageService.GetDiaPR("001", careSvr.Data.QuestionLevel);
            CodeFilter request = new CodeFilter();
            request.ItemTypes = new string[] { "K00.017", "E00.215", "E00.216", "E00.217" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(request).Data;
            
            doc.ReplaceText("Name", person.Data.Name);
            doc.ReplaceText("Number", feeNo.ToString());
            doc.ReplaceText("Inday", resident.Data.InDate.HasValue ? resident.Data.InDate.Value.ToString("yyyy-MM-dd") : "");
            doc.ReplaceText("Dormitory", resident.Data.BedNo);
            doc.ReplaceText("Evdate", careSvr.Data.RecDate.HasValue ? careSvr.Data.RecDate.Value.ToString("yyyy-MM-dd") : "");
            doc.ReplaceText("Evalman", emp.Data.EmpName);
            doc.ReplaceText("Place", dict.Find(it => it.ItemType == "K00.017" && it.ItemCode == careSvr.Data.SvrAddress).ItemName);
            doc.ReplaceText("Stype", dict.Find(it => it.ItemType == "E00.215" && it.ItemCode == careSvr.Data.SvrType).ItemName);
            doc.ReplaceText("Sman", careSvr.Data.SvrPeople);
            doc.ReplaceText("Treattype", dict.Find(it => it.ItemType == "E00.216" && it.ItemCode == careSvr.Data.RelationType).ItemName);
            doc.ReplaceText("Treatresult", dict.Find(it => it.ItemType == "E00.217" && it.ItemCode == careSvr.Data.EvalStatus).ItemName);
            doc.ReplaceText("Shour", careSvr.Data.EvalMinutes.HasValue ? ((decimal)careSvr.Data.EvalMinutes.Value / 60).ToString("F2") : "");
            doc.ReplaceText("H32_no", careSvrId.ToString());
            doc.ReplaceText("ProcessActivity", careSvr.Data.ProcessActivity);
            doc.ReplaceText("QuestionLevel", careSvr.Data.QuestionLevel);
            doc.ReplaceText("QuestionFocus", diaPR.Data.Find(it=>it.ItemCode == careSvr.Data.QuestionFocus).ItemName);
            doc.ReplaceText("QuestionDesc", careSvr.Data.QuestionDesc);
            doc.ReplaceText("TreatDesc", careSvr.Data.TreatDesc);
            doc.ReplaceText("EvalDesc", careSvr.Data.EvalDesc);
        }

        /// <summary>
        /// P10跌倒危险因子评估
        /// </summary>
        /// <param name="doc"></param>
        private void P10Operation(WordDocument doc)
        {
            int recordId = 9;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var question = reportManageService.GetQuestion(recordId);
            if (question == null) return;
            BindData(question, doc);
            var answers = reportManageService.GetAnswers(question.Id).ToList();

            for (var i = 129; i <= 139; i++)
            {
                var answer = answers.Find(o => o.Id == i);
                doc.ReplaceText("Value" + i, answer != null ? answer.Value : "未填");
            }
        }

        /// <summary>
        /// P11约束月统计表
        /// </summary>
        /// <param name="doc"></param>
        private void P11Operation(WordDocument doc)
        {
            DateTime date = DateTime.Parse("2016-04-01");
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            doc.ReplaceText("Org", org.Data.OrgName);
            doc.ReplaceText("Year", date.Year.ToString());
            doc.ReplaceText("Month", date.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var constraintList = reportManageService.GetConstraintList(date);
            string key = string.Empty;

            dynamic[] tags = new dynamic[] { 
                new { Type = "ExecReason", Value = "001", Text = "因预防跌倒而使用身体约束人数（bl)" },
                new { Type = "ExecReason", Value = "002", Text = "因预防自拔管路而使用身体约束人数（b2)" }, 
                new { Type = "ExecReason", Value = "003", Text = "因预防自伤而使用身体约束人数（b3)" }, 
                new { Type = "ExecReason", Value = "004", Text = "因行为紊乱而使用身体约束人数（b4)" },
                new { Type = "ExecReason", Value = "005", Text = "因协助治疗而使用身体约束人数（b5)" },
                new { Type = "ExecReason", Value = "006", Text = "因其他因素而使用身体约束人数（b6)" },
                new { Type = "Duration", Value = "002", Text = "约束持续大於4小时小於等於8小时人数（cl)" }, 
                new { Type = "Duration", Value = "003", Text = "约束持续大於8小时小於等於16小时人数（c2)" }, 
                new { Type = "Duration", Value = "004", Text = "约束持续大於16小时小於等於24小时人数（c3)" },
                new { Type = "Duration", Value = "005", Text = "约束持续大於24小时人数（c4)" },
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

        /// <summary>
        /// P12忧郁量表
        /// </summary>
        /// <param name="doc"></param>
        private void P12Operation(WordDocument doc)
        {
            int feeNo = 11;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var questionList = reportManageService.GetQuestionList(feeNo, 1);
            List<Dictionary<string, string>> list = new List<Dictionary<string,string>>();
            foreach (var question in questionList)
            {
                var dict = new Dictionary<string, string>();
                this.BindData(question, doc, dict);
                var answers = reportManageService.GetAnswers(question.Id).ToList();
                for (var i = 1; i <= 15; i++)
                {
                    var answer = answers.Find(o => o.Id == i);
                    dict.Add("Value" + i, answer != null ? answer.Value : "未填");
                }
                list.Add(dict);
            }
            doc.FillTable(0, list);
        }

        /// <summary>
        /// P14社工服务记录
        /// </summary>
        /// <param name="doc"></param>
        private void P14Operation(WordDocument doc)
        {
            long feeNo = 7;
            int careSvrId = 18;
            IResidentManageService residentManageService = IOCContainer.Instance.Resolve<IResidentManageService>();
            ISocialWorkerManageService socialWorkerManageService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            ICarePlansManageService carePlansManageService = IOCContainer.Instance.Resolve<ICarePlansManageService>();
            var resident = residentManageService.GetResident(feeNo);
            if(resident.Data==null) return;
            var person = residentManageService.GetPerson(resident.Data.RegNo ?? 0);
            var careSvr = socialWorkerManageService.GetCareSvrById(careSvrId);
            var emp = organizationManageService.GetEmployee(careSvr.Data.Carer);
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            var diaPR = carePlansManageService.GetDiaPR("001", careSvr.Data.QuestionLevel);
            CodeFilter request = new CodeFilter();
            request.ItemTypes = new string[] { "K00.017", "E00.215", "E00.216", "E00.217" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(request).Data;

            doc.ReplaceText("Org", org.Data.OrgName);
            doc.ReplaceText("Name", person.Data.Name);
            doc.ReplaceText("FeeNo", feeNo.ToString());
            //doc.ReplaceText("Inday", resident.Data.InDate.HasValue ? resident.Data.InDate.Value.ToString("yyyy-MM-dd") : "");
            doc.ReplaceText("RoomNo", resident.Data.RoomNo);
            doc.ReplaceText("Area", resident.Data.Floor);
            doc.ReplaceText("RecDate", careSvr.Data.RecDate.HasValue ? careSvr.Data.RecDate.Value.ToString("yyyy-MM-dd") : "");
            doc.ReplaceText("Carer", emp.Data.EmpName);
            doc.ReplaceText("SvrAddress", dict.Find(it => it.ItemType == "K00.017" && it.ItemCode == careSvr.Data.SvrAddress).ItemName);
            doc.ReplaceText("SvrType", dict.Find(it => it.ItemType == "E00.215" && it.ItemCode == careSvr.Data.SvrType).ItemName);
            doc.ReplaceText("SvrPeople", careSvr.Data.SvrPeople);
            doc.ReplaceText("RelationType", dict.Find(it => it.ItemType == "E00.216" && it.ItemCode == careSvr.Data.RelationType).ItemName);
            doc.ReplaceText("EvalStatus", dict.Find(it => it.ItemType == "E00.217" && it.ItemCode == careSvr.Data.EvalStatus).ItemName);
            doc.ReplaceText("EvalShour", careSvr.Data.EvalMinutes.HasValue ? ((decimal)careSvr.Data.EvalMinutes.Value / 60).ToString("F2") : "");
            //doc.ReplaceText("H32_no", careSvrId.ToString());
            doc.ReplaceText("ProcessActivity", careSvr.Data.ProcessActivity);
            //doc.ReplaceText("QuestionLevel", careSvr.Data.QuestionLevel);
            doc.ReplaceText("QuestionFocus", diaPR.Data.Find(it => it.ItemCode == careSvr.Data.QuestionFocus).ItemName);
            doc.ReplaceText("QuestionDesc", careSvr.Data.QuestionDesc);
            doc.ReplaceText("TreatDesc", careSvr.Data.TreatDesc);
            doc.ReplaceText("EvalDesc", careSvr.Data.EvalDesc);
        }

        /// <summary>
        /// P15个案日常生活记录
        /// </summary>
        /// <param name="doc"></param>
        private void P15Operation(WordDocument doc)
        {
            ISocialWorkerManageService socialWorkerService = IOCContainer.Instance.Resolve<ISocialWorkerManageService>();
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            BaseRequest<LifeRecordFilter> lifeRecordFilter = new BaseRequest<LifeRecordFilter>();
            lifeRecordFilter.CurrentPage = 1;
            lifeRecordFilter.PageSize = 1000;
            var org = organizationManageService.GetOrg(SecurityHelper.CurrentPrincipal.OrgId);
            var response = socialWorkerService.QueryLifeRecord(lifeRecordFilter);
            doc.ReplaceText("Org", org.Data.OrgName);

            CodeFilter codeFilter = new CodeFilter();
            codeFilter.ItemTypes = new string[] { "A00.400", "A00.401", "A00.402" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;
            
            DataTable dt = new DataTable();
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");
            dt.Columns.Add("c5");
            dt.Columns.Add("c6");
            dt.Columns.Add("c7");
            dt.Columns.Add("c8");
            dt.Columns.Add("c9");
            dt.Columns.Add("c10");
            dt.Columns.Add("c11");
            dt.Columns.Add("c12");
            if (response.Data != null)
            {
                CodeValue findItem;
                foreach (var item in response.Data)
                {
                    var dr = dt.NewRow();
                    dr["c1"] = item.Name;
                    dr["c2"] = item.FeeNo;
                    dr["c3"] = item.Floor + " " + item.RoomNo;
                    if (item.RecordDate.HasValue)
                    {
                        var data = item.RecordDate.Value;
                        dr["c4"] = string.Format("{0}/{1}/{2}", data.Year, data.Month, data.Day);
                    }
                    dr["c5"] = item.BodyTemp.HasValue ? item.BodyTemp.Value.ToString("N1") : "";
                    findItem =dict.Find(it => it.ItemType == "A00.400" && it.ItemCode == item.AmActivity);
                    dr["c10"] = findItem != null ? findItem.ItemName : "";
                    findItem = dict.Find(it => it.ItemType == "A00.401" && it.ItemCode == item.PmActivity);
                    dr["c11"] = findItem != null ? findItem.ItemName : "";
                    findItem = dict.Find(it => it.ItemType == "A00.403" && it.ItemCode == item.Comments);
                    dr["c12"] = findItem != null ? findItem.ItemName : "";
                    //dr["c1"] = item.Name;RecordByName
                    dt.Rows.Add(dr);
                }
            }
            doc.FillTable(0, dt, "", "" , 1);
            dt.Dispose();
        }

        /// <summary>
        /// P17护理计划
        /// </summary>
        /// <param name="doc"></param>
        private void P17Operation(WordDocument doc)
        {
            int seqNo = 6;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            var nscpl = reportManageService.GetNSCPLReportView(seqNo);
            doc.ReplaceText("Org", nscpl.Org);
            doc.ReplaceText("SeqNo", nscpl.SeqNo.ToString());
            doc.ReplaceText("StartDate", nscpl.StartDate);
            doc.ReplaceText("EndDate", nscpl.EndDate);
            doc.ReplaceText("RegName", nscpl.RegName);
            doc.ReplaceText("FeeNo", nscpl.FeeNo.ToString());
            doc.ReplaceText("Sex", nscpl.Sex);
            doc.ReplaceText("Age", nscpl.Age);
            doc.ReplaceText("EmpName", nscpl.EmpName);
            doc.ReplaceText("CpLevel", nscpl.CpLevel);
            doc.ReplaceText("CpDiag", nscpl.CpDiag);
            doc.ReplaceText("CpReason", nscpl.CpReason);
            doc.ReplaceText("NsDesc", nscpl.NsDesc);
            doc.ReplaceText("CpResult", nscpl.CpResult);
            doc.ReplaceText("TotalDays", nscpl.TotalDays);
            doc.ReplaceText("NscplGoal", nscpl.NscplGoal);
            doc.ReplaceText("NscplActivity", nscpl.NscplActivity);
            doc.ReplaceText("AssessValue", nscpl.AssessValue);
        }

        /// <summary>
        /// P18个护理记录
        /// </summary>
        /// <param name="doc"></param>
        private void P18Operation(WordDocument doc)
        {
            long feeNo = 8;
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            INursingWorkstationService nursingWorkstationService = IOCContainer.Instance.Resolve<INursingWorkstationService>();
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            var residentInfo = reportManageService.GetResidentInfo(feeNo);
            BaseRequest<NursingRecFilter> nursingRecFilter = new BaseRequest<NursingRecFilter>();
            nursingRecFilter.CurrentPage = 1;
            nursingRecFilter.PageSize = 1000;
            nursingRecFilter.Data.FeeNo = feeNo;
            var response = nursingWorkstationService.QueryNursingRec(nursingRecFilter);
            doc.ReplaceText("Org", residentInfo.Org);
            doc.ReplaceText("Name", residentInfo.Name);
            doc.ReplaceText("Sex", residentInfo.Sex);
            doc.ReplaceText("Age", residentInfo.Age.ToString());
            doc.ReplaceText("FeeNo", residentInfo.FeeNo.ToString());
            doc.ReplaceText("Floor", residentInfo.Floor);
            doc.ReplaceText("RoomNo", residentInfo.RoomNo);

            CodeFilter codeFilter = new CodeFilter();
            codeFilter.ItemTypes = new string[] { "J00.005" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;

            DataTable dt = new DataTable();
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");
            if (response.Data != null)
            {
                foreach (var item in response.Data)
                {
                    var dr = dt.NewRow();
                    var findItem = dict.Find(it => it.ItemType == "J00.005" && it.ItemCode == item.ClassType);
                    dr["c1"] = item.RecordDate.HasValue ? Util.ToTwDate(item.RecordDate.Value) : "";
                    dr["c2"] = string.Format("{0} {1}{2}", item.RecordDate.HasValue ? item.RecordDate.Value.ToString("HH:mm") : "", item.ClassType, findItem != null ? findItem.ItemName : "");
                    dr["c3"] = item.Content;
                    dr["c4"] = item.RecordNameBy;
                    dt.Rows.Add(dr);
                }
            }
            doc.FillTable(0, dt, "", "", 2);
            dt.Dispose();
        }
    }
}

