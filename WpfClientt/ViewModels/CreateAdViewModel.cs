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
        private IAdDetailsService adDetailsService;
        private string currenlytChosenImageFileName = "Choose Image...";
        private long selectedCategory;
        private ISet<Category> actualCategories;
        public ObservableCollection<string> Categories { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Subcategories { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Types { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Conditions { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Manufacturers { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> States { get; private set; } = new ObservableCollection<string>();
        public ICommand ImageChooseCommand { get; private set; }
        public ICommand CreateAdCommand { get; private set; }
        public Uri ImageUri { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId {
            get {
                return selectedCategory;
            }
            set {
                selectedCategory = value;
                SetSubcategoriesOf(value);
            } 
        }
        public long SubcategoryId { get; set; }
        public long StateId { get; set; }
        public long ManufacturerId { get; set; }
        public long ConditionId { get; set; }
        public long TypeId { get; set; }
        public int Price { get; set; }

        public string CurrentlyChosenFileName {
            get {
                return currenlytChosenImageFileName;
            }
            private set {
                currenlytChosenImageFileName = value;
                OnPropertyChanged("CurrentlyChosenFileName");
            }
        }


        private CreateAdViewModel(IAdService adService,IAdDetailsService adDetailsService,ISet<Category> categories,
            ISet<AdType> types, ISet<Condition> conditions, ISet<Manufacturer> manufacturers, ISet<State> states) {

            ImageChooseCommand = new DelegateCommand(ChooseImage);
            CreateAdCommand = new DelegateCommand(CreateAd);

            foreach(Category category in categories) {
                this.Categories.Add($"{category.Id}-{category.Title}");
            }

            foreach (AdType type in types) {
                this.Types.Add($"{type.Id}-{type.Title}");
            }

            foreach (Condition condition in conditions) {
                this.Conditions.Add($"{condition.Id}-{condition.Title}");
            }

            foreach (Manufacturer manufacturer in manufacturers) {
                this.Manufacturers.Add($"{manufacturer.Id}-{manufacturer.Title}");
            }

            foreach (State state in states) {
                this.States.Add($"{state.Id}-{state.Title}");
            }

            this.adService = adService;
            this.adDetailsService = adDetailsService;
            this.actualCategories = categories;
        }

        public static async Task<CreateAdViewModel> GetInstance(FactoryServices factory) {
            IAdDetailsService adDetailsService = factory.AdDetailsServiceInstance();
            ISet<Category> categories = await adDetailsService.Categories();
            ISet<AdType> types = await adDetailsService.Types();
            ISet<Condition> conditions = await adDetailsService.Conditions();
            ISet<Manufacturer> manufacturers = await adDetailsService.Manufacturers();
            ISet<State> states = await adDetailsService.States();
            if (viewModel == null) {
                viewModel = new CreateAdViewModel(await factory.AdServiceInstance(),factory.AdDetailsServiceInstance()
                    ,categories,types,conditions,manufacturers,states);
            }

            return viewModel;
        }

        private void ChooseImage(object param) {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg,*.jpeg,*.png) | *.jpg; *.jpeg;*.png";
            if (fileDialog.ShowDialog() == true) {
                ImageUri = new Uri($"file:///{fileDialog.FileName}");
                CurrentlyChosenFileName = ImageUri.LocalPath;
            }
        }

        private void CreateAd(object param) {
        }

        private async void SetSubcategoriesOf(long categoryId) {
            this.SubcategoryId = -1;
            Subcategories.Clear();
            Category category = actualCategories.Where(categ => categ.Id.Equals(categoryId)).First();
            foreach (Subcategory subcategory in await adDetailsService.SubcategoriesOf(category)) {
                Subcategories.Add($"{subcategory.Id}-{subcategory.Title}");
            }
        }
    }
}
