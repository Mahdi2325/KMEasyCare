using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IFstRegRecService : IBaseService
    {
        /// <summary>
        /// 查询首次入住记录
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<FstRegRec> GetFstRegRec(int regNo);

        /// <summary>
        /// 保存首次入住记录
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<FstRegRec> SaveFstRegRec(FstRegRec request);
    }
}
