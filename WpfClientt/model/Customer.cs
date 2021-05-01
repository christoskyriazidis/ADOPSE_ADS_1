using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {
    public class Customer {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public virtual string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public virtual string LastName { get; set; }

        [JsonPropertyName("username")]
        public virtual string Username { get; set; }

        [JsonPropertyName("address")]
        public virtual string Address{ get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; } = 0;

        [JsonPropertyName("email")]
        public virtual string Email { get; set; }
       
        [JsonPropertyName("phone")]
        public virtual string MobilePhone { get; set; }

        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("profileImg")]
        public Uri ProfileImageUri { get; set; }

        public virtual string Password {get; set;}
        public Customer() { 
        
        }


        public override string ToString() {
            return $"Id = {Id},Username={Username},Address={Address},Rating={Rating}";
        }
    }
}
