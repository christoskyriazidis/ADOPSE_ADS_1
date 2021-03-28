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
                customersWithPagination.PageSize = pagination.PageSize;
                customersWithPagination.CurrentPage = pagination.PageNumber;
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
