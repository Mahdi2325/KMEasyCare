using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class ResidentMonFeeModel
    {
        /// <summary>
        /// 参保人月费用汇总ID
        /// </summary>
        public long Rsmonfeeid { get; set; }
        /// <summary>
        /// 定点机构月费用ID
        /// </summary>
        public Nullable<long> Nsmonfeeid { get; set; }
        /// <summary>
        /// 定点服务机构ID
        /// </summary>
        public string Nsid { get; set; }
        /// <summary>
        /// 定点服务机构名称
        /// </summary>
        public string NsName { get; set; }
        /// <summary>
        /// 参保人身份证号
        /// </summary>
        public string Residentssid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 资格证书号
        /// </summary>
        public string Certno { get; set; }
        /// <summary>
        /// 住院时间
        /// </summary>
        public System.DateTime Hospentrydate { get; set; }
        /// <summary>
        /// 出院时间
        /// </summary>
        public System.DateTime Hospdischargedate { get; set; }
        /// <summary>
        /// 住院天数
        /// </summary>
        public int Hospday { get; set; }
        /// <summary>
        /// 报销标准
        /// </summary>
        public decimal Ncipaylevel { get; set; }
        /// <summary>
        /// 报销比例
        /// </summary>
        public decimal Ncipayscale { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal Totalamount { get; set; }
        /// <summary>
        /// 护理险可报销费用
        /// </summary>
        public decimal Ncipay { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        public string Createby { get; set; }
        public Nullable<System.DateTime> Createtime { get; set; }
        public string Updateby { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }
        public Nullable<bool> Isdelete { get; set; }
    }
}
