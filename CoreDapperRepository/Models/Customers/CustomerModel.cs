using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreDapperRepository.Web.Models.Customers
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            AvailableRoles = new List<SelectListItem>();
        }

        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Email { get; set; }

        public bool Active { get; set; }

        public int RoleId { get; set; }
        public IList<SelectListItem> AvailableRoles { get; set; }
    }
}