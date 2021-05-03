using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private IHubProxy proxy;
        private JsonSerializerOptions options;
        private HttpClient client;

        private ChatServiceSignalR(HttpClient client,JsonSerializerOptions options) {
            this.client = client;
            this.options = options;
        }

        public static async Task<ChatServiceSignalR> GetInstance(HttpClient client,JsonSerializerOptions options) {
            if(instance == null) {
                ChatServiceSignalR instance = new ChatServiceSignalR(client, options);
                HubConnection connection = new HubConnection("https://localhost:44374/SignalR");
                IHubProxy proxy = connection.CreateHubProxy("ChatHub");

                proxy.On("ReceiveMessage", (Message message) => {
                    foreach (Action<Message> listener in instance.listeners.ToArray()) {
                        listener.Invoke(message);
                    }
                });

                await connection.Start();
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
            throw new NotImplementedException();
        }
    }
}
