using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.model {
    /// <summary>
    /// Representes a customer.
    /// </summary>
    public class Customer {

        /// <summary>
        /// The id of the customer on the server.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// The first name of the customer
        /// </summary>
        [JsonPropertyName("name")]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// The last name of the customer
        /// </summary>
        [JsonPropertyName("lastName")]
        public virtual string LastName { get; set; }

        /// <summary>
        /// The username of the customer
        /// </summary>
        [JsonPropertyName("username")]
        public virtual string Username { get; set; }

        /// <summary>
        /// The address at wich the customer lives.
        /// </summary>
        [JsonPropertyName("address")]
        public virtual string Address{ get; set; }

        /// <summary>
        /// The rating this customer received by other customers.
        /// </summary>
        [JsonPropertyName("rating")]
        public int Rating { get; set; } = 0;

        /// <summary>
        /// The email of the customer.
        /// </summary>
        [JsonPropertyName("email")]
        public virtual string Email { get; set; }
       
        /// <summary>
        /// The mobile phone of the customer.
        /// </summary>
        [JsonPropertyName("phone")]
        public virtual string MobilePhone { get; set; }

        /// <summary>
        /// The uri points to the image of the customer.
        /// </summary>
        [JsonConverter(typeof(JsonStringToUriConverter))]
        [JsonPropertyName("profileImg")]
        public Uri ProfileImageUri { get; set; }

        /// <summary>
        /// The password of the customer.
        /// </summary>
        public virtual string Password {get; set;}
        public Customer() { 
        
        }


        public override string ToString() {
            return $"Id = {Id},Username={Username},Address={Address},Rating={Rating}";
        }
    }
}
