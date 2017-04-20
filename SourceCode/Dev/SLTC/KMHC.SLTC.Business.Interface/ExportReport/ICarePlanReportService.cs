using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Report;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface ICarePlanReportService
    {
        List<RegActivityRequEval> GetCarePlanData(long _feeNo, DateTime? startDate, DateTime? endDate, string floorId);
        List<ReportInfo> GetCareH35(long _feeNo, DateTime? startDate, DateTime? endDate, string floorId);
        List<ReportInfo> GetCareH10(long _feeNo, DateTime? startDate, DateTime? endDate, string floorId);
        List<ReportInfo> GetCareP18(long _feeNo, DateTime? startDate, DateTime? endDate, string classtype, string floorId);
        ExportResidentInfo GetExportResidentInfo(long feeNo);
        string GetCodedtlInfo(string itemcode, string itemtype);
        NutritionEvalModel QueryNutritionEval(long id);

        List<ReportCheckRec> QueryBiochemistryList(long feeno, DateTime enddate);

        BaseResponse<IList<DoctorCheckRec>> QueryDocCheckRecData(BaseRequest<DoctorCheckRecFilter> request);
    }
}
