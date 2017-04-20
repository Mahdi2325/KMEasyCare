using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.TSG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface ITsgService : IBaseService
    {
        BaseResponse<IList<TsgCategory>> QueryTsgData(BaseRequest<TsgCategoryFilter> request);
        BaseResponse<IList<TsgCategory>> QueryTsgCategory(BaseRequest<TsgCategoryFilter> request);
        BaseResponse<IList<TsgQuestion>> QueryTsgQuestion(BaseRequest<TsgQuestionFilter> request);
        BaseResponse<TsgQuestionData> GetTsgQuestion(int id);
        BaseResponse<TsgQuestionData> SaveTsgQuestion(TsgQuestionData request);
        BaseResponse DeleteTsgQuestion(int id);
    }
}
