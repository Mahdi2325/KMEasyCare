/*
创建人: 肖国栋
创建日期:2016-03-09
说明:机构管理
*/
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using System.Collections.Generic;

namespace KMHC.SLTC.Business.Interface
{
    public interface IOrganizationManageService : IBaseService
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Organization>> QueryOrg(BaseRequest<OrganizationFilter> request);
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        BaseResponse<Organization> GetOrg(string orgID);
        /// <summary>
        /// 保存机构
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Organization> SaveOrg(Organization request);
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="orgID"></param>
        BaseResponse DeleteOrg(string orgID);
        /// <summary>
        /// 根据机构id 查询机构no
        /// </summary>
        /// <param name="orgId">机构id</param>
        /// <returns>机构no</returns>
        string QueryOrgnsno(string orgId);
        /// <summary>
        /// 获取楼层基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgFloor>> QueryOrgFloor(BaseRequest<OrgFloorFilter> request);
        /// <summary>
        /// 获取楼层基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgFloor>> QueryOrgFloorExtend(BaseRequest<OrgFloorFilter> request);
        /// <summary>
        /// App获取楼层基本列表,楼层名正序排列
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgFloor>> QueryOrgFloorFromApp(BaseRequest<OrgFloorFilter> request);
        /// <summary>
        /// 获取楼层基本
        /// </summary>
        /// <param name="bedNO"></param>
        /// <returns></returns>
        BaseResponse<OrgFloor> GetOrgFloor(string bedNO);
        /// <summary>
        /// 保存楼层基本
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<OrgFloor> SaveOrgFloor(OrgFloor request);
        /// <summary>
        /// 删除楼层基本
        /// </summary>
        BaseResponse DeleteOrgFloor(string bedNO);
        /// <param name="bedNO"></param>
        /// <summary>
        /// 获取房间基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgRoom>> QueryOrgRoom(BaseRequest<OrgRoomFilter> request);
        /// <summary>
        /// 获取房间基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgRoom>> QueryOrgRoomExtend(BaseRequest<OrgRoomFilter> request);
        /// <summary>
        /// 获取房间基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgRoom>> QueryOrgRoomExtendV2(BaseRequest<OrgRoomFilter> request);
        /// <summary>
        /// 获取房间基本列表 For App
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<OrgRoom>> QueryOrgRoomForApp(BaseRequest<OrgRoomFilter> request);
        /// <summary>
        /// 获取房间基本
        /// </summary>
        /// <param name="bedNO"></param>
        /// <returns></returns>
        BaseResponse<OrgRoom> GetOrgRoom(string bedNO);
        /// <summary>
        /// 保存房间基本
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<OrgRoom> SaveOrgRoom(OrgRoom request);

        /// <summary>
        /// 批量保存房间和床位信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<OrgRoom> SaveOrgRoomAndBeds(OrgRoom request);
        /// <summary>
        /// 删除房间基本
        /// </summary>
        /// <param name="bedNO"></param>
        BaseResponse DeleteOrgRoom(string bedNO);
        /// <summary>
        /// 获取床位基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BedBasic>> QueryBedBasic(BaseRequest<BedBasicFilter> request);
        /// <summary>
        /// 更换床位 add by Duke
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse ChangeBed(ChangeBedModel request);
        /// <summary>
        /// 获取床位基本列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<BedBasic>> QueryBedBasicExtend(BaseRequest<BedBasicFilter> request);
        /// <summary>
        /// 获取床位基本
        /// </summary>
        /// <param name="bedNO"></param>
        /// <returns></returns>
        BaseResponse<BedBasic> GetBedBasic(string bedNO);
        /// <summary>
        /// 保存床位基本
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<BedBasic> SaveBedBasic(BedBasic request);
        /// <summary>
        /// 床位被使用，更新床位状态
        /// </summary>
        /// <param name="request"></param>
        BaseResponse UpdateBedBasic(BedBasic request);
        /// <summary>
        /// 删除床位基本
        /// </summary>
        /// <param name="bedNO"></param>
        BaseResponse DeleteBedBasic(string bedNO);
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<User>> QueryUser(BaseRequest<UserFilter> request);
        /// <summary>
        /// 获取用户扩展信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<User>> QueryUserExtend(BaseRequest<UserFilter> request);
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        BaseResponse<User> GetUser(int userID);
        BaseResponse<User> GetUserwithempno(string EMPNO);

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<User> SaveUser(User request);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="logonName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        BaseResponse ResetPassword(string orgID, string logonName, string password);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="logonName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        BaseResponse ChangePassWord(string orgID, string logonName, string oldPassword, string newPassword);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userID"></param>
        BaseResponse DeleteUser(int userID);
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Role>> QueryRole(BaseRequest<RoleFilter> request);
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        BaseResponse<Role> GetRole(string roleID);
        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Role> SaveRole(Role request);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleID"></param>
        BaseResponse DeleteRole(string roleID);
        /// <summary>
        /// 根据角色获取模块树的数据
        /// </summary>
        /// <param name="requestByRole">选择的模块</param>
        /// <param name="requestByTree">展现的模块</param>
        /// <returns></returns>
        BaseResponse<IList<TreeNode>> GetModuleByRole(BaseRequest<RoleFilter> requestByRole, BaseRequest<RoleFilter> requestByTree);
        /// <summary>
        /// 根据角色类型获取用户
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="roleType"></param>
        /// <returns></returns>
        BaseResponse<List<User>> GetUsreByRoleType(string orgId, string roleType);
        /// <summary>
        /// 校验登录账号 员工是否已关联其他账号，登录名是否重复
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse CheckUserEmpAndLogName(BaseRequest<UserFilter> request);
        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Employee>> QueryEmployee(BaseRequest<EmployeeFilter> request);
        BaseResponse<IList<EmployeeExt>> QueryEmployeeExt(BaseRequest<EmployeeFilter> request);
        /// <summary>
        /// 获取员工联合用户信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Employee>> QueryUserUnionEmp(BaseRequest<EmployeeFilter> request);
        /// <summary>
        /// 获取员工
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        BaseResponse<Employee> GetEmployee(string empNo);
        /// <summary>
        /// 保存员工
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Employee> SaveEmployee(Employee request);
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="empNo"></param>
        BaseResponse DeleteEmployee(string empNo, string orgId);
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Dept>> QueryDept(BaseRequest<DeptFilter> request);
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Dept>> QueryDeptExtend(BaseRequest<DeptFilter> request);
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="deptNo"></param>
        /// <returns></returns>
        BaseResponse<Dept> GetDept(string deptNo);
        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Dept> SaveDept(Dept request);
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptNo"></param>
        BaseResponse DeleteDept(string deptNo, string orgId);

