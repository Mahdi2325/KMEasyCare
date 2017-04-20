/*
 * 描述:BedSoreChgrec
 *  
 * 修订历史: 
 * 日期       修改人              Email                  内容
 * 3/26/2016 10:09:24 AM    张正泉            15986707042@163.com    创建 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class BedSoreChgrec
    {
        public long Id { get; set; }
        public Nullable<long> Seq { get; set; }
        public Nullable<DateTime> EcalDate { get; set; }
        public string WoundPart { get; set; }
        public string Degree { get; set; }
        public string Size_L { get; set; }
        public string Size_W { get; set; }
        public string Size_D { get; set; }
        public string WoundDirection { get; set; }
        public string WoundDepth { get; set; }
        public string WoundColor { get; set; }
        public string SkinDesc { get; set; }
        public string SecretionColor { get; set; }
        public string SecretionNature { get; set; }
        public string SecretionAmt { get; set; }
        public string Nurse { get; set; }
        public string Dressing { get; set; }
        public string Treatement { get; set; }
        public string Picture { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }

        /// <summary>
        /// 护理人员姓名
        /// </summary>
        public string  NurseName { get; set; }
    }
}

