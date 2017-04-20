using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Base
{
    public class BaseResponse
    {
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public int RecordsCount { get; set; }
        public string ResultMessage { get; set; }
        public int ResultCode { get; set; }
        public long Id { get; set; }
        /// <summary>
        /// 返回Token信息
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 返回处理结果
        /// </summary>
        public bool IsSuccess { get; set; }
        public BaseResponse()
        {
            IsSuccess = true;
        }
    }

    public class BaseResponse<T> : BaseResponse 
    {
        public T Data { get; set; }

        public BaseResponse()
        {
            
        }

        public BaseResponse(T data)
        {
            this.Data = data;
        }
    }
}
