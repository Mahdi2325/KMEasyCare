/*
创建人: 刘美方
创建日期:2016-03-15
说明:库存管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface
{
    public interface IGoodsManageService : IBaseService
    {
        /// <summary>
        /// 获取物品列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Goods>> QueryGoods(BaseRequest<GoodsFilter> request);
        /// <summary>
        /// 获取物品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Goods> GetGoods(int id);
        /// <summary>
        /// 保存物品
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Goods> SaveGoods(Goods request);
        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteGoods(int id);


        /// <summary>
        /// 获取厂商列表
        /// </summary>
        /// <returns></returns>
        BaseResponse<IList<Manufacture>> QueryManufacture();

        /// <summary>
        /// 获取厂商列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Manufacture>> QueryManufacture(BaseRequest<CommonFilter> request);
        /// <summary>
        /// 获取厂商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Manufacture> GetManufacture(int id);
        /// <summary>
        /// 保存厂商
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Manufacture> SaveManufacture(Manufacture request);
        /// <summary>
        /// 删除厂商
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteManufacture(int id);


        /// <summary>
        /// 获取进贷列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<GoodsLoan>> QueryGoodsLoan(BaseRequest<GoodsRecordFilter> request);
        /// <summary>
        /// 获取进贷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<GoodsLoan> GetGoodsLoan(int id);
        /// <summary>
        /// 保存进贷
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<GoodsLoan> SaveGoodsLoan(GoodsLoan request);
        /// <summary>
        /// 删除进贷
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteGoodsLoan(int id);


        /// <summary>
        /// 获取出贷列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<GoodsSale>> QueryGoodsSale(BaseRequest<GoodsRecordFilter> request);
        /// <summary>
        /// 获取出贷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<GoodsSale> GetGoodsSale(int id);
        /// <summary>
        /// 保存出贷
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<GoodsSale> SaveGoodsSale(GoodsSale request);
        /// <summary>
        /// 删除出贷
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteGoodsSale(int id);

    }
}
