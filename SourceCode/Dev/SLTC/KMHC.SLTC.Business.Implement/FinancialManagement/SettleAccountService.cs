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
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Model.FinancialManagement;
using AutoMapper;

namespace KMHC.SLTC.Business.Implement
{
    public class SettleAccountService : BaseService, ISettleAccountService
    {
        public BaseResponse<SettleAccountModel> QuerySettleAccountInfo(long feeNo)
        {
            BaseResponse<SettleAccountModel> response = new BaseResponse<SettleAccountModel>();
            var settleAccountData = (from a in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                                     join b in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on a.REGNO equals b.REGNO
                                     join c in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on b.REGNO equals c.REGNO into reg_relationinfo
                                     join d in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on a.FEENO equals d.FEENO into reg_nciinfo
                                     join e in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals e.ORGID
                                     from reg_nci in reg_nciinfo.DefaultIfEmpty()
                                     from reg_relation in reg_relationinfo.DefaultIfEmpty()
                                     where a.FEENO == feeNo
                                     select new SettleAccountModel()
                                     {
                                         OrgName = e.ORGNAME,
                                         Name = b.NAME,
                                         Sex = b.SEX,
                                         BrithDate = b.BRITHDATE,
                                         IDNo = b.IDNO,
                                         ResidengNo = b.RESIDENGNO,
                                         RSType = a.RSTYPE,
                                         CareTypeId = reg_nci.CARETYPEID,
                                         ContactPhone = reg_relation.CONTACTPHONE,
                                         FeeNo = a.FEENO,
                                         NCIPayLevel = reg_nci.NCIPAYLEVEL,
                                         NCIPayScale = reg_nci.NCIPAYSCALE,
                                         NCIPay = reg_nci.NCIPAYLEVEL * reg_nci.NCIPAYSCALE,
                                         DiseaseDiag = b.DISEASEDIAG
                                     }).ToList();

            if (settleAccountData.Count > 0)
            {
                var data = Mapper.Map<SettleAccountModel>(settleAccountData[0]);
                response.Data = data;
            }
            return response;
        }

        public BaseResponse<IList<BillV2>> QueryBillV2(long feeNo, string beginTime, string endTime)
        {
            DateTime dt1 = DateTime.Parse(beginTime + "-01");
            DateTime dtTemp = DateTime.Parse(endTime + "-01");
            DateTime dt2 = dtTemp.AddDays(1 - dtTemp.Day).AddMonths(1);
            BaseResponse<IList<BillV2>> response = new BaseResponse<IList<BillV2>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_BILLV2>().dbSet
                            join b in unitOfWork.GetRepository<LTC_BILLV2PAY>().dbSet on a.BILLPAYID equals b.BILLPAYID
                            join c in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on a.FEENO equals c.FEENO
                            join d in unitOfWork.GetRepository<LTC_RESIDENTBALANCE>().dbSet on a.FEENO equals d.FEENO
                            where a.FEENO == feeNo && (a.STATUS == 2 || a.STATUS == 20) && a.ISDELETE != true && a.BALANCESTARTTIME >= dt1 && a.BALANCEENDTIME < dt2
                            orderby a.CREATETIME descending
                            select new BillV2()
                            {
                                FeeNo = a.FEENO,
                                NCIItemTotalCost = a.NCIITEMTOTALCOST,
                                SelfPay = a.SELFPAY,
                                NCIPayLevel = c.NCIPAYLEVEL,
                                NCIPaysCale = c.NCIPAYSCALE,
                                NCIPay = a.NCIPAY,
                                NCIItemSelfPay = a.NCIITEMSELFPAY,
                                BalanceStartTime = a.BALANCESTARTTIME,
                                BalanceEndTime = a.BALANCEENDTIME,
                                HospDay = a.HOSPDAY,
                                InvoiceNo = b.INVOICENO,
                                TotalNciPay = d.TOTALNCIPAY
                            }).Distinct().ToList();

            response.Data = billData;
            return response;
        }

        public BaseResponse<IList<FeeRecordBaseInfo>> QueryRecord(long feeNo, string beginTime, string endTime)
        {
            DateTime dt1 = DateTime.Parse(beginTime + "-01");
            DateTime dtTemp = DateTime.Parse(endTime + "-01");
            DateTime dt2 = dtTemp.AddDays(1 - dtTemp.Day).AddMonths(1);
            BaseResponse<IList<FeeRecordBaseInfo>> response = new BaseResponse<IList<FeeRecordBaseInfo>>();
            var feeRecordInfo = (from a in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                 join b in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on a.CHARGERECORDID equals b.DRUGRECORDID
                                 join c in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on b.DRUGID equals c.DRUGID
                                 where a.FEENO == feeNo && a.CHARGERECORDTYPE == 1 && a.ISDELETE == false && b.TAKETIME >= dt1 && b.TAKETIME < dt2
                                 select new FeeRecordBaseInfo()
                            {
                                FeeRecordID = a.FEERECORDID,
                                Cost = a.COST,
                                ChargeRecordType = a.CHARGERECORDTYPE,
                                ChargeTypeId = c.CHARGETYPEID,
                                MCType = c.MCTYPE
                            }).Union(from a in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                     join b in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on a.CHARGERECORDID equals b.MATERIALRECORDID
                                     join c in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on b.MATERIALID equals c.MATERIALID
                                     where a.FEENO == feeNo && a.CHARGERECORDTYPE == 2 && a.ISDELETE == false && b.TAKETIME >= dt1 && b.TAKETIME < dt2
                                     select new FeeRecordBaseInfo()
                                     {
                                         FeeRecordID = a.FEERECORDID,
                                         Cost = a.COST,
                                         ChargeRecordType = a.CHARGERECORDTYPE,
                                         ChargeTypeId = c.CHARGETYPEID,
                                         MCType = ""
                                     }).Union(from a in unitOfWork.GetRepository<LTC_FEERECORD>().dbSet
                                              join b in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on a.CHARGERECORDID equals b.SERVICERECORDID
                                              join c in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on b.SERVICEID equals c.SERVICEID
                                              where a.FEENO == feeNo && a.CHARGERECORDTYPE == 3 && a.ISDELETE == false && b.TAKETIME >= dt1 && b.TAKETIME < dt2
                                              select new FeeRecordBaseInfo()
                                              {
                                                  FeeRecordID = a.FEERECORDID,
                                                  Cost = a.COST,
                                                  ChargeRecordType = a.CHARGERECORDTYPE,
                                                  ChargeTypeId = c.CHARGETYPEID,
                                                  MCType = ""
                                              }).ToList();
            response.Data = feeRecordInfo;
            return response;
        }
    }
}
