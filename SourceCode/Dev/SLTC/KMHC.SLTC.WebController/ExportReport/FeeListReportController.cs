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
using System.Web.Mvc;
using System.Data;

namespace KMHC.SLTC.WebController.ExportReport
{
    public partial class EvalReportController : ReportBaseController
    {
        IBillV2Service billservice = IOCContainer.Instance.Resolve<IBillV2Service>(); 

        public ActionResult FeeListExport()
        {
            string templateName = Request["templateName"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            string feeNo = Request["feeNo"];
            //string floorId = Request["floorId"] ?? "";
            using (WordDocument doc = new WordDocument())
            {
                //加载模板
                doc.LoadModelDoc(templateName);
                switch (templateName)
                {
                    //费用清单列表
                    case "FeeListReport":
                        var FeeList = GetFeeData(feeNo, startDateStr, endDateStr);
                        
                        if (FeeList.Count > 0)
                        {
                            foreach (var aitem in FeeList)
                            {
                                if (aitem.regInformation != null && aitem.regInformation.Count > 0 && aitem.feeRecordList != null && aitem.feeRecordList.Count > 0)
                                {
                                    foreach (var bitem in FeeList)
                                    {
                                        if (aitem.regInformation[0].RegNO == bitem.regInformation[0].RegNO && aitem.regInformation[0].FeeNO != bitem.regInformation[0].FeeNO)
                                        {
                                            foreach (var feeItem in bitem.feeRecordList)
                                            {
                                                aitem.feeRecordList.Add(feeItem);
                                            }
                                            bitem.regInformation = null;
                                            bitem.feeRecordList = null;
                                        }

                                    }
                                }

                            }
                            foreach (var item in FeeList)
                            {
                                if (item.regInformation != null && item.regInformation.Count>0 && item.feeRecordList != null && item.feeRecordList.Count>0)
                                {
                                    doc.NewPartDocument();
                                    doc.ReplacePartText("@Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
                                    doc.ReplacePartText("@Name", item.regInformation[0].Name ?? "");
                                    if (item.regInformation[0].Gender == null)
                                    { doc.ReplacePartText("@Sex", item.regInformation[0].Gender ?? ""); }
                                    else
                                    { doc.ReplacePartText("@Sex", item.regInformation[0].Gender == "M" ? "男" : "女"); }
                                    doc.ReplacePartText("@Age", GetAge(item.regInformation[0].Birthday ?? DateTime.Now, DateTime.Now));
                                    doc.ReplacePartText("@sDate", item.regInformation[0].STime.ToString("yyyy-MM") ?? "");
                                    doc.ReplacePartText("@eDate", item.regInformation[0].ETime.ToString("yyyy-MM") ?? "");
                                    doc.ReplacePartText("@InHosDays", item.regInformation[0].InHosDays.ToString() ?? "");
                                    doc.ReplacePartText("@ResNo", item.regInformation[0].ResidentNo ?? "");
                                    doc.ReplacePartText("@InHosCount", item.regInformation[0].InHosCount.ToString() ?? "");
                                    doc.ReplacePartText("@Floor", item.regInformation[0].Floor ?? "");
                                    doc.ReplacePartText("@RoomNo", item.regInformation[0].RoomNo ?? "");
                                    doc.ReplacePartText("@BedNo", item.regInformation[0].BedNo ?? "");
                                    decimal totalCost = 0;
                                    DataTable dt = new DataTable();
                                    //dt.Columns.Add("c1");
                                    dt.Columns.Add("c2", typeof(string));
                                    dt.Columns.Add("c3", typeof(decimal));
                                    dt.Columns.Add("c4", typeof(string));
                                    //dt.Columns.Add("c5");
                                    dt.Columns.Add("c6", typeof(decimal));
                                    dt.Columns.Add("c7", typeof(decimal));
                                    foreach (var fItem in item.feeRecordList)
                                    {
                                        var dr = dt.NewRow();
                                        //dr["c1"] = fItem.MCCode;
                                        dr["c2"] = fItem.ProjectName;
                                        dr["c3"] = fItem.UnitPrice;
                                        dr["c4"] = fItem.Units;
                                        //dr["c5"] = fItem.Spec;
                                        dr["c6"] = fItem.Count;
                                        dr["c7"] = fItem.Cost;
                                        totalCost += fItem.Cost;
                                        dt.Rows.Add(dr);
                                    }
                                    var query = dt.AsEnumerable()
                                                  .GroupBy(t => t.Field<string>("c2"))
                                                  .Select(g => new
                                                  {
                                                      c2 = g.Key,
                                                      c3 = g.First().Field<decimal>("c3"),
                                                      c4 = g.First().Field<string>("c4"),
                                                      c6 = g.Sum(m => m.Field<decimal>("c6")),
                                                      c7 = g.Sum(m => m.Field<decimal>("c7")),
                                                  }).OrderByDescending(x => x.c2);

                                    DataTable dtResult = dt.Clone();
                                    query.ToList().ForEach(q => dtResult.Rows.Add(q.c2, q.c3, q.c4, q.c6, q.c7));
                                    DataRow totaldr = dtResult.NewRow();
                                    totaldr["c2"] = "合计";
                                    totaldr["c7"] = totalCost;
                                    dtResult.Rows.Add(totaldr);
                                    doc.FillPartTable(0, dtResult, "", "", 1);
                                    dt.Dispose();
                                    doc.AddPartDocument();
                                }
                            }
                        }
                        break;
                    

                }
                if (!doc.IsDocNull())
                {
                    Util.DownloadFile(doc.SaveDoc(templateName));
                }
            }

            return View();
        }

        public ActionResult AllFeeListExport() 
        {
            string templateName = Request["templateName"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            using (WordDocument doc = new WordDocument())
            {
                //加载模板
                doc.LoadModelDoc(templateName);
                switch (templateName)
                {
                    //费用清单列表
                    case "FeeListReport":
                        var FeeList = GetFeeData(startDateStr, endDateStr);

                        if (FeeList.Count > 0)
                        {
                            foreach (var aitem in FeeList)
                            {
                                if (aitem.regInformation != null && aitem.regInformation.Count > 0 && aitem.feeRecordList != null && aitem.feeRecordList.Count > 0)
                                {
                                    foreach (var bitem in FeeList)
                                    {
                                        if (aitem.regInformation[0].RegNO == bitem.regInformation[0].RegNO && aitem.regInformation[0].FeeNO != bitem.regInformation[0].FeeNO)
                                        {
                                            foreach (var feeItem in bitem.feeRecordList)
                                            {
                                                aitem.feeRecordList.Add(feeItem);
                                            }
                                            bitem.regInformation = null;
                                            bitem.feeRecordList = null;
                                        }

                                    }
                                }
                            
                            }
                            foreach (var item in FeeList)
                            {
                                if (item.regInformation != null && item.regInformation.Count > 0 && item.feeRecordList != null && item.feeRecordList.Count > 0)
                                {
                                    doc.NewPartDocument();
                                    doc.ReplacePartText("@Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
                                    doc.ReplacePartText("@Name", item.regInformation[0].Name ?? "");
                                    if (item.regInformation[0].Gender==null)
                                    { doc.ReplacePartText("@Sex", item.regInformation[0].Gender ?? ""); }
                                    else
                                    { doc.ReplacePartText("@Sex", item.regInformation[0].Gender == "M" ? "男" : "女"); }
                                    
                                    doc.ReplacePartText("@Age", GetAge(item.regInformation[0].Birthday ?? DateTime.Now, DateTime.Now));
                                    doc.ReplacePartText("@sDate", item.regInformation[0].STime.ToString("yyyy-MM") ?? "");
                                    doc.ReplacePartText("@eDate", item.regInformation[0].ETime.ToString("yyyy-MM") ?? "");
                                    doc.ReplacePartText("@InHosDays", item.regInformation[0].InHosDays.ToString() ?? "");
                                    doc.ReplacePartText("@ResNo", item.regInformation[0].ResidentNo ?? "");
                                    doc.ReplacePartText("@InHosCount", item.regInformation[0].InHosCount.ToString() ?? "");
                                    doc.ReplacePartText("@Floor", item.regInformation[0].Floor ?? "");
                                    doc.ReplacePartText("@RoomNo", item.regInformation[0].RoomNo ?? "");
                                    doc.ReplacePartText("@BedNo", item.regInformation[0].BedNo ?? "");
                                    decimal totalCost = 0;
                                    DataTable dt = new DataTable();
                                    //dt.Columns.Add("c1");
                                    dt.Columns.Add("c2", typeof(string));
                                    dt.Columns.Add("c3", typeof(decimal));
                                    dt.Columns.Add("c4", typeof(string));
                                    //dt.Columns.Add("c5");
                                    dt.Columns.Add("c6", typeof(decimal));
                                    dt.Columns.Add("c7", typeof(decimal));
                                    foreach (var fItem in item.feeRecordList)
                                    {
                                        var dr = dt.NewRow();
                                        //dr["c1"] = fItem.MCCode;
                                        dr["c2"] = fItem.ProjectName;
                                        dr["c3"] = fItem.UnitPrice;
                                        dr["c4"] = fItem.Units;
                                        //dr["c5"] = fItem.Spec;
                                        dr["c6"] = fItem.Count;
                                        dr["c7"] = fItem.Cost;
                                        totalCost += fItem.Cost;
                                        dt.Rows.Add(dr);
                                    }
                                    var query = dt.AsEnumerable()
                                                  .GroupBy(t => t.Field<string>("c2"))
                                                  .Select(g => new
                                                  {
                                                      c2 = g.Key,
                                                      c3 = g.First().Field<decimal>("c3"),
                                                      c4 = g.First().Field<string>("c4"),
                                                      c6 = g.Sum(m => m.Field<decimal>("c6")),
                                                      c7 = g.Sum(m => m.Field<decimal>("c7")),
                                                  }).OrderByDescending(x => x.c2);

                                    DataTable dtResult = dt.Clone();
                                    query.ToList().ForEach(q => dtResult.Rows.Add(q.c2, q.c3, q.c4, q.c6, q.c7));
                                    DataRow totaldr = dtResult.NewRow();
                                    totaldr["c2"] = "合计";
                                    totaldr["c7"] = totalCost;
                                    dtResult.Rows.Add(totaldr);
                                    doc.FillPartTable(0, dtResult, "", "", 1);
                                    dt.Dispose();
                                    doc.AddPartDocument();
                                }
                            }
                        }
                        break;
                    

                }
                if (!doc.IsDocNull())
                {
                    Util.DownloadFile(doc.SaveDoc(templateName));
                }
            }

            return View();
        }

        public List<BillV2FeeList> GetFeeData(string _feeNo, string _startDate, string _endDate)
        {
            var feeNo = Convert.ToInt32(_feeNo ?? "0");
            DateTime startDate = DateTime.MinValue ;
            DateTime endDate = DateTime.MinValue;
            if (_startDate != "") startDate = Convert.ToDateTime(_startDate);
            if (_endDate != "") endDate = Convert.ToDateTime(_endDate);
            return billservice.QueryBillV2FeeListReport(feeNo, startDate, endDate);
        }
        public List<BillV2FeeList> GetFeeData(string _startDate, string _endDate)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            if (_startDate != "") startDate = Convert.ToDateTime(_startDate);
            if (_endDate != "") endDate = Convert.ToDateTime(_endDate);
            return billservice.QueryBillV2AllFeeListReport(startDate, endDate);
        }


        #region 公共方法
        ////获取年龄
        //public static string GetAge(DateTime dtBirthday, DateTime dtNow)
        //{
        //    string strAge = string.Empty;                         // 年龄的字符串表示
        //    int intYear = 0;                                    // 岁
        //    int intMonth = 0;                                    // 月
        //    int intDay = 0;                                    // 天
        //    if (dtBirthday == dtNow)
        //    {
        //        return string.Empty;
        //    }
        //    // 如果没有设定出生日期, 返回空
        //    if (string.IsNullOrEmpty(dtBirthday.ToString()))
        //    {
        //        return string.Empty;
        //    }

        //    // 计算天数
        //    intDay = dtNow.Day - dtBirthday.Day;
        //    if (intDay < 0)
        //    {
        //        dtNow = dtNow.AddMonths(-1);
        //        intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
        //    }

        //    // 计算月数
        //    intMonth = dtNow.Month - dtBirthday.Month;
        //    if (intMonth < 0)
        //    {
        //        intMonth += 12;
        //        dtNow = dtNow.AddYears(-1);
        //    }

        //    // 计算年数
        //    intYear = dtNow.Year - dtBirthday.Year;

        //    // 格式化年龄输出
        //    if (intYear >= 1)                                            // 年份输出
        //    {
        //        strAge = intYear.ToString() + "岁";
        //    }

        //    if (intMonth > 0 && intYear <= 5)                           // 五岁以下可以输出月数
        //    {
        //        strAge += intMonth.ToString() + "月";
        //    }

        //    if (intDay >= 0 && intYear < 1)                              // 一岁以下可以输出天数
        //    {
        //        if (strAge.Length == 0 || intDay > 0)
        //        {
        //            strAge += intDay.ToString() + "日";
        //        }
        //    }

        //    return strAge;
        //}
        #endregion
    }
}

