using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class GlobalVariable
    {
        public string Organization { get; set; }
        public string OrganizationId { get; set; }
        public string[] Roles { get; set; }
        public string MaximumPrivileges { get; set; } //权限最大角色或第一个
        public string CurrentLoginSys { get; set; }
    }
}
