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
        AdPagination GetAds(AdPageSizeNumberParameters adParameters);
        AdPagination GetAdsByFilters(AdParametresQuertyFilter adParametresFilter);
        int GetAdTableSize();
        Ad GetAd(int id);
        Ad UpdateAd(Ad ad);
        bool InsertAd(Ad ad);
        bool DeleteAd(int id);

        bool SubscribeToCategory(int categoryId, int customerId);

        bool AddToWishList(int adId, int customerId);

        IEnumerable<int> GetSuscribedCategories(int CustmerId);
        IEnumerable<CategoryNotification> GetCategoryNotifications(int CustmerId);
        IEnumerable<CategoryNotification> GetWishList(int CustmerId);
        IEnumerable<WishListNotification> GetWishListNotifications(int CustmerId);
        IEnumerable<AdFilter> GetCategories();
        IEnumerable<AdFilter> GetConditions();
        IEnumerable<AdFilter> GetManufacturers();
        IEnumerable<AdFilter> GetStates();
        IEnumerable<AdFilter> GetTypes();
    }
}
