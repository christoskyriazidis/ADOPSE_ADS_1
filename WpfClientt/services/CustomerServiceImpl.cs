using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    public class CustomerServiceImpl : ICustomerService {
        private HttpClient client;
        private string mainUrl = "https://6055bef691ea2900170d30d2.mockapi.io/customers";

        public CustomerServiceImpl(HttpClient client) {
            this.client = client;
        }


        public void Create(Customer t) {
            throw new NotImplementedException();
        }

        public void Delete(Customer t) {
            throw new NotImplementedException();
        }

        public bool Login(string username, string password) {
            throw new NotImplementedException();
        }

        public Customer ReadById(long id) {
            throw new NotImplementedException();
        }

        public IScroller<Customer> Scroller() {
            return new GenericScroller<Customer>(client, 10, mainUrl);
        }

        public void Update(Customer t) {
            throw new NotImplementedException();
        }
    }
}
