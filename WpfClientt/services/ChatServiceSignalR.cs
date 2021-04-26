using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model.chat;

namespace WpfClientt.services {
    class ChatServiceSignalR : IChatService {
        private static ChatServiceSignalR instance;

        private ConcurrentBag<Action<Message>> listeners = new ConcurrentBag<Action<Message>>();
        private IHubProxy proxy;

        private ChatServiceSignalR() {
            
        }

        private static async Task<ChatServiceSignalR> GetInstance() {
            ChatServiceSignalR instance = new ChatServiceSignalR();
            HubConnection connection = new HubConnection("https://localhost:44374/SignalR");
            IHubProxy proxy = connection.CreateHubProxy("ChatHub");

            proxy.On("ReceiveMessage", (Message message) => {
                foreach (Action<Message> listener in instance.listeners.ToArray()) {
                    listener.Invoke(message);
                }
            });

            await connection.Start();

            return instance;
        }

        public void AddMessageListener(Action<Message> listener) {
            listeners.Add(listener);
        }

        public async Task SendMessage(Message message) {
            return;
        }

        public void RemoveMessageListener(Action<Message> listener) {
            listeners.TryTake(out listener);
        }

        public Task Typing() {
            throw new NotImplementedException();
        }
    }
}
