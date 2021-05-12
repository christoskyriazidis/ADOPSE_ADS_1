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
    public class SubscribedSubcategoryViewModel : SubcategoryViewModel {

        public SubscribedSubcategoryViewModel(Subcategory subcategory, INotificationService service, Action<Subcategory> notifier)
            : base(subcategory, new AsyncCommand(async () => { 
                        await service.UnsubscribeFromSubcategories(subcategory);
                        notifier.Invoke(subcategory);
                    })) {
        }
    }
}
