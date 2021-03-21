using ApiOne.Models;
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
        IEnumerable<Ad> GetAds(AdParameters adParameters);
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
        IEnumerable<Category> GetCategories();
        IEnumerable<Condition> GetConditions();
        IEnumerable<Manufacturer> GetManufacturers();
        IEnumerable<State> GetStates();
        IEnumerable<Models.Type> GetTypes();
    }
}
