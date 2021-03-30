using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels.filters {
    public class FilterViewModel : BaseViewModel {

        private ObservableCollection<Ad> ads;
        private AdsFilterBuilder filterBuilder;

        public ObservableCollection<FilterMemberViewModel> FilterMemebers { get; private set; } = new ObservableCollection<FilterMemberViewModel>();


        public FilterViewModel(ObservableCollection<Ad> ads,FactoryServices factory) {
            this.ads = ads;
            IMapper mapper = factory.Mapper();
            mapper.LoadCategories(categories => {
                FilterMemebers.Add(new FilterMemberViewModel(categories, "Categories"));
            });
            mapper.LoadConditions(conditions => {
                FilterMemebers.Add(new FilterMemberViewModel(conditions, "Conditions"));
            });
            mapper.LoadManufacturers(manufacturers => {
                FilterMemebers.Add(new FilterMemberViewModel(manufacturers, "Manufacturers"));
            });
            mapper.LoadStates(states => {
                FilterMemebers.Add(new FilterMemberViewModel(states, "States"));
            });
            mapper.LoadTypes(types => {
                FilterMemebers.Add(new FilterMemberViewModel(types, "Types"));
            });
        }

    }
}
