using System;
using System.Text.Json.Serialization;
using WpfClientt.model.jsonConverters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WpfClientt.model {
    /// <summary>
    /// Represents a model class for the Ad.
    /// </summary>
    public class Ad {

        /// <summary>
        /// The id of the ad.
        /// </summary>
        [JsonPropertyName("id")]
        public virtual long Id { get; set; }

        /// <summary>
        /// The state in which the ad is.
        /// </summary>
        [JsonPropertyName("state")]
        public virtual State AdState { get; set; }

        /// <summary>
        /// The type of the ad.
        /// </summary>
        [Required(ErrorMessage = "The type of the ad is not specified.")]
        [JsonPropertyName("type")]
        public virtual AdType AdType { get; set; }

        /// <summary>
        /// The manufacturer of the item of this ad.
        /// </summary>
        [Required(ErrorMessage = "The manufacturer is not specified.")]
        [JsonPropertyName("manufacturer")]
        public virtual Manufacturer AdManufacturer { get; set; }

        /// <summary>
        /// The condition in which the item of the ad is(NEW,USED).
        /// </summary>
        [Required(ErrorMessage = "The condition is not specified.")]
        [JsonPropertyName("condition")]
        public virtual Condition AdCondition { get; set; }

        /// <summary>
        /// The category to wich this ad/ad's item belongs.
        /// </summary>
        [Required(ErrorMessage = "The category is not specified.")]
        [JsonPropertyName("category")]
        public virtual Category AdCategory { get; set; }

        /// <summary>
        /// The subcategory to which this ad/ad's item belongs.
        /// </summary>
        [Required(ErrorMessage = "The subcategory is not specified.")]
        [JsonPropertyName("subcategoryid")]
        public virtual Subcategory AdSubcategory { get; set; }

        /// <summary>
        /// The title of this ad.
        /// </summary>
        [StringLength(72, MinimumLength = 10,ErrorMessage ="The title length must be between [10,72] characters.")]
        [Required(ErrorMessage ="The title of the ad is not specified.")]
        [JsonPropertyName("title")]
        public virtual string Title { get; set; }

        /// <summary>
        /// The description of this ad.
        /// </summary>
        [StringLength(1024, MinimumLength = 20,ErrorMessage = "The description length must be between [20,1024] chracters.")]
        [Required(ErrorMessage = "The description is not specified.")]
        [JsonPropertyName("description")]
        public virtual string Description { get; set; }

        /// <summary>
        /// Uri representing the image of this ad.
        /// </summary>
        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("img")]
        public virtual Uri ImageUri { get; set; }

        /// <summary>
        /// The number of views this ad has been seen.
        /// </summary>
        [JsonPropertyName("views")]
        public virtual int Views { get; set; } = 0;

        /// <summary>
        /// The number of reports this ad has received.
        /// </summary>
        [JsonPropertyName("reports")]
        public virtual int Reports { get; set; } = 0;

        /// <summary>
        /// The id of the customer that published this ad.
        /// </summary>
        [JsonPropertyName("customer")]
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// The price of this ad.
        /// </summary>
        [Range(1, 100000,ErrorMessage = "The price must be between [1,100000]")]
        [Required(ErrorMessage = "The price is not specified")]
        [JsonPropertyName("price")]
        public virtual int Price { get; set; }

        public Ad() { 
        
        }

       
    }
}
