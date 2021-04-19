using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {
    public class Category {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("imageUrl")]
        public Uri ImageUri { get; set; }

        [JsonIgnore]
        public ICollection<Subcategory> Subcategories { get; } = new HashSet<Subcategory>();
    }
}
