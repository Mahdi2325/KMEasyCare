using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IEvalReportService
    {
        List<EvalReportHeader> GetEvalData(long _feeNo, DateTime? startDate, DateTime? endDate, string code,string floorId);
    }
}
