using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class ChatViewModel : BaseViewModel {

        private IChatService chatService;

        public Chat Chat { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        public string ButtonText { get; set; }

        public string MessageBody { get; set; }

        public ChatViewModel(Chat chat, IChatService chatService,ISet<Message> messages) {
            SendMessageCommand = new AsyncCommand(SendMessage);
            foreach (Message message in messages) {
                Messages.Add(message);
            }
            this.Chat = chat;
            ButtonText = !chat.Sold ? "Send Message" : "The item is sold!You can't send messages!";
            this.chatService = chatService;
        }

        private async Task SendMessage() {
            if(MessageBody.Trim().Length < 3) {
                return;
            }

            await chatService.SendMessage(new Message() { Body = MessageBody,ChatId = Chat.ChatId });
        }

       

    }

}

