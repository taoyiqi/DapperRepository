using System;
using System.Collections.Generic;
using CoreDapperRepository.Core;
using CoreDapperRepository.Core.Cache;
using CoreDapperRepository.Core.Constants;
using CoreDapperRepository.Core.Domain.Customers;
using CoreDapperRepository.Data.Repositories.BaseInterfaces;
using CoreDapperRepository.Services.BaseInterfaces;

namespace CoreDapperRepository.Services.Mysql.Customers
{
    public class CustomerService : ICustomerService, IMysqlService
    {
        private readonly ICustomerRepository _repository;
        private readonly IStaticCacheManager _cacheManager;

        public CustomerService(ICustomerRepository repository, IStaticCacheManager cacheManager)
        {
            _repository = repository;
            _cacheManager = cacheManager;
        }

        public Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
                return null;

            return _repository.GetCustomerById(customerId);
        }

        public CustomerDtoModel GetCustomerBy(int id)
        {
            if (id <= 0)
                return null;

            return _repository.GetCustomerBy(id);
        }

        public int InsertList(out long time, List<Customer> customers, int roleId)
        {
            var result = _repository.InsertList(out time, customers, roleId);
            if (result > 0)
            {
                _cacheManager.RemoveByPattern(string.Format(CustomerDefaults.CustomerCountPatternCacheKey, ConnKeyConstants.Mysql));
            }

            return result;
        }

        public IEnumerable<CustomerDtoModel> GetAllCustomers()
        {
            return _repository.GetAllCustomers();
        }

        public IEnumerable<CustomerDtoModelForPage> GetPagedCustomers(out int totalCount, string username = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool useStoredProcedure = false)
        {
            int total;

            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
            {
                // 缓存无搜索条件的总记录数
                total = _cacheManager.Get(string.Format(CustomerDefaults.CustomerTotalCountCacheKey, ConnKeyConstants.Mysql), () => _repository.GetCustomerCount(), 1440);
            }
            else
            {
                string filterKey = string.Format(CustomerDefaults.CustomerFilterCountCacheKey, ConnKeyConstants.Mysql, CommonHelper.GetHashString(username + email));
                total = _cacheManager.Get(filterKey, () => _repository.GetCustomerCount(username, email), 1440);
            }

            totalCount = total;

            return _repository.GetPagedCustomers(total, username, email, pageIndex, pageSize, useStoredProcedure);
        }

        public int InsertCustomer(Customer customer, int roleId)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var result = _repository.InsertCustomer(customer, roleId);

            if (result > 0)
            {
                _cacheManager.RemoveByPattern(string.Format(CustomerDefaults.CustomerCountPatternCacheKey, ConnKeyConstants.Mysql));
            }

            return result;
        }

        public int UpdateCustomer(Customer customer, int roleId)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            return _repository.UpdateCustomer(customer, roleId);
        }

        public bool DeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var result = _repository.Delete(customer.Id);

            if (result)
            {
                _cacheManager.RemoveByPattern(string.Format(CustomerDefaults.CustomerCountPatternCacheKey, ConnKeyConstants.Mysql));
            }

            return result;
        }
    }
}
