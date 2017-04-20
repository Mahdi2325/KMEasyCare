using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class NSCPL
    {
        public long SEQNO { get; set; }
        public string EMPNO { get; set; }
        public string CPSOURCE { get; set; }
        public string CPTYPE { get; set; }
        public string CPLEVEL { get; set; }
        public string CPDIAG { get; set; }
        public string CPNO { get; set; }
        public string NSDESC { get; set; }
        public string CPCAUSE { get; set; }
        public string CPREASON { get; set; }
        public string FINISHFLAG { get; set; }
        public string CPRESULT { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<int> NEEDDAYS { get; set; }
        public Nullable<int> TOTALDAYS { get; set; }
        public Nullable<System.DateTime> STARTDATE { get; set; }
        public Nullable<System.DateTime> TARGETDATE { get; set; }
        public Nullable<System.DateTime> FINISHDATE { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
    }
    public class NSCPLView
    {
        public string NAME { get; set; }
        public Nullable<long> SEQNO { get; set; }
        public string EMPNAME { get; set; }
        public string EMPNO { get; set; }
        public string CPSOURCE { get; set; }
        public string CPTYPE { get; set; }
        public string CPLEVEL { get; set; }
        public string CPDIAG { get; set; }
        public string NSDESC { get; set; }
        public string CPCAUSE { get; set; }
        public string CPREASON { get; set; }
        public bool? FINISHFLAG { get; set; }
        public string CPRESULT { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<int> NEEDDAYS { get; set; }
        public Nullable<int> TOTALDAYS { get; set; }
        public Nullable<System.DateTime> STARTDATE { get; set; }
        public Nullable<System.DateTime> TARGETDATE { get; set; }
        public Nullable<System.DateTime> FINISHDATE { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public int QUANTITY { get; set; }
        public string PERCENTAGE { get; set; }
        public int QUANFINISH { get; set; }
    }

    public class ASSESSVALUE
    {
        public long ID { get; set; }
        public string VALUEDESC { get; set; }
        public string RECORDBY { get; set; }
        public string CREATEBY { get; set; }
        public string ORGID { get; set; }
        public string EXECUTEBY { get; set; }
        public string EXECUTEBYNAME { get; set; }
        public Nullable<long> SEQNO { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> RECDATE { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
    }

    public class NSCPLReportView
    {
        public string Org { get; set; }
        public long SeqNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RegName { get; set; }
        public string FeeNo { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string EmpName { get; set; }
        public string CpLevel { get; set; }
        public string CpDiag { get; set; }
        public string CpReason { get; set; }
        public string NsDesc { get; set; }
        public string CpResult { get; set; }
        public string TotalDays { get; set; }
        
        public string NscplGoal { get; set; }
        public string NscplActivity { get; set; }
        public string AssessValue { get; set; }
    }
}
