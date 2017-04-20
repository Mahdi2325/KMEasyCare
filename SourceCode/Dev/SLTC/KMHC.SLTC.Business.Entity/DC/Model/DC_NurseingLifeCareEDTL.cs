using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{

    public class NurseingLifeList {


        public DC_NurseingLifeCareEDTL NurseingLifeCare { get; set;}

        public string Checkcy1 { get; set; }
        public string Checkcy2 { get; set; }
        public string Checkcy3 { get; set; }
        public string Checkcy4 { get; set; }
        public string Checkcy5 { get; set; }
    }

  public class DC_NurseingLifeCareEDTL
    {
        public long SEQNO { get; set; }
        public Nullable<long> ID { get; set; }
        public string ACTIVITY9 { get; set; }
        public string ACTIVITY11 { get; set; }
        public string ACTIVITY14 { get; set; }
        public string ACTIVITY15 { get; set; }
        public string ACTIVITY16 { get; set; }
        public Nullable<decimal> BODYTEMPERATURE { get; set; }
        public Nullable<int> PULSE { get; set; }
        public Nullable<int> BREATH { get; set; }
        public Nullable<int> SBP { get; set; }
        public Nullable<int> DBP { get; set; }
        public string MEDICINE { get; set; }
        public Nullable<System.DateTime> RECORDDATE { get; set; }
        public string DAYOFWEEK { get; set; }
        public string HOLIDAYFLAG { get; set; }
    }
}
