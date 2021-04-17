using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {
    public sealed class Customer {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("address")]
        public string Address{ get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; } = 0;

        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("profileImg")]
        public Uri ProfileImageUri { get; set; }

        public Customer() { 
        
        }

        public Customer(long id, string username, string address,int rating = 0) {
            Id = id;
            Username = username;
            Address = address;
            Rating = rating;
        }

        public override string ToString() {
            return $"Id = {Id},Username={Username},Address={Address},Rating={Rating}";
        }
    }
}
