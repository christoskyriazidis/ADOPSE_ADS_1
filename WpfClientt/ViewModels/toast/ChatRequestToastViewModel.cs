using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications.Core;
using WpfClientt.model;
using WpfClientt.services;
using WpfClientt.views;

namespace WpfClientt.viewModels {
    public class ChatRequestToastViewModel : NotificationBase {
        private NotificationDisplayPart displayPart;
        private IChatService chatService;
        public ChatRequest ChatRequest { get; set; }
        public ICommand AcceptChatRequestCommand { get; set; }
        public ICommand DeclineChatRequestCommand { get; set; }

        public override NotificationDisplayPart DisplayPart => displayPart;

        public ChatRequestToastViewModel(MessageOptions options, IChatService chatService, ChatRequest request) 
            : base(string.Empty, options) {
            this.ChatRequest = request;
            this.displayPart = new ChatRequestToastView(this);
            this.chatService = chatService;
            AcceptChatRequestCommand = new AsyncCommand(AcceptChatRequest);
            DeclineChatRequestCommand = new AsyncCommand(DeclineChatRequest);
            Mediator.Subscribe(MediatorToken.ChatRequestManagedInNotificationsToken, (_) => {
                displayPart.OnClose();
                return Task.CompletedTask;
            }); ;
        }

        private async Task AcceptChatRequest() {
            await chatService.AcceptChatRequest(ChatRequest);
            await Mediator.Notify(MediatorToken.ChatRequestManagedInToastToken, ChatRequest);
            displayPart.OnClose();
        }

        private async Task DeclineChatRequest() {
            await chatService.DeclineChatRequest(ChatRequest);
            await Mediator.Notify(MediatorToken.ChatRequestManagedInToastToken, ChatRequest);
            displayPart.OnClose();
        }

    }
}
