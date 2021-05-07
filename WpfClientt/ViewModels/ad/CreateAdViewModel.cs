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
    public class CreateAdViewModel : FormViewModel<Ad> {

        private static CreateAdViewModel instance;

        private IAdService adService;

        public override Ad Form { get; protected set; }

        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
        public ObservableCollection<Subcategory> Subcategories { get; } = new ObservableCollection<Subcategory>();
        public ObservableCollection<AdType> Types { get; } = new ObservableCollection<AdType>();
        public ObservableCollection<Condition> Conditions { get; } = new ObservableCollection<Condition>();
        public ObservableCollection<Manufacturer> Manufacturers { get; } = new ObservableCollection<Manufacturer>();
        public ICommand ImageChooseCommand { get; private set; }
        public ICommand ClearImageCommand { get; private set; }

        public string CurrentlyChosenFileName {
            get {
                return Form.ImageUri == null ? "Choose Image..." : Form.ImageUri.LocalPath;
            }
        }

        private CreateAdViewModel(IAdService adService, ISet<Category> categories,
            ISet<AdType> types, ISet<Condition> conditions, ISet<Manufacturer> manufacturers,ICustomerNotifier notifier) : base(notifier) {
            ImageChooseCommand = new DelegateCommand(ChooseImage);
            ClearImageCommand = new DelegateCommand(_ => ClearImage());

            foreach (Category category in categories) {
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
            Form = new AdDecorator(Validate, SetSubcategoriesOf);
        }

        public static async Task<CreateAdViewModel> GetInstance(FactoryServices factory) {
            IAdDetailsService adDetailsService = factory.AdDetailsServiceInstance();
            ISet<Category> categories = await adDetailsService.CategoriesWithSubcategories();
            ISet<AdType> types = await adDetailsService.Types();
            ISet<Condition> conditions = await adDetailsService.Conditions();
            ISet<Manufacturer> manufacturers = await adDetailsService.Manufacturers();
            if (instance == null) {
                instance = new CreateAdViewModel(await factory.AdServiceInstance(), categories, types, conditions, 
                    manufacturers,factory.CustomerNotifier());
            }

            return instance;
        }

        private void ChooseImage(object param) {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg,*.jpeg,*.png) | *.jpg; *.jpeg;*.png";
            if (fileDialog.ShowDialog() == true) {
                Form.ImageUri = new Uri($"file:///{fileDialog.FileName}");
                OnPropertyChanged(nameof(CurrentlyChosenFileName));
            }
        }


        private void SetSubcategoriesOf(Category category) {
            Subcategories.Clear();
            foreach (Subcategory subcategory in category.Subcategories) {
                Subcategories.Add(subcategory);
            }
        }

        private void ClearImage() {
            Form.ImageUri = null;
            OnPropertyChanged(nameof(CurrentlyChosenFileName));
        }

        protected override Func<Ad, Task> SubmitAction() => adService.Create;

        protected override void ClearFormStrep(){
            Form = new AdDecorator(Validate,SetSubcategoriesOf);
            ClearImage();
        }
    }
}
