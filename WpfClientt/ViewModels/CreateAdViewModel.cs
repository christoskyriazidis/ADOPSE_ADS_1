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

        public Ad Ad { get; private set; }

        public ObservableCollection<Category> Categories { get;} = new ObservableCollection<Category>();
        public ObservableCollection<Subcategory> Subcategories { get;} = new ObservableCollection<Subcategory>();
        public ObservableCollection<AdType> Types { get;} = new ObservableCollection<AdType>();
        public ObservableCollection<Condition> Conditions { get;} = new ObservableCollection<Condition>();
        public ObservableCollection<Manufacturer> Manufacturers { get;} = new ObservableCollection<Manufacturer>();
        public ObservableCollection<ValidationResult> Errors { get; } = new ObservableCollection<ValidationResult>();
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public ICommand ImageChooseCommand { get; private set; }
        public ICommand CreateAdCommand { get; private set; }
        public ICommand ClearImageCommand { get; private set; }

        public string CurrentlyChosenFileName {
            get {
                return Ad.ImageUri == null ? "Choose Image..." : Ad.ImageUri.LocalPath;
            }
        }

        private CreateAdViewModel(IAdService adService,ISet<Category> categories,
            ISet<AdType> types, ISet<Condition> conditions, ISet<Manufacturer> manufacturers) {

            ImageChooseCommand = new DelegateCommand(ChooseImage);
            CreateAdCommand = new DelegateCommand(CreateAd);
            ClearImageCommand = new DelegateCommand(ClearImage);
            
            foreach(Category category in categories) {
                this.Categories.Add(category);
            }

            foreach (AdType type in types) {
                this.Types.Add(type);
            }

            foreach (Condition condition in conditions) {
                this.Conditions.Add(condition);
            }

            foreach (Manufacturer manufacturer in manufacturers) {
                this.Manufacturers.Add(manufacturer);
            }

            this.adService = adService;
            Ad = new AdDecorator(Validate,SetSubcategoriesOf);
            Validate(Ad);
        }

        public static async Task<CreateAdViewModel> GetInstance(FactoryServices factory) {
            IAdDetailsService adDetailsService = factory.AdDetailsServiceInstance();
            ISet<Category> categories = await adDetailsService.CategoriesWithSubcategories();
            ISet<AdType> types = await adDetailsService.Types();
            ISet<Condition> conditions = await adDetailsService.Conditions();
            ISet<Manufacturer> manufacturers = await adDetailsService.Manufacturers();
            if (instance == null) {
                instance = new CreateAdViewModel(await factory.AdServiceInstance(),categories,types,conditions,manufacturers);
            }

            return instance;
        }

        private void ChooseImage(object param) {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg,*.jpeg,*.png) | *.jpg; *.jpeg;*.png";
            if (fileDialog.ShowDialog() == true) {
                Ad.ImageUri = new Uri($"file:///{fileDialog.FileName}");
                OnPropertyChanged("CurrentlyChosenFileName");
            }
        }

        private async void CreateAd(object param) {
            Validate(Ad);
            if (Errors.Count == 0) {
                Messages.Add("Trying to add the give ad.");
                await adService.Create(Ad);
                Messages.Clear();
                Messages.Add("The ad has been successfully added.");
                await ClearForm();
            } else {
                Messages.Clear();
                Messages.Add("The form can't be submitted because of the errors.");
            }
        }

        private async Task ClearForm() {
            Ad = new AdDecorator(Validate,SetSubcategoriesOf);
            OnPropertyChanged("Ad");
            ClearImage(null);
            await Task.Delay(3000);
            Messages.Clear();
        }

        private void Validate(Ad ad) {
            Errors.Clear();
            ValidationContext validationContext = new ValidationContext(ad);
            Validator.TryValidateObject(Ad, validationContext,Errors,true);
        }


        private void SetSubcategoriesOf(Category category) {
            Subcategories.Clear();
            foreach (Subcategory subcategory in category.Subcategories) {
                Subcategories.Add(subcategory);
            }
        }

        private void ClearImage(object param) {
            Ad.ImageUri = null;
            OnPropertyChanged("CurrentlyChosenFileName");
        }
    }
}
