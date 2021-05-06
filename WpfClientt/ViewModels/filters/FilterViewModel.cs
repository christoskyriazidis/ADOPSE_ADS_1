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

namespace WpfClientt.viewModels {
    /// <summary>
    /// Represents the filters available to the user to use.
    /// </summary>
    public class FilterViewModel {
        private static FilterViewModel instance;
        private AdsFilterBuilder filterBuilder;

        public ObservableCollection<FilterMember> FilterMemebers { get; private set; } = new ObservableCollection<FilterMember>();

        private FilterViewModel(ISet<Condition> conditions, ISet<Manufacturer> manufacturers, ISet<State> states,ISet<AdType> types,Subcategory subcategory) {
            filterBuilder = new AdsFilterBuilder(subcategory);
            FilterMemebers.Add(new SearchFilterMember(filterBuilder.SetTitleQuery,"Title"));
            FilterMemebers.Add(MultipleChoicesFilterMember.getInstance(conditions, "Conditions", filterBuilder.AddConditionFilter));
            FilterMemebers.Add(MultipleChoicesFilterMember.getInstance(manufacturers, "Manufacturers", filterBuilder.AddManufacturerFilter));
            FilterMemebers.Add(MultipleChoicesFilterMember.getInstance(states, "States", filterBuilder.AddStateFilter));
            FilterMemebers.Add(MultipleChoicesFilterMember.getInstance(types, "Types", filterBuilder.AddTypeFilter));
            FilterMemebers.Add(new MinMaxPriceFilterMember(filterBuilder.SetMinPrice,"Price Range",filterBuilder.SetMaxPrice));
        }

        public async static Task<FilterViewModel> GetInstance(FactoryServices factory,Subcategory subcategory) {
            if (instance == null) {
                IAdDetailsService service = factory.AdDetailsServiceInstance();
                var conditions = await service.Conditions();
                var manufacturers = await service.Manufacturers();
                var states = await service.States();
                var types = await service.Types();
                instance = new FilterViewModel(conditions, manufacturers, states, types,subcategory);
            }
            return instance; 
        }

        /// <summary>
        /// Returns the AdsFilterBuilder that has been populated with selected values.
        /// </summary>
        /// <returns></returns>
        public AdsFilterBuilder GetFilterBuilder() {
            foreach (FilterMember filterMember in FilterMemebers) {
                filterMember.Finish();
            }
            return filterBuilder;
        }

        /// <summary>
        /// Resets all the available filters to default settings.
        /// </summary>
        public void Reset() {
            foreach (FilterMember filterMember in FilterMemebers) {
                filterMember.Reset();
            }
        }

    }
}
