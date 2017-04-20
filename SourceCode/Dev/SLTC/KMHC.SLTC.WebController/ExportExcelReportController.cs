#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :2017/3/27 
** 说明     :
******************************************************************/
#endregion

using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Implement.Report.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KMHC.SLTC.WebController
{
    public class ExportExcelReportController : Controller
    {
        public async Task<ActionResult> Export(string fileTypeStr, string templateName, string startTimeStr, string endTimeStr, string nsno)
        {
            if (fileTypeStr == ReportFileType.Xls.ToString().ToLower())
            {
                try
                {
                    return await this.ExportXls(templateName, startTimeStr, endTimeStr, nsno);
                }
                catch (NOContactException ex)
                {
                    ViewBag.Msg = "护理险平台无法连接，请联系管理员！";
                }
                catch (NoDataException ex)
                {
                    ViewBag.Msg = "暂无数据";
                }
            }
            return View();
        }
        public async Task<ActionResult> ExportXls(string templateName, string startTimeStr, string endTimeStr, string nsno)
        {
            if (templateName == null) return null;
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;
            DateTime.TryParse(startTimeStr, out startTime);
            DateTime.TryParse(endTimeStr, out endTime);
            var re = ExeclReportFactory.CreateReport(templateName, startTime, endTime, nsno);
            await re.BindData();
            re.Download();
            return null;
        }
    }
}
