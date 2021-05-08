﻿
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
using WpfClientt.model.chat;

namespace WpfClientt.services {
    class ChatServiceSignalR : IChatService {

        private static ChatServiceSignalR instance;

        private ICustomerNotifier notifier;
        private ConcurrentBag<Func<Message, Task>> messageListeners = new ConcurrentBag<Func<Message, Task>>();
        private ConcurrentBag<Func<ChatRequest,Task>> chatRequestListeners = new ConcurrentBag<Func<ChatRequest, Task>>();
        private ConcurrentBag<Func<Chat, Task>> activeChatListeners = new ConcurrentBag<Func<Chat, Task>>();
        private ConcurrentBag<Func<Typing, Task>> typingListeners = new ConcurrentBag<Func<Typing, Task>>();
        private JsonSerializerOptions options;
        private HttpClient client;
        private HubConnection hubConnection;
        private IAdService adService;
        private ICustomerService customerService;

        private ChatServiceSignalR(HttpClient client,HubConnection hubConnection,IAdService adService,ICustomerService customerService,ICustomerNotifier notifier) {
            this.client = client;
            this.hubConnection = hubConnection;
            this.adService = adService;
            this.customerService = customerService;
            this.notifier = notifier;
            this.hubConnection.On("ReceiveMessage", async (string message) => await ReceiveMessage(message));
            this.hubConnection.On("ReceiveActiveChat", async (string message) => await ReceiveActiveChat(message));
            this.hubConnection.On("ReceiveChatRequest",async (string message) => await ReceiveChatRequest(message));
        }

        public static async Task<ChatServiceSignalR> GetInstance(HttpClient client, JsonSerializerOptions options, IAdService adService, ICustomerService customerService,ICustomerNotifier notifier) {
            if (instance == null) {
                string token = client.DefaultRequestHeaders.Authorization.ToString().Replace("Bearer ", ""); 
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(
                        ApiInfo.ChatHubMainUrl(),
                        config => { config.AccessTokenProvider = () => Task.FromResult(token); }
                    )
                    .Build();
                instance = new ChatServiceSignalR(client, connection, adService, customerService,notifier);
                instance.options = options;
                await connection.StartAsync();
            }

            return instance;
        }

        public void AddMessageListener(Func<Message, Task> listenerProvider) {
            messageListeners.Add(listenerProvider);
        }
        public void RemoveMessageListener(Func<Message, Task> listenerProvider) {
            messageListeners.TryTake(out listenerProvider);
        }


        public void AddChatRequestListener(Func<ChatRequest, Task> listenerProvider) {
            chatRequestListeners.Add(listenerProvider);
        }

        public void RemoveChatRequestListener(Func<ChatRequest, Task> listenerProvider) {
            chatRequestListeners.TryTake(out listenerProvider);
        }

        public void AddActiveChatListener(Func<Chat, Task> listener) {
            activeChatListeners.Add(listener);
        }

        public void RemoveActiveChatListener(Func<Chat, Task> listener) {
            activeChatListeners.TryTake(out listener);
        }

        public void AddChatTypingListener(Func<Typing, Task> listener) {
            typingListeners.Add(listener);
        }

        public void RemoveChatTypingListener(Func<Typing, Task> listener) {
            typingListeners.TryTake(out listener);
        }

        public async Task SendMessage(Message message) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiInfo.MessageMainUrl());
            request.Content = new StringContent(
                JsonSerializer.Serialize(
                    new { MessageText = message.Body, ActiveChat = message.ChatId.ToString() },options), 
                    Encoding.UTF8, 
                    "application/json"
                );
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }


        public async Task Typing() {
            await hubConnection.InvokeAsync("IamTyping");
        }

        public async Task<ISet<Chat>> Chats() {
            ISet<Chat> result = new HashSet<Chat>();
            ISet<ChatModel> serverChats = await ChatsFromServer();
            foreach (ChatModel chatmodel in serverChats) {
                result.Add(await MapChatServerToChat(chatmodel));
            }
            return result;
        }

        public IScroller<Message> Messages(Chat chat) {
            return new ChatScroller(chat,client);
        }

        public async Task<ISet<ChatRequest>> ChatRequests() {
            ISet<ChatRequest> result = new HashSet<ChatRequest>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.ChatRequestsMainUrl());
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
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
                if (response.IsSuccessStatusCode) {
                    notifier.Success($"Chat request for the ad {ad.Title} has been sent successfully.");
                } else {
                    notifier.Error($"Couldn't send chat request for the ad {ad.Title}.You've probably already have sent a chat request for this ad.");
                }
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


        private async Task ReceiveActiveChat(string message) {
            int chatId = int.Parse(message.Substring(message.IndexOf(":") + 1).Trim());

            ISet<ChatModel> chats = await ChatsFromServer();

            bool isCustomersChat = false;
            ChatModel foundChat = null;

            foreach (ChatModel chat in chats) {
                if (chat.ChatId.Equals(chatId)) {
                    isCustomersChat = true;
                    foundChat = chat;
                    break;
                }
            }

            if (isCustomersChat) {
                Chat chat = await MapChatServerToChat(foundChat);
                foreach (Func<Chat, Task> activeChatListener in activeChatListeners) {
                    await activeChatListener.Invoke(chat);
                }
            }
        }

        private async Task ReceiveChatRequest(string message) {
            long adId = long.Parse(message.Substring(message.IndexOf(":") + 1).Trim());

            IScroller<Ad> scroller = adService.ProfileAds();
            await scroller.Init();

            bool isCustomersAd = false; //Indicates whether the ad,for wich the chat request is happening,belongs to the logged in customer.

            while (!isCustomersAd) {
                foreach (Ad ad in scroller.CurrentPage().Objects()) {
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
                foreach (Func<ChatRequest, Task> listener in chatRequestListeners) {
                    await listener.Invoke( (await ChatRequests()).First() );
                }
            }
        }

        private Task ReceiveMessage(string message) {
            return Task.CompletedTask;
        }

        private async Task<ISet<ChatModel>> ChatsFromServer() {
            ISet<ChatModel> result = new HashSet<ChatModel>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiInfo.MyChatsMainUrl());
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                ChatModel[] chats = await JsonSerializer.DeserializeAsync<ChatModel[]>(await response.Content.ReadAsStreamAsync(), options);
                foreach (ChatModel chat in chats) {
                    result.Add(chat);
                }
            }
            return result;
        }

        private async Task<Chat> MapChatServerToChat(ChatModel model) {
            return new Chat() {
                ChatId = model.ChatId,
                Sold = model.Sold,
                LatestMessage = model.LatestMessage,
                Ad = await adService.ReadById(model.AdId),
                Customer = await customerService.ReadById(model.CustomerId)
            };
        }

    }
}
