using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class RegHosProof
    {
        //
        public string Org { get; set; }

        //
        public long FeeNo { get; set; }

        //
        public string ResidengNo { get; set; }
        //姓名
        public string Name { get; set; }

        //性别
        public string Sex { get; set; }

        //户籍地址
        public string PermanentAddress { get; set; }

        //身份证号
        public string IdNo { get; set; }

        //身心障碍等级
        public string DisabilityGrade { get; set; }

        //出生日期
        public Nullable<DateTime> BrithDay { get; set; }

        //入院日期
        public Nullable<DateTime> InDate { get; set; }
     
    }
}