        /// <summary>
        /// 获取集团列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Groups>> QueryGroup(BaseRequest<GroupFilter> request);
        /// <summary>
        /// 获取集团
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        BaseResponse<Groups> GetGroup(string GroupID);
        /// <summary>
        /// 保存集团
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Groups> SaveGroup(Groups request);
        /// <summary>
        /// 删除集团
        /// </summary>
        /// <param name="roleID"></param>
        BaseResponse DeleteGroup(string GroupID);


        /// <summary>
        /// 依据角色Id获取菜单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<Module> GetRoleModule(RoleFilter request);


        /// <summary>
        /// 依据角色Id获取菜单树列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<MenuTree> GetMenus(RoleFilter request);


        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CodeFile>> QueryCodeFile(BaseRequest<CommonFilter> request);
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CodeFile> GetCodeFile(string id);
        /// <summary>
        /// 保存字典
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CodeFile> SaveCodeFile(CodeFile request);
        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteCodeFile(string id);

        /// <summary>
        /// 获取字典小项列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<CodeDtl>> QueryCodeDtl(BaseRequest<CommonFilter> request);

        /// <summary>
        /// 获取字典小项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        BaseResponse<CodeDtl> GetCodeDtl(string id, string type);
        /// <summary>
        /// 保存字典小项
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<CodeDtl> SaveCodeDtl(CodeDtl request);

        /// <summary>
        /// 删除字典小项
        /// </summary>
        /// <param name="id"></param>
        int DeleteCodeDtl(string id, string type);



        /// <summary>
        /// 获取院内公告列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<Notice>> QueryNotices(BaseRequest<NoticeFilter> request);

        /// <summary>
        /// 获取院内公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<Notice> GetNotice(int id);

        /// <summary>
        /// 保存公告
        /// </summary>
        /// <param name="request"></param>
        BaseResponse<Notice> SaveNotice(Notice request);
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id"></param>
        BaseResponse DeleteNotice(int id);

        /// <summary>
        /// 查询评估量表List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_Question>> QueryQueList(BaseRequest<QuestionFilter> request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_Question>> QueryEvalTempSetList(BaseRequest<QuestionFilter> request);
        /// <summary>
        /// 获取问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<LTC_Question> GetQue(int id);
        /// <summary>
        /// 评估问题List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_MakerItem>> QueryMakerItemList(BaseRequest<MakerItemFilter> request);
        /// <summary>
        /// 评估结果List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<IList<LTC_QuestionResults>> QueryQuestionResultsList(BaseRequest<QuestionResultsFilter> request);
        /// <summary>
        /// 保存评估量表设置
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<List<EvalTempSetModel>> SaveEvalTemplateSet(string orgId, List<EvalTempSetModel> request);
        /// <summary>
        /// 保存评估表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_Question> SaveQuestion(LTC_Question request);

        /// <summary>
        /// 导入问题数据
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        BaseResponse SaveQuestionModleData(int questionId, string orgId, int exportQuestionId);
        /// <summary>
        /// 导入结果数据
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="orgId"></param>
        /// <param name="exportQuestionId"></param>
        /// <returns></returns>
        BaseResponse SaveResultModleData(int questionId, string orgId, int exportQuestionId);
        /// <summary>
        /// 保存评估问题数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_MakerItem> SaveMakerItem(LTC_MakerItem request);
        /// <summary>
        /// 保存评估结果数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_QuestionResults> SaveQuestionResults(LTC_QuestionResults request);
        /// <summary>
        /// 保存答案数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResponse<LTC_MakerItemLimitedValue> SaveAnswer(LTC_MakerItemLimitedValue request);
        /// <summary>
        /// 删除评估结果数据
        /// </summary>
        /// <param name="ResultId"></param>
        /// <returns></returns>
        BaseResponse DeleteQuestionResults(int ResultId);
        /// <summary>
        /// 删除评估问题答案
        /// </summary>
        /// <param name="LimitedValueId"></param>
        /// <returns></returns>
        BaseResponse DeleteAnswer(int LimitedValueId);

        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="MakerId"></param>
        /// <returns></returns>
        BaseResponse DeleteMakerItem(int MakerId);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        BaseResponse DeleteQuestion(int Id);


        #region LoginLog

        BaseResponse SaveUserLoginLog(LTC_UserLoginLog request);
        #endregion

        #region 根据nsno获取ltc_ipdreg
        object GetIpdByNsno(string nsno); 
        #endregion

    }

}

