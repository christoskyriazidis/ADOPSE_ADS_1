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
        public ChatRequest ChatRequest { get; set; }
        public ICommand AcceptChatRequestCommand { get; set; }
        public ICommand DeclineChatRequestCommand { get; set; }

        public override NotificationDisplayPart DisplayPart => displayPart;

        public ChatRequestToastViewModel(MessageOptions options, IChatService chatService, ChatRequest request) 
            : base(string.Empty, options) {
            this.ChatRequest = request;
            this.displayPart = new ChatRequestToastNotification(this);
            AcceptChatRequestCommand = new AsyncCommand(
                async () => { 
                    await chatService.AcceptChatRequest(request);
                    await Mediator.Notify(MediatorToken.ChatRequestManagedInToastToken, request);
                    displayPart.OnClose();
                });
            DeclineChatRequestCommand = new AsyncCommand(async () => { 
                await chatService.DeclineChatRequest(request);
                await Mediator.Notify(MediatorToken.ChatRequestManagedInToastToken, request);
                displayPart.OnClose();
            });
            Mediator.Subscribe(MediatorToken.ChatRequestManagedInNotificationsToken, (_) => {
                displayPart.OnClose();
                return Task.CompletedTask;
            }); ;
        }

    }
}
