﻿using System;
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

    }
}
