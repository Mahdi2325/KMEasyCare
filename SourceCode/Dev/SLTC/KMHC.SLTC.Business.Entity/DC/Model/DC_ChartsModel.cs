using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.DC.Model
{
    public class DC_ChartsModel
    {
        public string Date { get; set; }
        public string AccumulateUU { get; set; }
        public string ClosedUU { get; set; }
        public string AccumulateClosedUU { get; set; }
    }

    public class Chart_AccumulateUU
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int AccumulateUU { get; set; }
        public int ClosedUU { get; set; }
        public int AccumulateClosedUU { get; set; }
    }

    public class Chart_DiseaseDistribution
    {
        public int ID { get; set; }
        public string DiseaseName { get; set; }
        public int UUCount { get; set; }
    }
}
