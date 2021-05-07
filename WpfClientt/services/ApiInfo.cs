using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    static class ApiInfo {

        private static string host = "https://localhost:44374";

        public static string MyChatsMainUrl() {
            return $"{host}/activechat";
        }

        public static string MessageMainUrl() {
            return $"{host}/message";
        }

        public static string AdMainUrl() {
            return $"{host}/ad";
        }

        public static string CustomerMainUrl() {
            return $"{host}/customer";
        }

        public static string ProfileMainUrl() {
            return $"{host}/profile";
        }

        public static string CategoriesMainUrl() {
            return $"{host}/category";
        }
        public static string SubcategoriesMainUrl(Category category) {
            return $"{host}/category/{category.Id}";
        }

        public static string ConditionsMainUrl() {
            return $"{host}/condition";
        }

        internal static string NotificationHubMainUrl() {
            return $"{host}/notificationHub";
        }

        internal static string ChatHubMainUrl() {
            return $"{host}/chathub";
        }

        public static string StatesMainUrl() {
            return $"{host}/state";
        }

        public static string TypeMainUrl() {
            return $"{host}/type";
        }

        public static string ManufacturersMainUrl() {
            return $"{host}/manufacturer";
        }

        public static string RegistrationMainUrl() {
            return "https://localhost:44305/";
        }

        internal static string ChatRequestMainUrl(Ad ad) {
            return $"{host}/chat/chatrequest/{ad.Id}";
        }

        internal static string ConfirmChatRequestMainUrl(ChatRequest chatRequest) {
            return $"{host}/chat/chatrequest/confirm/{chatRequest.Id}";
        }

        internal static string DeclineChatRequestMainUrl(ChatRequest chatRequest) {
            return $"{host}/chat/chatrequest/decline/{chatRequest.Id}";
        }

        internal static string ProfileAdsUrl() {
            return $"{host}/profile/myads";
        }

        internal static string ChatRequestsMainUrl() {
            return $"{host}/chat/chatrequest";
        }

        internal static string SubscribedSubcategoriesMainUrl() {
            return $"{host}/subcategory/subscribe";
        }

        internal static string SubscribeSubcategoryMainUrl(Subcategory subcategory) {
            return $"{host}/category/subscribe/{subcategory.Id}";
        }

        internal static string UnsubscribeFromSubcategoriesMainUrl() {
            return $"{host}/category/subscribe";
        }
    }
}
