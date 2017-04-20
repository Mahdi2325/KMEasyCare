using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Filter
{
    public class ABFilter
    {
    
        public string REGNAME { get; set; }
        public string RESIDENTNO { get; set; }
        public string SEX { get; set; }
        public string REGNO { get; set; }
        public string ORGID { get; set;}
        public string NURSEAIDES { get; set; }
        public string Res { get; set; }
        public string Nur { get; set; }
        public int year { get; set; }
        public string Day { get; set; }
        public int month { get; set; }
        public Nullable<long> FEENO { get; set; }

        public string ACTIVITYNAME{get;set;}

        public string TITLENAME{get;set;}

        public string ITEMNAME { get; set; }
        //机构名称
        public string OrgName { get; set; }


             



    }
}

