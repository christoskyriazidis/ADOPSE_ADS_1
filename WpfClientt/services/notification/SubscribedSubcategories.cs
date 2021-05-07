using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.services {
    class SubscribedSubcategories {

        [JsonPropertyName("categories")]
        public int[] Categories { get; set; } = new int[0];

    }
}
