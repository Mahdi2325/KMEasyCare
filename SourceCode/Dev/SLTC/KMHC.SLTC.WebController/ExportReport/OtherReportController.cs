using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
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
        IResidentManageService rmService = IOCContainer.Instance.Resolve<IResidentManageService>();
        public ActionResult OtherExport()
        {
            string templateName = Request["templateName"];
            string startDateStr = Request["startDate"];
            string endDateStr = Request["endDate"];
            string feeNo = Request["feeNo"];
            string floorId = Request["floorId"] ?? "";
            using (WordDocument doc = new WordDocument())
            {
                //加载模板
                doc.LoadModelDoc(templateName);
                switch (templateName)
                {
                    //住民在院证明
                    case "HosProofReport":
                        var data = rmService.QueryRegHosProof(Convert.ToInt32(feeNo)).Data;
                        if (data!=null)
                        {        
                            var brithDate=" 年 月 日";
                            var inDate = " 年 月 日";
                            var printDate = " 年 月 日";
                            var sex = "";
                            if(data.BrithDay.HasValue)
                            {
                                brithDate = data.BrithDay.Value.Year.ToString() + "年" + data.BrithDay.Value.Month.ToString() + "月" + data.BrithDay.Value.Day.ToString() + "日";
                            }
                            if (data.InDate.HasValue)
                            {
                                inDate = data.InDate.Value.Year.ToString() + "年" + data.InDate.Value.Month.ToString() + "月" + data.InDate.Value.Day.ToString() + "日";
                            }
                            if (!string.IsNullOrEmpty(startDateStr))
                            {
                                var _printDate=Convert.ToDateTime(startDateStr);
                                printDate = (_printDate.Year-1911).ToString() + "年" + _printDate.Month.ToString() + "月" + _printDate.Day.ToString() + "日";
                            }
                            if (!string.IsNullOrEmpty(data.Sex))
                            {
                                if(data.Sex=="F")
                                {
                                    sex = "女";
                                }
                                else if (data.Sex == "M")
                                {
                                    sex="男";
                                }
                            }
                                doc.NewPartDocument();
                                doc.ReplacePartText("@ORG", data.Org ?? "");
                                doc.ReplacePartText("@Year", DateTime.Now.Year.ToString());
                                doc.ReplacePartText("@ResidengNo", data.ResidengNo ?? "");
                                doc.ReplacePartText("@Name", data.Name ?? "");
                                doc.ReplacePartText("@Sex",sex);
                                doc.ReplacePartText("@PermanentAddress", data.PermanentAddress ?? "");
                                doc.ReplacePartText("@IdNo", data.IdNo ?? "");
                                doc.ReplacePartText("@DisabilityGrade", data.DisabilityGrade ?? "");
                                doc.ReplacePartText("@BirthDate", brithDate);
                                doc.ReplacePartText("@InDate", inDate);
                                doc.ReplacePartText(" @PrintDate", printDate);
                                doc.AddPartDocument();
                        }
                        break;
                  

                }
                if (!doc.IsDocNull())
                {
                    Util.DownloadFile(doc.SaveDoc(templateName));
                }
            }

            return View("Export");
        }
    }
}

