using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public class FactoryServices {

        private ICustomerService customerService;
        private IAdDetailsService adDetailsService;
        private HttpClient client;

        public FactoryServices() {
            client = new HttpClient();
            customerService = new CustomerServiceImpl(client);
            adDetailsService = new AdDetailsServiceImpl(client);
        }

        public async Task<IAdService> AdServiceInstance() {
            return await AdServiceImpl.GetInstance(client,AdDetailsServiceInstance());
        }

        public ICustomerService CustomerServiceInstance() {
            return customerService;
        }

        public IAdDetailsService AdDetailsServiceInstance() {
            return adDetailsService;
        }

    }
}
