
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.PackageRelated;

namespace KMHC.SLTC.Business.Implement
{
    public class ServiceRecordService : BaseService, IServiceRecordService
    {
        public BaseResponse<ServiceRecords> SaveServiceRecord(CHARGEITEM request)
        {
            var newItem = new ServiceRecords()
            {
                SERVICEID = request.CHARGEITEMID,
                NSID = SecurityHelper.CurrentPrincipal.OrgId,
                FEENO = request.FEENO,
                SERVICENAME = request.NAME,
                UNITS = request.UNITS,
                QTY = request.FEEITEMCOUNT,
                UNITPRICE = request.UNITPRICE,
                COST = request.UNITPRICE * request.FEEITEMCOUNT,
                TAKEWAY = request.TAKEWAY ?? "",
                TAKETIME = request.TAKETIME,
                OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo,
                ISCHARGEGROUPITEM = true,
                ISNCIITEM = request.ISNCIITEM,
                STATUS =0,
                CREATETIME = DateTime.Now,
                CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                UPDATETIME = DateTime.Now,
                UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                ISDELETE = false,

            };
            var billInfo = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.SERVICERECORDID == newItem.SERVICERECORDID).FirstOrDefault();
            if (billInfo != null)
            {
                unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(billInfo);
                unitOfWork.Save();
            }
            return base.Save<LTC_SERVICERECORD, ServiceRecords>(newItem, (q) => q.SERVICERECORDID == newItem.SERVICERECORDID);
        }
    }
}
