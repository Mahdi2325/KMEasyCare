using AutoMapper;
using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using KMHC.SLTC.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using EntityFramework.Extensions;

namespace KMHC.SLTC.Business.Implement
{
    public class BaseService : IBaseService
    {
        private const string SoftDeleteProperty = "ISDELETE";
        private const string UpdateByProperty = "UPDATEBY";
        private const string UpdateTimeteProperty = "UPDATETIME";

        public IUnitOfWork unitOfWork = IOCContainer.Instance.Resolve<IUnitOfWork>();

        public virtual BaseResponse<IList<T>> Query<S, T>(BaseRequest request, Func<IQueryable<S>, IQueryable<S>> whereAndOrderBy) where S : class
        {
            BaseResponse<IList<T>> response = new BaseResponse<IList<T>>();
            Mapper.CreateMap<S, T>();
            var q = from m in unitOfWork.GetRepository<S>().dbSet
                    select m;

            if (whereAndOrderBy != null)
            {
                q = whereAndOrderBy(q);
            }
            response.RecordsCount = q.Count();
            List<S> list = null;
            if (request != null && request.PageSize > 0)
            {
                list = q.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.PagesCount = GetPagesCount(request.PageSize, response.RecordsCount);
            }
            else
            {
                list = q.ToList();
            }

            response.Data = Mapper.Map<IList<T>>(list);
            return response;
        }

        internal object Query<T1, T2>(object request, Func<IQueryable<LTC_BILLV2>, IQueryable<LTC_BILLV2>> p)
        {
            throw new NotImplementedException();
        }

        public virtual BaseResponse<T> Get<S, T>(Func<S, bool> where)
            where S : class
            where T : class
        {
            Mapper.CreateMap<S, T>();
            BaseResponse<T> response = new BaseResponse<T>();
            var findItem = unitOfWork.GetRepository<S>().dbSet.FirstOrDefault(where);
            if (findItem != null)
            {
                response.Data = Mapper.Map<T>(findItem);
            }
            return response;
        }

        public virtual BaseResponse<IList<T>> GetList<S, T>(Func<S, bool> where)
            where S : class
            where T : class
        {
            Mapper.CreateMap<S, T>();
            BaseResponse<IList<T>> response = new BaseResponse<IList<T>>();
            var findItem = unitOfWork.GetRepository<S>().dbSet.Where(where);
            if (findItem != null)
            {
                response.Data = Mapper.Map<IList<T>>(findItem);
            }
            return response;
        }

        public BaseResponse<T> Save<S, T>(T request, Func<S, bool> where)
            where S : class
            where T : class
        {
            return this.Save<S, T>(request, where, null, false);
        }

        public BaseResponse<T> Save<S, T>(T request, Func<S, bool> where, List<string> fields)
            where S : class
            where T : class
        {
            return this.Save<S, T>(request, where, fields, false);
        }

        public BaseResponse<T> Save<S, T>(T request, Func<S, bool> where, List<string> fields, bool reverse)
            where S : class
            where T : class
        {
            BaseResponse<T> response = new BaseResponse<T>();
            Mapper.Reset();
            var cm = Mapper.CreateMap<T, S>();
            if (fields != null)
            {
                if (reverse)
                {
                    cm.ForAllMembers(it => it.Condition(m => !fields.Contains(m.PropertyMap.SourceMember.Name)));
                }
                else
                {
                    cm.ForAllMembers(it => it.Condition(m => fields.Contains(m.PropertyMap.SourceMember.Name)));
                }
            }
            Mapper.CreateMap<S, T>();
            var model = unitOfWork.GetRepository<S>().dbSet.FirstOrDefault(where);
            if (model == null)
            {
                model = Mapper.Map<S>(request);
                unitOfWork.GetRepository<S>().Insert(model);
            }
            else
            {
                Mapper.Map(request, model);
                unitOfWork.GetRepository<S>().Update(model);
            }
            unitOfWork.Save();
            Mapper.Map(model, request);
            response.Data = request;
            return response;
        }

