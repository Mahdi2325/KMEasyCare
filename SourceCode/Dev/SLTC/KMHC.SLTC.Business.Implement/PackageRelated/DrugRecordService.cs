
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.PackageRelated;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KMHC.SLTC.Business.Entity.ChargeInputModel;

namespace KMHC.SLTC.Business.Implement
{
    public class DrugRecordService : BaseService, IDrugRecordService
    {
        #region 套餐费用录入
        public BaseResponse<DrugRecords> SaveDrugRecord(CHARGEITEM request)
        {
            var newItem = new DrugRecords()
            {
                DRUGID = request.CHARGEITEMID,
                NSID = SecurityHelper.CurrentPrincipal.OrgId,
                FEENO = request.FEENO,
                CNNAME = request.NAME,
                FORM = request.FORM ?? "",
                UNITS = request.UNITS,
                QTY = request.FEEITEMCOUNT,
                UNITPRICE = request.UNITPRICE,
                COST = request.UNITPRICE * request.FEEITEMCOUNT,
                DOSAGE=request.DOSAGE,
                TAKEWAY = request.TAKEWAY ?? "",
                FERQ = request.FERQ ?? "",
                TAKETIME = request.TAKETIME,
                OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo,
                ISCHARGEGROUPITEM = true,
                ISNCIITEM = request.ISNCIITEM,
                STATUS=0,
                CREATETIME = DateTime.Now,
                CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                UPDATETIME = DateTime.Now,
                UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                ISDELETE = false,
            };

            var billInfo = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.DRUGRECORDID == newItem.DRUGRECORDID).FirstOrDefault();
            if (billInfo != null)
            {
                unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(billInfo);
                unitOfWork.Save();
            }
            return base.Save<LTC_DRUGRECORD, DrugRecords>(newItem, (q) => q.DRUGRECORDID == newItem.DRUGRECORDID);
        }

        #endregion
    }
}
