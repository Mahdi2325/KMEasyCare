/*
创建人: 肖国栋
创建日期:2016-03-09
说明:护理工作站
*/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.Infrastructure.Cached;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using KMHC.SLTC.Business.Entity.DC.Model;
using System.Data.Objects;
using KMHC.SLTC.Business.Entity.NursingWorkstation;
using System.IO;
using System.Web;

namespace KMHC.SLTC.Business.Implement
{
    public class NursingWorkstationService : BaseService, INursingWorkstationService
    {
        #region 生命体征 FOR App
        public BaseResponse<List<MeaSuredRecord>> GetItemList(BaseRequest<MeasureFilter> request)
        {
            var response = new BaseResponse<List<MeaSuredRecord>>();
            var measureRepository = unitOfWork.GetRepository<LTC_MEASUREDRECORD>();
            var itemCodeArr = request.Data.MeasureItemCode.Split(new char[] { ',' });
            var q = from a in measureRepository.dbSet.Where(a => a.FEENO == request.Data.FeeNo && itemCodeArr.Contains(a.MEASUREITEMCODE))
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.MEASUREDPERSON equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new MeaSuredRecord
                    {
                        FeeNo = a.FEENO,
                        MeaSureTime = a.MEASURETIME,
                        MeasureItemCode = a.MEASUREITEMCODE,
                        MeaSuredPerson = a.MEASUREDPERSON,
                        MeaSuredPersonName = re.EMPNAME,
                        MeaSuredValue = a.MEASUREDVALUE.Value,
                        Description = a.DESCRIPTION
                    };
            if (request.Data.MeaSuredTime.HasValue)
            {
                DateTime sdt = request.Data.MeaSuredTime.Value.Date;
                DateTime dt = sdt.AddDays(1);
                q = q.Where(a => a.MeaSureTime >= sdt && a.MeaSureTime < dt);
            }
            q = q.OrderByDescending(a => a.MeaSureTime);
            response.Data = new List<MeaSuredRecord>();
            if (q.Count() != 0)
            {

                response.RecordsCount = q.Count();
                if (request != null && request.PageSize > 0)
                {
                    response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    response.Data = q.ToList();
                }
            }
            return response;
        }

        public BaseResponse<List<MeasureTotalHistory>> GetMeasureTotalHistory(BaseRequest<MeasureFilter> request)
        {
            var response = new BaseResponse<List<MeasureTotalHistory>>();

            var measureList = new List<MeasureTotalHistory>();
            response.Data = measureList;
            var measureRepository = unitOfWork.GetRepository<LTC_MEASUREDRECORD>();
            var measureItemRepository = unitOfWork.GetRepository<LTC_MEASUREITEM>();
            var q = from a in measureRepository.dbSet.Where(a => a.FEENO == request.Data.FeeNo)

                    select new MeasureTotalHistory
                     {
                         feeno = a.FEENO.Value,
                         Date = a.MEASURETIME.Value,
                         MeasureRs = (a.DESCRIPTION == "超标" ? "超标" : "正常")
                     };
            if (request.Data.MeaSuredTime.HasValue)
            {
                DateTime sdt = request.Data.MeaSuredTime.Value.Date;
                DateTime dt = sdt.AddDays(1);
                q = q.Where(a => a.Date >= sdt && a.Date < dt);
            }
            measureList = q.ToList();

            //获取体重数据
            var p = from a in unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().dbSet.Where(a => a.FEENO == request.Data.FeeNo)
                    select new MeasureTotalHistory
                    {
                        feeno = a.FEENO.Value,
                        Date = a.THISRECDATE.Value,
                        MeasureRs = (a.BMIRESULTS == "正常" ? "正常" : "超标")
                    };
            if (request.Data.MeaSuredTime.HasValue)
            {
                DateTime sdt = request.Data.MeaSuredTime.Value.Date;
                DateTime dt = sdt.AddDays(1);
                p = p.Where(a => a.Date >= sdt && a.Date < dt);
            }

            var weightList = p.ToList();

            if (weightList != null)
            {
                measureList.Concat(weightList);
            }

            var m = measureList.GroupBy(a => new { a.Date.Date, a.feeno }).Select(a => new { Date = a.Key.Date, FeeNo = a.Key.feeno, MeasureRs = a.Where(b => b.MeasureRs == "超标").Count() > 0 ? "超标" : "正常" }).OrderByDescending(a => a.Date).ToList();
            if (m.Count() != 0)
            {
                response.RecordsCount = m.Count();
                if (request != null && request.PageSize > 0)
                {
                    response.Data = Mapper.DynamicMap<List<MeasureTotalHistory>>(m.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList());
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    response.Data = Mapper.DynamicMap<List<MeasureTotalHistory>>(m.ToList());
                }
            }
            return response;
        }

        public BaseResponse<List<MeasureData>> GetMeasureDetailByDate(BaseRequest<MeasureFilter> request)
        {
            var response = new BaseResponse<List<MeasureData>>();
            response.Data = new List<MeasureData>();
            DateTime sdt = request.Data.MeaSuredTime.Value.Date;
            DateTime dt = sdt.AddDays(1);

            var measureRepository = unitOfWork.GetRepository<LTC_MEASUREDRECORD>();
            var measureItemRepository = unitOfWork.GetRepository<LTC_MEASUREITEM>();
            var msasuredRecordList = from a in measureRepository.dbSet.Where(a => a.FEENO == request.Data.FeeNo && a.MEASURETIME >= sdt && a.MEASURETIME < dt)
                                     join b in measureItemRepository.dbSet on a.MEASUREITEMCODE equals b.ITEMCODE
                                     group a by new { b.ITEMCODE, b.ITEMNAME } into g
                                     select new
                                     {
                                         ItemCode = g.Key.ITEMCODE,
                                         ItemName = g.Key.ITEMNAME,
                                         Recode = g.OrderByDescending(t => t.MEASURETIME).Select(a => new { a.MEASUREDVALUE, a.DESCRIPTION, a.MEASURETIME })
                                     }
                                     ;
            var measureRecordDetailList = msasuredRecordList.ToList();
            // 获取收缩压数据
            if (measureRecordDetailList != null)
            {
                var isBloodAdd = false;
                var bloodList = new List<MeaSuredRecordModel>();
                foreach (var record in measureRecordDetailList)
                {
                    MeaSuredRecordModel detail = new MeaSuredRecordModel();
                    detail.ItemCode = record.ItemCode;
                    detail.ItemName = record.ItemName;
                    var rs = record.Recode.FirstOrDefault();
                    if (rs != null)
                    {
                        var recode = record.Recode.FirstOrDefault();
                        detail.MeasureValue = recode.MEASUREDVALUE;
                        detail.MeasureRs = recode.DESCRIPTION;
                        detail.MeasureTime = recode.MEASURETIME.Value;
                    }

                    if (!record.ItemCode.StartsWith("S006"))
                    {
                        response.Data.Add(detail);
                    }
                    else
                    {
                        bloodList.Add(detail);
                    }
                }
                if (bloodList.Count > 0)
                {
                    response.Data.Add(bloodList.OrderByDescending(a => a.MeasureTime).FirstOrDefault());
                }
            }

            //获取体重数据
            var weightData = unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>().dbSet.Where(a => a.FEENO == request.Data.FeeNo && a.THISRECDATE >= sdt && a.THISRECDATE < dt).FirstOrDefault();
            if (weightData != null)
            {
                WeightModel detail = new WeightModel();
                detail.ItemCode = "w_001";
                detail.ItemName = "体重";
                detail.Weight = weightData.THISWEIGHT;
                detail.Height = weightData.THISHEIGHT;
                detail.BMI = weightData.BMI;
                detail.MeasureRs = weightData.BMIRESULTS;
                response.Data.Add(detail);
            }
            return response;
        }

