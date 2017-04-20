using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.BillManagement;
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
    public interface IIpdOrderService : IBaseService
    {
        /// <summary>
        /// 查询医嘱记录
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>医嘱历史数据</returns>
        BaseResponse<IList<IpdOrder>> QueryIpdOrder(BaseRequest<IpdOrderFilter> request);

        /// <summary>
        /// 查询医嘱明细信息
        /// </summary>
        /// <param name="request">请求参数IpdOrderFilter</param>
        /// <returns>医嘱明细数据</returns>
        BaseResponse<IList<IpdOrder>> QueryIpdOrderDtl(BaseRequest<IpdOrderFilter> request);

        /// <summary>
        /// 查询医嘱明细信息
        /// </summary>
        /// <param name="request">请求参数IpdOrderFilter</param>
        /// <returns>医嘱明细数据</returns>
        BaseResponse<IList<IpdOrder>> QueryLoadOrder(BaseRequest<IpdOrderFilter> request);

        /// <summary>
        /// 查询收费项目明细
        /// </summary>
        /// <param name="request">请求参数IpdOrderFilter</param>
        /// <returns>收费项目数据</returns>
        BaseResponse<IList<CHARGEITEM>> QueryChargeItem(BaseRequest<IpdOrderFilter> request);

        /// <summary>
        /// 处理医嘱信息 涉及审核、编辑、停止、作废和校对
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回保存信息</returns>
        BaseResponse<IpdOrder> SaveIpdOrder(IpdOrder request);

        /// <summary>
        /// 保存新开或编辑医嘱信息
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>保存完信息</returns>
        BaseResponse<List<NewIpdOrder>> SaveIpdOrderList(IpdOrderList request);

        /// <summary>
        /// 查询已发送医嘱记录
        /// </summary>
        /// <param name="request">请求参数IpdOrderFilter</param>
        /// <returns>已发送的医嘱信息</returns>
        BaseResponse<IList<IpdOrder>> QueryOrderPostRec(BaseRequest<IpdOrderFilter> request);

        /// <summary>
        /// 删除已发送医嘱
        /// </summary>
        /// <param name="id"></param>
        BaseResponse Delete(IpdOrder baseRequest);

        /// <summary>
        /// 保存未发送医嘱
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>保存完信息</returns>
        BaseResponse<List<IpdOrder>> SaveNoSendOrders(NoSendIpdOrderList request);

        /// <summary>
        /// 查询院内三目明细
        /// </summary>
        /// <returns>收费项目数据</returns>
        BaseResponse<IList<CHARGEITEM>> QueryAllChargeItem(BaseRequest<IpdOrderFilter> request);
    }
}
