#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :2017/3/27 
** 说明     :
******************************************************************/
#endregion

using ExcelReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public abstract class BaseExeclReport
    {
        protected abstract string FileNamePrefix { get; }

        public string TemplateName;

        protected static string TemplateRootPath = string.Format(@"{0}Templates\", HttpContext.Current.Server.MapPath(VirtualPathUtility.GetDirectory("~")));

        protected string TemplateFormatterPath
        {
            get
            {
                return string.Format(@"{0}{1}.xml", TemplateRootPath, TemplateName);
            }
        }
        private string TemplateFilePath
        {
            get
            {
                return string.Format(@"{0}{1}.xls", TemplateRootPath, TemplateName);
            }
        }
        private string TargetFilePath
        {
            get
            {
                return string.Format(@"{0}{1:yyyMMddHHmmss}.xls", FileNamePrefix, DateTime.Now);
            }
        }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string NsId { get; set; }
       

        protected SheetFormatter Formatter { get; set; }

        public async Task BindData()
        {
            await CreatFormatter();
        }

        protected abstract Task CreatFormatter();

        public void Download()
        {
            ExportHelper.ExportToWeb(TemplateFilePath, TargetFilePath, Formatter);
        }
    }
}
