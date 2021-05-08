using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class ChatRequestNotificationViewModel : NotificationViewModel {

        private IChatService chatService;

        public ICommand AcceptCommand { get; set; }
        public ICommand DeclineCommand { get; set; }
        public ChatRequest ChatRequest { get; set; }


        public ChatRequestNotificationViewModel(IChatService chatService,ChatRequest chatRequest) {
            this.chatService = chatService;
            this.ChatRequest = chatRequest;
            AcceptCommand = new AsyncCommand(AcceptChatReuqest);
            DeclineCommand = new AsyncCommand(DeclineChatRequest);
        }

        private async Task AcceptChatReuqest() {
            await chatService.AcceptChatRequest(ChatRequest);
            await Mediator.Notify(MediatorToken.ChatRequestManagedInNotificationsToken, ChatRequest);
        }

        private async Task DeclineChatRequest() {
            await chatService.DeclineChatRequest(ChatRequest);
            await Mediator.Notify(MediatorToken.ChatRequestManagedInNotificationsToken, ChatRequest);
        }



    }
}
