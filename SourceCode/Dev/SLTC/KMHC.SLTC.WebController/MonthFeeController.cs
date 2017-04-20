using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;

namespace KMHC.SLTC.WebController
{
    public class MonthFeeController : Controller
    {
        public async Task<ActionResult> Preview(string date, long? nSMonFeeID = null)
        {
            if (nSMonFeeID.HasValue)
            {
                await this.GeneratePDF("MonthFee", this.Operation, date, nSMonFeeID.Value);
            }
            else
            {
                await this.GeneratePDF("MonthFee", this.Operation, date, nSMonFeeID);
            }
            return View("Preview");
        }
        protected string GetOrgName(string orgId)
        {
            IOrganizationManageService organizationManageService = IOCContainer.Instance.Resolve<IOrganizationManageService>();
            var org = organizationManageService.GetOrg(orgId);
            return org.Data == null ? "" : org.Data.OrgName;
        }
        private void CalculateMonStartEndTime(string month, out DateTime startTime, out DateTime endTime)
        {
            startTime = DateTime.Parse(month + "-01 00:00:00");
            endTime = startTime.AddMonths(1).AddSeconds(-1);
        }
        protected async System.Threading.Tasks.Task Operation(WordDocument doc, string date, long? nSMonFeeID)
        {
            IBillV2Service service = IOCContainer.Instance.Resolve<IBillV2Service>();
            List<PrintMonthFee> list = new List<PrintMonthFee>();
            if (nSMonFeeID.HasValue)
            {
                var result = await HttpClientHelper.LtcHttpClient.GetAsync("/api/MonthFee/GetResMonthData?NSMonFeeID=" + nSMonFeeID.Value + "&currentPage=1&pageSize=1000");
                var res = result.Content.ReadAsAsync<BaseResponse<IList<ResidentMonFeeModel>>>().Result;
                list = service.GetPrintData(res);
            }
            else
            {
                BaseRequest<MonthFeeFilter> request = new BaseRequest<MonthFeeFilter>() { CurrentPage = 1, PageSize = 1000, Data = { date = date } };
                list = service.GetPrintData(request);
            }
            DateTime sDate, eDate;
            CalculateMonStartEndTime(date, out sDate, out eDate);
            doc.ReplaceText("sDate", sDate.ToString("yyyy年MM月dd日"));
            doc.ReplaceText("eDate", eDate.ToString("yyyy年MM月dd日"));
            doc.ReplaceText("Year", eDate.ToString("yyyy"));
            IDictManageService dictManageService = IOCContainer.Instance.Resolve<IDictManageService>();
            CodeFilter codeFilter = new CodeFilter();
            codeFilter.ItemTypes = new string[] { "A00.001", "A00.052" };
            var dict = (List<CodeValue>)dictManageService.QueryCode(codeFilter).Data;
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
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
            dt.Columns.Add("c13");
            dt.Columns.Add("c14");
            dt.Columns.Add("c15");
            int i = 1;
            foreach (PrintMonthFee monFee in list)
            {
                var dr = dt.NewRow();
                dr["c1"] = i++;
                dr["c2"] = monFee.Name;
                dr["c3"] = dict.Find(it => it.ItemType == "A00.001" && it.ItemCode == monFee.Sex) != null ?
                dict.Find(it => it.ItemType == "A00.001" && it.ItemCode == monFee.Sex).ItemName : "";
                dr["c4"] = monFee.ResidentSSId;
                dr["c5"] = monFee.BrithPlace;
                dr["c6"] = dict.Find(it => it.ItemType == "A00.052" && it.ItemCode == monFee.RsStatus) != null ?
                dict.Find(it => it.ItemType == "A00.052" && it.ItemCode == monFee.RsStatus).ItemName : "";
                dr["c7"] = monFee.DiseaseDiag;
                dr["c8"] = monFee.CareTypeId.Replace("1", "专护").Replace("2", "专护").Replace("3", "机构护理");
                dr["c9"] = monFee.CertStartTime == null ? null : monFee.CertStartTime.Value.ToString("yyyy-MM-dd");
                dr["c10"] = monFee.InDate == null ? null : monFee.InDate.Value.ToString("yyyy-MM-dd");
                dr["c11"] = monFee.OutDate == null ? null : monFee.OutDate.Value.ToString("yyyy-MM-dd");
                dr["c12"] = monFee.HospDay;
                dr["c13"] = monFee.TotalAmount;
                dr["c14"] = monFee.NCIPayLevel;
                dr["c15"] = monFee.NCIPay;
                dt.Rows.Add(dr);
            }

            doc.FillTable(0, dt, "", "", 1);
        }
        public async System.Threading.Tasks.Task GeneratePDF(string templateName, Func<WordDocument, string, long?, System.Threading.Tasks.Task> docOperation, string date, long? nSMonFeeID)
        {
            using (WordDocument doc = new WordDocument())
            {
                doc.Load(templateName);
                await docOperation(doc, date, nSMonFeeID);
                ViewBag.StartDocument = doc.SavePDF();
            }
        }
    }
}