        public BaseResponse<IList<T>> Save<S, T>(IList<T> request, Func<S, bool> where, List<string> fields = null, bool reverse = false)
            where S : class
            where T : class
        {
            BaseResponse<IList<T>> response = new BaseResponse<IList<T>>();
            var cm = Mapper.CreateMap<T, S>();
            if (fields != null)
            {
                if (reverse)
                {
                    cm.ForAllMembers(it => it.Condition(m => !fields.Contains(m.PropertyMap.SourceMember.Name)));
                }
                else
                {
                    cm.ForAllMembers(it => it.Condition(m => fields.Contains(m.PropertyMap.SourceMember.Name)));
                }
            }
            Mapper.CreateMap<S, T>();
            foreach (var item in request)
            {
                var model = unitOfWork.GetRepository<S>().dbSet.FirstOrDefault(where);
                if (model == null)
                {
                    model = Mapper.Map<S>(item);
                    unitOfWork.GetRepository<S>().Insert(model);
                }
                else
                {
                    Mapper.Map(item, model);
                    unitOfWork.GetRepository<S>().Update(model);
                }
            }
            unitOfWork.Save();
            response.Data = request;
            return response;
        }



        public virtual BaseResponse Delete<S>(object key) where S : class
        {
            BaseResponse response = new BaseResponse();
            unitOfWork.GetRepository<S>().Delete(key);
            unitOfWork.Save();
            return response;
        }


        public virtual int Delete<S>(Expression<Func<S, bool>> filter) where S : class
        {
            var rep = unitOfWork.GetRepository<S>();
            var result = rep.Delete(filter);
            unitOfWork.Save();
            return result;
        }

        public virtual BaseResponse SoftDelete<S>(object key) where S : class
        {
            BaseResponse response = new BaseResponse();

            var softDeleteProperty = typeof(S).GetProperty(SoftDeleteProperty);
            var updateByProperty = typeof(S).GetProperty(UpdateByProperty);
            var updateTimeProperty = typeof(S).GetProperty(UpdateTimeteProperty);
            if (null == softDeleteProperty
                || null == updateByProperty
                || null == updateTimeProperty)
            {
                throw new ArrayTypeMismatchException(string.Format("{0} has no SoftDelete properties, can not be SoftDelete!", typeof(S).Name));
            }

            var toSoftDelete = unitOfWork.GetRepository<S>().Get(key);

            softDeleteProperty.SetValue(toSoftDelete, true);
            updateByProperty.SetValue(toSoftDelete, SecurityHelper.CurrentPrincipal.EmpNo.ToString());
            updateTimeProperty.SetValue(toSoftDelete, DateTime.Now);

            unitOfWork.GetRepository<S>().Update(toSoftDelete);
            unitOfWork.Save();
            return response;
        }


        public string GenerateCode(string orgId, EnumCodeKey key)
        {
            BaseRequest<object> request = new BaseRequest<object>();
            BaseResponse<CodeRule> response = null;
            response = this.Get<SYS_CODERULE, CodeRule>((q) => q.ORGID == orgId && q.CODEKEY == key.ToString());

            if (response.Data == null)
            {
                if (GenerateRule(orgId, key))
                {
                    response = this.Get<SYS_CODERULE, CodeRule>((q) => q.ORGID == orgId && q.CODEKEY == key.ToString());
                }
                else
                {
                    throw new Exception("编码查找创建失败!");
                }
            }
            else
            {
                if (response.Data.FlagRule == "1")
                {
                    //返回原值
                    return ReplaceValue(response.Data.GenerateRule, response.Data.SerialNumber);
                }
            }
            var flag = this.ReplaceValue(response.Data.FlagRule);
            //if (string.IsNullOrEmpty(response.Data.Flag) || response.Data.Flag != flag)
            //{
            //    response.Data.OrgId = orgId;
            //    response.Data.Flag = flag;
            //    response.Data.SerialNumber = 1;
            //}
            response.Data.SerialNumber++;
            unitOfWork.BeginTransaction();
            this.Save<SYS_CODERULE, CodeRule>(response.Data, (q) => q.ORGID == orgId && q.CODEKEY == response.Data.CodeKey);
            unitOfWork.Commit();

            return ReplaceValue(response.Data.GenerateRule, response.Data.SerialNumber);
        }

