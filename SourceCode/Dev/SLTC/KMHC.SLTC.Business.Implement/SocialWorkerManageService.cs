/*************************************************************************************************
 * 描述:评估类controller,包含了部分其他模块的内容(管路指标，多重用药指标，护理评估中的部分内容)
 * 创建日期:2016-3-9
 * 创建人:杨金高
 * **********************************************************************************************/
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
using System.Data.Objects;
using System.Data;
using System.Text;
using System.Reflection;
using NPOI.SS.Formula.Functions;


namespace KMHC.SLTC.Business.Implement
{
    public class SocialWorkerManageService : BaseService, ISocialWorkerManageService
    {
        #region ***********************补助申请***********************
        /// <summary>
        /// 获取补助申请列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<SubsidyView>> QuerySubsidy(BaseRequest<SubsidyFilter> request)
        {
            BaseResponse<IList<SubsidyView>> response = new BaseResponse<IList<SubsidyView>>();
            var q = from d in unitOfWork.GetRepository<LTC_SUBSIDYREC>().dbSet
                    where d.FEENO == request.Data.FeeNo
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.APPLYBY equals emp.EMPNO into res
                    join emp2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.NEXTAPPLYBY equals emp2.EMPNO into res2
                    from em in res.DefaultIfEmpty()
                    from em2 in res2.DefaultIfEmpty()
                    select new
                    {
                        SubsidyView = d,
                        FeeNo = d.FEENO,
                        ApplyByName = em.EMPNAME,
                        NextApplyByName = em2.EMPNAME
                    };
            //if (request.Data.FeeNo > 0)
            //{
            //    q.Where(m => m.FeeNo == request.Data.FeeNo);
            //}
            q = q.OrderByDescending(m => m.SubsidyView.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<SubsidyView>();
                foreach (dynamic item in list)
                {
                    SubsidyView newItem = Mapper.DynamicMap<SubsidyView>(item.SubsidyView);
                    newItem.ApplyByName = item.ApplyByName;
                    newItem.NextApplyByName = item.NextApplyByName;
                    response.Data.Add(newItem);
                }

                DateTime dt = DateTime.Now;

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

        /// <summary>
        /// 获取补助申请
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<SubsidyView> GetSubsidyById(int id)
        {
            return base.Get<LTC_SUBSIDYREC, SubsidyView>((q) => q.ID == id);
        }

        /// <summary>
        /// 保存补助申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<SubsidyView> SaveSubsidy(SubsidyView request)
        {
            return base.Save<LTC_SUBSIDYREC, SubsidyView>(request, (q) => q.ID == request.Id);
        }

        /// <summary>
        /// 删除补助申请
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteSubsidyById(int id)
        {
            return base.Delete<LTC_SUBSIDYREC>(id);
        }
        #endregion

        #region***********************资源连接***********************
        /// <summary>
        /// 获取资源连接列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<ResourceLinkModel>> QueryResourceLink(BaseRequest<ResourceLinkFilter> request)
        {

            //var response = base.Query<LTC_RESOURCELINKREC, ResourceLinkModel>(request, (q) =>
            //{
            //    if (request.Data.FeeNo > 0)
            //    {
            //        q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    }
            //    q = q.OrderByDescending(m => m.ID);
            //    return q;
            //});
            //return response;
            BaseResponse<IList<ResourceLinkModel>> response = new BaseResponse<IList<ResourceLinkModel>>();
            var q = from re in unitOfWork.GetRepository<LTC_RESOURCELINKREC>().dbSet
                    where re.FEENO == request.Data.FeeNo
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on re.RECORDBY equals emp.EMPNO into res
                    from em in res.DefaultIfEmpty()
                    select new
                    {
                        ResourceLinkModel = re,
                        FeeNo = re.FEENO,
                        RecordByName = em.EMPNAME
                    };
            //if (request.Data.FeeNo > 0)
            //{
            //    q.Where(m => m.FeeNo == request.Data.FeeNo);
            //}
            q = q.OrderByDescending(m => m.ResourceLinkModel.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ResourceLinkModel>();
                foreach (dynamic item in list)
                {
                    ResourceLinkModel newItem = Mapper.DynamicMap<ResourceLinkModel>(item.ResourceLinkModel);
                    newItem.RecordByName = item.RecordByName;
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

        /// <summary>
        /// 获取单个资源信息
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<ResourceLinkModel> GetResourceLink(int regNo)
        {
            return base.Get<LTC_RESOURCELINKREC, ResourceLinkModel>((q) => q.REGNO == regNo);
        }

        /// <summary>
        /// 保存资源连接信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<ResourceLinkModel> SaveResourceLink(ResourceLinkModel request)
        {
            return base.Save<LTC_RESOURCELINKREC, ResourceLinkModel>(request, (q) => q.ID == request.Id);
        }

        /// <summary>
        /// 根据指定资源ID进行删除
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeleteResourceLink(int regNo)
        {
            return base.Delete<LTC_RESOURCELINKREC>(regNo);
        }

        #endregion

        #region***********************生活记录***********************

        /// <summary>
        /// 查询生活记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<LifeRecordListModel>> QueryLifeRecordList(BaseRequest<LifeRecordListFilter> request)
        {
            //long? nullLong = null;
            //int? nullInt = null;
            //int? date = 0;
            BaseResponse<IList<LifeRecordListModel>> response = new BaseResponse<IList<LifeRecordListModel>>();
            if (request.Data != null)
            {

                //var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                //		join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO into reg_ds
                //		join life in unitOfWork.GetRepository<LTC_LIFERECORDS>().dbSet on new { REGNO = ipd.REGNO, RecordDate = date } equals new { REGNO = life.REGNO, RecordDate = System.Data.Entity.DbFunctions.DiffDays(life.RECORDDATE, request.Data.RecordDate) } into life_ds
                //		from reg_d in reg_ds.DefaultIfEmpty()
                //		from life_d in life_ds.DefaultIfEmpty()
                //		join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on life_d.RECORDBY equals emp.EMPNO into res
                //		from em in res.DefaultIfEmpty()
                //		where ipd.ORGID == SecurityHelper.CurrentPrincipal.OrgId && (string.IsNullOrEmpty(request.Data.RoomName) || ipd.BEDNO.Contains(request.Data.RoomName)) && (string.IsNullOrEmpty(request.Data.FloorName) || ipd.FLOOR.Contains(request.Data.FloorName))
                //		select new LifeRecordListModel
                //		{
                //			FeeNo = ipd.FEENO,
                //			BedNo = ipd.BEDNO,
                //			Floor = ipd.FLOOR,
                //			Name = reg_d.NAME,
                //			RecordDate = life_d.RECORDDATE,
                //			RecordBy = life_d.RECORDBY,
                //			BodyTemp = life_d.BODYTEMP,
                //			AmActivity = life_d.AMACTIVITY,
                //			PmActivity = life_d.PMACTIVITY,
                //			Comments = life_d.COMMENTS,
                //			RecordByName = em.EMPNAME,
                //			RecordId = life_d != null ? life_d.ID : nullLong,
                //			RegNo = ipd != null ? ipd.REGNO : nullInt
                //		};
                var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                        join _reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals _reg.REGNO into _regs
                        join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on ipd.FLOOR equals f.FLOORID into fs
                        join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on ipd.ROOMNO equals r.ROOMNO into rs
                        from reg in _regs.DefaultIfEmpty()
                        from floor in fs.DefaultIfEmpty()
                        from room in rs.DefaultIfEmpty()
                            //where ipd.ORGID == SecurityHelper.CurrentPrincipal.OrgId && (string.IsNullOrEmpty(request.Data.RoomName) || ipd.BEDNO.Contains(request.Data.RoomName)) && (string.IsNullOrEmpty(request.Data.FloorName) || ipd.FLOOR.Contains(request.Data.FloorName))
                            //join _life in unitOfWork.GetRepository<LTC_LIFERECORDS>().dbSet on ipd.FEENO equals _life.FEENO into _lifes
                            //from life in _lifes.DefaultIfEmpty()
                            //join _emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on life.RECORDBY equals _emp.EMPNO into _emps
                            //from emp in _emps.DefaultIfEmpty()
                        select new LifeRecordListModel
                        {
                            FeeNo = ipd.FEENO,
                            RegNo = reg.REGNO,
                            BedNo = ipd.BEDNO,
                            Floor = ipd.FLOOR,
                            Name = reg.NAME,
                            RoomNo = ipd.ROOMNO,
                            FloorName = floor.FLOORNAME,
                            RoomName = room.ROOMNAME
                            //RecordDate = life.RECORDDATE,
                            //RecordByName = emp.EMPNAME,
                            //BodyTemp = life.BODYTEMP,
                            //AmActivity = life.AMACTIVITY,
                            //PmActivity = life.PMACTIVITY,
                            //Comments = life.COMMENTS,
                            //RecordId = life.ID,

                        };

                //if (request.Data.RecordDate != null) q = q.Where(m => m.RecordDate >= request.Data.RecordDate);
                if (!string.IsNullOrEmpty(request.Data.RoomName)) q = q.Where(m => m.RoomNo == request.Data.RoomName);
                if (!string.IsNullOrEmpty(request.Data.FloorName)) q = q.Where(m => m.Floor == request.Data.FloorName);
                //q=q.Where(m=>m.ORGID==SecurityHelper.CurrentPrincipal.OrgId)
                q = q.OrderByDescending(m => m.FeeNo);
                response.RecordsCount = q.Count();
                Action<IList> mapperResponse = (IList list) =>
                {
                    response.Data = Mapper.DynamicMap<List<LifeRecordListModel>>(list);
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
            }


            return response;
        }

        /// <summary>
        /// 批量保存生活记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<List<LifeRecordModel>> SaveList(List<LifeRecordListModel> request)
        {

            var list = Mapper.DynamicMap<List<LifeRecordModel>>(request);
            List<LifeRecordModel> saveList = new List<LifeRecordModel>();
            LifeRecordModel temp = null;
            Mapper.CreateMap<LifeRecordModel, LTC_LIFERECORDS>();
            foreach (var item in request)
            {
                temp = new LifeRecordModel();
                temp.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                temp.CreateDate = DateTime.Now;
                temp.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                temp.AmActivity = item.AmActivity;
                temp.BodyTemp = item.BodyTemp;
                temp.Comments = item.Comments;
                temp.FeeNo = item.FeeNo;
                if (item.RecordId.HasValue)
                {
                    temp.Id = item.RecordId.Value;
                }
                else if (item.Id.HasValue) temp.Id = (long)item.Id;
                temp.PmActivity = item.PmActivity;
                temp.RecordBy = item.RecordBy;
                temp.RegNo = item.RegNo;
                temp.RecordDate = item.RecordDate;

                var model = Mapper.Map<LTC_LIFERECORDS>(temp);
                if (model.ID == 0)
                {
                    unitOfWork.GetRepository<LTC_LIFERECORDS>().Insert(model);
                }
                else
                {
                    unitOfWork.GetRepository<LTC_LIFERECORDS>().Update(model);
                }
            }
            unitOfWork.Save();
            BaseResponse<List<LifeRecordModel>> response = new BaseResponse<List<LifeRecordModel>>();
            return response;
        }


        /// <summary>
        /// 获取生活记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<LifeRecordModel>> QueryLifeRecord(BaseRequest<LifeRecordFilter> request)
        {
            BaseResponse<IList<LifeRecordModel>> response = new BaseResponse<IList<LifeRecordModel>>();
            var q = from d in unitOfWork.GetRepository<LTC_LIFERECORDS>().dbSet.Where(m => m.FEENO == request.Data.FeeNo)
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.RECORDBY equals emp.EMPNO into res
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on d.FEENO equals ipd.FEENO                //  保存 生活记录时 会存在regno 为空的情况出现  通过 ipdreg  regno 查询数据
                    join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO into d_regs
                    join emp1 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.CREATEBY equals emp1.EMPNO into res1
                    from em in res.DefaultIfEmpty()
                    from d_reg in d_regs.DefaultIfEmpty()
                    from em1 in res1.DefaultIfEmpty()
                    select new
                    {
                        LifeRecordModel = d,
                        ResidentsNo = d_reg.RESIDENGNO,
                        FeeNo = d.FEENO,
                        RecordByName = em.EMPNAME,
                        Name = d_reg.NAME,
                        Floor = ipd.FLOOR,
                        RoomNo = ipd.ROOMNO,
                        BedNo = ipd.BEDNO,
                        CreateByName = em1.EMPNAME,
                        RecordDate = d.RECORDDATE,
                        Id = d.ID
                    };
            if (request.Data.FeeNo.HasValue)
            {
                q.Where(m => m.FeeNo == request.Data.FeeNo);
            }
            //if (request.Data.StartDate.HasValue)
            //{
            //    q.Where(m => m.RecordDate.HasValue && m.RecordDate.Value.Date.CompareTo(request.Data.StartDate.Value) >= 0);
            //}
            //if (request.Data.EndDate.HasValue)
            //{
            //    q.Where(m => m.RecordDate.HasValue && m.RecordDate.Value.Date.CompareTo(request.Data.EndDate.Value) <= 0);
            //}
            if (request.Data.StartDate != null && request.Data.EndDate != null)
            {
                q = q.Where(m => m.RecordDate.HasValue && m.RecordDate.Value >= request.Data.StartDate.Value && m.RecordDate.Value <= request.Data.EndDate.Value);
            }

            q = q.OrderByDescending(m => m.LifeRecordModel.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<LifeRecordModel>();
                foreach (dynamic item in list)
                {
                    LifeRecordModel newItem = Mapper.DynamicMap<LifeRecordModel>(item.LifeRecordModel);
                    newItem.RecordByName = item.RecordByName;
                    newItem.Name = item.Name;
                    newItem.Floor = item.Floor;
                    newItem.RoomNo = item.RoomNo;
                    newItem.BedNo = item.BedNo;
                    newItem.ResidentsNo = item.ResidentsNo;
                    newItem.CreateBy = item.CreateByName;
                    newItem.Id = item.Id;
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
        /// <summary>
        /// 获取生活记录(指定id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<LifeRecordModel> GetLifeRecordById(int id)
        {
            return base.Get<LTC_LIFERECORDS, LifeRecordModel>((q) => q.ID == id);
        }

        /// <summary>
        ///　删除生活记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteLifeRecordById(int id)
        {
            return base.Delete<LTC_LIFERECORDS>(id);
        }

        /// <summary>
        /// 保存生活记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<LifeRecordModel> SaveLifeRecord(LifeRecordModel request)
        {
            return base.Save<LTC_LIFERECORDS, LifeRecordModel>(request, (q) => q.ID == request.Id);
        }

        #endregion

        #region *************************转介*************************
        /// <summary>
        /// 获取社工转介列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<ReferralRecModel>> QueryReferralRec(BaseRequest<ReferralRecFilter> request)
        {
            BaseResponse<IList<ReferralRecModel>> response = new BaseResponse<IList<ReferralRecModel>>();
            var q = from d in unitOfWork.GetRepository<LTC_REFERRALREC>().dbSet
                    where d.FEENO == request.Data.FeeNo && d.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.EMPNO equals emp.EMPNO into res
                    from em in res.DefaultIfEmpty()
                    select new
                    {
                        ReferralRecModel = d,
                        FeeNo = d.FEENO,
                        EmpName = em.EMPNAME
                    };
            q = q.OrderByDescending(m => m.ReferralRecModel.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ReferralRecModel>();
                foreach (dynamic item in list)
                {
                    ReferralRecModel newItem = Mapper.DynamicMap<ReferralRecModel>(item.ReferralRecModel);
                    newItem.EmpName = item.EmpName;
                    response.Data.Add(newItem);
                }

                DateTime dt = DateTime.Now;

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

        /// <summary>
        /// 获取转介
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<ReferralRecModel> GetReferralById(int id)
        {
            return base.Get<LTC_REFERRALREC, ReferralRecModel>((q) => q.ID == id);
        }

        /// <summary>
        /// 保存转介
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<ReferralRecModel> SaveReferral(ReferralRecModel request)
        {
            return base.Save<LTC_REFERRALREC, ReferralRecModel>(request, (q) => q.ID == request.Id);
        }

        /// <summary>
        /// 删除转介
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteReferralById(int id)
        {
            return base.Delete<LTC_REFERRALREC>(id);
        }
        #endregion

        #region*********************社工服务记录*********************
        /// <summary>
        /// 获取社工记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<CaresvrRecModel>> QueryCareSvrRec(BaseRequest<CaresvrRecFilter> request)
        {
            BaseResponse<IList<CaresvrRecModel>> response = new BaseResponse<IList<CaresvrRecModel>>();
            var q = from d in unitOfWork.GetRepository<LTC_CARERSVRREC>().dbSet
                    where d.FEENO == request.Data.FeeNo
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.CARER equals emp.EMPNO into res
                    from em in res.DefaultIfEmpty()
                    select new
                    {
                        CaresvrRecModel = d,
                        FeeNo = d.FEENO,
                        CarerName = em.EMPNAME
                    };
            q = q.OrderByDescending(m => m.CaresvrRecModel.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<CaresvrRecModel>();
                foreach (dynamic item in list)
                {
                    CaresvrRecModel newItem = Mapper.DynamicMap<CaresvrRecModel>(item.CaresvrRecModel);
                    newItem.CarerName = item.CarerName;
                    response.Data.Add(newItem);
                }

                DateTime dt = DateTime.Now;

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

        /// <summary>
        /// 获取社工记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<CaresvrRecModel> GetCareSvrById(int id)
        {
            return base.Get<LTC_CARERSVRREC, CaresvrRecModel>((q) => q.ID == id);
        }

        /// <summary>
        ///　删除社工记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteCareSvrById(int id)
        {
            return base.Delete<LTC_CARERSVRREC>(id);
        }

        /// <summary>
        /// 保存社工记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<CaresvrRecModel> SaveCareSvr(CaresvrRecModel request)
        {
            return base.Save<LTC_CARERSVRREC, CaresvrRecModel>(request, (q) => q.ID == request.Id);
        }

        #endregion

        #region***********************权益申诉***********************
        /// <summary>
        /// 获取申诉列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<ComplainRecModel>> QueryComplainRec(BaseRequest<ComplainRecFilter> request)
        {
            BaseResponse<IList<ComplainRecModel>> response = new BaseResponse<IList<ComplainRecModel>>();
            var q = from d in unitOfWork.GetRepository<LTC_COMPLAINREC>().dbSet
                    where d.FEENO == request.Data.FeeNo
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.PROCESSBY equals emp.EMPNO into res
                    join emp2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.EMPNO equals emp2.EMPNO into res2
                    from em in res.DefaultIfEmpty()
                    from em2 in res2.DefaultIfEmpty()
                    select new
                    {
                        ComplainRecModel = d,
                        FeeNo = d.FEENO,
                        PrcessName = em.EMPNAME,
                        EmpName = em2.EMPNAME
                    };
            //if (request.Data.FeeNo > 0)
            //{
            //    q.Where(m => m.FeeNo == request.Data.FeeNo);
            //}
            q = q.OrderByDescending(m => m.ComplainRecModel.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<ComplainRecModel>();
                foreach (dynamic item in list)
                {
                    ComplainRecModel newItem = Mapper.DynamicMap<ComplainRecModel>(item.ComplainRecModel);
                    newItem.ProcessName = item.PrcessName;
                    newItem.EmpName = item.EmpName;
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

        /// <summary>
        /// 获取申诉(指定id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<ComplainRecModel> GetComplainRecById(int id)
        {
            return base.Get<LTC_COMPLAINREC, ComplainRecModel>((q) => q.ID == id);
        }

        /// <summary>
        ///　删除申诉
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteComplainRecById(int id)
        {
            return base.Delete<LTC_COMPLAINREC>(id);
        }

        /// <summary>
        /// 保存申诉
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<ComplainRecModel> SaveComplainRec(ComplainRecModel request)
        {
            return base.Save<LTC_COMPLAINREC, ComplainRecModel>(request, (q) => q.ID == request.Id);
        }
        #endregion

        #region***********************居家督导***********************
        /// <summary>
        /// 获取居家督导服务列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<HomeCareSuperviseModel>> QueryHomeCareSupervise(BaseRequest<HomeCareSuperviseFilter> request)
        {
            var response = base.Query<LTC_HOMECARESUPERVISE, HomeCareSuperviseModel>(request, (q) =>
            {
                if (request.Data.FeeNo > 0)
                {
                    q = q.Where(m => m.FEENO == request.Data.FeeNo);
                }
                q = q.OrderBy(m => m.FEENO);
                return q;
            });
            return response;
        }

        /// <summary>
        /// 获取居家督导服务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<HomeCareSuperviseModel> GetHomeCareSuperviseById(int id)
        {
            return base.Get<LTC_HOMECARESUPERVISE, HomeCareSuperviseModel>((q) => q.ID == id);
        }

        /// <summary>
        ///　删除居家督导服务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteHomeCareSuperviseById(int id)
        {
            return base.Delete<LTC_HOMECARESUPERVISE>(id);
        }

        /// <summary>
        /// 保存居家督导服务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<HomeCareSuperviseModel> SaveHomeCareSupervise(HomeCareSuperviseModel request)
        {
            return base.Save<LTC_HOMECARESUPERVISE, HomeCareSuperviseModel>(request, (q) => q.ID == request.Id);
        }
        #endregion

        #region***********************管路指标***********************
        /// <summary>
        /// 获取管路
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<PipelineRecModel>> QueryPipelineRec(BaseRequest<PipelineRecFilter> request)
        {
            BaseResponse<IList<PipelineRecModel>> response = new BaseResponse<IList<PipelineRecModel>>();
            var q = from pe in unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet
                    where pe.FEENO == request.Data.FeeNo && pe.REMOVEDFLAG == request.Data.Removed
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on pe.OPERATOR equals emp.EMPNO into res
                    join emp2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on pe.OPERATOR equals emp2.EMPNO into res2
                    from em in res.DefaultIfEmpty()
                    from em2 in res2.DefaultIfEmpty()
                    select new
                    {
                        FeeNo = pe.FEENO,
                        PipelineRecModel = pe,
                        RemoveByName = em.EMPNAME,
                        OperatorName = em2.EMPNAME,
                        RemovedFlag = pe.REMOVEDFLAG
                    };
            q = q.OrderByDescending(m => m.PipelineRecModel.SEQNO);
            response.RecordsCount = q.Count();

            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<PipelineRecModel>();
                foreach (dynamic item in list)
                {
                    PipelineRecModel newItem = Mapper.DynamicMap<PipelineRecModel>(item.PipelineRecModel);
                    newItem.OperatorName = item.OperatorName;
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

        /// <summary>
        /// 获取单条管路数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<PipelineRecModel> GetPipelineRecById(int seqNo)
        {
            return base.Get<LTC_PIPELINEREC, PipelineRecModel>((q) => q.SEQNO == seqNo);
        }

        /// <summary>
        ///　删除删除管路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeletePipelineRecById(int seqNo)
        {
            return base.Delete<LTC_PIPELINEREC>(seqNo);
        }

        /// <summary>
        /// 保存管路
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<PipelineRecModel> SavePipelineRec(PipelineRecModel request)
        {
            request.Operator = SecurityHelper.CurrentPrincipal.EmpNo;
            request.CreateDate = DateTime.Now;

            BaseResponse<PipelineRecModel> pip = new BaseResponse<PipelineRecModel>();

            #region 请假外出日期
            var reqPipelineRec = unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet.OrderByDescending(m => m.ID).Where(m => m.FEENO == request.FeeNo).FirstOrDefault();

            if (reqPipelineRec != null)
            {
                if ((request.CreateDate >= reqPipelineRec.STARTDATE && request.CreateDate <= reqPipelineRec.RETURNDATE) || (request.CreateDate >= reqPipelineRec.STARTDATE && reqPipelineRec.RETURNDATE == null))
                {
                    pip.ResultCode = 1001;   //1001: 存在符合条件的数据    
                    pip.ResultMessage = "当前住民处於请假期间！";
                    return pip;
                }
            }

            #endregion

            #region 非计划入院日期
            var unPlanIpd = unitOfWork.GetRepository<LTC_UNPLANEDIPD>().dbSet.OrderByDescending(m => m.ID).Where(m => m.FEENO == request.FeeNo).FirstOrDefault();

            if (unPlanIpd != null)
            {
                if ((request.CreateDate >= unPlanIpd.INDATE && request.CreateDate <= unPlanIpd.OUTDATE) || (request.CreateDate >= unPlanIpd.INDATE && unPlanIpd.OUTDATE == null))
                {
                    pip.ResultCode = 1001;   //1001: 存在符合条件的数据    
                    pip.ResultMessage = "当前住民处於非计划期间！";
                    return pip;
                }
            }
            #endregion

            request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            return base.Save<LTC_PIPELINEREC, PipelineRecModel>(request, (q) => q.SEQNO == request.SeqNo);
        }

        /// <summary>
        /// 移除管路信息
        /// </summary>
        /// <param name="feeno"></param>
        /// <param name="pipelineName"></param>
        /// <param name="removeInfo"></param>
        public void RemovePipelineRec(long feeno, string pipelineName, DateTime dateTime, string removeInfo)
        {
            var reqPipelineRec = unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet.Where(m => m.FEENO == feeno && m.REMOVEDFLAG == false && m.PIPELINENAME == pipelineName).FirstOrDefault();
            if (reqPipelineRec != null)
            {
                if (reqPipelineRec.RECORDDATE >= dateTime)
                {
                    dateTime = Convert.ToDateTime(reqPipelineRec.RECORDDATE);
                }
                reqPipelineRec.REMOVEDFLAG = true;
                reqPipelineRec.REMOVEDATE = dateTime;
                reqPipelineRec.REMOVEREASON = removeInfo;
                unitOfWork.GetRepository<LTC_PIPELINEREC>().Update(reqPipelineRec);
            }
            unitOfWork.Save();
        }

        #region 管路明细

        //

        /// <summary>
        /// 获取管路明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<PipelineEvalModel>> QueryPipelineEval(BaseRequest<PipelineEvalFilter> request)
        {
            BaseResponse<IList<PipelineEvalModel>> response = new BaseResponse<IList<PipelineEvalModel>>();
            var q = from pe in unitOfWork.GetRepository<LTC_PIPELINEEVAL>().dbSet
                    where pe.SEQNO == request.Data.SeqNo
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on pe.OPERATOR equals emp.EMPNO into res
                    from em in res.DefaultIfEmpty()
                    select new
                    {
                        PipelineEvalModel = pe,
                        EvalDate = pe.EVALDATE,
                        RecentDate = pe.RECENTDATE,
                        State = pe.STATE,
                        Operator = pe.OPERATOR,
                        NextDate = pe.NEXTDATE,
                        OperatorName = em.EMPNAME
                    };
            q = q.OrderByDescending(m => m.PipelineEvalModel.SEQNO);
            if (request.Data.SeqNo > 0)
            {
                q.Where(m => m.PipelineEvalModel.SEQNO == request.Data.SeqNo);
            }
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<PipelineEvalModel>();
                foreach (dynamic item in list)
                {
                    PipelineEvalModel newItem = Mapper.DynamicMap<PipelineEvalModel>(item.PipelineEvalModel);
                    newItem.OperatorName = item.OperatorName;
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


        /// <summary>
        ///　删除删除管路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeletePipelineEvalById(int id)
        {
            return base.Delete<LTC_PIPELINEEVAL>(id);
        }

        /// <summary>
        /// 保存管路
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<PipelineEvalModel> SavePipelineEval(PipelineEvalModel request)
        {
            return base.Save<LTC_PIPELINEEVAL, PipelineEvalModel>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse<PipelineEvalModel> GetPipelineEvalToNurse(int feeNo, DateTime recDate, string pipeLineName)
        {
            BaseResponse<PipelineEvalModel> response = new BaseResponse<PipelineEvalModel>();
            var ss = (from o in unitOfWork.GetRepository<LTC_PIPELINEEVAL>().dbSet
                      join b in unitOfWork.GetRepository<LTC_PIPELINEREC>().dbSet on o.SEQNO equals b.SEQNO
                      where o.RECENTDATE == recDate && b.FEENO == feeNo && b.PIPELINENAME == pipeLineName
                      select 0).Count();

            if (ss > 0)
            {
                response.ResultCode = 1001;
            }
            else
            {
                response.ResultCode = 1002;
            }

            return response;
        }
        #endregion

        #endregion

        #region***********************社工评估***********************
        /// <summary>
        /// 获取社工评估
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<RegEvaluateModel>> QueryRegEvaluate(BaseRequest<RegEvaluateFilter> request)
        {

            BaseResponse<IList<RegEvaluateModel>> response = new BaseResponse<IList<RegEvaluateModel>>();

            var q = from n in unitOfWork.GetRepository<LTC_REGEVALUATE>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.EVALUATEBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        regEvaluate = n,
                        EmpName = re.EMPNAME

                    };
            q = q.Where(m => m.regEvaluate.FEENO == request.Data.FeeNo);
            q = q.OrderByDescending(m => m.regEvaluate.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<RegEvaluateModel>();
                foreach (dynamic item in list)
                {
                    RegEvaluateModel newItem = Mapper.DynamicMap<RegEvaluateModel>(item.regEvaluate);
                    newItem.EmpName = item.EmpName;
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

        /// <summary>
        /// 获取单条社工评估
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<RegEvaluateModel> GetRegEvaluateById(int id)
        {
            return base.Get<LTC_REGEVALUATE, RegEvaluateModel>((q) => q.ID == id);
        }

        /// <summary>
        ///　删除社工评估
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteRegEvaluateById(int id)
        {
            return base.Delete<LTC_REGEVALUATE>(id);
        }

        /// <summary>
        /// 保存社工评估
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<RegEvaluateModel> SaveRegEvaluate(RegEvaluateModel request)
        {
            return base.Save<LTC_REGEVALUATE, RegEvaluateModel>(request, (q) => q.ID == request.Id);
        }

        #endregion

        #region***********************多重用药***********************
        /// <summary>
        /// 获取多重用药
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<Pharmacist>> QueryPharmacist(BaseRequest<PharmacistFilter> request)
        {
            BaseResponse<IList<Pharmacist>> response = new BaseResponse<IList<Pharmacist>>();
            var q = from pe in unitOfWork.GetRepository<LTC_PHARMACISTEVAL>().dbSet
                    where pe.FEENO == request.Data.FeeNo && pe.ORGID == SecurityHelper.CurrentPrincipal.OrgId
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on pe.EVALUATEBY equals emp.EMPNO into res
                    join emp2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on pe.NEXTEVALUATEBY equals emp2.EMPNO into res2
                    // join vd in (from in_vd in unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet where EntityFunctions.DiffDays(DateTime.Now,in_vd.STARTDATE)<=90 select in_vd) on pe.FEENO equals vd.FEENO into vds
                    from em in res.DefaultIfEmpty()
                    from em2 in res2.DefaultIfEmpty()
                        // from vidc in vds.DefaultIfEmpty()
                    select new
                    {
                        Pharmacist = pe,
                        EvaluateByName = em.EMPNAME,
                        NextEvaluateByName = em2.EMPNAME
                    };
            q = q.OrderByDescending(m => m.Pharmacist.EVALDATE);
            //if (request.Data.FeeNo > 0)
            //{
            //    q.Where(m => m.Pharmacist.FEENO == request.Data.FeeNo);
            //}
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<Pharmacist>();
                foreach (dynamic item in list)
                {
                    Pharmacist newItem = Mapper.DynamicMap<Pharmacist>(item.Pharmacist);
                    newItem.EvaluateByName = item.EvaluateByName;
                    newItem.NextEvaluateByName = item.NextEvaluateByName;
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

        /// <summary>
        /// 获取单条多重用药
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<Pharmacist> GetPharmacistById(int id)
        {
            return base.Get<LTC_PHARMACISTEVAL, Pharmacist>((q) => q.ID == id);
        }

        /// <summary>
        ///　删除多重用药
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeletePharmacistById(int id)
        {
            return base.Delete<LTC_PHARMACISTEVAL>(id);
        }

        /// <summary>
        /// 保存多重用药
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<Pharmacist> SavePharmacist(Pharmacist request)
        {
            request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            return base.Save<LTC_PHARMACISTEVAL, Pharmacist>(request, (q) => q.ID == request.Id);
        }

        #endregion

        #region***********************院民清册***********************
        /// <summary>
        /// 获取院民清册列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<RSDList>> QueryPersonsExtend(BaseRequest<ResidentFilter> request)
        {
            BaseResponse<IList<RSDList>> response = new BaseResponse<IList<RSDList>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    join reg in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals reg.REGNO into reg_ipds
                    join rel in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on ipd.REGNO equals rel.REGNO into reg_rels
                    join e1 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.CARER equals e1.EMPNO into ipd_e1
                    join e2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NURSENO equals e2.EMPNO into ipd_e2
                    join e3 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.DOCTOR equals e3.EMPNO into ipd_e3
                    join e4 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NUTRITIONIST equals e4.EMPNO into ipd_e4
                    join e5 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.PHYSIOTHERAPIST equals e5.EMPNO into ipd_e5
                    join bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bed.FEENO into beds
                    join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on new { FLOOR = ipd.FLOOR, OrgID = ipd.ORGID } equals new { FLOOR = f.FLOORID, OrgID = f.ORGID } into fs
                    join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on new { ROOMNO = ipd.ROOMNO, OrgID = ipd.ORGID } equals new { ROOMNO = r.ROOMNO, OrgID = r.ORGID } into rs
                    //join code in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet on ipd.IPDFLAG equals code.ITEMCODE into ipd_codes
                    from reg_ipd in reg_ipds.DefaultIfEmpty()
                    from reg_rel in reg_rels.DefaultIfEmpty()
                    from ipd_ename1 in ipd_e1.DefaultIfEmpty()
                    from ipd_ename2 in ipd_e2.DefaultIfEmpty()
                    from ipd_ename3 in ipd_e3.DefaultIfEmpty()
                    from ipd_ename4 in ipd_e4.DefaultIfEmpty()
                    from ipd_ename5 in ipd_e5.DefaultIfEmpty()
                    from lbed in beds.DefaultIfEmpty()
                    from floor in fs.DefaultIfEmpty()
                    from room in rs.DefaultIfEmpty()
                        //from ipd_code in ipd_codes.DefaultIfEmpty()
                    select new RSDList
                    {
                        FeeNo = ipd.FEENO,
                        RegNo = ipd.REGNO,
                        OrgId = ipd.ORGID,
                        Name = reg_ipd.NAME,
                        Sex = reg_ipd.SEX,
                        Age = reg_ipd.AGE ?? 0,
                        BirthDay = reg_ipd.BRITHDATE,
                        RsType = ipd.RSTYPE,
                        RsStatus = ipd.RSSTATUS,
                        Floor = floor.FLOORID,// lbed.FLOOR,
                        RoomNo = room.ROOMNO,//lbed.ROOMNO,
                        FloorName = floor.FLOORNAME,// lbed.FLOOR,
                        RoomName = room.ROOMNAME,//lbed.ROOMNO,
                        BedNo = lbed.BEDNO,
                        BedKind = ipd.BEDKIND,
                        Nutritionist = ipd.NUTRITIONIST,
                        Carer = ipd.CARER,
                        Physiotherapist = ipd.PHYSIOTHERAPIST,
                        Doctor = ipd.DOCTOR,
                        BedClass = ipd.BEDCLASS,
                        IpdFlag = ipd.IPDFLAG,
                        InDate = ipd.INDATE,
                        OutDate = ipd.OUTDATE,
                        StateReason = ipd.STATEREASON,
                        StateFlag = ipd.STATEFLAG,
                        CarerName = ipd_ename1.EMPNAME,
                        NurseName = ipd_ename2.EMPNAME,
                        DoctorName = ipd_ename3.EMPNAME,
                        NutritionistName = ipd_ename4.EMPNAME,
                        PhysiotherapistName = ipd_ename5.EMPNAME,
                        //IpdFlagName = ipd_code.ITEMNAME,
                        CtrlFlag = ipd.CTRLFLAG,
                        CtrlReason = ipd.CTRLREASON,
                        CarerTips = ipd.CARERTIPS,
                        NursingTips = ipd.NURSINGTIPS,
                        DeptNo = ipd.DEPTNO,
                        NurseNo = ipd.NURSENO,
                        RoomFlag = ipd.ROOMFLAG,
                        ProtFlaf = ipd.PROTFLAF,
                        DangerFlag = ipd.DANGERFLAG,
                        SickFlag = ipd.SICKFLAG,
                        DepositAmt = ipd.DEPOSITAMT,
                        PrepayAmt = ipd.PREPAYAMT,
                        ServiceType = ipd.SERVICETYPE,
                        NurLevel = ipd.NURLEVEL,
                        RsAtt = ipd.RSATT,
                        FmyRsAtt = ipd.FMYRSATT,
                        NurAssSugg = ipd.NURASSSUGG,
                        OrgSugg = ipd.ORGSUGG,
                        ImpComment = ipd.IMPCOMMENT,
                        ResidengNo = reg_ipd.RESIDENGNO
                    };
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            }
            if (!string.IsNullOrEmpty(request.Data.IpdFlag))
            {
                if (request.Data.IpdFlag == "I")
                    q = q.Where(m => m.IpdFlag == "I" || m.IpdFlag == "N");
                if (request.Data.IpdFlag == "O")
                    q = q.Where(m => m.IpdFlag == "O");
            }
            if (!string.IsNullOrEmpty(request.Data.keyword))
            {
                q = q.Where(m => m.Name.Contains(request.Data.keyword)
                    || m.ResidengNo.Contains(request.Data.keyword));
            }
            if (!string.IsNullOrEmpty(request.Data.FloorName))
            {
                q = q.Where(m => m.Floor == request.Data.FloorName);
            }
            if (!string.IsNullOrEmpty(request.Data.RoomName))
            {
                q = q.Where(m => m.RoomNo == request.Data.RoomName);
            }
            q = q.OrderByDescending(m => m.RegNo);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;

        }
        /// <summary>
        /// 获取单条管路数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<IList<RSDList>> GetResidentIpdById(int id)
        {
            //return base.Get<LTC_IPDREG, RSDList>((q) => q.FEENO == id);
            BaseResponse<IList<RSDList>> response = new BaseResponse<IList<RSDList>>();
            var q = from ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                    where ipd.FEENO == id
                    join emp in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipd.REGNO equals emp.REGNO into res
                    join bed in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet on ipd.FEENO equals bed.FEENO into beds
                    join e1 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.CARER equals e1.EMPNO into ipd_e1
                    join e2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NURSENO equals e2.EMPNO into ipd_e2
                    join e3 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.DOCTOR equals e3.EMPNO into ipd_e3
                    join e4 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.NUTRITIONIST equals e4.EMPNO into ipd_e4
                    join e5 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on ipd.PHYSIOTHERAPIST equals e5.EMPNO into ipd_e5
                    join f in unitOfWork.GetRepository<LTC_ORGFLOOR>().dbSet on ipd.FLOOR equals f.FLOORID into fs
                    join r in unitOfWork.GetRepository<LTC_ORGROOM>().dbSet on ipd.ROOMNO equals r.ROOMNO into rs
                    join a in unitOfWork.GetRepository<LTC_REGRELATION>().dbSet on ipd.FEENO equals a.FEENO into ass
                    join cert in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on ipd.FEENO equals cert.FEENO into reg_nciinfo
                    from reg_nci in reg_nciinfo.DefaultIfEmpty()
                    from reg in res.DefaultIfEmpty()
                    from lbed in beds.DefaultIfEmpty()
                    from ipd_ename1 in ipd_e1.DefaultIfEmpty()
                    from ipd_ename2 in ipd_e2.DefaultIfEmpty()
                    from ipd_ename3 in ipd_e3.DefaultIfEmpty()
                    from ipd_ename4 in ipd_e4.DefaultIfEmpty()
                    from ipd_ename5 in ipd_e5.DefaultIfEmpty()
                    from floor in fs.DefaultIfEmpty()
                    from room in rs.DefaultIfEmpty()
                    from relation in ass.DefaultIfEmpty()
                    select new RSDList
                    {
                        FeeNo = ipd.FEENO,
                        RegNo = ipd.REGNO,
                        OrgId = ipd.ORGID,
                        IdNo = reg.IDNO,
                        Name = reg.NAME,
                        Sex = reg.SEX,
                        Age = reg.AGE ?? 0,
                        BirthDay = reg.BRITHDATE,
                        RsType = ipd.RSTYPE,
                        RsStatus = ipd.RSSTATUS,
                        Floor = lbed.FLOOR,
                        RoomNo = lbed.ROOMNO,
                        FloorName = floor.FLOORNAME,
                        RoomName = room.ROOMNAME,
                        Area = room.AREA,
                        RoomType = room.ROOMTYPE,
                        BedNo = lbed.BEDNO,
                        BedKind = ipd.BEDKIND,
                        Nutritionist = ipd.NUTRITIONIST,
                        Carer = ipd.CARER,
                        Physiotherapist = ipd.PHYSIOTHERAPIST,
                        Doctor = ipd.DOCTOR,
                        BedClass = ipd.BEDCLASS,
                        BedType = ipd.BEDTYPE,
                        IpdFlag = ipd.IPDFLAG,
                        InDate = ipd.INDATE,
                        OutDate = ipd.OUTDATE,
                        StateReason = ipd.STATEREASON,
                        StateFlag = ipd.STATEFLAG,
                        CarerName = ipd_ename1.EMPNAME,
                        NurseName = ipd_ename2.EMPNAME,
                        DoctorName = ipd_ename3.EMPNAME,
                        NutritionistName = ipd_ename4.EMPNAME,
                        PhysiotherapistName = ipd_ename5.EMPNAME,
                        CtrlFlag = ipd.CTRLFLAG,
                        CtrlReason = ipd.CTRLREASON,
                        CarerTips = ipd.CARERTIPS,
                        NursingTips = ipd.NURSINGTIPS,
                        DeptNo = ipd.DEPTNO,
                        NurseNo = ipd.NURSENO,
                        RoomFlag = ipd.ROOMFLAG,
                        ProtFlaf = ipd.PROTFLAF,
                        DangerFlag = ipd.DANGERFLAG,
                        SickFlag = ipd.SICKFLAG,
                        DepositAmt = ipd.DEPOSITAMT,
                        PrepayAmt = ipd.PREPAYAMT,
                        ServiceType = ipd.SERVICETYPE,
                        NurLevel = ipd.NURLEVEL,
                        RsAtt = ipd.RSATT,
                        FmyRsAtt = ipd.FMYRSATT,
                        NurAssSugg = ipd.NURASSSUGG,
                        OrgSugg = ipd.ORGSUGG,
                        ImpComment = ipd.IMPCOMMENT,
                        City2 = relation.CITY2,
                        Address2 = relation.ADDRESS2,
                        Address2Dtl = relation.ADDRESS2DTL,
                        ContactPhone = relation.CONTACTPHONE,
                        CertNo = reg_nci.CERTNO,
                        CertStatus=reg_nci.STATUS
                    };
            q = q.OrderByDescending(m => m.InDate);

            response.Data = q.ToList();

            return response;
        }

        public BaseResponse<BedBasic> GetBasicById(int id, int type)
        {
            if (type == 1)
                return base.Get<LTC_BEDBASIC, BedBasic>((q => q.FEENO == id));
            else return base.Get<LTC_BEDBASIC, BedBasic>((q => q.BEDNO == id.ToString()));
        }
        /// <summary>
        /// 保存院民信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<RSDList> SaveRSDList(RSDList request)
        {
            //更新入住(IPDREG)信息,此外还需要更新床位表两条记录的床位状态，将新床位状态更新为使用中，旧床位更新为空闲

            //更新旧床位状态为空闲
            if (!string.IsNullOrEmpty(request.OldBedNo) && request.FeeNo != null && request.OldBedNo != request.BedNo)
            {

                var oldModel = unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet.FirstOrDefault(m => m.FEENO == request.FeeNo);
                //更新旧床位状态为空闲
                BedBasic bed = new BedBasic()
                {
                    BedStatus = BedStatus.Empty.ToString(),
                    BedNo = request.OldBedNo,
                    FEENO = null,
                    RoomNo = oldModel.ROOMNO,
                    BedKind = oldModel.BEDKIND,
                    BedClass = oldModel.BEDCLASS,
                    Floor = oldModel.FLOOR,
                    DeptNo = oldModel.DEPTNO,
                    BedType = oldModel.BEDTYPE,
                    SexType = oldModel.SEXTYPE,
                    Prestatus = oldModel.PRESTATUS,
                    InsbedFlag = oldModel.INSBEDFLAG,
                    UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo,
                    UpdateDate = DateTime.Now,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                };
                Mapper.CreateMap<BedBasic, LTC_BEDBASIC>();
                Mapper.Map(bed, oldModel);
                if (!string.IsNullOrEmpty(oldModel.BEDNO))
                    unitOfWork.GetRepository<LTC_BEDBASIC>().Update(oldModel);

                //更新新床位状态为使用中
                var newModel = unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet.FirstOrDefault(m => m.BEDNO == request.BedNo);
                BedBasic newBed = new BedBasic()
                {
                    BedStatus = BedStatus.Used.ToString(),
                    BedNo = request.BedNo,
                    FEENO = request.FeeNo,
                    RoomNo = newModel.ROOMNO,
                    BedKind = newModel.BEDKIND,
                    BedClass = newModel.BEDCLASS,
                    Floor = newModel.FLOOR,
                    DeptNo = newModel.DEPTNO,
                    BedType = newModel.BEDTYPE,
                    SexType = newModel.SEXTYPE,
                    Prestatus = oldModel.PRESTATUS,
                    InsbedFlag = oldModel.INSBEDFLAG,
                    UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo,
                    UpdateDate = DateTime.Now,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                };
                Mapper.CreateMap<BedBasic, LTC_BEDBASIC>();
                Mapper.Map(newBed, newModel);
                if (!string.IsNullOrEmpty(newModel.BEDNO))
                    unitOfWork.GetRepository<LTC_BEDBASIC>().Update(newModel);


                BedTransferModel tm = new BedTransferModel()
                {
                    ID = 0,
                    FEENO = request.FeeNo,
                    DEPT_O = request.DeptNo,
                    FLOOR_O = request.OldFloor,
                    ROOM_O = request.OldRoomNo,
                    BEDNO_O = request.OldBedNo,
                    DEPT_D = request.DeptNo,
                    FLOOR_D = request.Floor,
                    ROOMNO_D = request.RoomNo,
                    BEDNO_D = request.BedNo,
                    TRANDESC = "更换床位",
                    UPDATEDATE = DateTime.Now,
                    UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                    ORGID = SecurityHelper.CurrentPrincipal.OrgId
                };

                base.Save<LTC_BEDTRANSFER, BedTransferModel>(tm, (q) => q.ID == 0);

                unitOfWork.Save();

            }
            else
            {
                //更新新床位状态为使用中
                //var newModel = unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet.FirstOrDefault(m => m.BEDNO == request.BedNo);
                BedBasic newBed = new BedBasic()
                {
                    BedStatus = BedStatus.Used.ToString(),
                    BedNo = request.BedNo,
                    FEENO = request.FeeNo,
                    RoomNo = request.RoomNo,
                    BedKind = request.BedKind,
                    BedClass = request.BedClass,
                    Floor = request.Floor,
                    DeptNo = request.DeptNo,
                    BedType = request.BedType,
                    SexType = request.SexType,
                    Prestatus = request.Prestatus,
                    InsbedFlag = request.InsbedFlag,
                    UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo,
                    UpdateDate = DateTime.Now,
                    OrgId = SecurityHelper.CurrentPrincipal.OrgId
                };
                //Mapper.CreateMap<BedBasic, LTC_BEDBASIC>();
                //Mapper.Map(newBed, newModel);
                //unitOfWork.GetRepository<LTC_BEDBASIC>().Update(newBed);
                base.Save<LTC_BEDBASIC, BedBasic>(newBed, (q) => q.BEDNO == request.BedNo);

                BedTransferModel tm = new BedTransferModel()
                {
                    ID = 0,
                    FEENO = request.FeeNo,
                    DEPT_O = request.DeptNo,
                    FLOOR_O = request.OldFloor,
                    ROOM_O = request.OldRoomNo,
                    BEDNO_O = request.OldBedNo,
                    DEPT_D = request.DeptNo,
                    FLOOR_D = request.Floor,
                    ROOMNO_D = request.RoomNo,
                    BEDNO_D = request.BedNo,
                    TRANDESC = "更换床位",
                    UPDATEDATE = DateTime.Now,
                    UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo,
                    ORGID = SecurityHelper.CurrentPrincipal.OrgId
                };

                base.Save<LTC_BEDTRANSFER, BedTransferModel>(tm, (q) => q.ID == 0);

                //unitOfWork.Save();
            }

            //更新入住(IPDREG)信息
            return base.Save<LTC_IPDREG, RSDList>(request, (q) => q.FEENO == request.FeeNo);
        }
        public object GetNursingHomeAndLeaveRes()
        {
            return new
            {
                totalResNum = (from i in unitOfWork.GetRepository<LTC_IPDREG>().dbSet
                               where i.IPDFLAG == "I"
                               join r in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on i.FEENO equals r.FEENO
                               where r.STATUS == 0
                               select i).Count(),
                leaveResNum = (from l in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet
                               where l.STARTDATE <= DateTime.Now && (l.RETURNDATE >= DateTime.Now || l.RETURNDATE == null)
                               join r in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet on l.FEENO equals r.FEENO
                               where r.STATUS == 0
                               select l).Count(),
            };
        }
        #endregion

        #region***********************智能仪表盘*********************
        /// <summary>
        /// 获取智能仪表盘的入院分析结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<Dashboard>> QueryPersonsExtend(BaseRequest<DashboardFilter> request)
        {
            //var response = base.Query<ltc, HomeCareSuperviseModel>(request, (q) =>
            //{
            //    if (request.Data.FeeNo > 0)
            //    {
            //        q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    }
            //    q = q.OrderBy(m => m.FEENO);
            //    return q;
            //});
            return null;
        }

        /// <summary>
        /// 查询本院当前入住人数统计
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable QueryInData(string orgId)
        {
            DataTable dtRs = new DataTable();
            List<Dashboard> list = new List<Dashboard>();
            StringBuilder sbIn = new StringBuilder();
            try
            {
                //sbIn.Append("SELECT ISNULL(pvt.[01],0) AS [01],pvt.[02],pvt.[03],pvt.[04],pvt.[05],pvt.[06],pvt.[07],pvt.[08],pvt.[09],pvt.[10],pvt.[11],pvt.[12] FROM (");
                //sbIn.Append(" SELECT B.SEX,MONTH(INDATE) AS MM, CONVERT(char(10),A.INDATE,121) AS INDATE");
                //sbIn.Append(" FROM LTC_REGFILE B LEFT JOIN LTC_IPDREG A ON A.REGNO=B.REGNO ");
                //sbIn.AppendFormat(" WHERE INDATE<>'' AND B.NAME<>'' AND SEX<>'' AND A.ORGID='{0}' AND IPDFLAG IN('I','N') AND SEX IN('F','M','N') AND YEAR(INDATE)=YEAR(GETDATE())", orgId);
                //sbIn.Append(") P PIVOT(");
                //sbIn.Append("COUNT(INDATE) FOR MM IN ([01],[02],[03],[04],[05],[06],[07],[08],[09],[10],[11],[12]) ");
                //sbIn.Append(") PVT");
                var curYear = DateTime.Now.Year;
                sbIn.Append("SELECT ");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-01-01' AND '" + curYear + "-01-31'  THEN '1月' END )AS 'One',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-02-01' AND '" + curYear + "-02-28'  THEN '2月' END )AS 'Two',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-03-01' AND '" + curYear + "-03-31'  THEN '3月' END )AS 'Three',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-04-01' AND '" + curYear + "-04-30'  THEN '4月' END )AS 'Four',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-05-01' AND '" + curYear + "-05-31'  THEN '5月' END )AS 'Five',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-06-01' AND '" + curYear + "-06-30'  THEN '6月' END )AS 'Six',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-07-01' AND '" + curYear + "-07-31'  THEN '7月' END )AS 'Seven',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-08-01' AND '" + curYear + "-08-31'  THEN '8月' END )AS 'Eight',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-09-01' AND '" + curYear + "-09-30'  THEN '9月' END )AS 'Nine',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-10-01' AND '" + curYear + "-10-31'  THEN '10月' END )AS 'Ten',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-11-01' AND '" + curYear + "-11-30'  THEN '11月' END )AS 'Eleven',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.INDATE BETWEEN '" + curYear + "-12-01' AND '" + curYear + "-12-31'  THEN '12月' END )AS 'Twelve'");
                sbIn.Append(" FROM LTC_IPDREG LEFT JOIN LTC_REGFILE USING(REGNO) WHERE LTC_REGFILE.SEX IS NOT NULL AND LTC_IPDREG.ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "'  AND LTC_IPDREG.IPDFLAG='I' GROUP BY LTC_REGFILE.SEX");


                list = unitOfWork.GetRepository<Dashboard>().SqlQuery(sbIn.ToString()).ToList();//SqlQueryForDataTatable(sbIn.ToString(), null);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                sbIn.Clear();
                sbIn.Remove(0, sbIn.Length);
            }
            return ToDataTable(list);
        }
        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }
        /// <summary>
        /// 查询本院当前结案人数统计
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable QueryOutData(string orgId)
        {
            DataTable dtRs = new DataTable();
            List<Dashboard> list = new List<Dashboard>();
            StringBuilder sbIn = new StringBuilder();
            try
            {
                //sbIn.Append("SELECT ISNULL(pvt.[01],0) AS [01],pvt.[02],pvt.[03],pvt.[04],pvt.[05],pvt.[06],pvt.[07],pvt.[08],pvt.[09],pvt.[10],pvt.[11],pvt.[12] FROM (");
                //sbIn.Append(" SELECT B.SEX,MONTH(OUTDATE) AS MM, CONVERT(char(10),A.OUTDATE,121) AS OUTDATE");
                //sbIn.Append(" FROM LTC_REGFILE B LEFT JOIN  LTC_IPDREG A ON A.REGNO=B.REGNO ");
                //sbIn.AppendFormat(" WHERE OUTDATE<>'' AND B.NAME<>'' AND SEX<>'' AND A.ORGID='{0}' AND IPDFLAG IN('O') AND SEX IN('F','M','N') AND YEAR(INDATE)=YEAR(GETDATE())", orgId);
                //sbIn.Append(") P PIVOT(");
                //sbIn.Append("COUNT(OUTDATE) FOR MM IN ([01],[02],[03],[04],[05],[06],[07],[08],[09],[10],[11],[12]) ");
                //sbIn.Append(") PVT");

                var curYear = DateTime.Now.Year;
                sbIn.Append("SELECT ");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-01-01' AND '" + curYear + "-01-31'  THEN '1月' END )AS 'One',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-02-01' AND '" + curYear + "-02-28'  THEN '2月' END )AS 'Two',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-03-01' AND '" + curYear + "-03-31'  THEN '3月' END )AS 'Three',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-04-01' AND '" + curYear + "-04-30'  THEN '4月' END )AS 'Four',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-05-01' AND '" + curYear + "-05-31'  THEN '5月' END )AS 'Five',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-06-01' AND '" + curYear + "-06-30'  THEN '6月' END )AS 'Six',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-07-01' AND '" + curYear + "-07-31'  THEN '7月' END )AS 'Seven',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-08-01' AND '" + curYear + "-08-31'  THEN '8月' END )AS 'Eight',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-09-01' AND '" + curYear + "-09-30'  THEN '9月' END )AS 'Nine',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-10-01' AND '" + curYear + "-10-31'  THEN '10月' END )AS 'Ten',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-11-01' AND '" + curYear + "-11-30'  THEN '11月' END )AS 'Eleven',");
                sbIn.Append("COUNT(CASE WHEN LTC_IPDREG.OUTDATE BETWEEN '" + curYear + "-12-01' AND '" + curYear + "-12-31'  THEN '12月' END )AS 'Twelve'");
                sbIn.Append(" FROM LTC_IPDREG LEFT JOIN LTC_REGFILE USING(REGNO) WHERE LTC_REGFILE.SEX IS NOT NULL AND LTC_IPDREG.ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "'  AND LTC_IPDREG.IPDFLAG='O' GROUP BY LTC_REGFILE.SEX");


                list = unitOfWork.GetRepository<Dashboard>().SqlQuery(sbIn.ToString()).ToList();//SqlQueryForDataTatable(sbIn.ToString(), null);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                sbIn.Remove(0, sbIn.Length);
            }
            return ToDataTable(list);
        }


        public DataTable QueryBedData(string orgId)
        {
            DataTable dtRs = new DataTable();
            List<BedCount> list = new List<BedCount>();
            StringBuilder sbIn = new StringBuilder();
            try
            {
                //sbIn.Append("SELECT BEDSTATUS,COUNT(BEDNO) AS COUNTNUM FROM LTC_BEDBASIC WHERE ORGID='1' GROUP BY BEDSTATUS");
                sbIn.AppendFormat("SELECT BEDSTATUS, COUNT(BEDNO) AS Num FROM LTC_BEDBASIC WHERE ORGID='{0}' GROUP BY BEDSTATUS", orgId);
                if (!string.IsNullOrEmpty(orgId))
                {

                    list = unitOfWork.GetRepository<BedCount>().SqlQuery(sbIn.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                sbIn.Remove(0, sbIn.Length);
            }
            return ToDataTable(list);
        }
        public object QueryBedData2(string orgId)
        {
            return from l in unitOfWork.GetRepository<LTC_BEDBASIC>().dbSet
                   where l.ORGID == orgId
                   group l by l.BEDSTATUS into g
                   select new
                   {
                       BEDSTATUS = g.Key,
                       Num = g.Count()

                   };
        }
        /// <summary>
        /// 查询本院当前压疮人数统计
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable QueryBedSoreData(string orgId)
        {
            DataTable dtRs = new DataTable();
            List<BedSore> list = new List<BedSore>();
            StringBuilder sbIn = new StringBuilder();
            try
            {
                //sbIn.Append("SELECT pvt.[01] AS [01],pvt.[02],pvt.[03],pvt.[04],pvt.[05],pvt.[06],pvt.[07],pvt.[08],pvt.[09],pvt.[10],pvt.[11],pvt.[12] FROM(");
                //sbIn.Append(" SELECT ISNULL(A.SEX,0) AS SEX,MONTH(B.OCCURDATE) AS MM,B.OCCURDATE");
                //sbIn.AppendFormat(" FROM LTC_REGFILE A LEFT JOIN LTC_BEDSOREREC B ON A.REGNO=B.REGNO WHERE A.SEX<>'' AND A.SEX IN('F','M','N') AND YEAR(OCCURDATE)=YEAR(GETDATE()) AND A.ORGID='{0}'", orgId);
                //sbIn.Append(" ) P PIVOT(COUNT(OCCURDATE) FOR MM IN ([01],[02],[03],[04],[05],[06],[07],[08],[09],[10],[11],[12])");
                //sbIn.Append(") PVT");

                sbIn.Append("SELECT A.Sex,");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-01-01' and '2016-01-31'  THEN '1月' END )AS 'One',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-02-01' and '2016-02-28'  THEN '2月' END )AS 'Two',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-03-01' and '2016-03-31'  THEN '3月' END )AS 'Three',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-04-01' and '2016-04-30'  THEN '4月' END )AS 'Four',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-05-01' and '2016-05-31'  THEN '5月' END )AS 'Five',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-06-01' and '2016-06-30'  THEN '6月' END )AS 'Six',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-07-01' and '2016-07-31'  THEN '7月' END )AS 'Seven',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-08-01' and '2016-08-31'  THEN '8月' END )AS 'Eight',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-09-01' and '2016-09-30'  THEN '9月' END )AS 'Nine',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-10-01' and '2016-10-31'  THEN '10月' END )AS 'Ten',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-11-01' and '2016-11-30'  THEN '11月' END )AS 'Eleven',");
                sbIn.Append("COUNT(CASE WHEN B.OCCURDATE BETWEEN '2016-12-01' and '2016-12-31'  THEN '12月' END )AS 'Twelve'");
                sbIn.Append("FROM LTC_REGFILE A LEFT JOIN LTC_BEDSOREREC B USING(REGNO) WHERE A.SEX IS NOT NULL and A.ORGID='" + orgId + "' ");
                sbIn.Append("GROUP BY A.SEX ");


                list = unitOfWork.GetRepository<BedSore>().SqlQuery(sbIn.ToString()).ToList();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sbIn.Remove(0, sbIn.Length);
            }
            return ToDataTable(list);
        }

        #endregion

        #region **********************营养评估单**********************
        /// <summary>
        /// 获取营养评估单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<NutritionEvalModel>> QueryNutritionEval(BaseRequest<NutritionEvalFilter> request)
        {
            BaseResponse<IList<NutritionEvalModel>> response = new BaseResponse<IList<NutritionEvalModel>>();
            var q = from ne in unitOfWork.GetRepository<LTC_NUTRTIONEVAL>().dbSet.Where(m => m.FEENO == request.Data.FeeNo)
                    select new NutritionEvalModel
                    {
                        FEENO = request.Data.FeeNo,
                        NAME = ne.NAME,
                        ID = ne.ID,
                        DISEASEDIAG = ne.DISEASEDIAG,
                        CHEWDIFFCULT = ne.CHEWDIFFCULT,
                        SWALLOWABILITY = ne.SWALLOWABILITY,
                        EATPATTERN = ne.EATPATTERN,
                        DIGESTIONPROBLEM = ne.DIGESTIONPROBLEM,
                        FOODTABOO = ne.FOODTABOO,
                        ACTIVITYABILITY = ne.ACTIVITYABILITY,
                        PRESSURESORE = ne.PRESSURESORE,
                        EDEMA = ne.EDEMA,
                        CURRENTDIET = ne.CURRENTDIET,
                        EATAMOUNT = ne.EATAMOUNT,
                        WATER = ne.WATER,
                        SUPPLEMENTS = ne.SUPPLEMENTS,
                        SNACK = ne.SNACK,
                        EVALDATE = ne.EVALDATE
                    };
            //if (!string.IsNullOrEmpty(request.Data.FeeNo.ToString()))
            //	q = q.Where(p => p.FEENO == request.Data.FeeNo);

            q = q.OrderByDescending(p => p.ID);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }

            return response;
        }
        /// <summary>
        /// 根据住民收费编号(FeeNo)获取其生化检查数据
        /// </summary>
        /// <param name="feeNo"></param>
        /// <param name="code"></param>
        /// <param name="s_date"></param>
        /// <param name="e_date"></param>
        /// <returns></returns>
        public BaseResponse<IList<BiochemistryModel>> QueryBiochemistryByDate(int feeNo, string code1, string code2, string code3, DateTime s_date, DateTime e_date)
        {
            BaseResponse<IList<BiochemistryModel>> response = new BaseResponse<IList<BiochemistryModel>>();
            var q = from d in unitOfWork.GetRepository<LTC_CHECKRECDTL>().dbSet
                    join e in unitOfWork.GetRepository<LTC_CHECKREC>().dbSet on d.RECORDID equals e.RECORDID into dts
                    join i in unitOfWork.GetRepository<LTC_CHECKITEM>().dbSet on d.CHECKITEM equals i.ITEMCODE into its
                    from dtl in dts.DefaultIfEmpty()
                    from item in its.DefaultIfEmpty()
                    select new BiochemistryModel
                    {
                        FeeNo = dtl.FEENO,
                        ItemCode = item.ITEMCODE,
                        ItemName = item.ITEMNAME,
                        EvalDate = dtl.CHECKDATE,
                        CheckItem = d.CHECKITEM,
                        CheckDate = dtl.CHECKDATE
                    };
            if (feeNo > 0)
                q = q.Where(m => m.FeeNo == feeNo);
            if (!string.IsNullOrEmpty(code1) && !string.IsNullOrEmpty(code2) && !string.IsNullOrEmpty(code3))
                q = q.Where(m => m.CheckItem == code1 || m.CheckItem == code2 || m.CheckItem == code3);

            if (s_date != null)
                q = q.Where(m => m.EvalDate >= s_date);
            if (e_date != null)
                //e_date = e_date.AddDays(1);
                q = q.Where(m => m.EvalDate <= e_date);
            q = q.OrderByDescending(m => m.EvalDate);
            response.Data = q.ToList();
            return response;
        }
        /// <summary>
        /// 获取营养评估单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<NutritionEvalModel> GetNutritionEvalById(int id)
        {
            return base.Get<LTC_NUTRTIONEVAL, NutritionEvalModel>((q) => q.ID == id);
        }

        /// <summary>
        /// 保存营养评估单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<NutritionEvalModel> SaveNutritionEval(NutritionEvalModel request)
        {
            return base.Save<LTC_NUTRTIONEVAL, NutritionEvalModel>(request, (q) => q.ID == request.ID);
        }

        /// <summary>
        /// 删除营养评估单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteNutritionEvalById(long id)
        {
            return base.Delete<LTC_NUTRTIONEVAL>(id);
        }
        #endregion

        #region 个别化需求评估及计划
        public BaseResponse<IList<RegActivityRequEval>> QueryActivityRequEval(BaseRequest<RegActivityRequEvalFilter> request)
        {
            Mapper.CreateMap<LTC_REGACTIVITYREQUEVAL, RegActivityRequEval>();
            var response = new BaseResponse<IList<RegActivityRequEval>>();
            var q = from n in unitOfWork.GetRepository<LTC_REGACTIVITYREQUEVAL>().dbSet.Where(m => m.FEENO == request.Data.FeeNo)
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.CARER equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        RegActivityRequEval = n,
                        CarerName = re.EMPNAME
                    };
            q = q.OrderByDescending(m => m.RegActivityRequEval.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<RegActivityRequEval>();
                foreach (dynamic item in list)
                {
                    RegActivityRequEval newItem = Mapper.DynamicMap<RegActivityRequEval>(item.RegActivityRequEval);
                    newItem.CarerName = item.CarerName;
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
            //var result = q.ToList();
            //mapperResponse(result);
            return response;
        }

        public BaseResponse<RegActivityRequEval> GetActivityRequEval(long id)
        {
            return base.Get<LTC_REGACTIVITYREQUEVAL, RegActivityRequEval>((q) => q.ID == id);
        }

        public BaseResponse<RegActivityRequEval> SaveActivityRequEval(RegActivityRequEval request)
        {
            return base.Save<LTC_REGACTIVITYREQUEVAL, RegActivityRequEval>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteActivityRequEval(long id)
        {
            return base.Delete<LTC_REGACTIVITYREQUEVAL>(id);
        }
        #endregion

        #region **************新进住民环境介绍记录表*************
        public BaseResponse<IList<NewResideEntenvRec>> QueryNewResideEntenvRec(BaseRequest<NewResideEntenvRecFilter> request)
        {
            string Orgid = "";
            BaseResponse<IList<NewResideEntenvRec>> response = new BaseResponse<IList<NewResideEntenvRec>>();
            if (!string.IsNullOrEmpty(request.Data.ORGID))
            { Orgid = request.Data.ORGID; }
            else
            { Orgid = SecurityHelper.CurrentPrincipal.OrgId; }


            var q = from lcNewResideEntenvRec in unitOfWork.GetRepository<LTC_NEWRESIDENTENVREC>().dbSet
                    where (lcNewResideEntenvRec.ORGID.Equals(Orgid))
                    select new NewResideEntenvRec
                    {

                        ID = lcNewResideEntenvRec.ID,
                        RESIDENGNO = lcNewResideEntenvRec.RESIDENGNO,
                        BEDNO = lcNewResideEntenvRec.BEDNO,
                        SEX = lcNewResideEntenvRec.SEX,
                        INDATE = lcNewResideEntenvRec.INDATE,
                        RECORDDATE = lcNewResideEntenvRec.RECORDDATE,
                        BIRTHDATE = lcNewResideEntenvRec.BIRTHDATE,
                        FAMILYPARTICIPATION = lcNewResideEntenvRec.FAMILYPARTICIPATION,
                        CONTRACTFLAG = lcNewResideEntenvRec.CONTRACTFLAG,
                        LIFEFLAG = lcNewResideEntenvRec.LIFEFLAG,
                        STAFF1 = lcNewResideEntenvRec.STAFF1,
                        REGULARACTIVITY = lcNewResideEntenvRec.REGULARACTIVITY,
                        NOTREGULARACTIVITY = lcNewResideEntenvRec.NOTREGULARACTIVITY,
                        STAFF2 = lcNewResideEntenvRec.STAFF2,
                        BELLFLAG = lcNewResideEntenvRec.BELLFLAG,
                        LAMPFLAG = lcNewResideEntenvRec.LAMPFLAG,
                        TVFLAG = lcNewResideEntenvRec.TVFLAG,
                        LIGHTSWITCH = lcNewResideEntenvRec.LIGHTSWITCH,
                        ESCAPEDEVICE = lcNewResideEntenvRec.ESCAPEDEVICE,
                        ENVIRONMENT = lcNewResideEntenvRec.ENVIRONMENT,
                        COMMUNITYFACILITIES = lcNewResideEntenvRec.COMMUNITYFACILITIES,
                        POSTOFFICE = lcNewResideEntenvRec.POSTOFFICE,
                        SCHOOL = lcNewResideEntenvRec.SCHOOL,
                        BANK = lcNewResideEntenvRec.BANK,
                        STATION = lcNewResideEntenvRec.STATION,
                        PARK = lcNewResideEntenvRec.PARK,
                        TEMPLE = lcNewResideEntenvRec.TEMPLE,
                        HOSPITAL = lcNewResideEntenvRec.HOSPITAL,
                        OTHERFACILITIES = lcNewResideEntenvRec.OTHERFACILITIES,
                        CLEANLINESS = lcNewResideEntenvRec.CLEANLINESS,
                        MEDICALCARE = lcNewResideEntenvRec.MEDICALCARE,
                        MEALSERVICE = lcNewResideEntenvRec.MEALSERVICE,
                        WORKACTIVITIES = lcNewResideEntenvRec.WORKACTIVITIES,
                        STAFF3 = lcNewResideEntenvRec.STAFF3,
                        STAFF4 = lcNewResideEntenvRec.STAFF4,
                        PERSONINCHARGE = lcNewResideEntenvRec.PERSONINCHARGE,
                        DIRECTOR = lcNewResideEntenvRec.DIRECTOR,
                        NURSE = lcNewResideEntenvRec.NURSE,
                        NURSEAIDES = lcNewResideEntenvRec.NURSEAIDES,
                        RESIDENT = lcNewResideEntenvRec.RESIDENT,
                        DOCTOR = lcNewResideEntenvRec.DOCTOR,
                        SOCIALWORKER = lcNewResideEntenvRec.SOCIALWORKER,
                        DIETITIAN = lcNewResideEntenvRec.DIETITIAN,
                        OTHERPEOPLE = lcNewResideEntenvRec.OTHERPEOPLE,
                        STAFF5 = lcNewResideEntenvRec.STAFF5,
                        RECORDBY = lcNewResideEntenvRec.RECORDBY,
                        FEENO = lcNewResideEntenvRec.FEENO,
                        REGNO = lcNewResideEntenvRec.REGNO,
                        ORGID = lcNewResideEntenvRec.ORGID,
                    };

            q = q.Where(m => m.FEENO == request.Data.FEENO);

            q = q.OrderByDescending(m => m.ID);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }



            return response;


        }

        public BaseResponse<NewResideEntenvRec> GetNewResideEntenvRecById(int id)
        {

            return base.Get<LTC_NEWRESIDENTENVREC, NewResideEntenvRec>((q => q.ID == id));

        }

        public BaseResponse DeleteNewResideEntenvRecById(int id)
        {
            BaseResponse response = null;
            unitOfWork.BeginTransaction();
            var item = unitOfWork.GetRepository<LTC_NEWRESIDENTENVREC>().dbSet.Where(m => (m.ID == id)).FirstOrDefault();
            if (item != null)
            {
                base.Delete<LTC_NEWRESIDENTENVREC>(m => m.ID == id);

            }
            unitOfWork.Commit();
            return response;
        }

        public BaseResponse<NewResideEntenvRec> SaveNewResideEntenvRec(NewResideEntenvRec request)
        {
            BaseResponse<NewResideEntenvRec> dd = new BaseResponse<NewResideEntenvRec>();
            unitOfWork.BeginTransaction();

            var responsePerson = base.Save<LTC_NEWRESIDENTENVREC, NewResideEntenvRec>(request, (q) => q.ID == request.ID);

            unitOfWork.Commit();
            return responsePerson;

        }

        #endregion

        #region **************新进住民环境适应入辅导记录表*************
        public BaseResponse<IList<NewRegEnvAdaptation>> QueryNewRegEnvAdaptation(BaseRequest<NewResideEntenvRecFilter> request)
        {
            string Orgid = "";
            BaseResponse<IList<NewRegEnvAdaptation>> response = new BaseResponse<IList<NewRegEnvAdaptation>>();
            if (!string.IsNullOrEmpty(request.Data.ORGID))
            { Orgid = request.Data.ORGID; }
            else
            { Orgid = SecurityHelper.CurrentPrincipal.OrgId; }


            var q = from lcNewResideEntenvRec in unitOfWork.GetRepository<LTC_NEWREGENVADAPTATION>().dbSet
                    where (lcNewResideEntenvRec.ORGID.Equals(Orgid))
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on lcNewResideEntenvRec.CREATEBY equals emp.EMPNO into res
                    from em in res.DefaultIfEmpty()
                    select new NewRegEnvAdaptation
                    {

                        ID = lcNewResideEntenvRec.ID,
                        FEENO = lcNewResideEntenvRec.FEENO,
                        REGNO = lcNewResideEntenvRec.REGNO,
                        COORDINATION = lcNewResideEntenvRec.COORDINATION,
                        INTERPERSONAL = lcNewResideEntenvRec.INTERPERSONAL,
                        INDATE = lcNewResideEntenvRec.INDATE,
                        W1EVALDATE = lcNewResideEntenvRec.W1EVALDATE,
                        ORGID = lcNewResideEntenvRec.ORGID,
                        CREATEBY = em.EMPNAME,
                        INFORMFLAG = lcNewResideEntenvRec.INFORMFLAG,
                        WEEK = lcNewResideEntenvRec.WEEK,
                        COMMFLAG = lcNewResideEntenvRec.COMMFLAG,
                        PARTICIPATION = lcNewResideEntenvRec.PARTICIPATION,
                        EMOTION = lcNewResideEntenvRec.EMOTION,
                        RESISTANCE = lcNewResideEntenvRec.RESISTANCE,
                        HELP = lcNewResideEntenvRec.HELP,
                        PROCESSACTIVITY = lcNewResideEntenvRec.PROCESSACTIVITY,
                        TRACEREC = lcNewResideEntenvRec.TRACEREC,
                        EVALUATION = lcNewResideEntenvRec.EVALUATION
                    };


            if (!(request.Data.FEENO == 0 || request.Data.FEENO == null))
            {
                q = q.Where(m => m.FEENO == request.Data.FEENO);
            }
            q = q.OrderByDescending(m => m.ID);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }



            return response;


        }

        public BaseResponse<NewRegEnvAdaptation> GetNewRegEnvAdaptationById(int id)
        {

            return base.Get<LTC_NEWREGENVADAPTATION, NewRegEnvAdaptation>((q => q.ID == id));

        }

        public BaseResponse DeleteNewRegEnvAdaptationById(int id)
        {
            BaseResponse response = null;
            unitOfWork.BeginTransaction();
            var item = unitOfWork.GetRepository<LTC_NEWREGENVADAPTATION>().dbSet.Where(m => (m.ID == id)).FirstOrDefault();
            if (item != null)
            {
                base.Delete<LTC_NEWREGENVADAPTATION>(m => m.ID == id);

            }
            unitOfWork.Commit();
            return response;
        }

        public BaseResponse<NewRegEnvAdaptation> SaveNewRegEnvAdaptation(NewRegEnvAdaptation request)
        {
            BaseResponse<NewRegEnvAdaptation> dd = new BaseResponse<NewRegEnvAdaptation>();
            unitOfWork.BeginTransaction();

            var responsePerson = base.Save<LTC_NEWREGENVADAPTATION, NewRegEnvAdaptation>(request, (q) => q.ID == request.ID);

            unitOfWork.Commit();
            return responsePerson;

        }

        #endregion
    }
}

