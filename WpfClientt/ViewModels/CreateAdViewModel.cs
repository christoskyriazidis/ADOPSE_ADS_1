using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class CreateAdViewModel : BaseViewModel, IViewModel {
        private static CreateAdViewModel viewModel;

        private IAdService adService;
        private string currenlytChosenImageFileName = "Choose Image...";
        public ObservableCollection<string> Categories { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Types { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Conditions { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Manufacturers { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> States { get; private set; } = new ObservableCollection<string>();
        public ICommand ImageChooseCommand { get; private set; }
        public ICommand CreateAdCommand { get; private set; }
        public string CurrentlyChosenFileName {
            get {
                return currenlytChosenImageFileName;
            }
            private set {
                currenlytChosenImageFileName = value;
                OnPropertyChanged("CurrentlyChosenFileName");
            }
        }

        public Ad Ad { get; private set; } = new Ad();

        private CreateAdViewModel(IAdService adService,IDictionary<long,string> categories, 
            IDictionary<long, string> types, IDictionary<long, string> conditions,
            IDictionary<long, string> manufacturers, IDictionary<long, string> states) {

            ImageChooseCommand = new DelegateCommand(ChooseImage);
            CreateAdCommand = new DelegateCommand(CreateAd);

            foreach(KeyValuePair<long,string> category in categories) {
                this.Categories.Add($"{category.Key}-{category.Value}");
            }

            foreach (KeyValuePair<long, string> type in types) {
                this.Types.Add($"{type.Key}-{type.Value}");
            }

            foreach (KeyValuePair<long, string> condition in conditions) {
                this.Conditions.Add($"{condition.Key}-{condition.Value}");
            }

            foreach (KeyValuePair<long, string> manufacturer in manufacturers) {
                this.Manufacturers.Add($"{manufacturer.Key}-{manufacturer.Value}");
            }

            foreach (KeyValuePair<long, string> state in states) {
                this.States.Add($"{state.Key}-{state.Value}");
            }

            this.adService = adService;
        }

        public static async Task<CreateAdViewModel> GetInstance(FactoryServices factory) {
            IMapper mapper = factory.Mapper();
            IDictionary<long, string> categories = await mapper.Categories();
            IDictionary<long, string> types = await mapper.Types();
            IDictionary<long, string> conditions = await mapper.Conditions();
            IDictionary<long, string> manufacturers = await mapper.Manufacturers();
            IDictionary<long, string> states = await mapper.States();
            if (viewModel == null) {
                viewModel = new CreateAdViewModel(factory.AdServiceInstance(),categories,types,conditions,manufacturers,states);
            }

            return viewModel;
        }

        private void ChooseImage(object param) {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg,*.jpeg,*.png) | *.jpg; *.jpeg;*.png";
            if (fileDialog.ShowDialog() == true) {
                Ad.ImageUri = new Uri($"file:///{fileDialog.FileName}");
                CurrentlyChosenFileName = Ad.ImageUri.LocalPath;
            }
        }

        private void CreateAd(object param) {
            adService.Create(Ad);
        }
    }
}
