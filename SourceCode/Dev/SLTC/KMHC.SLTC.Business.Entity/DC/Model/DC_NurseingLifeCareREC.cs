using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    //用来修改，添加的
    public class NurseingLife1
    {

        public DC_NurseingLifeCareREC NurseingLifeCareREC { get; set; }
        public List<NurseingLifeList> NurseingLifeCareEDTL { get; set; }

    }
    //用来查询的
    public class NurseingLife
    {

        public DC_NurseingLifeCareREC NurseingLifeCareREC { get; set; }
        public List<DC_NurseingLifeCareEDTL> NurseingLifeCareEDTL { get; set; }

    }


    //用来查询的
    public class NurseingLife3
    {
        public NurseingLifeWeeks NurseingLifeCareREC { get; set; }
        public List<DC_NurseingLifeCareEDTL> NurseingLifeCareEDTL { get; set; }

    }

    public class NurseingLifeWeeks
    {

        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public Nullable<int> RESIDENTNO { get; set; }
        public string REGNAME { get; set; }
        public string SEX { get; set; }
        public string NURSEAIDES { get; set; }
        public Nullable<int> WEEKNUMBER { get; set; }
        public string SECURITYMEASURES { get; set; }
        public string ARTICLESCARRIED { get; set; }
        public string MEDICATIONINSTRUCTIONS { get; set; }
        public string ACTIVITYSUMMARY { get; set; }
        public string QUESTIONBEHAVIOR { get; set; }
        public string REMARKS { get; set; }
        public Nullable<System.DateTime> WEEKSTARTDATE { get; set; }
        public string NURSENO { get; set; }
        public string SUPERVISOR { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<bool> DELFLAG { get; set; }
        public Nullable<System.DateTime> DELDATE { get; set; }
        public string ORGID { get; set; }
        public Nullable<System.DateTime> WEEK1 { get; set; }
        public Nullable<System.DateTime> WEEK2 { get; set; }

        public Nullable<System.DateTime> WEEK3 { get; set; }

        public Nullable<System.DateTime> WEEK4 { get; set; }

        public Nullable<System.DateTime> WEEK5 { get; set; }


        public string Res { get; set; }

        public string Nur { get; set; }

        public string OrgName { get; set; }


        public List<DC_NurseingLifeCareEDTL> NurseingLifeCareEDTL { get; set; }
    
    }


   public  class DC_NurseingLifeCareREC
    {

        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public string REGNO { get; set; }
        public Nullable<int> RESIDENTNO { get; set; }
        public string REGNAME { get; set; }
        public string SEX { get; set; }
        public string NURSEAIDES { get; set; }
        public Nullable<int> WEEKNUMBER { get; set; }
        public string SECURITYMEASURES { get; set; }
        public string ARTICLESCARRIED { get; set; }
        public string MEDICATIONINSTRUCTIONS { get; set; }
        public string ACTIVITYSUMMARY { get; set; }
        public string QUESTIONBEHAVIOR { get; set; }
        public string REMARKS { get; set; }
        public Nullable<System.DateTime> WEEKSTARTDATE { get; set; }
        public string NURSENO { get; set; }
        public string SUPERVISOR { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public Nullable<bool> DELFLAG { get; set; }
        public Nullable<System.DateTime> DELDATE { get; set; }
        public string ORGID { get; set; }

        public List<DC_NurseingLifeCareEDTL> NurseingLifeCareEDTL { get; set; }
    }
}
