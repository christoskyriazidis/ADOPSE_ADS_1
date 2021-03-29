using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public interface IMapper {

        Task<string> MapCategory(long id);

        Task<string> MapCondition(long id);

        Task<string> MapState(long id);

        Task<string> MapType(long id);

        Task<string> MapManufacturer(long id);

        Task LoadCategories(Action<IDictionary<long,string>> afterLoad);

        Task LoadConditions(Action<IDictionary<long, string>> afterLoad);

        Task LoadTypes(Action<IDictionary<long, string>> afterLoad);

        Task LoadManufacturers(Action<IDictionary<long, string>> afterLoad);

        Task LoadStates(Action<IDictionary<long, string>> afterLoad);

    }
}
