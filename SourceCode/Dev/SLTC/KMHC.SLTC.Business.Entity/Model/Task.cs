/*
 * 描述:Task
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 2/18/2016 12:16:04 PM   Admin            15986707042@163.com    创建 首页任务消息提示
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Task
    {
        public string Name { get; set; }
        public int Progress { get; set; }

        public string TaskID { get; set; }
    }
}