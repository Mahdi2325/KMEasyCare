using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model.MedicalWork
{
    public class OwnDrugRecModel
    {
        /// <summary>
        /// 自带药品序号
        /// </summary>
        public int OwnDrugId { get; set; }
        /// <summary>
        /// 住民编号
        /// </summary>
        public long FeeNo { get; set; }
        /// <summary>
        /// 机构编号
        /// </summary>
        public string OrgId { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 担保人
        /// </summary>
        public string SponsorName { get; set; }
        /// <summary>
        /// 收药人
        /// </summary>
        public string OpertorName { get; set; }
        /// <summary>
        /// 收药时间
        /// </summary>
        public System.DateTime OpertorTime { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