        public BaseResponse SaveMeasure(List<MeasureFilter> request)
        {
            BaseResponse response = new BaseResponse();
            var measureRepository = unitOfWork.GetRepository<LTC_MEASUREDRECORD>();
            try
            {
                DateTime measureTime = DateTime.Now;
                foreach (var measure in request)
                {
                    LTC_MEASUREDRECORD msasuredRecord = Mapper.DynamicMap<LTC_MEASUREDRECORD>(measure);
                    msasuredRecord.SOURCE = "APP";
                    msasuredRecord.MEASURETIME = measureTime;
                    msasuredRecord.DESCRIPTION = GetDescription(GetMeasureItem(msasuredRecord.MEASUREITEMCODE), msasuredRecord.MEASUREDVALUE.Value);
                    msasuredRecord.CREATEBY = msasuredRecord.MEASUREDPERSON;
                    msasuredRecord.ORGID = msasuredRecord.ORGID;
                    msasuredRecord.CREATETIME = DateTime.Now;
                    msasuredRecord.ISDELETE = false;
                    measureRepository.Insert(msasuredRecord);
                }

                response.ResultCode = 1;
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ResultCode = 0;
                response.ResultMessage = ex.Message;
            }

            return response;

        }

        public BaseResponse SaveWeight(WeightFilter request)
        {
            BaseResponse response = new BaseResponse();
            var weightRepository = unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>();
            try
            {
                var weightData = new LTC_UNPLANWEIGHTIND();
                weightData.FEENO = request.FeeNo;
                weightData.THISHEIGHT = request.Height;
                weightData.THISWEIGHT = request.Weight;
                weightData.BMI = CountBMI(request.Height.Value, request.Weight.Value);
                weightData.BMIRESULTS = GetBMIResult(weightData.BMI.Value);
                weightData.THISRECDATE = DateTime.Now;
                weightData.RECORDBY = request.MeaSuredPerson;
                weightData.ORGID = request.OrgID;
                weightRepository.Insert(weightData);
                response.ResultCode = 1;
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ResultCode = 0;
                response.ResultMessage = ex.Message;
            }

            return response;

        }

        public BaseResponse<List<WeightRecord>> GeWeightList(BaseRequest<MeasureFilter> request)
        {
            var response = new BaseResponse<List<WeightRecord>>();

            var weightRepository = unitOfWork.GetRepository<LTC_UNPLANWEIGHTIND>();
            var q = from a in weightRepository.dbSet.Where(a => a.FEENO == request.Data.FeeNo)
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new WeightFilter
                    {
                        FeeNo = a.FEENO,
                        OrgID = a.ORGID,
                        Height = a.THISHEIGHT,
                        Weight = a.THISWEIGHT,
                        BMI = a.BMI,
                        BMI_RS = a.BMIRESULTS,
                        MeasureItemCode = "w_001",
                        MeaSuredPerson = a.RECORDBY,
                        MeaSuredPersonName = re.EMPNAME,
                        MeaSuredTime = a.THISRECDATE,
                        MeaSureTime = a.THISRECDATE
                    };
            if (request.Data.MeaSuredTime.HasValue)
            {
                DateTime sdt = request.Data.MeaSuredTime.Value.Date;
                DateTime dt = sdt.AddDays(1);
                q = q.Where(a => a.MeaSuredTime >= sdt && a.MeaSuredTime < dt);
            }

            q = q.OrderByDescending(a => a.MeaSuredTime);
            response.Data = new List<WeightRecord>();
            if (q.Count() != 0)
            {
                response.RecordsCount = q.Count();
                if (request != null && request.PageSize > 0)
                {
                    response.Data = Mapper.DynamicMap<List<WeightRecord>>(q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList());
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    response.Data = Mapper.DynamicMap<List<WeightRecord>>(q.ToList());
                }
            }
            return response;
        }

        private decimal CountBMI(decimal height, decimal weight)
        {
            return Math.Round(weight / ((height / 100) * (height / 100)) * 100) / 100;
        }

        private string GetBMIResult(decimal bmi)
        {
            if (bmi < (decimal)18.5)
            {
                return "过轻";
            }
            else if (bmi <= (decimal)24.99)
            {
                return "正常";
            }
            else if (bmi <= 28)
            {
                return "过重";
            }
            else if (bmi <= 32)
            {
                return "肥胖";
            }
            else
            {
                return "非常肥胖";
            }
        }
        #endregion

        #region 生命体征
        public BaseResponse<List<Vitalsign>> QueryVitalsign(BaseRequest<MeasureFilter> request)
        {

            var response = new BaseResponse<List<Vitalsign>>();

            var measureList = new List<Vitalsign>();
            response.Data = measureList;
            var measureRepository = unitOfWork.GetRepository<LTC_MEASUREDRECORD>();
            var measureItemRepository = unitOfWork.GetRepository<LTC_MEASUREITEM>();
            var q = from a in measureRepository.dbSet.Where(a => a.FEENO == request.Data.FeeNo && a.ISDELETE != true)
                    select new Vitalsign
                    {
                        Date = a.MEASURETIME.Value,
                        RecordDate = a.MEASURETIME,
                        ItemCode = a.MEASUREITEMCODE,
                        Measuredvalue = a.MEASUREDVALUE,
                    };

            measureList = q.OrderByDescending(m => m.RecordDate).ToList();

            if (measureList != null && measureList.Count > 0)
            {



                var newMeasureList = new List<Vitalsign>();
                var measure = new Vitalsign();

                for (int i = 0; i < measureList.Count; i++)
                {
                    if (i > 0)
                    {
                        if (Convert.ToDateTime(measureList[i].RecordDate).ToString("yyyy-MM-dd") != Convert.ToDateTime(measureList[i - 1].RecordDate).ToString("yyyy-MM-dd"))
                        {
                            measure.RecordDate = measureList[i].RecordDate;
                            measure.Date = measureList[i].Date;

                            measure.Bodytemp = measureList.Where(n => n.ItemCode == "001" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "001").FirstOrDefault().Measuredvalue : -1;
                            measure.SBP = measureList.Where(n => n.ItemCode == "004" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "004").FirstOrDefault().Measuredvalue : -1;
                            measure.DBP = measureList.Where(n => n.ItemCode == "005" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "005").FirstOrDefault().Measuredvalue : -1;
                            measure.Pulse = measureList.Where(n => n.ItemCode == "002" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "002").FirstOrDefault().Measuredvalue : -1;
                            measure.Breathe = measureList.Where(n => n.ItemCode == "003" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "003").FirstOrDefault().Measuredvalue : -1;
                            measure.Oxygen = measureList.Where(n => n.ItemCode == "006" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "006").FirstOrDefault().Measuredvalue : -1;
                            measure.Pain = measureList.Where(n => n.ItemCode == "010" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "010").FirstOrDefault().Measuredvalue : -1;
                            measure.Bowels = measureList.Where(n => n.ItemCode == "009" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "009").FirstOrDefault().Measuredvalue : -1;
                            measure.InValue = measureList.Where(n => n.ItemCode == "007" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (decimal)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "007").FirstOrDefault().Measuredvalue : -1;
                            measure.OutValue = measureList.Where(n => n.ItemCode == "008" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (decimal)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "008").FirstOrDefault().Measuredvalue : -1;

                            var hasBsInfo = measureList.Where(n => n.Date.Date == measure.Date.Date && (n.ItemCode == "S006001" || n.ItemCode == "S006002" || n.ItemCode == "S006003" || n.ItemCode == "S006004" || n.ItemCode == "S006005" || n.ItemCode == "S006006" || n.ItemCode == "S006007" || n.ItemCode == "S006008" || n.ItemCode == "S006009")).FirstOrDefault();
                            if (hasBsInfo != null)
                            {
                                measure.BSType = measureList.Where(n => n.Date.Date == measure.Date.Date && (n.ItemCode == "S006001" || n.ItemCode == "S006002" || n.ItemCode == "S006003" || n.ItemCode == "S006004" || n.ItemCode == "S006005" || n.ItemCode == "S006006" || n.ItemCode == "S006007" || n.ItemCode == "S006008" || n.ItemCode == "S006009")).FirstOrDefault().ItemCode;
                                measure.BloodSugar = (decimal)measureList.Where(n => n.Date.Date == measure.Date.Date && (n.ItemCode == "S006001" || n.ItemCode == "S006002" || n.ItemCode == "S006003" || n.ItemCode == "S006004" || n.ItemCode == "S006005" || n.ItemCode == "S006006" || n.ItemCode == "S006007" || n.ItemCode == "S006008" || n.ItemCode == "S006009")).FirstOrDefault().Measuredvalue;
                                measure.BSType = measure.BSType.Substring(4, 3);
                            }
                            else
                            {
                                measure.BSType = "009";
                                measure.BloodSugar = -1;
                            }
                            newMeasureList.Add(measure);
                            measure = new Vitalsign();
                        }
                    }
                    else
                    {
                        measure.RecordDate = measureList[i].RecordDate;
                        measure.Date = measureList[i].Date;
                        measure.Bodytemp = measureList.Where(n => n.ItemCode == "001" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "001").FirstOrDefault().Measuredvalue : -1;
                        measure.SBP = measureList.Where(n => n.ItemCode == "004" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "004").FirstOrDefault().Measuredvalue : -1;
                        measure.DBP = measureList.Where(n => n.ItemCode == "005" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "005").FirstOrDefault().Measuredvalue : -1;
                        measure.Pulse = measureList.Where(n => n.ItemCode == "002" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "002").FirstOrDefault().Measuredvalue : -1;
                        measure.Breathe = measureList.Where(n => n.ItemCode == "003" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "003").FirstOrDefault().Measuredvalue : -1;
                        measure.Oxygen = measureList.Where(n => n.ItemCode == "006" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "006").FirstOrDefault().Measuredvalue : -1;
                        measure.Pain = measureList.Where(n => n.ItemCode == "010" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "010").FirstOrDefault().Measuredvalue : -1;
                        measure.Bowels = measureList.Where(n => n.ItemCode == "009" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (int)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "009").FirstOrDefault().Measuredvalue : -1;
                        measure.InValue = measureList.Where(n => n.ItemCode == "007" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (decimal)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "007").FirstOrDefault().Measuredvalue : -1;
                        measure.OutValue = measureList.Where(n => n.ItemCode == "008" && n.Date.Date == measure.Date.Date).FirstOrDefault() != null ? (decimal)measureList.Where(n => n.Date.Date == measure.Date.Date && n.ItemCode == "008").FirstOrDefault().Measuredvalue : -1;

                        var hasBsInfo = measureList.Where(n => n.Date.Date == measure.Date.Date && (n.ItemCode == "S006001" || n.ItemCode == "S006002" || n.ItemCode == "S006003" || n.ItemCode == "S006004" || n.ItemCode == "S006005" || n.ItemCode == "S006006" || n.ItemCode == "S006007" || n.ItemCode == "S006008" || n.ItemCode == "S006009")).FirstOrDefault();
                        if (hasBsInfo != null)
                        {
                            measure.BSType = measureList.Where(n => n.Date.Date == measure.Date.Date && (n.ItemCode == "S006001" || n.ItemCode == "S006002" || n.ItemCode == "S006003" || n.ItemCode == "S006004" || n.ItemCode == "S006005" || n.ItemCode == "S006006" || n.ItemCode == "S006007" || n.ItemCode == "S006008" || n.ItemCode == "S006009")).FirstOrDefault().ItemCode;
                            measure.BloodSugar = (decimal)measureList.Where(n => n.Date.Date == measure.Date.Date && (n.ItemCode == "S006001" || n.ItemCode == "S006002" || n.ItemCode == "S006003" || n.ItemCode == "S006004" || n.ItemCode == "S006005" || n.ItemCode == "S006006" || n.ItemCode == "S006007" || n.ItemCode == "S006008" || n.ItemCode == "S006009")).FirstOrDefault().Measuredvalue;
                            measure.BSType = measure.BSType.Substring(4, 3);
                        }
                        else
                        {
                            measure.BSType = "009";
                            measure.BloodSugar = -1;
                        }

                        newMeasureList.Add(measure);
                        measure = new Vitalsign();
                    }
                }

                if (newMeasureList.Count() != 0)
                {
                    response.RecordsCount = newMeasureList.Count();
                    if (request != null && request.PageSize > 0)
                    {
                        response.Data = Mapper.DynamicMap<List<Vitalsign>>(newMeasureList.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList());
                        response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                    }
                    else
                    {
                        response.Data = Mapper.DynamicMap<List<Vitalsign>>(newMeasureList.ToList());
                    }
                }
            }
            return response;
        }

