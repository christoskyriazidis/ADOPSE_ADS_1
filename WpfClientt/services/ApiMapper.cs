using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    class ApiMapper : IMapper {

        private string defaultValue = "unknow";
        private HttpClient client;
        private ConcurrentDictionary<long, string> categories = new ConcurrentDictionary<long, string>();
        private ConcurrentDictionary<long, string> conditions = new ConcurrentDictionary<long, string>();
        private ConcurrentDictionary<long, string> states = new ConcurrentDictionary<long, string>();
        private ConcurrentDictionary<long, string> types = new ConcurrentDictionary<long, string>();
        private ConcurrentDictionary<long, string> manufacturers = new ConcurrentDictionary<long, string>();

        public ApiMapper(HttpClient client) {
            this.client = client;
        }

        private async Task LoadInto(string url, ConcurrentDictionary<long,string> dictionary) {
            Stream stream = await client.GetStreamAsync(url);
            List<Mapping> mappings = await JsonSerializer.DeserializeAsync<List<Mapping>>(stream);
            mappings.ForEach(mapping => {
                dictionary.TryAdd(mapping.id, mapping.title);
                }
            );
        }

        public async Task<IDictionary<long, string>> Categories() {
            if (categories.Count == 0) {
                await LoadInto(ApiInfo.CategoriesMainUrl(), categories);
            }
            return categories;
        }

        public async Task<IDictionary<long, string>> Conditions() {
            if (conditions.Count == 0) {
                await LoadInto(ApiInfo.ConditionsMainUrl(), conditions);
            }
            return conditions;
        }

        public async Task<IDictionary<long, string>> Types() {
            if (types.Count == 0) {
                await LoadInto(ApiInfo.TypeMainUrl(), types);
            }
            return types;
        }

        public async Task<IDictionary<long, string>> Manufacturers() {
            if (manufacturers.Count == 0) {
                await LoadInto(ApiInfo.ManufacturerMainUrl(), manufacturers);
            }
            return manufacturers;
        }

        public async Task<IDictionary<long, string>> States() {
            if (states.Count == 0) {
                await LoadInto(ApiInfo.StatesMainUrl(), states);
            }
            return states;
        }

    }
}
