/*
 * 描述:Organization
 *  
 * 修订历史: 
 * 日期         修改人         Email                     内容
 * 2/20/2016    zhangkai      Azhang@kmhealthcloud.com   创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Organization
    {
        //机构ID
        public string OrgId { get; set; }
        //机构代码
        public string GroupId { get; set; }
        //机构名称
        public string OrgName { get; set; }
        //机构类型
        public string OrgType { get; set; }
        //负责人
        public string Responsible { get; set; }
        //电话
        public string Tel { get; set; }
        //传真
        public string Fax { get; set; }
        //床位数
        public int BedCount { get; set; }
        //电子邮件
        public string Email { get; set; }
        //网站
        public string Website { get; set; }
        //状态
        public string Status { get; set; }
        //Admin RoleId
        public string RoleId { get; set; }

        public List<TreeNode> CheckModuleList { get; set; }
    }
}





