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
        private IDictionary<long, string> categories = new ConcurrentDictionary<long, string>();
        private IDictionary<long, string> conditions = new ConcurrentDictionary<long, string>();
        private IDictionary<long, string> states = new ConcurrentDictionary<long, string>();
        private IDictionary<long, string> types = new ConcurrentDictionary<long, string>();
        private IDictionary<long, string> manufacturers = new ConcurrentDictionary<long, string>();

        public ApiMapper(HttpClient client) {
            this.client = client;
        }

        private async Task LoadInto(string url,IDictionary<long,string> dictionary) {
            Stream stream = await client.GetStreamAsync(url);
            List<Mapping> mappings = await JsonSerializer.DeserializeAsync<List<Mapping>>(stream);
            mappings.ForEach(mapping => dictionary.Add(mapping.id,mapping.title));
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

        private string TruGetOrElseDefault(long id, IDictionary<long, string> dictionary) {
            return dictionary.TryGetValue(id, out string value) ? value : defaultValue;
        }
    }
}
