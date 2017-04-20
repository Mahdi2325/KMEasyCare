using KMHC.SLTC.Business.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IServiceDepositGrantList : IBaseService
    {
        string GetNsno();
    }
}
