using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
    public class Medicine
    {
        #region 基本属性
        //
        public int Medid { get; set; }
        //
        public string MedCode { get; set; }
        //中文名称
        public string ChnName { get; set; }
        //英文学名
        public string EngName { get; set; }
        //副作用
        public string Sideeffect { get; set; }
        //剂型
        public string MedKind { get; set; }
        //药品色相
        public string Medicolor { get; set; }
        //外观
        public string Medifacade { get; set; }
        //规格
        public string SpecDesc { get; set; }
        //用途说明
        public string UseDesc { get; set; }
        //
        public string Commcode { get; set; }
        //药理分类
        public string MedType { get; set; }
        //药品图像
        public string MedPict { get; set; }
        //健保药码
        public string InsNo { get; set; }
        //开药医院
        public string HospNo { get; set; }
        //
        public string OrgId { get; set; }
        #endregion
    }
}

