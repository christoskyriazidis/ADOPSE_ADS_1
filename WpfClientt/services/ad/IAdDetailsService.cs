using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {

    /// <summary>
    /// A service that allows retrieving ad detail components(Manufacturers,Subcategories,etc).
    /// </summary>
    public interface IAdDetailsService {

        /// <summary>
        /// Returns all the available cateogires.The subcategories of the categories are not set.
        /// </summary>
        /// <returns></returns>
        Task<ISet<Category>> Categories();

        /// <summary>
        /// Returns all the available subcategories.
        /// </summary>
        /// <returns></returns>
        Task<ISet<Subcategory>> Subcategories();

        /// <summary>
        /// Returns all the available conditions.
        /// </summary>
        /// <returns></returns>
        Task<ISet<Condition>> Conditions();

        /// <summary>
        /// Returns all the available manufacturers.
        /// </summary>
        /// <returns></returns>
        Task<ISet<Manufacturer>> Manufacturers();

        /// <summary>
        /// Returns all the available states.
        /// </summary>
        /// <returns></returns>
        Task<ISet<State>> States();

        /// <summary>
        /// Returns all the available ad types.
        /// </summary>
        /// <returns></returns>
        Task<ISet<AdType>> Types();

        /// <summary>
        /// Returns all the subcategories of the given category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<ISet<Subcategory>> SubcategoriesOf(Category category);

        /// <summary>
        /// Returns all the available categories with their subcategories set.
        /// </summary>
        /// <returns></returns>
        Task<ISet<Category>> CategoriesWithSubcategories();

    }
}
