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
    public class ChatsViewModel : BaseViewModel, IViewModel {
        private static ChatsViewModel instance;
        private Customer LoggedInCustomer;
        private IChatService chatService;
        public ChatViewModel SelectedChat { get; private set; }
        public ObservableCollection<Chat> Chats { get; private set; } = new ObservableCollection<Chat>();
        public ICommand SelectChatCommand { get; set; }

        private ChatsViewModel(IChatService chatService, ISet<Chat> chats, ChatViewModel selectedChat,Customer loggedInCustomer) {
            this.LoggedInCustomer = loggedInCustomer;
            this.chatService = chatService;
            foreach (Chat chat in chats) {
                Chats.Add(chat);
            }
            SelectedChat = selectedChat;
            SelectChatCommand = new AsyncCommand<Chat>(SelectChat);
            chatService.AddActiveChatListener(ActiveChatListener);
        }

        public static async Task<ChatsViewModel> GetInstance(FactoryServices factory) {
            Customer profile = await factory.CustomerServiceInstance().Profile();
            if (instance == null) {
                IChatService chatService = await factory.ChatServiceInstance();
                ISet<Chat> chats = await chatService.Chats();
                ChatViewModel selected = chats.Count > 0 ? new ChatViewModel(chats.First(), chatService, await GetAllMessagesOfChat(chats.First(), chatService),profile) : null;
                instance = new ChatsViewModel(chatService, chats, selected, profile);
            }
            return instance;
        }

        private Task ActiveChatListener(Chat chat) {
            Chats.Add(chat);
            return Task.CompletedTask;
        }

        private async Task SelectChat(Chat chat) {
            SelectedChat = new ChatViewModel(chat, chatService, await GetAllMessagesOfChat(chat, chatService),LoggedInCustomer);
            OnPropertyChanged(nameof(SelectedChat));
        }

        private static async Task<ISet<Message>> GetAllMessagesOfChat(Chat chat, IChatService chatService) {
            IScroller<Message> scroller = chatService.Messages(chat);
            await scroller.Init();
            ISet<Message> messages = new HashSet<Message>();
            foreach (Message message in scroller.CurrentPage().Objects()) {
                messages.Add(message);
            }
            while (await scroller.MoveNext()) {
                foreach (Message message in scroller.CurrentPage().Objects()) {
                    messages.Add(message);
                }
            }

            return messages;
        }

    }
}
