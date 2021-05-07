﻿using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class UnsubscribedSubcategoryViewModel : SubcategoryViewModel {
        public UnsubscribedSubcategoryViewModel(Subcategory subcategory,INotifyService service,Action<Subcategory> notifier) 
            : base(subcategory, new AsyncCommand(async () => {
                await service.SubscriberToSubcategory(subcategory);
                notifier.Invoke(subcategory);
            })) {
        }
    }
}
