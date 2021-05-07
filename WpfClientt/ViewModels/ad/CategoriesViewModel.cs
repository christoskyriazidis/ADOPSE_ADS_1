using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class CategoriesViewModel : IViewModel {

        public ISet<CategoryViewModel> Categories { get; private set; } = new HashSet<CategoryViewModel>();

        private CategoriesViewModel(ISet<CategoryViewModel> categories) {
            foreach(CategoryViewModel category in categories) {
                Categories.Add(category);
            }
        }

        public static async Task<CategoriesViewModel> CategoriesAdsViewModel(FactoryServices factory) {
            ISet<Category> cateogires = await factory.AdDetailsServiceInstance().CategoriesWithSubcategories();
            ISet<CategoryViewModel> categoryVIewModels = new HashSet<CategoryViewModel>();

            foreach(Category category in cateogires) {
                categoryVIewModels.Add(new CategoryAdsViewModel(category));
            }
            return new CategoriesViewModel(categoryVIewModels);
        }

        public static async Task<CategoriesViewModel> CategoriesSubscriptionViewModel(FactoryServices factory) {
            ISet<Category> categories = await factory.AdDetailsServiceInstance().CategoriesWithSubcategories();
            ISet<CategoryViewModel> categoryViewModels = new HashSet<CategoryViewModel>();

            foreach(Category category in categories) {
                categoryViewModels.Add(new CategorySubscriptionViewModel(category));
            }

            return new CategoriesViewModel(categoryViewModels);
        }

    }
}
