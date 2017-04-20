/*
 * 描述:User
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 2/18/2016 12:02:29 PM   Admin            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class CodeValue
    {
        //public int Id { get; set; }
        //public string Value { get; set; }
        //public string Name { get; set; }
        //public string Parent { get; set; }
        //public string  Description { get; set; }


        public string ItemCode { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Nullable<int>  Orderseq { get; set; }
        public Nullable<System.DateTime>  UpdateDate { get; set; }
        public string  UpdateBy { get; set; }
    }

    public class IdCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
}