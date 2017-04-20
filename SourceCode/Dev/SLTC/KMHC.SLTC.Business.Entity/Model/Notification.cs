/*
 * 描述:Notification
 *  
 * 修订历史: 
 * 日期                    修改人              Email                  内容
 * 2/18/2016 11:57:43 AM   张正泉            15986707042@163.com     创建 首页消息提示对象 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Notification
    {
        public string Title { get;  set; }
        public string Content { get;  set; }
        public DateTime Time { get;  set; }

        public string TimeRange { get; set; }

        public string Url { get; set; }

        public string InfoLevel { get; set; }
    }
}