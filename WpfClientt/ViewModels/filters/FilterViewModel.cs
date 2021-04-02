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

        public ObservableCollection<FilterMember> FilterMemebers { get; private set; } = new ObservableCollection<FilterMember>();

        private FilterViewModel(FactoryServices factory,IDictionary<long,string> categories,
            IDictionary<long, string> conditions, IDictionary<long, string> manufacturers, IDictionary<long, string> states,
            IDictionary<long, string> types) {
            IMapper mapper = factory.Mapper();
            FilterMemebers.Add(new SingleChoiceFilterMember(categories, "Categories", filterBuilder.AddCategoryFilter,"CategoryGroup"));
            FilterMemebers.Add(new MultipleChoicesFilterMember(conditions, "Conditions", filterBuilder.AddConditionFilter));
            FilterMemebers.Add(new MultipleChoicesFilterMember(manufacturers, "Manufacturers", filterBuilder.AddManufacturerFilter));
            FilterMemebers.Add(new MultipleChoicesFilterMember(states, "States", filterBuilder.AddStateFilter));
            FilterMemebers.Add(new MultipleChoicesFilterMember(types, "Types", filterBuilder.AddTypeFilter));
        }

        public async static Task<FilterViewModel> GetInstance(FactoryServices factory) {
            if (instance == null) {
                IMapper mapper = factory.Mapper();
                var categories = await mapper.Categories();
                var conditions = await mapper.Conditions();
                var manufacturers = await mapper.Manufacturers();
                var states = await mapper.States();
                var types = await mapper.Types();
                instance = new FilterViewModel(factory, categories, conditions, manufacturers, states, types);
            }
            return instance; 
        }

        public AdsFilterBuilder GetFilterBuilder() {
            foreach (FilterMember filterMember in FilterMemebers) {
                filterMember.Finish();
            }
            return filterBuilder;
        }

        public void Reset() {
            foreach (FilterMember filterMember in FilterMemebers) {
                filterMember.Reset();
            }
        }

    }
}
