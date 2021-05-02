using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    static class ApiInfo {

        private static string host = "https://localhost:44374/";

        public static string MyChatsMainUrl() {
            return "profile/Achat";
        }

        public static string MessageMainUrl() {
            return "message";
        }

        public static string AdMainUrl() {
            return "ad";
        }

        public static string CustomerMainUrl() {
            return "customer";
        }

        public static string ProfileMainUrl() {
            return "profile";
        }

        public static string CategoriesMainUrl() {
            return "category";
        }
        public static string SubcategoriesMainUrl(Category category) {
            return $"category/{category.Id}";
        }

        public static string ConditionsMainUrl() {
            return "condition";
        }

        public static string StatesMainUrl() {
            return "state";
        }

        public static string TypeMainUrl() {
            return "type";
        }

        public static string ManufacturersMainUrl() {
            return "manufacturer";
        }

    }
}
