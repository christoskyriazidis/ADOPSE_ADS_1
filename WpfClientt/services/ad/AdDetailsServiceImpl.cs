using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    class AdDetailsServiceImpl : IAdDetailsService {

        private HttpClient httpClient;

        private object categoreisLock = new object();
        private Task<ISet<Category>> categoriesTask;

        private object categoriesWithSubcategoriesLock = new object();
        private Task<ISet<Category>> categoriesWithSubcategoriesTask;

        private object conditionsLock = new object();
        private Task<ISet<Condition>> conditionsTask;

        private object manufacturersLock = new object();
        private Task<ISet<Manufacturer>> manufacturersTask;

        private object statesLock = new object();
        private Task<ISet<State>> statesTask;

        private object subcategoriesLock = new object();
        private Task<ISet<Subcategory>> subcategoriesTask;

        private object adTypesLock = new object();
        private Task<ISet<AdType>> adTypesTask;

        public AdDetailsServiceImpl(HttpClient httpClient) {
            this.httpClient = httpClient;
        }

        public Task<ISet<Category>> Categories() {
            lock (categoreisLock) {
                if(categoriesTask == null) {
                    categoriesTask = DownloadCategoriesTask();
                }
            }
            return categoriesTask;
        }

        private async Task<ISet<Category>> DownloadCategoriesTask() {
            ISet<Category> categories = new HashSet<Category>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.CategoriesMainUrl());
            using (HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach (Category category in await JsonSerializer.DeserializeAsync<Category[]>(content)) {
                    categories.Add(category);
                }
            }
            return categories;
        }

        public Task<ISet<Category>> CategoriesWithSubcategories() {
            lock (categoriesWithSubcategoriesLock) {
                if(categoriesWithSubcategoriesTask == null) {
                    categoriesWithSubcategoriesTask = DownloadCategoriesWithSubcategoriesTask();
                }
            }

            return categoriesWithSubcategoriesTask;
        }

        private async Task<ISet<Category>> DownloadCategoriesWithSubcategoriesTask() {
            ISet<Category> categories = await Categories();
            ISet<Category> categoriesWithSubcategories = new HashSet<Category>();

            foreach (Category category in categories) {
                foreach (Subcategory subcategory in await SubcategoriesOf(category)) {
                    category.Subcategories.Add(subcategory);
                    categoriesWithSubcategories.Add(category);
                }
            }

            return categoriesWithSubcategories;
        }

        public Task<ISet<Condition>> Conditions() {
            lock (conditionsLock) {
                if(conditionsTask == null) {
                    conditionsTask = DownloadConditionsTask();
                }
            }
            return conditionsTask;
        }

        private async Task<ISet<Condition>> DownloadConditionsTask() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.ConditionsMainUrl());
            ISet<Condition> conditions = new HashSet<Condition>();

            using (HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();

                Stream content = await response.Content.ReadAsStreamAsync();

                foreach (Condition condition in await JsonSerializer.DeserializeAsync<Condition[]>(content)) {
                    conditions.Add(condition);
                }

            }
            return conditions;
        }

        public Task<ISet<Manufacturer>> Manufacturers() {
            lock (manufacturersLock) {
                if(manufacturersTask == null) {
                    manufacturersTask = DownloadManufacturersTask();
                }
            }

            return manufacturersTask;
        }

        private async Task<ISet<Manufacturer>> DownloadManufacturersTask() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.ManufacturersMainUrl());
            ISet<Manufacturer> manufacturers = new HashSet<Manufacturer>();

            using (HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach (Manufacturer manufacturer in await JsonSerializer.DeserializeAsync<Manufacturer[]>(content)) {
                    manufacturers.Add(manufacturer);
                }
            }

            return manufacturers;
        }

        public Task<ISet<State>> States() {
            lock (statesLock) {
                if(statesTask == null) {
                    statesTask = DownloadStatesTask();
                }
            }
            return statesTask;
        }

        private async Task<ISet<State>> DownloadStatesTask() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.StatesMainUrl());
            ISet<State> states = new HashSet<State>();

            using (HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach (State state in await JsonSerializer.DeserializeAsync<State[]>(content)) {
                    states.Add(state);
                }
            }

            return states;
        }

        public Task<ISet<Subcategory>> Subcategories() {
            lock (subcategoriesLock) {
                if(subcategoriesTask == null) {
                    subcategoriesTask = DownloadSubcategoriesTask();
                }
            }
            return subcategoriesTask;
        }

        private async Task<ISet<Subcategory>> DownloadSubcategoriesTask() {
            ISet<Category> categories = await Categories();
            ISet<Subcategory> subcategories = new HashSet<Subcategory>();

            foreach (Category category in categories) {
                foreach (Subcategory subcategory in await SubcategoriesOf(category)) {
                    subcategories.Add(subcategory);
                }
            }

            return subcategories;
        }

        public async Task<ISet<Subcategory>> SubcategoriesOf(Category category) {
            ISet<Subcategory> subcategories = new HashSet<Subcategory>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.SubcategoriesMainUrl(category));

            using (HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();
                foreach (Subcategory subcategory in await JsonSerializer.DeserializeAsync<Subcategory[]>(content)) {
                    subcategories.Add(subcategory);
                }
            }

            return subcategories;
        }

        public Task<ISet<AdType>> Types() {
            lock (adTypesLock) {
                if(adTypesTask == null) {
                    adTypesTask = DownloadAdTypesTask();
                }
            }
            

            return adTypesTask;
        }

        private async Task<ISet<AdType>> DownloadAdTypesTask() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.TypeMainUrl());
            ISet<AdType> types = new HashSet<AdType>();

            using (HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach (AdType type in await JsonSerializer.DeserializeAsync<AdType[]>(content)) {
                    types.Add(type);
                }
            }

            return types;
        }

    }
}
