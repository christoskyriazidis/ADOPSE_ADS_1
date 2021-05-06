using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    class ChatScroller : IScroller<Message> {
        private object lockObject = new object();
        private Chat chat;
        private HttpClient client;
        private int nextPageNumber = 1;
        private ChatPage currentPage;

        public ChatScroller(Chat chat, HttpClient client) {
            this.client = client;
            this.chat = chat;
        }

        public async Task Init() {
            int currentPageNumber;
            lock (lockObject) {
                currentPageNumber = nextPageNumber;
                nextPageNumber = nextPageNumber + 1;
            }
            await RetrievePage(currentPageNumber);
        }

        public IPage<Message> CurrentPage() {
            return currentPage;
        }

        public async Task<bool> MoveBack() {
            int pageNumber;
            lock (lockObject) {
                pageNumber = nextPageNumber - 1;
                nextPageNumber = nextPageNumber - 1;
            }
            await RetrievePage(pageNumber);
            return CurrentPage().Objects().Count != 0;
        }

        public async Task<bool> MoveNext() {
            int pageNumber;
            lock (lockObject) {
                pageNumber = nextPageNumber;
                nextPageNumber = nextPageNumber + 1;
            }
            await RetrievePage(pageNumber);
            return CurrentPage().Objects().Count != 0;
        }

        public int NumberOfPages() {
            //not specified.
            return -1;
        }

        private async Task RetrievePage(int pageNumber) {
            string url = $"{ApiInfo.MessageMainUrl()}?PageNumber={pageNumber}&ChatId={chat.ChatId}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                IList<Message> messages = await JsonSerializer
                    .DeserializeAsync<IList<Message>>(await response.Content.ReadAsStreamAsync());
                currentPage = new ChatPage(pageNumber, messages);
            }
        }
    }
}
