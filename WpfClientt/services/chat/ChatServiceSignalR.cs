
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

        private ConcurrentBag<Action<Message>> listeners = new ConcurrentBag<Action<Message>>();
        private JsonSerializerOptions options;
        private HttpClient client;

        private ChatServiceSignalR(HttpClient client,JsonSerializerOptions options,HubConnection hubConnection) {
            this.client = client;
            this.options = options;
            hubConnection.On("ReceiveMessage", (Message message) => {
                foreach (Action<Message> listener in instance.listeners.ToArray()) {
                    listener.Invoke(message);
                }
            });

        }

        public static async Task<ChatServiceSignalR> GetInstance(HttpClient client,JsonSerializerOptions options) {
            Debug.WriteLine(client.DefaultRequestHeaders.Authorization.ToString());
            if(instance == null) {
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(ApiInfo.ChatHubMainUrl(), 
                    config => { config.AccessTokenProvider = () => Task.FromResult(client.DefaultRequestHeaders.Authorization.ToString()); }
                    ).Build();
                instance = new ChatServiceSignalR(client, options, connection);
                await connection.StartAsync();
            }

            return instance;
        }

        public void AddMessageListener(Action<Message> listener) {
            listeners.Add(listener);
        }

        public async Task SendMessage(Message message) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ApiInfo.MessageMainUrl());
            request.Content = new StringContent(JsonSerializer.Serialize(message,options));

            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

        public void RemoveMessageListener(Action<Message> listener) {
            listeners.TryTake(out listener);
        }

        public Task Typing() {
            throw new NotImplementedException();
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
    }
}
