using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {
    /// <summary>
    /// Ad detail component that represents the state of the ad(sold,for example).
    /// </summary>
    public class State : AdDetailComponent  {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        public override string ToString() {
            return $"{Title}";
        }
    }
}
