using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfClientt.model;

namespace WpfClientt.services {
    /// <summary>
    /// Service for the instnaces of the Customer class.
    /// </summary>
    public interface ICustomerService : IService<Customer>{

        /// <summary>
        /// Returns the profile of the logged in customer.
        /// </summary>
        /// <returns></returns>
        Task<Customer> Profile();

        /// <summary>
        /// Updates the profile image of the logged in customer.
        /// </summary>
        /// <param name="fileName">The full path to the image.</param>
        /// <returns></returns>
        Task UpdateProfileImage(string fileName);

        /// <summary>
        /// Updates the profile of the logged in customer.
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        Task UpdateProfile(Customer profile);

    }
}
