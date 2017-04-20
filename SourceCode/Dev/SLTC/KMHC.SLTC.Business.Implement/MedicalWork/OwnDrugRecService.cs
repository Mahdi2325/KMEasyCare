using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model.MedicalWork;
using KMHC.SLTC.Persistence;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Implement
{
    public class OwnDrugRecService : BaseService, IOwnDrugRecService
    {
        public BaseResponse<IList<OwnDrugDtlModel>> QueryOwnDrugDtl(BaseRequest<OwnDrugRecFilter> request)
        {
            BaseResponse<IList<OwnDrugDtlModel>> response = new BaseResponse<IList<OwnDrugDtlModel>>();
            var ownDrugDtl = (from a in unitOfWork.GetRepository<LTC_OWNDRUGREC>().dbSet
                              join b in unitOfWork.GetRepository<LTC_OWNDRUGDTL>().dbSet on a.OWNDRUGID equals b.OWNDRUGID
                              where a.FEENO == request.Data.FeeNo && b.ISDELETE == false && a.ORGID == SecurityHelper.CurrentPrincipal.OrgId && b.OWNDRUGID==request.Data.OwnDrugId
                              orderby a.OPERTORTIME descending
                              select new OwnDrugDtlModel()
                              {
                                  Id = b.ID,
                                  OwnDrugId = b.OWNDRUGID,
                                  DrugId=b.DRUGID,
                                  MCDrugCode = b.MCDRUGCODE,
                                  NSDrugCode = b.NSDRUGCODE,
                                  CNName = b.CNNAME,
                                  Qty=b.QTY,
                                  Units=b.UNITS,
                                  Manufacturer = b.MANUFACTURER,
                                  BatchNo = b.BATCHNO,
                                  Comment = b.COMMENT,
                                  CreateBy = b.CREATEBY,
                                  CreateTime = b.CREATETIME,
                                  UpdateBy = b.UPDATEBY,
                                  UpdateTime = b.UPDATETIME,
                                  IsDelete = b.ISDELETE,
                              }).ToList();
            response.RecordsCount = ownDrugDtl.Count;
            List<OwnDrugDtlModel> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = ownDrugDtl.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = ownDrugDtl.ToList();
            }
            response.Data = list;
            return response;
        }

        public BaseResponse<IList<OwnDrugRecModel>> QueryOwnDrugRec(BaseRequest<OwnDrugRecFilter> request)
        {
            BaseResponse<IList<OwnDrugRecModel>> response = new BaseResponse<IList<OwnDrugRecModel>>();
            var ownDrugRec = (from a in unitOfWork.GetRepository<LTC_OWNDRUGREC>().dbSet
                                  where a.FEENO == request.Data.FeeNo && a.ISDELETE == false && a.ORGID== SecurityHelper.CurrentPrincipal.OrgId
                                  orderby a.OPERTORTIME descending
                                  select new OwnDrugRecModel()
                                  {
                                      OwnDrugId = a.OWNDRUGID,
                                      FeeNo = a.FEENO,
                                      OrgId = a.ORGID,
                                      Reason = a.REASON,
                                      SponsorName = a.SPONSORNAME,
                                      OpertorName = a.OPERTORNAME,
                                      OpertorTime = a.OPERTORTIME,
                                      CreateBy = a.CREATEBY,
                                      CreateTime = a.CREATETIME,
                                      UpdateBy = a.UPDATEBY,
                                      UpdateTime = a.UPDATETIME,
                                      IsDelete = a.ISDELETE,
                                  }).ToList();
            response.RecordsCount = ownDrugRec.Count;
            List<OwnDrugRecModel> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = ownDrugRec.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = ownDrugRec.ToList();
            }
            response.Data = list;
            return response;
        }


        public BaseResponse Delete(int id)
        {
            return base.SoftDelete<LTC_OWNDRUGREC>(id);
        }

        public BaseResponse<OwnDrugRecModel> SaveOwnDrugRec(OwnDrugRecModel request)
        {
            var ownDrugRec = unitOfWork.GetRepository<LTC_OWNDRUGREC>().dbSet.Where(m => m.OWNDRUGID == request.OwnDrugId).FirstOrDefault();
            if (ownDrugRec != null)
            {
                request.OwnDrugId = ownDrugRec.OWNDRUGID;
                request.FeeNo = ownDrugRec.FEENO;
                request.OrgId = ownDrugRec.ORGID;
                request.Reason = request.Reason;
                request.SponsorName = request.SponsorName;
                request.OpertorName = request.OpertorName;
                request.OpertorTime = request.OpertorTime;
                request.CreateBy = ownDrugRec.CREATEBY;
                request.CreateTime = ownDrugRec.CREATETIME;
                request.IsDelete = ownDrugRec.ISDELETE;
                request.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.UpdateTime = DateTime.Now;
                return base.Save<LTC_OWNDRUGREC, OwnDrugRecModel>(request, (q) => q.OWNDRUGID == request.OwnDrugId);
            }
            else
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateTime = DateTime.Now;
                request.IsDelete = false;
                return base.Save<LTC_OWNDRUGREC, OwnDrugRecModel>(request, (q) => q.OWNDRUGID == request.OwnDrugId);
            }
            
        }

        public BaseResponse<List<OwnDrugDtlModel>> SaveOwnDrugDtl(OwnDrugDtlList request)
        {
            BaseResponse<List<OwnDrugDtlModel>> response = new BaseResponse<List<OwnDrugDtlModel>>();
            if (request.OwnDrugDtlLists != null )
            {
                if (request.OwnDrugDtlLists.Count > 0)
                {
                    unitOfWork.BeginTransaction();

                    //先全部delete
                    var befSaveOwnDrugDtl = unitOfWork.GetRepository<LTC_OWNDRUGDTL>().dbSet.Where(m => m.OWNDRUGID == request.UpdateOwnDrugId && m.ISDELETE==false).ToList();
                    if (befSaveOwnDrugDtl != null)
                    {
                        foreach (var bef in befSaveOwnDrugDtl)
                        {
                            base.SoftDelete<LTC_OWNDRUGDTL>(bef.ID);
                        }
                    }

                    #region 自带药品明细
                    foreach (var item in request.OwnDrugDtlLists)
                    {
                        item.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                        item.UpdateTime= DateTime.Now;

                        LTC_OWNDRUGDTL model = new LTC_OWNDRUGDTL();
                        model.ID = item.Id;
                        model.DRUGID = item.DrugId;
                        model.OWNDRUGID = item.OwnDrugId;
                        model.MCDRUGCODE = item.MCDrugCode;
                        model.NSDRUGCODE = item.NSDrugCode;
                        model.CNNAME = item.CNName;
                        model.QTY = item.Qty;
                        model.UNITS = item.Units;
                        model.MANUFACTURER = item.Manufacturer;
                        model.BATCHNO = item.BatchNo;
                        model.COMMENT = item.Comment;
                        model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                        model.CREATETIME = DateTime.Now;
                        model.ISDELETE = false;
                        unitOfWork.GetRepository<LTC_OWNDRUGDTL>().Insert(model);

                        //var ownDrugDtl = unitOfWork.GetRepository<LTC_OWNDRUGDTL>().dbSet.Where(m => m.ID == item.Id).FirstOrDefault();
                        //if (ownDrugDtl != null)
                        //{
                        //    ownDrugDtl.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                        //    ownDrugDtl.CREATETIME = DateTime.Now;
                        //    unitOfWork.GetRepository<LTC_OWNDRUGDTL>().Update(ownDrugDtl);
                        //}else
                        //{
                        //    LTC_OWNDRUGDTL model = new LTC_OWNDRUGDTL();
                        //    model.ID = item.Id;
                        //    model.OWNDRUGID = item.OwnDrugId;
                        //    model.MCDRUGCODE = item.MCDrugCode;
                        //    model.NSDRUGCODE = item.NSDrugCode;
                        //    model.CNNAME = item.CNName;
                        //    model.QTY = item.Qty;
                        //    model.UNITS = item.Units;
                        //    model.MANUFACTURER = item.Manufacturer;
                        //    model.BATCHNO = item.BatchNo;
                        //    model.COMMENT = item.Comment;
                        //    model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                        //    model.CREATETIME = DateTime.Now;
                        //    model.ISDELETE = false;
                        //    unitOfWork.GetRepository<LTC_OWNDRUGDTL>().Insert(model);
                        //}
                    }
                    unitOfWork.Save();
                    unitOfWork.Commit();
                    #endregion
                }
            }
            else
            {
                response.ResultCode = -1;
                response.ResultMessage = "未查询到自带药品信息！";
            }
            return response;
        }
    }
}
