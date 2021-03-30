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

        private AdsFilterBuilder filterBuilder = new AdsFilterBuilder();

        public ObservableCollection<FilterMemberViewModel> FilterMemebers { get; private set; } = new ObservableCollection<FilterMemberViewModel>();

        public FilterViewModel(FactoryServices factory) {
            IMapper mapper = factory.Mapper();
            mapper.LoadCategories(categories => {
                FilterMemebers.Add(new FilterMemberViewModel(categories, "Categories", filterBuilder.AddCategoryFilter));
            });
            mapper.LoadConditions(conditions => {
                FilterMemebers.Add(new FilterMemberViewModel(conditions, "Conditions",filterBuilder.AddConditionFilter));
            });
            mapper.LoadManufacturers(manufacturers => {
                FilterMemebers.Add(new FilterMemberViewModel(manufacturers, "Manufacturers",filterBuilder.AddManufacturerFilter));
            });
            mapper.LoadStates(states => {
                FilterMemebers.Add(new FilterMemberViewModel(states, "States",filterBuilder.AddStateFilter));
            });
            mapper.LoadTypes(types => {
                FilterMemebers.Add(new FilterMemberViewModel(types, "Types",filterBuilder.AddTypeFilter));
            });
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
