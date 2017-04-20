using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Module
    {
        //public string RoleId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Url { get; set; }
        public string SuperModuleId { get; set; }
        public string Target { get; set; }
        public int? RootOrder { get; set; }
        //菜单图标
        public string Icon { get; set; }

    }
}





