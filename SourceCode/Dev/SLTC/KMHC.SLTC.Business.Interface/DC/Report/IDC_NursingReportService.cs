using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface.DC.Report
{
    public interface IDC_NursingReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ReportRegMedicine>> QueryRegMedicineList(BaseRequest<ReportRegMedicine> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ReportRegCpl>> QueryRegCplList(BaseRequest<ReportRegCpl> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<ReportNurseingPlanEval> QueryNurseingPlanEval(BaseRequest<ReportNurseingPlanEval> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<ReportBaseInfoList>> QueryAllRegBaseInfoList(long feeNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<ReportRegActivityRequestEval> QueryCurrentRegActivityRequestEval(long id);
    }
}
