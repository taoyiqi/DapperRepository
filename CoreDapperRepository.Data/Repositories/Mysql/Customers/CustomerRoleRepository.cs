using System.Collections.Generic;
using System.Data;
using CoreDapperRepository.Core.Configuration;
using CoreDapperRepository.Core.Constants;
using CoreDapperRepository.Core.Domain.Customers;
using CoreDapperRepository.Data.Repositories.BaseInterfaces;

namespace CoreDapperRepository.Data.Repositories.Mysql.Customers
{
    public class CustomerRoleRepository : MysqlRepositoryBase<CustomerRole>, ICustomerRoleRepository, IMysqlRepository
    {
        public CustomerRoleRepository(IDbConnConfig connConfig) : base(connConfig)
        {
        }

        //protected override string ConnStrKey => ConnKeyConstants.LocalMysqlMasterKey;

        public IEnumerable<CustomerRole> GetCustomerRoles()
        {
            string sql = $"SELECT Id,Name,SystemName FROM {TableName}";

            return GetList(sql, commandType: CommandType.Text, useTransaction: true);
        }
    }
}
