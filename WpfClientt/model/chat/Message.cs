using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {
    /// <summary>
    /// Message that belongs to a chat.
    /// </summary>
    public class Message {

        /// <summary>
        /// The id of the message.
        /// </summary>
        [Required]
        [StringLength(250, MinimumLength = 3)]
        [JsonPropertyName("Id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MessageId { get; set; }

        /// <summary>
        /// The body of the message(text)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("Message")]
        public string Body { get; set; }

        /// <summary>
        /// The id of the customer who sent this message.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("Customer")]
        public int SenderId { get; set; }

        /// <summary>
        /// The id of the chat to which this message belongs.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        [JsonPropertyName("ActiveChat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ChatId { get; set; }

        /// <summary>
        /// A string indicating when the message was sent.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Timestamp { get; set; }
        
        /// <summary>
        /// The username of the sender customer.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Username { get; set; }

        /// <summary>
        /// The url to the profile image of the sender customer.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ProfileImg { get; set; }

        /// <summary>
        /// Subject id of the sender.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SubId { get; set; }
    }
}
