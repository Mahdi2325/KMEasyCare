using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.NursingWorkstation
{
    public class HandoverRecord
    {
        public long Id { get; set; }
        public Nullable<System.DateTime> HandoverDate { get; set; }
        public Nullable<int> NewComer { get; set; }
        public Nullable<int> TransferSociety { get; set; }
        public Nullable<int> TransferMiniter { get; set; }
        public Nullable<int> TransferDisabled { get; set; }
        public Nullable<int> OutOverall { get; set; }
        public Nullable<int> OutReturn { get; set; }
        public Nullable<int> OutStill { get; set; }
        public Nullable<int> ActualPopulation { get; set; }
        public Nullable<int> InnaiSociety { get; set; }
        public Nullable<int> InnaiMiniter { get; set; }
        public Nullable<int> InnaiDisabled { get; set; }
        public Nullable<int> InnaiOverall { get; set; }

        public virtual List<LTC_HandoverDtl> LTC_HandoverDtl { get; set; }
    }
}
