using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using System.Runtime.Serialization;

namespace KMHC.SLTC.Business.Entity.Model
{
	//编码生成规则
    public partial class CodeRule
    {
        /// <summary>
        /// 编码键
        /// </summary>		
        public string CodeKey { get; set; }
        /// <summary>
        /// 重置编码标识规则
        /// </summary>		
        public string FlagRule { get; set; }
        /// <summary>
        /// 生成规则
        /// </summary>		
        public string GenerateRule { get; set; }
        /// <summary>
        /// 重置编码标识
        /// </summary>		
        public string Flag { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>		
        public decimal SerialNumber { get; set; }
        /// <summary>
        /// 组织结构ID
        /// </summary>
        public string OrgId { get; set; }
    }
}


