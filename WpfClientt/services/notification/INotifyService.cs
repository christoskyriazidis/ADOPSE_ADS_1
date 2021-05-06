using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.model.notification;

namespace WpfClientt.services {
    public interface INotifyService {

        Task SubscriberToSubcategory(Subcategory subcategory);

        Task UnsubscribeFromSubcategory(Subcategory subcategory);

        Task<ISet<Subcategory>> SubscribedSubcategories();

        Task AddToWishList(Ad ad);

        Task RemoveFromWishList(Ad ad);

        Task<ISet<Ad>> WishList();

        IScroller<AdChangedNotification> Notifications();

        Task NotificationSeen(AdChangedNotification notification);

        void AddWishListChangedListener();

        void RemoveWishListChangedListener();

        void AddSubcategoryChangedListener();

        void RemoveSubcategoryChangedListener();
    }
}
