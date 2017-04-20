using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{

    public class CheckRecCollection
    {
        public CheckRec CheckRec { get; set; }
        public List<CheckRecdtl> CheckRecdtl { get; set; }
    }

    public class ReportCheckRec : CheckRec
    {
        public List<ReportCheckRecdtl> ReportCheckRecdtl { get; set; }
    }

    /// <summary>
    /// 检查项目扩展数据
    /// </summary>
    public class ReportCheckRecdtl : CheckRecdtl
    {
        public string CHECKITEMNAME { get; set; }
    }

    public class CheckRec
    {
        public long RECORDID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> CHECKDATE { get; set; }
        public string RECORDBY { get; set; }
        public string HOSPNAME { get; set; }
        public string CHECKRESULTS { get; set; }
        public Nullable<System.DateTime> NEXTCHECKDATE { get; set; }
        public string NEXTCHECKBY { get; set; }
        public string TRACESTATE { get; set; }
        public string DISEASEDESC { get; set; }
        public Nullable<bool> XRAYFLAG { get; set; }
        public Nullable<bool> NORMALFLAG { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }

        public List<CheckRecdtl> CheckRecdtl { get; set; }
    }
}
