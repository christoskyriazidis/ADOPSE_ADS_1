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
        AdsWithPagination GetAdsByCustomerId(Pagination adParameters, int customerId);
        //AdParametresQueryFilterFront GetAdsByFilters(AdParametresQueryFilterBack adParametresFilter, Pagination Params);
        Ad GetAd(int id);
        Ad UpdateAd(Ad ad);
        bool UpdateAdImg(int adId);

        bool NotificationSeen(int notId);
        int InsertAd(CreateAd ad);
        bool DeleteAd(int id);

        bool RemoveFromWishList(int customerId, int[] AdIds);
        bool AddToWishList(int adId, int customerId);

        bool SubscribeToSubCategory(int categoryId, int customerId);
        bool RemoveFromSubscribedSubCategories(int CustomerId, int[] CatIds);

        IEnumerable<int> GetSuscribedSubCategories(int CustmerId);
        IEnumerable<Subcategory> GetSubCategories(int SubId);
        IEnumerable<SubCategoryNotification> GetCategoryNotifications(int CustmerId);
        IEnumerable<WishList> GetWishList(int CustmerId);
        IEnumerable<WishListNotification> GetWishListNotifications(int CustmerId);
        IEnumerable<CategoryWithImg> GetCategories();
        IEnumerable<AdFilter> GetConditions();
        IEnumerable<AdFilter> GetManufacturers();
        IEnumerable<AdFilter> GetStates();
        IEnumerable<AdFilter> GetTypes();

    }
}
