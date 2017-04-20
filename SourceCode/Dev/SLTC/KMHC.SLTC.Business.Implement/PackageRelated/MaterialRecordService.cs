
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Persistence;
using System;
using System.Linq;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.PackageRelated;

namespace KMHC.SLTC.Business.Implement
{
    public class MaterialRecordService : BaseService, IMaterialRecordService
    {
        public BaseResponse<MaterialRecords> SaveMaterialRecord(CHARGEITEM request)
        {
            var newItem = new MaterialRecords()
            {
                MATERIALID = request.CHARGEITEMID,
                NSID = SecurityHelper.CurrentPrincipal.OrgId,
                FEENO = request.FEENO,
                MATERIALNAME = request.NAME,
                UNITS = request.UNITS,
                QTY = request.FEEITEMCOUNT,
                UNITPRICE = request.UNITPRICE,
                COST = request.UNITPRICE * request.FEEITEMCOUNT,
                TAKEWAY = request.TAKEWAY ?? "",
                TAKETIME = request.TAKETIME,
                OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo,
                ISCHARGEGROUPITEM = true,
                ISNCIITEM = request.ISNCIITEM,
                STATUS = 0,
                CREATETIME = DateTime.Now,
                CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                UPDATETIME = DateTime.Now,
                UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                ISDELETE = false,
            };
            var billInfo = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.MATERIALRECORDID == newItem.MATERIALRECORDID).FirstOrDefault();
            if (billInfo != null)
            {
                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(billInfo);
                unitOfWork.Save();
            }
            return base.Save<LTC_MATERIALRECORD, MaterialRecords>(newItem, (q) => q.MATERIALRECORDID == newItem.MATERIALRECORDID);
        }
    }
}
