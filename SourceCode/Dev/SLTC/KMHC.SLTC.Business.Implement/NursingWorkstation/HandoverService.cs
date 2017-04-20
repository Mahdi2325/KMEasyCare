using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.NursingWorkstation;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement
{
    public class HandoverService : BaseService, IHandoverService
    {
        #region LTC_WorkItem
        public BaseResponse<IList<WorkItemModel>> GetWorkItemByType(string itemType)
        {
            var response = base.GetList<LTC_WORKITEM, WorkItemModel>((q) => q.ITEMTYPE == itemType);
            return response;
        }
        
        #endregion

        #region LTC_Handover
        /// <summary>
        /// 获取handoverRecord记录，若指定日期存在该记录，则返回
        /// 若不存在 则先保存查询记录 再返回
        /// </summary>
        /// <param name="handoverDate"></param>
        /// <returns></returns>
        public BaseResponse<HandoverRecord> GetHandoverRecordByDate(DateTime handoverDate)
        {
            BaseResponse<HandoverRecord> response = new BaseResponse<HandoverRecord>();
            var record = unitOfWork.GetRepository<LTC_HANDOVERRECORD>().dbSet.Where(x => x.HANDOVERDATE == handoverDate).FirstOrDefault();
            if (record != null)
            {
                List<LTC_HandoverDtl> LTC_HandoverDtl = new List<LTC_HandoverDtl>();
                HandoverRecord handoverRecord = new HandoverRecord();
                if(record.LTC_HANDOVERDTL.Count>0)
                {
                    Mapper.CreateMap<LTC_HANDOVERDTL, LTC_HandoverDtl>();
                    LTC_HandoverDtl = Mapper.Map<List<LTC_HandoverDtl>>(record.LTC_HANDOVERDTL);
                }
                Mapper.CreateMap<LTC_HANDOVERRECORD, HandoverRecord>();
                handoverRecord = Mapper.Map<HandoverRecord>(record);
                handoverRecord.LTC_HandoverDtl = LTC_HandoverDtl;
                response.Data = handoverRecord;
            }
            else
            {
                string sql =string.Format(@"SELECT I.INDATE AS Flag, 'N' AS Type FROM LTC_IPDREG I 
                                                WHERE I.IPDFLAG='I' AND ORGID='{0}' AND INDATE='{1}'
                                                UNION ALL
                                                SELECT  CASE WHEN ISNULL(I.RSTYPE) THEN '001' ELSE I.RSTYPE END Flag, 'O' AS Type  FROM LTC_IPDREG I 
                                                WHERE I.IPDFLAG='O' AND ORGID='{0}' AND OUTDATE<='{1}' 
                                                UNION ALL
                                                SELECT  CASE WHEN ISNULL(I.RSTYPE) THEN '001' ELSE I.RSTYPE END Flag, 'I' AS Type FROM LTC_IPDREG I 
                                                WHERE I.IPDFLAG='I' AND ORGID='{0}' AND INDATE<='{1}' 
                                                UNION ALL
                                                SELECT DISTINCT FEENO AS Flag ,'LR' AS type FROM LTC_LEAVEHOSP WHERE FEENO>0 
                                                UNION ALL
                                                SELECT DISTINCT FEENO AS Flag ,'L' AS type FROM LTC_LEAVEHOSP WHERE 
                                                FEENO>0  AND ORGID='{0}' AND ((STARTDATE<='{1}'  AND (RETURNDATE>='{1}'  OR RETURNDATE IS NULL))) ;", SecurityHelper.CurrentPrincipal.OrgId, handoverDate.ToShortDateString());
                List<OverallUU> list = unitOfWork.GetRepository<OverallUU>().SqlQuery(sql).ToList();
                if(list!=null && list.Count>0)
                {
                    HandoverRecord NewRecord = new HandoverRecord()
                    {

                        HandoverDate = handoverDate,
                        NewComer = list.Where(x=>x.Type=="N").Count(),
                        TransferSociety = list.Where(x => x.Type == "O"&&x.Flag=="001").Count(),
                        TransferMiniter = list.Where(x => x.Type == "O" && x.Flag == "002").Count(),
                        TransferDisabled = list.Where(x => x.Type == "O" && x.Flag == "003").Count(),
                        OutOverall = list.Where(x => x.Type == "LR").Count(),
                        OutReturn = list.Where(x => x.Type == "LR").Count() - list.Where(x => x.Type == "L").Count(),
                        OutStill = list.Where(x => x.Type == "L").Count(),
                        ActualPopulation = list.Where(x => x.Type == "I").Count() - list.Where(x => x.Type == "L").Count(),
                        InnaiSociety = list.Where(x => x.Type == "I" && x.Flag == "001").Count(),
                        InnaiDisabled = list.Where(x => x.Type == "I" && x.Flag == "002").Count(),
                        InnaiMiniter = list.Where(x => x.Type == "I" && x.Flag == "003").Count(),
                        InnaiOverall = list.Where(x => x.Type == "I").Count()
                        
                    };
                    var result = base.Save<LTC_HANDOVERRECORD, HandoverRecord>(NewRecord, (q) => q.ID == NewRecord.Id);
                    response.Data = result.Data;
                }

            }

            return response;
        }






        #endregion

        #region LTC_HandRecordDtl

        public BaseResponse<LTC_HandoverDtl> SaveHandoverDtl(LTC_HandoverDtl handoverRecord)
        {
            if (handoverRecord.Id>0)
            {
                handoverRecord.UpdateTime = DateTime.Now;
                handoverRecord.UpdateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            }
            else
            {
                handoverRecord.CreateTime = DateTime.Now;
                handoverRecord.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
            }
            return base.Save<LTC_HANDOVERDTL, LTC_HandoverDtl>(handoverRecord, (q) => q.ID == handoverRecord.Id);
        }

        #endregion

    }
}
