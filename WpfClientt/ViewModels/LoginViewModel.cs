using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class LoginViewModel : IViewModel {

        private static LoginViewModel instance;
        private OpenIdConnectClient openIdConnectClient;

        public string LoginUrl { get; private set; }

        public ICommand ExchangeTokenCommand { get; private set; }

        private LoginViewModel(OpenIdConnectClient openIdConnectClient,string loginUrl) {
            this.openIdConnectClient = openIdConnectClient;
            ExchangeTokenCommand = new DelegateCommand(ExchangeToken);
            LoginUrl = loginUrl;
        }

        public static async Task<LoginViewModel> GetInstance(FactoryServices factory) {
            if(instance == null) {
                OpenIdConnectClient client = await factory.GetOpenIdConnectClient();
                instance = new LoginViewModel(client, await client.PrepareAuthorizationRequestUrl());
            }

            return instance;
        }

        private async void ExchangeToken(object redirectUri) {
            Mediator.Notify("DisplayPageView", "Authentication in progress");
            await openIdConnectClient.RetrieveAndSetAccessToken((string)redirectUri);
            Mediator.Notify("ChangeToLoginMenuView");
            Mediator.Notify("DisplayPageView", "Now you've logged in successfully.");
        }

    }
}
