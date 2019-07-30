using System.Collections.Generic;
using CoreDapperRepository.Core.Data;
using CoreDapperRepository.Core.Domain.Customers;

namespace CoreDapperRepository.Data.Repositories.BaseInterfaces
{
    public interface ICustomerRoleRepository : IRepository<CustomerRole>
    {
        IEnumerable<CustomerRole> GetCustomerRoles();
    }
}
