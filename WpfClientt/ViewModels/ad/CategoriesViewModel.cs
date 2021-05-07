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
        private static CategoriesViewModel instance;

        public ISet<CategoryViewModel> Categories { get; private set; } = new HashSet<CategoryViewModel>();

        private CategoriesViewModel(ISet<Category> categories) {
            foreach(Category category in categories) {
                Categories.Add(new CategoryToSubcategoriesViewModel(category));
            }
        }

        public static async Task<CategoriesViewModel> GetInstance(FactoryServices factory) {
            if(instance == null) {
                ISet<Category> cateogires = await factory.AdDetailsServiceInstance().CategoriesWithSubcategories();
                instance = new CategoriesViewModel(cateogires);
            }
            return instance;
        }

    }
}
