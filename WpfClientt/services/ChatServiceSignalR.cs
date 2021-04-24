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

        public ChatServiceSignalR(HubConnection connection) {
            this.proxy = connection.CreateHubProxy("ChatHub");
            this.proxy.On("ReceiveMessage", (Message message) => {
                foreach(Action<Message> listener in listeners.ToArray()) {
                    listener.Invoke(message);
                }
            });
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
