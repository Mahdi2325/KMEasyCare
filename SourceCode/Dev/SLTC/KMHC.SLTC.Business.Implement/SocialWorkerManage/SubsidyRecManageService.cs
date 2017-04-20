using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;
using KMHC.SLTC.Business.Implement.Base;
using KMHC.SLTC.Business.Interface.SocialWorkerManage;
using AutoMapper;
using System.Linq;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Business.Entity;
using System;
namespace KMHC.SLTC.Business.Implement.SocialWorkerManage
{
    public class SubsidyRecManageService:BaseService,ISubsidyRecManageService
    {
        /// <summary>
        /// 保存/修改补助申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<SubsidyView> Save(SubsidyView request)
        {
            var response = new BaseResponse<SubsidyView>();
            if (!string.IsNullOrEmpty(request.ApplyBy))
            {
                Mapper.CreateMap<SubsidyView, LTC_SUBSIDYREC>();
                var model = unitOfWork.GetRepository<LTC_SUBSIDYREC>().dbSet.FirstOrDefault(m => m.ID == request.Id);
                if (model == null)
                {
                    model = BuildModel(request);
                    //model = Mapper.Map<LTC_SUBSIDYREC>(request);
                    unitOfWork.GetRepository<LTC_SUBSIDYREC>().Insert(model);
                }
                else
                {
                    Mapper.Map(request, model);
                    unitOfWork.GetRepository<LTC_SUBSIDYREC>().Update(model);
                }
                try
                {
                    unitOfWork.Save();
                    response.ResultCode = (int)EnumResponseStatus.Success;
                }
                catch(Exception ex) {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                response.ResultCode = (int)EnumResponseStatus.Success;
                response.ResultMessage = "申请人不能为空！";
            }
            return response;
        }
        /// <summary>
        /// 删除一条补助申请记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse Delete(long id) {
            BaseResponse response = new BaseResponse();
            unitOfWork.GetRepository<LTC_SUBSIDYREC>().Delete(id);
            unitOfWork.Save();
            return response;
        }
        public BaseResponse<SubsidyView> Get(long id)
        {
            
            Mapper.CreateMap<LTC_SUBSIDYREC, SubsidyView>();
            var response = new BaseResponse<SubsidyView>();
            var findItem = unitOfWork.GetRepository<LTC_SUBSIDYREC>().dbSet.FirstOrDefault(m => m.ID == id);
            if (findItem != null) {
                response.Data = Mapper.Map<SubsidyView>(unitOfWork.GetRepository<LTC_SUBSIDYREC>().dbSet.FirstOrDefault(m => m.ID == id));
            }
            return response;
        }
        public BaseResponse<IList<SubsidyView>> Query(BaseRequest<SubsidyFilter> request) {
            BaseResponse<IList<SubsidyView>> response = new BaseResponse<IList<SubsidyView>>();
            Mapper.CreateMap<LTC_SUBSIDYREC,SubsidyView>();
            var q = from m in unitOfWork.GetRepository<LTC_SUBSIDYREC>().dbSet
                    select m;
            response.RecordsCount = q.Count();
            List<LTC_SUBSIDYREC> list = null;
            if (request != null)
            {
                list = q.Skip(request.CurrentPage * request.PageSize).Take(request.PageSize).ToList();
            }
            else {
                list = q.ToList();
            }
            response.Data = Mapper.Map<IList<SubsidyView>>(list);
            return response;
        }
        private LTC_SUBSIDYREC BuildModel(SubsidyView model)
        {
            LTC_SUBSIDYREC svModel = new LTC_SUBSIDYREC()
            {
               
                APPLYBY="Mofel",
                APPLYDATE=DateTime.Parse("2016-06-06"),
                CREATEBY="Admin",
                CREATEDATE=DateTime.Now,
                DESCRIPTION="Test",
                ITEMNAME="补助一",
                NEXTAPPLYBY="Dennis",
                NEXTAPPLYDATE=DateTime.Now.AddDays(30),
                ORGID="1010101",
                FEENO=93249802341,
                REGNO=104
            };
            return svModel;
        }
    }
}