        /// <summary>
        /// 数据库未创建规则时,可在此方法中创建
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool GenerateRule(string orgId, EnumCodeKey key)
        {
            bool isGenerated = false;
            SYS_CODERULE rule = new SYS_CODERULE();
            switch (key)
            {
                case EnumCodeKey.LCRegNo:
                    rule.ORGID = "LCRegNo";
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:8}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 10;
                    isGenerated = true;
                    break;
                case EnumCodeKey.DCRegNo:
                    rule.ORGID = "DCRegNo";
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:8}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 10;
                    isGenerated = true;
                    break;
                case EnumCodeKey.NCIEvaluate:
                    rule.ORGID = "NCIEvaluate";
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:11}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 10;
                    isGenerated = true;
                    break;
                case EnumCodeKey.OrgId:
                    rule.ORGID = "ProduceOrg";
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:10}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.RoleId:
                    rule.ORGID = orgId;  // orgId="RoleId"
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "R{number:7}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.QuestionId:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:4}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 13;
                    isGenerated = true;
                    break;
                case EnumCodeKey.LimitedId:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:4}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1468;
                    isGenerated = true;
                    break;
                case EnumCodeKey.EmpSysPre:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "EMP{number:3}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.EmpPrefix:
                    string orgEmpSeriaNo = GenerateCode("EmpSysPre", EnumCodeKey.EmpSysPre);
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "1";
                    rule.GENERATERULE = orgEmpSeriaNo;
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = null;
                    isGenerated = true;
                    break;
                case EnumCodeKey.StaffId:
                    rule.ORGID = orgId;  // 集团ID
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:4}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.RoomId:
                    rule.ORGID = "RoomId";  // 房间ID
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "R{number:8}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.FloorId:
                    rule.ORGID = "FloorId";  // 房间ID
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "F{number:4}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.LeaveHospId:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:10}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.BillNo:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:9}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.GoodNo:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:9}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.GoodsLoanNo:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:9}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.GoodsSaleNo:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:9}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.ManufactureNo:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:9}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.PayBillNo:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:9}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.ChargeGroupNo:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:9}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
                case EnumCodeKey.MeaSuredRecordId:
                    rule.ORGID = orgId;
                    rule.CODEKEY = key.ToString();
                    rule.FLAGRULE = "0";
                    rule.GENERATERULE = "{number:11}";
                    rule.FLAG = "0";
                    rule.SERIALNUMBER = 1;
                    isGenerated = true;
                    break;
            }

            if (isGenerated)
            {
                unitOfWork.GetRepository<SYS_CODERULE>().dbSet.Add(rule);
                unitOfWork.Save();
            }
            return isGenerated;
        }


        /// <summary>
        /// 例如生成 D201601-0001
        /// </summary>
        /// <param name="formatString">D{time:yyyyMM}-{number:4}</param>
        /// <param name="serialNumber">0</param>
        /// <returns></returns>
        private string ReplaceValue(string formatString, decimal serialNumber = 0)
        {
            const string pattern = @"(?<={)[\w:]+(?=})";
            var ms = Regex.Matches(formatString, pattern);
            foreach (Match m in ms)
            {
                var arr = m.Value.Split(':');
                if (arr.Length == 2)
                {
                    var value = string.Empty;
                    switch (arr[0].ToLower())
                    {
                        //case "left":
                        //    value = inputValue.Substring(0, int.Parse(arr[1]));
                        //    break;
                        case "time":
                            value = DateTime.Now.ToString(arr[1]);
                            break;
                        case "number":
                            value = serialNumber.ToString().PadLeft(int.Parse(arr[1]), '0');
                            break;
                    }
                    formatString = formatString.Replace("{" + m.Value + "}", value);
                }
            }
            return formatString;
        }


        public int GetPagesCount(int pageSize, int total)
        {
            if (pageSize <= 0)
            {
                return 1;
            }
            var count = total / pageSize;
            if (total % pageSize > 0)
            {
                count += 1;
            }
            return count;
        }

    }
}

