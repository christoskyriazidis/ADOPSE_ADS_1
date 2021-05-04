using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {

    /// <summary>
    /// Ad detail component that represents the subcategory of the ad/ad's item.
    /// </summary>
    public class Subcategory : AdDetailComponent {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Uri points to the image of this subcategory.
        /// </summary>
        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("imageUrl")]
        public Uri ImageUri { get; set; }

        /// <summary>
        /// The id of the category to wich this subcategory belongs.
        /// </summary>
        [JsonPropertyName("categoryId")]
        public long CategoryId { get; set; }
        public override string ToString() {
            return $"{Title}";
        }
    }
}
