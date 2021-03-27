using AggeliesProject.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    public class AdServiceImpl : IAdService {

        private HttpClient client;
        private string mainUrl = "https://6055bef691ea2900170d30d2.mockapi.io/ads";

        public AdServiceImpl(HttpClient client) {
            this.client = client;
        }

        public void Create(Ad ad) {
            throw new NotImplementedException();
        }

        public void Delete(Ad ad) {
            throw new NotImplementedException();
        }

        public Ad ReadById(long id) {
            throw new NotImplementedException();
        }

        public IScroller<Ad> Scroller() {
            return new GenericScroller<Ad>(client, 10, mainUrl);
        }

        public void Update(Ad ad) {
            throw new NotImplementedException();
        }
    }
}
