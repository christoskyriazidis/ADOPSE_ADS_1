using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model.chat {
    public class Chat {

        [JsonPropertyName("Id")]
        public int ChatId { get; set; }

        [JsonPropertyName("customerId")]
        public int SellerId { get; set; }

        public int AdId { get; set; }

        public bool Sold { get; set; }

        public string ProfileImg { get; set; }

        public string Username { get; set; }

        public AdType Type { get; set; }

        public string LatestMessage { get; set; }
    }
}
