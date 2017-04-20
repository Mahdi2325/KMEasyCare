using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public static class DictHelper
    {
        private static IDictManageService dicSvc = IOCContainer.Instance.Resolve<IDictManageService>();

        /// <summary>
        /// 根据传入月份 返回护理险月份信息
        /// </summary>
        /// <param name="month">月份</param>
        /// <returns>月份信息</returns>
        public static LTC_NCIFinancialMonth GetFeeIntervalByMonth(string month)
        {
            return dicSvc.GetFeeIntervalByMonth(month);
        }

        /// <summary>
        /// 根据日期 获取日期对应护理险结算月份信息
        /// 例: string yearMonth = "2017-05";
        /// var date=DictHelper.GetFeeIntervalByYearMonth(yearMonth);
        /// 输出：Month:05 ,StartDate:2017-04-21 ,EndDate:2017-05-20 
        /// </summary>
        /// <param name="d">日期</param>
        /// <returns>日期所在月信息</returns>
        public static LTC_NCIFinancialMonth GetFeeIntervalByYearMonth(string yearMonth)
        {

            try
            {
                Regex reg = new Regex(@"^0+");
                string month = reg.Replace(yearMonth.Split('-')[1], "");
                string year = yearMonth.Split('-')[0];
                LTC_NCIFinancialMonth financialMonth = dicSvc.GetFeeIntervalByMonth(month);
                var temp = Util.DeepCopy<LTC_NCIFinancialMonth>(financialMonth);
                if (DateTime.Compare(Convert.ToDateTime(year + "-" + financialMonth.EndDate), Convert.ToDateTime(year + "-" + financialMonth.StartDate)) < 0)
                {
                    temp.StartDate = Convert.ToDateTime(year + "-" + financialMonth.StartDate).AddYears(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    temp.StartDate = year + "-" + financialMonth.StartDate;
                }
                temp.EndDate = year + "-" + financialMonth.EndDate;

                return temp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        /// <summary>
        /// 根据日期 获取日期对应护理险结算月份信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>日期所在月信息</returns>
        public static LTC_NCIFinancialMonth GetFeeIntervalByDate(string date)
        {

            DateTime d;
            if (DateTime.TryParse(date, out d))
            {
                try
                {
                    string month = d.Month.ToString();
                    LTC_NCIFinancialMonth financialMonth = dicSvc.GetFeeIntervalByMonth(month);
                    if (DateTime.Compare(Convert.ToDateTime(d.Year + "-" + financialMonth.EndDate), d) >= 0)
                    {
                        return financialMonth;
                    }
                    else
                    {
                        return dicSvc.GetFeeIntervalByMonth((d.Month == 12 ? 1 : d.Month + 1).ToString());
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 根据日期 获取日期对应护理险结算月份信息
        /// </summary>
        /// <param name="d">日期</param>
        /// <returns>日期所在月信息</returns>
        public static LTC_NCIFinancialMonth GetFeeIntervalByDate(DateTime d)
        {

            try
            {
                d = Convert.ToDateTime(d.ToShortDateString());
                string month = d.Month.ToString();
                LTC_NCIFinancialMonth financialMonth = dicSvc.GetFeeIntervalByMonth(month);
                if (DateTime.Compare(Convert.ToDateTime(d.Year + "-" + financialMonth.EndDate), d) >= 0)
                {
                    return financialMonth;
                }
                else
                {
                    return dicSvc.GetFeeIntervalByMonth((d.Month == 12 ? 1 : d.Month + 1).ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        /// <summary>
        /// 根据传入日期返回 日期对应月份开始时间
        /// 例: DateTime dt = Convert.ToDateTime("2017-05-01");
        /// var date=DictHelper.GetFeeIntervalStartDateByDate(dt);
        ///   Console.WriteLine(date); //输出：2017-04-21
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime GetFeeIntervalStartDateByDate(DateTime d)
        {

            try
            {

                LTC_NCIFinancialMonth monthInfo = DictHelper.GetFeeIntervalByDate(d);
                DateTime startDate = Convert.ToDateTime(d.Year + "-" + monthInfo.StartDate);
                DateTime endDate = Convert.ToDateTime(d.Year + "-" + monthInfo.EndDate);
                if (startDate.Month > endDate.Month)
                {
                    return startDate.AddYears(-1);
                }
                else
                {
                    return startDate;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }


        /// <summary>
        /// 根据传入日期返回 日期对应月份开始时间
        /// 例: string dt = "2017-05-01";
        /// var date=DictHelper.GetFeeIntervalStartDateByDate(dt);
        ///   Console.WriteLine(date); //输出：2017-04-21
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetFeeIntervalStartDateByDate(string date)
        {

            try
            {
                DateTime d = Convert.ToDateTime(date);
                LTC_NCIFinancialMonth monthInfo = DictHelper.GetFeeIntervalByDate(d);
                DateTime startDate = Convert.ToDateTime(d.Year + "-" + monthInfo.StartDate);
                DateTime endDate = Convert.ToDateTime(d.Year + "-" + monthInfo.EndDate);
                if (startDate.Month > endDate.Month)
                {
                    return startDate.AddYears(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    return startDate.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        /// <summary>
        /// 根据传入日期返回 日期对应月份结束时间
        /// 例: DateTime dt = Convert.ToDateTime("2017-05-01");
        /// var date=DictHelper.GetFeeIntervalEndDateByDate(dt);
        ///   Console.WriteLine(date); //输出：2017-05-20
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime GetFeeIntervalEndDateByDate(DateTime d)
        {

            try
            {
                LTC_NCIFinancialMonth monthInfo = DictHelper.GetFeeIntervalByDate(d);
                DateTime startDate = Convert.ToDateTime(d.Year + "-" + monthInfo.StartDate);
                DateTime endDate = Convert.ToDateTime(d.Year + "-" + monthInfo.EndDate);
                if (startDate.Month > endDate.Month || endDate.Month < d.Month)
                {
                    return endDate.AddYears(1);
                }
                else
                {
                    return endDate;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }


        /// <summary>
        /// 根据传入日期返回 日期对应月份结束时间
        /// 例: string dt = "2017-05-01";
        /// var date=DictHelper.GetFeeIntervalEndDateByDate(dt);
        ///   Console.WriteLine(date); //输出：2017-05-20
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetFeeIntervalEndDateByDate(string date)
        {

            try
            {
                DateTime d = Convert.ToDateTime(date);
                LTC_NCIFinancialMonth monthInfo = DictHelper.GetFeeIntervalByDate(d);
                DateTime startDate = Convert.ToDateTime(d.Year + "-" + monthInfo.StartDate);
                DateTime endDate = Convert.ToDateTime(d.Year + "-" + monthInfo.EndDate);
                if (startDate.Month > endDate.Month || endDate.Month < d.Month)
                {
                    return endDate.AddYears(1).ToString("yyyy-MM-dd");
                }
                else
                {
                    return endDate.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public static int CalcMonthIntervalDay(LTC_NCIFinancialMonth monthInfo)
        {
            try
            {
                int days;
                DateTime d = DateTime.Now;
                DateTime startDate = Convert.ToDateTime(d.Year + "-" + monthInfo.StartDate);
                DateTime endDate = Convert.ToDateTime(d.Year + "-" + monthInfo.EndDate);
                if (startDate.Month > endDate.Month)
                {
                    days = (endDate - startDate.AddYears(-1)).Days;
                }
                else
                {
                    days = (endDate - startDate).Days;
                }
                return days + 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
