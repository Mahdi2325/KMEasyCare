/*
 * 描述:Email
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 2/18/2016 12:16:42 PM   Admin            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Message
    {
        public string FromName { get; set; }
        public string Content { get; set; }
        public string TimeRange { get; set; }
        public string FromUrl { get; set; }
        public string MsgId { get; set; }
    }
}