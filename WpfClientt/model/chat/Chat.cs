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

        /// <summary>
        /// The id of the seller customer.
        /// </summary>
        [JsonPropertyName("customerId")]
        public int SellerId { get; set; }

        /// <summary>
        /// The id of the ad for wich the chat started.
        /// </summary>
        public int AdId { get; set; }

        /// <summary>
        /// Indicates whether the ad has been sold or not.
        /// </summary>
        public bool Sold { get; set; }

        /// <summary>
        /// The url to the profile image of the seller customer.
        /// </summary>
        public string ProfileImg { get; set; }

        /// <summary>
        /// The username of the seller
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The type of the ad for which the chat started.
        /// </summary>
        public AdType Type { get; set; }

        /// <summary>
        /// The latest message sent in this chat.
        /// </summary>
        public string LatestMessage { get; set; }
    }
}
