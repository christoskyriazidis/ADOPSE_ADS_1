using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public sealed class Ad {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("state")]
        public long StateId { get; set; }

        [JsonPropertyName("type")]
        public long TypeId { get; set; }

        [JsonPropertyName("manufacturer")]
        public long ManufacturerId { get; set; }

        [JsonPropertyName("condition")]
        public long ConditionId { get; set; }

        [JsonPropertyName("category")]
        public long CategoryId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("img")]
        public Uri ImageUri { get; set; }

        [JsonPropertyName("views")]
        public int Views { get; set; } = 0;

        [JsonPropertyName("reports")]
        public int Reports { get; set; } = 0;

        [JsonPropertyName("customer")]
        public long CustomerId { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }
        public int SubCategoryId { get; set; }

        public Ad() { 
        
        }

        public Ad(long id, long stateId, long typeId, long manufacturerId, long conditionId, 
            long categoryId, string title, string description, Uri imageUri,int price, int views, int reports) {
            this.Id = id;
            this.StateId = stateId;
            this.TypeId = typeId;
            this.ManufacturerId = manufacturerId;
            this.ConditionId = conditionId;
            this.CategoryId = categoryId;
            this.Title = title;
            this.Description = description;
            this.ImageUri = imageUri;
            this.Views = views;
            this.Reports = reports;
            this.Price = price;
        }

        public void Report() {
            Reports++;
        }

        public int GetReports() {
            return Reports;
        }

        public void Viewed() {
            Views++;
        }

        public int GetViews() {
            return Views;
        }

        public override string ToString() {
            return $"Id={Id},StateId={StateId},TypeId={TypeId},ManufacturerId={ManufacturerId},ConditionId={ConditionId}," +
                $"CategoryId={CategoryId},Title={Title},Description={Description},Img={ImageUri},Reports={Reports},Views={Views}";
        }
    }
}
