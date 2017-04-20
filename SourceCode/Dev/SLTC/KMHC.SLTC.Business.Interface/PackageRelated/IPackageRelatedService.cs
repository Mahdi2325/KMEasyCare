
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.PackageRelated;
using System.Collections.Generic;
namespace KMHC.SLTC.Business.Interface
{
    public interface IPackageRelatedService : IBaseService
    {
      
        BaseResponse<IList<CHARGEGROUP>> QueryChargeGroupList(BaseRequest<PackageRelatedFilter> request);
        BaseResponse<CHARGEGROUP> SavePacMaintain(CHARGEGROUP request);
        BaseResponse<CHARGEGROUP> GetChargeGroup(string id);
        BaseResponse DeleteChargeGroup(string id);
        BaseResponse DeleteChargeItem(int id);
        BaseResponse<IList<RESCHARGEGRO>> QueryResChargeGro(BaseRequest<PackageRelatedFilter> request);
        BaseResponse<RESCHARGEGRO> SaveResChargeGro(RESCHARGEGRO request);
        BaseResponse DeleteResChargeGro(long id);

        #region 套餐费用录入
        /// <summary>
        /// 查询套餐费用录入列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<MainChargeItem>> QueryChargeGroupRec(BaseRequest<ChargeGroupRecFilter> request);

        /// <summary>
        /// 套餐详细列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CHARGEITEM>> QueryChargeGroupList(BaseRequest<ChargeGroupRecFilter> request);
        /// <summary>
        /// 保存套餐费用录入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ChargeGroupRec> SaveChargeGroupRec(ChargeItemData request);

        /// <summary>
        /// 查询单条药品记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<DrugRecord> GetDrugRec(BaseRequest<DrugRecordFilter> request);

        /// <summary>
        /// 查询单条耗材记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<MaterialRecord> GetMaterialRec(BaseRequest<MaterialRecordFilter> request);

        /// <summary>
        /// 查询单条服务记录信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ServiceRecord> GetServiceRec(BaseRequest<ServiceRecordFilter> request);


        #endregion
    }
}
