using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {
    public sealed class Ad {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("state")]
        public State AdState{ get; set; }

        [JsonPropertyName("type")]
        public AdType AdType{ get; set; }

        [JsonPropertyName("manufacturer")]
        public Manufacturer AdManufacturer { get; set; }

        [JsonPropertyName("condition")]
        public Condition AdCondition { get; set; }

        [JsonPropertyName("category")]
        public Category AdCategory { get; set; }

        [JsonPropertyName("subcategory")]
        public Subcategory AdSubcategory { get; set; }

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
        public Customer AdCustomer{ get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        public Ad() { 
        
        }

       
    }
}
