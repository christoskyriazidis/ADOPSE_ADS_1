using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;
using WpfClientt.services.filtering;

namespace WpfClientt.viewModels.filters {
    public class FilterViewModel : BaseViewModel {
        private static FilterViewModel instance;
        private AdsFilterBuilder filterBuilder = new AdsFilterBuilder();

        public ObservableCollection<FilterMemberViewModel> FilterMemebers { get; private set; } = new ObservableCollection<FilterMemberViewModel>();

        private FilterViewModel(FactoryServices factory,IDictionary<long,string> categories,
            IDictionary<long, string> conditions, IDictionary<long, string> manufacturers, IDictionary<long, string> states,
            IDictionary<long, string> types) {
            IMapper mapper = factory.Mapper();
            FilterMemebers.Add(new FilterMemberViewModel(categories, "Categories", filterBuilder.AddCategoryFilter));
            FilterMemebers.Add(new FilterMemberViewModel(conditions, "Conditions", filterBuilder.AddConditionFilter));
            FilterMemebers.Add(new FilterMemberViewModel(manufacturers, "Manufacturers", filterBuilder.AddManufacturerFilter));
            FilterMemebers.Add(new FilterMemberViewModel(states, "States", filterBuilder.AddStateFilter));
            FilterMemebers.Add(new FilterMemberViewModel(types, "Types", filterBuilder.AddTypeFilter));
        }

        public async static Task<FilterViewModel> GetInstance(FactoryServices factory) {
            if (instance == null) {
                IMapper mapper = factory.Mapper();
                var categories = await mapper.LoadCategories();
                var conditions = await mapper.LoadConditions();
                var manufacturers = await mapper.LoadManufacturers();
                var states = await mapper.LoadStates();
                var types = await mapper.LoadTypes();
                instance = new FilterViewModel(factory, categories, conditions, manufacturers, states, types);
            }
            return instance; 
        }

        public AdsFilterBuilder GetFilterBuilder() {
            foreach (FilterMemberViewModel filterMember in FilterMemebers) {
                filterMember.Finish();
            }
            return filterBuilder;
        }

        public void Reset() {
            foreach (FilterMemberViewModel filterMember in FilterMemebers) {
                filterMember.Reset();
            }
        }

    }
}
