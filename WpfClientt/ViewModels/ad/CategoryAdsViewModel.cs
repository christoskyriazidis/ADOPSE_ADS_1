using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;

namespace WpfClientt.viewModels {

    /// <summary>
    /// Category view model that shows the subcategories that belong to this category when customer
    /// executes the command.
    /// </summary>
    public class CategoryAdsViewModel : CategoryViewModel {
        public CategoryAdsViewModel(Category category) 
            : base(category,new AsyncCommand(async () => await Mediator.Notify("SubcategoriesToAdsView", category))) {
        }
    }
}
