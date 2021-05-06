
using Microsoft.AspNetCore.SignalR.Client;
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
    class ChatServiceSignalR : IChatService {

        private static ChatServiceSignalR instance;

        private ConcurrentBag<Func<Task>> messageListeners = new ConcurrentBag<Func<Task>>();
        private ConcurrentBag<Func<Task>> chatRequestListeners = new ConcurrentBag<Func<Task>>();
        private JsonSerializerOptions options;
        private HttpClient client;
        private HubConnection hubConnection;
        private IAdService adService;
        private ICustomerService customerService;

        private ChatServiceSignalR(HttpClient client,JsonSerializerOptions options,HubConnection hubConnection,IAdService adService,ICustomerService customerService) {
            this.client = client;
            this.options = options;
            this.hubConnection = hubConnection;
            this.adService = adService;
            this.customerService = customerService;
            this.hubConnection.On("ReceiveMessage", async (string message) => await ReceiveMessage(message));
            this.hubConnection.On("ReceiveChatRequest",async (string message) => await ReceiveChatRequest(message));
        }

        private async Task ReceiveChatRequest(string message) {
            long adId = long.Parse(message.Substring(message.IndexOf(":") + 1).Trim());

            IScroller<Ad> scroller = adService.ProfileAds();
            await scroller.Init();

            bool isCustomersAd = false; //Indicates whether the ad,for wich the chat request is happening,belongs to the logged in customer.

            while (!isCustomersAd) {
                foreach(Ad ad in scroller.CurrentPage().Objects()) {
                    if (ad.Id.Equals(adId)) {
                        isCustomersAd = true;
                        break;
                    }
                }
                if (!await scroller.MoveNext()) {
                    break;
                }
            }

            if (isCustomersAd) {
                foreach(Func<Task> listener in chatRequestListeners) {
                    await listener.Invoke();
                }
            }
        }

        private async Task ReceiveMessage(string message) { 
        
        }

        public static async Task<ChatServiceSignalR> GetInstance(HttpClient client,JsonSerializerOptions options,IAdService adService,ICustomerService customerService) {
            if(instance == null) {
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(ApiInfo.ChatHubMainUrl(), 
                    config => { config.AccessTokenProvider = () => Task.FromResult(client.DefaultRequestHeaders.Authorization.ToString().Replace("Bearer ","")); })
                    .Build();
                instance = new ChatServiceSignalR(client, options, connection,adService,customerService);
                await connection.StartAsync();
            }

            return instance;
        }

        public void AddMessageListener(Func<Task> listenerProvider) {
            messageListeners.Add(listenerProvider);
        }
        public void RemoveMessageListener(Func<Task> listenerProvider) {
            messageListeners.TryTake(out listenerProvider);
        }


        public void AddChatRequestListener(Func<Task> listenerProvider) {
            chatRequestListeners.Add(listenerProvider);
        }

        public void RemoveChatRequestListener(Func<Task> listenerProvider) {
            chatRequestListeners.TryTake(out listenerProvider);
        }

        public async Task SendMessage(Message message) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiInfo.MessageMainUrl());
            request.Content = new StringContent(JsonSerializer.Serialize(message,options));

            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }


        public async Task Typing() {
            await hubConnection.InvokeAsync("IamTyping");
        }

        public async Task<ISet<Chat>> Chats() {
            ISet<Chat> result = new HashSet<Chat>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.MyChatsMainUrl());
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                Chat[] chats = await JsonSerializer.DeserializeAsync<Chat[]>(await response.Content.ReadAsStreamAsync(), options);
                foreach(Chat chat in chats) {
                    result.Add(chat);
                }
            }
            return result;
        }

        public IScroller<Message> Messages(int chatId) {
            return new ChatScroller(chatId,client);
        }

        public async Task<ISet<ChatRequest>> ChatRequests() {
            ISet<ChatRequest> result = new HashSet<ChatRequest>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.ChatRequestsMainUrl());
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                ChatRequestModel[] chatRequests = JsonSerializer.Deserialize<ChatRequestModel[]>(await response.Content.ReadAsStringAsync(), options);
                foreach(ChatRequestModel chatRequest in chatRequests) {
                    result.Add(new ChatRequest() {
                        Id = chatRequest.Id,
                        Timestamp = chatRequest.Timestamp,
                        Ad = await adService.ReadById(chatRequest.AdId),
                        Buyer = await customerService.ReadById(chatRequest.BuyerId)
                    });
                }
            }
            return result;
        }

        public async Task SendChatRequest(Ad ad) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiInfo.ChatRequestMainUrl(ad));
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task AcceptChatRequest(ChatRequest chatRequest) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiInfo.ConfirmChatRequestMainUrl(chatRequest));
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeclineChatRequest(ChatRequest chatRequest) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiInfo.DeclineChatRequestMainUrl(chatRequest));
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

    }
}
