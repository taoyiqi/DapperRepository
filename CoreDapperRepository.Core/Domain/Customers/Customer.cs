using System;

namespace CoreDapperRepository.Core.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
