using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using CoreDapperRepository.Core.Configuration;
using CoreDapperRepository.Core.Data;
using CoreDapperRepository.Core.Domain;
using MySql.Data.MySqlClient;

namespace CoreDapperRepository.Data
{
    public class SessionFactory
    {
        private static IDbConnection CreateConnection(DatabaseType dataType = DatabaseType.Mssql, string connStrKey = "", IDbConnConfig connConfig = null)
        {
            IDbConnection conn;
            switch (dataType)
            {
                case DatabaseType.Mssql:
                    conn = new SqlConnection(connConfig != null ? connConfig.GetConnectionString(connStrKey) : string.Empty);
                    break;

                case DatabaseType.Mysql:
                    conn = new MySqlConnection(connConfig != null ? connConfig.GetConnectionString(connStrKey) : string.Empty);
                    break;

                case DatabaseType.Oracle:
                    conn = new OracleConnection(connConfig != null ? connConfig.GetConnectionString(connStrKey) : string.Empty);
                    break;

                default:
                    conn = new SqlConnection(connConfig != null ? connConfig.GetConnectionString(connStrKey) : string.Empty);
                    break;
            }

            conn.Open();

            return conn;
        }

        /// <summary>
        /// 创建数据库连接会话
        /// </summary>
        /// <returns></returns>
        public static IDbSession CreateSession(DatabaseType databaseType, string key, IDbConnConfig connConfig)
        {
            IDbConnection conn = CreateConnection(databaseType, key, connConfig);
            IDbSession session = new DbSession(conn);
            return session;
        }
    }
}
