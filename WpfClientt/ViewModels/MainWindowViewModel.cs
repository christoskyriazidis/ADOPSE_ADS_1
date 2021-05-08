using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class MainWindowViewModel : BaseViewModel {

        private IViewModel currentPageView;
        private IMenu currentMenuView;
        private FactoryServices factory;
        private Stack<IViewModel> history = new Stack<IViewModel>();
        public bool IsBackButtonVisible { 
            get {
                return history.Count > 1;
            }
        }

        public IViewModel CurrentPageViewModel {
            get {
                return currentPageView;
            }
            set {
                currentPageView = value;
                OnPropertyChanged(nameof(CurrentPageViewModel));
            }
        }

        public IMenu CurrentMenuView {
            get {
                return currentMenuView;
            }
            set {
                currentMenuView = value;
                OnPropertyChanged(nameof(CurrentMenuView));
            }
        }

        public MainWindowViewModel() {
            factory = new FactoryServices();
            CurrentMenuView = new GuestMenu();
            DisplayView("Welcome To Easy Market!");

            Mediator.Subscribe(MediatorToken.DisplayPageViewToken, DisplayView);
            Mediator.Subscribe(MediatorToken.LoginMenuViewToken, LoginMenu);
            Mediator.Subscribe(MediatorToken.AdsViewToken, AdsView);
            Mediator.Subscribe(MediatorToken.CategoriesAdsViewToken, CategoriesAdsView);
            Mediator.Subscribe(MediatorToken.SubcategoriesAdsViewToken, SubcategoriesAdsView);
            Mediator.Subscribe(MediatorToken.RegisterViewToken, RegisterView);
            Mediator.Subscribe(MediatorToken.LoginViewToken, LoginView);
            Mediator.Subscribe(MediatorToken.AdDetailsViewToken, GuestAdDetailsView);
            Mediator.Subscribe(MediatorToken.CreateAdViewToken, CreateAdView);
            Mediator.Subscribe(MediatorToken.ProfileViewToken, ProfileView);
            Mediator.Subscribe(MediatorToken.PreviousViewToken, PreviousViewModel);
            Mediator.Subscribe(MediatorToken.ChatsViewToken, ChatsView);
            Mediator.Subscribe(MediatorToken.NotificationsViewToken, NotificationsView);
            Mediator.Subscribe(MediatorToken.CategoriesSubscriptionsViewToken, CategoriesSubscriptionsView);
            Mediator.Subscribe(MediatorToken.SubscriptionsViewToken, SubscriptionsView);
        }


        private Task ChangeViewModel(IViewModel viewModel) {
            CurrentPageViewModel = viewModel;
            return Task.CompletedTask;
        }

        private Task ChangeMenuView(IMenu menu)   {
            if(menu is LoginCustomerMenu) {
                Mediator.Unsubscribe(MediatorToken.AdDetailsViewToken, GuestAdDetailsView);
                Mediator.Subscribe(MediatorToken.AdDetailsViewToken, LoginAdDetailsView);
            }
            this.CurrentMenuView = menu;
            return Task.CompletedTask;
        }

        private async Task SubscriptionsView(object category) {
            await DisplayView("Loading Page");
            AddToHistory(await SubscriptionsViewModel.GetInstance((Category)category,factory));
        }

        private async Task CategoriesSubscriptionsView(object _) {
            await DisplayView("Loading Page");
            AddToHistory(await CategoriesViewModel.CategoriesSubscriptionViewModel(factory));
        }

        private async Task LoginMenu(object param) {
            await ChangeMenuView(await LoginCustomerMenu.GetInstance(factory));
        }
        private async Task NotificationsView(object _) {
            await DisplayView("Loading Page");
            AddToHistory(await NotificationsViewModel.GetInstance(factory));
        }

        private async Task AdsView(object subcategory) {
            await DisplayView("Loading Page");
            AddToHistory(await AdsViewModel.GetInstanceWithSubcategoryAds(factory,(Subcategory)subcategory));
        }

        private async Task SubcategoriesAdsView(object category) {
            await DisplayView("Loading Page");
            AddToHistory( await SubcategoriesViewModel.SubcategoriesToAdsViewModel(factory, (Category)category) );
        }

        private async Task RegisterView(object obj) {
            await DisplayView("Loading Page");
            AddToHistory(await RegisterViewModel.GetInstance(factory));
        }

        private async Task LoginView(object obj) {
            AddToHistory(await LoginViewModel.GetInstance(factory));
        }

        private async Task CategoriesAdsView(object obj) {
            await DisplayView("Loading Page");
            AddToHistory(await CategoriesViewModel.CategoriesAdsViewModel(factory));
        }

        private async Task CreateAdView(object obj) {
            await DisplayView("Loading Page");
            AddToHistory(await CreateAdViewModel.GetInstance(factory));
        }

        private Task DisplayView(object text) {
            CurrentPageViewModel = DisplayTextViewModel.GetInstance((string)text);
            return Task.CompletedTask;
        }

        private async Task GuestAdDetailsView(object param) {
            await DisplayView("Loading Page");
            AddToHistory(await AdGuestDetailsViewModel.GetInstance((Ad)param,factory));
        }

        private async Task LoginAdDetailsView(object param) {
            await DisplayView("Loading Page");
            AddToHistory(await AdDetailsViewModel.GetInstance((Ad)param, factory));
        }

        private async Task ProfileView(object param) {
            await DisplayView("Loading Page");
            await ChangeViewModel(await ProfileViewModel.GetInstance(factory));
        }

        private async Task ChatsView(object obj) {
            AddToHistory(await ChatsViewModel.GetInstance(factory));
        }
        private IViewModel currentViewModel() {
            return history.Peek();
        }

        private async Task PreviousViewModel(object param) {
            history.Pop();
            await ChangeViewModel(currentViewModel());
            OnPropertyChanged(nameof(IsBackButtonVisible));
        }

        private async void AddToHistory(IViewModel viewModel) {
            if (!history.Contains(viewModel)) {
                history.Push(viewModel);
            }
            await ChangeViewModel(viewModel);
            OnPropertyChanged(nameof(IsBackButtonVisible));
        }
    }
}
