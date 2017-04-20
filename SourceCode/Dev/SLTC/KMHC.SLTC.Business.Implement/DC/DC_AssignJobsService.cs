using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.DC.Filter;
using KMHC.SLTC.Business.Entity.DC.Model;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.DC;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Implement.DC
{
    public class DC_AssignJobsService : BaseService, IDC_AssignJobsService
    {
        public BaseResponse<IList<DC_TaskRemind>> QueryTaskList(BaseRequest<DC_AssignJobsFilter> request)
        {
            BaseResponse<IList<DC_TaskRemind>> response = new BaseResponse<IList<DC_TaskRemind>>();
            var q = from n in unitOfWork.GetRepository<DC_TASKREMIND>().dbSet
                    join i in unitOfWork.GetRepository<DC_IPDREG>().dbSet on n.FEENO equals i.FEENO into ip
                    from ipg in ip.DefaultIfEmpty()
                    join r in unitOfWork.GetRepository<DC_REGFILE>().dbSet on ipg.REGNO equals r.REGNO into re
                    from reg in re.DefaultIfEmpty()
                    select new
                    {
                        TaskModel = n,
                        Name = reg.REGNAME
                    };
                    q = q.Where(m => m.TaskModel.ASSIGNEE == request.Data.Assignee);
                    if (request.Data.RecStatus.HasValue)
                    {
                        q = q.Where(m => m.TaskModel.RECSTATUS == request.Data.RecStatus.Value);
                    }
                    if (request.Data.NewRecFlag.HasValue)
                    {
                        q = q.Where(m => m.TaskModel.NEWRECFLAG == request.Data.NewRecFlag.Value);
                    }
                    if (request.Data.AssignStartDate.HasValue)
                    {
                        q = q.Where(m => m.TaskModel.ASSIGNDATE >= request.Data.AssignStartDate.Value);
                    }
                    if (request.Data.AssignEndDate.HasValue)
                    {
                        request.Data.AssignEndDate = request.Data.AssignEndDate.Value.AddDays(1);
                        q = q.Where(m => m.TaskModel.ASSIGNDATE <= request.Data.AssignEndDate);
                    }
                    q = q.OrderByDescending(m => m.TaskModel.ASSIGNDATE).OrderBy(m => m.TaskModel.RECSTATUS);
                    Action<IList> mapperResponse = (IList list) =>
                    {
                        response.Data = new List<DC_TaskRemind>();
                        foreach (dynamic item in list)
                        {
                            DC_TaskRemind newItem = Mapper.DynamicMap<DC_TaskRemind>(item.TaskModel);
                            newItem.ResidentName = item.Name;
                            response.Data.Add(newItem);
                        }

                    };
                 response.RecordsCount = q.Count();
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
        public object QueryAssTask(BaseRequest<AssignTaskJobFilter> request)
        {

            var q = from t in unitOfWork.GetRepository<DC_TASKREMIND>().dbSet.Where(x => x.ASSIGNEE == request.Data.Assignee)
                    select new
                    {
                        id = t.ID,
                        title = t.CONTENT,
                        start = t.PERFORMDATE

                    };

            q = q.Where(m => m.start >= request.Data.start && m.start <= request.Data.end);
            return q;

        }
        public BaseResponse ChangeRecStatus(int id, bool? recStatus, DateTime? finishDate)
        {
            BaseResponse response = new BaseResponse();
            DC_TASKREMIND tr = unitOfWork.GetRepository<DC_TASKREMIND>().dbSet.Where(x => x.ID == id && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            tr.RECSTATUS = recStatus;
            tr.FINISHDATE = finishDate;
            unitOfWork.GetRepository<DC_TASKREMIND>().Update(tr);
            unitOfWork.Commit();
            return response;
        }

        public BaseResponse<DC_TaskRemind> SaveTaskRemind(DC_TaskRemind request)
        {
            return base.Save<DC_TASKREMIND, DC_TaskRemind>(request, (q) => q.ID == request.ID);
        }
        public BaseResponse<IList<DC_TaskEmpModel>> QueryEmpList()
        {
            BaseResponse<IList<DC_TaskEmpModel>> response = new BaseResponse<IList<DC_TaskEmpModel>>();
            var q = from emp in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet.Where(e => e.ORGID == SecurityHelper.CurrentPrincipal.OrgId)
                    join co in unitOfWork.GetRepository<LTC_CODEDTL_REF>().dbSet.Where(c=>c.ITEMTYPE=="E00.013") on emp.EMPGROUP equals co.ITEMCODE into cod
                    from code in cod.DefaultIfEmpty()
                    select new DC_TaskEmpModel
                      {
                          EmpNo = emp.EMPNO,
                          EmpName = emp.EMPNAME,
                          EmpGroup=emp.EMPGROUP,
                          JobTitle=emp.JOBTITLE,
                          EmpGroupName = code.ITEMNAME,
                          Checked=false
                      };

            response.Data = q.ToList();
            return response;
        }

        public BaseResponse SaveAllocateTask(long ID, IList<DC_TaskEmpModel> empList)
        {
            BaseResponse response = new BaseResponse();
           
            DC_TASKREMIND tr = unitOfWork.GetRepository<DC_TASKREMIND>().dbSet.Where(x => x.ID == ID && x.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault();
            List<DC_TASKREMIND> newTaskList=new List<DC_TASKREMIND>();
            foreach(DC_TaskEmpModel t in empList)
            {
                Mapper.CreateMap<DC_TASKREMIND, DC_TaskRemind>();
                DC_TASKREMIND task =new DC_TASKREMIND();
                DC_TaskRemind taskMap = Mapper.Map<DC_TaskRemind>(tr); 
                taskMap.ID = 0;
                taskMap.AssignedBy = SecurityHelper.CurrentPrincipal.EmpNo;
                taskMap.AssignedName = SecurityHelper.CurrentPrincipal.EmpName;
                taskMap.Assignee = t.EmpNo;
                taskMap.AssignName = t.EmpName;
                taskMap.NewRecFlag = true;
                Mapper.CreateMap<DC_TaskRemind,DC_TASKREMIND>();
                task = Mapper.Map<DC_TASKREMIND>(taskMap); 

                newTaskList.Add(task);
            }
            unitOfWork.GetRepository<DC_TASKREMIND>().InsertRange(newTaskList);
            unitOfWork.GetRepository<DC_TASKREMIND>().Delete(tr);
            unitOfWork.Commit();
            return response;

        }

        #region 获取全局变量

        public BaseResponse<GlobalVariable> GetGlobalVariable()
        {
            BaseResponse<GlobalVariable> response = new BaseResponse<GlobalVariable>();
            GlobalVariable data = new GlobalVariable();
            data.CurrentLoginSys = SecurityHelper.CurrentPrincipal.CurrentLoginSys;
            data.Organization = unitOfWork.GetRepository<LTC_ORG>().dbSet.Where(m => m.ORGID == SecurityHelper.CurrentPrincipal.OrgId).FirstOrDefault().ORGNAME;
            data.OrganizationId = SecurityHelper.CurrentPrincipal.OrgId;
            if(SecurityHelper.CurrentPrincipal.CurrentLoginSys=="LC"){
                data.Roles = SecurityHelper.CurrentPrincipal.LTCRoleType;
            }
            else if (SecurityHelper.CurrentPrincipal.CurrentLoginSys == "DC")
            {
                data.Roles = SecurityHelper.CurrentPrincipal.DCRoleType;
            }
        
            if (data.Roles!=null)
            {
                if (data.Roles.Contains(EnumRoleType.SuperAdmin.ToString()))
                {
                    data.MaximumPrivileges = EnumRoleType.SuperAdmin.ToString();
                }
                else if (data.Roles.Contains(EnumRoleType.Admin.ToString()))
                {
                    data.MaximumPrivileges = EnumRoleType.Admin.ToString();
                }
                else
                {
                    data.MaximumPrivileges = data.Roles[0];
                }
            }
         
             response.Data=  data;
             return response;
        }

        #endregion

        #region 工作照会保存

        public BaseResponse SaveAssignWorkNote(DC_ReAllocateTaskModel list)
        {
            BaseResponse response = new BaseResponse();
            List<DC_TASKREMIND> newTaskList = new List<DC_TASKREMIND>();
            DateTime now = DateTime.Now;
            foreach (DC_TaskEmpModel t in list.empList)
            {
                DC_TASKREMIND task = new DC_TASKREMIND();               
                task.ASSIGNEDBY = SecurityHelper.CurrentPrincipal.EmpNo;
                task.ASSIGNEDNAME = SecurityHelper.CurrentPrincipal.EmpName;
                task.ORGID = SecurityHelper.CurrentPrincipal.OrgId;
                task.ASSIGNEE = t.EmpNo;
                task.ASSIGNNAME = t.EmpName;
                task.NEWRECFLAG = true;
                task.CONTENT = list.Content;
                task.PERFORMDATE = list.PerformDate;
                task.ASSIGNDATE = now;
                newTaskList.Add(task);
            }
            unitOfWork.GetRepository<DC_TASKREMIND>().InsertRange(newTaskList);
            unitOfWork.Commit();
            return response;
        }

        #endregion

    }
}
