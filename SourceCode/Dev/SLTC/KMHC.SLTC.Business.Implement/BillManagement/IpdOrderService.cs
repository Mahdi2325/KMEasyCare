using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using NPOI.SS.Formula.Functions;
using KM.Common;
using KMHC.SLTC.Business.Entity.BillManagement;
using KMHC.SLTC.Business.Entity.PackageRelated;

namespace KMHC.SLTC.Business.Implement
{
    public class IpdOrderService : BaseService, IIpdOrderService
    {
        public BaseResponse<IList<IpdOrder>> QueryIpdOrder(BaseRequest<IpdOrderFilter> request)
        {
            List<IpdOrder> ListResult = new List<IpdOrder>();
            List<CHARGEGROUP> ChargeItemResult = new List<CHARGEGROUP>();
            BaseResponse<IList<IpdOrder>> response = new BaseResponse<IList<IpdOrder>>();
            var orderData = (from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                             join b in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on a.FEECODE equals b.DRUGID
                             join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                             from doc_info in doc.DefaultIfEmpty()
                             join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                             from nur_info in nur.DefaultIfEmpty()
                             join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                             from freq_info in freq.DefaultIfEmpty()
                             join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                             from ipdreg_info in ipdreg.DefaultIfEmpty()
                             join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                             from regfile_info in regfile.DefaultIfEmpty()
                             where a.ITEMTYPE == 1 && a.ISDELETE != true && b.STATUS == 0 && b.ISDELETE != true
                             orderby a.CREATETIME descending
                             select new IpdOrder()
                             {
                                 RsName=regfile_info.NAME,
                                 OrderNo = a.ORDERNO,
                                 OrderType = a.ORDERTYPE,
                                 OrderName = b.CNNAME,
                                 FeeCode = a.FEECODE,
                                 ItemType = a.ITEMTYPE,
                                 AcRemark = a.ACREMARK,
                                 TakeQty = a.TAKEQTY ?? 1,
                                 TakeDay = a.TAKEDAY,
                                 TakeFreq = a.TAKEFREQ,
                                 TakeFreqQty = freq_info.FREQQTY ?? 1,
                                 TakeWay = a.TAKEWAY,
                                 ConversionRatio = b.CONVERSIONRATIO,
                                 PrescribeUnits = b.PRESCRIBEUNITS,
                                 Units = b.UNITS,
                                 UnitPrice = b.UNITPRICE,
                                 ChargeQty = Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                 Amount = b.UNITPRICE * Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                 ConfirmFlag = a.CONFIRMFLAG,
                                 ConfirmDate = a.CONFIRMDATE,
                                 CheckFlag = a.CHECKFLAG,
                                 CheckDate = a.CHECKDATE,
                                 StopFlag = a.STOPFLAG,
                                 StopDate = a.STOPDATE,
                                 StopCheckFlag = a.STOPCHECKFLAG,
                                 StopCheckDate = a.STOPCHECKDATE,
                                 StartDate = a.STARTDATE,
                                 EndDate = a.ENDDATE,
                                 ExecTimes = a.EXECTIMES,
                                 BillDate = a.BILLDATE,
                                 DeleteFlag = a.DELETEFLAG,
                                 FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                 ChargeGroupId = a.CHARGEGROUPID,
                                 DoctorNo = a.DOCTORNO,
                                 NurseNo = a.NURSENO,
                                 DoctorName = doc_info.EMPNAME,
                                 NurseName = nur_info.EMPNAME,
                                 FeeNo = a.FEENO,
                                 RegNo = a.REGNO,
                                 OrgId = a.ORGID,
                                 SortNumber = a.SORTNUMBER,
                                 CreateBy = a.CREATEBY,
                                 CreateTime = a.CREATETIME,
                                 UpdateBy = a.UPDATEBY,
                                 UpdateTime = a.UPDATETIME,
                                 IsDelete = a.ISDELETE
                             }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                      join b in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on a.FEECODE equals b.MATERIALID
                                      join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                      from doc_info in doc.DefaultIfEmpty()
                                      join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                      from nur_info in nur.DefaultIfEmpty()
                                      join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                      from freq_info in freq.DefaultIfEmpty()
                                      join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                                      from ipdreg_info in ipdreg.DefaultIfEmpty()
                                      join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                      from regfile_info in regfile.DefaultIfEmpty()
                                      where a.ITEMTYPE == 2 && a.ISDELETE != true && b.STATUS == 0 && b.ISDELETE != true
                                      orderby a.CREATETIME descending
                                      select new IpdOrder()
                                      {
                                          RsName=regfile_info.NAME,
                                          OrderNo = a.ORDERNO,
                                          OrderType = a.ORDERTYPE,
                                          OrderName = b.MATERIALNAME,
                                          FeeCode = a.FEECODE,
                                          ItemType = a.ITEMTYPE,
                                          AcRemark = a.ACREMARK,
                                          TakeQty = a.TAKEQTY ?? 1,
                                          TakeDay = a.TAKEDAY,
                                          TakeFreq = a.TAKEFREQ,
                                          TakeFreqQty = freq_info.FREQQTY ?? 1,
                                          TakeWay = a.TAKEWAY,
                                          ConversionRatio = 1,
                                          PrescribeUnits = b.UNITS,
                                          Units = b.UNITS,
                                          UnitPrice = b.UNITPRICE,
                                          ChargeQty = a.TAKEQTY ?? 1,
                                          Amount = b.UNITPRICE * a.TAKEQTY ?? 1,
                                          ConfirmFlag = a.CONFIRMFLAG,
                                          ConfirmDate = a.CONFIRMDATE,
                                          CheckFlag = a.CHECKFLAG,
                                          CheckDate = a.CHECKDATE,
                                          StopFlag = a.STOPFLAG,
                                          StopDate = a.STOPDATE,
                                          StopCheckFlag = a.STOPCHECKFLAG,
                                          StopCheckDate = a.STOPCHECKDATE,
                                          StartDate = a.STARTDATE,
                                          EndDate = a.ENDDATE,
                                          ExecTimes = a.EXECTIMES,
                                          BillDate = a.BILLDATE,
                                          DeleteFlag = a.DELETEFLAG,
                                          FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                          ChargeGroupId = a.CHARGEGROUPID,
                                          DoctorNo = a.DOCTORNO,
                                          NurseNo = a.NURSENO,
                                          DoctorName = doc_info.EMPNAME,
                                          NurseName = nur_info.EMPNAME,
                                          FeeNo = a.FEENO,
                                          RegNo = a.REGNO,
                                          OrgId = a.ORGID,
                                          SortNumber = a.SORTNUMBER,
                                          CreateBy = a.CREATEBY,
                                          CreateTime = a.CREATETIME,
                                          UpdateBy = a.UPDATEBY,
                                          UpdateTime = a.UPDATETIME,
                                          IsDelete = a.ISDELETE
                                      }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                               join b in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on a.FEECODE equals b.SERVICEID
                                               join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                               from doc_info in doc.DefaultIfEmpty()
                                               join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                               from nur_info in nur.DefaultIfEmpty()
                                               join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                               from freq_info in freq.DefaultIfEmpty()
                                               join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                                               from ipdreg_info in ipdreg.DefaultIfEmpty()
                                               join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                               from regfile_info in regfile.DefaultIfEmpty()
                                               where a.ITEMTYPE == 3 && a.ISDELETE != true && b.STATUS == 0 && b.ISDELETE != true
                                               orderby a.CREATETIME descending
                                               select new IpdOrder()
                                               {
                                                   RsName=regfile_info.NAME,
                                                   OrderNo = a.ORDERNO,
                                                   OrderType = a.ORDERTYPE,
                                                   OrderName = b.SERVICENAME,
                                                   FeeCode = a.FEECODE,
                                                   ItemType = a.ITEMTYPE,
                                                   AcRemark = a.ACREMARK,
                                                   TakeQty = a.TAKEQTY ?? 1,
                                                   TakeDay = a.TAKEDAY,
                                                   TakeFreq = a.TAKEFREQ,
                                                   TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                   TakeWay = a.TAKEWAY,
                                                   ConversionRatio = 1,
                                                   PrescribeUnits = b.UNITS,
                                                   Units = b.UNITS,
                                                   UnitPrice = b.UNITPRICE,
                                                   ChargeQty = a.TAKEQTY ?? 1,
                                                   Amount = b.UNITPRICE * a.TAKEQTY ?? 1,
                                                   ConfirmFlag = a.CONFIRMFLAG,
                                                   ConfirmDate = a.CONFIRMDATE,
                                                   CheckFlag = a.CHECKFLAG,
                                                   CheckDate = a.CHECKDATE,
                                                   StopFlag = a.STOPFLAG,
                                                   StopDate = a.STOPDATE,
                                                   StopCheckFlag = a.STOPCHECKFLAG,
                                                   StopCheckDate = a.STOPCHECKDATE,
                                                   StartDate = a.STARTDATE,
                                                   EndDate = a.ENDDATE,
                                                   ExecTimes = a.EXECTIMES,
                                                   BillDate = a.BILLDATE,
                                                   DeleteFlag = a.DELETEFLAG,
                                                   FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                   ChargeGroupId = a.CHARGEGROUPID,
                                                   DoctorNo = a.DOCTORNO,
                                                   NurseNo = a.NURSENO,
                                                   DoctorName = doc_info.EMPNAME,
                                                   NurseName = nur_info.EMPNAME,
                                                   FeeNo = a.FEENO,
                                                   RegNo = a.REGNO,
                                                   OrgId = a.ORGID,
                                                   SortNumber = a.SORTNUMBER,
                                                   CreateBy = a.CREATEBY,
                                                   CreateTime = a.CREATETIME,
                                                   UpdateBy = a.UPDATEBY,
                                                   UpdateTime = a.UPDATETIME,
                                                   IsDelete = a.ISDELETE
                                               }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                                        join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                        join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                                        from doc_info in doc.DefaultIfEmpty()
                                                        join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                                        from nur_info in nur.DefaultIfEmpty()
                                                        join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                                        from freq_info in freq.DefaultIfEmpty()
                                                        join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                                                        from ipdreg_info in ipdreg.DefaultIfEmpty()
                                                        join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                                        from regfile_info in regfile.DefaultIfEmpty()
                                                        where a.ITEMTYPE == 4 && a.ISDELETE != true && b.STATUS == 0 && (b.ISDELETE == false || b.ISDELETE == null)
                                                        orderby a.CREATETIME descending
                                                        select new IpdOrder()
                                                        {
                                                            RsName=regfile_info.NAME,
                                                            OrderNo = a.ORDERNO,
                                                            OrderType = a.ORDERTYPE,
                                                            OrderName = b.CHARGEGROUPNAME,
                                                            FeeCode = a.FEECODE,
                                                            ItemType = 4,
                                                            AcRemark = a.ACREMARK,
                                                            TakeQty = a.TAKEQTY ?? 1,
                                                            TakeDay = a.TAKEDAY,
                                                            TakeFreq = a.TAKEFREQ,
                                                            TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                            TakeWay = a.TAKEWAY,
                                                            ConversionRatio = 1,
                                                            PrescribeUnits = "次",
                                                            Units = "次",
                                                            UnitPrice = 0,
                                                            ChargeQty = a.TAKEQTY ?? 1,
                                                            Amount = 0,
                                                            ConfirmFlag = a.CONFIRMFLAG,
                                                            ConfirmDate = a.CONFIRMDATE,
                                                            CheckFlag = a.CHECKFLAG,
                                                            CheckDate = a.CHECKDATE,
                                                            StopFlag = a.STOPFLAG,
                                                            StopDate = a.STOPDATE,
                                                            StopCheckFlag = a.STOPCHECKFLAG,
                                                            StopCheckDate = a.STOPCHECKDATE,
                                                            StartDate = a.STARTDATE,
                                                            EndDate = a.ENDDATE,
                                                            ExecTimes = a.EXECTIMES,
                                                            BillDate = a.BILLDATE,
                                                            DeleteFlag = a.DELETEFLAG,
                                                            FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                            ChargeGroupId = a.CHARGEGROUPID,
                                                            DoctorNo = a.DOCTORNO,
                                                            NurseNo = a.NURSENO,
                                                            DoctorName = doc_info.EMPNAME,
                                                            NurseName = nur_info.EMPNAME,
                                                            FeeNo = a.FEENO,
                                                            RegNo = a.REGNO,
                                                            OrgId = a.ORGID,
                                                            SortNumber = a.SORTNUMBER,
                                                            CreateBy = a.CREATEBY,
                                                            CreateTime = a.CREATETIME,
                                                            UpdateBy = a.UPDATEBY,
                                                            UpdateTime = a.UPDATETIME,
                                                            IsDelete = a.ISDELETE
                                                        });

            //搜寻条件
            if (request.Data.LoadType == 1)
            {
                orderData = orderData.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            if (request.Data.OrderType != 99)
            {
                orderData = orderData.Where(m => m.OrderType == request.Data.OrderType);
            }
            if (request.Data.ConfirmFlag != 99)
            {
                orderData = orderData.Where(m => m.ConfirmFlag == request.Data.ConfirmFlag);
            }
            if (request.Data.CheckFlag != 99)
            {
                orderData = orderData.Where(m => m.CheckFlag == request.Data.CheckFlag);
            }
            if (request.Data.StopFlag != 99)
            {
                orderData = orderData.Where(m => m.StopFlag == request.Data.StopFlag);
            }
            if (request.Data.CancelFlag != 99)
            {
                orderData = orderData.Where(m => m.DeleteFlag == request.Data.CancelFlag);
            }
            //if (request.Data.TimeFlag != 0)
            //{
            //    orderData = orderData.Where(m => m.StartDate >= request.Data.StartDate);
            //}
            if (request.Data.SortType == 1)
            {
                orderData = orderData.OrderBy(m => m.ConfirmFlag).ThenBy(m => m.CheckFlag).ThenBy(m => m.StopFlag).ThenBy(m => m.DeleteFlag).ThenBy(m => m.OrderNo);
            }
            else
            {
                orderData = orderData.OrderBy(m => m.CheckFlag).ThenBy(m => m.ConfirmFlag).ThenBy(m => m.StopFlag).ThenBy(m => m.DeleteFlag).ThenBy(m => m.OrderNo);
            }

            ListResult = orderData.ToList();

            foreach (var item in ListResult)
            {
                if (item.ItemType == 1)
                {
                    item.ChargeQty = Math.Ceiling(item.TakeQty * item.TakeFreqQty / item.ConversionRatio ?? 1);
                    if (item.ItemType != 4)
                    {
                        item.Amount = item.ChargeQty * item.UnitPrice;
                    }
                }
                else
                {
                    item.ChargeQty = item.TakeQty * item.TakeFreqQty;
                    if (item.ItemType != 4)
                    {
                        item.Amount = item.ChargeQty * item.UnitPrice;
                    }
                }

                if (item.ChargeGroupId != null && item.ChargeGroupId != "")
                {
                    var chargeItem = ((from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                       join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on it.CHARGEITEMID equals d.DRUGID
                                       where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 1 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                       select new CHARGEITEM
                                       {
                                           CHARGEGROUPID = it.CHARGEGROUPID,
                                           FEEITEMCOUNT = it.FEEITEMCOUNT,
                                           UNITPRICE = d.UNITPRICE

                                       }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                 join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on it.CHARGEITEMID equals d.MATERIALID
                                                 where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 2 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                                 select new CHARGEITEM
                                                 {
                                                     CHARGEGROUPID = it.CHARGEGROUPID,
                                                     FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                     UNITPRICE = d.UNITPRICE
                                                 }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                           join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on it.CHARGEITEMID equals d.SERVICEID
                                                           where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 3 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                                           select new CHARGEITEM
                                                           {
                                                               CHARGEGROUPID = it.CHARGEGROUPID,
                                                               FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                               UNITPRICE = d.UNITPRICE
                                                           })).ToList();


                    //计算套餐总价
                    var cg = chargeItem.Select(o => o.FEEITEMCOUNT * o.UNITPRICE).Sum();
                    item.UnitPrice = cg;
                    item.Amount = cg * item.ChargeQty;
                }
            }

            response.RecordsCount = ListResult.Count;
            List<IpdOrder> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = ListResult.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = ListResult.ToList();
            }

            response.Data = list;
            return response;
        }
        public BaseResponse<IList<IpdOrder>> QueryIpdOrderDtl(BaseRequest<IpdOrderFilter> request)
        {
            BaseResponse<IList<IpdOrder>> response = new BaseResponse<IList<IpdOrder>>();
            var orderDtlData = (from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                join b in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on a.FEECODE equals b.DRUGID
                                join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                from doc_info in doc.DefaultIfEmpty()
                                join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                from nur_info in nur.DefaultIfEmpty()
                                join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                from freq_info in freq.DefaultIfEmpty()
                                where a.ORDERNO == request.Data.OrderNo && a.ITEMTYPE == 1
                                orderby a.CREATETIME descending
                                select new IpdOrder()
                                {
                                    OrderNo = a.ORDERNO,
                                    OrderType = a.ORDERTYPE,
                                    OrderName = b.CNNAME,
                                    FeeCode = a.FEECODE,
                                    ItemType = a.ITEMTYPE,
                                    AcRemark = a.ACREMARK,
                                    TakeQty = a.TAKEQTY ?? 1,
                                    TakeDay = a.TAKEDAY,
                                    TakeFreq = a.TAKEFREQ,
                                    TakeFreqQty = freq_info.FREQQTY ?? 1,
                                    TakeWay = a.TAKEWAY,
                                    ConversionRatio = b.CONVERSIONRATIO,
                                    PrescribeUnits = b.PRESCRIBEUNITS,
                                    Units = b.UNITS,
                                    UnitPrice = b.UNITPRICE,
                                    ChargeQty = Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                    Amount = b.UNITPRICE * Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                    ConfirmFlag = a.CONFIRMFLAG,
                                    ConfirmDate = a.CONFIRMDATE,
                                    CheckFlag = a.CHECKFLAG,
                                    CheckDate = a.CHECKDATE,
                                    StopFlag = a.STOPFLAG,
                                    StopDate = a.STOPDATE,
                                    StopCheckFlag = a.STOPCHECKFLAG,
                                    StopCheckDate = a.STOPCHECKDATE,
                                    StartDate = a.STARTDATE,
                                    EndDate = a.ENDDATE,
                                    ExecTimes = a.EXECTIMES,
                                    BillDate = a.BILLDATE,
                                    DeleteFlag = a.DELETEFLAG,
                                    FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                    ChargeGroupId = a.CHARGEGROUPID,
                                    DoctorNo = a.DOCTORNO,
                                    NurseNo = a.NURSENO,
                                    DoctorName = doc_info.EMPNAME,
                                    NurseName = nur_info.EMPNAME,
                                    FeeNo = a.FEENO,
                                    RegNo = a.REGNO,
                                    OrgId = a.ORGID,
                                    SortNumber = a.SORTNUMBER,
                                    CreateBy = a.CREATEBY,
                                    CreateTime = a.CREATETIME,
                                    UpdateBy = a.UPDATEBY,
                                    UpdateTime = a.UPDATETIME,
                                    IsDelete = a.ISDELETE
                                }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                         join b in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on a.FEECODE equals b.MATERIALID
                                         join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                         from doc_info in doc.DefaultIfEmpty()
                                         join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                         from nur_info in nur.DefaultIfEmpty()
                                         join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                         from freq_info in freq.DefaultIfEmpty()
                                         where a.ORDERNO == request.Data.OrderNo && a.ITEMTYPE == 2
                                         orderby a.CREATETIME descending
                                         select new IpdOrder()
                                         {
                                             OrderNo = a.ORDERNO,
                                             OrderType = a.ORDERTYPE,
                                             OrderName = b.MATERIALNAME,
                                             FeeCode = a.FEECODE,
                                             ItemType = a.ITEMTYPE,
                                             AcRemark = a.ACREMARK,
                                             TakeQty = a.TAKEQTY ?? 1,
                                             TakeDay = a.TAKEDAY,
                                             TakeFreq = a.TAKEFREQ,
                                             TakeFreqQty = freq_info.FREQQTY ?? 1,
                                             TakeWay = a.TAKEWAY,
                                             ConversionRatio = 1,
                                             PrescribeUnits = b.UNITS,
                                             Units = b.UNITS,
                                             UnitPrice = b.UNITPRICE,
                                             ChargeQty = a.TAKEQTY ?? 1,
                                             Amount = b.UNITPRICE * a.TAKEQTY ?? 1,
                                             ConfirmFlag = a.CONFIRMFLAG,
                                             ConfirmDate = a.CONFIRMDATE,
                                             CheckFlag = a.CHECKFLAG,
                                             CheckDate = a.CHECKDATE,
                                             StopFlag = a.STOPFLAG,
                                             StopDate = a.STOPDATE,
                                             StopCheckFlag = a.STOPCHECKFLAG,
                                             StopCheckDate = a.STOPCHECKDATE,
                                             StartDate = a.STARTDATE,
                                             EndDate = a.ENDDATE,
                                             ExecTimes = a.EXECTIMES,
                                             BillDate = a.BILLDATE,
                                             DeleteFlag = a.DELETEFLAG,
                                             FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                             ChargeGroupId = a.CHARGEGROUPID,
                                             DoctorNo = a.DOCTORNO,
                                             NurseNo = a.NURSENO,
                                             DoctorName = doc_info.EMPNAME,
                                             NurseName = nur_info.EMPNAME,
                                             FeeNo = a.FEENO,
                                             RegNo = a.REGNO,
                                             OrgId = a.ORGID,
                                             SortNumber = a.SORTNUMBER,
                                             CreateBy = a.CREATEBY,
                                             CreateTime = a.CREATETIME,
                                             UpdateBy = a.UPDATEBY,
                                             UpdateTime = a.UPDATETIME,
                                             IsDelete = a.ISDELETE
                                         }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                                  join b in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on a.FEECODE equals b.SERVICEID
                                                  join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                                  from doc_info in doc.DefaultIfEmpty()
                                                  join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                                  from nur_info in nur.DefaultIfEmpty()
                                                  join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                                  from freq_info in freq.DefaultIfEmpty()
                                                  where a.ORDERNO == request.Data.OrderNo && a.ITEMTYPE == 3
                                                  orderby a.CREATETIME descending
                                                  select new IpdOrder()
                                                  {
                                                      OrderNo = a.ORDERNO,
                                                      OrderType = a.ORDERTYPE,
                                                      OrderName = b.SERVICENAME,
                                                      FeeCode = a.FEECODE,
                                                      ItemType = a.ITEMTYPE,
                                                      AcRemark = a.ACREMARK,
                                                      TakeQty = a.TAKEQTY ?? 1,
                                                      TakeDay = a.TAKEDAY,
                                                      TakeFreq = a.TAKEFREQ,
                                                      TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                      TakeWay = a.TAKEWAY,
                                                      ConversionRatio = 1,
                                                      PrescribeUnits = b.UNITS,
                                                      Units = b.UNITS,
                                                      UnitPrice = b.UNITPRICE,
                                                      ChargeQty = a.TAKEQTY ?? 1,
                                                      Amount = b.UNITPRICE * a.TAKEQTY ?? 1,
                                                      ConfirmFlag = a.CONFIRMFLAG,
                                                      ConfirmDate = a.CONFIRMDATE,
                                                      CheckFlag = a.CHECKFLAG,
                                                      CheckDate = a.CHECKDATE,
                                                      StopFlag = a.STOPFLAG,
                                                      StopDate = a.STOPDATE,
                                                      StopCheckFlag = a.STOPCHECKFLAG,
                                                      StopCheckDate = a.STOPCHECKDATE,
                                                      StartDate = a.STARTDATE,
                                                      EndDate = a.ENDDATE,
                                                      ExecTimes = a.EXECTIMES,
                                                      BillDate = a.BILLDATE,
                                                      DeleteFlag = a.DELETEFLAG,
                                                      FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                      ChargeGroupId = a.CHARGEGROUPID,
                                                      DoctorNo = a.DOCTORNO,
                                                      NurseNo = a.NURSENO,
                                                      DoctorName = doc_info.EMPNAME,
                                                      NurseName = nur_info.EMPNAME,
                                                      FeeNo = a.FEENO,
                                                      RegNo = a.REGNO,
                                                      OrgId = a.ORGID,
                                                      SortNumber = a.SORTNUMBER,
                                                      CreateBy = a.CREATEBY,
                                                      CreateTime = a.CREATETIME,
                                                      UpdateBy = a.UPDATEBY,
                                                      UpdateTime = a.UPDATETIME,
                                                      IsDelete = a.ISDELETE
                                                  }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                                           join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                           join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                                           from doc_info in doc.DefaultIfEmpty()
                                                           join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                                           from nur_info in nur.DefaultIfEmpty()
                                                           join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                                           from freq_info in freq.DefaultIfEmpty()
                                                           where a.ORDERNO == request.Data.OrderNo && a.ITEMTYPE == 4
                                                           orderby a.CREATETIME descending
                                                           select new IpdOrder()
                                                           {
                                                               OrderNo = a.ORDERNO,
                                                               OrderType = a.ORDERTYPE,
                                                               OrderName = b.CHARGEGROUPNAME,
                                                               FeeCode = a.FEECODE,
                                                               ItemType = a.ITEMTYPE,
                                                               AcRemark = a.ACREMARK,
                                                               TakeQty = a.TAKEQTY ?? 1,
                                                               TakeDay = a.TAKEDAY,
                                                               TakeFreq = a.TAKEFREQ,
                                                               TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                               TakeWay = a.TAKEWAY,
                                                               ConversionRatio = 1,
                                                               PrescribeUnits = "次",
                                                               Units = "次",
                                                               UnitPrice = 0,
                                                               ChargeQty = a.TAKEQTY ?? 1,
                                                               Amount = 0,
                                                               ConfirmFlag = a.CONFIRMFLAG,
                                                               ConfirmDate = a.CONFIRMDATE,
                                                               CheckFlag = a.CHECKFLAG,
                                                               CheckDate = a.CHECKDATE,
                                                               StopFlag = a.STOPFLAG,
                                                               StopDate = a.STOPDATE,
                                                               StopCheckFlag = a.STOPCHECKFLAG,
                                                               StopCheckDate = a.STOPCHECKDATE,
                                                               StartDate = a.STARTDATE,
                                                               EndDate = a.ENDDATE,
                                                               ExecTimes = a.EXECTIMES,
                                                               BillDate = a.BILLDATE,
                                                               DeleteFlag = a.DELETEFLAG,
                                                               FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                               ChargeGroupId = a.CHARGEGROUPID,
                                                               DoctorNo = a.DOCTORNO,
                                                               NurseNo = a.NURSENO,
                                                               DoctorName = doc_info.EMPNAME,
                                                               NurseName = nur_info.EMPNAME,
                                                               FeeNo = a.FEENO,
                                                               RegNo = a.REGNO,
                                                               OrgId = a.ORGID,
                                                               SortNumber = a.SORTNUMBER,
                                                               CreateBy = a.CREATEBY,
                                                               CreateTime = a.CREATETIME,
                                                               UpdateBy = a.UPDATEBY,
                                                               UpdateTime = a.UPDATETIME,
                                                               IsDelete = a.ISDELETE
                                                           }).ToList();


            foreach (var item in orderDtlData)
            {
                if (item.ItemType == 1)
                {
                    item.ChargeQty = Math.Ceiling(item.TakeQty * item.TakeFreqQty / item.ConversionRatio ?? 1);
                    item.Amount = item.ChargeQty * item.UnitPrice;
                }
                else
                {
                    item.ChargeQty = item.TakeQty * item.TakeFreqQty;
                    item.Amount = item.ChargeQty * item.UnitPrice;
                }
            }
            response.Data = orderDtlData;
            return response;
        }
        public BaseResponse<IList<CHARGEITEM>> QueryChargeItem(BaseRequest<IpdOrderFilter> request)
        {
            List<CHARGEITEM> ListResult = new List<CHARGEITEM>();
            BaseResponse<IList<CHARGEITEM>> response = new BaseResponse<IList<CHARGEITEM>>();
            if (request.Data.ItemType == 4)
            {
                var chargeGrpData = (from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                     join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                     join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                     join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on c.CHARGEITEMID equals d.DRUGID
                                     where a.CHARGEGROUPID == request.Data.ChargeGroupId && a.ISDELETE != true && c.CHARGEITEMTYPE == 1
                                     orderby c.CGCIID descending
                                     select new CHARGEITEM()
                                     {
                                         NAME = d.CNNAME,
                                         CHARGERECORDTYPE = 1,
                                         PRESCRIBEUNITS = d.PRESCRIBEUNITS,
                                         UNITPRICE = d.UNITPRICE,
                                         QTY = a.TAKEQTY ?? 1,
                                         ChargeQty = Math.Ceiling(a.TAKEQTY ?? 1 / d.CONVERSIONRATIO ?? 1),
                                         UNITS = d.UNITS,
                                         COST = d.UNITPRICE * Math.Ceiling(a.TAKEQTY ?? 1 / d.CONVERSIONRATIO ?? 1)
                                     }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                              join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                              join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                              join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on c.CHARGEITEMID equals d.MATERIALID
                                              where a.CHARGEGROUPID == request.Data.ChargeGroupId && a.ISDELETE != true && c.CHARGEITEMTYPE == 2
                                              orderby c.CGCIID ascending
                                              select new CHARGEITEM()
                                              {
                                                  NAME = d.MATERIALNAME,
                                                  CHARGERECORDTYPE = 2,
                                                  PRESCRIBEUNITS = d.UNITS,
                                                  UNITPRICE = d.UNITPRICE,
                                                  QTY = a.TAKEQTY ?? 1,
                                                  ChargeQty = a.TAKEQTY ?? 1,
                                                  UNITS = d.UNITS,
                                                  COST = d.UNITPRICE * a.TAKEQTY ?? 1
                                              }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                                       join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                       join c in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                                       join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on c.CHARGEITEMID equals d.SERVICEID
                                                       where a.CHARGEGROUPID == request.Data.ChargeGroupId && a.ISDELETE != true && c.CHARGEITEMTYPE == 3
                                                       orderby c.CGCIID descending
                                                       select new CHARGEITEM()
                                                       {
                                                           NAME = d.SERVICENAME,
                                                           CHARGERECORDTYPE = 3,
                                                           PRESCRIBEUNITS = d.UNITS,
                                                           UNITPRICE = d.UNITPRICE,
                                                           QTY = a.TAKEQTY ?? 1,
                                                           ChargeQty = a.TAKEQTY ?? 1,
                                                           UNITS = d.UNITS,
                                                           COST = d.UNITPRICE * a.TAKEQTY ?? 1
                                                       }).ToList();

                chargeGrpData = chargeGrpData.ToList().OrderByDescending(m => m.CGCRID).ToList();
                response.RecordsCount = chargeGrpData.Count;
                List<CHARGEITEM> list = null;
                if (request != null && request.PageSize > 0)
                {
                    list = chargeGrpData.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = chargeGrpData.ToList();
                }
                response.Data = list;
            }
            else
            {
                var chargeItemData = (from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                      join b in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on a.FEECODE equals b.DRUGID
                                      where a.FEECODE == request.Data.FeeCode && a.ISDELETE != true && a.ITEMTYPE == 1
                                      orderby a.CREATETIME descending
                                      select new CHARGEITEM()
                                      {
                                          OrderNo = a.ORDERNO,
                                          FeeCode = a.FEECODE,
                                          NAME = b.CNNAME,
                                          ITEMTYPE = a.ITEMTYPE,
                                          CHARGERECORDTYPE = 1,
                                          PRESCRIBEUNITS = b.PRESCRIBEUNITS,
                                          UNITPRICE = b.UNITPRICE,
                                          QTY = a.TAKEQTY ?? 1,
                                          ChargeQty = Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                          UNITS = b.UNITS,
                                          COST = b.UNITPRICE * Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                          CREATETIME = a.CREATETIME
                                      }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                               join b in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on a.FEECODE equals b.MATERIALID
                                               where a.FEECODE == request.Data.FeeCode && a.ISDELETE != true && a.ITEMTYPE == 2
                                               orderby a.CREATETIME descending
                                               select new CHARGEITEM()
                                               {
                                                   OrderNo = a.ORDERNO,
                                                   FeeCode = a.FEECODE,
                                                   NAME = b.MATERIALNAME,
                                                   ITEMTYPE = a.ITEMTYPE,
                                                   CHARGERECORDTYPE = 2,
                                                   PRESCRIBEUNITS = b.UNITS,
                                                   UNITPRICE = b.UNITPRICE,
                                                   QTY = a.TAKEQTY ?? 1,
                                                   ChargeQty = a.TAKEQTY ?? 1,
                                                   UNITS = b.UNITS,
                                                   COST = b.UNITPRICE * a.TAKEQTY ?? 1,
                                                   CREATETIME = a.CREATETIME
                                               }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                                        join b in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on a.FEECODE equals b.SERVICEID
                                                        where a.FEECODE == request.Data.FeeCode && a.ISDELETE != true && a.ITEMTYPE == 3
                                                        orderby a.CREATETIME descending
                                                        select new CHARGEITEM()
                                                        {
                                                            OrderNo = a.ORDERNO,
                                                            FeeCode = a.FEECODE,
                                                            NAME = b.SERVICENAME,
                                                            ITEMTYPE = a.ITEMTYPE,
                                                            CHARGERECORDTYPE = 3,
                                                            PRESCRIBEUNITS = b.UNITS,
                                                            UNITPRICE = b.UNITPRICE,
                                                            QTY = a.TAKEQTY ?? 1,
                                                            ChargeQty = a.TAKEQTY ?? 1,
                                                            UNITS = b.UNITS,
                                                            COST = b.UNITPRICE * a.TAKEQTY ?? 1,
                                                            CREATETIME = a.CREATETIME
                                                        });

                chargeItemData = chargeItemData.Where(m => m.OrderNo == request.Data.OrderNo);
                chargeItemData = chargeItemData.OrderByDescending(m => m.CREATETIME);
                ListResult = chargeItemData.ToList();
                response.RecordsCount = ListResult.Count;
                List<CHARGEITEM> list = null;
                if (request != null && request.PageSize > 0)
                {
                    list = chargeItemData.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    list = chargeItemData.ToList();
                }
                response.Data = list;
            }

            return response;
        }
        public BaseResponse<IpdOrder> SaveIpdOrder(IpdOrder request)
        {
            var response = new BaseResponse<IpdOrder>();
            try
            {
                unitOfWork.BeginTransaction();
                Mapper.CreateMap<LTC_IPDORDER, IpdOrder>();
                var model = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(x => x.ORDERNO == request.OrderNo).FirstOrDefault();
                var orderInfo = Mapper.Map<IpdOrder>(model);
                if (orderInfo != null)
                {
                    if (orderInfo.ConfirmFlag != request.ConfirmFlag)
                    {
                        orderInfo.ConfirmFlag = request.ConfirmFlag;
                        orderInfo.ConfirmDate = DateTime.Now;
                    }

                    if (orderInfo.CheckFlag != request.CheckFlag)
                    {
                        orderInfo.CheckFlag = request.CheckFlag;
                        orderInfo.NurseNo = SecurityHelper.CurrentPrincipal.EmpNo;
                        orderInfo.CheckDate = DateTime.Now;
                    }

                    if (orderInfo.StopFlag != request.StopFlag)
                    {
                        orderInfo.StopFlag = request.StopFlag;
                        orderInfo.StopDate = DateTime.Now;
                        orderInfo.StopCheckFlag = request.StopCheckFlag;
                        orderInfo.StopCheckDate = DateTime.Now;
                    }

                    if (orderInfo.DeleteFlag != request.DeleteFlag)
                    {
                        orderInfo.DeleteFlag = request.DeleteFlag;
                    }

                    orderInfo.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                    orderInfo.UpdateTime = DateTime.Now;
                    base.Save<LTC_IPDORDER, IpdOrder>(orderInfo, (q) => q.ORDERNO == request.OrderNo);
                    unitOfWork.Save();
                    unitOfWork.Commit();
                }
                else
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "变更失败，请联系管理员";
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = "变更异常，请联系管理员！";
            }
            return response;
        }
        public BaseResponse<List<NewIpdOrder>> SaveIpdOrderList(IpdOrderList request)
        {
            BaseResponse<List<NewIpdOrder>> response = new BaseResponse<List<NewIpdOrder>>();
            try
            {
                unitOfWork.BeginTransaction();
                foreach (var item in request.IpdOrderLists)
                {
                    if (item.NOrderNo != -1)
                    {
                        var l_io = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(m => m.ORDERNO == item.NOrderNo).ToList();
                        if (l_io != null)
                        {
                            if (l_io.Count > 0)
                            {
                                if (item.NStartDate != null && item.NEndDate != null)
                                {
                                    DateTime dt1 = Convert.ToDateTime(item.NStartDate);
                                    DateTime dt2 = Convert.ToDateTime(item.NEndDate);
                                    TimeSpan ts = dt2 - dt1;
                                    l_io[0].TAKEDAY = ts.Days;
                                }
                                l_io[0].ORDERTYPE = item.NOrderType;
                                l_io[0].ACREMARK = item.NAcRemark;
                                l_io[0].TAKEQTY = item.NTakeQty;
                                l_io[0].TAKEFREQ = item.NTakeFreq;
                                l_io[0].TAKEWAY = item.NTakeWay;
                                l_io[0].STARTDATE = item.NStartDate;
                                l_io[0].ENDDATE = item.NEndDate;
                                l_io[0].FIRSTDAYQUANTITY = item.NFirstDayQuantity;
                                l_io[0].SORTNUMBER = item.NSortNumber;
                                l_io[0].UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                l_io[0].UPDATETIME = DateTime.Now;
                                unitOfWork.GetRepository<LTC_IPDORDER>().Update(l_io[0]);
                            }
                            else
                            {
                                LTC_IPDORDER ipdordermodel = new LTC_IPDORDER();
                                if (item.NStartDate != null && item.NEndDate != null)
                                {
                                    DateTime dt1 = Convert.ToDateTime(item.NStartDate);
                                    DateTime dt2 = Convert.ToDateTime(item.NEndDate);
                                    TimeSpan ts = dt2 - dt1;
                                    ipdordermodel.TAKEDAY = ts.Days;
                                }

                                ipdordermodel.ORDERTYPE = item.NOrderType;
                                ipdordermodel.FEECODE = item.NFeeCode;
                                ipdordermodel.ITEMTYPE = item.NItemType;
                                ipdordermodel.ACREMARK = item.NAcRemark;
                                ipdordermodel.TAKEQTY = item.NTakeQty;
                                ipdordermodel.TAKEFREQ = item.NTakeFreq;
                                ipdordermodel.TAKEWAY = item.NTakeWay;
                                ipdordermodel.CONFIRMFLAG = 0;
                                ipdordermodel.CHECKFLAG = 0;
                                ipdordermodel.STOPFLAG = 0;
                                ipdordermodel.STOPCHECKFLAG = 0;
                                ipdordermodel.STARTDATE = item.NStartDate;
                                ipdordermodel.ENDDATE = item.NEndDate;
                                ipdordermodel.EXECTIMES = 0;
                                ipdordermodel.BILLDATE = DateTime.Now;
                                ipdordermodel.DELETEFLAG = 0;
                                ipdordermodel.FIRSTDAYQUANTITY = item.NFirstDayQuantity;
                                ipdordermodel.SORTNUMBER = 1;
                                ipdordermodel.CHARGEGROUPID = item.NChargeGroupId;
                                ipdordermodel.DOCTORNO = SecurityHelper.CurrentPrincipal.EmpNo;
                                ipdordermodel.FEENO = item.FeeNo;
                                ipdordermodel.REGNO = item.RegNo;
                                ipdordermodel.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                                ipdordermodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                ipdordermodel.CREATETIME = DateTime.Now;
                                ipdordermodel.ISDELETE = false;
                                unitOfWork.GetRepository<LTC_IPDORDER>().Insert(ipdordermodel);
                            }
                        }
                    }
                    unitOfWork.Save();
                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = "保存异常，请联系管理员！";
            }
            return response;
        }
        public BaseResponse<IList<IpdOrder>> QueryOrderPostRec(BaseRequest<IpdOrderFilter> request)
        {
            List<IpdOrder> ListResult = new List<IpdOrder>();
            List<CHARGEGROUP> ChargeItemResult = new List<CHARGEGROUP>();
            BaseResponse<IList<IpdOrder>> response = new BaseResponse<IList<IpdOrder>>();
            var orderData = (from iopr in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                             join a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on iopr.ORDERNO equals a.ORDERNO
                             join b in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on a.FEECODE equals b.DRUGID
                             join bb in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on iopr.ORDERNO equals bb.ORDERNO
                             join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                             from doc_info in doc.DefaultIfEmpty()
                             join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into cnur
                             from checknur_info in cnur.DefaultIfEmpty()
                             join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on iopr.NURSENO equals e.EMPNO into pnur
                             from postnur_info in pnur.DefaultIfEmpty()
                             join f in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals f.FREQNO into freq
                             from freq_info in freq.DefaultIfEmpty()
                             join ff in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals ff.FEENO into ipdreg
                             from ipdreg_info in ipdreg.DefaultIfEmpty()
                             join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                             from regfile_info in regfile.DefaultIfEmpty()
                             where a.ITEMTYPE == 1 && a.ISDELETE != true && iopr.ISDELETE != true && b.STATUS == 0 && (b.ISDELETE == false || b.ISDELETE == null)
                             orderby a.CREATETIME descending
                             select new IpdOrder()
                             {
                                 RsName = regfile_info.NAME,
                                 OrderPostRecNo = iopr.ORDERPOSTRECNO,
                                 PostDate = iopr.POSTDATE,
                                 PostNurseNo = iopr.NURSENO,
                                 PostNurseName = postnur_info.EMPNAME,
                                 PostIsDelete = iopr.ISDELETE,
                                 Status = bb.STATUS,
                                 OrderNo = a.ORDERNO,
                                 OrderType = a.ORDERTYPE,
                                 OrderName = b.CNNAME,
                                 FeeCode = a.FEECODE,
                                 ItemType = a.ITEMTYPE,
                                 AcRemark = a.ACREMARK,
                                 TakeQty = a.TAKEQTY ?? 1,
                                 TakeDay = a.TAKEDAY,
                                 TakeFreq = a.TAKEFREQ,
                                 TakeFreqQty = freq_info.FREQQTY ?? 1,
                                 TakeWay = a.TAKEWAY,
                                 ConversionRatio = b.CONVERSIONRATIO,
                                 PrescribeUnits = b.PRESCRIBEUNITS,
                                 Units = b.UNITS,
                                 UnitPrice = b.UNITPRICE,
                                 ChargeQty = Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                 Amount = b.UNITPRICE * Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                 ConfirmFlag = a.CONFIRMFLAG,
                                 ConfirmDate = a.CONFIRMDATE,
                                 CheckFlag = a.CHECKFLAG,
                                 CheckDate = a.CHECKDATE,
                                 StopFlag = a.STOPFLAG,
                                 StopDate = a.STOPDATE,
                                 StopCheckFlag = a.STOPCHECKFLAG,
                                 StopCheckDate = a.STOPCHECKDATE,
                                 StartDate = a.STARTDATE,
                                 EndDate = a.ENDDATE,
                                 ExecTimes = a.EXECTIMES,
                                 BillDate = a.BILLDATE,
                                 DeleteFlag = a.DELETEFLAG,
                                 FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                 ChargeGroupId = a.CHARGEGROUPID,
                                 DoctorNo = a.DOCTORNO,
                                 NurseNo = a.NURSENO,
                                 DoctorName = doc_info.EMPNAME,
                                 NurseName = checknur_info.EMPNAME,
                                 FeeNo = a.FEENO,
                                 RegNo = a.REGNO,
                                 OrgId = a.ORGID,
                                 SortNumber = a.SORTNUMBER,
                                 CreateBy = a.CREATEBY,
                                 CreateTime = a.CREATETIME,
                                 UpdateBy = a.UPDATEBY,
                                 UpdateTime = a.UPDATETIME,
                                 IsDelete = iopr.ISDELETE
                             }).Union(from iopr in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                      join a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on iopr.ORDERNO equals a.ORDERNO
                                      join b in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on a.FEECODE equals b.MATERIALID
                                      join bb in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on iopr.ORDERNO equals bb.ORDERNO
                                      join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                      from doc_info in doc.DefaultIfEmpty()
                                      join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into cnur
                                      from checknur_info in cnur.DefaultIfEmpty()
                                      join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on iopr.NURSENO equals e.EMPNO into pnur
                                      from postnur_info in pnur.DefaultIfEmpty()
                                      join f in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals f.FREQNO into freq
                                      from freq_info in freq.DefaultIfEmpty()
                                      join ff in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals ff.FEENO into ipdreg
                                      from ipdreg_info in ipdreg.DefaultIfEmpty()
                                      join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                      from regfile_info in regfile.DefaultIfEmpty()
                                      where a.ITEMTYPE == 2 && a.ISDELETE != true && iopr.ISDELETE != true && b.STATUS == 0 && (b.ISDELETE == false || b.ISDELETE == null)
                                      orderby a.CREATETIME descending
                                      select new IpdOrder()
                                      {
                                          RsName = regfile_info.NAME,
                                          OrderPostRecNo = iopr.ORDERPOSTRECNO,
                                          PostDate = iopr.POSTDATE,
                                          PostNurseNo = iopr.NURSENO,
                                          PostNurseName = postnur_info.EMPNAME,
                                          PostIsDelete = iopr.ISDELETE,
                                          Status = bb.STATUS,
                                          OrderNo = a.ORDERNO,
                                          OrderType = a.ORDERTYPE,
                                          OrderName = b.MATERIALNAME,
                                          FeeCode = a.FEECODE,
                                          ItemType = a.ITEMTYPE,
                                          AcRemark = a.ACREMARK,
                                          TakeQty = a.TAKEQTY ?? 1,
                                          TakeDay = a.TAKEDAY,
                                          TakeFreq = a.TAKEFREQ,
                                          TakeFreqQty = freq_info.FREQQTY ?? 1,
                                          TakeWay = a.TAKEWAY,
                                          ConversionRatio = 1,
                                          PrescribeUnits = b.UNITS,
                                          Units = b.UNITS,
                                          UnitPrice = b.UNITPRICE,
                                          ChargeQty = a.TAKEQTY ?? 1,
                                          Amount = b.UNITPRICE * a.TAKEQTY ?? 1,
                                          ConfirmFlag = a.CONFIRMFLAG,
                                          ConfirmDate = a.CONFIRMDATE,
                                          CheckFlag = a.CHECKFLAG,
                                          CheckDate = a.CHECKDATE,
                                          StopFlag = a.STOPFLAG,
                                          StopDate = a.STOPDATE,
                                          StopCheckFlag = a.STOPCHECKFLAG,
                                          StopCheckDate = a.STOPCHECKDATE,
                                          StartDate = a.STARTDATE,
                                          EndDate = a.ENDDATE,
                                          ExecTimes = a.EXECTIMES,
                                          BillDate = a.BILLDATE,
                                          DeleteFlag = a.DELETEFLAG,
                                          FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                          ChargeGroupId = a.CHARGEGROUPID,
                                          DoctorNo = a.DOCTORNO,
                                          NurseNo = a.NURSENO,
                                          DoctorName = doc_info.EMPNAME,
                                          NurseName = checknur_info.EMPNAME,
                                          FeeNo = a.FEENO,
                                          RegNo = a.REGNO,
                                          OrgId = a.ORGID,
                                          SortNumber = a.SORTNUMBER,
                                          CreateBy = a.CREATEBY,
                                          CreateTime = a.CREATETIME,
                                          UpdateBy = a.UPDATEBY,
                                          UpdateTime = a.UPDATETIME,
                                          IsDelete = iopr.ISDELETE
                                      }).Union(from iopr in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                               join a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on iopr.ORDERNO equals a.ORDERNO
                                               join b in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on a.FEECODE equals b.SERVICEID
                                               join bb in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on iopr.ORDERNO equals bb.ORDERNO
                                               join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                               from doc_info in doc.DefaultIfEmpty()
                                               join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into cnur
                                               from checknur_info in cnur.DefaultIfEmpty()
                                               join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on iopr.NURSENO equals e.EMPNO into pnur
                                               from postnur_info in pnur.DefaultIfEmpty()
                                               join f in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals f.FREQNO into freq
                                               from freq_info in freq.DefaultIfEmpty()
                                               join ff in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals ff.FEENO into ipdreg
                                               from ipdreg_info in ipdreg.DefaultIfEmpty()
                                               join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                               from regfile_info in regfile.DefaultIfEmpty()
                                               where a.ITEMTYPE == 3 && a.ISDELETE != true && iopr.ISDELETE != true && b.STATUS == 0 && (b.ISDELETE == false || b.ISDELETE == null)
                                               orderby a.CREATETIME descending
                                               select new IpdOrder()
                                               {
                                                   RsName = regfile_info.NAME,
                                                   OrderPostRecNo = iopr.ORDERPOSTRECNO,
                                                   PostDate = iopr.POSTDATE,
                                                   PostNurseNo = iopr.NURSENO,
                                                   PostNurseName = postnur_info.EMPNAME,
                                                   PostIsDelete = iopr.ISDELETE,
                                                   Status = bb.STATUS,
                                                   OrderNo = a.ORDERNO,
                                                   OrderType = a.ORDERTYPE,
                                                   OrderName = b.SERVICENAME,
                                                   FeeCode = a.FEECODE,
                                                   ItemType = a.ITEMTYPE,
                                                   AcRemark = a.ACREMARK,
                                                   TakeQty = a.TAKEQTY ?? 1,
                                                   TakeDay = a.TAKEDAY,
                                                   TakeFreq = a.TAKEFREQ,
                                                   TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                   TakeWay = a.TAKEWAY,
                                                   ConversionRatio = 1,
                                                   PrescribeUnits = b.UNITS,
                                                   Units = b.UNITS,
                                                   UnitPrice = b.UNITPRICE,
                                                   ChargeQty = a.TAKEQTY ?? 1,
                                                   Amount = b.UNITPRICE * a.TAKEQTY ?? 1,
                                                   ConfirmFlag = a.CONFIRMFLAG,
                                                   ConfirmDate = a.CONFIRMDATE,
                                                   CheckFlag = a.CHECKFLAG,
                                                   CheckDate = a.CHECKDATE,
                                                   StopFlag = a.STOPFLAG,
                                                   StopDate = a.STOPDATE,
                                                   StopCheckFlag = a.STOPCHECKFLAG,
                                                   StopCheckDate = a.STOPCHECKDATE,
                                                   StartDate = a.STARTDATE,
                                                   EndDate = a.ENDDATE,
                                                   ExecTimes = a.EXECTIMES,
                                                   BillDate = a.BILLDATE,
                                                   DeleteFlag = a.DELETEFLAG,
                                                   FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                   ChargeGroupId = a.CHARGEGROUPID,
                                                   DoctorNo = a.DOCTORNO,
                                                   NurseNo = a.NURSENO,
                                                   DoctorName = doc_info.EMPNAME,
                                                   NurseName = checknur_info.EMPNAME,
                                                   FeeNo = a.FEENO,
                                                   RegNo = a.REGNO,
                                                   OrgId = a.ORGID,
                                                   SortNumber = a.SORTNUMBER,
                                                   CreateBy = a.CREATEBY,
                                                   CreateTime = a.CREATETIME,
                                                   UpdateBy = a.UPDATEBY,
                                                   UpdateTime = a.UPDATETIME,
                                                   IsDelete = iopr.ISDELETE
                                               }).Union(from iopr in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                                        join a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on iopr.ORDERNO equals a.ORDERNO
                                                        join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                        join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                                        from doc_info in doc.DefaultIfEmpty()
                                                        join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into cnur
                                                        from checknur_info in cnur.DefaultIfEmpty()
                                                        join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on iopr.NURSENO equals e.EMPNO into pnur
                                                        from postnur_info in pnur.DefaultIfEmpty()
                                                        join f in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals f.FREQNO into freq
                                                        from freq_info in freq.DefaultIfEmpty()
                                                        join ff in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals ff.FEENO into ipdreg
                                                        from ipdreg_info in ipdreg.DefaultIfEmpty()
                                                        join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                                        from regfile_info in regfile.DefaultIfEmpty()
                                                        where a.ITEMTYPE == 4 && a.ISDELETE != true && iopr.ISDELETE != true && b.STATUS == 0 && (b.ISDELETE == false || b.ISDELETE == null)
                                                        orderby a.CREATETIME descending
                                                        select new IpdOrder()
                                                        {
                                                            RsName=regfile_info.NAME,
                                                            OrderPostRecNo = iopr.ORDERPOSTRECNO,
                                                            PostDate = iopr.POSTDATE,
                                                            PostNurseNo = iopr.NURSENO,
                                                            PostNurseName = postnur_info.EMPNAME,
                                                            PostIsDelete = iopr.ISDELETE,
                                                            Status = 0,
                                                            OrderNo = a.ORDERNO,
                                                            OrderType = a.ORDERTYPE,
                                                            OrderName = b.CHARGEGROUPNAME,
                                                            FeeCode = a.FEECODE,
                                                            ItemType = 4,
                                                            AcRemark = a.ACREMARK,
                                                            TakeQty = a.TAKEQTY ?? 1,
                                                            TakeDay = a.TAKEDAY,
                                                            TakeFreq = a.TAKEFREQ,
                                                            TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                            TakeWay = a.TAKEWAY,
                                                            ConversionRatio = 1,
                                                            PrescribeUnits = "次",
                                                            Units = "次",
                                                            UnitPrice = 0,
                                                            ChargeQty = a.TAKEQTY ?? 1,
                                                            Amount = 0,
                                                            ConfirmFlag = a.CONFIRMFLAG,
                                                            ConfirmDate = a.CONFIRMDATE,
                                                            CheckFlag = a.CHECKFLAG,
                                                            CheckDate = a.CHECKDATE,
                                                            StopFlag = a.STOPFLAG,
                                                            StopDate = a.STOPDATE,
                                                            StopCheckFlag = a.STOPCHECKFLAG,
                                                            StopCheckDate = a.STOPCHECKDATE,
                                                            StartDate = a.STARTDATE,
                                                            EndDate = a.ENDDATE,
                                                            ExecTimes = a.EXECTIMES,
                                                            BillDate = a.BILLDATE,
                                                            DeleteFlag = a.DELETEFLAG,
                                                            FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                            ChargeGroupId = a.CHARGEGROUPID,
                                                            DoctorNo = a.DOCTORNO,
                                                            NurseNo = a.NURSENO,
                                                            DoctorName = doc_info.EMPNAME,
                                                            NurseName = checknur_info.EMPNAME,
                                                            FeeNo = a.FEENO,
                                                            RegNo = a.REGNO,
                                                            OrgId = a.ORGID,
                                                            SortNumber = a.SORTNUMBER,
                                                            CreateBy = a.CREATEBY,
                                                            CreateTime = a.CREATETIME,
                                                            UpdateBy = a.UPDATEBY,
                                                            UpdateTime = a.UPDATETIME,
                                                            IsDelete = iopr.ISDELETE
                                                        });
            //搜寻条件
            if (request.Data.LoadType == 1)
            {
                orderData = orderData.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            orderData = orderData.OrderByDescending(m => m.OrderNo).ThenByDescending(m => m.PostDate).ThenByDescending(m => m.CreateTime);
            ListResult = orderData.Distinct().ToList();
            foreach (var item in ListResult)
            {
                if (item.ItemType == 1)
                {
                    item.ChargeQty = Math.Ceiling(item.TakeQty * item.TakeFreqQty / item.ConversionRatio ?? 1);
                    if (item.ItemType != 4)
                    {
                        item.Amount = item.ChargeQty * item.UnitPrice;
                    }
                }
                else
                {
                    item.ChargeQty = item.TakeQty * item.TakeFreqQty;
                    if (item.ItemType != 4)
                    {
                        item.Amount = item.ChargeQty * item.UnitPrice;
                    }
                }
                if (item.ChargeGroupId != null && item.ChargeGroupId != "")
                {
                    var chargeItem = ((from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                       join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on it.CHARGEITEMID equals d.DRUGID
                                       where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 1 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                       select new CHARGEITEM
                                       {
                                           CHARGEGROUPID = it.CHARGEGROUPID,
                                           FEEITEMCOUNT = it.FEEITEMCOUNT,
                                           UNITPRICE = d.UNITPRICE

                                       }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                 join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on it.CHARGEITEMID equals d.MATERIALID
                                                 where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 2 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                                 select new CHARGEITEM
                                                 {
                                                     CHARGEGROUPID = it.CHARGEGROUPID,
                                                     FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                     UNITPRICE = d.UNITPRICE
                                                 }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                           join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on it.CHARGEITEMID equals d.SERVICEID
                                                           where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 3 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                                           select new CHARGEITEM
                                                           {
                                                               CHARGEGROUPID = it.CHARGEGROUPID,
                                                               FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                               UNITPRICE = d.UNITPRICE
                                                           })).ToList();


                    //计算套餐总价
                    var cg = chargeItem.Select(o => o.FEEITEMCOUNT * o.UNITPRICE).Sum();
                    item.UnitPrice = cg;
                    item.Amount = cg * item.ChargeQty;
                }
            }

            response.RecordsCount = ListResult.Count;
            List<IpdOrder> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = ListResult.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = ListResult.ToList();
            }

            response.Data = list;
            return response;
        }
        public BaseResponse<IList<IpdOrder>> QueryLoadOrder(BaseRequest<IpdOrderFilter> request)
        {
            List<IpdOrder> SendOrderList = new List<IpdOrder>();
            List<IpdOrder> ListResult = new List<IpdOrder>();
            List<CHARGEGROUP> ChargeItemResult = new List<CHARGEGROUP>();
            BaseResponse<IList<IpdOrder>> response = new BaseResponse<IList<IpdOrder>>();
            var orderData = (from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                             join b in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on a.FEECODE equals b.DRUGID
                             join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                             from doc_info in doc.DefaultIfEmpty()
                             join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                             from nur_info in nur.DefaultIfEmpty()
                             join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                             from freq_info in freq.DefaultIfEmpty()
                             join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                             from ipdreg_info in ipdreg.DefaultIfEmpty()
                             join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                             from regfile_info in regfile.DefaultIfEmpty()
                             where a.ITEMTYPE == 1 && a.ISDELETE != true && b.STATUS == 0 && b.ISDELETE != true
                             orderby a.CREATETIME descending
                             select new IpdOrder()
                             {
                                 RsName = regfile_info.NAME,
                                 OrderNo = a.ORDERNO,
                                 OrderType = a.ORDERTYPE,
                                 OrderName = b.CNNAME,
                                 FeeCode = a.FEECODE,
                                 ItemType = a.ITEMTYPE,
                                 AcRemark = a.ACREMARK,
                                 TakeQty = a.TAKEQTY ?? 1,
                                 TakeDay = a.TAKEDAY,
                                 TakeFreq = a.TAKEFREQ,
                                 TakeFreqQty = freq_info.FREQQTY ?? 1,
                                 TakeWay = a.TAKEWAY,
                                 ConversionRatio = b.CONVERSIONRATIO,
                                 PrescribeUnits = b.PRESCRIBEUNITS,
                                 Units = b.UNITS,
                                 UnitPrice = b.UNITPRICE,
                                 ChargeQty = Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                 Amount = b.UNITPRICE * Math.Ceiling(a.TAKEQTY ?? 1 / b.CONVERSIONRATIO ?? 1),
                                 ConfirmFlag = a.CONFIRMFLAG,
                                 ConfirmDate = a.CONFIRMDATE,
                                 CheckFlag = a.CHECKFLAG,
                                 CheckDate = a.CHECKDATE,
                                 StopFlag = a.STOPFLAG,
                                 StopDate = a.STOPDATE,
                                 StopCheckFlag = a.STOPCHECKFLAG,
                                 StopCheckDate = a.STOPCHECKDATE,
                                 StartDate = a.STARTDATE,
                                 EndDate = a.ENDDATE,
                                 ExecTimes = a.EXECTIMES,
                                 BillDate = a.BILLDATE,
                                 DeleteFlag = a.DELETEFLAG,
                                 FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                 ChargeGroupId = a.CHARGEGROUPID,
                                 DoctorNo = a.DOCTORNO,
                                 NurseNo = a.NURSENO,
                                 DoctorName = doc_info.EMPNAME,
                                 NurseName = nur_info.EMPNAME,
                                 FeeNo = a.FEENO,
                                 RegNo = a.REGNO,
                                 OrgId = a.ORGID,
                                 SortNumber = a.SORTNUMBER,
                                 CreateBy = a.CREATEBY,
                                 CreateTime = a.CREATETIME,
                                 UpdateBy = a.UPDATEBY,
                                 UpdateTime = a.UPDATETIME,
                                 IsDelete = a.ISDELETE
                             }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                      join b in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on a.FEECODE equals b.MATERIALID
                                      join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                      from doc_info in doc.DefaultIfEmpty()
                                      join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                      from nur_info in nur.DefaultIfEmpty()
                                      join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                      from freq_info in freq.DefaultIfEmpty()
                                      join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                                      from ipdreg_info in ipdreg.DefaultIfEmpty()
                                      join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                      from regfile_info in regfile.DefaultIfEmpty()
                                      where a.ITEMTYPE == 2 && a.ISDELETE != true && b.STATUS == 0 && b.ISDELETE != true
                                      orderby a.CREATETIME descending
                                      select new IpdOrder()
                                      {
                                          RsName = regfile_info.NAME,
                                          OrderNo = a.ORDERNO,
                                          OrderType = a.ORDERTYPE,
                                          OrderName = b.MATERIALNAME,
                                          FeeCode = a.FEECODE,
                                          ItemType = a.ITEMTYPE,
                                          AcRemark = a.ACREMARK,
                                          TakeQty = a.TAKEQTY ?? 1,
                                          TakeDay = a.TAKEDAY,
                                          TakeFreq = a.TAKEFREQ,
                                          TakeFreqQty = freq_info.FREQQTY ?? 1,
                                          TakeWay = a.TAKEWAY,
                                          ConversionRatio = 1,
                                          PrescribeUnits = b.UNITS,
                                          Units = b.UNITS,
                                          UnitPrice = b.UNITPRICE,
                                          ChargeQty = a.TAKEQTY ?? 1 * freq_info.FREQQTY ?? 1,
                                          Amount = b.UNITPRICE * a.TAKEQTY ?? 1 * freq_info.FREQQTY ?? 1,
                                          ConfirmFlag = a.CONFIRMFLAG,
                                          ConfirmDate = a.CONFIRMDATE,
                                          CheckFlag = a.CHECKFLAG,
                                          CheckDate = a.CHECKDATE,
                                          StopFlag = a.STOPFLAG,
                                          StopDate = a.STOPDATE,
                                          StopCheckFlag = a.STOPCHECKFLAG,
                                          StopCheckDate = a.STOPCHECKDATE,
                                          StartDate = a.STARTDATE,
                                          EndDate = a.ENDDATE,
                                          ExecTimes = a.EXECTIMES,
                                          BillDate = a.BILLDATE,
                                          DeleteFlag = a.DELETEFLAG,
                                          FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                          ChargeGroupId = a.CHARGEGROUPID,
                                          DoctorNo = a.DOCTORNO,
                                          NurseNo = a.NURSENO,
                                          DoctorName = doc_info.EMPNAME,
                                          NurseName = nur_info.EMPNAME,
                                          FeeNo = a.FEENO,
                                          RegNo = a.REGNO,
                                          OrgId = a.ORGID,
                                          SortNumber = a.SORTNUMBER,
                                          CreateBy = a.CREATEBY,
                                          CreateTime = a.CREATETIME,
                                          UpdateBy = a.UPDATEBY,
                                          UpdateTime = a.UPDATETIME,
                                          IsDelete = a.ISDELETE
                                      }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                               join b in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on a.FEECODE equals b.SERVICEID
                                               join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                               from doc_info in doc.DefaultIfEmpty()
                                               join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                               from nur_info in nur.DefaultIfEmpty()
                                               join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                               from freq_info in freq.DefaultIfEmpty()
                                               join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                                               from ipdreg_info in ipdreg.DefaultIfEmpty()
                                               join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                               from regfile_info in regfile.DefaultIfEmpty()
                                               where a.ITEMTYPE == 3 && a.ISDELETE != true && b.STATUS == 0 && b.ISDELETE != true
                                               orderby a.CREATETIME descending
                                               select new IpdOrder()
                                               {
                                                   RsName = regfile_info.NAME,
                                                   OrderNo = a.ORDERNO,
                                                   OrderType = a.ORDERTYPE,
                                                   OrderName = b.SERVICENAME,
                                                   FeeCode = a.FEECODE,
                                                   ItemType = a.ITEMTYPE,
                                                   AcRemark = a.ACREMARK,
                                                   TakeQty = a.TAKEQTY ?? 1,
                                                   TakeDay = a.TAKEDAY,
                                                   TakeFreq = a.TAKEFREQ,
                                                   TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                   TakeWay = a.TAKEWAY,
                                                   ConversionRatio = 1,
                                                   PrescribeUnits = b.UNITS,
                                                   Units = b.UNITS,
                                                   UnitPrice = b.UNITPRICE,
                                                   ChargeQty = a.TAKEQTY ?? 1 * freq_info.FREQQTY ?? 1,
                                                   Amount = b.UNITPRICE * a.TAKEQTY ?? 1 * freq_info.FREQQTY ?? 1,
                                                   ConfirmFlag = a.CONFIRMFLAG,
                                                   ConfirmDate = a.CONFIRMDATE,
                                                   CheckFlag = a.CHECKFLAG,
                                                   CheckDate = a.CHECKDATE,
                                                   StopFlag = a.STOPFLAG,
                                                   StopDate = a.STOPDATE,
                                                   StopCheckFlag = a.STOPCHECKFLAG,
                                                   StopCheckDate = a.STOPCHECKDATE,
                                                   StartDate = a.STARTDATE,
                                                   EndDate = a.ENDDATE,
                                                   ExecTimes = a.EXECTIMES,
                                                   BillDate = a.BILLDATE,
                                                   DeleteFlag = a.DELETEFLAG,
                                                   FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                   ChargeGroupId = a.CHARGEGROUPID,
                                                   DoctorNo = a.DOCTORNO,
                                                   NurseNo = a.NURSENO,
                                                   DoctorName = doc_info.EMPNAME,
                                                   NurseName = nur_info.EMPNAME,
                                                   FeeNo = a.FEENO,
                                                   RegNo = a.REGNO,
                                                   OrgId = a.ORGID,
                                                   SortNumber = a.SORTNUMBER,
                                                   CreateBy = a.CREATEBY,
                                                   CreateTime = a.CREATETIME,
                                                   UpdateBy = a.UPDATEBY,
                                                   UpdateTime = a.UPDATETIME,
                                                   IsDelete = a.ISDELETE
                                               }).Union(from a in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet
                                                        join b in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                        join c in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.DOCTORNO equals c.EMPNO into doc
                                                        from doc_info in doc.DefaultIfEmpty()
                                                        join d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.NURSENO equals d.EMPNO into nur
                                                        from nur_info in nur.DefaultIfEmpty()
                                                        join e in unitOfWork.GetRepository<LTC_DRGFREQREF>().dbSet on a.TAKEFREQ equals e.FREQNO into freq
                                                        from freq_info in freq.DefaultIfEmpty()
                                                        join f in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on a.FEENO equals f.FEENO into ipdreg
                                                        from ipdreg_info in ipdreg.DefaultIfEmpty()
                                                        join g in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdreg_info.REGNO equals g.REGNO into regfile
                                                        from regfile_info in regfile.DefaultIfEmpty()
                                                        where a.ITEMTYPE == 4 && a.ISDELETE != true && b.STATUS == 0 && (b.ISDELETE == false || b.ISDELETE == null)
                                                        orderby a.CREATETIME descending
                                                        select new IpdOrder()
                                                        {
                                                            RsName=regfile_info.NAME,
                                                            OrderNo = a.ORDERNO,
                                                            OrderType = a.ORDERTYPE,
                                                            OrderName = b.CHARGEGROUPNAME,
                                                            FeeCode = a.FEECODE,
                                                            ItemType = 4,
                                                            AcRemark = a.ACREMARK,
                                                            TakeQty = a.TAKEQTY ?? 1,
                                                            TakeDay = a.TAKEDAY,
                                                            TakeFreq = a.TAKEFREQ,
                                                            TakeFreqQty = freq_info.FREQQTY ?? 1,
                                                            TakeWay = a.TAKEWAY,
                                                            ConversionRatio = 1,
                                                            PrescribeUnits = "次",
                                                            Units = "次",
                                                            UnitPrice = 0,
                                                            ChargeQty = a.TAKEQTY ?? 1 * freq_info.FREQQTY ?? 1,
                                                            Amount = 0,
                                                            ConfirmFlag = a.CONFIRMFLAG,
                                                            ConfirmDate = a.CONFIRMDATE,
                                                            CheckFlag = a.CHECKFLAG,
                                                            CheckDate = a.CHECKDATE,
                                                            StopFlag = a.STOPFLAG,
                                                            StopDate = a.STOPDATE,
                                                            StopCheckFlag = a.STOPCHECKFLAG,
                                                            StopCheckDate = a.STOPCHECKDATE,
                                                            StartDate = a.STARTDATE,
                                                            EndDate = a.ENDDATE,
                                                            ExecTimes = a.EXECTIMES,
                                                            BillDate = a.BILLDATE,
                                                            DeleteFlag = a.DELETEFLAG,
                                                            FirstDayQuantity = a.FIRSTDAYQUANTITY,
                                                            ChargeGroupId = a.CHARGEGROUPID,
                                                            DoctorNo = a.DOCTORNO,
                                                            NurseNo = a.NURSENO,
                                                            DoctorName = doc_info.EMPNAME,
                                                            NurseName = nur_info.EMPNAME,
                                                            FeeNo = a.FEENO,
                                                            RegNo = a.REGNO,
                                                            OrgId = a.ORGID,
                                                            SortNumber = a.SORTNUMBER,
                                                            CreateBy = a.CREATEBY,
                                                            CreateTime = a.CREATETIME,
                                                            UpdateBy = a.UPDATEBY,
                                                            UpdateTime = a.UPDATETIME,
                                                            IsDelete = a.ISDELETE
                                                        });

            if (request.Data.LoadType == 1)
            {
                orderData = orderData.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            if (request.Data.OrderType != 99)
            {
                orderData = orderData.Where(m => m.OrderType == request.Data.OrderType);
            }
            if (request.Data.ConfirmFlag != 99)
            {
                orderData = orderData.Where(m => m.ConfirmFlag == request.Data.ConfirmFlag);
            }
            if (request.Data.CheckFlag != 99)
            {
                orderData = orderData.Where(m => m.CheckFlag == request.Data.CheckFlag);
            }
            if (request.Data.StopFlag != 99)
            {
                orderData = orderData.Where(m => m.StopFlag == request.Data.StopFlag);
            }
            if (request.Data.CancelFlag != 99)
            {
                orderData = orderData.Where(m => m.DeleteFlag == request.Data.CancelFlag);
            }
            //if (request.Data.TimeFlag != 0)
            //{
            //    orderData = orderData.Where(m => m.StartDate >= request.Data.StartDate);
            //}
            orderData = orderData.Where(m => m.OrgId == SecurityHelper.CurrentPrincipal.OrgId && m.ConfirmFlag == 1 && m.CheckFlag == 1 && m.StopFlag == 0 && m.StopCheckFlag == 0 && m.DeleteFlag == 0 && m.IsDelete != true);
            //搜寻条件
            if (request.Data.LoadType == 1)
            {
                orderData = orderData.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            orderData = orderData.OrderBy(m => m.ConfirmFlag).ThenBy(m => m.CheckFlag).ThenBy(m => m.StopFlag).ThenBy(m => m.DeleteFlag).ThenBy(m => m.OrderNo);
            ListResult = orderData.ToList();

            foreach (var item in ListResult)
            {
                if (item.ItemType == 1)
                {
                    item.ChargeQty = Math.Ceiling(item.TakeQty * item.TakeFreqQty / item.ConversionRatio ?? 1);
                    if (item.ItemType != 4)
                    {
                        item.Amount = item.ChargeQty * item.UnitPrice;
                    }
                }
                else
                {
                    item.ChargeQty = item.TakeQty * item.TakeFreqQty;
                    if (item.ItemType != 4)
                    {
                        item.Amount = item.ChargeQty * item.UnitPrice;
                    }
                }
                if (item.ChargeGroupId != null && item.ChargeGroupId != "")
                {
                    var chargeItem = ((from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                       join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on it.CHARGEITEMID equals d.DRUGID
                                       where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 1 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                       select new CHARGEITEM
                                       {
                                           CHARGEGROUPID = it.CHARGEGROUPID,
                                           FEEITEMCOUNT = it.FEEITEMCOUNT,
                                           UNITPRICE = d.UNITPRICE

                                       }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                 join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on it.CHARGEITEMID equals d.MATERIALID
                                                 where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 2 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                                 select new CHARGEITEM
                                                 {
                                                     CHARGEGROUPID = it.CHARGEGROUPID,
                                                     FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                     UNITPRICE = d.UNITPRICE
                                                 }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                           join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on it.CHARGEITEMID equals d.SERVICEID
                                                           where it.CHARGEGROUPID == item.ChargeGroupId && it.CHARGEITEMTYPE == 3 && d.STATUS == 0 && (d.ISDELETE == null || d.ISDELETE == false)
                                                           select new CHARGEITEM
                                                           {
                                                               CHARGEGROUPID = it.CHARGEGROUPID,
                                                               FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                               UNITPRICE = d.UNITPRICE
                                                           })).ToList();


                    //计算套餐总价
                    var cg = chargeItem.Select(o => o.FEEITEMCOUNT * o.UNITPRICE).Sum();
                    item.UnitPrice = cg;
                    item.Amount = cg * item.ChargeQty;
                }

                string currtime = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd");

                if (request.Data.TimeFlag == 1)
                {
                    currtime = string.Format("{0:d}", request.Data.EndDate.AddDays(1));
                }

                //string starttime = string.Format("{0:d}", item.StartDate);
                string starttime = string.Format("{0:d}", request.Data.StartDate);  //下载时间范围自定义

                DateTime dt = DateTime.Now.Date;
                DateTime dt3 = DateTime.Now.Date;
                DateTime dt1 = Convert.ToDateTime(starttime);
                DateTime dt2 = Convert.ToDateTime(currtime);
                TimeSpan ts = dt2 - dt1;

                //当前日期大于等于开始执行日期
                if (Convert.ToDateTime(currtime) >= Convert.ToDateTime(starttime))
                {
                    if (ts.Days > 0)
                    {
                        for (int i = 0; i <= ts.Days - 1; i++)
                        {
                            IpdOrder io = new IpdOrder();
                            if (i > 0)
                            {
                                dt1 = dt1.AddDays(1);
                            }

                            if (item.EndDate == null)
                            {
                                var sendlist = unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet.Where(m => m.ORDERNO == item.OrderNo && m.ISDELETE != true && m.POSTDATE == dt1).ToList();
                                if (sendlist.Count == 0)
                                {
                                    io.RsName = item.RsName;
                                    io.checkNoSendFlag = true;
                                    io.OrderNo = item.OrderNo;
                                    io.OrderType = item.OrderType;
                                    io.FeeCode = item.FeeCode;
                                    io.OrderName = item.OrderName;
                                    io.ItemType = item.ItemType;
                                    io.AcRemark = item.AcRemark;
                                    io.PrescribeUnits = item.PrescribeUnits;
                                    io.TakeQty = item.TakeQty;
                                    io.TakeDay = item.TakeDay;
                                    io.TakeFreq = item.TakeFreq;
                                    io.TakeFreqQty = item.TakeFreqQty;
                                    io.TakeWay = item.TakeWay;
                                    io.Units = item.Units;
                                    io.UnitPrice = item.UnitPrice;
                                    io.ChargeQty = item.ChargeQty;
                                    io.Amount = item.Amount;
                                    io.StartDate = item.StartDate;
                                    io.EndDate = item.EndDate;
                                    io.BillDate = item.BillDate;
                                    io.DoctorName = item.DoctorName;
                                    io.NurseName = item.NurseName;
                                    io.ChargeGroupId = item.ChargeGroupId;
                                    io.FeeNo = item.FeeNo;
                                    io.RegNo = item.RegNo;
                                    io.OrgId = item.OrgId;
                                    io.PostDate = dt1;
                                    io.PostIsDelete = false;
                                    io.PostNurseNo = SecurityHelper.CurrentPrincipal.EmpNo;
                                    if (item.FirstDayQuantity == -1)
                                    {
                                        SendOrderList.Add(io);
                                    }
                                    else if (item.FirstDayQuantity == 0)
                                    {
                                        if (i > 0)
                                        {
                                            SendOrderList.Add(io);
                                        }
                                    }
                                    else if (item.FirstDayQuantity == 2)
                                    {
                                        SendOrderList.Add(io);
                                        SendOrderList.Add(io);
                                    }
                                }
                            }
                            else
                            {
                                string endtime = string.Format("{0:d}", item.EndDate);
                                dt3 = Convert.ToDateTime(endtime);
                                if (dt1 <= dt3)
                                {
                                    var sendlist = unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet.Where(m => m.ORDERNO == item.OrderNo && m.ISDELETE != true && m.POSTDATE == dt1).ToList();
                                    if (sendlist.Count == 0)
                                    {
                                        io.RsName = item.RsName;
                                        io.checkNoSendFlag = true;
                                        io.OrderNo = item.OrderNo;
                                        io.OrderType = item.OrderType;
                                        io.FeeCode = item.FeeCode;
                                        io.OrderName = item.OrderName;
                                        io.ItemType = item.ItemType;
                                        io.AcRemark = item.AcRemark;
                                        io.PrescribeUnits = item.PrescribeUnits;
                                        io.TakeQty = item.TakeQty;
                                        io.TakeDay = item.TakeDay;
                                        io.TakeFreq = item.TakeFreq;
                                        io.TakeFreqQty = item.TakeFreqQty;
                                        io.TakeWay = item.TakeWay;
                                        io.Units = item.Units;
                                        io.UnitPrice = item.UnitPrice;
                                        io.ChargeQty = item.ChargeQty;
                                        io.Amount = item.Amount;
                                        io.StartDate = item.StartDate;
                                        io.EndDate = item.EndDate;
                                        io.BillDate = item.BillDate;
                                        io.DoctorName = item.DoctorName;
                                        io.NurseName = item.NurseName;
                                        io.ChargeGroupId = item.ChargeGroupId;
                                        io.FeeNo = item.FeeNo;
                                        io.RegNo = item.RegNo;
                                        io.OrgId = item.OrgId;
                                        io.PostDate = dt1;
                                        io.PostIsDelete = false;
                                        io.PostNurseNo = SecurityHelper.CurrentPrincipal.EmpNo;
                                        if (item.FirstDayQuantity == -1)
                                        {
                                            SendOrderList.Add(io);
                                        }
                                        else if (item.FirstDayQuantity == 0)
                                        {
                                            if (i > 0)
                                            {
                                                SendOrderList.Add(io);
                                            }
                                        }
                                        else if (item.FirstDayQuantity == 2)
                                        {
                                            SendOrderList.Add(io);
                                            SendOrderList.Add(io);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            response.Data = SendOrderList;
            return response;
        }
        public BaseResponse Delete(IpdOrder baseRequest)
        {
            var response = new BaseResponse<IpdOrder>();
            List<IpdOrder> ListResult = new List<IpdOrder>();
            if (baseRequest.ChargeGroupId == null)
            {
                var io_list = ((from a in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                join b in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on a.ORDERNO equals b.ORDERNO
                                join c in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on a.ORDERNO equals c.ORDERNO into drugrec
                                from cc in drugrec.DefaultIfEmpty()
                                where b.ITEMTYPE == 1 && (cc.STATUS == 1 || cc.STATUS == 2) && cc.TAKETIME == a.POSTDATE
                                select new IpdOrder
                                {
                                    OrderNo = a.ORDERNO,
                                    OrderPostRecNo = a.ORDERPOSTRECNO
                                }).Concat(from a in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                          join b in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on a.ORDERNO equals b.ORDERNO
                                          join c in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on a.ORDERNO equals c.ORDERNO into drugrec
                                          from cc in drugrec.DefaultIfEmpty()
                                          where b.ITEMTYPE == 2 && (cc.STATUS == 1 || cc.STATUS == 2) && cc.TAKETIME == a.POSTDATE
                                          select new IpdOrder
                                          {
                                              OrderNo = a.ORDERNO,
                                              OrderPostRecNo = a.ORDERPOSTRECNO
                                          }).Concat(from a in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                                    join b in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on a.ORDERNO equals b.ORDERNO
                                                    join c in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on a.ORDERNO equals c.ORDERNO into drugrec
                                                    from cc in drugrec.DefaultIfEmpty()
                                                    where b.ITEMTYPE == 3 && (cc.STATUS == 1 || cc.STATUS == 2) && cc.TAKETIME == a.POSTDATE
                                                    select new IpdOrder
                                                    {
                                                        OrderNo = a.ORDERNO,
                                                        OrderPostRecNo = a.ORDERPOSTRECNO
                                                    }));

                ListResult = io_list.ToList();
            }
            else
            {
                var io_list = ((from a in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                join b in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on a.ORDERNO equals b.ORDERNO
                                join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                join d in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on c.CHARGEGROUPID equals d.CHARGEGROUPID
                                join e in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on d.CHARGEITEMID equals e.DRUGID
                                join f in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet on e.DRUGID equals f.DRUGID
                                where d.CHARGEITEMTYPE == 1 && (f.STATUS == 1 || f.STATUS == 2) && f.TAKETIME == a.POSTDATE
                                select new IpdOrder
                                {
                                    OrderNo = a.ORDERNO,
                                    OrderPostRecNo = a.ORDERPOSTRECNO
                                }).Concat(from a in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                          join b in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on a.ORDERNO equals b.ORDERNO
                                          join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                          join d in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on c.CHARGEGROUPID equals d.CHARGEGROUPID
                                          join e in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on d.CHARGEITEMID equals e.MATERIALID
                                          join f in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet on e.MATERIALID equals f.MATERIALID
                                          where d.CHARGEITEMTYPE == 2 && (f.STATUS == 1 || f.STATUS == 2) && f.TAKETIME == a.POSTDATE
                                          select new IpdOrder
                                          {
                                              OrderNo = a.ORDERNO,
                                              OrderPostRecNo = a.ORDERPOSTRECNO
                                          }).Concat(from a in unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet
                                                    join b in unitOfWork.GetRepository<LTC_IPDORDER>().dbSet on a.ORDERNO equals b.ORDERNO
                                                    join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                                    join d in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on c.CHARGEGROUPID equals d.CHARGEGROUPID
                                                    join e in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on d.CHARGEITEMID equals e.SERVICEID
                                                    join f in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet on e.SERVICEID equals f.SERVICEID
                                                    where d.CHARGEITEMTYPE == 3 && (f.STATUS == 1 || f.STATUS == 2) && f.TAKETIME == a.POSTDATE
                                                    select new IpdOrder
                                                    {
                                                        OrderNo = a.ORDERNO,
                                                        OrderPostRecNo = a.ORDERPOSTRECNO
                                                    }));
                ListResult = io_list.ToList();
            }

            if (ListResult.Count > 0)
            {
                response.ResultCode = -1;
                response.ResultMessage = "删除失败，此医嘱的费用记录已生成账单或已收费!";
            }
            else
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    LTC_DRUGRECORD drugrecordmodel = new LTC_DRUGRECORD();
                    LTC_MATERIALRECORD materialrecordmodel = new LTC_MATERIALRECORD();
                    LTC_SERVICERECORD servicerecordmodel = new LTC_SERVICERECORD();

                    var l_io = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.ORDERNO == baseRequest.OrderNo && m.TAKETIME == baseRequest.PostDate).ToList();
                    if (l_io != null)
                    {
                        if (l_io.Count > 0)
                        {
                            foreach (var item in l_io)
                            {
                                item.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                item.UPDATETIME = DateTime.Now;
                                item.ISDELETE = true;
                                unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(item);
                            }
                        }
                    }

                    var l_io2 = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.ORDERNO == baseRequest.OrderNo && m.TAKETIME == baseRequest.PostDate).ToList();
                    if (l_io2 != null)
                    {
                        if (l_io2.Count > 0)
                        {
                            foreach (var item in l_io2)
                            {
                                item.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                item.UPDATETIME = DateTime.Now;
                                item.ISDELETE = true;
                                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(item);
                            }
                        }
                    }

                    var l_io3 = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.ORDERNO == baseRequest.OrderNo && m.TAKETIME == baseRequest.PostDate).ToList();
                    if (l_io3 != null)
                    {
                        if (l_io3.Count > 0)
                        {
                            foreach (var item in l_io3)
                            {
                                item.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                item.UPDATETIME = DateTime.Now;
                                item.ISDELETE = true;
                                unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(item);
                            }
                        }
                    }

                    if (l_io.Count > 0 || l_io2.Count > 0 || l_io3.Count > 0)
                    {
                        var ipdorderlist = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(m => m.ISDELETE != true && m.ORDERNO == baseRequest.OrderNo).ToList();
                        if (ipdorderlist.Count > 0)
                        {
                            //更改执行次数和撤销结束日期到期自动停止
                            ipdorderlist[0].EXECTIMES = ipdorderlist[0].EXECTIMES - 1;
                            if (ipdorderlist[0].STOPFLAG == 1)
                            {
                                ipdorderlist[0].STOPFLAG = 0;
                                ipdorderlist[0].STOPDATE = null;
                                ipdorderlist[0].STOPCHECKFLAG = 0;
                                ipdorderlist[0].STOPCHECKDATE = null;
                                unitOfWork.GetRepository<LTC_IPDORDER>().Update(ipdorderlist[0]);
                            }
                        }
                    }

                    unitOfWork.Save();
                    unitOfWork.Commit();

                    return base.SoftDelete<LTC_IPDORDERPOSTREC>(baseRequest.OrderPostRecNo);
                }
                catch (Exception ex)
                {
                    response.ResultCode = -1;
                    response.ResultMessage = "删除异常，请联系管理员!";
                }
            }
            return response;
        }
        public BaseResponse<List<IpdOrder>> SaveNoSendOrders(NoSendIpdOrderList request)
        {
            BaseResponse<List<IpdOrder>> response = new BaseResponse<List<IpdOrder>>();
            try
            {
                unitOfWork.BeginTransaction();
                foreach (var item in request.NoSendIpdOrderLists)
                {
                    LTC_IPDORDERPOSTREC ipdorderpostrecmodel = new LTC_IPDORDERPOSTREC();
                    LTC_IPDORDER ipdordermodel = new LTC_IPDORDER();

                    if (item.checkNoSendFlag == true)
                    {
                        var orderlist = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(m => m.ISDELETE != true && m.ORDERNO == item.OrderNo).ToList();
                        if (orderlist.Count > 0)
                        {
                            string starttime = string.Format("{0:d}", item.StartDate);
                            DateTime dtstarttime = Convert.ToDateTime(starttime);
                            //首日执行两次
                            if (orderlist[0].FIRSTDAYQUANTITY == 2 && dtstarttime == item.PostDate)
                            {
                                var sentlist = unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet.Where(m => m.ISDELETE != true && m.ORDERNO == item.OrderNo && m.POSTDATE == item.PostDate).ToList();
                                if (sentlist.Count < 2)
                                {
                                    ipdorderpostrecmodel.ORDERNO = item.OrderNo;
                                    ipdorderpostrecmodel.POSTDATE = item.PostDate;
                                    ipdorderpostrecmodel.NURSENO = SecurityHelper.CurrentPrincipal.EmpNo;
                                    ipdorderpostrecmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                    ipdorderpostrecmodel.CREATETIME = DateTime.Now;
                                    ipdorderpostrecmodel.ISDELETE = false;
                                    unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().Insert(ipdorderpostrecmodel);

                                    var ipdorderlist = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(m => m.ISDELETE != true && m.ORDERNO == item.OrderNo).ToList();
                                    if (ipdorderlist.Count > 0)
                                    {
                                        ipdorderlist[0].EXECTIMES = ipdorderlist[0].EXECTIMES + 1;
                                        unitOfWork.GetRepository<LTC_IPDORDER>().Update(ipdorderlist[0]);
                                    }

                                    if (item.ItemType == 1)
                                    {
                                        LTC_DRUGRECORD drugrecordmodel = new LTC_DRUGRECORD();
                                        var drugList = unitOfWork.GetRepository<LTC_NSDRUG>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.DRUGID == item.FeeCode && m.ISDELETE != true).ToList();
                                        var drugRecordList = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                        if (drugList.Count > 0)
                                        {
                                            if (drugRecordList.Count < 2)
                                            {
                                                drugrecordmodel.ORDERNO = item.OrderNo;
                                                drugrecordmodel.DRUGID = item.FeeCode ?? 0;
                                                drugrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                drugrecordmodel.FEENO = item.FeeNo ?? -1;
                                                drugrecordmodel.CNNAME = drugList[0].CNNAME;
                                                drugrecordmodel.CONVERSIONRATIO = drugList[0].CONVERSIONRATIO ?? 1;
                                                drugrecordmodel.FORM = drugList[0].FORM;
                                                drugrecordmodel.PRESCRIBEUNITS = drugList[0].PRESCRIBEUNITS;
                                                drugrecordmodel.DRUGQTY = item.TakeQty;
                                                drugrecordmodel.UNITS = drugList[0].UNITS;
                                                drugrecordmodel.QTY = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1);
                                                drugrecordmodel.UNITPRICE = drugList[0].UNITPRICE;
                                                drugrecordmodel.COST = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1) * drugList[0].UNITPRICE;
                                                drugrecordmodel.DOSAGE = item.TakeQty;
                                                drugrecordmodel.TAKEWAY = item.TakeWay;
                                                drugrecordmodel.FERQ = item.TakeFreq;
                                                drugrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                drugrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                drugrecordmodel.COMMENT = "";
                                                drugrecordmodel.ISNCIITEM = drugList[0].ISNCIITEM;
                                                drugrecordmodel.ISCHARGEGROUPITEM = false;
                                                drugrecordmodel.STATUS = 0;
                                                drugrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                drugrecordmodel.CREATETIME = DateTime.Now;
                                                drugrecordmodel.ISDELETE = false;
                                                unitOfWork.GetRepository<LTC_DRUGRECORD>().Insert(drugrecordmodel);
                                            }
                                        }
                                    }
                                    else if (item.ItemType == 2)
                                    {
                                        LTC_MATERIALRECORD materialrecordmodel = new LTC_MATERIALRECORD();
                                        var medicalmaterialList = unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.MATERIALID == item.FeeCode && m.ISDELETE != true).ToList();
                                        var materialRecordList = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                        if (medicalmaterialList.Count > 0)
                                        {
                                            if (materialRecordList.Count < 2)
                                            {
                                                materialrecordmodel.ORDERNO = item.OrderNo;
                                                materialrecordmodel.MATERIALID = item.FeeCode ?? 0;
                                                materialrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                materialrecordmodel.FEENO = item.FeeNo ?? -1;
                                                materialrecordmodel.MATERIALNAME = medicalmaterialList[0].MATERIALNAME;
                                                materialrecordmodel.UNITS = medicalmaterialList[0].UNITS;
                                                materialrecordmodel.QTY = item.ChargeQty;
                                                materialrecordmodel.UNITPRICE = medicalmaterialList[0].UNITPRICE;
                                                materialrecordmodel.COST = item.ChargeQty * medicalmaterialList[0].UNITPRICE;
                                                materialrecordmodel.TAKEWAY = item.TakeWay;
                                                materialrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                materialrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                materialrecordmodel.COMMENT = "";
                                                materialrecordmodel.ISNCIITEM = medicalmaterialList[0].ISNCIITEM;
                                                materialrecordmodel.ISCHARGEGROUPITEM = false;
                                                materialrecordmodel.STATUS = 0;
                                                materialrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                materialrecordmodel.CREATETIME = DateTime.Now;
                                                materialrecordmodel.ISDELETE = false;
                                                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Insert(materialrecordmodel);
                                            }
                                        }
                                    }
                                    else if (item.ItemType == 3)
                                    {
                                        LTC_SERVICERECORD servicerecordmodel = new LTC_SERVICERECORD();
                                        var serviceList = unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.SERVICEID == item.FeeCode && m.ISDELETE != true).ToList();
                                        var serviceRecordList = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                        if (serviceList.Count > 0)
                                        {
                                            if (serviceRecordList.Count < 2)
                                            {
                                                servicerecordmodel.ORDERNO = item.OrderNo;
                                                servicerecordmodel.SERVICEID = item.FeeCode ?? 0;
                                                servicerecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                servicerecordmodel.FEENO = item.FeeNo ?? -1;
                                                servicerecordmodel.SERVICENAME = serviceList[0].SERVICENAME;
                                                servicerecordmodel.UNITS = serviceList[0].UNITS;
                                                servicerecordmodel.QTY = item.ChargeQty;
                                                servicerecordmodel.UNITPRICE = serviceList[0].UNITPRICE;
                                                servicerecordmodel.COST = item.ChargeQty * serviceList[0].UNITPRICE;
                                                servicerecordmodel.TAKEWAY = item.TakeWay;
                                                servicerecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                servicerecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                servicerecordmodel.COMMENT = "";
                                                servicerecordmodel.ISNCIITEM = serviceList[0].ISNCIITEM;
                                                servicerecordmodel.ISCHARGEGROUPITEM = false;
                                                servicerecordmodel.STATUS = 0;
                                                servicerecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                servicerecordmodel.CREATETIME = DateTime.Now;
                                                servicerecordmodel.ISDELETE = false;
                                                unitOfWork.GetRepository<LTC_SERVICERECORD>().Insert(servicerecordmodel);
                                            }
                                        }
                                    }
                                    else if (item.ItemType == 4)
                                    {
                                        var io_list = ((from a in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet
                                                        join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                        join c in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on b.CHARGEITEMID equals c.DRUGID
                                                        where b.CHARGEITEMTYPE == 1 && c.ISDELETE != true && a.CHARGEGROUPID == item.ChargeGroupId
                                                        select new CHARGEITEM
                                                        {
                                                            ITEMTYPE = 1,
                                                            FeeCode = c.DRUGID,
                                                            NAME = c.CNNAME,
                                                            CONVERSIONRATIO = c.CONVERSIONRATIO,
                                                            FORM = c.FORM,
                                                            PRESCRIBEUNITS = c.PRESCRIBEUNITS,
                                                            UNITS = c.UNITS,
                                                            UNITPRICE = c.UNITPRICE,
                                                            ISNCIITEM = c.ISNCIITEM
                                                        }).Concat(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet
                                                                  join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                                  join c in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on b.CHARGEITEMID equals c.MATERIALID
                                                                  where b.CHARGEITEMTYPE == 2 && c.ISDELETE != true && a.CHARGEGROUPID == item.ChargeGroupId
                                                                  select new CHARGEITEM
                                                                  {
                                                                      ITEMTYPE = 2,
                                                                      FeeCode = c.MATERIALID,
                                                                      NAME = c.MATERIALNAME,
                                                                      CONVERSIONRATIO = 1,
                                                                      FORM = "",
                                                                      PRESCRIBEUNITS = c.UNITS,
                                                                      UNITS = c.UNITS,
                                                                      UNITPRICE = c.UNITPRICE,
                                                                      ISNCIITEM = c.ISNCIITEM
                                                                  }).Concat(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet
                                                                            join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                                            join c in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on b.CHARGEITEMID equals c.SERVICEID
                                                                            where b.CHARGEITEMTYPE == 3 && c.ISDELETE != true && a.CHARGEGROUPID == item.ChargeGroupId
                                                                            select new CHARGEITEM
                                                                            {
                                                                                ITEMTYPE = 3,
                                                                                FeeCode = c.SERVICEID,
                                                                                NAME = c.SERVICENAME,
                                                                                CONVERSIONRATIO = 1,
                                                                                FORM = "",
                                                                                PRESCRIBEUNITS = c.UNITS,
                                                                                UNITS = c.UNITS,
                                                                                UNITPRICE = c.UNITPRICE,
                                                                                ISNCIITEM = c.ISNCIITEM
                                                                            })).ToList();

                                        foreach (var chargeitem in io_list)
                                        {
                                            if (chargeitem.ITEMTYPE == 1)
                                            {
                                                LTC_DRUGRECORD drugrecordmodel = new LTC_DRUGRECORD();
                                                var drugList = unitOfWork.GetRepository<LTC_NSDRUG>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.DRUGID == chargeitem.FeeCode && m.ISDELETE != true).ToList();
                                                var drugRecordList = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.DRUGID == chargeitem.FeeCode && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                                if (drugList.Count > 0)
                                                {
                                                    if (drugRecordList.Count < 2)
                                                    {
                                                        drugrecordmodel.ORDERNO = item.OrderNo;
                                                        drugrecordmodel.DRUGID = chargeitem.FeeCode ?? 0;
                                                        drugrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                        drugrecordmodel.FEENO = item.FeeNo ?? -1;
                                                        drugrecordmodel.CNNAME = drugList[0].CNNAME;
                                                        drugrecordmodel.CONVERSIONRATIO = drugList[0].CONVERSIONRATIO ?? 1;
                                                        drugrecordmodel.FORM = drugList[0].FORM;
                                                        drugrecordmodel.PRESCRIBEUNITS = drugList[0].PRESCRIBEUNITS;
                                                        drugrecordmodel.DRUGQTY = item.TakeQty;
                                                        drugrecordmodel.UNITS = drugList[0].UNITS;
                                                        drugrecordmodel.QTY = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1);
                                                        drugrecordmodel.UNITPRICE = drugList[0].UNITPRICE;
                                                        drugrecordmodel.COST = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1) * drugList[0].UNITPRICE;
                                                        drugrecordmodel.DOSAGE = item.TakeQty;
                                                        drugrecordmodel.TAKEWAY = item.TakeWay;
                                                        drugrecordmodel.FERQ = item.TakeFreq;
                                                        drugrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                        drugrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        drugrecordmodel.COMMENT = "";
                                                        drugrecordmodel.ISNCIITEM = drugList[0].ISNCIITEM;
                                                        drugrecordmodel.ISCHARGEGROUPITEM = false;
                                                        drugrecordmodel.STATUS = 0;
                                                        drugrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        drugrecordmodel.CREATETIME = DateTime.Now;
                                                        drugrecordmodel.ISDELETE = false;
                                                        unitOfWork.GetRepository<LTC_DRUGRECORD>().Insert(drugrecordmodel);
                                                    }
                                                }
                                            }
                                            else if (chargeitem.ITEMTYPE == 2)
                                            {
                                                LTC_MATERIALRECORD materialrecordmodel = new LTC_MATERIALRECORD();
                                                var medicalmaterialList = unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.MATERIALID == chargeitem.FeeCode && m.ISDELETE != true).ToList();
                                                var materialRecordList = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.MATERIALID == chargeitem.FeeCode && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                                if (medicalmaterialList.Count > 0)
                                                {
                                                    if (materialRecordList.Count < 2)
                                                    {
                                                        materialrecordmodel.ORDERNO = item.OrderNo;
                                                        materialrecordmodel.MATERIALID = chargeitem.FeeCode ?? 0;
                                                        materialrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                        materialrecordmodel.FEENO = item.FeeNo ?? -1;
                                                        materialrecordmodel.MATERIALNAME = medicalmaterialList[0].MATERIALNAME;
                                                        materialrecordmodel.UNITS = medicalmaterialList[0].UNITS;
                                                        materialrecordmodel.QTY = item.ChargeQty;
                                                        materialrecordmodel.UNITPRICE = medicalmaterialList[0].UNITPRICE;
                                                        materialrecordmodel.COST = item.ChargeQty * medicalmaterialList[0].UNITPRICE;
                                                        materialrecordmodel.TAKEWAY = item.TakeWay;
                                                        materialrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                        materialrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        materialrecordmodel.COMMENT = "";
                                                        materialrecordmodel.ISNCIITEM = medicalmaterialList[0].ISNCIITEM;
                                                        materialrecordmodel.ISCHARGEGROUPITEM = false;
                                                        materialrecordmodel.STATUS = 0;
                                                        materialrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        materialrecordmodel.CREATETIME = DateTime.Now;
                                                        materialrecordmodel.ISDELETE = false;
                                                        unitOfWork.GetRepository<LTC_MATERIALRECORD>().Insert(materialrecordmodel);
                                                    }
                                                }
                                            }
                                            else if (chargeitem.ITEMTYPE == 3)
                                            {
                                                LTC_SERVICERECORD servicerecordmodel = new LTC_SERVICERECORD();
                                                var serviceList = unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.SERVICEID == chargeitem.FeeCode && m.ISDELETE != true).ToList();
                                                var serviceRecordList = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.SERVICEID == chargeitem.FeeCode && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                                if (serviceList.Count > 0)
                                                {
                                                    if (serviceRecordList.Count < 2)
                                                    {
                                                        servicerecordmodel.ORDERNO = item.OrderNo;
                                                        servicerecordmodel.SERVICEID = chargeitem.FeeCode ?? 0;
                                                        servicerecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                        servicerecordmodel.FEENO = item.FeeNo ?? -1;
                                                        servicerecordmodel.SERVICENAME = serviceList[0].SERVICENAME;
                                                        servicerecordmodel.UNITS = serviceList[0].UNITS;
                                                        servicerecordmodel.QTY = item.ChargeQty;
                                                        servicerecordmodel.UNITPRICE = serviceList[0].UNITPRICE;
                                                        servicerecordmodel.COST = item.ChargeQty * serviceList[0].UNITPRICE;
                                                        servicerecordmodel.TAKEWAY = item.TakeWay;
                                                        servicerecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                        servicerecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        servicerecordmodel.COMMENT = "";
                                                        servicerecordmodel.ISNCIITEM = serviceList[0].ISNCIITEM;
                                                        servicerecordmodel.ISCHARGEGROUPITEM = false;
                                                        servicerecordmodel.STATUS = 0;
                                                        servicerecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        servicerecordmodel.CREATETIME = DateTime.Now;
                                                        servicerecordmodel.ISDELETE = false;
                                                        unitOfWork.GetRepository<LTC_SERVICERECORD>().Insert(servicerecordmodel);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var sentlist = unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet.Where(m => m.ISDELETE != true && m.ORDERNO == item.OrderNo && m.POSTDATE == item.PostDate).ToList();
                                if (sentlist.Count == 0)
                                {
                                    ipdorderpostrecmodel.ORDERNO = item.OrderNo;
                                    ipdorderpostrecmodel.POSTDATE = item.PostDate;
                                    ipdorderpostrecmodel.NURSENO = SecurityHelper.CurrentPrincipal.EmpNo;
                                    ipdorderpostrecmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                    ipdorderpostrecmodel.CREATETIME = DateTime.Now;
                                    ipdorderpostrecmodel.ISDELETE = false;
                                    unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().Insert(ipdorderpostrecmodel);

                                    var ipdorderlist = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(m => m.ISDELETE != true && m.ORDERNO == item.OrderNo).ToList();
                                    if (ipdorderlist.Count > 0)
                                    {
                                        ipdorderlist[0].EXECTIMES = ipdorderlist[0].EXECTIMES + 1;
                                        unitOfWork.GetRepository<LTC_IPDORDER>().Update(ipdorderlist[0]);
                                    }

                                    if (item.ItemType == 1)
                                    {
                                        LTC_DRUGRECORD drugrecordmodel = new LTC_DRUGRECORD();
                                        var drugList = unitOfWork.GetRepository<LTC_NSDRUG>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.DRUGID == item.FeeCode && m.ISDELETE != true).ToList();
                                        var drugRecordList = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                        if (drugList.Count > 0)
                                        {
                                            if (drugRecordList.Count == 0)
                                            {
                                                drugrecordmodel.ORDERNO = item.OrderNo;
                                                drugrecordmodel.DRUGID = item.FeeCode ?? 0;
                                                drugrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                drugrecordmodel.FEENO = item.FeeNo ?? -1;
                                                drugrecordmodel.CNNAME = drugList[0].CNNAME;
                                                drugrecordmodel.CONVERSIONRATIO = drugList[0].CONVERSIONRATIO ?? 1;
                                                drugrecordmodel.FORM = drugList[0].FORM;
                                                drugrecordmodel.PRESCRIBEUNITS = drugList[0].PRESCRIBEUNITS;
                                                drugrecordmodel.DRUGQTY = item.TakeQty;
                                                drugrecordmodel.UNITS = drugList[0].UNITS;
                                                drugrecordmodel.QTY = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1);
                                                drugrecordmodel.UNITPRICE = drugList[0].UNITPRICE;
                                                drugrecordmodel.COST = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1) * drugList[0].UNITPRICE;
                                                drugrecordmodel.DOSAGE = item.TakeQty;
                                                drugrecordmodel.TAKEWAY = item.TakeWay;
                                                drugrecordmodel.FERQ = item.TakeFreq;
                                                drugrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                drugrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                drugrecordmodel.COMMENT = "";
                                                drugrecordmodel.ISNCIITEM = drugList[0].ISNCIITEM;
                                                drugrecordmodel.ISCHARGEGROUPITEM = false;
                                                drugrecordmodel.STATUS = 0;
                                                drugrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                drugrecordmodel.CREATETIME = DateTime.Now;
                                                drugrecordmodel.ISDELETE = false;
                                                unitOfWork.GetRepository<LTC_DRUGRECORD>().Insert(drugrecordmodel);
                                            }
                                        }
                                    }
                                    else if (item.ItemType == 2)
                                    {
                                        LTC_MATERIALRECORD materialrecordmodel = new LTC_MATERIALRECORD();
                                        var medicalmaterialList = unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.MATERIALID == item.FeeCode && m.ISDELETE != true).ToList();
                                        var materialRecordList = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                        if (medicalmaterialList.Count > 0)
                                        {
                                            if (materialRecordList.Count == 0)
                                            {
                                                materialrecordmodel.ORDERNO = item.OrderNo;
                                                materialrecordmodel.MATERIALID = item.FeeCode ?? 0;
                                                materialrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                materialrecordmodel.FEENO = item.FeeNo ?? -1;
                                                materialrecordmodel.MATERIALNAME = medicalmaterialList[0].MATERIALNAME;
                                                materialrecordmodel.UNITS = medicalmaterialList[0].UNITS;
                                                materialrecordmodel.QTY = item.ChargeQty;
                                                materialrecordmodel.UNITPRICE = medicalmaterialList[0].UNITPRICE;
                                                materialrecordmodel.COST = item.ChargeQty * medicalmaterialList[0].UNITPRICE;
                                                materialrecordmodel.TAKEWAY = item.TakeWay;
                                                materialrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                materialrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                materialrecordmodel.COMMENT = "";
                                                materialrecordmodel.ISNCIITEM = medicalmaterialList[0].ISNCIITEM;
                                                materialrecordmodel.ISCHARGEGROUPITEM = false;
                                                materialrecordmodel.STATUS = 0;
                                                materialrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                materialrecordmodel.CREATETIME = DateTime.Now;
                                                materialrecordmodel.ISDELETE = false;
                                                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Insert(materialrecordmodel);
                                            }
                                        }
                                    }
                                    else if (item.ItemType == 3)
                                    {
                                        LTC_SERVICERECORD servicerecordmodel = new LTC_SERVICERECORD();
                                        var serviceList = unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.SERVICEID == item.FeeCode && m.ISDELETE != true).ToList();
                                        var serviceRecordList = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                        if (serviceList.Count > 0)
                                        {
                                            if (serviceRecordList.Count == 0)
                                            {
                                                servicerecordmodel.ORDERNO = item.OrderNo;
                                                servicerecordmodel.SERVICEID = item.FeeCode ?? 0;
                                                servicerecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                servicerecordmodel.FEENO = item.FeeNo ?? -1;
                                                servicerecordmodel.SERVICENAME = serviceList[0].SERVICENAME;
                                                servicerecordmodel.UNITS = serviceList[0].UNITS;
                                                servicerecordmodel.QTY = item.ChargeQty;
                                                servicerecordmodel.UNITPRICE = serviceList[0].UNITPRICE;
                                                servicerecordmodel.COST = item.ChargeQty * serviceList[0].UNITPRICE;
                                                servicerecordmodel.TAKEWAY = item.TakeWay;
                                                servicerecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                servicerecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                servicerecordmodel.COMMENT = "";
                                                servicerecordmodel.ISNCIITEM = serviceList[0].ISNCIITEM;
                                                servicerecordmodel.ISCHARGEGROUPITEM = false;
                                                servicerecordmodel.STATUS = 0;
                                                servicerecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                servicerecordmodel.CREATETIME = DateTime.Now;
                                                servicerecordmodel.ISDELETE = false;
                                                unitOfWork.GetRepository<LTC_SERVICERECORD>().Insert(servicerecordmodel);
                                            }
                                        }
                                    }
                                    else if (item.ItemType == 4)
                                    {
                                        var io_list = ((from a in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet
                                                        join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                        join c in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on b.CHARGEITEMID equals c.DRUGID
                                                        where b.CHARGEITEMTYPE == 1 && c.ISDELETE != true && a.CHARGEGROUPID == item.ChargeGroupId
                                                        select new CHARGEITEM
                                                        {
                                                            ITEMTYPE = 1,
                                                            FeeCode = c.DRUGID,
                                                            NAME = c.CNNAME,
                                                            CONVERSIONRATIO = c.CONVERSIONRATIO,
                                                            FORM = c.FORM,
                                                            PRESCRIBEUNITS = c.PRESCRIBEUNITS,
                                                            UNITS = c.UNITS,
                                                            UNITPRICE = c.UNITPRICE,
                                                            ISNCIITEM = c.ISNCIITEM
                                                        }).Concat(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet
                                                                  join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                                  join c in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on b.CHARGEITEMID equals c.MATERIALID
                                                                  where b.CHARGEITEMTYPE == 2 && c.ISDELETE != true && a.CHARGEGROUPID == item.ChargeGroupId
                                                                  select new CHARGEITEM
                                                                  {
                                                                      ITEMTYPE = 2,
                                                                      FeeCode = c.MATERIALID,
                                                                      NAME = c.MATERIALNAME,
                                                                      CONVERSIONRATIO = 1,
                                                                      FORM = "",
                                                                      PRESCRIBEUNITS = c.UNITS,
                                                                      UNITS = c.UNITS,
                                                                      UNITPRICE = c.UNITPRICE,
                                                                      ISNCIITEM = c.ISNCIITEM
                                                                  }).Concat(from a in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet
                                                                            join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet on a.CHARGEGROUPID equals b.CHARGEGROUPID
                                                                            join c in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on b.CHARGEITEMID equals c.SERVICEID
                                                                            where b.CHARGEITEMTYPE == 3 && c.ISDELETE != true && a.CHARGEGROUPID == item.ChargeGroupId
                                                                            select new CHARGEITEM
                                                                            {
                                                                                ITEMTYPE = 3,
                                                                                FeeCode = c.SERVICEID,
                                                                                NAME = c.SERVICENAME,
                                                                                CONVERSIONRATIO = 1,
                                                                                FORM = "",
                                                                                PRESCRIBEUNITS = c.UNITS,
                                                                                UNITS = c.UNITS,
                                                                                UNITPRICE = c.UNITPRICE,
                                                                                ISNCIITEM = c.ISNCIITEM
                                                                            })).ToList();

                                        foreach (var chargeitem in io_list)
                                        {
                                            if (chargeitem.ITEMTYPE == 1)
                                            {
                                                LTC_DRUGRECORD drugrecordmodel = new LTC_DRUGRECORD();
                                                var drugList = unitOfWork.GetRepository<LTC_NSDRUG>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.DRUGID == chargeitem.FeeCode && m.ISDELETE != true).ToList();
                                                var drugRecordList = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.DRUGID == chargeitem.FeeCode && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                                if (drugList.Count > 0)
                                                {
                                                    if (drugRecordList.Count == 0)
                                                    {
                                                        drugrecordmodel.ORDERNO = item.OrderNo;
                                                        drugrecordmodel.DRUGID = chargeitem.FeeCode ?? 0;
                                                        drugrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                        drugrecordmodel.FEENO = item.FeeNo ?? -1;
                                                        drugrecordmodel.CNNAME = drugList[0].CNNAME;
                                                        drugrecordmodel.CONVERSIONRATIO = drugList[0].CONVERSIONRATIO ?? 1;
                                                        drugrecordmodel.FORM = drugList[0].FORM;
                                                        drugrecordmodel.PRESCRIBEUNITS = drugList[0].PRESCRIBEUNITS;
                                                        drugrecordmodel.DRUGQTY = item.TakeQty;
                                                        drugrecordmodel.UNITS = drugList[0].UNITS;
                                                        drugrecordmodel.QTY = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1);
                                                        drugrecordmodel.UNITPRICE = drugList[0].UNITPRICE;
                                                        drugrecordmodel.COST = Math.Ceiling(item.TakeQty * item.TakeFreqQty / drugList[0].CONVERSIONRATIO ?? 1) * drugList[0].UNITPRICE;
                                                        drugrecordmodel.DOSAGE = item.TakeQty;
                                                        drugrecordmodel.TAKEWAY = item.TakeWay;
                                                        drugrecordmodel.FERQ = item.TakeFreq;
                                                        drugrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                        drugrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        drugrecordmodel.COMMENT = "";
                                                        drugrecordmodel.ISNCIITEM = drugList[0].ISNCIITEM;
                                                        drugrecordmodel.ISCHARGEGROUPITEM = false;
                                                        drugrecordmodel.STATUS = 0;
                                                        drugrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        drugrecordmodel.CREATETIME = DateTime.Now;
                                                        drugrecordmodel.ISDELETE = false;
                                                        unitOfWork.GetRepository<LTC_DRUGRECORD>().Insert(drugrecordmodel);
                                                    }
                                                }
                                            }
                                            else if (chargeitem.ITEMTYPE == 2)
                                            {
                                                LTC_MATERIALRECORD materialrecordmodel = new LTC_MATERIALRECORD();
                                                var medicalmaterialList = unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.MATERIALID == chargeitem.FeeCode && m.ISDELETE != true).ToList();
                                                var materialRecordList = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.MATERIALID == chargeitem.FeeCode && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                                if (medicalmaterialList.Count > 0)
                                                {
                                                    if (materialRecordList.Count == 0)
                                                    {
                                                        materialrecordmodel.ORDERNO = item.OrderNo;
                                                        materialrecordmodel.MATERIALID = chargeitem.FeeCode ?? 0;
                                                        materialrecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                        materialrecordmodel.FEENO = item.FeeNo ?? -1;
                                                        materialrecordmodel.MATERIALNAME = medicalmaterialList[0].MATERIALNAME;
                                                        materialrecordmodel.UNITS = medicalmaterialList[0].UNITS;
                                                        materialrecordmodel.QTY = item.ChargeQty;
                                                        materialrecordmodel.UNITPRICE = medicalmaterialList[0].UNITPRICE;
                                                        materialrecordmodel.COST = item.ChargeQty * medicalmaterialList[0].UNITPRICE;
                                                        materialrecordmodel.TAKEWAY = item.TakeWay;
                                                        materialrecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                        materialrecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        materialrecordmodel.COMMENT = "";
                                                        materialrecordmodel.ISNCIITEM = medicalmaterialList[0].ISNCIITEM;
                                                        materialrecordmodel.ISCHARGEGROUPITEM = false;
                                                        materialrecordmodel.STATUS = 0;
                                                        materialrecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        materialrecordmodel.CREATETIME = DateTime.Now;
                                                        materialrecordmodel.ISDELETE = false;
                                                        unitOfWork.GetRepository<LTC_MATERIALRECORD>().Insert(materialrecordmodel);
                                                    }
                                                }
                                            }
                                            else if (chargeitem.ITEMTYPE == 3)
                                            {
                                                LTC_SERVICERECORD servicerecordmodel = new LTC_SERVICERECORD();
                                                var serviceList = unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId && m.SERVICEID == chargeitem.FeeCode && m.ISDELETE != true).ToList();
                                                var serviceRecordList = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.FEENO == item.FeeNo && m.ORDERNO == item.OrderNo && m.SERVICEID == chargeitem.FeeCode && m.TAKETIME == item.PostDate && m.ISDELETE != true && (m.STATUS == 0 || m.STATUS == 8)).ToList();
                                                if (serviceList.Count > 0)
                                                {
                                                    if (serviceRecordList.Count == 0)
                                                    {
                                                        servicerecordmodel.ORDERNO = item.OrderNo;
                                                        servicerecordmodel.SERVICEID = chargeitem.FeeCode ?? 0;
                                                        servicerecordmodel.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                                                        servicerecordmodel.FEENO = item.FeeNo ?? -1;
                                                        servicerecordmodel.SERVICENAME = serviceList[0].SERVICENAME;
                                                        servicerecordmodel.UNITS = serviceList[0].UNITS;
                                                        servicerecordmodel.QTY = item.ChargeQty;
                                                        servicerecordmodel.UNITPRICE = serviceList[0].UNITPRICE;
                                                        servicerecordmodel.COST = item.ChargeQty * serviceList[0].UNITPRICE;
                                                        servicerecordmodel.TAKEWAY = item.TakeWay;
                                                        servicerecordmodel.TAKETIME = item.PostDate ?? DateTime.Now;
                                                        servicerecordmodel.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        servicerecordmodel.COMMENT = "";
                                                        servicerecordmodel.ISNCIITEM = serviceList[0].ISNCIITEM;
                                                        servicerecordmodel.ISCHARGEGROUPITEM = false;
                                                        servicerecordmodel.STATUS = 0;
                                                        servicerecordmodel.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                                                        servicerecordmodel.CREATETIME = DateTime.Now;
                                                        servicerecordmodel.ISDELETE = false;
                                                        unitOfWork.GetRepository<LTC_SERVICERECORD>().Insert(servicerecordmodel);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                unitOfWork.Save();
                unitOfWork.Commit();

                //判断医嘱是否：结束日期到期自动停止
                unitOfWork.BeginTransaction();
                foreach (var item in request.NoSendIpdOrderLists)
                {
                    if (item.checkNoSendFlag == true)
                    {
                        var orderlist = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(m => m.ISDELETE != true && m.ORDERNO == item.OrderNo).ToList();
                        if (orderlist.Count > 0)
                        {

                            if (orderlist[0].STARTDATE != null && orderlist[0].ENDDATE != null)
                            {
                                var intIndex = 0;
                                string orderstartdate = string.Format("{0:d}", orderlist[0].STARTDATE);
                                string orderenddate = string.Format("{0:d}", orderlist[0].ENDDATE);
                                DateTime dtorderstart = Convert.ToDateTime(orderstartdate);
                                DateTime dtorderend = Convert.ToDateTime(orderenddate).AddDays(1);
                                TimeSpan orderdays = dtorderend - dtorderstart;

                                if (orderdays.Days > 0)
                                {
                                    for (int j = 0; j <= orderdays.Days - 1; j++)
                                    {
                                        var ipdorderpostrecList = unitOfWork.GetRepository<LTC_IPDORDERPOSTREC>().dbSet.Where(m => m.ORDERNO == item.OrderNo && m.POSTDATE == dtorderstart && m.ISDELETE != true).ToList();
                                        if (orderlist[0].FIRSTDAYQUANTITY == 0)
                                        {
                                            if (j > 0)
                                            {
                                                if (ipdorderpostrecList.Count == 0)
                                                {
                                                    intIndex++;
                                                }
                                            }
                                        }
                                        else if (orderlist[0].FIRSTDAYQUANTITY == -1)
                                        {
                                            if (ipdorderpostrecList.Count == 0)
                                            {
                                                intIndex++;
                                            }
                                        }
                                        else if (orderlist[0].FIRSTDAYQUANTITY == 2)
                                        {
                                            if (ipdorderpostrecList.Count < 2)
                                            {
                                                intIndex++;
                                            }
                                        }
                                        dtorderstart = dtorderstart.AddDays(1);
                                    }

                                    if (intIndex == 0)
                                    {
                                        var ipdorderlist = unitOfWork.GetRepository<LTC_IPDORDER>().dbSet.Where(m => m.ORDERNO == item.OrderNo && m.ISDELETE != true).ToList();
                                        if (ipdorderlist.Count > 0)
                                        {
                                            if (ipdorderlist[0].STOPFLAG != 1)
                                            {
                                                ipdorderlist[0].STOPFLAG = 1;
                                                ipdorderlist[0].STOPDATE = DateTime.Now;
                                                ipdorderlist[0].STOPCHECKFLAG = 1;
                                                ipdorderlist[0].STOPCHECKDATE = DateTime.Now;
                                                unitOfWork.GetRepository<LTC_IPDORDER>().Update(ipdorderlist[0]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                unitOfWork.Save();
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = "发送异常，请联系管理员!";
            }
            return response;
        }
        public BaseResponse<IList<CHARGEITEM>> QueryAllChargeItem(BaseRequest<IpdOrderFilter> request)
        {
            List<CHARGEITEM> ListResult = new List<CHARGEITEM>();
            BaseResponse<IList<CHARGEITEM>> response = new BaseResponse<IList<CHARGEITEM>>();
            var chargeItemData = (from a in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet
                                  where a.ISDELETE != true && a.STATUS == 0 && a.NSID == SecurityHelper.CurrentPrincipal.OrgId
                                  select new CHARGEITEM()
                                  {
                                      NSID = a.NSID,
                                      ITEMTYPE = 1,
                                      FeeCode = a.DRUGID,
                                      NAME = a.CNNAME,
                                      ENNAME = a.ENNAME,
                                      PINYIN = a.PINYIN,
                                      MCDRUGCODE = a.MCDRUGCODE,
                                      NSDRUGCODE = a.NSDRUGCODE,
                                      ISNCIITEM = a.ISNCIITEM,
                                      SPEC = a.SPEC,
                                      UNITS = a.UNITS,
                                      PRESCRIBEUNITS = a.PRESCRIBEUNITS,
                                      UNITPRICE = a.UNITPRICE,
                                      FORM = a.FORM,
                                      CONVERSIONRATIO = a.CONVERSIONRATIO,
                                      FERQ = a.FREQUENCY,
                                      DRUGUSAGEMODE = a.DRUGUSAGEMODE,
                                      CREATETIME = a.CREATETIME
                                  }).Union(from a in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet
                                           where a.ISDELETE != true && a.STATUS == 0 && a.NSID == SecurityHelper.CurrentPrincipal.OrgId
                                           select new CHARGEITEM()
                                           {
                                               NSID = a.NSID,
                                               ITEMTYPE = 2,
                                               FeeCode = a.MATERIALID,
                                               NAME = a.MATERIALNAME,
                                               ENNAME = "",
                                               PINYIN = a.PINYIN,
                                               MCDRUGCODE = a.MCMATERIALCODE,
                                               NSDRUGCODE = a.NSMATERIALCODE,
                                               ISNCIITEM = a.ISNCIITEM,
                                               SPEC = a.SPEC,
                                               UNITS = a.UNITS,
                                               PRESCRIBEUNITS = a.UNITS,
                                               UNITPRICE = a.UNITPRICE,
                                               FORM = "",
                                               CONVERSIONRATIO = 1,
                                               FERQ = "",
                                               DRUGUSAGEMODE = "",
                                               CREATETIME = a.CREATETIME
                                           }).Union(from a in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet
                                                    where a.ISDELETE != true && a.STATUS == 0 && a.NSID == SecurityHelper.CurrentPrincipal.OrgId
                                                    select new CHARGEITEM()
                                                    {
                                                        NSID = a.NSID,
                                                        ITEMTYPE = 3,
                                                        FeeCode = a.SERVICEID,
                                                        NAME = a.SERVICENAME,
                                                        ENNAME = "",
                                                        PINYIN = a.PINYIN,
                                                        MCDRUGCODE = a.MCSERVICECODE,
                                                        NSDRUGCODE = a.NSSERVICECODE,
                                                        ISNCIITEM = a.ISNCIITEM,
                                                        SPEC = "",
                                                        UNITS = a.UNITS,
                                                        PRESCRIBEUNITS = a.UNITS,
                                                        UNITPRICE = a.UNITPRICE,
                                                        FORM = "",
                                                        CONVERSIONRATIO = 1,
                                                        FERQ = "",
                                                        DRUGUSAGEMODE = "",
                                                        CREATETIME = a.CREATETIME
                                                    });

            if (!string.IsNullOrWhiteSpace(request.Data.KeyWord))
            {
                chargeItemData = chargeItemData.Where(m => m.NAME.ToUpper().Contains(request.Data.KeyWord.ToUpper()) ||
                   m.ENNAME.ToUpper().Contains(request.Data.KeyWord.ToUpper()) ||
                   m.PINYIN.ToUpper().Contains(request.Data.KeyWord.ToUpper()) ||
                   m.MCDRUGCODE.ToUpper().Contains(request.Data.KeyWord.ToUpper()) ||
                   m.NSDRUGCODE.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                   );
            }
            chargeItemData = chargeItemData.OrderBy(m => m.ITEMTYPE).OrderByDescending(m => m.CREATETIME);
            ListResult = chargeItemData.ToList();
            response.RecordsCount = ListResult.Count;
            List<CHARGEITEM> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = chargeItemData.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = chargeItemData.ToList();
            }
            response.Data = list;
            return response;
        }
    }
}
