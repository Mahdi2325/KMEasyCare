using AutoMapper;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC
{
    public class CommfileService : BaseService, ICommfileService
    {
        public BaseResponse<IList<DC_CommUseWord>> QueryCommonFile(CommonUseWordFilter request)
        {
            Mapper.CreateMap<DC_COMMDTL, DC_CommUseWord>();
            var response = new BaseResponse<IList<DC_CommUseWord>>();
            var q = unitOfWork.GetRepository<DC_COMMDTL>().dbSet.Select(m => m);
            if (!string.IsNullOrEmpty(request.TypeName))
            {
                q = q.Where(o => o.ITEMTYPE.Trim() == request.TypeName);
            }
            else if (request.TypeNames != null)
            {
                q = q.Where(o => request.TypeNames.Contains(o.ITEMTYPE.Trim()));
            }
            var queryList = q.ToList();
            queryList.ForEach(m =>
            {
                m.ITEMNAME = m.ITEMNAME.Trim();
                m.ITEMTYPE = m.ITEMTYPE.Trim();
            });
            response.Data = Mapper.Map<IList<DC_CommUseWord>>(queryList);
            return response;
        }
    }
}
