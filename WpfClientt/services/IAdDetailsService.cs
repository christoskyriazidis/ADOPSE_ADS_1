using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    public interface IAdDetailsService {

        Task<ISet<Category>> Categories();

        Task<ISet<Subcategory>> Subcategories();

        Task<ISet<Condition>> Conditions();

        Task<ISet<Manufacturer>> Manufacturers();

        Task<ISet<State>> States();

        Task<ISet<AdType>> Types();

        Task<ISet<Subcategory>> SubcategoriesOf(Category category);

    }
}
