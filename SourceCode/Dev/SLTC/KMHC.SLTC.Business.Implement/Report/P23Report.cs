using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Word;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository.Base;
using NPOI.SS.Util;

namespace KMHC.SLTC.Business.Implement.Report
{
    public class P23Report:BaseReport
    {

        protected override void Operation(WordDocument doc)
        {
            doc.ReplaceText("Org", GetOrgName(SecurityHelper.CurrentPrincipal.OrgId));
            DateTime now = StartDate;
            doc.ReplaceText("year", now.Year.ToString());
            doc.ReplaceText("month", now.Month.ToString());
            IReportManageService reportManageService = IOCContainer.Instance.Resolve<IReportManageService>();
            decimal total = reportManageService.GetResidentTotal(now);
            decimal ipdRegInTotal = total * DateTime.DaysInMonth(now.Year, now.Month); //当月在院住民总人日数

            decimal ipdRegOutTotal = OutLeaveUnPlanTotal(now.Year, now.Month, reportManageService.GetIpdOutTotal(now.Year));    //当月结案的人日次数
            decimal leaveHospTotal = OutLeaveUnPlanTotal(now.Year, now.Month, reportManageService.GetLeaveHospTotal(now.Year)); // 当月请假的人日次数
            decimal unPlanIpdTotal = OutLeaveUnPlanTotal(now.Year, now.Month, reportManageService.GetUnPlanIpdTotal(now.Year));      //当月非计划住院人日次数
            decimal ipdRegCount = ipdRegInTotal + ipdRegOutTotal - leaveHospTotal - unPlanIpdTotal;   //当月住民总人日数

            if (ipdRegCount == 0)
            {
                doc.ReplaceText("UTotal", "0");
                doc.ReplaceText("STotal", "0");
                doc.ReplaceText("CTotal", "0");
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
                doc.ReplaceText("N0", "0");
                doc.ReplaceText("N1", "0");
                doc.ReplaceText("N2", "0");
                doc.ReplaceText("N3", "0");
                doc.ReplaceText("N4", "0");
                doc.ReplaceText("N5", "0");
                doc.ReplaceText("R0", "0");
                doc.ReplaceText("R1", "0");
                doc.ReplaceText("R2", "0");
                doc.ReplaceText("R3", "0");
                doc.ReplaceText("R4", "0");
                doc.ReplaceText("R5", "0");
                doc.ReplaceText("CompareNum", "");
                doc.ReplaceText("ReasonInfo", "");
                doc.ReplaceText("CompareState", "");
                return;
            }
            doc.ReplaceText("CTotal", ipdRegCount.ToString("#0"));

            var list = reportManageService.GetInfection(now);
            var lastList = reportManageService.GetInfection(now.AddMonths(-1));
            decimal nTotal = list.Sum(o => o.Total);
            decimal lastnTotal = lastList.Sum(o => o.Total);
            if (nTotal == 0)
            {
                doc.ReplaceText("NTotal", "0");
                doc.ReplaceText("RTotal", "0");
            }
            else
            {
                doc.ReplaceText("NTotal", nTotal.ToString("#0"));
                doc.ReplaceText("RTotal", (nTotal / ipdRegCount * 100).ToString("#0.0"));
            }

            if (nTotal < lastnTotal)
            {
                doc.ReplaceText("CompareNum", "较上月有所减少，");
            }
            else if (nTotal == lastnTotal)
            {
                doc.ReplaceText("CompareNum", "较上月相同，");
            }
            else if (nTotal > lastnTotal)
            {
                doc.ReplaceText("CompareNum", "较上月有所增加，");
            }

            // 使用导尿管(膀胱)人日数
            decimal blTotal = OutLeaveUnPlanTotal(now.Year, now.Month, reportManageService.GetUsedPipeDaysTotal(now.Year,"002"));
            // 使用导尿管(尿道)人日数
            decimal urTotal = OutLeaveUnPlanTotal(now.Year, now.Month, reportManageService.GetUsedPipeDaysTotal(now.Year, "003"));
            
            decimal sTotal = blTotal + urTotal;   //使用导尿管人日数

            decimal uTotal = ipdRegCount - sTotal;

            doc.ReplaceText("STotal", sTotal.ToString("#0"));
            doc.ReplaceText("UTotal", uTotal.ToString("#0"));
            var keys = new[] { "001", "002", "003", "004", "005" };
            var msgInfo = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                if (nTotal == 0)
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                    msgInfo += "";
                    continue;
                }
                var obj = list.FirstOrDefault(o => o.Type == keys[i]);
                if (obj != null)
                {
                    if (i == 2)
                    {
                        doc.ReplaceText("N" + i, obj.Total.ToString());
                        doc.ReplaceText("R" + i, sTotal != 0 ? (obj.Total / sTotal * 100).ToString("#0.0") : "0");
                        msgInfo += obj.Total == 0 ? "" : (",当月使用存留导尿管泌尿道感染人次" + obj.Total.ToString() + "位");
                    }
                    else if (i == 3)
                    {
                        doc.ReplaceText("N" + i, obj.Total.ToString());
                        doc.ReplaceText("R" + i, uTotal != 0 ? (obj.Total / uTotal * 100).ToString("#0.0") : "0");
                        msgInfo += obj.Total == 0 ? "" : (",当月未使用存留导尿管泌尿道感染人次" + obj.Total.ToString() + "位");
                    }
                    else
                    {
                        doc.ReplaceText("N" + i, obj.Total.ToString());
                        doc.ReplaceText("R" + i, (obj.Total / ipdRegCount * 100).ToString("#0.0"));
                    }

                }
                else
                {
                    doc.ReplaceText("N" + i, "0");
                    doc.ReplaceText("R" + i, "0");
                    msgInfo += "";
                }
            }

