using WpfClientt.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    class AdServiceImpl : IAdService {

        private HttpClient client;
        private string mainUrl = ApiInfo.AdMainUrl();

        public AdServiceImpl(HttpClient client) {
            this.client = client;
        }

        public Task Create(Ad ad) {
            throw new NotImplementedException();
        }

        public Task Delete(Ad ad) {
            throw new NotImplementedException();
        }

        public IScroller<Ad> Fiter(AdsFilterBuilder adsFilterBuilder) {
            string url = $"{ApiInfo.FilterMainUrl()}?{adsFilterBuilder.build()}";
            return new GenericScroller<Ad>(client, 10, url);
        }

        public Task<Ad> ReadById(long id) {
            throw new NotImplementedException();
        }

        public IScroller<Ad> Scroller() {
            return new GenericScroller<Ad>(client, 10, mainUrl);
        }

        public Task Update(Ad ad) {
            throw new NotImplementedException();
        }
    }
}
