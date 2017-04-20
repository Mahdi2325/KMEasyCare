/*
创建人: 肖国栋
创建日期:2016-03-09
说明:费用管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface
{
    public interface ICostManageService
    {
        #region LTC_BILL
        /// <summary>
        /// 获取LTC_BILL列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Bill>> QueryBill(BaseRequest<BillFilter> request);
        /// <summary>
        /// 获取LTC_BILL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Bill> GetBill(long id);
        /// <summary>
        /// 创建账单
        /// </summary>
        /// <returns></returns>
        BaseResponse GenerateBill(int FeeNo, string OrgId);
        /// <summary>
        /// 保存LTC_BILL
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Bill> SaveBill(Bill request);
        /// <summary>
        /// 删除LTC_BILL
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteBill(long id);
        #endregion

        #region LTC_PAYBILL
        /// <summary>
        /// 获取LTC_PAYBILL列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<PayBill>> QueryPayBill(BaseRequest<PayBillFilter> request);
        /// <summary>
        /// 获取LTC_PAYBILL列表COST列的和
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<decimal?> getSum(BaseRequest<PayBillFilter> request);
        /// <summary>
        /// 获取LTC_PAYBILL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<PayBill> GetPayBill(long id);
        /// <summary>
        /// 保存LTC_PAYBILL
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<PayBill> SavePayBill(PayBill request);
        /// <summary>
        /// 删除LTC_PAYBILL
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeletePaybill(long id);
        #endregion

        #region LTC_FIXEDCOST
        /// <summary>
        /// 获取LTC_FIXEDCOST列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<FixedCost>> QueryFixedCost(BaseRequest<FixedCostFilter> request);
        /// <summary>
        /// 获取LTC_FIXEDCOST
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<FixedCost> GetFixedCost(int id);
        /// <summary>
        /// 保存LTC_FIXEDCOST
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<FixedCost> SaveFixedCost(FixedCost request);
        /// <summary>
        /// 保存费用套餐中的收费项目
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        BaseResponse<List<FixedCost>> SaveFixedCost(int groupId, int regNo, long feeNo, DateTime? StartDate, DateTime? EndDate);
        /// <summary>
        /// 删除LTC_FIXEDCOST
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteFixedCost(int id);
        #endregion

        #region LTC_COSTITEM
        /// <summary>
        /// 获取LTC_COSTITEM列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CostItem>> QueryCostItem(BaseRequest<CostItemFilter> request);
        /// <summary>
        /// 获取LTC_COSTITEM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CostItem> GetCostItem(int id);
        /// <summary>
        /// 保存LTC_COSTITEM
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CostItem> SaveCostItem(CostItem request);
        /// <summary>
        /// 删除LTC_COSTITEM
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteCostItem(int id);
        #endregion

        #region LTC_COSTGROUP
        /// <summary>
        /// 获取LTC_COSTGROUP列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CostGroup>> QueryCostGroup(BaseRequest<CostGroupFilter> request);
        /// <summary>
        /// 获取LTC_COSTGROUP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CostGroup> GetCostGroup(int id);
         /// <summary>
        /// 获取LTC_COSTGROUP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CostGroup> GetCostGroupExtend(int id);
        /// <summary>
        /// 保存LTC_COSTGROUP
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CostGroup> SaveCostGroup(CostGroup request);
        /// <summary>
        /// 删除LTC_COSTGROUP
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteCostGroup(int id);
        #endregion

        #region LTC_COSTGROUPDTL
        /// <summary>
        /// 获取LTC_COSTGROUPDTL列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CostGroupDtl>> QueryCostGroupDtl(BaseRequest<CostGroupDtlFilter> request);
        /// <summary>
        /// 获取LTC_COSTGROUPDTL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CostGroupDtl> GetCostGroupDtl(int id);
        /// <summary>
        /// 保存LTC_COSTGROUPDTL
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CostGroupDtl> SaveCostGroupDtl(CostGroupDtl request);
        /// <summary>
        /// 删除LTC_COSTGROUPDTL
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteCostGroupDtl(int id);

        /// <summary>
        /// 删除LTC_COSTGROUPDTL
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteCostGroupDtl(List<string> id);
        /// <summary>
        /// 删除LTC_COSTGROUPDTL根据GroupId
        /// </summary>
        /// <param name="id"></param>
        int DeleteCostGroupDtlByGroupId(int Groupid);
        #endregion

        #region LTC_COSTDTL
        /// <summary>
        /// 获取LTC_COSTDTL列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CostDtl>> QueryCostDtl(BaseRequest<CostDtlFilter> request);
        /// <summary>
        /// 获取LTC_COSTDTL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CostDtl> GetCostDtl(long id);
        /// <summary>
        /// 保存LTC_COSTDTL
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CostDtl> SaveCostDtl(CostDtl request);
        /// <summary>
        /// 删除LTC_COSTDTL
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteCostDtl(long id);
        #endregion

        #region LTC_PINMONEY
        /// <summary>
        /// 获取LTC_PINMONEY列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<PinMoney>> QueryPinMoney(BaseRequest<PinMoneyFilter> request);
        /// <summary>
        /// 获取LTC_PINMONEY
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<PinMoney> GetPinMoney(long id);
        /// <summary>
        /// 保存LTC_PINMONEY
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<PinMoney> SavePinMoney(PinMoney request);
        /// <summary>
        /// 删除LTC_PINMONEY
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeletePinMoney(long id);
        #endregion

        #region LTC_RECEIPTS
        /// <summary>
        /// 获取LTC_RECEIPTS列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Receipts>> QueryReceipts(BaseRequest<ReceiptsFilter> request);
        /// <summary>
        /// 获取LTC_RECEIPTS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Receipts> GetReceipts(long id);
        /// <summary>
        /// 保存LTC_RECEIPTS
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Receipts> SaveReceipts(Receipts request);
        /// <summary>
        /// 删除LTC_RECEIPTS
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteReceipts(long id);
        #endregion

        BaseResponse<IList<CostItemCostGroup>> QueryCostItemAndCostGroup(BaseRequest<CostItemCostGroupFilter> request);
    }
}
