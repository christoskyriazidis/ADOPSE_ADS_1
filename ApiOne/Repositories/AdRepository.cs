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

        public AdParametresQueryFilterFront GetAdsByFilters(AdParametresQueryFilterBack adParametresFilter)
        {
            try
            {
                AdParametresQueryFilterFront adParametresQueryFilterFront = new AdParametresQueryFilterFront();
                using var conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC Dynamic_Ad_Filters @Filter,@pageSize,@pageNumber; SELECT count(*)as AdCount FROM [Ad] ";
                AdPagination adPagination = new AdPagination();
                using (var results = conn.QueryMultiple(sql, new { Filter=adParametresFilter.FinalQuery,adParametresFilter.Params.PageNumber, adParametresFilter.Params.PageSize }))
                {
                    adParametresQueryFilterFront.Ads = results.Read<Ad>().ToList();
                    adParametresQueryFilterFront.TotalAds = results.Read<int>().FirstOrDefault();
                };

                return adParametresQueryFilterFront;
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
                int lastPageNumber = (adPagination.TotalAds % adParameters.PageSize==0)? adPagination.TotalAds / adParameters.PageSize: adPagination.TotalAds / adParameters.PageSize+1;
                adPagination.PageSize = adParameters.PageSize;
                int nextPageNumber = (adParameters.PageNumber == lastPageNumber) ? lastPageNumber : adParameters.PageNumber + 1; 
                adPagination.NextPageUrl= $"https://localhost:44374/ad?PageNumber={nextPageNumber}&PageSize={adParameters.PageSize}";
                int previousPageNumber = (adParameters.PageNumber < 2) ? 1 : adParameters.PageNumber - 1;
                adPagination.PreviousPageUrl = $"https://localhost:44374/ad?PageNumber={previousPageNumber}&PageSize={adParameters.PageSize}";
                adPagination.LastPageUrl= $"https://localhost:44374/ad?PageNumber={lastPageNumber}&PageSize={adParameters.PageSize}"; ;
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

        public IEnumerable<WishList> GetWishList(int CustmerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT w.adId,c.username,a.title,a.Img FROM [WishListt] w " +
                    "join [Customer] c on (c.id=w.customerId)" +
                    "join [Ad] a on (a.id=w.adId)" +
                    "where customerId=@CustomerId ";
                var wishList = conn.Query<WishList>(sql,new { CustomerId=CustmerId }).ToList();
                return wishList;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            };
        }


        public IEnumerable<AdFilter> GetConditions()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [Condition]";
                var conditions = conn.Query<AdFilter>(sql).ToList();
                return conditions;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            };
        }

        public IEnumerable<AdFilter> GetManufacturers()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [Manufacturer]";
                var manufacturers = conn.Query<AdFilter>(sql).ToList();
                return manufacturers;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            };
        }

        public IEnumerable<AdFilter> GetStates()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [State]";
                var states = conn.Query<AdFilter>(sql).ToList();
                return states;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            };
        }

        public IEnumerable<AdFilter> GetTypes()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * from [Type]";
                var types = conn.Query<AdFilter>(sql).ToList();
                return types;
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

    }
}
