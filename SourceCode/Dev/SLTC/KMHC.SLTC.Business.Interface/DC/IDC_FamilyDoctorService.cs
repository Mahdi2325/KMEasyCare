using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface.DC
{
    public interface IDC_FamilyDoctorService
    {
        #region 健康记录追踪
        BaseResponse<IList<DC_RegCheckRecordModel>> QueryRegCheckRecord(BaseRequest<DC_RegCheckRecordFilter> request);
        BaseResponse<List<DC_RegCheckRecordDtlModel>> GetRegCheckRecordDtl(BaseRequest<DC_RegCheckRecordFilter> request);
        BaseResponse<DC_RegCheckRecordModel> SaveRegCheckRecord(DC_RegCheckRecordModel request, List<string> fields);
        BaseResponse DeleteRegCheckRecord(long regCheckRecordId);
        #endregion
        #region 健康记录数据
        BaseResponse<IList<DC_RegCheckRecordDataModel>> QueryRegCheckRecordData(BaseRequest<DC_RegCheckRecordDataFilter> request);
        #endregion
        #region 关怀记录
        BaseResponse<IList<DC_RegNoteRecordModel>> QueryRegNoteRecord(BaseRequest<DC_RegNoteRecordFilter> request);
        BaseResponse<DC_RegNoteRecordModel> SaveRegNoteRecord(DC_RegNoteRecordModel request, List<string> fields);
        BaseResponse DeleteRegNoteRecord(long regNoteId);
        #endregion
        #region 关怀内容
        BaseResponse<IList<DC_NoteModel>> QueryNote(BaseRequest<DC_NoteFilter> request);
        BaseResponse<DC_NoteModel> SaveNote(DC_NoteModel request, List<string> fields);
        BaseResponse DeleteNote(long noteId);
        #endregion
        #region 访谈记录
        BaseResponse<IList<DC_RegVisitRecordModel>> QueryRegVisitRecord(BaseRequest<DC_RegVisitRecordFilter> request);
        BaseResponse<DC_RegVisitRecordModel> SaveRegVisitRecord(DC_RegVisitRecordModel request, List<string> fields);
        BaseResponse DeleteRegVisitRecord(long regVisitId);
        #endregion
        #region 下拉框数据
        BaseResponse<IList<CheckTemplateModel>> GetCheckTemplateList(BaseRequest<CheckTemplateFilter> request);
        #endregion
    }
}

