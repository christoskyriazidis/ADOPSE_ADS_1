using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {
    public sealed class Customer {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [StringLength(25, MinimumLength = 4)]
        [Required]
        [JsonPropertyName("name")]
        public string FirstName { get; set; }

        [StringLength(25, MinimumLength = 4)]
        [Required]
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [JsonPropertyName("address")]
        public string Address{ get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; } = 0;

        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("profileImg")]
        public Uri ProfileImageUri { get; set; }

        public Customer() { 
        
        }


        public override string ToString() {
            return $"Id = {Id},Username={Username},Address={Address},Rating={Rating}";
        }
    }
}
