using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class SubscriptionsViewModel : IViewModel {

        private INotifyService service;
        public ObservableCollection<SubscribedSubcategoryViewModel> SubsribedSubcategories { get; set; } = new ObservableCollection<SubscribedSubcategoryViewModel>();
        public ObservableCollection<UnsubscribedSubcategoryViewModel> UnsubscribedSubcategories { get; set; } = new ObservableCollection<UnsubscribedSubcategoryViewModel>();

        private SubscriptionsViewModel(ISet<Subcategory> subscribedSubcategories,INotifyService service,ISet<Subcategory> unsubscribedSubcategories) {
            this.service = service;
            foreach(Subcategory subsribecSubcategory in subscribedSubcategories) {
                SubsribedSubcategories.Add(new SubscribedSubcategoryViewModel(subsribecSubcategory, service, Unsubscribed));
            }

            foreach(Subcategory unsubscribedSubcategory in unsubscribedSubcategories) {
                UnsubscribedSubcategories.Add(new UnsubscribedSubcategoryViewModel(unsubscribedSubcategory, service, Subscribed));
            }
        }

        public static async Task<SubscriptionsViewModel> GetInstance(Category category,FactoryServices factoryServices) {
            INotifyService service = await factoryServices.NotifyService();
            ICollection<Subcategory> subcategoriesOfCategory = category.Subcategories;
            ISet<Subcategory> subscribedSubcategories = await service.SubscribedSubcategories();
            ISet<Subcategory> unsubscribedSubcategories = new HashSet<Subcategory>();

            foreach(Subcategory subcategory in subcategoriesOfCategory) {
                bool alreadySubscribed = false;
                foreach(Subcategory subscribedSubcategory in subscribedSubcategories) {
                    if (subscribedSubcategory.Id.Equals(subcategory.Id)) {
                        alreadySubscribed = true;
                        break;
                    }
                }
                if (!alreadySubscribed) {
                    unsubscribedSubcategories.Add(subcategory);
                }
            }

            return new SubscriptionsViewModel(subscribedSubcategories, service, unsubscribedSubcategories);
        }

        private void Unsubscribed(Subcategory subcategory) {
            foreach(SubscribedSubcategoryViewModel subsribecSubcategory in SubsribedSubcategories) {
                if (subsribecSubcategory.Subcategory.Id.Equals(subcategory.Id)) {
                    SubsribedSubcategories.Remove(subsribecSubcategory);
                    UnsubscribedSubcategories.Add(new UnsubscribedSubcategoryViewModel(subcategory, service, Subscribed));
                    break;
                }
            }
        }

        private void Subscribed(Subcategory subcategory) {
            UnsubscribedSubcategoryViewModel removed = null;
            foreach (UnsubscribedSubcategoryViewModel unsubscribedSubscategory in UnsubscribedSubcategories) {
                if (unsubscribedSubscategory.Subcategory.Id.Equals(subcategory.Id)) {
                    removed = unsubscribedSubscategory;
                    break;
                }
            }
            UnsubscribedSubcategories.Remove(removed);
            SubsribedSubcategories.Add(new SubscribedSubcategoryViewModel(removed.Subcategory, service, Unsubscribed));
        }

    }
}
