using ApiOne.Models;
using ApiOne.Models.Ads;
using ApiOne.Models.Notification;
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
        AdsWithPagination GetActiveAdsByCustomerId(Pagination adParameters, int customerId);
        AdsWithPagination GetMyAds(Pagination adParameters, int customerId);
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

        PaginationBSA GetSoldAds(Pagination pagination, int CustomerId);
        PaginationBSA GetBoughtAds(Pagination pagination, int CustomerId);
        IEnumerable<WishSubNotification> GetNotifications(int PageNumber, int CustomerId);
        IEnumerable<int> GetSuscribedSubCategories(int CustmerId);
        IEnumerable<Subcategory> GetSubCategories(int SubId);
        IEnumerable<WishList> GetWishList(int CustmerId);
        IEnumerable<CategoryWithImg> GetCategories();
        IEnumerable<AdFilter> GetConditions();
        IEnumerable<AdFilter> GetManufacturers();
        IEnumerable<AdFilter> GetStates();
        IEnumerable<AdFilter> GetTypes();

    }
}
