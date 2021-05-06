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
    public class AdDetailsViewModel : BaseViewModel, IViewModel {

        private IChatService chatService;

        public Ad Ad { get; set; }
        public Customer Customer{get;set;}

        public ICommand RequestChatCommand { get; set; }

        private AdDetailsViewModel(Ad ad,IChatService chatService,Customer customer) {
            this.Ad = ad;
            this.Customer = customer;
            this.chatService = chatService;
            RequestChatCommand = new AsyncCommand(RequestChat);
        }

        private async Task RequestChat() {
            await chatService.SendChatRequest(Ad);
        }

        public async static Task<AdDetailsViewModel> GetInstance(Ad ad,FactoryServices factory) {
            return new AdDetailsViewModel(ad, await factory.ChatServiceInstance(),await factory.CustomerServiceInstance().ReadById(ad.CustomerId));
        }

    }
}
