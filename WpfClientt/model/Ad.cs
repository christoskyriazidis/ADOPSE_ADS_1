using System;
using System.Text.Json.Serialization;
using WpfClientt.model.jsonConverters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WpfClientt.model {
    public sealed class Ad {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("state")]
        public State AdState{ get; set; }

        [Required(ErrorMessage = "The type of the ad is not specified.")]
        [JsonPropertyName("type")]
        public AdType AdType{ get; set; }

        [Required(ErrorMessage = "The manufacturer is not specified.")]
        [JsonPropertyName("manufacturer")]
        public Manufacturer AdManufacturer { get; set; }

        [Required(ErrorMessage = "The condition is not specified.")]
        [JsonPropertyName("condition")]
        public Condition AdCondition { get; set; }

        [Required(ErrorMessage = "The category is not specified.")]
        [JsonPropertyName("category")]
        public Category AdCategory { get; set; }

        [Required(ErrorMessage = "The subcategory is not specified.")]
        [JsonPropertyName("subcategory")]
        public Subcategory AdSubcategory { get; set; }

        [StringLength(72, MinimumLength = 10,ErrorMessage ="The title length must be between [10,72] characters.")]
        [Required(ErrorMessage ="The title of the ad is not specified.")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [StringLength(1024, MinimumLength = 20,ErrorMessage = "The description length must be between [20,1024] chracters.")]
        [Required(ErrorMessage = "The description is not specified.")]
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

        [Range(1, 100000,ErrorMessage = "The price must be between [1,100000]")]
        [Required(ErrorMessage = "The price is not specified")]
        [JsonPropertyName("price")]
        public int? Price { get; set; }

        public Ad() { 
        
        }

       
    }
}
