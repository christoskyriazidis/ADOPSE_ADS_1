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

        private IViewModel _currentPageViewModel;
        private IMenu _currentMenuView;
        private FactoryServices factory;

        public List<IViewModel> PageViewModels { get; } = new List<IViewModel>();

        public IViewModel CurrentPageViewModel {
            get {
                return _currentPageViewModel;
            }
            set {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        public IMenu CurrentMenuView {
            get {
                return _currentMenuView;
            }
            set {
                _currentMenuView = value;
                OnPropertyChanged("CurrentMenuView");
            }
        }

        public MainWindowViewModel() {
            factory = new FactoryServices();
            PageViewModels.Add(new AdsViewModel(factory));
            PageViewModels.Add(new RegisterViewModel());

            CurrentMenuView = new GuestMenu();
            CurrentPageViewModel = PageViewModels[0];

            Mediator.Subscribe("AdsView", ChangeToAdsView);
            Mediator.Subscribe("RegisterView", ChangeToRegisterView);
            Mediator.Subscribe("LoginView", ChangeToLoginView);
        }

        private void ChangeViewModel(IViewModel viewModel) {
            if (!PageViewModels.Contains(viewModel)) {
                PageViewModels.Add(viewModel);
            }
            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }

        private void ChangeToAdsView(object obj) {
            ChangeViewModel(PageViewModels[0]);
        }

        private void ChangeToRegisterView(object obj) {
            ChangeViewModel(PageViewModels[1]);
        }

        private void ChangeToLoginView(object obj) { 
        
        }
    }
}
