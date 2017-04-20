using KMHC.Infrastructure;
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
    public class NSServiceManageService : BaseService, INSServiceManageService
    {
        public BaseResponse<IList<NSService>> Query(BaseRequest<NSServiceFilter> request)
        {
            var response = base.Query<LTC_NSSERVICE, NSService>(request, (q) =>
            {
                if (request.Data.ServiceId.HasValue)
                {
                    q = q.Where(m => m.SERVICEID == request.Data.ServiceId);
                }
                else 
                {
                    if (!string.IsNullOrWhiteSpace(request.Data.KeyWord))
                    {
                        q = q.Where(m => m.SERVICENAME.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                            || m.MCSERVICECODE.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                            || m.NSSERVICECODE.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                            || m.PINYIN.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                            );
                    }
                }
                q = q.Where(m => m.ISDELETE == false);
                q = q.Where(m => m.NSID == SecurityHelper.CurrentPrincipal.OrgId);
                if (request.Data.Status != null)
                {
                    q = q.Where(m => m.STATUS == request.Data.Status);
                }
                q = q.OrderByDescending(m => m.CREATETIME);
                return q;
            });
            return response;
        }

        public BaseResponse<NSService> Get(int id)
        {
            return base.Get<LTC_NSSERVICE, NSService>((q) => q.SERVICEID == id && q.ISDELETE == false);
        }
        public BaseResponse<NSService> GetByMCServiceCode(string mcServiceCode)
        {
            return base.Get<LTC_NSSERVICE, NSService>((q) => q.MCSERVICECODE == mcServiceCode && q.ISDELETE == false && q.NSID == SecurityHelper.CurrentPrincipal.OrgId);
        }

        public BaseResponse<NSService> Save(NSService request)
        {
            if (request.ServiceId == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
                request.CreateTime = DateTime.Now;
                request.IsDelete = false;
                request.NSId = SecurityHelper.CurrentPrincipal.OrgId;
                request.IsRequireUpdate = false;
                request.LastUpdateTime = DateTime.Now;
                request.CreateTime = DateTime.Now;
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                //TODO 医保规则码暂用0, 待有确定规则后更新赋值逻辑
                request.MCRuleId = "0";
            }
            else
            {
                request.LastUpdateTime = DateTime.Now;
                request.UpdateBy = SecurityHelper.CurrentPrincipal.UserId.ToString();
                request.UpdateTime = DateTime.Now;
            }
            //不管是不是护理险项目都默认是为护理险项目
            request.IsNCIItem = true;
            return base.Save<LTC_NSSERVICE, NSService>(request, (q) => q.SERVICEID == request.ServiceId);
        }


        public BaseResponse Delete(int id)
        {
            return base.SoftDelete<LTC_NSSERVICE>(id);
        }
    }
}
