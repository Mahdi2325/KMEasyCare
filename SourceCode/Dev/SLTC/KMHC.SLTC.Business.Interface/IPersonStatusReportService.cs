using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Filter;


namespace KMHC.SLTC.Business.Interface
{
    public interface IPersonStatusReportService : IBaseService
    {
        PersonStatusReportModel QueryPersonStatusInfo(PersonStatusFilter baseRequest);
    }
}
