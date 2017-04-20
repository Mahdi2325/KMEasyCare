using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;

namespace KMHC.SLTC.Business.Implement
{
    public class MonthlyPayBillRecordService : BaseService, IMonthlyPayBillRecordService
    {
        #region LTC_MonthlyPayBillRecord

        public BaseResponse<List<MonthlyPayBillRecords>> SaveMonthlyPayBillRecord(MonthlyPayBillRecords.MonthlyPayBillRecordsList request)
        {
            BaseResponse<List<MonthlyPayBillRecords>> response = new BaseResponse<List<MonthlyPayBillRecords>>();
            if (request.MonthlyPayBillRecordsLists != null && request.MonthlyPayBillRecordsLists.Count > 0)
            {
                #region 账单记录
                foreach (var item in request.MonthlyPayBillRecordsLists)
                {
                    LTC_MONTHLYPAYBILLRECORD model = new LTC_MONTHLYPAYBILLRECORD();

                    //model.ID = item.ID;
                    model.BILLID = item.BILLID;
                    //model.YEARMONTH = item.YEARMONTH;
                    model.FEENO = item.FEENO;
                    //model.PAYEDAMOUNT = item.PAYEDAMOUNT;
                    //model.COMPSTARTDATE = item.COMPSTARTDATE;
                    //model.COMPENDDATE = item.COMPENDDATE;
                    model.STATUS = item.STATUS;
                    //model.CREATEBY = item.CREATEBY;
                    //model.CREATETIME = item.CREATETIME;
                    model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.UPDATETIME = DateTime.Now;
                    //model.ISDELETE = item.ISDELETE;
                }
                unitOfWork.Save();
                response.ResultCode = 1001;
                #endregion
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "未查询到有效的账单数据！";
            }
            return response;
        }

        #endregion
    }
}
