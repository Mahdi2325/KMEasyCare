using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.PackageRelated;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class CostService : BaseService, ICostService
    {
        /// <summary>
        /// 查询耗材记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<MaterialRecord>> QueryMaterialRec(BaseRequest<MaterialRecordFilter> request)
        {
            var response = base.Query<LTC_MATERIALRECORD, MaterialRecord>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATETIME);
                return q;
            });
            return response;
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
        /// 查询单条药品记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DrugRecord> GetDrugRec(BaseRequest<DrugRecordFilter> request)
        {
            return base.Get<LTC_DRUGRECORD, DrugRecord>(q => q.DRUGRECORDID == request.Data.DrugrecordId);
        }
        /// <summary>
        /// 查询服务记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<ServiceRecord>> QueryServiceRec(BaseRequest<ServiceRecordFilter> request)
        {
            var response = base.Query<LTC_SERVICERECORD, ServiceRecord>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderByDescending(m => m.CREATETIME);
                return q;
            });
            return response;
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
        /// 查询药品、耗材、服务的加总数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<CommonRecord>> QueryCommonRec(BaseRequest<ServiceRecordFilter> request)
        {
            var response = new BaseResponse<IList<CommonRecord>>();
            List<CommonRecord> ListResult = new List<CommonRecord>();
            var qDrugRec = from m in unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet
                               select new CommonRecord
                               {
                                   Id = m.DRUGRECORDID,
                                   NsId = m.NSID,
                                   Name = m.CNNAME,
                                   Units = m.UNITS,
                                   Qty = m.QTY,
                                   Unitprice = m.UNITPRICE,
                                   Cost = m.COST,
                                   Takeway = m.TAKEWAY,
                                   TakeTime = m.TAKETIME,
                                   Status=m.STATUS,
                                   Operator = m.OPERATOR,
                                   Comment = m.COMMENT,
                                   IsNciItem = m.ISNCIITEM,
                                   IsChargeGroupItem = m.ISCHARGEGROUPITEM,
                                   RecordType = "D",
                                   FeeNo = m.FEENO,
                                   IsDelete = m.ISDELETE,
                                   CreateTime = m.CREATETIME,
                               };
            qDrugRec = qDrugRec.Where(m => m.FeeNo == request.Data.FeeNo);
            qDrugRec = qDrugRec.Where(m => m.IsDelete == false);
            qDrugRec = qDrugRec.Where(m => m.IsChargeGroupItem == false);
            var qMaterialRec = from m in unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet
                     select new CommonRecord
                     {
                         Id = m.MATERIALRECORDID,
                         NsId = m.NSID,
                         Name = m.MATERIALNAME,
                         Units = m.UNITS,
                         Qty = m.QTY,
                         Unitprice = m.UNITPRICE,
                         Cost = m.COST,
                         Takeway = m.TAKEWAY,
                         TakeTime = m.TAKETIME,
                         Status = m.STATUS,
                         Operator = m.OPERATOR,
                         Comment = m.COMMENT,
                         IsNciItem = m.ISNCIITEM,
                         IsChargeGroupItem = m.ISCHARGEGROUPITEM,
                         RecordType = "M",
                         FeeNo = m.FEENO,
                         IsDelete = m.ISDELETE,
                         CreateTime=m.CREATETIME,
                     };
            qMaterialRec = qMaterialRec.Where(m => m.FeeNo == request.Data.FeeNo);
            qMaterialRec = qMaterialRec.Where(m => m.IsDelete == false);
            qMaterialRec = qMaterialRec.Where(m => m.IsChargeGroupItem == false);
            var qServiceRec = from m in unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet
                     select new CommonRecord
                     {
                         Id = m.SERVICERECORDID,
                         NsId = m.NSID,
                         Name = m.SERVICENAME,
                         Units = m.UNITS,
                         Qty = m.QTY,
                         Unitprice = m.UNITPRICE,
                         Cost = m.COST,
                         Takeway = m.TAKEWAY,
                         TakeTime = m.TAKETIME,
                         Status = m.STATUS,
                         Operator = m.OPERATOR,
                         Comment = m.COMMENT,
                         IsNciItem = m.ISNCIITEM,
                         IsChargeGroupItem = m.ISCHARGEGROUPITEM,
                         RecordType = "S",
                         FeeNo = m.FEENO,
                         IsDelete=m.ISDELETE,
                         CreateTime=m.CREATETIME,
                     };
            qServiceRec = qServiceRec.Where(m => m.FeeNo == request.Data.FeeNo);
            qServiceRec = qServiceRec.Where(m => m.IsDelete == false);
            qServiceRec = qServiceRec.Where(m => m.IsChargeGroupItem == false);
            List<CommonRecord> list = null;
            ListResult = qDrugRec.ToList().Union(qMaterialRec.ToList()).Union(qServiceRec.ToList()).ToList();
            ListResult = ListResult.OrderByDescending(m => m.CreateTime).ToList();
            response.RecordsCount = ListResult.Count;
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

        /// <summary>
        /// 保存服务信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<ServiceRecord>> SaveServiceRec(ServiceRec request)
        {
            var response = new BaseResponse<IList<ServiceRecord>>();
            foreach (var item in request.Data)
            {
                Mapper.Reset();
                Mapper.CreateMap<ServiceRecord, LTC_SERVICERECORD>();
                Mapper.CreateMap<LTC_SERVICERECORD, ServiceRecord>();
                var Model = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.FirstOrDefault(m => m.SERVICERECORDID == item.ServiceRecordId);
                if (Model == null)
                {
                    Model = Mapper.Map<LTC_SERVICERECORD>(item);
                    Model.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                    Model.ISDELETE = false;
                    Model.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.STATUS = 0;
                    Model.CREATETIME = DateTime.Now;
                    Model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_SERVICERECORD>().Insert(Model);
                }
                else
                {
                    Mapper.Map(item, Model);
                    Model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(Model);
                }
                unitOfWork.Save();
            }
            return response;
        }
        /// <summary>
        /// 保存耗材信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<MaterialRecord>> SaveMaterialRec(MaterialRec request)
        {
            var response = new BaseResponse<IList<MaterialRecord>>();
            foreach (var item in request.Data)
            {
                Mapper.Reset();
                Mapper.CreateMap<MaterialRecord, LTC_MATERIALRECORD>();
                Mapper.CreateMap<LTC_MATERIALRECORD, MaterialRecord>();
                var Model = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.FirstOrDefault(m => m.MATERIALRECORDID == item.MaterialRecordId);
                if (Model == null)
                {
                    Model = Mapper.Map<LTC_MATERIALRECORD>(item);
                    Model.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                    Model.ISDELETE = false;
                    Model.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.STATUS = 0;
                    Model.CREATETIME = DateTime.Now;
                    Model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_MATERIALRECORD>().Insert(Model);
                }
                else
                {
                    Mapper.Map(item, Model);
                    Model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(Model);
                }
                unitOfWork.Save();

            }
            return response;
        }

        public BaseResponse<IList<DrugRecord>> SaveDrugRec(DrugRec request)
        {
            var response = new BaseResponse<IList<DrugRecord>>();
            foreach (var item in request.Data)
            {
                Mapper.Reset();
                Mapper.CreateMap<DrugRecord, LTC_DRUGRECORD>();
                Mapper.CreateMap<LTC_DRUGRECORD, DrugRecord>();
                var Model = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.FirstOrDefault(m => m.DRUGRECORDID == item.DrugrecordId);
                if (Model == null)
                {
                    Model = Mapper.Map<LTC_DRUGRECORD>(item);
                    Model.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                    Model.ISDELETE = false;
                    Model.OPERATOR = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.CREATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.STATUS = 0;
                    Model.CREATETIME = DateTime.Now;
                    Model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_DRUGRECORD>().Insert(Model);
                }
                else
                {
                    Mapper.Map(item, Model);
                    Model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                    Model.UPDATETIME = DateTime.Now;
                    unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(Model);
                }
                unitOfWork.Save();

            }
            return response;
        }
        /// <summary>
        /// 删除套餐使用记录
        /// </summary>
        /// <param name="CgcrId"></param>
        /// <returns></returns>
        public BaseResponse DeleteChargeGroup(int CgcrId)
        {
            var response = new BaseResponse()
            {
                ResultCode = -1
            };
            var model = unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGERECORD>().dbSet.Where(m => m.CGCRID == CgcrId).FirstOrDefault();
            if (model != null)
            {
                model.ISDELETE = true;
                unitOfWork.GetRepository<LTC_CHARGEGROUP_CHARGERECORD>().Update(model);
            }

            var materialRecordList = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.CGCRID == CgcrId).ToList();
            if (materialRecordList != null)
            {
                materialRecordList.ForEach(subModel =>
                {
                    subModel.ISDELETE = true;
                    unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(subModel);
                });
            }

            var serviceRecordList = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.CGCRID == CgcrId).ToList();
            if (serviceRecordList != null)
            {
                serviceRecordList.ForEach(subModel =>
                {
                    subModel.ISDELETE = true;
                    unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(subModel);
                });
            }

            var drugRecordList = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.CGCRID == CgcrId).ToList();
            if (drugRecordList != null)
            {
                drugRecordList.ForEach(subModel =>
                {
                    subModel.ISDELETE = true;
                    unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(subModel);
                });
            }
            unitOfWork.Save();
            response.ResultCode = 0;
            return response;

        }
        /// <summary>
        /// 删除耗材信息
        /// </summary>
        /// <param name="MaterialRecordId"></param>
        public void DeleteMaterialRec(int MaterialRecordId)
        {
            var model = unitOfWork.GetRepository<LTC_MATERIALRECORD>().dbSet.Where(m => m.MATERIALRECORDID == MaterialRecordId).FirstOrDefault();
            if (model != null)
            {
                model.ISDELETE = true;
                unitOfWork.GetRepository<LTC_MATERIALRECORD>().Update(model);
                unitOfWork.Save();
            }
            //var strSql = String.Format("update LTC_MATERIALRECORD set ISDELETE=true where MATERIALRECORDID='{0}'", MaterialRecordId);
            //unitOfWork.GetRepository<LTC_MATERIALRECORD>().ExecuteSqlCommand(strSql);
            //unitOfWork.Save();
        }

        /// <summary>
        /// 删除服务信息
        /// </summary>
        /// <param name="serviceId"></param>
        public void DeleteServiceRec(int serviceRecordId)
        {
            var model = unitOfWork.GetRepository<LTC_SERVICERECORD>().dbSet.Where(m => m.SERVICERECORDID == serviceRecordId).FirstOrDefault();
            if (model != null)
            {
                model.ISDELETE = true;
                unitOfWork.GetRepository<LTC_SERVICERECORD>().Update(model);
                unitOfWork.Save();
            }
            //var strSql = String.Format("update LTC_SERVICERECORD set ISDELETE=true where SERVICERECORDID='{0}'", serviceRecordId);
            //unitOfWork.GetRepository<LTC_SERVICERECORD>().ExecuteSqlCommand(strSql);
            //unitOfWork.Save();
        }

        /// <summary>
        /// 删除药品信息
        /// </summary>
        /// <param name="serviceId"></param>
        public void DeleteDrugRec(int drugRecordId)
        {
            var model = unitOfWork.GetRepository<LTC_DRUGRECORD>().dbSet.Where(m => m.DRUGRECORDID == drugRecordId).FirstOrDefault();
            if (model != null)
            {
                model.ISDELETE = true;
                unitOfWork.GetRepository<LTC_DRUGRECORD>().Update(model);
                unitOfWork.Save();
            }
            //var strSql = String.Format("update LTC_DRUGRECORD set ISDELETE=true where DRUGRECORDID='{0}'", drugRecordId);
            //unitOfWork.GetRepository<LTC_DRUGRECORD>().ExecuteSqlCommand(strSql);
            //unitOfWork.Save();
        }


        ///Add By Duke On 20170117
        /// <summary>
        /// 按关键字查询定点机构药品信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<NSDrug>> QueryNsDrugByKeyWord(BaseRequest<PackageRelatedFilter> request)
        {
            var response = base.Query<LTC_NSDRUG, NSDrug>(request, (q) =>
            {
                if (!string.IsNullOrEmpty(request.Data.keyWord))
                {
                    q = q.Where(m => m.CNNAME.Contains(request.Data.keyWord) || m.ENNAME.Contains(request.Data.keyWord));
                }
                q = q.OrderBy(m => m.DRUGID);
                return q;
            });
            return response;
        }


    }
}
