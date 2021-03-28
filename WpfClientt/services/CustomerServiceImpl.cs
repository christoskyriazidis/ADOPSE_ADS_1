using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    class CustomerServiceImpl : ICustomerService {
        private HttpClient client;
        private string mainUrl = ApiInfo.CustomerMainUrl();

        public CustomerServiceImpl(HttpClient client) {
            this.client = client;
        }


        public Task Create(Customer t) {
            throw new NotImplementedException();
        }

        public Task Delete(Customer t) {
            throw new NotImplementedException();
        }

        public Task<bool> Login(string username, string password) {
            throw new NotImplementedException();
        }

        public Task<Customer> ReadById(long id) {
            throw new NotImplementedException();
        }

        public IScroller<Customer> Scroller() {
            return new GenericScroller<Customer>(client, 10, mainUrl);
        }

        public Task Update(Customer t) {
            throw new NotImplementedException();
        }
    }
}
