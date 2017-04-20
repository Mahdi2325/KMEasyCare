using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Contact
    {
        /// <summary>
        /// 户籍邮政区号
        /// </summary>
        public string CensusPostcode { get; set; }
        /// <summary>
        /// 户籍县/市
        /// </summary>
        public string CensusCity { get; set; }
        /// <summary>
        /// 户籍乡镇区
        /// </summary>
        public string CensusRegion { get; set; }
        /// <summary>
        /// 户籍街道巷弄
        /// </summary>
        public string CensusStreet { get; set; }
        /// <summary>
        /// 是否户籍在院
        /// </summary>
        public bool IsCensusOfBeadhouse { get; set; }
        /// <summary>
        /// 通迅邮政区号
        /// </summary>
        public int HabitationPostcode { get; set; }
        /// <summary>
        /// 通迅县/市
        /// </summary>
        public string HabitationCity { get; set; }
        /// <summary>
        /// 通迅乡镇区
        /// </summary>
        public string HabitationRegion { get; set; }
        /// <summary>
        /// 通迅街道巷弄
        /// </summary>
        public string HabitationStreet { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 传真电话
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 主要交费者
        /// </summary>
        public string Payer { get; set; }
        /// <summary>
        /// 账单地址
        /// </summary>
        public string BillAddress { get; set; }
    }
}