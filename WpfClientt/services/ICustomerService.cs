using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    public interface ICustomerService : IService<Customer>{

        bool Login(string username, string password);

    }
}
