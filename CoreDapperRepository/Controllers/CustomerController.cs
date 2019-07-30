using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreDapperRepository.Core.Constants;
using CoreDapperRepository.Core.Domain.Customers;
using CoreDapperRepository.Services.BaseInterfaces;
using CoreDapperRepository.Web.Models.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace CoreDapperRepository.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly ICustomerService _customerService;
        private readonly ICustomerRoleService _customerRoleService;

        public CustomerController(IConfiguration configuration, ICustomerService customerService, ICustomerRoleService customerRoleService)
        {
            _configuration = configuration;

            _customerService = customerService;
            _customerRoleService = customerRoleService;
        }

        [HttpPost]
        public ActionResult InsertSampleData(int num)
        {
            Random rd = new Random();

            const string lowerStr = "abcdefghijklmnopqrstuvwxyz";
            ArrayList lowerArr = new ArrayList();
            foreach (char t in lowerStr)
            {
                lowerArr.Add(t);
            }

            const string upperStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            ArrayList upperArr = new ArrayList();
            foreach (char t in upperStr)
            {
                upperArr.Add(t);
            }

            const string numStr = "0123456789";
            ArrayList numArr = new ArrayList();
            foreach (char t in numStr)
            {
                numArr.Add(t);
            }

            List<string> resultList = new List<string>();

            while (resultList.Count < num)
            {
                string name = string.Empty;
                for (int i = 0; i < 1; i++)
                {
                    name += upperArr[rd.Next(0, upperStr.Length)];
                    for (int j = 0; j < 6; j++)
                    {
                        name += lowerArr[rd.Next(lowerArr.Count)];
                    }

                    resultList.Add(name);
                }
            }

            List<string> numList = new List<string>();

            while (numList.Count < num)
            {
                string qq = string.Empty;
                for (int j = 0; j < 9; j++)
                {
                    qq += numArr[rd.Next(0, numArr.Count)];
                }

                numList.Add(qq);
            }

            //批量插入数据，用于测试
            List<Customer> customers = new List<Customer>();

            DateTime now = DateTime.Now;

            int resultListCount = resultList.Count;
            for (int i = 0; i < resultListCount; i++)
            {
                customers.Add(new Customer
                {
                    Username = resultList[i],
                    Email = resultList[i] + "@test.com",
                    Active = true,
                    CreationTime = now.AddSeconds(i)
                });
            }

            int numListCount = numList.Count;
            for (int i = 0; i < numListCount; i++)
            {
                customers.Add(new Customer
                {
                    Username = numList[i],
                    Email = numList[i] + "@qq.com",
                    Active = true,
                    CreationTime = now.AddSeconds(resultListCount + i)
                });
            }

            // 这里为了演示方便，实际上role id 需要由客户端用户选择后传role id，然后这里做个验证判断
            CustomerRole customerRole = _customerRoleService.GetCustomerRoles().FirstOrDefault(x => x.SystemName == "Admin");
            int roleId = customerRole?.Id ?? 0;

            int result = _customerService.InsertList(out long time, customers, roleId);

            return Json(new { ExecuteResult = result, ExecuteTime = time });
        }

        public ActionResult List()
        {
            ViewBag.DbSource = _configuration.GetSection("DapperRepositoryConfig").GetSection("CurrentDbTypeName").Value;

            return View();
        }

        [HttpGet]
        public ActionResult CustomerList(SearchCustomerModel model)
        {
            string currentDbTypeName = _configuration.GetSection("DapperRepositoryConfig").GetSection("CurrentDbTypeName").Value;

            IEnumerable<CustomerDtoModelForPage> customers = _customerService.GetPagedCustomers(out int total, model.Username, model.Email, model.PageIndex - 1, model.PageSize,
                (!string.IsNullOrEmpty(currentDbTypeName) && currentDbTypeName == ConnKeyConstants.Mssql));

            IEnumerable<CustomerRole> customerRoles = _customerRoleService.GetCustomerRoles();

            List<CustomerPagedResultModel> result = customers.Select(x =>
            {
                string roleName = "";

                CustomerRole customerRole = customerRoles.FirstOrDefault(c => c.Id == x.CustomerRoleId);
                if (customerRole != null)
                    roleName = customerRole.Name;

                CustomerPagedResultModel customerPagedResultModel = new CustomerPagedResultModel
                {
                    Id = x.Id,
                    Username = x.Username,
                    RoleName = roleName,
                    Email = x.Email,
                    Active = x.Active,
                    CreationTime = x.CreationTime.ToString("yyyy-MM-dd")
                };
                return customerPagedResultModel;
            }).ToList();

            return Json(new { code = 0, data = result, count = total });
        }

        public ActionResult PopCustomer(string viewName, int id = 0)
        {
            CustomerModel model = new CustomerModel();

            ViewBag.ViewName = viewName;

            if (id > 0)
            {
                CustomerDtoModel customer = _customerService.GetCustomerBy(id);

                if (customer == null)
                {
                    return RedirectToAction("List");
                }

                model.Id = customer.Id;
                model.Username = customer.Username;
                model.Email = customer.Email;
                model.Active = customer.Active;

                model.AvailableRoles = _customerRoleService.GetCustomerRoles().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == customer.CustomerRole.Id
                }).ToList();
            }
            else
            {
                model.AvailableRoles = _customerRoleService.GetCustomerRoles().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
            }

            return PartialView("_PopCustomer", model);
        }

        [HttpPost]
        public ActionResult CreateCustomer(CustomerModel model)
        {
            try
            {
                Customer customer = new Customer
                {
                    Username = model.Username.Trim(),
                    Email = model.Email.Trim(),
                    Active = model.Active,
                    CreationTime = DateTime.Now
                };

                int result = _customerService.InsertCustomer(customer, model.RoleId);

                return Json(new { status = result, msg = result > 0 ? "added successfully" : "added failed" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = "added failed:" + ex.Message });

            }
        }

        [HttpPost]
        public ActionResult EditCustomer(CustomerModel model)
        {
            Customer customer = _customerService.GetCustomerById(model.Id);

            if (customer == null)
            {
                return Json(new { status = false, msg = "no customer found with the specified id" });
            }

            try
            {
                customer.Username = model.Username.Trim();
                customer.Email = model.Email.Trim();
                customer.Active = model.Active;

                int result = _customerService.UpdateCustomer(customer, model.RoleId);

                return Json(new { status = result, msg = result > 0 ? "edited successfully" : "edited failed" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = "edited failed:" + ex.Message });

            }
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int id)
        {
            Customer customer = _customerService.GetCustomerById(id);
            if (customer == null)
                return Json(new { status = false, msg = "no customer found with the specified id" });

            try
            {
                bool result = _customerService.DeleteCustomer(customer);
                return Json(new { status = result, msg = result ? "deleted successfully" : "deleted failed" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = "deleted failed:" + ex.Message });
            }
        }
    }
}