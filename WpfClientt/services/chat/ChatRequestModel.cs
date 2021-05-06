using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.services {
    class ChatRequestModel {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("buyerId")]
        public int BuyerId { get; set; }

        [JsonPropertyName("adId")]
        public int AdId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("profileImg")]
        public string ProfileImg { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
    }
}
