using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {
    /// <summary>
    /// Represents a single chat.
    /// </summary>
    public class Chat {

        /// <summary>
        /// The chat id.
        /// </summary>
        [JsonPropertyName("Id")]
        public int ChatId { get; set; }

        public Ad Ad { get; set; }

        public Customer Customer { get; set; }

        /// <summary>
        /// Indicates whether the ad has been sold or not.
        /// </summary>
        public bool Sold { get; set; }


        /// <summary>
        /// The latest message sent in this chat.
        /// </summary>
        public string LatestMessage { get; set; }
    }
}
