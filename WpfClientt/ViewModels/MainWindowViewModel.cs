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
            Mediator.Subscribe("BackView", previousViewModel);
        }

        private void ChangeViewModel(IViewModel viewModel) {
            if (!viewModels.Contains(viewModel)) {
                viewModels.Add(viewModel);
            }
            CurrentPageViewModel = viewModels.FirstOrDefault(vm => vm == viewModel);
        }

        private async void ChangeToAdsView(object obj) {
            ChangeToDisplayView("Loading Page");
            addToHistory(await AdsViewModel.GetInstance(factory));
        }

        private async void ChangeToRegisterView(object obj) {
            ChangeToDisplayView("Loading Page");
            addToHistory(await RegisterViewModel.GetInstance(factory));
        }

        private async void ChangeToLoginView(object obj) { 
        
        }

        private async void ChangeToCreateAdView(object obj) {
            addToHistory(await CreateAdViewModel.GetInstance(factory));
        }

        private void ChangeToDisplayView(string text) {
            CurrentPageViewModel = DisplayTextViewModel.GetInstance(text);
        }

        private async void ChangeToAdDetailsView(object param) {
            ChangeToDisplayView("Loading Page");
            long id = (long)param;
            addToHistory(await AdDetailsViewModel.GetInstance(factory, id));
        }

        private async void ChangeToProfileView(object param) {
            ChangeToDisplayView("Loading Page");
            ChangeViewModel(await ProfileViewModel.getInstance(factory));
        }

        private IViewModel currentViewModel() {
            return history.Peek();
        }

        private void previousViewModel(object param) {
            history.Pop();
            ChangeViewModel(currentViewModel());
            OnPropertyChanged("IsBackButtonVisible");
        }

        private void addToHistory(IViewModel viewModel) {
            history.Push(viewModel);
            ChangeViewModel(currentViewModel());
            OnPropertyChanged("IsBackButtonVisible");
        }
    }
}
