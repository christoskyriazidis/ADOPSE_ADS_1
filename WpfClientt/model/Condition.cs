using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {
    /// <summary>
    /// Ad detail component representing the condition of the ad:new,used,etc.
    /// </summary>
    public class Condition : AdDetailComponent {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        public override string ToString() {
            return $"{Title}";
        }
    }
}
