using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
  public  class NurseRpttpr
    {
        public long ID { get; set; }
        public Nullable<long> FEENO { get; set; }
        public Nullable<int> REGNO { get; set; }
        public Nullable<System.DateTime> RECDATE { get; set; }
        public string CLASSTYPE { get; set; }
        public string RECORDBY { get; set; }
        public Nullable<decimal> BODYTEMP { get; set; }
        public Nullable<int> SBP { get; set; }
        public Nullable<int> DBP { get; set; }
        public Nullable<int> PULSE { get; set; }
        public Nullable<int> BREATH { get; set; }
        public Nullable<decimal> OXYGEN { get; set; }
        public Nullable<int> INVALUE { get; set; }
        public Nullable<int> OUTVALUE { get; set; }
        public string EDEMA { get; set; }
        public string OTHERDESC { get; set; }
        public Nullable<int> NOISEI1 { get; set; }
        public Nullable<int> NOISEI2 { get; set; }
        public Nullable<int> NOISEI3 { get; set; }
        public Nullable<int> FPG { get; set; }
        public Nullable<int> PPBS { get; set; }
        public Nullable<bool> NASOGASTRIC { get; set; }
        public Nullable<bool> CATHETER { get; set; }
        public Nullable<bool> TRACHEOSTOMY { get; set; }
        public Nullable<bool> STOMAFISTULA { get; set; }
        public Nullable<bool> WOUNDSKINCARE { get; set; }
        public Nullable<bool> SPRAYINHALATION { get; set; }
        public string OXYGENUSE { get; set; }
        public string APPETITE { get; set; }
        public Nullable<int> VOMITINGTIMES { get; set; }
        public Nullable<int> SLEEPHOURS { get; set; }
        public string SLEEPSTATE { get; set; }
        public Nullable<bool> MENSTRUALCYCLE { get; set; }
        public string INTESTINALPERISTALSIS { get; set; }
        public string STOOLNATURE { get; set; }
        public Nullable<int> STOOLTIMES { get; set; }
        public string REHABILITATION { get; set; }
        public Nullable<int> OUTBEDNUMBER { get; set; }
        public string CONSTRAINTSEVAL { get; set; }
        public string SKININTEGRITY { get; set; }
        public Nullable<bool> DOCDIAGFLAG { get; set; }
        public Nullable<bool> PAINFLAG { get; set; }
        public string PAINPART { get; set; }
        public string PAINLEVEL { get; set; }
        public string PAINPRESCRIPTION { get; set; }
        public string SIDEEFFECT { get; set; }
        public string PAINCARE { get; set; }
        public string THERAPYRESPONSE { get; set; }
        public string PULMONARYMURMUR { get; set; }
        public string SPUTUMCOLOR { get; set; }
        public Nullable<bool> REGTALKFLAG { get; set; }
        public Nullable<bool> FAMILYTALKFLAG { get; set; }
        public Nullable<bool> CARERECFLAG { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public string CREATEBY { get; set; }
        public string DEFECATIONWAY { get; set; }
        public string ORGID { get; set; }

    }
}
