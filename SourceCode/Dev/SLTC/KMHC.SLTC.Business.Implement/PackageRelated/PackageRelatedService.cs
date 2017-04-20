
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
    public class PackageRelatedService : BaseService, IPackageRelatedService
    {

        public BaseResponse<CHARGEGROUP> SavePacMaintain(CHARGEGROUP request)
        {
            var response = new BaseResponse<CHARGEGROUP>();
            var chargeItemList = request.CHARGEITEM;
            request.CHARGEITEM = null;
            if (string.IsNullOrEmpty(request.CHARGEGROUPID))
            {
                request.CHARGEGROUPID = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.ChargeGroupNo);
                request.CREATEBY = SecurityHelper.CurrentPrincipal.EmpName;
                request.CREATETIME = DateTime.Now;
                request.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpName;
                request.UPDATETIME = DateTime.Now;
                request.ISDELETE = false;
            }
            else
            {
                request.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpName;
                request.UPDATETIME = DateTime.Now;
            }
            unitOfWork.BeginTransaction();
            response = base.Save<LTC_CHARGEGROUP, CHARGEGROUP>(request, (q) => q.CHARGEGROUPID == request.CHARGEGROUPID);
            chargeItemList.ForEach((p) =>
            {
                p.CHARGEGROUPID = response.Data.CHARGEGROUPID;
                if (p.CGCIID == 0 || p.CGCIID == null)
                {
                    p.CREATEBY = SecurityHelper.CurrentPrincipal.EmpName;
                    p.CREATETIME = DateTime.Now;
                    p.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpName;
                    p.UPDATETIME = DateTime.Now;
                    p.ISDELETE = false;
                }
                else
                {
                    p.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpName;
                    p.UPDATETIME = DateTime.Now;
                }
                base.Save<LTC_CHARGEGROUP_CHARGEITEM, CHARGEITEM>(p, (q) => p.CGCIID == q.CGCIID);
            });
            unitOfWork.Commit();
            return response;
        }
        public BaseResponse<IList<CHARGEGROUP>> QueryChargeGroupList(BaseRequest<PackageRelatedFilter> request)
        {
            var response = base.Query<LTC_CHARGEGROUP, CHARGEGROUP>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.Name))
                {
                    q = q.Where(m => m.CHARGEGROUPNAME == request.Data.Name);
                }
                q = q.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId);
                q = q.OrderBy(m => m.CHARGEGROUPID);
                return q;
            });

            foreach (var item in response.Data)
            {
                if (item.CHARGEGROUPPERIOD == "001")
                {
                    item.CHARGEGROUPPERIODNAME = "日";
                }
                else if (item.CHARGEGROUPPERIOD == "002")
                {
                    item.CHARGEGROUPPERIODNAME = "月";
                }
                else if (item.CHARGEGROUPPERIOD == "003")
                {
                    item.CHARGEGROUPPERIODNAME = "季度";
                }
                else if (item.CHARGEGROUPPERIOD == "004")
                {
                    item.CHARGEGROUPPERIODNAME = "年";
                }
                else if (item.CHARGEGROUPPERIOD == "999")
                {
                    item.CHARGEGROUPPERIODNAME = "未填";
                }

                item.CHARGEITEM = ((from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                    where it.CHARGEGROUPID == item.CHARGEGROUPID && it.CHARGEITEMTYPE == 1
                                    join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on it.CHARGEITEMID equals d.DRUGID
                                    where d.STATUS == 0 && d.ISDELETE == false
                                    select new CHARGEITEM
                                    {
                                        CGCIID = it.CGCIID,
                                        CHARGEGROUPID = it.CHARGEGROUPID,
                                        CHARGEITEMID = it.CHARGEITEMID,
                                        CHARGEITEMTYPE = it.CHARGEITEMTYPE,
                                        FEEITEMCOUNT = it.FEEITEMCOUNT,
                                        NAME = d.CNNAME,
                                        FORM = d.FORM,
                                        FERQ = d.FREQUENCY,
                                        TAKEWAY = d.DRUGUSAGEMODE,
                                        PRESCRIBEUNITS=d.PRESCRIBEUNITS,
                                        CONVERSIONRATIO=d.CONVERSIONRATIO,
                                        UNITPRICE = d.UNITPRICE,
                                        UNITS = d.UNITS,
                                        MCCODE = d.MCDRUGCODE,
                                        NSCODE = d.NSDRUGCODE,
                                        ISNCIITEM = d.ISNCIITEM,
                                        SPEC = d.SPEC

                                    }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                              where it.CHARGEGROUPID == item.CHARGEGROUPID && it.CHARGEITEMTYPE == 2
                                              join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on it.CHARGEITEMID equals d.MATERIALID
                                              where d.STATUS == 0 && d.ISDELETE == false
                                              select new CHARGEITEM
                                              {
                                                  CGCIID = it.CGCIID,
                                                  CHARGEGROUPID = it.CHARGEGROUPID,
                                                  CHARGEITEMID = it.CHARGEITEMID,
                                                  CHARGEITEMTYPE = it.CHARGEITEMTYPE,
                                                  FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                  NAME = d.MATERIALNAME,
                                                  FORM = "",
                                                  FERQ = "",
                                                  TAKEWAY = "",
                                                  PRESCRIBEUNITS = d.UNITS,
                                                  CONVERSIONRATIO = 1,
                                                  UNITPRICE = d.UNITPRICE,
                                                  UNITS = d.UNITS,
                                                  MCCODE = d.MCMATERIALCODE,
                                                  NSCODE = d.NSMATERIALCODE,
                                                  ISNCIITEM = d.ISNCIITEM,
                                                  SPEC = d.SPEC

                                              }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                        where it.CHARGEGROUPID == item.CHARGEGROUPID && it.CHARGEITEMTYPE == 3
                                                        join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on it.CHARGEITEMID equals d.SERVICEID
                                                        where d.STATUS == 0 && d.ISDELETE == false
                                                        select new CHARGEITEM
                                                        {
                                                            CGCIID = it.CGCIID,
                                                            CHARGEGROUPID = it.CHARGEGROUPID,
                                                            CHARGEITEMID = it.CHARGEITEMID,
                                                            CHARGEITEMTYPE = it.CHARGEITEMTYPE,
                                                            FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                            NAME = d.SERVICENAME,
                                                            FORM = "",
                                                            FERQ = "",
                                                            TAKEWAY = "",
                                                            PRESCRIBEUNITS = d.UNITS,
                                                            CONVERSIONRATIO = 1,
                                                            UNITPRICE = d.UNITPRICE,
                                                            UNITS = d.UNITS,
                                                            MCCODE = d.MCSERVICECODE,
                                                            NSCODE = d.NSSERVICECODE,
                                                            ISNCIITEM = d.ISNCIITEM,
                                                            SPEC = ""
                                                        })).ToList();
            }
            return response;
        }
        public BaseResponse<CHARGEGROUP> GetChargeGroup(string id)
        {
            var response = base.Get<LTC_CHARGEGROUP, CHARGEGROUP>((q) => q.CHARGEGROUPID == id);
            if (response != null && response.Data != null)
            {
                response.Data.CHARGEITEM = ((from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                             where it.CHARGEGROUPID == response.Data.CHARGEGROUPID && it.CHARGEITEMTYPE == 1
                                             join d in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet on it.CHARGEITEMID equals d.DRUGID
                                             select new CHARGEITEM
                                             {
                                                 CGCIID = it.CGCIID,
                                                 CHARGEGROUPID = it.CHARGEGROUPID,
                                                 CHARGEITEMID = it.CHARGEITEMID,
                                                 CHARGEITEMTYPE = it.CHARGEITEMTYPE,
                                                 FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                 PRESCRIBEUNITS = d.PRESCRIBEUNITS,
                                                 CONVERSIONRATIO = d.CONVERSIONRATIO,
                                                 NAME = d.CNNAME,
                                                 UNITPRICE = d.UNITPRICE,
                                                 MCCODE = d.MCDRUGCODE,
                                                 NSCODE = d.NSDRUGCODE,
                                                 SPEC = d.SPEC,
                                                 UNITS = d.UNITS
                                             }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                       where it.CHARGEGROUPID == response.Data.CHARGEGROUPID && it.CHARGEITEMTYPE == 2
                                                       join d in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet on it.CHARGEITEMID equals d.MATERIALID
                                                       select new CHARGEITEM
                                                       {
                                                           CGCIID = it.CGCIID,
                                                           CHARGEGROUPID = it.CHARGEGROUPID,
                                                           CHARGEITEMID = it.CHARGEITEMID,
                                                           CHARGEITEMTYPE = it.CHARGEITEMTYPE,
                                                           FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                           PRESCRIBEUNITS = d.UNITS,
                                                           CONVERSIONRATIO = 1,
                                                           NAME = d.MATERIALNAME,
                                                           UNITPRICE = d.UNITPRICE,
                                                           MCCODE = d.MCMATERIALCODE,
                                                           NSCODE = d.NSMATERIALCODE,
                                                           SPEC = d.SPEC,
                                                           UNITS = d.UNITS
                                                       }).Concat(from it in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet
                                                                 where it.CHARGEGROUPID == response.Data.CHARGEGROUPID && it.CHARGEITEMTYPE == 3
                                                                 join d in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet on it.CHARGEITEMID equals d.SERVICEID
                                                                 select new CHARGEITEM
                                                                 {
                                                                     CGCIID = it.CGCIID,
                                                                     CHARGEGROUPID = it.CHARGEGROUPID,
                                                                     CHARGEITEMID = it.CHARGEITEMID,
                                                                     CHARGEITEMTYPE = it.CHARGEITEMTYPE,
                                                                     FEEITEMCOUNT = it.FEEITEMCOUNT,
                                                                     PRESCRIBEUNITS = d.UNITS,
                                                                     CONVERSIONRATIO = 1,
                                                                     NAME = d.SERVICENAME,
                                                                     UNITPRICE = d.UNITPRICE,
                                                                     MCCODE = d.MCSERVICECODE,
                                                                     NSCODE = d.NSSERVICECODE,
                                                                     SPEC = "",
                                                                     UNITS = d.UNITS
                                                                 })).ToList();
            }
            return response;
        }
        public BaseResponse DeleteChargeGroup(string id)
        {
            var response = new BaseResponse();
            List<LTC_CHARGEGROUP_CHARGEITEM> regQueData = unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet.Where(m => m.CHARGEGROUPID == id).ToList();
            regQueData.ForEach(p => unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().Delete(p));

            LTC_CHARGEGROUP reqQue = unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet.Where(m => m.CHARGEGROUPID == id).FirstOrDefault();
            unitOfWork.GetRepository<LTC_CHARGEGROUP>().Delete(reqQue);
            unitOfWork.Save();
            return response;
        }
        public BaseResponse DeleteChargeItem(int id)
        {
            return base.Delete<LTC_CHARGEGROUP_CHARGEITEM>(id);
        }
        public BaseResponse<IList<RESCHARGEGRO>> QueryResChargeGro(BaseRequest<PackageRelatedFilter> request)
        {
            var response = new BaseResponse<IList<RESCHARGEGRO>>();
            var q = from r in unitOfWork.GetRepository<LTC_CHARGEGROUP_RESIDENT>().dbSet
                    join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on r.CHARGEGROUPID equals c.CHARGEGROUPID
                    select new
                    {
                        cgr = r,
                        CHARGEGROUPNAME = c.CHARGEGROUPNAME,
                        CHARGEGROUPPERIOD = c.CHARGEGROUPPERIOD,

                    };
            q = q.Where(m => m.cgr.FEENO == request.Data.FeeNO);
            q = q.OrderByDescending(m => m.cgr.CGRID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<RESCHARGEGRO>();
                foreach (dynamic item in list)
                {
                    RESCHARGEGRO newItem = Mapper.DynamicMap<RESCHARGEGRO>(item.cgr);
                    newItem.CHARGEGROUPNAME = item.CHARGEGROUPNAME;
                    newItem.CHARGEGROUPPERIOD = item.CHARGEGROUPPERIOD;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<RESCHARGEGRO> SaveResChargeGro(RESCHARGEGRO request)
        {
            var response = new BaseResponse<RESCHARGEGRO>();
            // unitOfWork.BeginTransaction();
            if (request.CGRID == 0)
            {
                request.CREATETIME = DateTime.Now;
                request.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                request.UPDATETIME = DateTime.Now;
                request.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
            }
            else
            {
                request.UPDATETIME = DateTime.Now;
                request.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
            }
            response = base.Save<LTC_CHARGEGROUP_RESIDENT, RESCHARGEGRO>(request, (q) => q.CGRID == request.CGRID);

            //套餐不跑Job 所以这边三目记录表不需要插入数据 mod By Duke

            //if (request.OVERALLBEGINDATE.HasValue && request.OVERALLBEGINDATE.Value.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
            //{
            //    CHARGERECORD chargerecord = new CHARGERECORD
            //    {
            //        CHARGEGROUPID = request.CHARGEGROUPID,
            //        RESIDENTID = request.FEENO,
            //    };
            //    base.Save<LTC_CHARGEGROUP_CHARGERECORD, CHARGERECORD>(chargerecord, (q) => q.CGCRID == chargerecord.CGCRID);
            //    List<LTC_CHARGEGROUP_CHARGEITEM> regQueData = unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGEITEM>().dbSet.Where(m => m.CHARGEGROUPID == request.CHARGEGROUPID).ToList();
            //    var drugId = regQueData.Where(w => w.CHARGEITEMTYPE == 1).Select(s => s.CHARGEITEMID);
            //    var drugList = (from o in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet where drugId.Contains(o.DRUGID) select o).ToList();
            //    drugList.ForEach(p =>
            //    {
            //        Mapper.CreateMap<LTC_NSDRUG, LTC_DRUGRECORD>();
            //        var drug = Mapper.Map<LTC_DRUGRECORD>(p);
            //        drug.FERQ = "";
            //        drug.QTY = 0;
            //        drug.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
            //        drug.FEENO = request.FEENO;
            //        unitOfWork.GetRepository<LTC_DRUGRECORD>().Insert(drug);
            //    });
            //    var materialId = regQueData.Where(w => w.CHARGEITEMTYPE == 2).Select(s => s.CHARGEITEMID);
            //    var materialList = (from o in unitOfWork.GetRepository<LTC_NSMEDICALMATERIAL>().dbSet where drugId.Contains(o.MATERIALID) select o).ToList();
            //    materialList.ForEach(p =>
            //    {
            //        Mapper.CreateMap<LTC_NSMEDICALMATERIAL, LTC_MATERIALRECORD>();
            //        var mat = Mapper.Map<LTC_MATERIALRECORD>(p);
            //        mat.TAKEWAY = "";
            //        mat.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
            //        mat.QTY = 0;
            //        mat.FEENO = request.FEENO;
            //        unitOfWork.GetRepository<LTC_MATERIALRECORD>().Insert(mat);
            //    });
            //    var serviceId = regQueData.Where(w => w.CHARGEITEMTYPE == 3).Select(s => s.CHARGEITEMID);
            //    var serviceList = (from o in unitOfWork.GetRepository<LTC_NSSERVICE>().dbSet where drugId.Contains(o.SERVICEID) select o).ToList();
            //    serviceList.ForEach(p =>
            //    {
            //        Mapper.CreateMap<LTC_NSSERVICE, LTC_SERVICERECORD>();
            //        var ser = Mapper.Map<LTC_SERVICERECORD>(p);
            //        ser.QTY = 0;
            //        ser.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
            //        ser.FEENO = request.FEENO;
            //        unitOfWork.GetRepository<LTC_SERVICERECORD>().Insert(ser);
            //    });
            //}
            //unitOfWork.Commit();
            return response;
        }

        public BaseResponse DeleteResChargeGro(long id)
        {
            return base.Delete<LTC_CHARGEGROUP_RESIDENT>(id);
        }

        #region 套餐费用录入
        public BaseResponse<ChargeGroupRec> SaveChargeGroupRec(ChargeItemData request)
        {
            var chargeGroupRecModel = new ChargeGroupRec()
            {
                ChargeGroupId = request.ChargeGroupRec.ChargeGroupId,
                FeeNo = request.ChargeGroupRec.FeeNo,
                CreateTime = DateTime.Now,
                CreateBy = SecurityHelper.CurrentPrincipal.EmpNo,
                UpdateTime = DateTime.Now,
                UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo,
                IsDelete = false,
            };
            var response = base.Save<LTC_CHARGEGROUP_CHARGERECORD, ChargeGroupRec>(chargeGroupRecModel, (q) => q.CGCRID == chargeGroupRecModel.CgcrId);

            unitOfWork.BeginTransaction();
            request.ChargeItem.ForEach(p =>
            {
                if (p.CHARGEITEMTYPE == 1)
                {
                    var drugRecordsModel = new DrugRecords()
                    {
                        CGCRID = response.Data.CgcrId,
                        DRUGID = p.CHARGEITEMID,
                        NSID = SecurityHelper.CurrentPrincipal.OrgId,
                        FEENO = p.FEENO,
                        CNNAME = p.NAME,
                        CONVERSIONRATIO = p.CONVERSIONRATIO ?? 1,
                        FORM = p.FORM ?? "",
                        PRESCRIBEUNITS = p.PRESCRIBEUNITS,
                        DRUGQTY = p.FEEITEMCOUNT,
                        UNITS = p.UNITS,
                        QTY = p.QTY,
                        UNITPRICE = p.UNITPRICE,
                        COST = p.COST ?? 0,
                        DOSAGE = p.DOSAGE,
                        TAKEWAY = p.TAKEWAY ?? "",
                        FERQ = p.FERQ ?? "",
                        TAKETIME = p.TAKETIME,
                        OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo,
                        ISCHARGEGROUPITEM = true,
                        ISNCIITEM = p.ISNCIITEM,
                        STATUS = 0,
                        CREATETIME = DateTime.Now,
                        CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                        UPDATETIME = DateTime.Now,
                        UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                        ISDELETE = false,
                    };
                    base.Save<LTC_DRUGRECORD, DrugRecords>(drugRecordsModel, (q) => q.DRUGRECORDID == drugRecordsModel.DRUGRECORDID);
                }
                else if (p.CHARGEITEMTYPE == 2)
                {
                    var materialRecordsModel = new MaterialRecords()
                    {
                        CGCRID = response.Data.CgcrId,
                        MATERIALID = p.CHARGEITEMID,
                        NSID = SecurityHelper.CurrentPrincipal.OrgId,
                        FEENO = p.FEENO,
                        MATERIALNAME = p.NAME,
                        UNITS = p.UNITS,
                        QTY = p.QTY,
                        UNITPRICE = p.UNITPRICE,
                        COST = p.COST ?? 0,
                        TAKEWAY = p.TAKEWAY ?? "",
                        TAKETIME = p.TAKETIME,
                        OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo,
                        ISCHARGEGROUPITEM = true,
                        ISNCIITEM = p.ISNCIITEM,
                        STATUS = 0,
                        CREATETIME = DateTime.Now,
                        CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                        UPDATETIME = DateTime.Now,
                        UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                        ISDELETE = false,
                    };
                    base.Save<LTC_MATERIALRECORD, MaterialRecords>(materialRecordsModel, (q) => q.MATERIALRECORDID == materialRecordsModel.MATERIALRECORDID);
                }
                else if (p.CHARGEITEMTYPE == 3)
                {
                    var serviceRecordsModel = new ServiceRecords()
                    {
                        CGCRID = response.Data.CgcrId,
                        SERVICEID = p.CHARGEITEMID,
                        NSID = SecurityHelper.CurrentPrincipal.OrgId,
                        FEENO = p.FEENO,
                        SERVICENAME = p.NAME,
                        UNITS = p.UNITS,
                        QTY = p.QTY,
                        UNITPRICE = p.UNITPRICE,
                        COST = p.COST ?? 0,
                        TAKEWAY = p.TAKEWAY ?? "",
                        TAKETIME = p.TAKETIME,
                        OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo,
                        ISCHARGEGROUPITEM = true,
                        ISNCIITEM = p.ISNCIITEM,
                        STATUS = 0,
                        CREATETIME = DateTime.Now,
                        CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                        UPDATETIME = DateTime.Now,
                        UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                        ISDELETE = false,
                    };
                    base.Save<LTC_SERVICERECORD, ServiceRecords>(serviceRecordsModel, (q) => q.SERVICERECORDID == serviceRecordsModel.SERVICERECORDID);
                }
            });
            unitOfWork.Commit();
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 查询单条药品记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DrugRecord> GetDrugRec(BaseRequest<DrugRecordFilter> request)
        {
            return base.Get<LTC_DRUGRECORD, DrugRecord>(q => q.DRUGRECORDID == request.Data.DrugrecordId);
        }

        /// <summary>
        /// 查询单条耗材记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<MaterialRecord> GetMaterialRec(BaseRequest<MaterialRecordFilter> request)
        {
            return base.Get<LTC_MATERIALRECORD, MaterialRecord>(q => q.MATERIALRECORDID == request.Data.MaterialRecordId);
        }

        /// <summary>
        /// 查询单条服务记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<ServiceRecord> GetServiceRec(BaseRequest<ServiceRecordFilter> request)
        {
            return base.Get<LTC_SERVICERECORD, ServiceRecord>(q => q.SERVICERECORDID == request.Data.ServiceRecordId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<MainChargeItem>> QueryChargeGroupRec(BaseRequest<ChargeGroupRecFilter> request)
        {
            BaseResponse<IList<MainChargeItem>> response = new BaseResponse<IList<MainChargeItem>>();
            var q = from a in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGERECORD>().dbSet
                    join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on a.CHARGEGROUPID equals c.CHARGEGROUPID into cg
                    from ccg in cg.DefaultIfEmpty()
                    select new MainChargeItem()
                    {
                        FEENO=a.FEENO,
                        NSID=ccg.NSID,
                        CGCRID=a.CGCRID,
                        CHARGEGROUPID = a.CHARGEGROUPID,
                        CHARGEGROUPNAME = ccg.CHARGEGROUPNAME,
                        CREATETIME = a.CREATETIME,
                        ISDELETE=a.ISDELETE,
                    };
            q = q.Where(m => m.FEENO == request.Data.FeeNo && m.ISDELETE==false && m.NSID==SecurityHelper.CurrentPrincipal.OrgId);
            q = q.OrderBy(m => m.CREATETIME);
            List<MainChargeItem> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }
            response.Data = list;
            foreach (var item in response.Data)
            {
                request.Data.ChargeGroupId = item.CHARGEGROUPID;
                request.Data.CgcrId = item.CGCRID;
                item.ChargeItemList = QueryChargeGroupList(request).Data;
            }
            return response;
        }
        /// <summary>
        /// 套餐详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<CHARGEITEM>> QueryChargeGroupList(BaseRequest<ChargeGroupRecFilter> request)
        {
            BaseResponse<IList<CHARGEITEM>> response = new BaseResponse<IList<CHARGEITEM>>();
            var billData = (from a in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet
                            join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGERECORD>().dbSet on a.CGCRID equals b.CGCRID
                            join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                            where a.ISDELETE == false && b.FEENO == request.Data.FeeNo && c.NSID == SecurityHelper.CurrentPrincipal.OrgId && b.CHARGEGROUPID == request.Data.ChargeGroupId && b.CGCRID == request.Data.CgcrId
                            orderby b.CGCRID descending

                            select new CHARGEITEM()
                            {
                                CHARGEGROUPNAME = c.CHARGEGROUPNAME,
                                CGCRID = b.CGCRID,
                                FEENO = b.FEENO,
                                CHARGERECORDTYPE = 1,
                                CHARGERECORDID = a.DRUGRECORDID,
                                NAME = a.CNNAME,
                                CHARGEID = a.DRUGID,
                                UNITPRICE = a.UNITPRICE,
                                QTY = a.QTY,
                                UNITS = a.UNITS,
                                COST = a.COST,
                                TAKETIME = a.TAKETIME,
                                STATUS = a.STATUS,
                                CREATETIME = b.CREATETIME,
                            }).Union(from a in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet
                                     join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGERECORD>().dbSet on a.CGCRID equals b.CGCRID
                                     join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                     where a.ISDELETE == false && b.FEENO == request.Data.FeeNo && c.NSID == SecurityHelper.CurrentPrincipal.OrgId && b.CHARGEGROUPID == request.Data.ChargeGroupId && b.CGCRID == request.Data.CgcrId
                                     orderby b.CGCRID descending
                                     select new CHARGEITEM()
                                     {
                                         CHARGEGROUPNAME = c.CHARGEGROUPNAME,
                                         CGCRID = b.CGCRID,
                                         FEENO = b.FEENO,
                                         CHARGERECORDTYPE = 2,
                                         CHARGERECORDID = a.MATERIALRECORDID,
                                         NAME = a.MATERIALNAME,
                                         CHARGEID = a.MATERIALID,
                                         UNITPRICE = a.UNITPRICE,
                                         QTY = a.QTY,
                                         UNITS = a.UNITS,
                                         COST = a.COST,
                                         TAKETIME = a.TAKETIME,
                                         STATUS = a.STATUS,
                                         CREATETIME = b.CREATETIME,
                                     }).Union(from a in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet
                                              join b in unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGERECORD>().dbSet on a.CGCRID equals b.CGCRID
                                              join c in unitOfWork.GetRepository<LTC_CHARGEGROUP>().dbSet on b.CHARGEGROUPID equals c.CHARGEGROUPID
                                              where a.ISDELETE == false && b.FEENO == request.Data.FeeNo && c.NSID == SecurityHelper.CurrentPrincipal.OrgId && b.CHARGEGROUPID == request.Data.ChargeGroupId && b.CGCRID == request.Data.CgcrId
                                              orderby b.CGCRID descending
                                              select new CHARGEITEM()
                                              {
                                                  CHARGEGROUPNAME = c.CHARGEGROUPNAME,
                                                  CGCRID = b.CGCRID,
                                                  FEENO = b.FEENO,
                                                  CHARGERECORDTYPE = 3,
                                                  CHARGERECORDID = a.SERVICERECORDID,
                                                  NAME = a.SERVICENAME,
                                                  CHARGEID = a.SERVICEID,
                                                  UNITPRICE = a.UNITPRICE,
                                                  QTY = a.QTY,
                                                  UNITS = a.UNITS,
                                                  COST = a.COST,
                                                  TAKETIME = a.TAKETIME,
                                                  STATUS = a.STATUS,
                                                  CREATETIME = b.CREATETIME,
                                              }).ToList();

            billData = billData.ToList().OrderByDescending(m => m.CGCRID).ToList();
            response.RecordsCount = billData.Count;
            List<CHARGEITEM> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = billData.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = billData.ToList();
            }

            response.Data = list;
            return response;
        }



        #endregion
    }
}
