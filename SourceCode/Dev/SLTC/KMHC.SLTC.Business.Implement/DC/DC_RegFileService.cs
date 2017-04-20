/*************************************************************************************************
 * 描述:日间照顾-社工
 * 创建日期:2016-5-9
 * 创建人:杨金高
 * **********************************************************************************************/
using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Data;
using System.Text;

namespace KMHC.SLTC.Business.Implement
{
    public class DC_RegFileService : BaseService,IDC_RegFileService
    {
        #region***********************日照部分--社工*********************
        /// <summary>
        /// 获取个案基本资料列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<IList<DC_RegFileModel>> QueryPersonBasic(BaseRequest<DC_RegFileFilter> request)
        {
            BaseResponse<IList<DC_RegFileModel>> response = new BaseResponse<IList<DC_RegFileModel>>();
            var q = from reg in unitOfWork.GetRepository<DC_REGFILE>().dbSet
                   // where d.REGNO == request.Data.RegNo
                    join ipd in unitOfWork.GetRepository<DC_IPDREG>().dbSet on reg.REGNO equals ipd.REGNO into ipds
                    //join emp2 in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on d.NEXTAPPLYBY equals emp2.EMPNO into res2
                    from ipd_reg in ipds.DefaultIfEmpty()
                    //from em2 in res2.DefaultIfEmpty()
                    select new DC_RegFileModel
                    {
                       OrgId=reg.ORGID,
                       RegNo=reg.REGNO,
                       RegName=reg.REGNAME,
                       ResidentNo=ipd_reg.RESIDENTNO,
                       Sex=reg.SEX,
                       BirthPlace=reg.BIRTHPLACE,
                       OriginPlace=reg.ORIGINPLACE,
                       BirthDate=reg.BIRTHDATE,
                       IdNo=reg.IDNO,
                       PType=reg.PTYPE,
                       Phone=reg.PHONE,
                       LivingAddress=reg.LIVINGADDRESS,
                       PermanentAddress=reg.PERMANENTADDRESS,
                       Language=reg.LANGUAGE,
                       Education=reg.EDUCATION,
                       LivCondition=reg.LIVCONDITION,
                       MerryState=reg.MERRYSTATE,
                       Profession=reg.PROFESSION,
                       Religion=reg.RELIGION,
                       EconomicSources=reg.ECONOMICSOURCES,
                       SourceType=reg.SOURCETYPE,
                       ObstacleManual=reg.OBSTACLEMANUAL,
                       DiseaseInfo=reg.DISEASEINFO,
                       SuretyName=reg.SURETYNAME,
                       SuretyAge=reg.SURETYAGE,
                       SuretyUnit=reg.SURETYUNIT,
                       SuretyAddress=reg.SURETYADDRESS,
                       SuretyPhone=reg.SURETYPHONE,
                       ContactName1=reg.CONTACTNAME1,
                       ContactAge1=reg.CONTACTAGE1,
                       ContactAddress1=reg.CONTACTADDRESS1,
                       ContactPhone1=reg.CONTACTPHONE1,
                       ContactName2 = reg.CONTACTNAME2,
                       ContactAge2 = reg.CONTACTAGE2,
                       ContactAddress2 = reg.CONTACTADDRESS2,
                       ContactPhone2 = reg.CONTACTPHONE2,
                       EcologicalMap=reg.ECOLOGICALMAP,
                       InDate=ipd_reg.INDATE
                    };
            if (!string.IsNullOrEmpty(request.Data.OrgId))
                q = q.Where(m => m.OrgId == request.Data.OrgId);
            if (!string.IsNullOrEmpty(request.Data.RegNo))
                q = q.Where(m => m.RegNo == request.Data.RegNo);
            q = q.OrderByDescending(m => m.RegNo);
            response.RecordsCount = q.Count();
            //Action<IList> mapperResponse = (IList list) =>
            //{
            //    response.Data = new List<DC_RegFileModel>();
            //    foreach (dynamic item in list)
            //    {
            //        DC_RegFileModel newItem = Mapper.DynamicMap<DC_RegFileModel>(item.DC_RegFileModel);
            //        //newItem.ApplyByName = item.ApplyByName;
            //        //newItem.NextApplyByName = item.NextApplyByName;
            //        response.Data.Add(newItem);
            //    }

            //    DateTime dt = DateTime.Now;

            //};
            if (request != null && request.PageSize > 0)
            {
                //var list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.Data = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
               // mapperResponse(list);
            }
            else
            {
                //var list = q.ToList();
                //mapperResponse(list);
                response.Data = q.ToList();
            }
            return response;
        }

        /// <summary>
        /// 获取个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegFileModel> GetPersonBasicById(string id)
        {
            return base.Get<DC_REGFILE, DC_RegFileModel>((q) => q.REGNO == id);
        }

        /// <summary>
        /// 保存个案基本资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<DC_RegFileModel> SavePersonBasic(DC_RegFileModel request)
        {
            return base.Save<DC_REGFILE, DC_RegFileModel>(request, (q) => q.REGNO == request.RegNo);
        }

        /// <summary>
        /// 删除个案基本资料
        /// </summary>
        /// <param name="regNo"></param>
        /// <returns></returns>
        public BaseResponse DeletePersonBasicById(string id)
        {
            return base.Delete<DC_REGFILE>(id);
        }
        #endregion********************************************
    }
}

