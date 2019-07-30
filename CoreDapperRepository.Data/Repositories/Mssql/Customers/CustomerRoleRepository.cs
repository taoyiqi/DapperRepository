using System.Collections.Generic;
using System.Data;
using CoreDapperRepository.Core.Configuration;
using CoreDapperRepository.Core.Domain.Customers;
using CoreDapperRepository.Data.Repositories.BaseInterfaces;

namespace CoreDapperRepository.Data.Repositories.Mssql.Customers
{
    public class CustomerRoleRepository : MssqlRepositoryBase<CustomerRole>, ICustomerRoleRepository, IMssqlRepository
    {
        public CustomerRoleRepository(IDbConnConfig connConfig) : base(connConfig)
        {
        }

        public IEnumerable<CustomerRole> GetCustomerRoles()
        {
            string sql = $"SELECT Id,Name,SystemName FROM {TableName}";

            return GetList(sql, commandType: CommandType.Text, useTransaction: true);
        }
    }
}
