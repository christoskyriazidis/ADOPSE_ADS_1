using ApiOne.Helpers;
using ApiOne.Interfaces;
using ApiOne.Models;
using ApiOne.Models.Customer;
using ApiOne.Models.Queries;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerDetails GetCustomer()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT id,username,profileImg,rating FROM [Customer]";
                var customers = conn.Query<CustomerDetails>(sql).FirstOrDefault();
                return customers;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public CustomersWithPagination GetCustomers(Pagination pagination)
        {
            try
            {
                using var conn = ConnectionManager.GetSqlConnection();
                string sql = $"EXEC getCustomerByPage @pageNumber,@pageSize; SELECT count(*) as TotalCustomers FROM [Customer]";
                CustomersWithPagination customersWithPagination = new CustomersWithPagination();
                using (var results = conn.QueryMultiple(sql, new { pagination.PageNumber, pagination.PageSize }))
                {
                    customersWithPagination.Customers = results.Read<CustomerDetails>().ToList();
                    customersWithPagination.TotalCustomers = results.Read<int>().FirstOrDefault();
                };
                customersWithPagination.CurrentPage = pagination.PageNumber;
                customersWithPagination.PageSize = pagination.PageSize;
                int lastPageNumber = (customersWithPagination.TotalCustomers % pagination.PageSize == 0) ? customersWithPagination.TotalCustomers / pagination.PageSize : customersWithPagination.TotalCustomers / pagination.PageSize + 1;
                int nextPageNumber = (pagination.PageNumber == lastPageNumber) ? lastPageNumber : pagination.PageNumber + 1;
                customersWithPagination.NextPageUrl = $"https://localhost:44374/customer?PageNumber={nextPageNumber}&PageSize={pagination.PageSize}";
                int previousPageNumber = (pagination.PageNumber < 2) ? 1 : pagination.PageNumber - 1;
                customersWithPagination.PreviousPageUrl = $"https://localhost:44374/customer?PageNumber={previousPageNumber}&PageSize={pagination.PageSize}";
                customersWithPagination.LastPageUrl = $"https://localhost:44374/customer?PageNumber={lastPageNumber}&PageSize={pagination.PageSize}"; ;
                customersWithPagination.TotalPages = lastPageNumber;
                return customersWithPagination;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }
    }
}
