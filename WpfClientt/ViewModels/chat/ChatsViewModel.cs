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
    public class ChatsViewModel : IViewModel {
        private static ChatsViewModel instance;

        private IChatService chatService;
        public ObservableCollection<Chat> Chats { get; private set; } = new ObservableCollection<Chat>();
        public ObservableCollection<Message> Messages { get; private set; } = new ObservableCollection<Message>();
        public ICommand SendCommand { get; private set; }
        public Message Message { get; private set; } = new Message();

        private ChatsViewModel(IChatService chatService,ISet<Chat> chats) {
            this.chatService = chatService;
            SendCommand = new AsyncCommand(SendMessage);
            foreach(Chat chat in chats) {
                Chats.Add(chat);
            }
        }

        public static async Task<ChatsViewModel> GetInstance(FactoryServices factory) {
            if(instance == null) {
                IChatService chatService = await factory.ChatServiceInstance();
                ISet<Chat> chats = await chatService.Chats();
                instance = new ChatsViewModel(chatService,chats);
            }
            return instance;
        }

        public async Task SendMessage() {
            await chatService.SendMessage(Message);
        }
        


    }
}
