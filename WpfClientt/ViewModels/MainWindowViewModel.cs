using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.services;
using WpfClientt.viewModels.menu;

namespace WpfClientt.viewModels {
    public class MainWindowViewModel : BaseViewModel {

        private IViewModel currentPageView;
        private IMenu currentMenuView;
        private FactoryServices factory;
        private IList<IViewModel> viewModels = new List<IViewModel>();

        public IViewModel CurrentPageViewModel {
            get {
                return currentPageView;
            }
            set {
                currentPageView = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        public IMenu CurrentMenuView {
            get {
                return currentMenuView;
            }
            set {
                currentMenuView = value;
                OnPropertyChanged("CurrentMenuView");
            }
        }

        public MainWindowViewModel() {
            factory = new FactoryServices();
            CurrentMenuView = new GuestMenu();
            ChangeToDisplayView("Welcome To Easy Market!");

            Mediator.Subscribe("AdsView", ChangeToAdsView);
            Mediator.Subscribe("RegisterView", ChangeToRegisterView);
            Mediator.Subscribe("LoginView", ChangeToLoginView);
            Mediator.Subscribe("AdDetailsView", ChangeToAdDetailsView);
            Mediator.Subscribe("CreateView", ChangeToCreateAdView);
            Mediator.Subscribe("ProfileView", ChangeToProfileView);
        }

        private void ChangeViewModel(IViewModel viewModel) {
            if (!viewModels.Contains(viewModel)) {
                viewModels.Add(viewModel);
            }
            CurrentPageViewModel = viewModels.FirstOrDefault(vm => vm == viewModel);
        }

        private async void ChangeToAdsView(object obj) {
            ChangeToDisplayView("Loading Page");
            ChangeViewModel(await AdsViewModel.GetInstance(factory)); ;
        }

        private async void ChangeToRegisterView(object obj) {
            ChangeToDisplayView("Loading Page");
            ChangeViewModel(await RegisterViewModel.GetInstance(factory));
        }

        private async void ChangeToLoginView(object obj) { 
        
        }

        private async void ChangeToCreateAdView(object obj) {
            ChangeViewModel(await CreateAdViewModel.GetInstance(factory));
        }

        private void ChangeToDisplayView(string text) {
            ChangeViewModel(DisplayTextViewModel.GetInstance(text));
        }

        private async void ChangeToAdDetailsView(object param) {
            ChangeToDisplayView("Loading Page");
            long id = (long)param;
            CurrentPageViewModel = await AdDetailsViewModel.GetInstance(factory, id);
        }

        private async void ChangeToProfileView(object param) {
            ChangeToDisplayView("Loading Page");
            ChangeViewModel(await ProfileViewModel.getInstance(factory));
        }
    }
}
