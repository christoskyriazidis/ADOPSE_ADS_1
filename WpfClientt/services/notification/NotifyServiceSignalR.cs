using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.model.notification;

namespace WpfClientt.services {
    class NotifyServiceSignalR : INotifyService {
        private static NotifyServiceSignalR instance;

        private HttpClient client;
        private JsonSerializerOptions options;
        private HubConnection hubConnection;
        private IAdDetailsService adDetailsService;

        private NotifyServiceSignalR(HttpClient client,JsonSerializerOptions options,
            HubConnection hubConnection,IAdDetailsService adDetailsService) {
            this.client = client;
            this.options = options;
            this.hubConnection = hubConnection;
            this.adDetailsService = adDetailsService;
        }

        public static Task<NotifyServiceSignalR> GetInstance(HttpClient client,JsonSerializerOptions options, IAdDetailsService adDetailsService) {
            if(instance == null) {
                string token = client.DefaultRequestHeaders.Authorization.ToString().Replace("Bearer ", "");
                HubConnection hubConnection = new HubConnectionBuilder()
                    .WithUrl(
                        ApiInfo.NotificationHubMainUrl(),
                        config => { config.AccessTokenProvider = () => Task.FromResult(token); }
                    ).Build();
                instance = new NotifyServiceSignalR(client, options, hubConnection,adDetailsService);
            }

            return Task.FromResult(instance);
        }

        public void AddSubcategoryChangedListener() {
            throw new NotImplementedException();
        }

        public Task AddToWishList(Ad ad) {
            throw new NotImplementedException();
        }

        public void AddWishListChangedListener() {
            throw new NotImplementedException();
        }

        public IScroller<AdChangedNotification> Notifications() {
            throw new NotImplementedException();
        }

        public Task NotificationSeen(AdChangedNotification notification) {
            throw new NotImplementedException();
        }

        public Task RemoveFromWishList(Ad ad) {
            throw new NotImplementedException();
        }

        public void RemoveSubcategoryChangedListener() {
            throw new NotImplementedException();
        }

        public void RemoveWishListChangedListener() {
            throw new NotImplementedException();
        }

        public async Task<ISet<Subcategory>> SubscribedSubcategories() {
            ISet<Subcategory> subscribedSubcategories = new HashSet<Subcategory>();
            ISet<Subcategory> subcategories = await adDetailsService.Subcategories();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.SubscribedSubcategoriesMainUrl());
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                int[] subcategoriesIds = await JsonSerializer.DeserializeAsync<int[]>(await response.Content.ReadAsStreamAsync(),options);
                foreach(int id in subcategoriesIds) {
                    foreach(Subcategory subcategory in subcategories) {
                        if (subcategory.Id.Equals(id)) { subscribedSubcategories.Add(subcategory); }
                    }
                }
            }

            return subscribedSubcategories;
        }

        public async Task SubscriberToSubcategory(Subcategory subcategory) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiInfo.SubscribeSubcategoryMainUrl(subcategory));
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task UnsubscribeFromSubcategories(Subcategory[] subcategories) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, ApiInfo.UnsubscribeFromSubcategoriesMainUrl());
            request.Content = new StringContent(
                JsonSerializer.Serialize(subcategories.Select(sub => sub.Id).ToArray(),options),
                Encoding.UTF8,
                "application/json"
                );
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

        public Task<ISet<Ad>> WishList() {
            throw new NotImplementedException();
        }
    }
}
