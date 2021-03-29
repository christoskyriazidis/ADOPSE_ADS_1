using ApiOne.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Customer
{
    public class CustomersWithPagination
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalPages { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public List<CustomerDetails> Customers { get; set; }

        public CustomersWithPagination()
        {

        }
    }
}
