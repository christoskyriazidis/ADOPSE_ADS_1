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
    public class SubcategoriesViewModel : IViewModel {

        public ISet<SubcategoryViewModel> Subcategories { get; private set; } = new HashSet<SubcategoryViewModel>();

        private SubcategoriesViewModel(ISet<Subcategory> subcategories) {
            foreach(Subcategory subcategory in subcategories) {
                Subcategories.Add(new SubcategoryToAdsViewModel(subcategory));
            }
        }

        public static async Task<SubcategoriesViewModel> GetInstance(FactoryServices factory,Category category) {
            return new SubcategoriesViewModel(await factory.AdDetailsServiceInstance().SubcategoriesOf(category));
        }

    }
}
