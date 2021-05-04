using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {
    /// <summary>
    /// Ad detail component representing the category of the ad/ad's item.
    /// </summary>
    public class Category : AdDetailComponent {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The image presented to the user that represents this category.
        /// </summary>
        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("imageUrl")]
        public Uri ImageUri { get; set; }

        /// <summary>
        /// The subcategories of this category.
        /// </summary>
        [JsonIgnore]
        public ICollection<Subcategory> Subcategories { get; } = new HashSet<Subcategory>();

        public override string ToString() {
            return $"{Title}";
        }
    }
}
