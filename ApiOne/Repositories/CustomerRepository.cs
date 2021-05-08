using ApiOne.Helpers;
using ApiOne.Interfaces;
using ApiOne.Models;
using ApiOne.Models.Customer;
using ApiOne.Models.Queries;
using ApiOne.Models.Review;
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
        public bool CheckIfCustomerOwnThisAd(int AdId, int CustomerId)
        {
            try
            {
                //id
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT count(*) FROM ad WHERE customer=@CustomerId and id=@AdId";
                var rows = conn.ExecuteScalar<int>(sql, new { CustomerId,AdId});
                //true false analogos ta rows pou alaksan
                return (rows == 1);
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public CustomerDetails GetCustomer(int id)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [Customer] WHERE id=@Id";
                var customers = conn.Query<CustomerDetails>(sql,new { id}).FirstOrDefault();
                return customers;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public int GetCustomerIdFromSub(string SubId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT TOP 1 id FROM Customer WHERE SubId=@SubId";
                var customers = conn.Query<int>(sql, new { SubId }).FirstOrDefault();
                return customers;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return -1;
            }
        }

        public IEnumerable<CustomerReview> GetCustomerReviews(int CustomerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT r.*,c.username,c.profileImg FROM Review r join customer c ON (r.BuyerId=c.id) WHERE r.SellerId=@CustomerId";
                var customerReviews = conn.Query<CustomerReview>(sql, new { CustomerId }).ToList();
                return customerReviews;
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
                    customersWithPagination.Result = results.Read<CustomerDetails>().ToList();
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

        public IEnumerable<LoginLogs> GetLoginLogs(int CustomerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * FROM LOGINLOGS WHERE CUSTOMERID=@CustomerId order by id desc";
                var loginLogs = conn.Query<LoginLogs>(sql, new { CustomerId }).ToList();
                return loginLogs;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public CustomerDetails GetMyProfileInfo(int id)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT id,username,reviews,profileImg,Name,LastName,Address,rating,email FROM [Customer] where id=@id";
                var customers = conn.Query<CustomerDetails>(sql,new { id}).FirstOrDefault();
                return customers;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool ReviewAndRateCustomer(PostReview postReview)
        {
            try
            {
                //id
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "exec [review_and_rating] @RatingNumb,@ReviewTxt,@SoldAd,@buyerId";
                var rows = conn.ExecuteScalar<int>(sql, new {  postReview.RatingNumb,postReview.ReviewTxt,postReview.SoldAd,postReview.BuyerId });
                //true false analogos ta rows pou alaksan
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool SellAd(int AdId, int BuyerId)
        {
            try
            {
                //id
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC [dbo].[sell_ad_to_customer] @buyerId,@adId";
                var rows = conn.ExecuteScalar<int>(sql, new { BuyerId, AdId });
                //true false analogos ta rows pou alaksan
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool UpdateProfile(CustomerDetails customerDetails)
        {
            try
            {
                //id
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "  UPDATE [Customer] SET Name=@Name,LastName=@LastName,Address=@Address where id =@Id";
                var rows = conn.Execute(sql,customerDetails);
                //true false analogos ta rows pou alaksan
                return (rows == 1);
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool UpdateProfileImage(int customerId)
        {
            try
            {
                string profileImg = $"https://localhost:44374/images/Profile/{customerId}.png";
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "UPDATE [Customer] SET profileImg=@profileImg where id=@Id";
                var rows = conn.Execute(sql, new { profileImg ,Id=customerId});
                //true false analogos ta rows pou alaksan
                return (rows == 1);
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

    }
}
