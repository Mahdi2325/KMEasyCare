using KM.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.Infrastructure
{
    public class EfLogHelper : IDbCommandInterceptor
    {

        static readonly ConcurrentDictionary<DbCommand, DateTime> MStartTime = new ConcurrentDictionary<DbCommand, DateTime>();
        //记录开始执行时的时间
        private static void OnStart(DbCommand command)
        {
#if DEBUG
            MStartTime.TryAdd(command, DateTime.Now);
#endif
        }
        private static void Log<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {
#if DEBUG
            DateTime startTime;
            TimeSpan duration;
            //得到此command的开始时间
            MStartTime.TryRemove(command, out startTime);
            if (startTime != default(DateTime))
            {
                duration = DateTime.Now - startTime;
            }
            else
                duration = TimeSpan.Zero;

            var sql = command.CommandText;
            //循环获取执行语句的参数值
            foreach (DbParameter param in command.Parameters)
            {
                sql = sql.Replace("@" + param.ParameterName, (param.DbType != DbType.Int16 && param.DbType !=
                    DbType.Int32 && param.DbType != DbType.Int64) ? "'" + param.Value.ToString() + "'" : param.Value.ToString());
            }

            LogHelper.WriteDebug(sql);
#endif
        }
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Log(command, interceptionContext);
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            OnStart(command);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {

            Log(command, interceptionContext);
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            OnStart(command);
        }
        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            Log(command, interceptionContext);
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

            OnStart(command);
        }
    }
}
