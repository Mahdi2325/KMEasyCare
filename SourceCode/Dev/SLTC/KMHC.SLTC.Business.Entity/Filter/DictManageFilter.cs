using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Filter
{
    public class CodeFilter
    {
        //字典类型
        public string ItemType { get; set; }
        //字典类型数组
        public string[] ItemTypes { get; set; }

    }

    public class CommonUseWordFilter
    {
        //类型名称
        public string TypeName { get; set; }
        //类型名称数组
        public string[] TypeNames { get; set; }
    }
}





