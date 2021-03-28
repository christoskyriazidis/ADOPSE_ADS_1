using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public class FactoryServices {

        private IAdService adService;
        private ICustomerService customerService;

        public FactoryServices() {
            HttpClient client = new HttpClient();
            adService = new AdServiceImpl(client);
            customerService = new CustomerServiceImpl(client);
        }

        public IAdService AdServiceInstance() {
            return adService;
        }

        public ICustomerService CustomerServiceInstance() {
            return customerService;
        }

    }
}
