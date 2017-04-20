using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using KMHC.SLTC.Persistence;

namespace KMHC.SLTC.Business.Implement
{
    public class RsChargeGroupService : BaseService, IRsChargeGroupService
    {
        public BaseResponse<IList<RsChargeGroup>> GetRsChargeGroup(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<RsChargeGroup>> response = new BaseResponse<IList<RsChargeGroup>>();
            var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                   join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                   join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                   join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.CHARGEITEMID equals d.DRUGID
                                   join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                   where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == 1 && a.ISDELETE != true
                                       && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0 && d.STATUS == 0
                                   orderby c.CHARGEITEMID descending
                                   select new RsChargeGroup()
                                   {
                                       CGRId = a.CGRID,
                                       ChargeGroupId = b.CHARGEGROUPID,
                                       ChargeGroupName = b.CHARGEGROUPNAME,
                                       FeeNo = a.FEENO,
                                       Status = d.STATUS,
                                       ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                       ChargeItemId = c.CHARGEITEMID,
                                       ChargeItemType = c.CHARGEITEMTYPE,
                                       ChargeTypeId = d.CHARGETYPEID,
                                       ConversionRatio = d.CONVERSIONRATIO ?? 1,
                                       FeeItemCount = c.FEEITEMCOUNT,
                                       MCCode = d.MCDRUGCODE,
                                       NSCode = d.NSDRUGCODE,
                                       Name = d.CNNAME,
                                       IsNciItem = d.ISNCIITEM,
                                       Spec = d.SPEC,
                                       Units = d.UNITS,
                                       UnitPrice = d.UNITPRICE
                                   }).Union(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                            join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                            join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                            join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.CHARGEITEMID equals d.MATERIALID
                                            join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                            where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == 2 && a.ISDELETE != true
                                                && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0 && d.STATUS == 0
                                            orderby c.CHARGEITEMID descending

                                            select new RsChargeGroup()
                                            {
                                                CGRId = a.CGRID,
                                                ChargeGroupId = b.CHARGEGROUPID,
                                                ChargeGroupName = b.CHARGEGROUPNAME,
                                                FeeNo = a.FEENO,
                                                Status = d.STATUS,
                                                ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                                ChargeItemId = c.CHARGEITEMID,
                                                ChargeItemType = c.CHARGEITEMTYPE,
                                                ChargeTypeId = d.CHARGETYPEID,
                                                ConversionRatio = 1,
                                                FeeItemCount = c.FEEITEMCOUNT,
                                                MCCode = d.MCMATERIALCODE,
                                                NSCode = d.NSMATERIALCODE,
                                                Name = d.MATERIALNAME,
                                                IsNciItem = d.ISNCIITEM,
                                                Spec = d.SPEC,
                                                Units = d.UNITS,
                                                UnitPrice = d.UNITPRICE
                                            }).Union(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                                     join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                     join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                                     join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.CHARGEITEMID equals d.SERVICEID
                                                     join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                                     where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == 3 && a.ISDELETE != true
                                                         && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0 && d.STATUS == 0
                                                     orderby c.CHARGEITEMID descending

                                                     select new RsChargeGroup()
                                                     {
                                                         CGRId = a.CGRID,
                                                         ChargeGroupId = b.CHARGEGROUPID,
                                                         ChargeGroupName = b.CHARGEGROUPNAME,
                                                         FeeNo = a.FEENO,
                                                         Status = d.STATUS,
                                                         ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                                         ChargeItemId = c.CHARGEITEMID,
                                                         ChargeItemType = c.CHARGEITEMTYPE,
                                                         ChargeTypeId = d.CHARGETYPEID,
                                                         ConversionRatio = 1,
                                                         FeeItemCount = c.FEEITEMCOUNT,
                                                         MCCode = d.MCSERVICECODE,
                                                         NSCode = d.NSSERVICECODE,
                                                         Name = d.SERVICENAME,
                                                         IsNciItem = d.ISNCIITEM,
                                                         Spec = "",
                                                         Units = d.UNITS,
                                                         UnitPrice = d.UNITPRICE
                                                     }).ToList();

            response.Data = chargeGroupData;
            return response;
        }

