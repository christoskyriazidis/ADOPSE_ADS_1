using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public interface IMapper {

        Task<IDictionary<long,string>> LoadCategories();

        Task<IDictionary<long, string>> LoadConditions();

        Task<IDictionary<long, string>> LoadTypes();

        Task<IDictionary<long, string>> LoadManufacturers();

        Task<IDictionary<long, string>> LoadStates();

    }
}
