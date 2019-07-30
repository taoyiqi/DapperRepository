using CoreDapperRepository.Core;
using CoreDapperRepository.Core.Configuration;
using CoreDapperRepository.Core.Constants;
using CoreDapperRepository.Core.Domain;

namespace CoreDapperRepository.Data.Repositories.Mssql
{
    public class MssqlRepositoryBase<T> : RepositoryBase<T> where T : BaseEntity
    {
        private IDbConnConfig _connConfig;

        public MssqlRepositoryBase(IDbConnConfig connConfig)
        {
            _connConfig = connConfig;
        }

        protected sealed override DatabaseType DataType => DatabaseType.Mssql;

        /// <inheritdoc />
        /// <summary>
        /// 当前数据库连接串的key(默认主数据库key)
        /// </summary>
        protected override string ConnStrKey => ConnKeyConstants.MssqlMasterKey;

        /// <inheritdoc />
        /// <summary>
        /// 数据表名(默认类名，如果不是，需要在子类重写)
        /// </summary>
        protected override string TableName => $"[dbo].[{typeof(T).Name}]";

        /// <inheritdoc />
        /// <summary>
        /// 数据连接串配置接口
        /// </summary>
        protected override IDbConnConfig DbConnConfig
        {
            get => _connConfig;
            set => _connConfig = value;
        }
    }
}
