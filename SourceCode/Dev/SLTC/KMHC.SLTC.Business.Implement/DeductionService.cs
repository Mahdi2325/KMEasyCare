using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class DeductionService : BaseService, IDeductionService
    {
        /// <summary>
        /// 查询扣款记录数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<NCIDeductionModel>> QueryDeductionList(BaseRequest<DeductionFilter> request)
        {
            BaseResponse<IList<NCIDeductionModel>> response = new BaseResponse<IList<NCIDeductionModel>>();
            var q = (from a in unitOfWork.GetRepository<LTC_NCIDEDUCTION>().dbSet
                                 join b in unitOfWork.GetRepository<LTC_ORG>().dbSet on a.ORGID equals b.ORGID
                                 select new NCIDeductionModel()
                                 {
                                     ID = a.ID,
                                     BillID = a.BILLID,
                                     DeductionType = a.DEDUCTIONTYPE,
                                     Debitmonth = a.DEBITMONTH,
                                     Leaveid = a.LEAVEID,
                                     Debitdays = a.DEBITDAYS,
                                     Amount = a.AMOUNT,
                                     DeductionReason = a.DEDUCTIONREASON,
                                     DeductionStatus = a.DEDUCTIONSTATUS,
                                     CreateBy = a.CREATEBY,
                                     CreatTime = a.CREATTIME,
                                     Updateby = a.UPDATEBY,
                                     UpdateTime = a.UPDATETIME,
                                     Orgid = a.ORGID,
                                     IsDelete = a.ISDELETE,
                                     OrgName = b.ORGNAME
                                 }).ToList();
            q = q.Where(m => m.IsDelete != true && m.DeductionType == (int)DeductionType.NCIOpr).ToList();
            if (request.Data.NsNo != "-1")
            {
                q = q.Where(m => m.Orgid == request.Data.NsNo).ToList();
            }
            if (!string.IsNullOrEmpty(request.Data.StartTime))
            {
                q = q.Where(m => m.Debitmonth.CompareTo(request.Data.StartTime) >= 0).ToList();
            }
            if (!string.IsNullOrEmpty(request.Data.EndTime))
            {
                q = q.Where(m => m.Debitmonth.CompareTo(request.Data.EndTime) <= 0).ToList();
            }
            q = q.OrderBy(m => m.DeductionStatus).ThenByDescending(m => m.CreatTime).ToList();

            response.RecordsCount = q.Count;
            List<NCIDeductionModel> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }
            response.Data = list;
            return response;
        }

        public BaseResponse SaveDeduction(NCIDeductionModel request)
        {
            return base.Save<LTC_NCIDEDUCTION, NCIDeductionModel>(request, (q) => q.ID == request.ID);
        }
        public object GetDeductionList(BaseRequest request,string month)
        {
            var response = base.Query<LTC_NCIDEDUCTION, NCIDeductionModel>(request, (q) =>
            {
                q = q.Where(m => string.Compare(m.DEBITMONTH, month) <= 0 && m.DEDUCTIONSTATUS == 0 && m.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
                q = q.OrderByDescending(o => o.DEBITMONTH);
                return q;
            });
            return response;
        }
    }
}
