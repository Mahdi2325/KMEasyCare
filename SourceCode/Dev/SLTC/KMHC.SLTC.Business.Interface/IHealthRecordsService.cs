using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.ChargeInputModel;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface
{
    public interface IHealthRecordsService : IBaseService
    {
        HealthRecords GetMeasurementInfo(int feeNo);
        BaseResponse SaveMeaSuredRecord(Measurement baseRequest);
        BaseResponse<IList<Measurement>> QueryHealthRecordList(BaseRequest<HeathRecordFilter> request);
        BaseResponse<object> GetMeasureDataForExtApi(BaseRequest<MeasureDataFilter> request);
        HealthRecords GetMedicationInfo(int feeNo);
        HealthRecords GetBiochemistryInfo(int feeNo);
        HealthRecords GetEvaluationInfo(int feeNo);
        HealthRecords GetDrugInfo(int feeNo);
        BaseResponse<IList<DrugRecord>> QuerydrugRecord(BaseRequest<DrugRecordinfoFilter> request);
    }
}
