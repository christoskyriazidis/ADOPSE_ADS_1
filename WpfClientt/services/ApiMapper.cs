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
            await LoadCategories(dictionary => { });
            return TruGetOrElseDefault(id, categories);
        }

        public async Task<string> MapCondition(long id) {
            await LoadConditions(dictionary => { });
            return TruGetOrElseDefault(id, conditions);
        }

        public async Task<string> MapState(long id) {
            await LoadStates(dictionary => { });
            return TruGetOrElseDefault(id, states);
        }
        public async Task<string> MapManufacturer(long id) {
            await LoadManufacturers(dictionary => { });
            return TruGetOrElseDefault(id, manufacturers);
        }

        public async Task<string> MapType(long id) {
            await LoadTypes(dictionary => { });
            return TruGetOrElseDefault(id, types);
        }

        private async Task LoadInto(string url,IDictionary<long,string> dictionary) {
            Stream stream = await client.GetStreamAsync(url);
            List<Mapping> mappings = await JsonSerializer.DeserializeAsync<List<Mapping>>(stream);
            mappings.ForEach(mapping => dictionary.Add(mapping.id,mapping.title));
        }

        public async Task LoadCategories(Action<IDictionary<long, string>> afterLoad) {
            if (categories.Count == 0) {
                await LoadInto(ApiInfo.CategoriesMainUrl(), categories);
            }
            afterLoad.Invoke(categories);
        }

        public async Task LoadConditions(Action<IDictionary<long, string>> afterLoad) {
            if (conditions.Count == 0) {
                await LoadInto(ApiInfo.ConditionsMainUrl(), conditions);
            }
            afterLoad.Invoke(conditions);
        }

        public async Task LoadTypes(Action<IDictionary<long, string>> afterLoad) {
            if (types.Count == 0) {
                await LoadInto(ApiInfo.TypeMainUrl(), types);
            }
            afterLoad.Invoke(types);
        }

        public async Task LoadManufacturers(Action<IDictionary<long, string>> afterLoad) {
            if (manufacturers.Count == 0) {
                await LoadInto(ApiInfo.ManufacturerMainUrl(), manufacturers);
            }
            afterLoad.Invoke(manufacturers);
        }

        public async Task LoadStates(Action<IDictionary<long, string>> afterLoad) {
            if (states.Count == 0) {
                await LoadInto(ApiInfo.StatesMainUrl(), states);
            }
            afterLoad.Invoke(states);
        }

        private string TruGetOrElseDefault(long id, IDictionary<long, string> dictionary) {
            return dictionary.TryGetValue(id, out string value) ? value : defaultValue;
        }
    }
}
