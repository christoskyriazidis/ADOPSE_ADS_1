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

        private SubcategoriesViewModel(ISet<SubcategoryViewModel> subcategories) {
            foreach(SubcategoryViewModel subcategory in subcategories) {
                Subcategories.Add(subcategory);
            }
        }

        public static async Task<SubcategoriesViewModel> SubcategoriesToAdsViewModel(FactoryServices factory,Category category) {
            ISet<SubcategoryViewModel> subcategories = new HashSet<SubcategoryViewModel>();
            foreach(Subcategory subcategory in await factory.AdDetailsServiceInstance().SubcategoriesOf(category)) {
                subcategories.Add(new SubcategoryAdsViewModel(subcategory));
            }
            return new SubcategoriesViewModel(subcategories);
        }

    }
}
