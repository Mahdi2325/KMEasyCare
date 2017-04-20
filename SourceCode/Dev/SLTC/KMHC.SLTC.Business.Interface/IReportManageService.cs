using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMHC.SLTC.Business.Entity.Report;

namespace KMHC.SLTC.Business.Interface
{
    public interface IReportManageService : IBaseService
    {
        #region H20非计画住院
        void DownloadUnPlanedIpdStatisticsFile(int year, string templateFile);
        #endregion

        #region H49院内感染指标统计表
        void DownloadInfectionStatisticsFile(int year, string templateFile);

        List<InfectionInd> GetInfectionIndList(DateTime date);

        #endregion

        #region H77跌倒危险因子评估
        //void DownloadH77(long feeNo, string templateFile);
        void DownloadH77(long feeNo, string templateFile, DateTime? startDate, DateTime? endDate);
        #endregion

        #region H79外出请假记录表
        /// <summary>
        /// 下载H79 外出请假记录
        /// </summary>
        /// <param name="feeNo">住民编号</param>
        /// <param name="templateFile">文件路径</param>
        void DownloadH79(long feeNo, string templateFile);
        #endregion

        #region H50约束
        List<ConstraintRec> GetConstraintList(DateTime date);
        void DownloadH50(int year, string templateFile);
        #endregion

        #region H76压疮风险评估
        //void DownloadPrsSoreRisk(string templateFile, long feeNo);

        void DownloadPrsSoreRisk(string templateFile, long feeNo, DateTime? startDate, DateTime? endDate);
        #endregion


        Question GetQuestion(int id);

        List<Question> GetQuestionList(long feeNo, int questionId);
        Question GetQuestion(long recordId);
        IEnumerable<Answer> GetAnswers(long qId);

        /// <summary>
        /// 公元年份转民国年份
        /// </summary>
        /// <param name="year"></param>
        /// <param name="isDateFormat"></param>
        /// <returns></returns>
        string ADToMinYear(int year, bool isDateFormat);

        IList<Statistic> GetUnPlanEdipd(DateTime date, bool h72Flag);

        int GetUnPlanEdipdH72Total(DateTime date);

        int GetNewResidentTotal(DateTime date);

        /// <summary>
        /// 当月住民总人数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>人数</returns>
        int GetResidentTotal(DateTime date);

        IList<Statistic> GetInfection(DateTime date);

        int GetUsedPipeTotal(DateTime date);

        NSCPLReportView GetNSCPLReportView(int seqNo);

        ResidentInfo GetResidentInfo(long feeNo);

        void DownloadP19(long seqNo, string templateFile);

        BaseResponse<IList<ReportModel>> QueryReport(BaseRequest<ReportFilter> request);
        BaseResponse<List<ReportSetModel>> SaveReport(string orgId, List<ReportSetModel> request);

        /// <summary>
        /// 获取已结案人次日数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>人次日数和</returns>
        List<TimeDiffEntity> GetIpdOutTotal(int year);

        /// <summary>
        /// 获取请假人员离院天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>时间参数集合</returns>
        List<TimeDiffEntity> GetLeaveHospTotal(int year);

        /// <summary>
        /// 非计划人员住院天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>时间参数集合</returns>
        List<TimeDiffEntity> GetUnPlanIpdTotal(int year);

        /// <summary>
        /// 使用导尿管人日数
        /// </summary>
        /// <param name="year">日期</param>
        /// <returns>List</returns>
        List<TimeDiffEntity> GetUsedPipeDaysTotal(int year,string pipName);

        List<TimeDiffEntity> GetUsedPipeDaysTotal(int year);
        /// <summary>
        /// 获取 结案/请假/非计划入院/使用导尿管 住民人日次数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="list">List集合</param>
        /// <returns>人日次数</returns>
        int OutLeaveUnPlanTotal(int year, int month, List<TimeDiffEntity> list);

        #region H35
        CareDemandEvalPrivew GetCareDemandHis(int id, string OrgId);
        #endregion
    } 
}

