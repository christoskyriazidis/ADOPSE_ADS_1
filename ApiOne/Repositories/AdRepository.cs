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

        public AdParametresQueryFilterFront GetAdsByFilters(AdParametresQueryFilterBack adParametresFilter, Pagination Params)
        {
            try
            {
                AdParametresQueryFilterFront adParametresQueryFilterFront = new AdParametresQueryFilterFront();
                using var conn = ConnectionManager.GetSqlConnection();
                string sql = $"EXEC Dynamic_Ad_Filters @Filter,@pageSize,@pageNumber; SELECT count(*) as AdCount FROM [Ad] where {adParametresFilter.FinalQuery}";
                AdsWithPagination adPagination = new AdsWithPagination();
                using (var results = conn.QueryMultiple(sql, new { Filter=adParametresFilter.FinalQuery,Params.PageNumber, Params.PageSize }))
                {
                    adParametresQueryFilterFront.Ads = results.Read<Ad>().ToList();
                    adParametresQueryFilterFront.TotalAds = results.Read<int>().FirstOrDefault();
                };
                var urlFilters = "";
                //loop through se ka8e property gia na gemise to front object
                foreach (var prop in adParametresFilter.GetType().GetProperties())
                {
                    var value = prop.GetValue(adParametresFilter, null);
                    if (value != null && prop.Name != "FinalQuery")
                    {
                        //pattern  type=1_2_3&category=1_2_3
                        urlFilters += $"{prop.Name}={value}&";
                        var stringFilter = value.ToString().Length > 2 ? value.ToString().Split("_") : value.ToString().Split("_");
                        var intArrayFilter = stringFilter.Select(Int32.Parse).ToList();
                        adParametresQueryFilterFront.GetType().GetProperty(prop.Name).SetValue(adParametresQueryFilterFront, intArrayFilter);
                    }
                }
                adParametresQueryFilterFront.Params = Params;
                int lastPageNumber = (adParametresQueryFilterFront.TotalAds % Params.PageSize == 0) ? adParametresQueryFilterFront.TotalAds / Params.PageSize : adParametresQueryFilterFront.TotalAds / Params.PageSize + 1;
                int nextPageNumber = (Params.PageNumber == lastPageNumber) ? lastPageNumber : Params.PageNumber + 1;
                adParametresQueryFilterFront.NextPageUrl = $"https://localhost:44374/filter?{urlFilters}PageNumber={nextPageNumber}&PageSize={Params.PageSize}";
                int previousPageNumber = (Params.PageNumber < 2) ? 1 : Params.PageNumber - 1;
                adParametresQueryFilterFront.PreviousPageUrl = $"https://localhost:44374/filter?{urlFilters}PageNumber={previousPageNumber}&PageSize={Params.PageSize}";
                adParametresQueryFilterFront.LastPageUrl = $"https://localhost:44374/filter?{urlFilters}PageNumber={lastPageNumber}&PageSize={Params.PageSize}";
                adParametresQueryFilterFront.CurrentPageUrl = $"https://localhost:44374/filter?{urlFilters}PageNumber={Params.PageNumber}&PageSize={Params.PageSize}";
                adParametresQueryFilterFront.TotalPages = lastPageNumber;
                return adParametresQueryFilterFront;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public AdsWithPagination GetAds(Pagination adParameters)
        {
            try
            { 
                using var conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC getAdByPage @PageNumber,@PageSize;SELECT count(*)as AdCount FROM [Ad] ";
                AdsWithPagination adPagination = new AdsWithPagination();
                using (var results = conn.QueryMultiple(sql, new { adParameters.PageNumber, adParameters.PageSize }))
                {
                    adPagination.Ads = results.Read<Ad>().ToList();
                    adPagination.TotalAds = results.Read<int>().FirstOrDefault();
                };
                int lastPageNumber = (adPagination.TotalAds % adParameters.PageSize==0)? adPagination.TotalAds / adParameters.PageSize: adPagination.TotalAds / adParameters.PageSize+1;
                adPagination.PageSize = adParameters.PageSize;
                adPagination.CurrentPage = adParameters.PageNumber;
                int nextPageNumber = (adParameters.PageNumber == lastPageNumber) ? lastPageNumber : adParameters.PageNumber + 1; 
                adPagination.NextPageUrl= $"https://localhost:44374/ad?PageNumber={nextPageNumber}&PageSize={adParameters.PageSize}";
                int previousPageNumber = (adParameters.PageNumber < 2) ? 1 : adParameters.PageNumber - 1;
                adPagination.PreviousPageUrl = $"https://localhost:44374/ad?PageNumber={previousPageNumber}&PageSize={adParameters.PageSize}";
                adPagination.LastPageUrl= $"https://localhost:44374/ad?PageNumber={lastPageNumber}&PageSize={adParameters.PageSize}"; ;
                adPagination.TotalPages = lastPageNumber;
                return adPagination;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
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

        public int InsertAd(CreateAd ad)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                ad.CreateDate = DateTime.Now.ToString();
                ad.LastUpdate = DateTime.Now.ToString();
                string sql = "  exec insert_ad_with_img  @title,@description,@createDate,@state,@type,@condition,@category,@customer,@Manufacturer,@lastUpdate,@price";
                var result = conn.Query<int>(sql, new { ad.Title,ad.Description,ad.CreateDate,ad.State,ad.Type,ad.Category,ad.Condition,ad.Customer,ad.Manufacturer,ad.LastUpdate,ad.Price }).FirstOrDefault();
                return result;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return -1;
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
                string sql = "SELECT c.AdId,cust.username,a.Img,a.title,c.Clicked,c.categoryId from [CategoryNotification] c " +
                    "join [Ad] a on (c.AdId =a.id) " +
                    "join [Customer] cust on (cust.id=c.CustomerId) where CustomerId=@Id order by c.id desc";
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
                string sql = "SELECT w.adId,c.username,a.Img,a.title,a.LastUpdate,w.clicked,w.customerId,a.Price " +
                    "FROM [WishListNotification] w join [Ad] a ON (w.adId=a.id) join [Customer] c ON (c.id=w.customerId) where w.customerId=@CId";
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

        public bool RemoveFromWishList(int CustomerId, int[] AdIds)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "DELETE FROM WishListt WHERE adId in @AdIds and customerId=@CustomerId";
                var result = conn.Execute(sql, new { CustomerId, AdIds });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool RemoveFromSubscribedCategories(int CustomerId, int[] CatIds)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "DELETE FROM SubscribedCategories WHERE categoryId in @CatIds and customerId=@CustomerId";
                var result = conn.Execute(sql, new { CustomerId, CatIds });
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }
    }
}
