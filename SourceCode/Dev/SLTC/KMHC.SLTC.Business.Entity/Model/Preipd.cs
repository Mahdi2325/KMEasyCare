using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Preipd
    {
        //
        public long PreFeeNo { get; set; }
        //资料状态 P预约 D取消 A已办入院
        public string RecStatus { get; set; }
        //来源别 1.门诊 2.病房转诊 3.他院转介4. 自由登记 5.其他
        public string SourceType { get; set; }
        //特殊身份   
        public string InsMark { get; set; }
        //预约日期
        public Nullable<System.DateTime> PreDate { get; set; }
        //部门编号
        public string DeptNo { get; set; }
        //床号
        public string BedNo { get; set; }
        //备注
        public string CommDesc { get; set; }
        //是否要
        public bool WaitFlag { get; set; }
        //联系人姓名
        public string ContactName { get; set; }
        //联系方式
        public string ContactTel { get; set; }
        //取消原因
        public string CancelReason { get; set; }
        //病人姓名
        public string PName { get; set; }
        //病人电话
        public string Phone { get; set; }
        //性别
        public string Sex { get; set; }
        //年龄
        public int Age { get; set; }
        //身份证号
        public string IdNo { get; set; }
        //
        public Nullable<System.DateTime> UpdateDate { get; set; }
        //
        public string UpdateBy { get; set; }
        //
        public string OrgId { get; set; }


        #region 扩展属性
        public string DeptName { get; set; }
        #endregion
    }
}