        public BaseResponse<IList<RsChargeGroup>> QueryRsChargeGroup(BaseRequest<PaymentMgmtFilter> request)
        {
            BaseResponse<IList<RsChargeGroup>> response = new BaseResponse<IList<RsChargeGroup>>();
            var chargeGroupData = (from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                            join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                            join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                            join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.CHARGEITEMID equals d.DRUGID
                            join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                            where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == 1 && a.ISDELETE != true 
                                && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS==0 && b.STATUS==0 && d.STATUS==0
                            orderby c.CHARGEITEMID descending
                            select new RsChargeGroup()
                            {
                                CGRId = a.CGRID,
                                ChargeGroupId = b.CHARGEGROUPID,
                                ChargeGroupName = b.CHARGEGROUPNAME,
                                FeeNo = a.FEENO,
                                Status = d.STATUS,
                                ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                ChargeItemId = c.CHARGEITEMID,
                                ChargeItemType = c.CHARGEITEMTYPE,
                                ChargeTypeId = d.CHARGETYPEID,
                                ConversionRatio = d.CONVERSIONRATIO ?? 1,
                                FeeItemCount = c.FEEITEMCOUNT,
                                MCCode = d.MCDRUGCODE,
                                NSCode = d.NSDRUGCODE,
                                Name = d.CNNAME,
                                IsNciItem = d.ISNCIITEM,
                                Spec = d.SPEC,
                                Units = d.UNITS,
                                UnitPrice=d.UNITPRICE
                            }).Union(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                     join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                     join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                     join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.CHARGEITEMID equals d.MATERIALID
                                     join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                     where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == 2 && a.ISDELETE != true
                                         && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0 && d.STATUS == 0
                                     orderby c.CHARGEITEMID descending

                                     select new RsChargeGroup()
                                     {
                                         CGRId = a.CGRID,
                                         ChargeGroupId = b.CHARGEGROUPID,
                                         ChargeGroupName = b.CHARGEGROUPNAME,
                                         FeeNo = a.FEENO,
                                         Status = d.STATUS,
                                         ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                         ChargeItemId = c.CHARGEITEMID,
                                         ChargeItemType = c.CHARGEITEMTYPE,
                                         ChargeTypeId = d.CHARGETYPEID,
                                         ConversionRatio = 1,
                                         FeeItemCount = c.FEEITEMCOUNT,
                                         MCCode = d.MCMATERIALCODE,
                                         NSCode = d.NSMATERIALCODE,
                                         Name = d.MATERIALNAME,
                                         IsNciItem = d.ISNCIITEM,
                                         Spec = d.SPEC,
                                         Units = d.UNITS,
                                         UnitPrice = d.UNITPRICE
                                     }).Union(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                                              join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                              join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                              join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.CHARGEITEMID equals d.SERVICEID
                                              join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on b.NSID equals e.ORGID
                                              where a.FEENO == request.Data.FEENO && c.CHARGEITEMTYPE == 3 && a.ISDELETE != true
                                                  && b.ISDELETE != true && c.ISDELETE != true && d.ISDELETE != true && a.STATUS == 0 && b.STATUS == 0 && d.STATUS == 0
                                              orderby c.CHARGEITEMID descending

                                              select new RsChargeGroup()
                                              {
                                                  CGRId = a.CGRID,
                                                  ChargeGroupId = b.CHARGEGROUPID,
                                                  ChargeGroupName = b.CHARGEGROUPNAME,
                                                  FeeNo = a.FEENO,
                                                  Status = d.STATUS,
                                                  ChargeGroupPeriod = b.CHARGEGROUPPERIOD,
                                                  ChargeItemId = c.CHARGEITEMID,
                                                  ChargeItemType = c.CHARGEITEMTYPE,
                                                  ChargeTypeId = d.CHARGETYPEID,
                                                  ConversionRatio = 1,
                                                  FeeItemCount = c.FEEITEMCOUNT,
                                                  MCCode = d.MCSERVICECODE,
                                                  NSCode = d.NSSERVICECODE,
                                                  Name = d.SERVICENAME,
                                                  IsNciItem = d.ISNCIITEM,
                                                  Spec = "",
                                                  Units = d.UNITS,
                                                  UnitPrice = d.UNITPRICE
                                              }).ToList();

            response.RecordsCount = chargeGroupData.Count;
            List<RsChargeGroup> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = chargeGroupData.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = chargeGroupData.ToList();
            }

            response.Data = list;
            return response;
        }
    }
}
