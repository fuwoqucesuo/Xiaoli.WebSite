using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
//using System.Data.SQLite;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace xiaoliweb.DBhelper
{
    /// <summary>数据访问封装</summary>
    public static class DbHelper
    {
        /// <summary>null转换成DBNull</summary>
        /// <param name="value">数据库参数值</param>
        /// <returns>数据库安全的参数值</returns>
        [Obsolete("不再需要，请移除！", true)]
        public static object GetDbValue(object value)
        {//DbHelper\.GetDbValue\((.+)?\) $1
            return value == null ? DBNull.Value : value;
        }

        #region CreateDbHelper
        /// <summary>创建默认的数据库连接</summary>
        public static BaseDbHelper CreateDefaultDbHelper()
        {
            return CreateDefaultDbHelper(false);
        }
        /// <summary>创建默认的数据库连接</summary>
        /// <param name="readOnly">是否连接到只读库，默认为false</param>
        public static BaseDbHelper CreateDefaultDbHelper(bool readOnly)
        {
            return new SqlDbHelper(readOnly);
        }

        public static BaseDbHelper CreateCacheDbHelper()
        {
            return new SqlDbHelper(ConfigurationManager.ConnectionStrings["Cache"].ConnectionString);
        }

        [Obsolete("请使用CreateDefaultDbHelper(true)或第一个参数使用true", true)]
        public static BaseDbHelper CreateReadOnlyDbHelper()
        {
            return new SqlDbHelper(true);
        }

        public static BaseDbHelper CreateAliyunDbHelper()
        {
            return new SqlDbHelper(ConfigurationManager.ConnectionStrings["Aliyun"].ConnectionString);
        }

        //public static BaseDbHelper CreateMySqlDbHelper()
        //{
        //    return new MySqlDbHelper();
        //}

        public static BaseDbHelper CreateDbHelper(string key = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                var dbs = Settings.Load<DbSettings>();
                if (dbs != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbs.DefConnstr))
                    {
                        return new SqlDbHelper(dbs.DefConnstr);
                    }
                    else if (!string.IsNullOrWhiteSpace(dbs.DefaultKey))
                    {
                        return new SqlDbHelper(ConfigurationManager.ConnectionStrings[dbs.DefaultKey].ConnectionString);
                    }
                }
                return new SqlDbHelper();
            }
            else
            {
                return new SqlDbHelper(ConfigurationManager.ConnectionStrings[key].ConnectionString);
            }
        }

        public static BaseDbHelper GetDbHelper(string key = null)
        {
            return new SqlDbHelper(ConfigurationManager.ConnectionStrings[key].ConnectionString);
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(string cmdString)
        {
            return ExecuteNonQuery(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteNonQuery(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteNonQuery(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteNonQuery(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteNonQuery(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(DbCommand cmd)
        {
            return ExecuteNonQuery(false, cmd);
        }
        #endregion

        #region ExecuteNonQuery ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(bool readOnly, string cmdString)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteNonQuery(cmdString);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteNonQuery(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteNonQuery(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteNonQuery(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteNonQuery(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>受影响的行数。</returns>
        public static int ExecuteNonQuery(bool readOnly, DbCommand cmd)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteNonQuery(cmd);
            }
        }
        #endregion

        #region ExecuteNonQueryAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(string cmdString)
        {
            return ExecuteNonQueryAsync(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteNonQueryAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteNonQueryAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteNonQueryAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteNonQueryAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(DbCommand cmd)
        {
            return ExecuteNonQueryAsync(false, cmd);
        }
        #endregion

        #region ExecuteNonQueryAsync ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(bool readOnly, string cmdString)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteNonQueryAsync(cmdString).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteNonQueryAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteNonQueryAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteNonQueryAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteNonQueryAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>受影响的行数。</returns>
        public static Task<int> ExecuteNonQueryAsync(bool readOnly, DbCommand cmd)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteNonQueryAsync(cmd).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        #endregion
        #region ExecuteScalar
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(string cmdString)
        {
            return ExecuteScalar(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteScalar(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteScalar(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteScalar(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteScalar(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(DbCommand cmd)
        {
            return ExecuteScalar(false, cmd);
        }
        #endregion

        #region ExecuteScalar ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(bool readOnly, string cmdString)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteScalar(cmdString);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteScalar(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteScalar(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteScalar(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteScalar(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static object ExecuteScalar(bool readOnly, DbCommand cmd)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteScalar(cmd);
            }
        }
        #endregion

        #region ExecuteScalarAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(string cmdString)
        {
            return ExecuteScalarAsync(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteScalarAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteScalarAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteScalarAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteScalarAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(DbCommand cmd)
        {
            return ExecuteScalarAsync(false, cmd);
        }
        #endregion

        #region ExecuteScalarAsync ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(bool readOnly, string cmdString)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteScalarAsync(cmdString).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteScalarAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteScalarAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteScalarAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteScalarAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public static Task<object> ExecuteScalarAsync(bool readOnly, DbCommand cmd)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteScalarAsync(cmd).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        #endregion
        #region ExecuteDataSet
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(string cmdString)
        {
            return ExecuteDataSet(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteDataSet(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataSet(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteDataSet(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataSet(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(DbCommand cmd)
        {
            return ExecuteDataSet(false, cmd);
        }
        #endregion

        #region ExecuteDataSet ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(bool readOnly, string cmdString)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataSet(cmdString);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataSet(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataSet(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataSet(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataSet(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>多个查询结果。</returns>
        public static DataSet ExecuteDataSet(bool readOnly, DbCommand cmd)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataSet(cmd);
            }
        }
        #endregion

        #region ExecuteDataSetAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(string cmdString)
        {
            return ExecuteDataSetAsync(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteDataSetAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataSetAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteDataSetAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataSetAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(DbCommand cmd)
        {
            return ExecuteDataSetAsync(false, cmd);
        }
        #endregion

        #region ExecuteDataSetAsync ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(bool readOnly, string cmdString)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataSetAsync(cmdString).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataSetAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataSetAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataSetAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataSetAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>多个查询结果。</returns>
        public static Task<DataSet> ExecuteDataSetAsync(bool readOnly, DbCommand cmd)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataSetAsync(cmd).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        #endregion
        #region ExecuteDataTable
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(string cmdString)
        {
            return ExecuteDataTable(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteDataTable(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataTable(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteDataTable(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataTable(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(DbCommand cmd)
        {
            return ExecuteDataTable(false, cmd);
        }
        #endregion

        #region ExecuteDataTable ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(bool readOnly, string cmdString)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataTable(cmdString);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataTable(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataTable(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataTable(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataTable(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static DataTable ExecuteDataTable(bool readOnly, DbCommand cmd)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataTable(cmd);
            }
        }
        #endregion

        #region ExecuteDataTableAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(string cmdString)
        {
            return ExecuteDataTableAsync(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteDataTableAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataTableAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteDataTableAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataTableAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(DbCommand cmd)
        {
            return ExecuteDataTableAsync(false, cmd);
        }
        #endregion

        #region ExecuteDataTableAsync ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(bool readOnly, string cmdString)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataTableAsync(cmdString).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataTableAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataTableAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataTableAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataTableAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static Task<DataTable> ExecuteDataTableAsync(bool readOnly, DbCommand cmd)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataTableAsync(cmd).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        #endregion
        #region ExecuteDataRow
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(string cmdString)
        {
            return ExecuteDataRow(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteDataRow(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataRow(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteDataRow(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataRow(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(DbCommand cmd)
        {
            return ExecuteDataRow(false, cmd);
        }
        #endregion

        #region ExecuteDataRow ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(bool readOnly, string cmdString)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataRow(cmdString);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataRow(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataRow(cmdString, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataRow(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataRow(cmdString, commandType, parameters);
            }
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static DataRow ExecuteDataRow(bool readOnly, DbCommand cmd)
        {
            using (var dbHelper = CreateDefaultDbHelper(readOnly))
            {
                return dbHelper.ExecuteDataRow(cmd);
            }
        }
        #endregion

        #region ExecuteDataRowAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(string cmdString)
        {
            return ExecuteDataRowAsync(false, cmdString);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExecuteDataRowAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataRowAsync(false, cmdString, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExecuteDataRowAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataRowAsync(false, cmdString, commandType, parameters);
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(DbCommand cmd)
        {
            return ExecuteDataRowAsync(false, cmd);
        }
        #endregion

        #region ExecuteDataRowAsync ReadOnly
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(bool readOnly, string cmdString)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataRowAsync(cmdString).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(bool readOnly, string cmdString, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataRowAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(bool readOnly, string cmdString, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataRowAsync(cmdString, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(bool readOnly, string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataRowAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(bool readOnly, string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataRowAsync(cmdString, commandType, parameters).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="readOnly">是否连只读库，默认为false</param>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public static Task<DataRow> ExecuteDataRowAsync(bool readOnly, DbCommand cmd)
        {
            var dbHelper = CreateDefaultDbHelper(readOnly);

            return dbHelper.ExecuteDataRowAsync(cmd).ContinueWith(task =>
            {
                using (dbHelper)
                {
                    return task.Result;
                }
            });
        }
        #endregion
    }

    public class DbSettings
    {
        public string DefConnstr { get; set; }
        public string DefaultKey { get; set; }
    }

    public class TransactionResult
    {
        public int Code { get; set; }
        public Exception Error { get; set; }
        public bool IsSuccess { get { return string.IsNullOrEmpty(ErrorMsg) && Code == 0 && Error == null; } }

        public string ErrorMsg { get { return Error == null ? string.Empty : Error.Message; } }
    }

    public abstract partial class BaseDbHelper : IDbTransaction
    {
        private readonly DbConnection connection;

        public BaseDbHelper(string connectionString)
        {
            connection = CreateDbConnection();
            connection.ConnectionString = connectionString;
        }

        public abstract DbConnection CreateDbConnection();

        #region CreateDbCommand
        public DbCommand CreateDbCommand() => connection.CreateCommand();
        public DbCommand CreateDbCommand(string cmdString)
        {
            var cmd = CreateDbCommand();
            if (cmd != null)
                cmd.CommandText = cmdString;
            return cmd;
        }
        #endregion

        #region CreateDataAdapter
        public abstract DbDataAdapter CreateDataAdapter();
        public DbDataAdapter CreateDataAdapter(string cmdString)
        {
            return CreateDataAdapter(CreateDbCommand(cmdString));
        }
        public DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            var adapter = CreateDataAdapter();
            if (adapter != null)
                adapter.SelectCommand = cmd;
            return adapter;
        }
        #endregion

        #region CreateDataParameter
        public abstract DbParameter CreateDataParameter();
        public DbParameter CreateDataParameter(string parameterName, object value)
        {
            return CreateDataParameter(parameterName, value, ParameterDirection.Input);
        }
        public DbParameter CreateDataParameter(string parameterName, object value, ParameterDirection direction)
        {
            var parameter = CreateDataParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = value;
            parameter.Direction = direction;

            return parameter;
        }
        public DbParameter CreateDataParameter(string parameterName, DbType dbType, ParameterDirection direction)
        {
            var parameter = CreateDataParameter();

            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Direction = direction;

            return parameter;
        }
        #endregion

        #region Transaction
        private IDbTransaction transaction;
        public IDbTransaction Transaction { get { return transaction; } }

        public TransactionResult TransactionScope(Func<BaseDbHelper, bool> callback)
        {
            var r = new TransactionResult();
            if (callback != null)
            {
                try
                {
                    BeginTransaction();
                    if (!callback(this))
                    {
                        Rollback();
                    }
                    else
                    {
                        Commit();
                    }
                    return r;
                }
                catch (Exception ex)
                {
                    r.Error = ex;
                    Rollback();
                    return r;
                }
            }
            else
            {
                return r;
            }
        }

        public IDbConnection Connection { get { return connection; } }
        public IsolationLevel IsolationLevel { get { return transaction.IsolationLevel; } }
        public void BeginTransaction()
        {
            if (connection.State != ConnectionState.Open)
                lock (connection)
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                }
            transaction = connection.BeginTransaction();
        }
        public void BeginTransaction(IsolationLevel level)
        {
            if (connection.State != ConnectionState.Open)
                lock (connection)
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                }
            transaction = connection.BeginTransaction(level);
        }
        public void Commit()
        {
            if (transaction != null)
                transaction.Commit();
        }
        public void Rollback()
        {
            if (transaction != null)
                transaction.Rollback();
        }
        public void Dispose()
        {
            if (transaction != null)
                transaction.Dispose();
            connection.Dispose();
        }
        #endregion

        #region Prepare
        private void OpenConnection(IDbCommand cmd)
        {
            cmd.Connection = connection;
            if (transaction != null)
                cmd.Transaction = transaction;

            ProcessDbParamenter(cmd);

            if (connection.State != ConnectionState.Open)
                lock (connection)
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
        }

        private SemaphoreSlim semaphore;
        private async Task OpenConnectionAsync(IDbCommand cmd)
        {
            cmd.Connection = connection;
            if (transaction != null)
                cmd.Transaction = transaction;

            ProcessDbParamenter(cmd);

            if (semaphore == null)
                lock (connection)
                    if (semaphore == null)
                        semaphore = new SemaphoreSlim(1, 1);

            if (connection.State != ConnectionState.Open)
                try
                {
                    await semaphore.WaitAsync();
                    if (connection.State != ConnectionState.Open)
                        await connection.OpenAsync();
                }
                finally
                {
                    semaphore.Release();
                }
        }

        private static void ProcessDbParamenter(IDbCommand cmd)
        {
            if (cmd != null && cmd.Parameters.Count > 0)
                foreach (IDataParameter param in cmd.Parameters)
                {
                    if (param.Value == null)
                        param.Value = DBNull.Value;
                }
        }
        #endregion

        #region Excute
        private T Excute<T>(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters, Func<DbCommand, T> func)
        {
            using (var cmd = CreateDbCommand(cmdString))
            {
                cmd.CommandType = commandType;

                if (parameters != null)
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                return func(cmd);
            }
        }

        private async Task<T> ExcuteAsync<T>(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters, Func<DbCommand, Task<T>> func)
        {
            using (var cmd = CreateDbCommand(cmdString))
            {
                cmd.CommandType = commandType;

                if (parameters != null)
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                return await func(cmd);
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>受影响的行数。</returns>
        public int ExecuteNonQuery(string cmdString)
        {
            return Excute(cmdString, CommandType.Text, null, ExecuteNonQuery);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public int ExecuteNonQuery(string cmdString, params DbParameter[] parameters)
        {
            return Excute(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteNonQuery);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public int ExecuteNonQuery(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteNonQuery);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public int ExecuteNonQuery(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteNonQuery);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public int ExecuteNonQuery(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteNonQuery);
        }

        #endregion

        #region ExecuteNonQueryAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>受影响的行数。</returns>
        public Task<int> ExecuteNonQueryAsync(string cmdString)
        {
            return ExcuteAsync(cmdString, CommandType.Text, null, ExecuteNonQueryAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public Task<int> ExecuteNonQueryAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteNonQueryAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public Task<int> ExecuteNonQueryAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteNonQueryAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public Task<int> ExecuteNonQueryAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteNonQueryAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>受影响的行数。</returns>
        public Task<int> ExecuteNonQueryAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteNonQueryAsync);
        }

        #endregion
        #region ExecuteScalar
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string cmdString)
        {
            return Excute(cmdString, CommandType.Text, null, ExecuteScalar);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string cmdString, params DbParameter[] parameters)
        {
            return Excute(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteScalar);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteScalar);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteScalar);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public object ExecuteScalar(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteScalar);
        }

        #endregion

        #region ExecuteScalarAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Task<object> ExecuteScalarAsync(string cmdString)
        {
            return ExcuteAsync(cmdString, CommandType.Text, null, ExecuteScalarAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Task<object> ExecuteScalarAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteScalarAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Task<object> ExecuteScalarAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteScalarAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Task<object> ExecuteScalarAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteScalarAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Task<object> ExecuteScalarAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteScalarAsync);
        }

        #endregion
        #region ExecuteDataSet
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>多个查询结果。</returns>
        public DataSet ExecuteDataSet(string cmdString)
        {
            return Excute(cmdString, CommandType.Text, null, ExecuteDataSet);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public DataSet ExecuteDataSet(string cmdString, params DbParameter[] parameters)
        {
            return Excute(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataSet);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public DataSet ExecuteDataSet(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataSet);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public DataSet ExecuteDataSet(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteDataSet);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public DataSet ExecuteDataSet(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteDataSet);
        }

        #endregion

        #region ExecuteDataSetAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>多个查询结果。</returns>
        public Task<DataSet> ExecuteDataSetAsync(string cmdString)
        {
            return ExcuteAsync(cmdString, CommandType.Text, null, ExecuteDataSetAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public Task<DataSet> ExecuteDataSetAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataSetAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public Task<DataSet> ExecuteDataSetAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataSetAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public Task<DataSet> ExecuteDataSetAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteDataSetAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>多个查询结果。</returns>
        public Task<DataSet> ExecuteDataSetAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteDataSetAsync);
        }

        #endregion
        #region ExecuteDataTable
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public DataTable ExecuteDataTable(string cmdString)
        {
            return Excute(cmdString, CommandType.Text, null, ExecuteDataTable);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public DataTable ExecuteDataTable(string cmdString, params DbParameter[] parameters)
        {
            return Excute(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataTable);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public DataTable ExecuteDataTable(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataTable);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public DataTable ExecuteDataTable(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteDataTable);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public DataTable ExecuteDataTable(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteDataTable);
        }

        #endregion

        #region ExecuteDataTableAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>查询结果。</returns>
        public Task<DataTable> ExecuteDataTableAsync(string cmdString)
        {
            return ExcuteAsync(cmdString, CommandType.Text, null, ExecuteDataTableAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public Task<DataTable> ExecuteDataTableAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataTableAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public Task<DataTable> ExecuteDataTableAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataTableAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public Task<DataTable> ExecuteDataTableAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteDataTableAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>查询结果。</returns>
        public Task<DataTable> ExecuteDataTableAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteDataTableAsync);
        }

        #endregion
        #region ExecuteDataRow
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>单行查询结果。</returns>
        public DataRow ExecuteDataRow(string cmdString)
        {
            return Excute(cmdString, CommandType.Text, null, ExecuteDataRow);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public DataRow ExecuteDataRow(string cmdString, params DbParameter[] parameters)
        {
            return Excute(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataRow);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public DataRow ExecuteDataRow(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataRow);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public DataRow ExecuteDataRow(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteDataRow);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public DataRow ExecuteDataRow(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return Excute(cmdString, commandType, parameters, ExecuteDataRow);
        }

        #endregion

        #region ExecuteDataRowAsync
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <returns>单行查询结果。</returns>
        public Task<DataRow> ExecuteDataRowAsync(string cmdString)
        {
            return ExcuteAsync(cmdString, CommandType.Text, null, ExecuteDataRowAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public Task<DataRow> ExecuteDataRowAsync(string cmdString, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.Length == 0 ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataRowAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public Task<DataRow> ExecuteDataRowAsync(string cmdString, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, parameters == null || parameters.FirstOrDefault() == null ? CommandType.Text : CommandType.StoredProcedure, parameters, ExecuteDataRowAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public Task<DataRow> ExecuteDataRowAsync(string cmdString, CommandType commandType, params DbParameter[] parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteDataRowAsync);
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmdString">Sql语句</param>
        /// <param name="commandType">Sql类型</param>
        /// <param name="parameters">Sql参数</param>
        /// <returns>单行查询结果。</returns>
        public Task<DataRow> ExecuteDataRowAsync(string cmdString, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExcuteAsync(cmdString, commandType, parameters, ExecuteDataRowAsync);
        }

        #endregion

        #region Execute DbCommand
        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>受影响的行数。</returns>
        public virtual int ExecuteNonQuery(DbCommand cmd)
        {
            OpenConnection(cmd);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回受影响的行数。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>受影响的行数。</returns>
        public virtual async Task<int> ExecuteNonQueryAsync(DbCommand cmd)
        {
            await OpenConnectionAsync(cmd);

            return await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public virtual object ExecuteScalar(DbCommand cmd)
        {
            OpenConnection(cmd);

            return cmd.ExecuteScalar();
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回结果集中第一行的第一列。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public virtual async Task<object> ExecuteScalarAsync(DbCommand cmd)
        {
            await OpenConnectionAsync(cmd);

            return await cmd.ExecuteScalarAsync();
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>多个查询结果。</returns>
        public virtual DataSet ExecuteDataSet(DbCommand cmd)
        {
            OpenConnection(cmd);

            var ds = new DataSet();
            using (var dataAdapter = CreateDataAdapter(cmd))
            {
                dataAdapter.Fill(ds);
            }
            return ds;
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回多个查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>多个查询结果。</returns>
        public virtual async Task<DataSet> ExecuteDataSetAsync(DbCommand cmd)
        {
            await OpenConnectionAsync(cmd);

            var ds = new DataSet();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (!reader.IsClosed)
                {
                    var dt = new DataTable();
                    ds.Tables.Add(dt);

                    dt.Load(reader);
                }
            }
            return ds;
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public virtual DataTable ExecuteDataTable(DbCommand cmd)
        {
            OpenConnection(cmd);

            var dt = new DataTable();
            using (var reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
            }
            return dt;
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>查询结果。</returns>
        public virtual async Task<DataTable> ExecuteDataTableAsync(DbCommand cmd)
        {
            await OpenConnectionAsync(cmd);

            var dt = new DataTable();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                dt.Load(reader);
            }
            return dt;
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>单行查询结果。</returns>
        public virtual DataRow ExecuteDataRow(DbCommand cmd)
        {
            var dt = ExecuteDataTable(cmd);

            return dt == null || dt.Rows.Count == 0 ? null : dt.Rows[0];
        }

        /// <summary>针对 .NET Framework 数据提供程序的 Connection 对象执行 SQL 语句，并返回单行查询结果。</summary>
        /// <param name="cmd">Sql命令</param>
        /// <returns>单行查询结果。</returns>
        public virtual async Task<DataRow> ExecuteDataRowAsync(DbCommand cmd)
        {
            var dt = await ExecuteDataTableAsync(cmd);

            return dt == null || dt.Rows.Count == 0 ? null : dt.Rows[0];
        }
        #endregion
    }

    public partial class SqlDbHelper : BaseDbHelper
    {
        private static readonly string _ConnectionString;

        public SqlDbHelper() : this(_ConnectionString) { }
        public SqlDbHelper(string connectionString) : base(connectionString) { }

        public override DbConnection CreateDbConnection() { return new SqlConnection(); }

        public override DbParameter CreateDataParameter() { return new SqlParameter(); }

        public override DbDataAdapter CreateDataAdapter() { return new SqlDataAdapter(); }

        public new SqlTransaction Transaction { get { return base.Transaction as SqlTransaction; } }

        public new SqlConnection Connection { get { return base.Connection as SqlConnection; } }
    }
    //public partial class SQLiteDbHelper : BaseDbHelper
    //{
    //    private static readonly string _ConnectionString;

    //    public SQLiteDbHelper() : this(_ConnectionString) { }
    //    public SQLiteDbHelper(string connectionString) : base(connectionString) { }

    //    public override DbConnection CreateDbConnection() { return new SQLiteConnection(); }

    //    public override DbParameter CreateDataParameter() { return new SQLiteParameter(); }

    //    public override DbDataAdapter CreateDataAdapter() { return new SQLiteDataAdapter(); }

    //    public new SQLiteTransaction Transaction { get { return base.Transaction as SQLiteTransaction; } }

    //    public new SQLiteConnection Connection { get { return base.Connection as SQLiteConnection; } }
    //}
    //public partial class MySqlDbHelper : BaseDbHelper
    //{
    //    private static readonly string _ConnectionString;

    //    public MySqlDbHelper() : this(_ConnectionString) { }
    //    public MySqlDbHelper(string connectionString) : base(connectionString) { }

    //    //public override DbConnection CreateDbConnection() { return new MySqlConnection(); }

    //    //public override DbParameter CreateDataParameter() { return new MySqlParameter(); }

    //    //public override DbDataAdapter CreateDataAdapter() { return new MySqlDataAdapter(); }

    //    //public new MySqlTransaction Transaction { get { return base.Transaction as MySqlTransaction; } }

    //    //public new MySqlConnection Connection { get { return base.Connection as MySqlConnection; } }
    //}

    public partial class SqlDbHelper : BaseDbHelper
    {
        private static readonly string _ReadOnlyConnectionString;
        static SqlDbHelper()
        {
            _ConnectionString = ProcessConnectionString("Sqlcon", false);
            _ReadOnlyConnectionString = ProcessConnectionString("SqlconReadOnly", true) ?? ProcessConnectionString("Gungnir", true);
        }

        public SqlDbHelper(bool readOnly) : base(readOnly ? _ReadOnlyConnectionString : _ConnectionString) { }

        private static string ProcessConnectionString(string connectionStringName, bool readOnly)
        {
            var gungnir = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (gungnir == null)
                return null;

            if (string.Compare(gungnir.ProviderName, "System.Data.SqlClient", StringComparison.OrdinalIgnoreCase) != 0)
                return gungnir.ConnectionString;

            var sb = new SqlConnectionStringBuilder(gungnir.ConnectionString);

            if (readOnly)
                sb.ApplicationIntent = ApplicationIntent.ReadOnly;

            sb.MultipleActiveResultSets = true;

            return sb.ToString();
        }

        #region SaveToDatabase
        /// <summary>批量复制</summary>
        /// <param name="table">数据源(目标表名放在TableName)</param>
        public void SaveToDatabase(DataTable table)
        {
            SaveToDatabase(table, null, null, 1000);
        }

        /// <summary>批量复制</summary>
        /// <param name="table">数据源(目标表名放在TableName)</param>
        /// <param name="destinationTableName">目标表名</param>
        /// <param name="columnMappings">列映射(key：源表列；value：目标表列)</param>
        /// <param name="timeout">超时(默认1000ms)</param>
        public void SaveToDatabase(DataTable table, string destinationTableName, IDictionary<string, string> columnMappings, int timeout)
        {
            using (var sbc = new SqlBulkCopy(Connection, SqlBulkCopyOptions.Default, Transaction))
            {
                sbc.BatchSize = 1000;
                sbc.BulkCopyTimeout = timeout;

                //将DataTable表名作为待导入库中的目标表名
                sbc.DestinationTableName = string.IsNullOrWhiteSpace(destinationTableName) ? table.TableName : destinationTableName;

                //将数据集合和目标服务器库表中的字段对应
                if (columnMappings == null)
                    foreach (DataColumn col in table.Columns)
                    {
                        //列映射定义数据源中的列和目标表中的列之间的关系
                        sbc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                else
                    foreach (var col in columnMappings)
                    {
                        //列映射定义数据源中的列和目标表中的列之间的关系
                        sbc.ColumnMappings.Add(col.Key, col.Value);
                    }

                sbc.WriteToServer(table);
            }
        }
        #endregion

        #region SaveToDatabaseAsync
        /// <summary>批量复制</summary>
        /// <param name="table">数据源(目标表名放在TableName)</param>
        public Task SaveToDatabaseAsync(DataTable table)
        {
            return SaveToDatabaseAsync(table, null, null, 1000);
        }

        /// <summary>批量复制</summary>
        /// <param name="table">数据源(目标表名放在TableName)</param>
        /// <param name="destinationTableName">目标表名</param>
        /// <param name="columnMappings">列映射(key：源表列；value：目标表列)</param>
        /// <param name="timeout">超时(默认1000ms)</param>
        public async Task SaveToDatabaseAsync(DataTable table, string destinationTableName, IDictionary<string, string> columnMappings, int timeout)
        {
            using (var sbc = new SqlBulkCopy(Connection, SqlBulkCopyOptions.Default, Transaction))
            {
                sbc.BatchSize = 1000;
                sbc.BulkCopyTimeout = timeout;

                //将DataTable表名作为待导入库中的目标表名
                sbc.DestinationTableName = string.IsNullOrWhiteSpace(destinationTableName) ? table.TableName : destinationTableName;

                //将数据集合和目标服务器库表中的字段对应
                if (columnMappings == null)
                    foreach (DataColumn col in table.Columns)
                    {
                        //列映射定义数据源中的列和目标表中的列之间的关系
                        sbc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                else
                    foreach (var col in columnMappings)
                    {
                        //列映射定义数据源中的列和目标表中的列之间的关系
                        sbc.ColumnMappings.Add(col.Key, col.Value);
                    }

                await sbc.WriteToServerAsync(table);
            }
        }
        #endregion
    }

    //public partial class SQLiteDbHelper : BaseDbHelper
    //{
    //    static SQLiteDbHelper() { _ConnectionString = ConfigurationManager.ConnectionStrings["WebLogger"].ConnectionString; }
    //}

    //public partial class MySqlDbHelper : BaseDbHelper
    //{
    //    static MySqlDbHelper() { _ConnectionString = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString; }
    //}
}