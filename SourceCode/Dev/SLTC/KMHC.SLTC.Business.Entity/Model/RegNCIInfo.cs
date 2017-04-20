using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Entity.Model
{
   public class RegNCIInfo
    {
        public int ID { get; set; }
        public long Feeno { get; set; }
        public string Certno { get; set; }
        public System.DateTime CertStartTime { get; set; }
        public System.DateTime CertexpiredTime { get; set; }
        public string Caretypeid { get; set; }
        public string CaretypeName { get; set; }
        public decimal NCIPaylevel { get; set; }
        public decimal NCIPayscale { get; set; }
        public int Status { get; set; }
        public string Createby { get; set; }
        public Nullable<System.DateTime> Createtime { get; set; }

        public Nullable<System.DateTime> ApplyHosTime { get; set; }
        public string Updateby { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }

        public string RegName { get; set; }
        public string IdNo { get; set; }
        public string SsNo { get; set; }
        public bool IsHaveNCI { get; set; }
        public string NsId { get; set; }
        public string Sex { get; set; }
        public string Diseasediag { get; set; }

        #region 扩展属性
        public DateTime? ipdoutTime { get; set; }
        #endregion

    }

   public class CareInsInfo
   {
       public long FeeNo { get; set; }

       public RegNCIInfo regNCIInfo { get; set; }
       public AppcertLTCEntity appCertInfo { get; set; }
   }


   //资格申请表
   public class AppcertEntity
   {
       //资格申请ID
       public int AppcertId { get; set; }
       //上次的申请ID(复审时填)
       public int? BaseAppcertIdOfReApp { get; set; }
       //经办机构评估记录号
       public int? AgencyAsstRecordId { get; set; }
       //定点机构评估记录号
       public int? NsAsstRecordId { get; set; }
       //机构代码
       public string NsId { get; set; }
       //资格申请流水号
       public string AppcertSN { get; set; }
       //姓名
       public string Name { get; set; }
       //性别
       public string Gender { get; set; }
       //年龄
       public int Age { get; set; }
       //身份证号
       public string IdNo { get; set; }
       //社会保障号
       public string SsNo { get; set; }
       //定点服务机构申请护理的形式
       public int NsappcareType { get; set; }
       //人员身份(医保类型)
       public int McType { get; set; }
       //病种
       public string Disease { get; set; }
       /// <summary>
       /// 病种详情
       /// </summary>
       public string DiseaseTxt { get; set; }
       //现住址
       public string Address { get; set; }
       //联系电话
       public string Phone { get; set; }
       //经办机构批准的护理形式
       public int? AgencyapprovedcareType { get; set; }
       //申请原因
       public string AppReason { get; set; }
       //审批状态: 
       //0：未提交   （数据保存，但尚未提交）
       //1：已撤回   （提交后，下一级机构尚未审批，撤回）
       //3: 待审核    （已提交，待上一级机构审核）
       //6：审核通过 
       //9：审核不通过
       //11：重新审核
       public int Status { get; set; }
       //相关材料附件
       public string UploadFiles { get; set; }
       //定点服务机构意见
       public string NsComment { get; set; }
       //定点服务机构意见时间
       public DateTime? NsOperateTime { get; set; }
       //定点服务机构人员姓名
       public string NsUserId { get; set; }
       //承办保险机构是否通过
       public int? IcResult { get; set; }
       //承办保险机构意见
       public string IcComment { get; set; }
       //承办机构操作时间
       public DateTime? IcOperateTime { get; set; }
       //承办机构人员姓名
       public string IcStaffName { get; set; }
       //经办机构是否通过
       public int? AgencyResult { get; set; }
       //经办机构意见
       public string AgencyComment { get; set; }
       //经办机构操作时间
       public DateTime? AgencyOperateTime { get; set; }
       //经办机构人员姓名
       public string AgencyUserId { get; set; }
       public string AgencyId { get; set; }
       //资格证书号
       public string CertNo { get; set; }
       //资格证书生效时间
       public DateTime? CertStartTime { get; set; }
       //资格证书失效时间
       public DateTime? CertExpiredTime { get; set; }
       //创建人
       public string CreateBy { get; set; }
       //创建时间
       public DateTime? CreateTime { get; set; }
       //更新人
       public string UpdateBy { get; set; }
       //更新时间
       public DateTime? UpdateTime { get; set; }
       //是否删除
       public bool? IsDelete { get; set; }

       #region 扩展属性
       //ADL评估总分
       public int TotalScore { get; set; }
       //-1：删除
       //0：新增保存 编辑保存
       //1：撤销
       //3：提交保存
       //6：审核通过保存
       //9：审核不通过保存
       //11：重新审核保存
       public int ActionStatus { get; set; }
       #endregion
   }

   public class AppcertLTCEntity : AppcertEntity
   {
       /// <summary>
       /// 护理类型名称
       /// </summary>
       public string CaretypeName { get; set; }
       /// <summary>
       /// 筹区统内报销标准
       /// </summary>
       public decimal NCIPayLevel { get; set; }
       /// <summary>
       /// 筹区统内报销比例
       /// </summary>
       public decimal NCIPayScale { get; set; }

       public DateTime InHospDate { get; set; }

       public string Residence { get; set; }

   }
}
