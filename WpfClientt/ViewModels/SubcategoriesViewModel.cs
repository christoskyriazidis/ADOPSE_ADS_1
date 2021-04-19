using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class SubcategoriesViewModel : IViewModel {

        public ISet<Subcategory> Subcategories { get; private set; }

        private SubcategoriesViewModel(ISet<Subcategory> subcategories) {
            Subcategories = subcategories;
        }

        public static async Task<SubcategoriesViewModel> GetInstance(FactoryServices factory,Category category) {
            return new SubcategoriesViewModel(await factory.AdDetailsServiceInstance().SubcategoriesOf(category));
        }

    }
}
