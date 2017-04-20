using AutoMapper;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using KMHC.SLTC.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
   测试类
   孙伟 新增新桌面类
 */

namespace KMHC.SLTC.Business.Implement {

	public class myDeskService : BaseService, IMyDeskService 
	{

		/// <summary>
		/// 获取住民信息列表并返回记录数
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<Resident>> QueryResidentList() {

			BaseResponse<IList<Resident>> response = new BaseResponse<IList<Resident>>();
			var q = from n in unitOfWork.GetRepository<LTC_REGFILE>().dbSet
					join e in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on n.REGNO equals e.REGNO
					select new {
						regFile = n,
						ipdReg = e
					};
			q = q.Where(m => m.regFile.ORGID == SecurityHelper.CurrentPrincipal.OrgId && m.ipdReg.IPDFLAG == "I");
			response.RecordsCount = q.Count();
			return response;
		}

		/// <summary>
		/// 获取非计划住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_UNPLANEDIPD>> QueryUnPlanList() {

			BaseResponse<IList<LTC_UNPLANEDIPD>> response = new BaseResponse<IList<LTC_UNPLANEDIPD>>();

			var q = from n in unitOfWork.GetRepository<LTC_UNPLANEDIPD>().dbSet
					join e in unitOfWork.GetRepository<LTC_REGHEALTH>().dbSet on n.FEENO equals e.FEENO
					select new {
						UnPlanFile = n,
						regHealth = e
					};
			q = q.Where(m => m.UnPlanFile.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();
			return response;

		}

		/// <summary>
		/// 获取请假住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_LEAVEHOSP>> QueryLeaveHospList() 
		{
			BaseResponse<IList<LTC_LEAVEHOSP>> response = new BaseResponse<IList<LTC_LEAVEHOSP>>();

			var q = from n in unitOfWork.GetRepository<LTC_LEAVEHOSP>().dbSet
					select new {
						LeaveHosp = n
						
					};
			q = q.Where(m => m.LeaveHosp.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();
			return response;
		}

		/// <summary>
		/// 获取跌倒指标住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_FALLINCIDENTEVENT>> QueryFallPersonList() {

			BaseResponse<IList<LTC_FALLINCIDENTEVENT>> response = new BaseResponse<IList<LTC_FALLINCIDENTEVENT>>();

			var q = from n in unitOfWork.GetRepository<LTC_FALLINCIDENTEVENT>().dbSet
					join e in unitOfWork.GetRepository<LTC_EMPFILE>().dbSet on n.RECORDBY equals e.EMPNO
					select new {
						FALLIN = n,

					};
			q = q.Where(m => m.FALLIN.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();
			return response;
		}
		/// <summary>
		///  获取压疮指标住民
		/// </summary>
		/// <returns></returns>
		public BaseResponse<IList<LTC_BEDSOREREC>> QueryPrePersonList() {
			BaseResponse<IList<LTC_BEDSOREREC>> response = new BaseResponse<IList<LTC_BEDSOREREC>>();

			var q = from n in unitOfWork.GetRepository<LTC_BEDSOREREC>().dbSet
					select new {
						Pre = n
					};
			q = q.Where(m => m.Pre.ORGID == SecurityHelper.CurrentPrincipal.OrgId);
			response.RecordsCount = q.Count();

			return response;
		}

		/// <summary>
		/// 查询工作照会，并返回工作照会的数量
		/// </summary>
		/// <returns></returns>
        public object QueryAssTask(BaseRequest<AssignTaskFilterByBobDu> request)
		{

            var q = from n in unitOfWork.GetRepository<LTC_ASSIGNTASK>().dbSet
                    join ipd in unitOfWork.GetRepository<LTC_IPDREG>().dbSet on n.FEENO equals ipd.FEENO into ipd_Reg
                    from ipdReg in ipd_Reg.DefaultIfEmpty()
                    join regf in unitOfWork.GetRepository<LTC_REGFILE>().dbSet on ipdReg.REGNO equals regf.REGNO into reg_f
                    from reg_file in reg_f.DefaultIfEmpty()
                    where n.ASSIGNEE == "" + SecurityHelper.CurrentPrincipal.EmpNo + ""
                    select new
                    {
                        id = n.ID,
                        title = reg_file.NAME == null ? n.CONTENT : "(" + reg_file.NAME + ")" + n.CONTENT,
                        start=n.PERFORMDATE,
                        regName=reg_file.NAME
                    };
           
            q = q.Where(m => m.start>= request.Data.start&&m.start<=request.Data.end);
            return q;

		}
        public object QueryKPI()
        {
            string sql = @"SELECT FEENO ,'I'as type  FROM LTC_IPDREG
where IPDFLAG='I' and ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + 
"' AND FEENO>0 UNION ALL  SELECT DISTINCT FEENO ,'L'as type FROM LTC_LEAVEHOSP WHERE ORGID='" + SecurityHelper.CurrentPrincipal.OrgId +
"' AND FEENO>0 and ((STARTDATE<=NOW()AND (RETURNDATE>=NOW()OR RETURNDATE IS NULL))) UNION ALL SELECT DISTINCT FEENO ,'U'as type FROM LTC_UNPLANEDIPD WHERE  OUTFLAG=0  and ORGID='"
+ SecurityHelper.CurrentPrincipal.OrgId +
"' AND FEENO>0 UNION ALL SELECT DISTINCT FEENO ,'F'as type FROM LTC_FALLINCIDENTEVENT WHERE date_format(EVENTDATE, '%Y-%m') = date_format(NOW(), '%Y-%m') and ORGID='" 
+ SecurityHelper.CurrentPrincipal.OrgId +
"' AND FEENO>0 UNION ALL SELECT DISTINCT FEENO ,'B'as type FROM LTC_BEDSOREREC WHERE date_format(OCCURDATE, '%Y-%m') = date_format(NOW(), '%Y-%m')  and ORGID='" 
+ SecurityHelper.CurrentPrincipal.OrgId + "'  AND FEENO>0 UNION ALL SELECT CASE  WHEN isnull(a.FEENO) THEN 0 ELSE a.FEENO END  FEENO ,'A'as type FROM LTC_ASSIGNTASK a left JOIN LTC_IPDREG i ON a.FEENO = i.FEENO left JOIN LTC_REGFILE r ON i.REGNO = r.REGNO where a.ASSIGNEE='" 
+ SecurityHelper.CurrentPrincipal.EmpNo + "' and a.NEWRECFLAG=1 and a.ORGID='" + SecurityHelper.CurrentPrincipal.OrgId + "' ;";
           List<MyDeskKPI> list=  unitOfWork.GetRepository<MyDeskKPI>().SqlQuery(sql).ToList();
           return list;
        }
	}
}

