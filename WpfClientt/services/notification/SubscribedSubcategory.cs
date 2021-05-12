using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.services {
    class SubscribedSubcategory {

        [JsonPropertyName("categoryId")]
        public int SubcategoryId { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

    }
}
