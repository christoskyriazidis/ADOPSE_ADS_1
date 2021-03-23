using ApiOne.Helpers;
using ApiOne.Interfaces;
using ApiOne.Models;
using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Repositories
{
    public class AdRepository : IAdRepository
    {

        public AdRepository()
        {

        }
        public Ad GetAd(int id)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [Ad] where id=@Id";
                var ad = conn.Query<Ad>(sql, new { Id = id }).FirstOrDefault();
                return ad;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }
            
        public AdPagination GetAdsByFilters(AdParametresQuertyFilter adParametresFilter)
        {
            try
            {
                using var conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC Dynamic_Ad_Filters @Filter,@pageSize,@pageNumber; SELECT count(*)as AdCount FROM [Ad] ";
                AdPagination adPagination = new AdPagination();
                using (var results = conn.QueryMultiple(sql, new { adParametresFilter.Params.PageNumber, adParametresFilter.Params.PageSize }))
                {
                    adPagination.Ads = results.Read<Ad>().ToList();
                    adPagination.TotalAds = results.Read<int>().FirstOrDefault();
                };
                //https://localhost:44374/ad?PageNumber=5&PageSize=5
                int lastPageNumber = (adPagination.TotalAds % adParametresFilter.Params.PageSize == 0) ? adPagination.TotalAds / adParametresFilter.Params.PageSize : adPagination.TotalAds / adParametresFilter.Params.PageSize + 1;
                adPagination.PageSize = adParametresFilter.Params.PageSize;
                adPagination.NextPageUrl = $"{(adParametresFilter.Params.PageNumber == lastPageNumber ? lastPageNumber : adParametresFilter.Params.PageNumber + 1)}";
                adPagination.PreviousPageUrl = $"previous:{(adParametresFilter.Params.PageNumber < 2 ? 1 : adParametresFilter.Params.PageNumber - 1)}";
                adPagination.LastPageUrl = $"lastpage:{lastPageNumber}";
                adPagination.CurrentPage = adParametresFilter.Params.PageNumber;
                adPagination.TotalPages = lastPageNumber;
                return adPagination;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }


        public AdPagination GetAds(AdPageSizeNumberParameters adParameters)
        {
            try
            { 
                using var conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC getAdByPage @PageNumber,@PageSize;SELECT count(*)as AdCount FROM [Ad] ";
                AdPagination adPagination = new AdPagination();
                using (var results = conn.QueryMultiple(sql, new { adParameters.PageNumber, adParameters.PageSize }))
                {
                    adPagination.Ads = results.Read<Ad>().ToList();
                    adPagination.TotalAds = results.Read<int>().FirstOrDefault();
                };
                //https://localhost:44374/ad?PageNumber=5&PageSize=5
                int lastPageNumber = (adPagination.TotalAds % adParameters.PageSize==0)? adPagination.TotalAds / adParameters.PageSize: adPagination.TotalAds / adParameters.PageSize+1;
                adPagination.PageSize = adParameters.PageSize;
                adPagination.NextPageUrl= $"{(adParameters.PageNumber== lastPageNumber ? lastPageNumber : adParameters.PageNumber + 1)}";
                adPagination.PreviousPageUrl = $"previous:{(adParameters.PageNumber<2?1:adParameters.PageNumber-1)}";
                adPagination.LastPageUrl= $"lastpage:{lastPageNumber}";
                adPagination.CurrentPage = adParameters.PageNumber;
                adPagination.TotalPages = lastPageNumber;
                return adPagination;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }


        public int GetAdTableSize()
        {
            try {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT count(*) as AdCount FROM [Ad]";
                int count = conn.Query<int>(sql).FirstOrDefault();
                return count;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return -1;
            }
        }

        // na dw ta gamimena kleidia, mallon cascade h kati tetoio
        public bool DeleteAd(int id)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "DELETE FROM [Ad] WHERE id=@Id";
                var result = conn.Execute(sql, new { Id = id });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool InsertAd(Ad ad)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                ad.Date = DateTime.Now.ToString();
                ad.LastUpdate = DateTime.Now.ToString();
                string sql = "  INSERT INTO [Ad] (title,Description,Date,State,Img,Type,Category,Condition,Customer,Manufacturer,LastUpdate)" +
                            "VALUES (@Title,@Description,@Date,@State,@Img,@Type,@Category,@Condition,@Customer,@Manufacturer,@LastUpdate)";
                var result = conn.Execute(sql, new { ad.Title, ad.Description, ad.Date, ad.State, ad.Img, ad.Type, ad.Category, ad.Condition, ad.Customer, ad.Manufacturer, ad.LastUpdate });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public Ad UpdateAd(Ad ad)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                ad.LastUpdate = DateTime.Now.ToString();
                string sql = " UPDATE [Ad] SET Title=@Title,Description=@Description,LastUpdate=@LastUpdate,State=@State,Img=@Img,Type=@Type,Category=@Category,Condition=@Condition," +
                                "Customer=@Customer,Manufacturer=@Manufacturer where id=@Id";
                var result = conn.Execute(sql, new
                {
                    ad.Title,
                    ad.Description,
                    ad.LastUpdate,
                    ad.State,
                    ad.Img,
                    ad.Type,
                    ad.Category,
                    ad.Condition,
                    ad.Customer,
                    ad.Manufacturer,
                    ad.Id
                });
                return ad;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            };
        }

        public IEnumerable<AdFilter> GetCategories()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [Category]";
                var categories = conn.Query<AdFilter>(sql).ToList();
                return categories;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            };
        }

        


        public bool SubscribeToCategory(int categoryId, int customerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = " insert into [SubscribedCategories] (customerId,categoryId) values (@CustomerId,@CategoryId)";
                var result = conn.Execute(sql, new { CustomerId = customerId, CategoryId = categoryId });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool AddToWishList(int adId, int customerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "insert into [WishListt] (customerId,adId) values (@CustomerId,@AdId)";
                var result = conn.Execute(sql, new { CustomerId = customerId, AdId = adId });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public IEnumerable<CategoryNotification> GetCategoryNotifications(int CustmerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [CategoryNotification] where CustomerId=@Id";
                var categoryNotifications = conn.Query<CategoryNotification>(sql, new { Id = CustmerId }).ToList();
                return categoryNotifications;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<WishListNotification> GetWishListNotifications(int custmerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT w.clicked,w.adId,c.username,a.Img,a.title,a.LastUpdate,w.clicked FROM [WishListNotification] w join [Ad] a ON (w.adId=a.id) join [Customer] c ON (c.id=w.customerId) where w.customerId=@CId";
                var wishListNotifications = conn.Query<WishListNotification>(sql, new { CId = custmerId }).ToList();
                return wishListNotifications;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<int> GetSuscribedCategories(int CustmerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT categoryId from [SubscribedCategories] where CustomerId=@Id";
                var wishListNotifications = conn.Query<int>(sql, new { Id = CustmerId }).ToList();
                return wishListNotifications;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<CategoryNotification> GetWishList(int CustmerId)
        {
            throw new NotImplementedException();
        }

       

        public IEnumerable<AdFilter> GetConditions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdFilter> GetManufacturers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdFilter> GetStates()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdFilter> GetTypes()
        {
            throw new NotImplementedException();
        }
    }
}
