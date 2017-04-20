using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace kmhc.hr.infrastructure.Core
{
    /// <summary>
    /// <para>Copyright (C) 2015 康美健康云服务有限公司版权所有</para>
    /// <para>文 件 名： ObjectFactory.cs</para>
    /// <para>文件功能： 通过IOC创建对象</para>
    /// <para>开发部门： 平台部</para>
    /// <para>创 建 人： lmf</para>
    /// <para>创建日期： 2015.10.19</para>
    /// <para>修 改 人： </para>
    /// <para>修改日期： </para>
    /// <para>备    注： </para>
    /// </summary>
    public class ObjectFactory
    {
        private ObjectFactory()
        {
        }

        public static T GetType<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }

        public static object GetType(Type type)
        {
            return DependencyResolver.Current.GetService(type);
        }

        public static IEnumerable<T> GetTypes<T>()
        {
            return DependencyResolver.Current.GetServices<T>();
        }

        public static IEnumerable GetTypes(Type type)
        {
            return DependencyResolver.Current.GetServices(type);
        }
    }
}
