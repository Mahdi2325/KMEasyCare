using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.SLTC.Business.Interface {

 public interface IMyDeskService:IBaseService 
 {

	 #region 桌面数量查询接口

	 /// <summary>
     /// 获取当前入住人数统计
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<Resident>> QueryResidentList();

	 /// <summary>
	 /// 非计划住民列表，并返回非计划住民人数
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_UNPLANEDIPD>> QueryUnPlanList();


	 /// <summary>
	 /// 获取请假人数，并返回请假住民人数
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_LEAVEHOSP>> QueryLeaveHospList();

	 /// <summary>
	 /// 获取跌倒住名列表，并返回住民列表数
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_FALLINCIDENTEVENT>> QueryFallPersonList();


	 /// <summary>
	 /// 获取压疮住民列表，并返回住民列表数
	 /// </summary>
	 /// <returns></returns>
	 BaseResponse<IList<LTC_BEDSOREREC>> QueryPrePersonList();


	 /// <summary>
	 /// 查询工作照会,并返回数量
	 /// </summary>
	 /// <returns></returns>
     object QueryAssTask(BaseRequest<AssignTaskFilterByBobDu> request);

     object QueryKPI();

	 #endregion


	 #region 几张图形分析绑定接口




	 #endregion

 }

}

