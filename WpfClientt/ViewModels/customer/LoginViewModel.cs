using AsyncAwaitBestPractices.MVVM;
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
        private ICustomerNotifier notifier;

        public string LoginUrl { get; private set; }

        public ICommand ExchangeTokenCommand { get; private set; }

        private LoginViewModel(OpenIdConnectClient openIdConnectClient,string loginUrl,ICustomerNotifier notifier) {
            this.notifier = notifier;
            this.openIdConnectClient = openIdConnectClient;
            ExchangeTokenCommand = new AsyncCommand<string>(ExchangeToken);
            LoginUrl = loginUrl;
        }

        public static async Task<LoginViewModel> GetInstance(FactoryServices factory) {
            if(instance == null) {
                OpenIdConnectClient client = await factory.GetOpenIdConnectClient();
                instance = new LoginViewModel(client, await client.PrepareAuthorizationRequestUrl(),factory.CustomerNotifier());
            }

            return instance;
        }

        private async Task ExchangeToken(string redirectUri) {
            await Mediator.Notify(MediatorToken.DisplayPageViewToken, "Authentication in progress");
            await openIdConnectClient.RetrieveAndSetAccessToken(redirectUri);
            await Mediator.Notify(MediatorToken.LoginMenuViewToken);
            notifier.Success("You've logged in successfully!");
        }

    }
}
