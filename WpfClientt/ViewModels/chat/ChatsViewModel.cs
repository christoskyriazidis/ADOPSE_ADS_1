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
        private IChatService chatService;
        public ChatViewModel SelectedChat { get; private set; }
        public ObservableCollection<ChatBoxViewModel> Chats { get; private set; } = new ObservableCollection<ChatBoxViewModel>();


        private ChatsViewModel(IChatService chatService, ISet<Chat> chats, ChatViewModel selectedChat) {
            this.chatService = chatService;
            Mediator.Subscribe(MediatorToken.ChangeChatViewToken, SelectChat);
            foreach (Chat chat in chats) {
                Chats.Add(new ChatBoxViewModel(chat,chatService));
            }
            SelectedChat = selectedChat;
            chatService.AddActiveChatListener(ActiveChatListener);
        }

        private async Task SelectChat(object chat) {
            ISet<Message> messages = await GetAllMessagesOfChat((Chat)chat, chatService);
            SelectedChat = new ChatViewModel((Chat)chat,chatService,messages);
            OnPropertyChanged(nameof(SelectedChat));
        }
     

        public static async Task<ChatsViewModel> GetInstance(FactoryServices factory) {
            if (instance == null) {
                IChatService chatService = await factory.ChatServiceInstance();
                ISet<Chat> chats = await chatService.Chats();
                ChatViewModel selected = chats.Count > 0 ? new ChatViewModel(chats.First(), chatService, await GetAllMessagesOfChat(chats.First(), chatService)) : null;
                instance = new ChatsViewModel(chatService, chats, selected);
            }
            return instance;
        }

        private Task ActiveChatListener(Chat chat) {
            Chats.Insert(0,new ChatBoxViewModel(chat,chatService));
            return Task.CompletedTask;
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
