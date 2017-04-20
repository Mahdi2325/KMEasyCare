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

namespace KMHC.SLTC.WebController.ExportReport
{
    public partial class EvalReportController : ReportBaseController
    {
        IEvalReportService service = IOCContainer.Instance.Resolve<IEvalReportService>();

        public ActionResult Export()
        {
            string templateName = Request["templateName"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            string feeNo = Request["feeNo"];
            string floorId = Request["floorId"] ?? "";
            using (WordDocument doc = new WordDocument())
            {

                if (templateName == "PillReport") return View();
                //加载模板
                doc.LoadModelDoc(templateName);
                switch (templateName)
                {
                    //巴氏量表评估
                    case "ADLReport":
                        var adlList = GetEvalData(feeNo, startDateStr, endDateStr, "ADL", floorId);
                        if (adlList.Count > 0)
                        {
                            foreach (var item in adlList)
                            {
                                doc.NewPartDocument();
                                doc.ReplacePartText("Org", item.Org ?? "");
                                doc.ReplacePartText("Name", item.Name ?? "");
                                doc.ReplacePartText("ResidengNo", item.ResidengNo??"");
                                doc.ReplacePartText("Area", item.Area ?? "");
                                doc.ReplacePartText("BedNo", item.BedNo ?? "");
                                doc.ReplacePartText("Age", GetAge(item.Brithdate ?? DateTime.Now, DateTime.Now));
                                doc.ReplacePartText("CreateDate", item.CreateDate.ToString() ?? "");
                                doc.ReplacePartText("NextDate", item.NextDate.ToString() ?? "");
                                doc.ReplacePartText("EvaluateBy", item.EvaluateBy ?? "");
                                doc.ReplacePartText("Result", item.Result ?? "");
                                doc.ReplacePartText("Score", item.Score.ToString() ?? "");
                                var answers = item.answer.ToList();
                                for (var i = 31; i <= 40; i++)
                                {
                                    var answer = answers.Find(o => o.Id == i);
                                    doc.ReplacePartText("Value" + i, answer != null ? answer.Value : "未填");
                                }
                                doc.AddPartDocument();
                            }
                        }
                        break;
                    //柯氏量表评估
                    case "ColeScaleReport":
                        var karnofskyList = GetEvalData(feeNo, startDateStr, endDateStr, "KARNOFSKY", floorId);
                        if (karnofskyList.Count > 0)
                        {
                            foreach (var item in karnofskyList)
                            {
                                doc.NewPartDocument();
                                doc.ReplacePartText("Org", item.Org ?? "");
                                doc.ReplacePartText("Name", item.Name ?? "");
                                doc.ReplacePartText("ResidengNo", item.ResidengNo ?? "");
                                doc.ReplacePartText("CDTW", item.CDTW ?? "");
                                doc.ReplacePartText("NDTW", item.NDTW ?? "");
                                doc.ReplacePartText("EvaluateBy", item.EvaluateBy ?? "");
                                doc.ReplacePartText("Result", item.Result ?? "");
                                doc.ReplacePartText("OneEvaluateTotal", item.OneEvaluateTotal.ToString() ?? "");
                                doc.ReplacePartText("EvaluateTotal", item.EvaluateTotal.ToString() ?? "");
                                var answers = item.answer.ToList();
                                for (var i = 66; i <= 66; i++)
                                {
                                    var answer = answers.Find(o => o.Id == i);
                                    doc.ReplacePartText("Value" + i, answer != null ? answer.Value : "未填");
                                }
                                doc.AddPartDocument();
                            }
                        }
                        break;
                    //压疮风险评估
                    case "PrsSoreReport":
                        var soreList = GetEvalData(feeNo, startDateStr, endDateStr, "SORE", floorId);
                        if (soreList.Count > 0)
                        {
                            foreach (var item in soreList)
                            {
                                doc.NewPartDocument();
                                doc.ReplacePartText("Org", item.Org ?? "");
                                doc.ReplacePartText("Name", item.Name ?? "");
                                doc.ReplacePartText("ResidengNo", item.ResidengNo ?? "");
                                doc.ReplacePartText("Area", item.Area ?? "");
                                doc.ReplacePartText("BedNo", item.BedNo ?? "");
                                doc.ReplacePartText("Age", GetAge(item.Brithdate ?? DateTime.Now, DateTime.Now));
                                doc.ReplacePartText("CDTW", item.CDTW ?? "");
                                doc.ReplacePartText("NDTW", item.NDTW ?? "");
                                doc.ReplacePartText("EvaluateBy", item.EvaluateBy ?? "");
                                doc.ReplacePartText("Score", item.Score.ToString() ?? "");
                                doc.ReplacePartText("Result", item.Result ?? "");
                                doc.ReplacePartText("OneEvaluateTotal", item.OneEvaluateTotal.ToString() ?? "");
                                doc.ReplacePartText("EvaluateTotal", item.EvaluateTotal.ToString() ?? "");
                                var answers = item.answer.ToList();
                                for (var i = 100; i <= 105; i++)
                                {
                                    var answer = answers.Find(o => o.Id == i);
                                    doc.ReplacePartText("Value" + i, answer != null ? answer.Value : "未填");
                                }
                                doc.AddPartDocument();
                            }
                        }
                        break;
                    //跌倒风险评估
                    case "FallReport":
                        var fallList = GetEvalData(feeNo, startDateStr, endDateStr, "FALL", floorId);
                        if (fallList.Count > 0)
                        {
                            foreach (var item in fallList)
                            {
                                doc.NewPartDocument();
                                doc.ReplacePartText("Org", item.Org ?? "");
                                doc.ReplacePartText("Name", item.Name ?? "");
                                doc.ReplacePartText("ResidengNo", item.ResidengNo ?? "");
                                doc.ReplacePartText("Area", item.Area ?? "");
                                doc.ReplacePartText("BedNo", item.BedNo ?? "");
                                doc.ReplacePartText("Age", GetAge(item.Brithdate ?? DateTime.Now, DateTime.Now));
                                doc.ReplacePartText("CDTW", item.CDTW ?? "");
                                doc.ReplacePartText("NDTW", item.NDTW ?? "");
                                doc.ReplacePartText("EvaluateBy", item.EvaluateBy ?? "");
                                doc.ReplacePartText("Score", item.Score.ToString() ?? "");
                                doc.ReplacePartText("Result", item.Result ?? "");
                                doc.ReplacePartText("OneEvaluateTotal", item.OneEvaluateTotal.ToString() ?? "");
                                doc.ReplacePartText("EvaluateTotal", item.EvaluateTotal.ToString() ?? "");
                                var answers = item.answer.ToList();
                                for (var i = 106; i <= 116; i++)
                                {
                                    var answer = answers.Find(o => o.Id == i);
                                    doc.ReplacePartText("Value" + i, answer != null ? answer.Value : "未填");
                                }
                                doc.AddPartDocument();
                            }
                        }
                        break;
                    //给药记录单
                    case "PillReport":
                        break;
                    //工具性日常生活功能量表评估
                    case "IADLReport":
                        var iadlList = GetEvalData(feeNo, startDateStr, endDateStr, "IADL", floorId);
                        if (iadlList.Count > 0)
                        {
                            foreach (var item in iadlList)
                            {
                                doc.NewPartDocument();
                                doc.ReplacePartText("Org", item.Org ?? "");
                                doc.ReplacePartText("Name", item.Name ?? "");
                                doc.ReplacePartText("ResidengNo", item.ResidengNo ?? "");
                                doc.ReplacePartText("Area", item.Area ?? "");
                                doc.ReplacePartText("BedNo", item.BedNo ?? "");
                                doc.ReplacePartText("Age", GetAge(item.Brithdate ?? DateTime.Now, DateTime.Now));
                                doc.ReplacePartText("CreateDate", item.CreateDate.ToString() ?? "");
                                doc.ReplacePartText("NextDate", item.NextDate.ToString() ?? "");
                                doc.ReplacePartText("EvaluateBy", item.EvaluateBy ?? "");
                                doc.ReplacePartText("Score", item.Score.ToString() ?? "");
                                doc.ReplacePartText("Result", item.Result ?? "");
                                var answers = item.answer.ToList();
                                for (var i = 58; i <= 65; i++)
                                {
                                    var answer = answers.Find(o => o.Id == i);
                                    doc.ReplacePartText("Value" + i, answer != null ? answer.Value : "未填");
                                }
                                doc.AddPartDocument();
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

        public List<EvalReportHeader> GetEvalData(string _feeNo, string _startDate, string _endDate, string code, string floorId)
        {
            var feeNo = Convert.ToInt32(_feeNo ?? "0");
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (_startDate != "") startDate = Convert.ToDateTime(_startDate);
            if (_endDate != "") endDate = Convert.ToDateTime(_endDate);
            return service.GetEvalData(feeNo, startDate, endDate, code, floorId);
        }



        #region 公共方法
        //获取年龄
        public static string GetAge(DateTime dtBirthday, DateTime dtNow)
        {
            string strAge = string.Empty;                         // 年龄的字符串表示
            int intYear = 0;                                    // 岁
            int intMonth = 0;                                    // 月
            int intDay = 0;                                    // 天
            if (dtBirthday == dtNow)
            {
                return string.Empty;
            }
            // 如果没有设定出生日期, 返回空
            if (string.IsNullOrEmpty(dtBirthday.ToString()))
            {
                return string.Empty;
            }

            // 计算天数
            intDay = dtNow.Day - dtBirthday.Day;
            if (intDay < 0)
            {
                dtNow = dtNow.AddMonths(-1);
                intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            }

            // 计算月数
            intMonth = dtNow.Month - dtBirthday.Month;
            if (intMonth < 0)
            {
                intMonth += 12;
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - dtBirthday.Year;

            // 格式化年龄输出
            if (intYear >= 1)                                            // 年份输出
            {
                strAge = intYear.ToString() + "岁";
            }

            if (intMonth > 0 && intYear <= 5)                           // 五岁以下可以输出月数
            {
                strAge += intMonth.ToString() + "月";
            }

            if (intDay >= 0 && intYear < 1)                              // 一岁以下可以输出天数
            {
                if (strAge.Length == 0 || intDay > 0)
                {
                    strAge += intDay.ToString() + "日";
                }
            }

            return strAge;
        }
        #endregion
    }
}

