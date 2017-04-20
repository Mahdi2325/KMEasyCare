using AutoMapper;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using KMHC.Infrastructure;

namespace KMHC.SLTC.Business.Implement
{
    public class GoodsManageService : BaseService, IGoodsManageService
    {
        #region 物品
        public BaseResponse<IList<Goods>> QueryGoods(BaseRequest<GoodsFilter> request)
        {
            var response = base.Query<LTC_GOODS, Goods>(request, (q) =>
            {
                q = q.Where(o => o.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
                if (!string.IsNullOrEmpty(request.Data.Name))
                {
                    q = q.Where(m => m.Name.Contains(request.Data.Name));
                }
                if (!string.IsNullOrEmpty(request.Data.Type))
                {
                    q = q.Where(m => m.Type==request.Data.Type);
                }
                if (!string.IsNullOrEmpty(request.Data.No))
                {
                    q = q.Where(m => m.No.Contains(request.Data.No));
                }
                q = q.OrderByDescending(m => m.Id);
                return q;
            });
            return response;
        }

        public BaseResponse<Goods> GetGoods(int id)
        {
            return base.Get<LTC_GOODS, Goods>((q) => q.Id == id);
        }

        public BaseResponse<Goods> SaveGoods(Goods request)
        {
            if (string.IsNullOrEmpty(request.No))
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.No = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.GoodNo);
            }
            return base.Save<LTC_GOODS, Goods>(request, (q) => q.Id == request.Id);
        }

        public BaseResponse DeleteGoods(int id)
        {
            return base.Delete<LTC_GOODS>(id);
        }

        public BaseResponse<IList<Manufacture>> QueryManufacture()
        {
            var response = base.Query<LTC_MANUFACTURE, Manufacture>(null, (q) =>
            {
                q = q.Where(o => o.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
                q = q.OrderByDescending(m => m.Id);
                return q;
            });
            return response;
        }

        #endregion


        #region 厂商
        public BaseResponse<IList<Manufacture>> QueryManufacture(BaseRequest<CommonFilter> request)
        {
            var response = base.Query<LTC_MANUFACTURE, Manufacture>(request, (q) =>
            {
                q = q.Where(o => o.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
                if (!string.IsNullOrEmpty(request.Data.Keywords))
                {
                    q = q.Where(m => m.Name.Contains(request.Data.Keywords) || m.Description.Contains(request.Data.Keywords) || m.No.Contains(request.Data.Keywords));
                }
                q = q.OrderByDescending(m => m.Id);
                return q;
            });
            return response;
        }

        public BaseResponse<Manufacture> GetManufacture(int id)
        {
            return base.Get<LTC_MANUFACTURE, Manufacture>((q) => q.Id == id);
        }

        public BaseResponse<Manufacture> SaveManufacture(Manufacture request)
        {
            if (string.IsNullOrEmpty(request.No))
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.No = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.ManufactureNo);
            }
            return base.Save<LTC_MANUFACTURE, Manufacture>(request, (q) => q.Id == request.Id);
        }

        public BaseResponse DeleteManufacture(int id)
        {
            return base.Delete<LTC_MANUFACTURE>(id);
        }

        public BaseResponse<IList<GoodsLoan>> QueryGoodsLoan(BaseRequest<GoodsRecordFilter> request)
        {
            var response = base.Query<LTC_GOODSLOAN, GoodsLoan>(request, (q) =>
            {
                q = q.Where(o => o.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
                q = q.Where(m => m.GoodsId==request.Data.GoodsId);
                if (request.Data.StartDate.HasValue && request.Data.EndDate.HasValue)
                {
                    var endDate = ((DateTime) request.Data.EndDate).AddDays(1);
                    q = q.Where(m => m.LoanDate >= request.Data.StartDate && m.LoanDate < endDate);
                }
                else if (request.Data.StartDate.HasValue)
                {
                    q = q.Where(m => m.LoanDate >= request.Data.StartDate);
                }
                else if (request.Data.EndDate.HasValue)
                {
                    var endDate = ((DateTime)request.Data.EndDate).AddDays(1);
                    q = q.Where(m => m.LoanDate <= endDate);
                }
                q = q.OrderByDescending(m => m.Id);
                return q;
            });
            return response;
        }

        public BaseResponse<GoodsLoan> GetGoodsLoan(int id)
        {
            return base.Get<LTC_GOODSLOAN, GoodsLoan>((q) => q.Id == id);
        }

        public BaseResponse<GoodsLoan> SaveGoodsLoan(GoodsLoan request)
        {
            if (string.IsNullOrEmpty(request.No))
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.No = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.GoodsLoanNo);
            }

            return base.Save<LTC_GOODSLOAN, GoodsLoan>(request, (q) => q.Id == request.Id);
        }

        public BaseResponse DeleteGoodsLoan(int id)
        {
            return base.Delete<LTC_GOODSLOAN>(id);
        }

        public BaseResponse<IList<GoodsSale>> QueryGoodsSale(BaseRequest<GoodsRecordFilter> request)
        {
            var response = base.Query<LTC_GOODSSALE, GoodsSale>(request, (q) =>
            {
                q = q.Where(o => o.OrgId == SecurityHelper.CurrentPrincipal.OrgId);
                q = q.Where(m => m.GoodsId== request.Data.GoodsId);
                if (request.Data.StartDate.HasValue && request.Data.EndDate.HasValue)
                {
                    var endDate = ((DateTime)request.Data.EndDate).AddDays(1);
                    q = q.Where(m => m.SaleTime >= request.Data.StartDate && m.SaleTime < endDate);
                }
                else if (request.Data.StartDate.HasValue)
                {
                    q = q.Where(m => m.SaleTime >= request.Data.StartDate);
                }
                else if (request.Data.EndDate.HasValue)
                {
                    var endDate = ((DateTime)request.Data.EndDate).AddDays(1);
                    q = q.Where(m => m.SaleTime <endDate);
                }
                q = q.OrderByDescending(m => m.Id);
                return q;
            });
            return response;
        }

        public BaseResponse<GoodsSale> GetGoodsSale(int id)
        {
            return base.Get<LTC_GOODSSALE, GoodsSale>((q) => q.Id == id);
        }

        public BaseResponse<GoodsSale> SaveGoodsSale(GoodsSale request)
        {
            BaseResponse<GoodsSale> response = new BaseResponse<GoodsSale>();
            response.ResultCode = 0;

            var totalNum = unitOfWork.GetRepository<LTC_GOODS>().dbSet.Where(o => o.Id == request.GoodsId).ToList().FirstOrDefault().InventoryQuantity;

            if (string.IsNullOrEmpty(request.No))
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                request.No = base.GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.GoodsSaleNo);
            }

            if (request.Id == 0)
            {
               
                if (Convert.ToInt32(totalNum) < request.Amount)
                {
                    response.ResultCode = 1001;
                    return response;
                }
            }
            else
            {
                var goodNum = unitOfWork.GetRepository<LTC_GOODSSALE>().dbSet.Where(o => o.Id == request.Id).ToList().FirstOrDefault().Amount;
                var  oldGoodNum = Convert.ToInt32(goodNum) + Convert.ToInt32(totalNum);
                if (oldGoodNum < request.Amount)
                {
                    response.ResultCode = 1001;
                    return response;
                }
            }
            return base.Save<LTC_GOODSSALE, GoodsSale>(request, (q) => q.Id == request.Id);
        }

        public BaseResponse DeleteGoodsSale(int id)
        {
            return base.Delete<LTC_GOODSSALE>(id);
        }

        #endregion
    }
}