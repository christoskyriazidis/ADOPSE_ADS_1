using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    public interface INotificationService {

        Task SubscriberToSubcategory(Subcategory subcategory);

        Task UnsubscribeFromSubcategories(params Subcategory[] subcategories);

        Task<ISet<Subcategory>> SubscribedSubcategories();

        Task AddToWishList(Ad ad);

        Task RemoveFromWishList(Ad ad);

        Task<ISet<Ad>> WishList();

        Task<ISet<SubcategoryChangedNotification>> SubcategoryNotifications();

        Task<ISet<WishlistAdChangedNotification>> WishlistAdNotifications();

        Task<ISet<ReviewAdNotification>> ReviewAdNotifications();

        Task NotificationSeen(AdNotification notification);

        void AddWishListChangedListener(Action notifier);

        void RemoveWishListChangedListener(Action notifier);

        void AddSubcategoryChangedListener(Action notifier);

        void RemoveSubcategoryChangedListener(Action notifier);

        void AddReviewNotificationListener(Func<ReviewAdNotification, Task> listener);
        void RemoveReviewNotificationListener(Func<ReviewAdNotification, Task> listener);
    }
}
