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

        public ISet<Category> Categories { get; private set; }
        public ICommand ShowSubcategories { get; private set; }

        private CategoriesViewModel(ISet<Category> categories) {
            this.Categories = categories;
            ShowSubcategories = new AsyncCommand<Category>(async category => {
                await Mediator.Notify("SubcategoriesView", category);
            });
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
