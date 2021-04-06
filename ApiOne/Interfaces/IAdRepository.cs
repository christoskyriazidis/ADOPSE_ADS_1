using ApiOne.Models;
using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Interfaces
{
    public interface IAdRepository
    {
        AdsWithPagination GetAds(Pagination adParameters);
        AdParametresQueryFilterFront GetAdsByFilters(AdParametresQueryFilterBack adParametresFilter, Pagination Params);
        Ad GetAd(int id);
        Ad UpdateAd(Ad ad);
        public bool UpdateAdImg(int adId);

        int InsertAd(CreateAd ad);
        bool DeleteAd(int id);

        bool RemoveFromWishList(int customerId, int[] AdIds);
        bool AddToWishList(int adId, int customerId);

        bool SubscribeToCategory(int categoryId, int customerId);
        bool RemoveFromSubscribedCategories(int CustomerId, int[] CatIds);

        IEnumerable<int> GetSuscribedCategories(int CustmerId);
        IEnumerable<CategoryNotification> GetCategoryNotifications(int CustmerId);
        IEnumerable<WishList> GetWishList(int CustmerId);
        IEnumerable<WishListNotification> GetWishListNotifications(int CustmerId);
        IEnumerable<AdFilter> GetCategories();
        IEnumerable<AdFilter> GetConditions();
        IEnumerable<AdFilter> GetManufacturers();
        IEnumerable<AdFilter> GetStates();
        IEnumerable<AdFilter> GetTypes();

    }
}
