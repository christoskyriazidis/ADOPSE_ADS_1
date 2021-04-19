using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class CreateAdViewModel : BaseViewModel, IViewModel{
        private static CreateAdViewModel viewModel;

        private IAdService adService;
        private IAdDetailsService adDetailsService;
        private string currenlytChosenImageFileName = "Choose Image...";
        private Ad ad = new Ad();

        public ObservableCollection<string> Categories { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Subcategories { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Types { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Conditions { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Manufacturers { get;} = new ObservableCollection<string>();
        public ObservableCollection<ValidationResult> Errors { get; } = new ObservableCollection<ValidationResult>();
        public ICommand ImageChooseCommand { get; private set; }
        public ICommand CreateAdCommand { get; private set; }
        public Uri ImageUri { get; private set; }

        public string Title {
            set {
                ad.Title = value;
                Validate();
            } 
        }
        public string Description {
            set {
                ad.Description = value;
                Validate();
            } 
        }
        public int? AdCategory {
            set {
                AdSubcategory = null;
                ad.AdSubcategory = null;
                if (value != null) {
                    ad.AdCategory = adDetailsService.Categories().Result.Where(category => category.Id.Equals(value)).First();
                    SetSubcategoriesOf(value);
                }
                Validate();
            } 
        }
        public int? AdSubcategory {
            set {
                if(value != null) {
                    ad.AdSubcategory = adDetailsService.Subcategories().Result.Where(subcategory => subcategory.Id.Equals(value)).First();
                }
                Validate();
            } 
        }
        public int? AdManufacturer {
            set {
                if(value != null) {
                    ad.AdManufacturer = adDetailsService.Manufacturers().Result.Where(manufacturer => manufacturer.Id.Equals(value)).First();
                }
                Validate();
            } 
        }
        public int? AdCondition {
            set {
                if(value != null) {
                    ad.AdCondition = adDetailsService.Conditions().Result.Where(condition => condition.Id.Equals(value)).First();
                }
                Validate();
            }
        }
        public int? AdType {
            set { 
                if(value != null) {
                    ad.AdType = adDetailsService.Types().Result.Where(adType => adType.Id.Equals(value)).First();
                }
                Validate();
            } 
        }
        public int? Price {
            set {
                if(value != null) {
                    ad.Price = (int)value;
                }
                Validate();
            } 
        }

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
            ISet<AdType> types, ISet<Condition> conditions, ISet<Manufacturer> manufacturers) {

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

            this.adService = adService;
            this.adDetailsService = adDetailsService;
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
                    ,categories,types,conditions,manufacturers);
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

        private void Validate() {
            Errors.Clear();
            ValidationContext validationContext = new ValidationContext(ad);
            Validator.TryValidateObject(ad, validationContext,Errors,true);
        }


        private async void SetSubcategoriesOf(int? categoryId) {
            this.AdSubcategory = null;
            Subcategories.Clear();
            Category category = (await adDetailsService.Categories()).Where(categ => categ.Id.Equals(categoryId)).First();
            foreach (Subcategory subcategory in await adDetailsService.SubcategoriesOf(category)) {
                Subcategories.Add($"{subcategory.Id}-{subcategory.Title}");
            }
        }

    }
}
