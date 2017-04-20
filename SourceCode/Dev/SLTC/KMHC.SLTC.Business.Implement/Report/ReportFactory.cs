using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.Report
{
    public  class ReportFactory
    {
        //private static readonly IDictionary ReportMapping = ConfigurationManager.GetSection("report") as IDictionary;
        private static readonly string DLLName="KMHC.SLTC.Business.Implement.Report.";

        public static BaseReport CreateReport(string type, long id, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("type为空");
            }
            BaseReport report = (BaseReport)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(DLLName + type, false);
            if (report == null)
            {
                throw new Exception("创建对象失败！");
            }
            report.TemplateName = type;
            report.ParamId = id;
            report.StartDate = startDate;
            report.EndDate = endDate;
            return report;
        }

        public static BaseReport CreateReport(string type, long id, DateTime startDate, DateTime endDate,string order)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Exception("type为空");
            }
            BaseReport report = (BaseReport)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(DLLName + type, false);
            if (report == null)
            {
                throw new Exception("创建对象失败！");
            }
            report.TemplateName = type;
            report.ParamId = id;
            report.StartDate = startDate;
            report.EndDate = endDate;
            report.Order = order;
            return report;
        }
    }
}
