using System.Collections.Generic;
using CoreDapperRepository.Core.Cache;
using CoreDapperRepository.Core.Constants;
using CoreDapperRepository.Core.Domain.Customers;
using CoreDapperRepository.Data.Repositories.BaseInterfaces;
using CoreDapperRepository.Services.BaseInterfaces;

namespace CoreDapperRepository.Services.Mssql.Customers
{
    public class CustomerRoleService : ICustomerRoleService, IMssqlService
    {
        private readonly ICustomerRoleRepository _repository;
        private readonly IStaticCacheManager _cacheManager;

        public CustomerRoleService(ICustomerRoleRepository repository, IStaticCacheManager cacheManager)
        {
            _repository = repository;
            _cacheManager = cacheManager;
        }

        public IEnumerable<CustomerRole> GetCustomerRoles()
        {
            return _cacheManager.Get(string.Format(CustomerDefaults.CustomerRolesAllCacheKey, ConnKeyConstants.Mssql),
                () => _repository.GetCustomerRoles(), 1440);
        }
    }
}