            var non1 = list.Where(o => o.Type == "001" || o.Type == "002").Sum(o => o.Total);
            msgInfo += non1 == 0 ? "" : (",当月呼吸道感染人次" + non1.ToString() + "位");

            var n5 = list.Where(o => o.Type == "003" || o.Type == "004").Sum(o => o.Total);
            doc.ReplaceText("N5", n5.ToString());
            doc.ReplaceText("R5", n5 != 0 ? (n5 / ipdRegCount * 100).ToString("#0.0") : "0");
            msgInfo += n5 == 0 ? "" : (",当月泌尿道感染人次" + non1.ToString() + "位");

            var n4 = list.Where(o => o.Type == "005").Sum(o => o.Total);
            msgInfo += n4 == 0 ? "" : (",当月皮肤感染人次" + n4.ToString() + "位");

            doc.ReplaceText("ReasonInfo", msgInfo);
        }

        #region 获取 结案/请假/非计划入院/使用导尿管 住民人日次数
        /// <summary>
        /// 获取 结案/请假/非计划入院 住民人日次数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="list">List集合</param>
        /// <returns>人日次数</returns>
        public int OutLeaveUnPlanTotal(int year, int month, List<TimeDiffEntity> list)
        {
            DateTime minDate, maxDate;
            DateTime now = StartDate;
            int Total = 0;

            minDate = new DateTime(year, month, 1);
            maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            if (list != null && list.Count > 0)
            {
                foreach (var le in list)
                {
                    if (le.EndTime < minDate || le.BeginTime > maxDate)
                    {
                        Total += 0;
                    }
                    else
                    {
                        if (le.BeginTime < minDate && le.EndTime == null)
                        {
                            Total += DateTime.DaysInMonth(now.Year, now.Month);
                        }
                        else if (le.BeginTime < minDate && le.EndTime > maxDate)
                        {
                            Total += DateTime.DaysInMonth(now.Year, now.Month);
                        }
                        else if (le.BeginTime < minDate && le.EndTime <= maxDate)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(minDate.Year), Convert.ToInt32(minDate.Month), Convert.ToInt32(minDate.Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(((DateTime)le.EndTime).Year), Convert.ToInt32(((DateTime)le.EndTime).Month), Convert.ToInt32(((DateTime)le.EndTime).Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                        else if (le.BeginTime >= minDate && le.EndTime == null)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(((DateTime)le.BeginTime).Year), Convert.ToInt32(((DateTime)le.BeginTime).Month), Convert.ToInt32(((DateTime)le.BeginTime).Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(maxDate.Year), Convert.ToInt32(maxDate.Month), Convert.ToInt32(maxDate.Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                        else if (le.BeginTime >= minDate && le.EndTime > maxDate)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(((DateTime)le.BeginTime).Year), Convert.ToInt32(((DateTime)le.BeginTime).Month), Convert.ToInt32(((DateTime)le.BeginTime).Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(maxDate.Year), Convert.ToInt32(maxDate.Month), Convert.ToInt32(maxDate.Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                        else if (le.BeginTime >= minDate && le.EndTime <= maxDate)
                        {
                            DateTime dtThis = new DateTime(Convert.ToInt32(((DateTime)le.BeginTime).Year), Convert.ToInt32(((DateTime)le.BeginTime).Month), Convert.ToInt32(((DateTime)le.BeginTime).Day));
                            DateTime dtLast = new DateTime(Convert.ToInt32(((DateTime)le.EndTime).Year), Convert.ToInt32(((DateTime)le.EndTime).Month), Convert.ToInt32(((DateTime)le.EndTime).Day));
                            Total += new TimeSpan(dtLast.Ticks - dtThis.Ticks).Days;
                        }
                    }
                }
            }
            else
            {
                Total = 0;
            }
            return Total;
        }
        #endregion
    }
}

