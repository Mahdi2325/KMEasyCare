using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Ipdregout
    {
        //
        public long FeeNo { get; set; }
        //入院日期
        public Nullable<System.DateTime> InDate { get; set; }
        //出院日期
        public Nullable<System.DateTime> OutDate { get; set; }
        //是否结案
        public bool CloseFlag { get; set; }
        //结案原因
        public string CloseReason { get; set; }
        //是否亡故
        public bool DeadFlag { get; set; }
        //亡故原因
        public string DeadReason { get; set; }
        //亡故日期
        public Nullable<System.DateTime> DeadDate { get; set; }
        //结案日期
        public Nullable<System.DateTime> CloseDate { get; set; }
        //入住天数
        public int TotalDay { get; set; }
        //创建日期
        public Nullable<System.DateTime> CreateDate { get; set; }
        //创建人
        public string CreateBy { get; set; }
        //
        public string OrgId { get; set; }
        public int Status { get; set; }
    }
}

