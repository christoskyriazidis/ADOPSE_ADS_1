using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public interface AdDetailComponent {

        [JsonPropertyName("id")]
        int Id { get; set; }

        [JsonPropertyName("title")]
        string Title { get; set; }

        
    }
}
