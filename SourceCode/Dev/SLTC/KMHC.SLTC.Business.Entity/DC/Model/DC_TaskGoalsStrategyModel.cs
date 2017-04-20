using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity
{
    public class DC_TaskGoalsStrategyModel
    {
        public long EVALPLANID { get; set; }
        /// <summary>
        /// 需求类型
        /// </summary>
        public string QUESTIONTYPE { get; set; }
        public string CPDIA { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string QUESTIONDESC { get; set; }
        /// <summary>
        /// 处遇目标
        /// </summary>
        public string TREATMENTGOAL { get; set; }
        public string QUESTIONANALYSIS { get; set; }
        public string PLANACTIVITY { get; set; }//?计划活动
        public DateTime? RECDATE { get; set; }
        public string RECORDBY { get; set; }
        public string CHECKDATE { get; set; }
        public string CHECKEDBY { get; set; }
        public string EVALUATIONVALUE { get; set; }
        public long ID { get; set; }
        public int? MAJORTYPE { get; set; }
        public string MAJORNAME { get; set; }
        public long? FEENO { get; set; }
        public string UNFINISHREASON { get; set; }
        public DateTime? EVALDATE { get; set; }
        public int? EVALNUMBER { get; set; }
    }
}
