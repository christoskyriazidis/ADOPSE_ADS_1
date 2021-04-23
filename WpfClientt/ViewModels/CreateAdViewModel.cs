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
        private static CreateAdViewModel instance;

        private IAdService adService;
        private IAdDetailsService adDetailsService;
        private Ad ad = new Ad();

        public ObservableCollection<string> Categories { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Subcategories { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Types { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Conditions { get;} = new ObservableCollection<string>();
        public ObservableCollection<string> Manufacturers { get;} = new ObservableCollection<string>();
        public ObservableCollection<ValidationResult> Errors { get; } = new ObservableCollection<ValidationResult>();
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public ICommand ImageChooseCommand { get; private set; }
        public ICommand CreateAdCommand { get; private set; }
        public ICommand ClearImageCommand { get; private set; }

        public string Title {
            get {
                return ad.Title;
            }
            set {
                ad.Title = value;
                OnPropertyChanged("Title");
                Validate();
            } 
        }
        public string Description {
            get { return ad.Description; }
            set {
                ad.Description = value;
                OnPropertyChanged("Description");
                Validate();
            } 
        }
        public int? AdCategory {
            set {
                AdSubcategory = null;
                ad.AdSubcategory = null;
                if(value != null) {
                    ad.AdCategory = adDetailsService.Categories().Result.Where(category => category.Id.Equals(value)).First();
                    SetSubcategoriesOf(ad.AdCategory);
                }
                OnPropertyChanged("AdCategory");
                Validate();
            } 
        }
        public int? AdSubcategory {
            set {
                if (value != null) {
                    ad.AdSubcategory = adDetailsService.Subcategories().Result.Where(subcategory => subcategory.Id.Equals(value)).First();
                }
                OnPropertyChanged("AdSubcategory");
                Validate();
            } 
        }
        public int? AdManufacturer {
            set {
                if (value != null) {
                    ad.AdManufacturer = adDetailsService.Manufacturers().Result.Where(manufacturer => manufacturer.Id.Equals(value)).First();
                }
                OnPropertyChanged("AdManufacturer");
                Validate();
            } 
        }
        public int? AdCondition {
            set {
                if (value != null) {
                    ad.AdCondition = adDetailsService.Conditions().Result.Where(condition => condition.Id.Equals(value)).First();
                }
                OnPropertyChanged("AdCondition");
                Validate();
            }
        }
        public int? AdType {
            set {
                if (value != null) {
                    ad.AdType = adDetailsService.Types().Result.Where(adType => adType.Id.Equals(value)).First();
                }
                OnPropertyChanged("AdType");
                Validate();
            } 
        }
        public int? Price {
            set {
                ad.Price = value;
                OnPropertyChanged("Price");
                Validate();
            } 
        }

        public string CurrentlyChosenFileName {
            get {
                return ad.ImageUri == null ? "Choose Image..." : ad.ImageUri.LocalPath;
            }
        }

        private CreateAdViewModel(IAdService adService,IAdDetailsService adDetailsService,ISet<Category> categories,
            ISet<AdType> types, ISet<Condition> conditions, ISet<Manufacturer> manufacturers) {

            ImageChooseCommand = new DelegateCommand(ChooseImage);
            CreateAdCommand = new DelegateCommand(CreateAd);
            ClearImageCommand = new DelegateCommand(ClearImage);

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
            ISet<Category> categories = await adDetailsService.CategoriesWithSubcategories();
            ISet<AdType> types = await adDetailsService.Types();
            ISet<Condition> conditions = await adDetailsService.Conditions();
            ISet<Manufacturer> manufacturers = await adDetailsService.Manufacturers();
            if (instance == null) {
                instance = new CreateAdViewModel(await factory.AdServiceInstance(),factory.AdDetailsServiceInstance()
                    ,categories,types,conditions,manufacturers);
            }

            return instance;
        }

        private void ChooseImage(object param) {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg,*.jpeg,*.png) | *.jpg; *.jpeg;*.png";
            if (fileDialog.ShowDialog() == true) {
                ad.ImageUri = new Uri($"file:///{fileDialog.FileName}");
                OnPropertyChanged("CurrentlyChosenFileName");
            }
        }

        private async void CreateAd(object param) {
            if (Errors.Count == 0) {
                Messages.Add("Trying to add the give ad.");
                await adService.Create(ad);
                Messages.Clear();
                Messages.Add("The ad has been successfully added.");
                await ClearForm();
            } else {
                Messages.Clear();
                Messages.Add("The form can't be submitted because of the errors.");
            }
        }

        private async Task ClearForm() {
            ad = new Ad();
            Title = string.Empty;
            Description = string.Empty;
            AdCategory = null;
            AdSubcategory = null;
            AdCondition = null;
            AdManufacturer = null;
            AdType = null;
            Price = null;
            ClearImage(null);
            await Task.Delay(1000);
            Messages.Clear();
        }

        private void Validate() {
            Errors.Clear();
            ValidationContext validationContext = new ValidationContext(ad);
            Validator.TryValidateObject(ad, validationContext,Errors,true);
        }


        private void SetSubcategoriesOf(Category category) {
            this.AdSubcategory = null;
            Subcategories.Clear();
            foreach (Subcategory subcategory in category.Subcategories) {
                Subcategories.Add($"{subcategory.Id}-{subcategory.Title}");
            }
        }

        private void ClearImage(object param) {
            ad.ImageUri = null;
            OnPropertyChanged("CurrentlyChosenFileName");
        }
    }
}
