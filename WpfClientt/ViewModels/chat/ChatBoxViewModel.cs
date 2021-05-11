using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class ChatBoxViewModel : BaseViewModel {
        private Chat chat;

        public Customer Customer { get; set; }

        public Ad Ad { get; set; }

        public string LatestMessage { get; set; }
        public ICommand SelectChatCommand { get; set; }

        public ChatBoxViewModel(Chat chat, IChatService chatService) {
            this.chat = chat;
            Customer = chat.Customer;
            Ad = chat.Ad;
            LatestMessage = chat.LatestMessage;
            chatService.AddMessageListener(MessageListener);
            SelectChatCommand = new AsyncCommand(SelectChat);
        }

        private Task MessageListener(Message message) {
            if (message.ChatId.Equals(chat.ChatId)) {
                LatestMessage = message.Body;
                OnPropertyChanged(nameof(LatestMessage));
            }

            return Task.CompletedTask;
        }

        private async Task SelectChat() {
            await Mediator.Notify(MediatorToken.ChangeChatViewToken, chat);
        }
    }
}
