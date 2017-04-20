using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity;
using System.Reflection;
using System.ComponentModel;
using KMHC.SLTC.Persistence;


namespace KMHC.SLTC.Business.Implement
{
    public class PersonStatusReportService : BaseService, IPersonStatusReportService
    {
        public PersonStatusReportModel QueryPersonStatusInfo(PersonStatusFilter baseRequest)
        {
            var response = new PersonStatusReportModel();
            var xyyValue = GetEnumDescription(OrgValue.Xyy).ToString();
            var qkyyValue = GetEnumDescription(OrgValue.Qkyy).ToString();
            var jmyyValue = GetEnumDescription(OrgValue.Jmyy).ToString();
            var nDate = DateTime.Now;

            #region 长照信息统计
            #region 药品导入数
            response.DrugEntryNum = new StatisticalNum();
            response.DrugEntryNum.TotalNum = (from o in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet
                                              where o.CREATETIME.HasValue && o.CREATETIME >= baseRequest.startDate.Value && o.CREATETIME <= baseRequest.endDate.Value && o.ISDELETE != true && o.STATUS == 0
                                              select 0).Count();
            response.DrugEntryNum.XyyNum = (from o in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet
                                            where o.NSID == xyyValue && o.CREATETIME.HasValue && o.CREATETIME >= baseRequest.startDate.Value && o.CREATETIME <= baseRequest.endDate.Value && o.ISDELETE != true && o.STATUS == 0
                                            select 0).Count();
            response.DrugEntryNum.QkyyNum = (from o in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet
                                             where o.NSID == qkyyValue && o.CREATETIME.HasValue && o.CREATETIME >= baseRequest.startDate.Value && o.CREATETIME <= baseRequest.endDate.Value && o.ISDELETE != true && o.STATUS == 0
                                             select 0).Count();
            response.DrugEntryNum.JmyyNum = (from o in unitOfWork.GetRepository<LTC_NSDRUG>().dbSet
                                             where o.NSID == jmyyValue && o.CREATETIME.HasValue && o.CREATETIME >= baseRequest.startDate.Value && o.CREATETIME <= baseRequest.endDate.Value && o.ISDELETE != true && o.STATUS == 0
                                             select 0).Count();

            #endregion

            #region 护理计划
            response.NSCPLNum = new StatisticalNum();
            var q = from m in unitOfWork.GetRepository<LTC_NSCPL>().dbSet.Where(o => o.CREATEDATE.HasValue && o.CREATEDATE >= baseRequest.startDate.Value && o.CREATEDATE <= baseRequest.endDate.Value)
                    join n in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(o => o.STATUS == 0) on m.FEENO equals n.FEENO into nrd
                    from nr in nrd.DefaultIfEmpty()
                    select new RegNCIInfo
                    {
                        Feeno = m.FEENO.Value,
                        Status = nr.STATUS,
                        Createtime = m.CREATEDATE.Value,
                        NsId = m.ORGID
                    };
            var nscpl = q.Where(m => m.Status == 0 && m.Createtime.HasValue && m.Createtime >= baseRequest.startDate.Value && m.Createtime <= baseRequest.endDate.Value).ToList();
            response.NSCPLNum.TotalNum = nscpl.Count();
            response.NSCPLNum.XyyNum = nscpl.Count(m => m.NsId == xyyValue);
            response.NSCPLNum.QkyyNum = nscpl.Count(m => m.NsId == qkyyValue);
            response.NSCPLNum.JmyyNum = nscpl.Count(m => m.NsId == jmyyValue);
            #endregion

            #region 账单数量
            response.BillV2Num = new StatisticalNum();
            var b = from m in unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(o => o.ISDELETE != true && o.STATUS != (int)BillStatus.Refund && o.CREATETIME.HasValue && o.CREATETIME >= baseRequest.startDate.Value && o.CREATETIME <= baseRequest.endDate.Value)
                    select new BillV2
                    {
                        CreateTime = m.CREATETIME,
                        BillId = m.BILLID,
                        OrgId = m.ORGID,
                        NCIPaysCale = m.NCIPAYSCALE,
                    };
            var bill = b.Where(m => m.NCIPaysCale > 0 && m.CreateTime.HasValue && m.CreateTime >= baseRequest.startDate.Value && m.CreateTime <= baseRequest.endDate.Value).ToList();
            response.BillV2Num.TotalNum = bill.Count();
            response.BillV2Num.XyyNum = bill.Count(m => m.OrgId == xyyValue);
            response.BillV2Num.QkyyNum = bill.Count(m => m.OrgId == qkyyValue);
            response.BillV2Num.JmyyNum = bill.Count(m => m.OrgId == jmyyValue);
            #endregion

            #region 总费用
            response.CostNum = new StatisticalSum();
            var c = from m in unitOfWork.GetRepository<LTC_BILLV2>().dbSet.Where(o => o.ISDELETE != true && (o.STATUS == (int)BillStatus.Charge || o.STATUS == (int)BillStatus.Uploaded))
                    join bp in unitOfWork.GetRepository<LTC_BILLV2PAY>().dbSet on m.BILLPAYID equals bp.BILLPAYID into bpi
                    from bpinfo in bpi.DefaultIfEmpty()
                    select new BillV2PAY
                    {
                        BILLPAYID = m.BILLPAYID.Value,
                        SELFPAY = m.SELFPAY,
                        NCIITEMSELFPAY = m.NCIITEMSELFPAY,
                        PAYTIME = bpinfo.PAYTIME,
                        OrgId = m.ORGID,
                        RegScal = m.NCIPAYSCALE
                    };
            var cost = c.Where(m => m.PAYTIME.HasValue && m.PAYTIME >= baseRequest.startDate.Value && m.PAYTIME <= baseRequest.endDate.Value && m.RegScal > 0).ToList();
            response.CostNum.TotalNum = Convert.ToDouble(cost.Sum(m => m.SELFPAY) + cost.Sum(m => m.NCIITEMSELFPAY));
            response.CostNum.XyyNum = Convert.ToDouble(cost.Where(m => m.OrgId == xyyValue).Sum(m => m.SELFPAY) + cost.Where(m => m.OrgId == xyyValue).Sum(m => m.NCIITEMSELFPAY));
            response.CostNum.QkyyNum = Convert.ToDouble(cost.Where(m => m.OrgId == qkyyValue).Sum(m => m.SELFPAY) + cost.Where(m => m.OrgId == qkyyValue).Sum(m => m.NCIITEMSELFPAY));
            response.CostNum.JmyyNum = Convert.ToDouble(cost.Where(m => m.OrgId == jmyyValue).Sum(m => m.SELFPAY) + cost.Where(m => m.OrgId == jmyyValue).Sum(m => m.NCIITEMSELFPAY));
            #endregion
            #endregion

            #region 在院/不在院情况统计				
            #region 在院人数
            response.InHospNum = new StatisticalNum();
            var p = from m in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(o => o.IPDFLAG == "I")
                    join n in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(o => o.STATUS == 0) on m.FEENO equals n.FEENO into nrd
                    from nr in nrd.DefaultIfEmpty()
                    select new Person
                    {
                        IpdFlag = m.IPDFLAG,
                        FeeNo = m.FEENO,
                        OrgId = m.ORGID,
                        Status = nr.STATUS
                    };
            var per = p.Where(m => m.IpdFlag == "I" && m.Status == 0).ToList();
            response.InHospNum.TotalNum = per.Count();
            response.InHospNum.XyyNum = per.Count(m => m.OrgId == xyyValue);
            response.InHospNum.QkyyNum = per.Count(m => m.OrgId == qkyyValue);
            response.InHospNum.JmyyNum = per.Count(m => m.OrgId == jmyyValue);
            #endregion
            #region 出院人数

            var outhosp = from m in unitOfWork.GetRepository<LTC_IPDREGOUT>().dbSet.Where(o => o.CLOSEDATE.HasValue && o.CLOSEDATE <= nDate)
                          join n in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(o => o.STATUS == 1) on m.FEENO equals n.FEENO into nrd
                          from nr in nrd.DefaultIfEmpty()
                          select new Ipdregout
                          {
                              OrgId = m.ORGID,
                              CloseDate = m.CLOSEDATE,
                              CloseReason = m.CLOSEREASON,
                              Status = nr.STATUS,
                              FeeNo = m.FEENO
                          };

            #region 其他
            response.OutHospOfOtherNum = new StatisticalNum();
            var outhospList = outhosp.Where(m => m.CloseReason != "006" && m.Status == 1).ToList();

            response.OutHospOfOtherNum.TotalNum = outhospList.Distinct(new PersonNumComparer()).Count();
            response.OutHospOfOtherNum.XyyNum = outhospList.Distinct(new PersonNumComparer()).Count(m => m.OrgId == xyyValue);
            response.OutHospOfOtherNum.QkyyNum = outhospList.Distinct(new PersonNumComparer()).Count(m => m.OrgId == qkyyValue);
            response.OutHospOfOtherNum.JmyyNum = outhospList.Distinct(new PersonNumComparer()).Count(m => m.OrgId == jmyyValue);
            #endregion
            #region 死亡
            response.OutHospOfDeadNum = new StatisticalNum();
            var deadList = outhosp.Where(m => m.CloseReason == "006" && m.Status == 1).ToList();
            response.OutHospOfDeadNum.TotalNum = deadList.Distinct(new PersonNumComparer()).Count();
            response.OutHospOfDeadNum.XyyNum = deadList.Distinct(new PersonNumComparer()).Count(m => m.OrgId == xyyValue);
            response.OutHospOfDeadNum.QkyyNum = deadList.Distinct(new PersonNumComparer()).Count(m => m.OrgId == qkyyValue);
            response.OutHospOfDeadNum.JmyyNum = deadList.Distinct(new PersonNumComparer()).Count(m => m.OrgId == jmyyValue);
            #endregion
            #endregion

            #region 请假
            var leahosp = from m in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet.Where(o => o.RETURNDATE.HasValue && o.RETURNDATE >= nDate && o.STARTDATE.HasValue && o.STARTDATE <= nDate && o.ISDELETE != true)
                          join n in unitOfWork.GetRepository<LTC_REGNCIINFO>().dbSet.Where(o => o.STATUS == 0) on m.FEENO equals n.FEENO into nrd
                          from nr in nrd.DefaultIfEmpty()
                          select new LeaveHosp
                          {
                              OrgId = m.ORGID,
                              Status = nr.STATUS
                          };
            response.LeaveHospNum = new StatisticalNum();
            var leavehospList = leahosp.Where(m => m.Status == 0).ToList();
            response.LeaveHospNum.TotalNum = leavehospList.Count();
            response.LeaveHospNum.XyyNum = leavehospList.Count(m => m.OrgId == xyyValue);
            response.LeaveHospNum.QkyyNum = leavehospList.Count(m => m.OrgId == qkyyValue);
            response.LeaveHospNum.JmyyNum = leavehospList.Count(m => m.OrgId == jmyyValue);


            response.InHospNum.TotalNum -= response.LeaveHospNum.TotalNum;
            response.InHospNum.XyyNum -= response.LeaveHospNum.XyyNum;
            response.InHospNum.QkyyNum -= response.LeaveHospNum.QkyyNum;
            response.InHospNum.JmyyNum -= response.LeaveHospNum.JmyyNum;


            #endregion
            #endregion
            return response;
        }

        public static string GetEnumDescription(Enum enumSubitem)
        {
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
            Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return strValue;
            }
            else
            {
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                return da.Description;
            }

        }
    }

    public class PersonNumComparer : IEqualityComparer<Ipdregout>
    {
        public bool Equals(Ipdregout p1, Ipdregout p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.FeeNo == p2.FeeNo;
        }

        public int GetHashCode(Ipdregout p)
        {
            if (p == null)
                return 0;
            return p.FeeNo.GetHashCode();
        }
    }
}
