using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/Report")]
    public class ReportController : ApiController
    {
        public IHttpActionResult View()
        {
            string dir = HttpContext.Current.Server.MapPath(VirtualPathUtility.GetDirectory("~"));
            FlexPaperConfig configManager = new FlexPaperConfig(dir);
            string doc = HttpContext.Current.Request["doc"];
            string page = HttpContext.Current.Request["page"];

            string swfFilePath = configManager.getConfig("path.swf") + doc + page + ".swf";
            string pdfFilePath = configManager.getConfig("path.pdf") + doc;
            if (!Util.validPdfParams(pdfFilePath, doc, page))
            {
                HttpContext.Current.Response.Write("[Incorrect file specified]");
            }
            else
            {
                String output = new pdf2swf(dir).convert(doc, page);
                if (output.Equals("[Converted]"))
                {
                    if (configManager.getConfig("allowcache") == "true")
                    {
                        Util.setCacheHeaders(HttpContext.Current);
                    }

                    HttpContext.Current.Response.AddHeader("Content-type", "application/x-shockwave-flash");
                    HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                    HttpContext.Current.Response.AddHeader("Content-Length", new System.IO.FileInfo(swfFilePath).Length.ToString());

                    HttpContext.Current.Response.WriteFile(swfFilePath);
                }
                else
                {
                    HttpContext.Current.Response.Write(output);
                }
            }
            HttpContext.Current.Response.End();
            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }
            if (File.Exists(swfFilePath))
            {
                File.Delete(swfFilePath);
            }
            return Ok();
        }

        [HttpGet, Route("{code}")]
        public IHttpActionResult DownloadExcel(string code, long feeNo = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            string mapPath = HttpContext.Current.Server.MapPath(VirtualPathUtility.GetDirectory("~"));
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            string path = string.Empty;
            switch (code)
            {
                case "H20":
                    if (startDate.HasValue) {
                        path = string.Format(@"{0}Templates\{1}.xls", mapPath, "H20");
                        reportManageService.DownloadUnPlanedIpdStatisticsFile(startDate.Value.Year, path);
                    }
                    break;
                case "H49":
                    if (startDate.HasValue) {
                        path = string.Format(@"{0}Templates\{1}.xls", mapPath, "H49");
                        reportManageService.DownloadInfectionStatisticsFile(startDate.Value.Year, path);
                    }
                    break;
                case "H50":
                    if (startDate.HasValue) {
                        path = string.Format(@"{0}Templates\{1}.xls", mapPath, "H50");
                        reportManageService.DownloadH50(startDate.Value.Year, path);
                    }
                    break;
                case "H76":
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        path = string.Format(@"{0}Templates\{1}.xls", mapPath, "H76");
                        reportManageService.DownloadPrsSoreRisk(path, feeNo, startDate, endDate);
                    }
                    break;
                case "H77":
                    path = string.Format(@"{0}Templates\{1}.xls", mapPath, "H77");
                    reportManageService.DownloadH77(feeNo, path, startDate, endDate);
                    break;
                case "H79":
                    path = string.Format(@"{0}Templates\{1}.xls", mapPath, "H79");
                    reportManageService.DownloadH79(feeNo, path);
                    break;

                case "P19":
                    path = string.Format(@"{0}Templates\{1}.xls", mapPath, "P19");
                    int seqNo = 0;
                    if (int.TryParse(HttpContext.Current.Request["seqNo"], out seqNo)) {
                        reportManageService.DownloadP19(seqNo, path);
                    }
                    break;
                case "View":
                    this.View();
                    break;
            }
            //HttpContext.Current.Response.AddHeader("Content-type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
            //HttpContext.Current.Response.AddHeader("Content-Length", new System.IO.FileInfo(swfFilePath).Length.ToString());
            //HttpContext.Current.Response.WriteFile(swfFilePath);

            //HttpContext.Current.Response.End();
            return Ok();
        }

    }
}