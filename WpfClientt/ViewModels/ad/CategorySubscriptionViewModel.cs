using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;

namespace WpfClientt.viewModels {
    public class CategorySubscriptionViewModel : CategoryViewModel {

        public CategorySubscriptionViewModel(Category category) 
            : base(category, new AsyncCommand( async () => await Mediator.Notify(MediatorToken.SubscriptionsViewToken,category))) {
        }

    }
}
