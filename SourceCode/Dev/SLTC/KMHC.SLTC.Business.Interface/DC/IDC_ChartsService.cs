using KMHC.SLTC.Business.Entity.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC
{
    public interface IDC_ChartsService
    {
        List<Chart_AccumulateUU> GetAccumulateUU();
        List<Chart_DiseaseDistribution> GetDiseaseDistribution();
    }
}
