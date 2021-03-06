using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfClientt.services {
    class GenericScroller<T> : IScroller<T> {

        private HttpClient client;
        private int size;
        private GenericPage<T> currentPage;
        private string url;
        private JsonSerializerOptions options;

        public GenericScroller(HttpClient client, int size, string url,JsonSerializerOptions options = null) {
            this.client = client;
            this.size = size;
            this.options = options;
            if (url.Contains("?")) {
                this.url = $"{url}&PageNumber=1&PageSize={size}";
            } else {
                this.url = $"{url}?PageNumber=1&PageSize={size}";
            }
        }

        public async Task Init() {
            await SetCurrentPage();
        }

        public IPage<T> CurrentPage() {
            return currentPage;
        }

        public async Task<bool> MoveBack() {
            string previousPageUrl = currentPage.PreviousPageUrl;
            if (previousPageUrl.Equals(url)) {
                return false;
            }
            this.url = previousPageUrl;
            await SetCurrentPage();
            return true;
        }

        public async Task<bool> MoveNext() {
            string nextPageUrl = currentPage.NextPageUrl;
            if (nextPageUrl.Equals(url)) {
                return false;
            }
            this.url = nextPageUrl;
            await SetCurrentPage();
            return true;
        }

        public int NumberOfPages() {
            return currentPage.NumOfPages;
        }

        private async Task SetCurrentPage() {
            Stream stream = await this.client.GetStreamAsync(this.url);
            currentPage = await JsonSerializer.DeserializeAsync<GenericPage<T>>(stream,options);
        }
    }
}
