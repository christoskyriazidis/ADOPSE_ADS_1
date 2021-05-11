using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.model.chat;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class ChatViewModel : BaseViewModel{
        private IChatService chatService;
        private IAdService adService;

        public Chat Chat { get; set; }

        public Visibility SellButtonVisibility { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public ICommand SellCommand { get; set; }

        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        public string ButtonText { get; set; }

        public string MessageBody { get; set; }

        public ChatViewModel(Chat chat, IChatService chatService, IAdService adService,ISet<Message> messages,bool isCustomersAd) {
            SendMessageCommand = new AsyncCommand(SendMessage);
            SellCommand = new AsyncCommand(SellAd);
            foreach (Message message in messages) {
                Messages.Insert(0, message);
            }
            if(chat.Sold || !isCustomersAd) {
                SellButtonVisibility = Visibility.Hidden;
            } else {
                SellButtonVisibility = Visibility.Visible;
            }
            Chat = chat;
            ButtonText = !chat.Sold ? "Send Message" : "The item is sold!You can't send messages!";
            this.chatService = chatService;
            this.chatService.AddMessageListener(MessageListener);
            this.adService = adService;
        }

        private async Task SellAd() {
            await adService.SellAd(Chat.Ad, Chat.Customer);
            Chat.Sold = true;
            SellButtonVisibility = Visibility.Hidden;
            OnPropertyChanged(nameof(Chat.Sold));
            OnPropertyChanged(nameof(SellButtonVisibility));
        }

        private Task MessageListener(Message message) {
            if (message.ChatId.Equals(Chat.ChatId)) {
                Messages.Add(message);
            }
            return Task.CompletedTask;
        }

        private async Task SendMessage() {
            if (MessageBody.Trim().Length < 3) {
                return;
            }
            Message message = new Message() { 
                Body = MessageBody, ChatId = Chat.ChatId
            };
            await chatService.SendMessage(message);
            MessageBody = string.Empty;
            OnPropertyChanged(nameof(MessageBody));
        }

    }
}
