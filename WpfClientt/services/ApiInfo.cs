using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    static class ApiInfo {

        public static string AdMainUrl() {
            return "https://localhost:44374/ad";
        }

        public static string CustomerMainUrl() {
            return "https://localhost:44374/customer";
        }

        public static string ProfileMainUrl() {
            return "https://localhost:44374/profile";
        }

        public static string CategoriesMainUrl() {
            return "https://localhost:44374/category";
        }
        public static string SubcategoriesMainUrl(Category category) {
            return $"https://localhost:44374/category/{category.Id}";
        }

        public static string ConditionsMainUrl() {
            return "https://localhost:44374/condition";
        }

        public static string StatesMainUrl() {
            return "https://localhost:44374/state";
        }

        public static string TypeMainUrl() {
            return "https://localhost:44374/type";
        }

        public static string ManufacturersMainUrl() {
            return "https://localhost:44374/manufacturer";
        }

    }
}
