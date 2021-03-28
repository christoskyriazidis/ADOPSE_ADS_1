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

        public async Task<string> MapCategory(long id) {
            if (categories.Count == 0) {
                await LoadInto(ApiInfo.CategoriesMainUrl(), categories);
            }
            return TruGetOrElseDefault(id, categories);
        }

        public async Task<string> MapCondition(long id) {
            if (conditions.Count == 0) {
                await LoadInto(ApiInfo.ConditionsMainUrl(), conditions);
            }
            return TruGetOrElseDefault(id, conditions);
        }

        public async Task<string> MapState(long id) {
            if (states.Count == 0) {
                await LoadInto(ApiInfo.StatesMainUrl(), states);
            }
            return TruGetOrElseDefault(id, states);
        }
        public async Task<string> MapManufacturer(long id) {
            if (manufacturers.Count == 0) {
                await LoadInto(ApiInfo.ManufacturerMainUrl(), manufacturers);
            }
            return TruGetOrElseDefault(id, manufacturers);
        }

        public async Task<string> MapType(long id) {
            if (types.Count == 0) {
                await LoadInto(ApiInfo.TypeMainUrl(), types);
            }
            return TruGetOrElseDefault(id, types);
        }

        private async Task LoadInto(string url,IDictionary<long,string> dictionary) {
            Stream stream = await client.GetStreamAsync(url);
            List<Mapping> mappings = await JsonSerializer.DeserializeAsync<List<Mapping>>(stream);
            mappings.ForEach(mapping => dictionary.Add(mapping.id,mapping.title));
        }

        private string TruGetOrElseDefault(long id, IDictionary<long, string> dictionary) {
            return dictionary.TryGetValue(id, out string value) ? value : defaultValue;
        }

    }
}
