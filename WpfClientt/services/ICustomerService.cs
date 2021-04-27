using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfClientt.model;

namespace WpfClientt.services {
    public interface ICustomerService : IService<Customer>{

        Task<Customer> Profile();

        Task UpdateProfileImage(string fileName);

        Task UpdateProfile(Customer profile);

    }
}
