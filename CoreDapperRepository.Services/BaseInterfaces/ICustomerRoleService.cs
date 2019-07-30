using System.Collections.Generic;
using CoreDapperRepository.Core.Domain.Customers;

namespace CoreDapperRepository.Services.BaseInterfaces
{
    public interface ICustomerRoleService
    {
        IEnumerable<CustomerRole> GetCustomerRoles();
    }
}
