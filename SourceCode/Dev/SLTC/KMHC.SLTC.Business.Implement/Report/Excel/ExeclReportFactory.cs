#region 文件描述
/******************************************************************
** 创建人   :BobDu
** 创建时间 :2017/3/27 
** 说明     :
******************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.Report.Excel
{
    public class ExeclReportFactory
    {
        private static readonly string DllName = "KMHC.SLTC.Business.Implement.Report.";

        public static BaseExeclReport CreateReport(string type, DateTime startTime, DateTime endTime, string nsno)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("type为空");
            }
            BaseExeclReport report = (BaseExeclReport)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(DllName + type, false);
            if (report == null)
            {
                throw new Exception("创建对象失败！");
            }
            report.TemplateName = type;
            report.StartTime = startTime;
            report.EndTime = endTime;
            report.NsId = nsno;
            return report;
        }
    }
}
