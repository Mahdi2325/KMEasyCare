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
    public class NSDrugManageService : BaseService, INSDrugManageService
    {
        #region 机构药品数据

        /// <summary>
        /// 查询机构药品
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>药品数据信息</returns>
        public BaseResponse<IList<NSDrug>> Query(BaseRequest<NSDrugFilter> request)
        {
            var response = base.Query<LTC_NSDRUG, NSDrug>(request, (q) =>
            {
                if (request.Data.DrugId.HasValue)
                {
                    q = q.Where(m => m.DRUGID == request.Data.DrugId);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(request.Data.KeyWord))
                    {
                        q = q.Where(m => m.CNNAME.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                                         || m.ENNAME.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                                         || m.PINYIN.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                                         || m.MCDRUGCODE.ToUpper().Contains(request.Data.KeyWord.ToUpper())
                                         || m.NSDRUGCODE.ToUpper().Contains(request.Data.KeyWord.ToUpper()));
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

        public BaseResponse<NSDrug> Get(int id)
        {
            return base.Get<LTC_NSDRUG, NSDrug>((q) => q.DRUGID == id && q.ISDELETE == false);
        }

        public BaseResponse<NSDrug> GetByMCDrugCode(string mcDrugCode)
        {
            return base.Get<LTC_NSDRUG, NSDrug>((q) => q.MCDRUGCODE == mcDrugCode && q.ISDELETE == false && q.NSID == SecurityHelper.CurrentPrincipal.OrgId);
        }

        public BaseResponse<NSDrug> Save(NSDrug request)
        {
            if (request.DrugId == 0)
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
                request.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.UpdateTime = DateTime.Now;
            }
            //不管是不是护理险项目都默认是为护理险项目
            request.IsNCIItem = true;
            return base.Save<LTC_NSDRUG, NSDrug>(request, (q) => q.DRUGID == request.DrugId);
        }

        public BaseResponse Delete(int id)
        {
            return base.SoftDelete<LTC_NSDRUG>(id);
        }
        #endregion
    }
}