        public BaseResponse<Vitalsign> GetVitalsign(long seqNO)
        {
            return base.Get<LTC_VITALSIGN, Vitalsign>((q) => q.SEQNO == seqNO);
        }
        public BaseResponse<Vitalsign> GetVitalsignToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
        {
            return base.Get<LTC_VITALSIGN, Vitalsign>((q) => q.FEENO == FEENO && q.CLASSTYPE == CLASSTYPE && q.RECORDDATE.Value.ToString("yyyy-MM-dd") == RECDATE.ToString("yyyy-MM-dd"));
        }
        public BaseResponse<Vitalsign> SaveVitalsign(Vitalsign request)
        {
            if (request.SeqNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_VITALSIGN, Vitalsign>(request, (q) => q.SEQNO == request.SeqNo);
        }

        public MeasureItem GetMeasureItem(string itemCode)
        {
            Mapper.CreateMap<LTC_MEASUREITEM, MeasureItem>();
            var item = unitOfWork.GetRepository<LTC_MEASUREITEM>().dbSet.FirstOrDefault(m => m.ITEMCODE == itemCode);
            var measureItem = Mapper.Map<MeasureItem>(item);
            return measureItem;
        }

        public string GetDescription(MeasureItem model, float meaSuredValue)
        {
            string description = string.Empty;
            if (model.Lower != null && model.Upper != null)
            {
                if (meaSuredValue >= (float)model.Lower && meaSuredValue <= (float)model.Upper)
                {
                    description = "正常";
                }
                else
                {
                    description = "超出正常值范围";
                }
            }
            return description;
        }

        public BaseResponse<List<Vitalsign>> SaveVitalsign(List<Vitalsign> request)
        {
            BaseResponse<List<Vitalsign>> response = new BaseResponse<List<Vitalsign>>();

            request.ForEach(m =>
            {
                #region 血压(收缩压)
                if (m.SBP != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "004";
                    model.MeaSuredValue = (float)m.SBP;
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 血压(舒张压)
                if (m.DBP != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "005";
                    model.MeaSuredValue = (float)m.DBP;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 脉搏
                if (m.Pulse != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "002";
                    model.MeaSuredValue = (float)m.Pulse;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 体温
                if (m.Bodytemp != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "001";
                    model.MeaSuredValue = (float)m.Bodytemp;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 呼吸
                if (m.Breathe != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "003";
                    model.MeaSuredValue = (float)m.Breathe;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 血氧
                if (m.Oxygen != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "006";
                    model.MeaSuredValue = (float)m.Oxygen;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 疼痛程度
                if (m.Pain != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "010";
                    model.MeaSuredValue = (float)m.Pain;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 大便次数
                if (m.Bowels != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "009";
                    model.MeaSuredValue = (float)m.Bowels;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 注射量
                if (m.InValue != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "007";
                    model.MeaSuredValue = (float)m.InValue;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 排放量
                if (m.OutValue != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    model.MeasureItemCode = "008";
                    model.MeaSuredValue = (float)m.OutValue;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }
                #endregion
                #region 血糖
                if (m.BloodSugar != null)
                {
                    MeaSuredRecord model = new MeaSuredRecord();
                    model.ID = Convert.ToInt32(GenerateCode(SecurityHelper.CurrentPrincipal.OrgId, EnumCodeKey.MeaSuredRecordId));
                    model.FeeNo = m.FeeNo;
                    model.MeaSureTime = m.RecordDate;
                    if (string.IsNullOrEmpty(m.BSType))
                    {
                        m.BSType = "009";
                    }
                    model.MeasureItemCode = "S006" + m.BSType;
                    model.MeaSuredValue = (float)m.OutValue;
                    #region 描述信息
                    model.Description = GetDescription(GetMeasureItem(model.MeasureItemCode), model.MeaSuredValue);
                    model.MeaSuredPerson = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.Createby = SecurityHelper.CurrentPrincipal.EmpNo;
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = false;
                    model.Orgid = SecurityHelper.CurrentPrincipal.OrgId;
                    model.Source = "PC";
                    #endregion
                    SaveMeaSuredRecord(model);
                }

                #endregion
            });
            response.Data = request;
            return response;
        }

        public void SaveMeaSuredRecord(MeaSuredRecord model)
        {
            try
            {
                base.Save<LTC_MEASUREDRECORD, MeaSuredRecord>(model, (q) => q.ID == model.ID);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public BaseResponse DeleteVitalsign(long seqNO)
        {
            return base.Delete<LTC_VITALSIGN>(seqNO);
        }
        #endregion

        #region 输出量
        public BaseResponse<IList<OutValueModel>> QueryOutValue(BaseRequest<OutValueFilter> request)
        {
            BaseResponse<IList<OutValueModel>> response = new BaseResponse<IList<OutValueModel>>();
            var q = from n in unitOfWork.GetRepository<LTC_OUTVALUE>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        OutValueModel = n,
                        EmpName = re.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.OutValueModel.FEENO == request.Data.FeeNo);
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.OutValueModel.ORGID == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.OutValueModel.OUTNO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<OutValueModel>();
                foreach (dynamic item in list)
                {
                    OutValueModel newItem = Mapper.DynamicMap<OutValueModel>(item.OutValueModel);
                    newItem.RecordNameBy = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<OutValueModel> GetOutValue(long outNo)
        {
            return base.Get<LTC_OUTVALUE, OutValueModel>((q) => q.OUTNO == outNo);
        }
        public BaseResponse<OutValueModel> GetOutValueToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
        {
            return base.Get<LTC_OUTVALUE, OutValueModel>((q) => q.FEENO == FEENO && q.CLASSTYPE == CLASSTYPE && q.RECDATE.Value.ToString("yyyy-MM-dd") == RECDATE.ToString("yyyy-MM-dd"));
        }
        public BaseResponse<OutValueModel> SaveOutValue(OutValueModel request)
        {
            if (request.OutNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_OUTVALUE, OutValueModel>(request, (q) => q.OUTNO == request.OutNo);
        }

        public BaseResponse<List<OutValueModel>> SaveOutValue(List<OutValueModel> request)
        {
            BaseResponse<List<OutValueModel>> response = new BaseResponse<List<OutValueModel>>();
            DateTime nowTime = DateTime.Now;
            Mapper.CreateMap<OutValueModel, LTC_OUTVALUE>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_OUTVALUE>(m);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                model.UPDATEDATE = nowTime;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                if (model.OUTNO > 0)
                {
                    unitOfWork.GetRepository<LTC_OUTVALUE>().Update(model);
                }
                else
                {
                    unitOfWork.GetRepository<LTC_OUTVALUE>().Insert(model);
                }

            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteOutValue(long outNo)
        {
            return base.Delete<LTC_OUTVALUE>(outNo);
        }

        #endregion

        #region 输入量
        public BaseResponse<IList<InValueModel>> QueryInValue(BaseRequest<InValueFilter> request)
        {
            BaseResponse<IList<InValueModel>> response = new BaseResponse<IList<InValueModel>>();
            var q = from n in unitOfWork.GetRepository<LTC_INVALUE>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        InValueModel = n,
                        EmpName = re.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.InValueModel.FEENO == request.Data.FeeNo);
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.InValueModel.ORGID == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.InValueModel.INNO);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<InValueModel>();
                foreach (dynamic item in list)
                {
                    InValueModel newItem = Mapper.DynamicMap<InValueModel>(item.InValueModel);
                    newItem.RecordNameBy = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<InValueModel> GetInValue(long inNo)
        {
            return base.Get<LTC_INVALUE, InValueModel>((q) => q.INNO == inNo);
        }
        public BaseResponse<InValueModel> GetInValueToNurse(long FEENO, string CLASSTYPE, DateTime RECDATE)
        {
            return base.Get<LTC_INVALUE, InValueModel>((q) => q.FEENO == FEENO && q.CLASSTYPE == CLASSTYPE && q.RECDATE.Value.ToString("yyyy-MM-dd") == RECDATE.ToString("yyyy-MM-dd"));
        }
        public BaseResponse<InValueModel> SaveInValue(InValueModel request)
        {
            if (request.InNo == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_INVALUE, InValueModel>(request, (q) => q.INNO == request.InNo);
        }

        public BaseResponse<List<InValueModel>> SaveInValue(List<InValueModel> request)
        {
            BaseResponse<List<InValueModel>> response = new BaseResponse<List<InValueModel>>();
            DateTime nowTime = DateTime.Now;
            Mapper.CreateMap<InValueModel, LTC_INVALUE>();
            request.ForEach(m =>
            {
                var model = Mapper.Map<LTC_INVALUE>(m);
                model.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                model.UPDATEDATE = nowTime;
                model.UPDATEBY = SecurityHelper.CurrentPrincipal.EmpNo;
                if (model.INNO > 0)
                {
                    unitOfWork.GetRepository<LTC_INVALUE>().Update(model);
                }
                else
                {
                    unitOfWork.GetRepository<LTC_INVALUE>().Insert(model);
                }

            });
            unitOfWork.Save();
            response.Data = request;
            return response;
        }

        public BaseResponse DeleteInValue(long inNo)
        {
            return base.Delete<LTC_INVALUE>(inNo);
        }

        #endregion

        #region 护理记录
        public BaseResponse<IList<NursingRec>> QueryNursingRec(BaseRequest<NursingRecFilter> request)
        {
            BaseResponse<IList<NursingRec>> response = new BaseResponse<IList<NursingRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_NURSINGREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        NursingRec = n,
                        EmpName = re.EMPNAME
                    };
            if (request.Data.Id > 0)
            {
                q = q.Where(m => m.NursingRec.ID == request.Data.Id);
            }
            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.FEENO == request.Data.FeeNo);
            }
            if (request.Data.RegNo.HasValue && request.Data.RegNo > 0)
            {
                q = q.Where(m => m.NursingRec.REGNO == request.Data.RegNo);
            }

            if (request.Data.PrintFlag != null && Convert.ToBoolean(request.Data.PrintFlag))
            {
                q = q.Where(m => m.NursingRec.PRINTFLAG == true);
            }
            if (request.Data.SDate.HasValue)
            {
                q = q.Where(m => m.NursingRec.RECORDDATE != null && m.NursingRec.RECORDDATE >= request.Data.SDate);
            }
            if (request.Data.EDate.HasValue)
            {
                q = q.Where(m => m.NursingRec.RECORDDATE != null && m.NursingRec.RECORDDATE < request.Data.EDate);
            }

            if (request.Data.Order == "asc")
            {
                q = q.OrderBy(m => m.NursingRec.RECORDDATE);
            }
            else
            {
                q = q.OrderByDescending(m => m.NursingRec.RECORDDATE);
            }
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<NursingRec>();
                foreach (dynamic item in list)
                {
                    NursingRec newItem = Mapper.DynamicMap<NursingRec>(item.NursingRec);
                    newItem.RecordNameBy = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        /// <summary>
        /// 打印数据查询
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回实体集合</returns>
        public BaseResponse<IList<NursingRec>> QueryPrintInfo(BaseRequest<NursingRecFilter> request)
        {
            BaseResponse<IList<NursingRec>> response = new BaseResponse<IList<NursingRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_NURSINGREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        NursingRec = n,
                        EmpName = re.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.FEENO == request.Data.FeeNo);
            }
            if (request.Data.RegNo.HasValue && request.Data.RegNo > 0)
            {
                q = q.Where(m => m.NursingRec.REGNO == request.Data.RegNo);
            }

            if (request.Data.PrintFlag != null && Convert.ToBoolean(request.Data.PrintFlag))
            {
                q = q.Where(m => m.NursingRec.PRINTFLAG == true);
            }
            q = q.OrderByDescending(m => m.NursingRec.CREATEDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<NursingRec>();
                foreach (dynamic item in list)
                {
                    NursingRec newItem = Mapper.DynamicMap<NursingRec>(item.NursingRec);
                    newItem.RecordNameBy = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<NursingRec> GetNursingRec(long id)
        {
            return base.Get<LTC_NURSINGREC, NursingRec>((q) => q.ID == id);
        }

        public BaseResponse<NursingRec> SaveNursingRec(NursingRec request)
        {
            if (request.Id == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;

                if (Convert.ToBoolean(request.SynchronizeFlag))
                {
                    NursingHandover nursingHandover = new NursingHandover();
                    nursingHandover.FeeNo = request.FeeNo;
                    nursingHandover.RegNo = request.RegNo;
                    if (request.ClassType == "D")
                    {
                        nursingHandover.Content_D = request.Content;
                        nursingHandover.Recdate_D = request.RecordDate;
                        nursingHandover.Recordby_D = request.RecordBy;
                        nursingHandover.Nurse_D = request.RecordNameBy;
                    }
                    else if (request.ClassType == "E")
                    {
                        nursingHandover.Content_E = request.Content;
                        nursingHandover.Recdate_E = request.RecordDate;
                        nursingHandover.Recordby_E = request.RecordBy;
                        nursingHandover.Nurse_E = request.RecordNameBy;
                    }
                    else if (request.ClassType == "N")
                    {
                        nursingHandover.Content_N = request.Content;
                        nursingHandover.Recdate_N = request.RecordDate;
                        nursingHandover.Recordby_N = request.RecordBy;
                        nursingHandover.Nurse_N = request.RecordNameBy;
                    }

                    nursingHandover.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                    nursingHandover.CreateDate = DateTime.Now;
                    nursingHandover.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                    base.Save<LTC_NURSINGHANDOVER, NursingHandover>(nursingHandover, (n) => n.ID == request.Id);
                }
            }
            return base.Save<LTC_NURSINGREC, NursingRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse SaveNursingRecList(NursingRecList request)
        {
            var response = new BaseResponse();
            try
            {
                request.list.ForEach((p) =>
                {
                    SaveNursingRec(p);
                });
                response.ResultCode = 1;
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = ex.Message;
            }
            return response;
        }

        public BaseResponse DeleteNursingRec(long id)
        {
            return base.Delete<LTC_NURSINGREC>(id);
        }
        #endregion

        #region 护理交班
        public BaseResponse<IList<NursingHandover>> QueryNursingHandover(BaseRequest<NursingHandoverFilter> request)
        {
            BaseResponse<IList<NursingHandover>> response = new BaseResponse<IList<NursingHandover>>();
            var q = from n in unitOfWork.GetRepository<LTC_NURSINGHANDOVER>().dbSet
                    join e_d in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY_D equals e_d.EMPNO into re_ds
                    join e_e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY_E equals e_e.EMPNO into re_es
                    join e_n in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY_N equals e_n.EMPNO into re_ns
                    from re_d in re_ds.DefaultIfEmpty()
                    from re_e in re_es.DefaultIfEmpty()
                    from re_n in re_ns.DefaultIfEmpty()
                    select new
                    {
                        NursingRec = n,
                        Nurse_D = re_d.EMPNAME,
                        Nurse_E = re_e.EMPNAME,
                        Nurse_N = re_n.EMPNAME
                    };

            if (request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.FEENO == request.Data.FeeNo);
            }
            if (request.Data.RegNo.HasValue)
            {
                q = q.Where(m => m.NursingRec.REGNO == request.Data.RegNo);
            }
            q = q.OrderByDescending(m => m.NursingRec.CREATEDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<NursingHandover>();
                foreach (dynamic item in list)
                {
                    NursingHandover newItem = Mapper.DynamicMap<NursingHandover>(item.NursingRec);
                    newItem.Nurse_D = item.Nurse_D;
                    newItem.Nurse_E = item.Nurse_E;
                    newItem.Nurse_N = item.Nurse_N;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<NursingHandover> GetNursingHandover(long id)
        {
            return base.Get<LTC_NURSINGHANDOVER, NursingHandover>((q) => q.ID == id);
        }
        public BaseResponse<NursingHandover> SaveNursingHandover(NursingHandover request)
        {
            if (request.Id == 0)
            {
                request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                request.CreateDate = DateTime.Now;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_NURSINGHANDOVER, NursingHandover>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse SaveMulNursingHandover(List<NursingHandover> request)
        {


            var response = new BaseResponse();
            if (request != null && request.Count > 0)
            {
                unitOfWork.BeginTransaction();
                request.ForEach((p) =>
                {
                    if (p.Id == 0)
                    {
                        p.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                        p.CreateDate = DateTime.Now;
                        p.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                    }
                    base.Save<LTC_NURSINGHANDOVER, NursingHandover>(p, (q) => q.ID == p.Id);
                });
                unitOfWork.Commit();
            }
            return response;
        }

        public BaseResponse DeleteNursingHandover(long id)
        {
            return base.Delete<LTC_NURSINGHANDOVER>(id);
        }
        #endregion

        #region 行政交班

        public BaseResponse<IList<AffairsHandover>> QueryAffairsHandover(BaseRequest<AffairsHandoverFilter> request)
        {
            var response = base.Query<LTC_AFFAIRSHANDOVER, AffairsHandover>(request, (q) =>
            {
                if (request != null && !string.IsNullOrEmpty(request.Data.RecorderName))
                {
                    q = q.Where(m => m.RECORDERNAME.Contains(request.Data.RecorderName));
                }
                q = q.OrderByDescending(m => m.ID);
                return q;
            });
            return response;
        }

        public BaseResponse<IList<AffairsHandover>> QueryAffairsHandoverExtend(BaseRequest<AffairsHandoverFilter> request)
        {
            BaseResponse<IList<AffairsHandover>> response = new BaseResponse<IList<AffairsHandover>>();
            var q = from u in unitOfWork.GetRepository<LTC_AFFAIRSHANDOVER>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on u.RECORDBY equals e.EMPNO into e1
                    from show1 in e1.DefaultIfEmpty()
                    join s in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on u.EXECUTEBY equals s.EMPNO into e2
                    from show2 in e2.DefaultIfEmpty()
                    select new AffairsHandover
                    {
                        Id = u.ID,
                        RecordDate = u.RECORDDATE,
                        RecorderName = show1.EMPNAME,
                        ExecuteDate = u.EXECUTEDATE,
                        ExecutiveName = show2.EMPNAME,
                        FinishFlag = u.FINISHFLAG,
                        Content = u.CONTENT,
                        FinishDate = u.FINISHDATE
                    };

            if (request != null && !string.IsNullOrEmpty(request.Data.RecorderName))
            {
                q = q.Where(m => m.RecorderName.Contains(request.Data.RecorderName));
            }
            q = q.OrderByDescending(m => m.RecordDate);
            response.RecordsCount = q.Count();
            List<AffairsHandover> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }
            response.Data = list;
            return response;
        }

        public BaseResponse<AffairsHandover> GetAffairsHandover(long id)
        {
            return base.Get<LTC_AFFAIRSHANDOVER, AffairsHandover>((q) => q.ID == id);
        }

        public BaseResponse<AffairsHandover> SaveAffairsHandover(AffairsHandover request)
        {
            return base.Save<LTC_AFFAIRSHANDOVER, AffairsHandover>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse SaveMulAffairsHandover(List<AffairsHandover> request)
        {


            var response = new BaseResponse();
            if (request != null && request.Count > 0)
            {
                unitOfWork.BeginTransaction();
                request.ForEach((p) =>
                {
                    base.Save<LTC_AFFAIRSHANDOVER, AffairsHandover>(p, (q) => q.ID == p.Id);
                });
                unitOfWork.Commit();
            }
            return response;
        }
        public BaseResponse DeleteAffairsHandover(long id)
        {
            return base.Delete<LTC_AFFAIRSHANDOVER>(id);
        }
        #endregion

        #region 工作照会
        public BaseResponse<IList<AssignTask>> QueryAssignTask(BaseRequest<AssignTaskFilter> request)
        {
            var response = new BaseResponse<IList<AssignTask>>();
            var q = from m in unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on m.FEENO equals ipd.FEENO into ipd_Reg
                    from ipdReg in ipd_Reg.DefaultIfEmpty()
                    join regf in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdReg.REGNO equals regf.REGNO into reg_f
                    from reg_file in reg_f.DefaultIfEmpty()
                    join emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on m.ASSIGNEE equals emp.EMPNO into emp_f
                    from emp_file in emp_f.DefaultIfEmpty()
                    select new
                    {
                        AssignTask = m,
                        ResidentName = reg_file.NAME,
                        EMPNAME = emp_file.EMPNAME
                    };

            if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.AssignTask.ORGID == request.Data.OrgId);
            }
            if (request != null && request.Data.FeeNo.HasValue)
            {
                q = q.Where(m => m.AssignTask.FEENO == request.Data.FeeNo);
            }
            //if (request != null && request.Data.RecStatus.HasValue)
            //{
            //    q = q.Where(m => m.AssignTask.RECSTATUS == request.Data.RecStatus);
            //}
            //if (request != null && request.Data.NewRecFlag.HasValue)
            //{
            //    q = q.Where(m => m.AssignTask.NEWRECFLAG == request.Data.NewRecFlag);
            //}

            if (request != null && request.Data.SDate.HasValue && request.Data.EDate.HasValue)
            {
                var endDate = request.Data.EDate.Value.AddDays(1);
                q = q.Where(m => m.AssignTask.PERFORMDATE >= request.Data.SDate && m.AssignTask.PERFORMDATE < endDate);
            }
            else if (request != null && request.Data.SDate.HasValue)
            {
                q = q.Where(m => m.AssignTask.PERFORMDATE >= request.Data.SDate);
            }
            else if (request != null && request.Data.EDate.HasValue)
            {
                var endDate = request.Data.EDate.Value.AddDays(1);
                q = q.Where(m => m.AssignTask.PERFORMDATE < endDate);
            }
            if (request != null && !string.IsNullOrWhiteSpace(request.Data.Assignee))
            {
                q = q.Where(m => m.AssignTask.ASSIGNEE == request.Data.Assignee);
            }

            if (request != null && request.Data.TaskStatus != null)
            {
                var q1 = q.Where(m => m.AssignTask.ID == -1);
                var q2 = q.Where(m => m.AssignTask.ID == -1);
                var q3 = q.Where(m => m.AssignTask.ID == -1);
                var q4 = q.Where(m => m.AssignTask.ID == -1);

                if (request.Data.TaskStatus.Contains("R0"))
                {
                    q1 = q.Where(m => m.AssignTask.NEWRECFLAG == true);
                }
                if (request.Data.TaskStatus.Contains("R1"))
                {
                    q2 = q.Where(m => m.AssignTask.NEWRECFLAG == false);
                }
                if (request.Data.TaskStatus.Contains("F0"))
                {
                    q3 = q.Where(m => m.AssignTask.RECSTATUS == false);
                }
                if (request.Data.TaskStatus.Contains("F1"))
                {
                    q4 = q.Where(m => m.AssignTask.RECSTATUS == true);
                }

                q = q1.Union(q2).Union(q3).Union(q4);

            }

            q = q.OrderByDescending(m => m.AssignTask.NEWRECFLAG).ThenBy(m => m.AssignTask.RECSTATUS).ThenByDescending(m => m.AssignTask.PERFORMDATE).ThenByDescending(m => m.AssignTask.ID);

            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<AssignTask>();
                foreach (dynamic item in list)
                {
                    AssignTask newItem = Mapper.DynamicMap<AssignTask>(item.AssignTask);
                    newItem.ResidentName = item.ResidentName;
                    newItem.EMPNAME = item.EMPNAME;
                    response.Data.Add(newItem);
                }
            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            //var response = base.Query<LTC_ASSIGNTASK, AssignTask>(request, (q) =>
            //{
            //    if (request != null && !string.IsNullOrEmpty(request.Data.OrgId))
            //    {
            //        q = q.Where(m => m.ORGID == request.Data.OrgId);
            //    }
            //    if (request != null && request.Data.FeeNo.HasValue)
            //    {
            //        q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    }
            //    if (request != null && request.Data.SDate.HasValue && request.Data.EDate.HasValue)
            //    {
            //        var endDate = request.Data.EDate.Value.AddDays(1);
            //        q = q.Where(m => m.ASSIGNDATE >= request.Data.SDate && m.ASSIGNDATE < endDate);
            //    }
            //    else if (request != null && request.Data.SDate.HasValue)
            //    {
            //        q = q.Where(m => m.ASSIGNDATE >= request.Data.SDate);
            //    }
            //    else if (request != null && request.Data.EDate.HasValue)
            //    {
            //        var endDate = request.Data.EDate.Value.AddDays(1);
            //        q = q.Where(m => m.ASSIGNDATE < endDate);
            //    }
            //    if (request != null && !string.IsNullOrWhiteSpace(request.Data.Assignee))
            //    {
            //        q = q.Where(m => m.ASSIGNEE == request.Data.Assignee);
            //    }
            //    q = q.OrderByDescending(m => m.ID);
            //    return q;
            //});
            return response;
        }

        public BaseResponse<AssignTask> GetAssignTask(long id)
        {
            return base.Get<LTC_ASSIGNTASK, AssignTask>((q) => q.ID == id);
        }

        /// <summary>
        /// 更新工作状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="recStatus">recStatus</param>
        /// <param name="finishDate">finishDate</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse ChangeRecStatus(int id, bool? recStatus, DateTime? finishDate, bool? newrecFlag)
        {
            BaseResponse response = new BaseResponse();
            LTC_ASSIGNTASK tr = unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet.Where(x => x.ID == id && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            tr.RECSTATUS = recStatus;
            tr.FINISHDATE = finishDate;
            tr.NEWRECFLAG = newrecFlag;
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().Update(tr);
            unitOfWork.Commit();
            return response;
        }
        /// <summary>
        /// 更新未读状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="newRecFlag">newRecFlag</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse ChangeNewRecStatus(int id, bool? newRecFlag)
        {
            BaseResponse response = new BaseResponse();
            LTC_ASSIGNTASK tr = unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet.Where(x => x.ID == id && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            tr.NEWRECFLAG = newRecFlag;
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().Update(tr);
            unitOfWork.Commit();
            return response;
        }

        public BaseResponse<AssignTask> SaveAssignTask(AssignTask request)
        {
            if (request.Id == 0)
            {
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            request.RecStatus = false;
            request.NewrecFlag = true;
            request.AutoFlag = false;
            return base.Save<LTC_ASSIGNTASK, AssignTask>(request, (q) => q.ID == request.Id);
        }
        public BaseResponse<AssignTask> SaveAssignTask2(AssignTask2 request)
        {
            AssignTask ass = new AssignTask
            {
                OrgId = SecurityHelper.CurrentPrincipal.OrgId,
                PerformDate = request.NEXTEVALDATE,
                Assignee = request.NEXTEVALUATEBY == null ? SecurityHelper.CurrentPrincipal.EmpNo : request.NEXTEVALUATEBY,
                FeeNo = request.FEENO,
                Content = AutoTaskTmp.AutoTask[request.KEY].Content,
                RecStatus = false,
                NewrecFlag = true,
                AutoFlag = true,
                AssignDate = DateTime.Now,
                AssignedBy = SecurityHelper.CurrentPrincipal.EmpNo,
                AssignedName = SecurityHelper.CurrentPrincipal.EmpName
            };
            return base.Save<LTC_ASSIGNTASK, AssignTask>(ass, (q) => (false));
        }
        public BaseResponse SaveAssignTask(List<AssignTask> request)
        {
            var response = new BaseResponse();
            if (request != null && request.Count > 0)
            {
                unitOfWork.BeginTransaction();
                request.ForEach((p) =>
                {
                    if (p.Id == 0)
                    {
                        p.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
                        p.RecStatus = false;
                        p.NewrecFlag = true;
                        p.AutoFlag = false;
                    }
                    base.Save<LTC_ASSIGNTASK, AssignTask>(p, (q) => q.ID == p.Id);
                });
                unitOfWork.Commit();
            }
            return response;
        }

        public BaseResponse DeleteAssignTask(long id)
        {
            return base.Delete<LTC_ASSIGNTASK>(id);
        }

        public BaseResponse ReAssignTask(AssignTask oldTask, IList<TaskEmpFile> empList)
        {
            BaseResponse response = new BaseResponse();
            //var oldTask = GetAssignTask(id);
            List<LTC_ASSIGNTASK> newTaskList = empList.Select(t => new LTC_ASSIGNTASK
            {
                ID = 0,
                ASSIGNEDBY = SecurityHelper.CurrentPrincipal.EmpNo,
                ASSIGNEDNAME = SecurityHelper.CurrentPrincipal.EmpName,
                ASSIGNEE = t.EmpNo,
                ASSIGNNAME = t.EmpName,
                NEWRECFLAG = true,
                ASSIGNDATE = DateTime.Now,
                CONTENT = oldTask.Content,
                FEENO = oldTask.FeeNo,
                ORGID = oldTask.OrgId,
            }).ToList();
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().InsertRange(newTaskList);
            unitOfWork.GetRepository<LTC_ASSIGNTASK>().Delete(oldTask.Id);
            unitOfWork.Commit();
            return response;
        }

        #endregion

        #region 医师评估
        public BaseResponse<IList<DoctorEvalRec>> QueryDocEvalRecData(BaseRequest<DoctorEvalRecFilter> request)
        {
            //var response = base.Query<LTC_DOCTOREVALREC, DoctorEvalRec>(request, (q) =>
            //{
            //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    q = q.OrderByDescending(m => m.FEENO);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<DoctorEvalRec>> response = new BaseResponse<IList<DoctorEvalRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_DOCTOREVALREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.DOCNAME equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        DoctorEvalRec = n,
                        EmpName = re.EMPNAME
                    };


            q = q.Where(m => m.DoctorEvalRec.FEENO == request.Data.FeeNo);

            q = q.OrderByDescending(m => m.DoctorEvalRec.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<DoctorEvalRec>();
                foreach (dynamic item in list)
                {
                    DoctorEvalRec newItem = Mapper.DynamicMap<DoctorEvalRec>(item.DoctorEvalRec);
                    newItem.DocActName = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<DoctorEvalRec> GetDocEvalRecData(long id)
        {
            return base.Get<LTC_DOCTOREVALREC, DoctorEvalRec>((q) => q.ID == id);
        }

        public BaseResponse<DoctorEvalRec> SaveDocEvalRecData(DoctorEvalRec request)
        {
            //request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo; 
            request.CreateBy = "1";
            request.CreateDate = DateTime.Now;
            return base.Save<LTC_DOCTOREVALREC, DoctorEvalRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteDocEvalRecData(long id)
        {
            return base.Delete<LTC_DOCTOREVALREC>(id);
        }
        #endregion

        #region 医师巡诊
        public BaseResponse<IList<DoctorCheckRec>> QueryDocCheckRecData(BaseRequest<DoctorCheckRecFilter> request)
        {
            //var response = base.Query<LTC_DOCTORCHECKREC, DoctorCheckRec>(request, (q) =>
            //{
            //    q = q.Where(m => m.FEENO == request.Data.FeeNo);
            //    q = q.OrderByDescending(m => m.ID);
            //    return q;
            //});
            //return response;

            BaseResponse<IList<DoctorCheckRec>> response = new BaseResponse<IList<DoctorCheckRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_DOCTORCHECKREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.DOCNO equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        DoctorCheckRec = n,
                        EmpName = re.EMPNAME
                    };


            q = q.Where(m => m.DoctorCheckRec.FEENO == request.Data.FeeNo);

            q = q.OrderByDescending(m => m.DoctorCheckRec.ID);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<DoctorCheckRec>();
                foreach (dynamic item in list)
                {
                    DoctorCheckRec newItem = Mapper.DynamicMap<DoctorCheckRec>(item.DoctorCheckRec);
                    newItem.DocName = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<DoctorCheckRec> GetDocCheckRecData(long id)
        {
            return base.Get<LTC_DOCTORCHECKREC, DoctorCheckRec>((q) => q.ID == id);
        }

        public BaseResponse<DoctorCheckRec> SaveDocCheckRecData(DoctorCheckRec request)
        {

            //request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo; 

            return base.Save<LTC_DOCTORCHECKREC, DoctorCheckRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteDocCheckRecData(long id)
        {
            return base.Delete<LTC_DOCTORCHECKREC>(id);
        }
        #endregion

        #region 用药记录
        public BaseResponse<IList<VisitPrescription>> QueryVisitPrescription(BaseRequest<VisitPrescriptionFilter> request)
        {
            BaseResponse<IList<VisitPrescription>> response = new BaseResponse<IList<VisitPrescription>>();
            var q = from it in unitOfWork.GetRepository<LTC_VISITPRESCRIPTION>().dbSet
                    join n in unitOfWork.GetRepository<LTC_VISITDOCRECORDS>().dbSet on it.SEQNO equals n.SEQNO into nns
                    from nn in nns.DefaultIfEmpty()
                    join m in unitOfWork.GetRepository<LTC_MEDICINE>().dbSet on it.MEDID equals m.MEDID into mms
                    from mm in mms.DefaultIfEmpty()
                    join e2 in unitOfWork.GetRepository<LTC_VISITHOSPITAL>().dbSet on nn.VISITHOSP equals e2.HOSPNO into vr_e2
                    from vr_emp2 in vr_e2.DefaultIfEmpty()
                    join e3 in unitOfWork.GetRepository<LTC_VISITDEPT>().dbSet on nn.VISITDEPT equals e3.DEPTNO into vr_e3
                    from vr_emp3 in vr_e3.DefaultIfEmpty()
                    join e4 in unitOfWork.GetRepository<LTC_VISITDOCTOR>().dbSet on nn.VISITDOCTOR equals e4.DEPTNO into vr_e4
                    from vr_emp4 in vr_e4.DefaultIfEmpty()
                    select new VisitPrescription
                    {
                        PId = it.PID,
                        SeqNo = it.SEQNO,
                        MedId = it.MEDID,
                        TakeQty = it.TAKEQTY,
                        Qty = it.QTY,
                        Freq = it.FREQ,
                        Freqday = it.FREQDAY,
                        Freqqty = it.FREQQTY,
                        TakeWay = it.TAKEWAY,
                        Freqtime = it.FREQTIME,
                        LongFlag = it.LONGFLAG,
                        UseFlag = it.USEFLAG,
                        StartDate = it.STARTDATE,
                        EndDate = it.ENDDATE,
                        Description = it.DESCRIPTION,
                        OrgId = it.ORGID,
                        EngName = mm.ENGNAME,
                        MedKind = mm.MEDKIND,
                        CnName = it.CNNAME,
                        FeeNo = nn.FEENO,
                        VisitDoctorName = vr_emp4.DOCNAME,
                        VisitHospName = vr_emp2.HOSPNAME,
                        VisitDeptName = vr_emp3.DEPTNAME,
                        VisitType = nn.VISITTYPE,
                        TakeDays = nn.TAKEDAYS
                    };
            q = q.Where(m => m.FeeNo == request.Data.FeeNo);
            if (request.Data.StartDate != null && request.Data.EndDate != null)
            {
                q = q.Where(m => m.StartDate.Value >= request.Data.StartDate.Value && m.StartDate.Value <= request.Data.EndDate.Value);
            }
            if (request.Data.StartDate != null && request.Data.EndDate == null)
            {
                q = q.Where(m => m.StartDate.Value >= request.Data.StartDate.Value);
            }
            if (request.Data.StartDate == null && request.Data.EndDate != null)
            {
                q = q.Where(m => m.StartDate.Value <= request.Data.EndDate.Value);
            }
            q = q.OrderByDescending(m => m.SeqNo);
            response.RecordsCount = q.Count();
            if (request != null && request.PageSize > 0)
            {
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                response.Data = q.ToList();
            }
            return response;
        }

        #endregion

        #region 团队活动
        public BaseResponse<IList<GroupActivityRec>> QueryGroupActivityRec(BaseRequest<GroupActivityRecFilter> request)
        {
            BaseResponse<IList<GroupActivityRec>> response = new BaseResponse<IList<GroupActivityRec>>();
            var q = from n in unitOfWork.GetRepository<LTC_GROUPACTIVITYREC>().dbSet
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.LEADERNAME equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        GroupActivityRec = n,
                        EmpName = re.EMPNAME
                    };
            if (!string.IsNullOrEmpty(request.Data.ActivityName))
            {
                q = q.Where(m => m.GroupActivityRec.ACTIVITYNAME.Contains(request.Data.ActivityName));
            }
            if (!string.IsNullOrEmpty(request.Data.OrgId))
            {
                q = q.Where(m => m.GroupActivityRec.ORGID == request.Data.OrgId);
            }
            q = q.OrderByDescending(m => m.GroupActivityRec.RECORDDATE);
            response.RecordsCount = q.Count();
            Action<IList> mapperResponse = (IList list) =>
            {
                response.Data = new List<GroupActivityRec>();
                foreach (dynamic item in list)
                {
                    GroupActivityRec newItem = Mapper.DynamicMap<GroupActivityRec>(item.GroupActivityRec);
                    newItem.LeaderName = item.EmpName;
                    response.Data.Add(newItem);
                }

            };
            if (request != null && request.PageSize > 0)
            {
                var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                mapperResponse(list);
            }
            else
            {
                var list = q.ToList();
                mapperResponse(list);
            }
            return response;
        }

        public BaseResponse<GroupActivityRec> GetGroupActivityRec(int id)
        {
            BaseResponse<GroupActivityRec> response = base.Get<LTC_GROUPACTIVITYREC, GroupActivityRec>((q) => q.ID == id);
            if (response.Data != null)
            {
                if (!string.IsNullOrEmpty(response.Data.AttendNo))
                {
                    List<string> resident = new List<string>(response.Data.AttendNo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                    List<long> residentNo = new List<long>();
                    residentNo.AddRange(resident.ConvertAll(o => long.Parse(o)));
                    var residentName = (from r in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
                                        join ip in unitOfWork.GetRepository<LTC_IPDREG>().dbSet.Where(x => residentNo.Contains(x.FEENO)) on r.REGNO equals ip.REGNO
                                        select new
                                        {
                                            r.NAME
                                        });
                    if (residentName != null)
                    {
                        response.Data.AttendName = string.Join(",", residentName.Select(x => x.NAME).ToList());
                    }
                }
            }
            return response;
        }

        public BaseResponse<GroupActivityRec> SaveGroupActivityRec(GroupActivityRec request)
        {
            if (request.Id == 0)
            {
                if (string.IsNullOrEmpty(request.CreateBy))
                {
                    request.CreateBy = SecurityHelper.CurrentPrincipal.EmpNo;
                }
                request.CreateDate = DateTime.Now;
                request.OrgId = SecurityHelper.CurrentPrincipal.OrgId;
            }
            return base.Save<LTC_GROUPACTIVITYREC, GroupActivityRec>(request, (q) => q.ID == request.Id);
        }

        public BaseResponse DeleteGroupActivityRec(int id)
        {
            return base.Delete<LTC_GROUPACTIVITYREC>(id);
        }
        #endregion

        #region 巡房记录
        public BaseResponse<List<LookOverModel>> GetLookOverList(BaseRequest<LookOverModel> request)
        {
            var response = new BaseResponse<List<LookOverModel>>();
            var lookOverRepository = unitOfWork.GetRepository<LTC_LOOKOVER>();
            var floorRepository = unitOfWork.GetRepository<LTC_ORGFLOOR>();
            var q = from a in lookOverRepository.dbSet.Where(a => !a.ISDETELE.Value)
                    join b in floorRepository.dbSet on a.FLOORID equals b.FLOORID
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        a.ID,
                        a.LOOKOVERTIME,
                        a.ITEMCODE,
                        a.FLOORID,
                        b.FLOORNAME,
                        a.LOOKOVERPHOTOS,
                        a.CONTENT,
                        a.RECORDBY,
                        RECORDNAME = re.EMPNAME
                    };
            if (request.Data.LookOverTime.HasValue)
            {
                DateTime sdt = request.Data.LookOverTime.Value.Date;
                DateTime dt = sdt.AddDays(1);
                q = q.Where(a => a.LOOKOVERTIME >= sdt && a.LOOKOVERTIME < dt);
            }

            if (!string.IsNullOrEmpty(request.Data.FloorId))
            {
                q = q.Where(a => a.FLOORID == request.Data.FloorId);
            }

            q = q.OrderByDescending(a => a.LOOKOVERTIME);
            response.Data = new List<LookOverModel>();
            if (q.Count() != 0)
            {
                response.RecordsCount = q.Count();
                if (request != null && request.PageSize > 0)
                {
                    response.Data = Mapper.DynamicMap<List<LookOverModel>>(q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList());
                    response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
                }
                else
                {
                    response.Data = Mapper.DynamicMap<List<LookOverModel>>(q.ToList());
                }
            }
            foreach (var lookOver in response.Data)
            {
                if (!string.IsNullOrEmpty(lookOver.LookOverPhotos))
                {
                    string[] photoArr = lookOver.LookOverPhotos.Split(new char[] { ';' });
                    lookOver.PhotoList = photoArr;
                }
            }

            return response;
        }

        public BaseResponse<LookOverModel> GetLookOverById(int id)
        {
            var response = new BaseResponse<LookOverModel>();
            response.Data = new LookOverModel();
            var q = from a in unitOfWork.GetRepository<LTC_LOOKOVER>().dbSet.Where(a => a.ID == id)
                    join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on a.RECORDBY equals e.EMPNO into res
                    from re in res.DefaultIfEmpty()
                    select new
                    {
                        a.ID,
                        a.LOOKOVERTIME,
                        a.ITEMCODE,
                        a.FLOORID,
                        a.LOOKOVERPHOTOS,
                        a.CONTENT,
                        a.RECORDBY,
                        RECORDNAME = re.EMPNAME
                    };
            var lookOver = q.FirstOrDefault();
            if (lookOver != null)
            {
                response.Data = Mapper.DynamicMap<LookOverModel>(lookOver);

                if (!string.IsNullOrEmpty(response.Data.LookOverPhotos))
                {
                    string[] photoArr = response.Data.LookOverPhotos.Split(new char[] { ';' });
                    response.Data.PhotoList = photoArr;
                }
            }

            return response;
        }

        public BaseResponse<string> SaveLookOver(LookOverModel request)
        {
            BaseResponse<string> response = new BaseResponse<string>();
            var lookOverRepository = unitOfWork.GetRepository<LTC_LOOKOVER>();
            try
            {

                if (request.ID == 0)
                {
                    LTC_LOOKOVER lookOverRecord = Mapper.DynamicMap<LTC_LOOKOVER>(request);
                    lookOverRecord.CREATEBY = request.RecordBy;
                    lookOverRecord.CREATETIME = DateTime.Now;
                    lookOverRecord.ISDETELE = false;
                    lookOverRepository.Insert(lookOverRecord);
                }
                else
                {
                    LTC_LOOKOVER lookOverRecord = lookOverRepository.Get(request.ID);
                    lookOverRecord.FLOORID = request.FloorId;
                    lookOverRecord.CONTENT = request.Content;
                    lookOverRecord.LOOKOVERPHOTOS = request.LookOverPhotos;
                    lookOverRecord.UPDATEBY = request.RecordBy;
                    lookOverRecord.UPDATETIME = DateTime.Now;
                    lookOverRepository.Update(lookOverRecord);
                }
                response.ResultCode = 1;
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ResultCode = 0;
                response.ResultMessage = ex.Message;
            }

            return response;
        }

        public BaseResponse<string> DeleteLookOver(int id)
        {

            BaseResponse<string> response = new BaseResponse<string>();

            var lookOverRepository = unitOfWork.GetRepository<LTC_LOOKOVER>();
            LTC_LOOKOVER lookOverRecord = lookOverRepository.Get(id);
            if (lookOverRecord != null)
            {

                if (!string.IsNullOrEmpty(lookOverRecord.LOOKOVERPHOTOS))
                {

                    var imgList = lookOverRecord.LOOKOVERPHOTOS.Split(new char[] { ';' });
                    try
                    {
                        foreach (var file in imgList)
                        {
                            var fileName = HttpContext.Current.Server.MapPath(file);
                            File.Delete(fileName);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                lookOverRecord.ISDETELE = true;
                lookOverRepository.Update(lookOverRecord);
                unitOfWork.Save();
            }
            return response;
        }
        #endregion
    }
}
