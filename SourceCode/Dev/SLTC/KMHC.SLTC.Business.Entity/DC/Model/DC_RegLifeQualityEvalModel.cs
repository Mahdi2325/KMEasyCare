using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    /// <summary>
    /// 家庭照顾者生活品质评估问卷
    /// </summary>
    public class DC_RegLifeQualityEvalModel
    {
        public int Id { get; set; }
        public long? FeeNo { get; set; }
        public string Health { get; set; }
        public string Energy { get; set; }
        public string Mood { get; set; }
        public string LivingCondition { get; set; }
        public string Memory { get; set; }
        public string Family { get; set; }
        public string Merry { get; set; }
        public string Friends { get; set; }
        public string Self { get; set; }
        public string FamilyAbility { get; set; }
        public string Ability { get; set; }
        public string EnterTainment { get; set; }
        public string Money { get; set; }
        public string WholeLife { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string RegNo { get; set; }
        public string OrgId { get; set; }
    }
}

