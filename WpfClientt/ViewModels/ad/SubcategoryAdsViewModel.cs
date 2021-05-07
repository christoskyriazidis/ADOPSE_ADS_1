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
    /// SubcategoryViewModel that shows the ads that belong to this subcategory when customer
    /// executes the command.
    /// </summary>
    public class SubcategoryAdsViewModel : SubcategoryViewModel {

        public SubcategoryAdsViewModel(Subcategory subcategory) : base(subcategory
            ,new AsyncCommand(async () => await Mediator.Notify("AdsSubcategoryView", subcategory))) {
        }

    }
}
