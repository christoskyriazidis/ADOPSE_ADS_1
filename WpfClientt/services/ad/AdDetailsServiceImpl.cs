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
        private ISet<Category> categories;
        private ISet<Category> categoriesWithSubcategories;
        private ISet<Condition> conditions;
        private ISet<Manufacturer> manufacturers;
        private ISet<State> states;
        private ISet<AdType> types;
        private ISet<Subcategory> subcategories;

        public AdDetailsServiceImpl(HttpClient httpClient) {
            this.httpClient = httpClient;
        }

        public async Task<ISet<Category>> Categories() {
            if(categories != null) {
                return categories;
            }
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.CategoriesMainUrl());
            categories = new HashSet<Category>();
            using(HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach(Category category in await JsonSerializer.DeserializeAsync<Category[]>(content)) {
                    categories.Add(category);
                }
            }

            return categories;
        }

        public async Task<ISet<Category>> CategoriesWithSubcategories() {
            if(categoriesWithSubcategories == null) {
                categoriesWithSubcategories = new HashSet<Category>();
                ISet<Category> categories = await Categories();

                foreach (Category category in categories) {
                    foreach (Subcategory subcategory in await SubcategoriesOf(category)) {
                        category.Subcategories.Add(subcategory);
                        categoriesWithSubcategories.Add(category);
                    }
                }
            }
            

            return categoriesWithSubcategories;
        }

        public async Task<ISet<Condition>> Conditions() {
            if(conditions != null) {
                return conditions;
            }
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.ConditionsMainUrl());
            conditions = new HashSet<Condition>();

            using(HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach(Condition condition in await JsonSerializer.DeserializeAsync<Condition[]>(content)) {
                    conditions.Add(condition);
                }
            }

            return conditions;
        }

        public async Task<ISet<Manufacturer>> Manufacturers() {
            if(manufacturers != null) {
                return manufacturers;
            }
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.ManufacturersMainUrl());
            manufacturers = new HashSet<Manufacturer>();

            using(HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach(Manufacturer manufacturer in await JsonSerializer.DeserializeAsync<Manufacturer[]>(content)) {
                    manufacturers.Add(manufacturer);
                }
            }

            return manufacturers;
        }

        public async Task<ISet<State>> States() {
            if(states != null) {
                return states;
            }
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.StatesMainUrl());
            states = new HashSet<State>();

            using(HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach(State state in await JsonSerializer.DeserializeAsync<State[]>(content)) {
                    states.Add(state);
                }
            }

            return states;
        }

        public async Task<ISet<Subcategory>> Subcategories() {
            if(subcategories != null) {
                return subcategories;
            }
            ISet<Category> categories = await Categories();
            subcategories = new HashSet<Subcategory>();

            foreach(Category category in categories) {
                foreach(Subcategory subcategory in await SubcategoriesOf(category)) {
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

        public async Task<ISet<AdType>> Types() {
            if(types != null) {
                return types;
            }
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.TypeMainUrl());
            types = new HashSet<AdType>();

            using(HttpResponseMessage response = await httpClient.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Stream content = await response.Content.ReadAsStreamAsync();

                foreach(AdType type in await JsonSerializer.DeserializeAsync<AdType[]>(content)) {
                    types.Add(type);
                }
            }

            return types;
        }

    }
}
