using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.services {
    class ChatModel {

        [JsonPropertyName("id")]
        public int ChatId { get; set; }

        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("adId")]
        public int AdId { get; set; }

        [JsonPropertyName("sold")]
        public bool Sold { get; set; }

        [JsonPropertyName("profileImg")]
        public string ProfileImg { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("latestMessage")]
        public string LatestMessage { get; set; }
    }
}
