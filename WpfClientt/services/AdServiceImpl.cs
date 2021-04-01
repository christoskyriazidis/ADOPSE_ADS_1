using WpfClientt.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services.filtering;
using System.IO;
using System.Text.Json;

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

        public async Task Delete(Ad ad) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{mainUrl}/{ad.Id}");
            HttpResponseMessage response =  await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) {
                throw new ApplicationException("Unsuccessful delete request");
            }
        }

        public IScroller<Ad> Fiter(AdsFilterBuilder adsFilterBuilder) {
            string url = adsFilterBuilder.build();
            adsFilterBuilder.ClearFilters();
            return new GenericScroller<Ad>(client, 10, url);
        }

        public async Task<Ad> ReadById(long id) {
            Stream stream = await client.GetStreamAsync($"{mainUrl}/{id}");
            Ad ad = await JsonSerializer.DeserializeAsync<Ad>(stream);

            return ad;
        }

        public IScroller<Ad> Scroller() {
            return new GenericScroller<Ad>(client, 10, mainUrl);
        }

        public Task Update(Ad ad) {
            throw new NotImplementedException();
        }
    }
}
