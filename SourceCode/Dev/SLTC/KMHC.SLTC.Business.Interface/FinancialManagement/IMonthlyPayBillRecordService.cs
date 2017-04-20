using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KMHC.SLTC.Business.Interface
{
    public interface IMonthlyPayBillRecordService
    {
        #region LTC_MonthlyPayBillRecord
        BaseResponse<List<MonthlyPayBillRecords>> SaveMonthlyPayBillRecord(MonthlyPayBillRecords.MonthlyPayBillRecordsList request);
        #endregion

    }
}
