using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.PackageRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface ICostService : IBaseService
    {
        /// <summary>
        /// 查询耗材记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<MaterialRecord>> QueryMaterialRec(BaseRequest<MaterialRecordFilter> request);

        /// <summary>
        /// 查询单条耗材记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<MaterialRecord> GetMaterialRec(BaseRequest<MaterialRecordFilter> request);

        /// <summary>
        /// 查询单条药品记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DrugRecord> GetDrugRec(BaseRequest<DrugRecordFilter> request);

        /// <summary>
        /// 查询服务记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ServiceRecord>> QueryServiceRec(BaseRequest<ServiceRecordFilter> request);

        /// <summary>
        /// 查询单条服务记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ServiceRecord> GetServiceRec(BaseRequest<ServiceRecordFilter> request);

        /// <summary>
        /// 查询服务耗材加总信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CommonRecord>> QueryCommonRec(BaseRequest<ServiceRecordFilter> request);

        /// <summary>
        /// 删除套餐使用记录
        /// </summary>
        /// <param name="CgcrId"></param>
        /// <returns></returns>
        BaseResponse DeleteChargeGroup(int CgcrId);

        /// <summary>
        /// 删除耗材信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void DeleteMaterialRec(int MaterialRecordId);

        /// <summary>
        /// 删除服务信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void DeleteServiceRec(int serviceRecordId);
        /// <summary>
        /// 删除药品信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void DeleteDrugRec(int drugRecordId);

        /// <summary>
        /// 保存服务信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ServiceRecord>> SaveServiceRec(ServiceRec request);

        /// <summary>
        /// 保存耗材信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<MaterialRecord>> SaveMaterialRec(MaterialRec request);

        /// <summary>
        /// 保存药品信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<DrugRecord>> SaveDrugRec(DrugRec request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<NSDrug>> QueryNsDrugByKeyWord(BaseRequest<PackageRelatedFilter> request);
    }
}

