using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    class NotificationServiceSignalR : INotificationService {
        private static NotificationServiceSignalR instance;

        private HttpClient client;
        private JsonSerializerOptions options;
        private HubConnection hubConnection;
        private IAdDetailsService adDetailsService;
        private ICustomerService customerService;
        private IAdService adService;
        private ConcurrentBag<Func<ReviewAdNotification, Task>> reviewListeners = new ConcurrentBag<Func<ReviewAdNotification, Task>>();
        private ConcurrentBag<Func<Ad, Task>> soldAdListeners = new ConcurrentBag<Func<Ad, Task>>();

        private NotificationServiceSignalR(HttpClient client,HubConnection hubConnection,IAdDetailsService adDetailsService,IAdService adService,ICustomerService customerService) {
            this.client = client;
            this.hubConnection = hubConnection;
            this.adDetailsService = adDetailsService;
            this.adService = adService;
            this.customerService = customerService;
            this.hubConnection.On<int>("ReceiveReviewNotificationsWpf", ReceiveReviewNotification);
        }

        public static async Task<NotificationServiceSignalR> GetInstance(HttpClient client,JsonSerializerOptions options, IAdDetailsService adDetailsService,IAdService adService,ICustomerService customerService) {
            if(instance == null) {
                string token = client.DefaultRequestHeaders.Authorization.ToString().Replace("Bearer ", "");
                HubConnection hubConnection = new HubConnectionBuilder()
                    .WithUrl(
                        ApiInfo.NotificationHubMainUrl(),
                        config => { config.AccessTokenProvider = () => Task.FromResult(token); }
                    ).WithAutomaticReconnect()
                    .ConfigureLogging(logging => {
                        logging.AddDebug();
                        logging.SetMinimumLevel(LogLevel.Debug);
                    }).Build();
                instance = new NotificationServiceSignalR(client, hubConnection,adDetailsService,adService,customerService);
                instance.options = options;
                await hubConnection.StartAsync();
            }

            return instance;
        }

        public async Task<ISet<ReviewAdNotification>> ReviewAdNotifications() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.ReviewNotificationsMainUrl());
            ISet<ReviewAdNotification> notifications = new HashSet<ReviewAdNotification>();

            using(HttpResponseMessage response = await client.SendAsync(request)) {
                ReviewNotification[] reviewNotifications = await JsonSerializer
                                                            .DeserializeAsync<ReviewNotification[]>(await response.Content.ReadAsStreamAsync(), options);
                foreach(ReviewNotification reviewNotification in reviewNotifications) {
                    Ad ad = await adService.ReadById(reviewNotification.AdId);
                    Customer customer = await customerService.ReadById(reviewNotification.CustomerId);
                    string timestamp = new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(reviewNotification.Timestamp)).ToString("MMMM dd yyyy hh:mm tt");
                    notifications.Add(new ReviewAdNotification(ad, timestamp,customer));
                }

            }

            return notifications;
        }


        public Task<ISet<SubcategoryChangedNotification>> SubcategoryNotifications() {
            throw new NotImplementedException();
        }


        public Task<ISet<WishlistAdChangedNotification>> WishlistAdNotifications() {
            throw new NotImplementedException();
        }


        public void AddReviewNotificationListener(Func<ReviewAdNotification, Task> listener) {
            reviewListeners.Add(listener);
        }

        public void RemoveReviewNotificationListener(Func<ReviewAdNotification, Task> listener) {
            reviewListeners.Add(listener);
        }

        public void AddAdSoldListener(Func<Ad, Task> listener) {
            soldAdListeners.Add(listener);
        }

        public void RemoveAdSoldListener(Func<Ad, Task> listener) {
            soldAdListeners.TryTake(out listener);
        }

        public void AddSubcategoryChangedListener(Action notifier) {
            throw new NotImplementedException();
        }

        public Task AddToWishList(Ad ad) {
            throw new NotImplementedException();
        }

        public void AddWishListChangedListener(Action notifier) {
            throw new NotImplementedException();
        }

        public Task NotificationSeen(AdNotification notification) {
            throw new NotImplementedException();
        }

        public Task RemoveFromWishList(Ad ad) {
            throw new NotImplementedException();
        }


        public void RemoveSubcategoryChangedListener(Action notifier) {
            throw new NotImplementedException();
        }

        public void RemoveWishListChangedListener(Action notifier) {
            throw new NotImplementedException();
        }

        public async Task<ISet<Subcategory>> SubscribedSubcategories() {
            ISet<Subcategory> result = new HashSet<Subcategory>();
            ISet<Subcategory> subcategories = await adDetailsService.Subcategories();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.SubscribedSubcategoriesMainUrl());
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                SubscribedSubcategories subscribedSubcategories = await JsonSerializer.DeserializeAsync<SubscribedSubcategories>(await response.Content.ReadAsStreamAsync(),options);
                foreach(SubscribedSubcategory subscribedSubcategory in subscribedSubcategories.Subcategories) {
                    foreach(Subcategory subcategory in subcategories) {
                        if (subscribedSubcategory.SubcategoryId.Equals(subcategory.Id)) {
                            result.Add(subcategory);
                        }
                    }
                }
            }

            return result;
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
                JsonSerializer.Serialize(new DeleteFromCategorySub(subcategories)),
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


        private async void ReceiveReviewNotification(int adId) {
            await NotifySoldAdListeners(adId);
            ISet<ReviewAdNotification> notifications = await ReviewAdNotifications();
            bool isForCustomer = false;
            ReviewAdNotification foundNotification = null;

            foreach(ReviewAdNotification notification in notifications) {
                if (notification.Ad.Id.Equals(adId)) {
                    isForCustomer = true;
                    foundNotification = notification;
                    break;
                }
            }

            if (isForCustomer) {
                foreach(Func<ReviewAdNotification,Task> listener in reviewListeners) {
                    await listener.Invoke(foundNotification);
                }
            }

        }

        private async Task NotifySoldAdListeners(int adId) {
            Ad ad = await adService.ReadById(adId);
            foreach(Func<Ad,Task> listener in soldAdListeners) {
                await listener.Invoke(ad);
            }
        }

    }
}
