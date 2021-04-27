using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfClientt.model;

namespace WpfClientt.services {
    class CustomerServiceImpl : ICustomerService {

        private HttpClient client;
        private string mainUrl = ApiInfo.CustomerMainUrl();

        public CustomerServiceImpl(HttpClient client) {
            this.client = client;
        }

        public async Task Create(Customer customer) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, mainUrl);
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("username",customer.Username);
            parameters.Add("address", customer.Address);
            parameters.Add("name", customer.FirstName);
            parameters.Add("lastName", customer.LastName);
            request.Content = new FormUrlEncodedContent(parameters);
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                if(customer.ProfileImageUri != null) {
                    await UpdateProfileImage(customer.ProfileImageUri.LocalPath);
                }
            }
        }

        public async Task Delete(Customer customer) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{mainUrl}/{customer.Id}");
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }


        public async Task<Customer> Profile() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,ApiInfo.ProfileMainUrl());
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                return await JsonSerializer.DeserializeAsync<Customer>(await response.Content.ReadAsStreamAsync());
            }
        }

        public async Task<Customer> ReadById(long id) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{mainUrl}/{id}");
            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                return await JsonSerializer.DeserializeAsync<Customer>(await response.Content.ReadAsStreamAsync());
            }
        }

        public IScroller<Customer> Scroller() {
            return new GenericScroller<Customer>(client, 10, mainUrl);
        }

        public async Task Update(Customer customer) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{mainUrl}/{customer.Id}");

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", customer.FirstName);
            parameters.Add("lastName", customer.LastName);
            parameters.Add("address", customer.Address);

            FormUrlEncodedContent form = new FormUrlEncodedContent(parameters);
            request.Content = form;

            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task UpdateProfile(Customer profile) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, ApiInfo.ProfileMainUrl());
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", profile.FirstName);
            parameters.Add("lastName", profile.LastName);
            parameters.Add("address", profile.Address);

            request.Content = new FormUrlEncodedContent(parameters);

            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }

        }

        public async Task UpdateProfileImage(string path) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put,$"{ApiInfo.ProfileMainUrl()}/image");
            MultipartFormDataContent form = new MultipartFormDataContent();

            if (!File.Exists(path)) {
                throw new FileNotFoundException("File not found at specified path : " + path);
            }

            using(ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(path))) {
                form.Add(fileContent, "image", Path.GetFileName(path));
                using (HttpResponseMessage response = await client.SendAsync(request)) {
                    response.EnsureSuccessStatusCode();
                }
            }

        }
    }
}
