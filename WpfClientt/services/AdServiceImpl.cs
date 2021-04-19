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
using System.Diagnostics;
using System.Net.Http.Headers;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.services {
    class AdServiceImpl : IAdService {

        private static AdServiceImpl adServiceImpl;

        private HttpClient client;
        private string mainUrl = ApiInfo.AdMainUrl();
        private JsonSerializerOptions options;

        private AdServiceImpl(HttpClient client,JsonSerializerOptions options) {
            this.client = client;
            this.options = options;
        }

        public async static Task<AdServiceImpl> getInstance(HttpClient httpClient,IAdDetailsService adDetailsService) { 
            
            if(adServiceImpl == null) {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(await CategoryConverter.getInstance(adDetailsService));
                options.Converters.Add(await ConditionConverter.getInstance(adDetailsService));
                options.Converters.Add(await ManufacturerConverter.getInstance(adDetailsService));
                options.Converters.Add(await StateConverter.getInstance(adDetailsService));
                options.Converters.Add(await SubcategoryConverter.getInstance(adDetailsService));
                options.Converters.Add(await TypeConverter.getInstance(adDetailsService));
                adServiceImpl = new AdServiceImpl(httpClient, options);
            }

            return adServiceImpl;

        }

        public async Task Create(Ad ad) {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(ad.AdState.Id.ToString()), "State");
            form.Add(new StringContent(ad.AdType.Id.ToString()), "Type");
            form.Add(new StringContent(ad.AdManufacturer.Id.ToString()), "Manufacturer");
            form.Add(new StringContent(ad.AdCondition.Id.ToString()), "Condition");
            form.Add(new StringContent(ad.AdCategory.Id.ToString()), "Category");
            form.Add(new StringContent(ad.AdSubcategory.Id.ToString()), "Subcategory");
            form.Add(new StringContent(ad.Title), "Title");
            form.Add(new StringContent(ad.Description), "Description");
            form.Add(new StringContent(ad.Price.ToString()), "Price");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, mainUrl);

            ByteArrayContent image = null;
            if (ad.ImageUri != null && File.Exists(ad.ImageUri.LocalPath)) {
                image = new ByteArrayContent(File.ReadAllBytes(ad.ImageUri.LocalPath));
                image.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(image,"Img",new FileInfo(ad.ImageUri.LocalPath).Name);
            }

            request.Content = form;
            Debug.WriteLine(await request.Content.ReadAsStringAsync());
            Debug.WriteLine("\n");
            Debug.WriteLine(request.Headers);
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                Debug.WriteLine(response.Headers);
                Debug.WriteLine(response.StatusCode);
                response.EnsureSuccessStatusCode();
            }

            if(image != null) {
                image.Dispose();
            }
        }

        public async Task Delete(Ad ad) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{mainUrl}/{ad.Id}");
            using (HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
            }
        }

        public IScroller<Ad> Fiter(AdsFilterBuilder adsFilterBuilder) {
            string url = adsFilterBuilder.build();
            adsFilterBuilder.ClearFilters();
            return new GenericScroller<Ad>(client, 10, url);
        }

        public IScroller<Ad> ProfileAds() {
            throw new NotImplementedException();
        }

        public async Task<Ad> ReadById(long id) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{mainUrl}/{id}");
            using ( HttpResponseMessage response = await client.SendAsync(request) ) {
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Ad>(stream,options);
            }
        }

        public IScroller<Ad> Scroller() {
            return new GenericScroller<Ad>(client, 10, mainUrl,options);
        }

        public IScroller<Ad> SubcategoryAds(Subcategory subcategory) {
            throw new NotImplementedException();
        }

        public async Task Update(Ad ad) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, mainUrl);

            IDictionary<string, string> parameteres = new Dictionary<string, string>() {
                {"id", ad.Id.ToString() },
                {"state", ad.AdState.Id.ToString() },
                {"type", ad.AdType.Id.ToString() },
                {"manufacturer", ad.AdManufacturer.Id.ToString() },
                {"condition", ad.AdCondition.Id.ToString() },
                {"category", ad.AdCategory.Id.ToString() },
                {"title", ad.Title },
                {"description", ad.Description },
                {"price", ad.Price.ToString() }
            };

            request.Content = new FormUrlEncodedContent(parameteres);

            using(HttpResponseMessage response = await client.SendAsync(request)) {
                response.EnsureSuccessStatusCode();
                //TODO:check if image changed.If not,don't send the request.
                await UpdateAdImage(ad.ImageUri.LocalPath, ad.Id);
            }

        }

        public async Task UpdateAdImage(string path,long id) {
            if (!File.Exists(path)) {
                throw new FileNotFoundException("File not found at specified path : " + path);
            }

            MultipartFormDataContent form = new MultipartFormDataContent();

            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                { "id",id.ToString()}
            });
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{mainUrl}/image");

            using (ByteArrayContent image = new ByteArrayContent(File.ReadAllBytes(path))) {
                form.Add(content);
                form.Add(image);
                request.Content = form;
                using(HttpResponseMessage response = await client.SendAsync(request)) {
                    response.EnsureSuccessStatusCode();
                }
            }

            
        }
    }
}
