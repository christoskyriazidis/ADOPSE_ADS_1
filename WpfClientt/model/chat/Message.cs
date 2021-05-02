using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model.chat {
    public class Message {

        [Required]
        [StringLength(250, MinimumLength = 3)]
        [JsonPropertyName("Id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MessageId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("Message")]
        public string Body { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("Customer")]
        public int SenderId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [JsonPropertyName("ActiveChat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ChatId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Timestamp { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Username { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ProfileImg { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SubId { get; set; }
    }
}
