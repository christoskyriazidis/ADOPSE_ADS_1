using WpfClientt.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services.filtering;

namespace WpfClientt.services {
    /// <summary>
    /// Service for the instnaces of the Ad class.
    /// </summary>
    public interface IAdService : IService<Ad> {

        /// <summary>
        /// Returns a scroller(pagination) with the ads that satisfy the given filter.
        /// </summary>
        /// <param name="adsFilterBuilder"></param>
        /// <returns></returns>
        IScroller<Ad> Fiter(AdsFilterBuilder adsFilterBuilder);

        /// <summary>
        /// Returns a scroller(pagination) with the ads published by the logged in customer.
        /// </summary>
        /// <returns></returns>
        IScroller<Ad> ProfileAds();

        /// <summary>
        /// Updates the image of the ad that has the given id.
        /// </summary>
        /// <param name="path">The full path to the image</param>
        /// <param name="id">The id of the ad</param>
        /// <returns></returns>
        Task UpdateAdImage(string path,long id);

        /// <summary>
        /// Returns a scroller(pagination) with ads that belong to the given 
        /// subcategory.
        /// </summary>
        /// <param name="subcategory"></param>
        /// <returns></returns>
        IScroller<Ad> SubcategoryAds(Subcategory subcategory);

    }
}
