using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {

    /// <summary>
    /// Interface that is implemented by Ad details components(Type,Manufacturer,etc).
    /// </summary>
    public interface AdDetailComponent {

        /// <summary>
        /// The id of this component used on the server.
        /// </summary>
        [JsonPropertyName("id")]
        int Id { get; set; }

        /// <summary>
        /// The user friendly title of this component.
        /// </summary>
        [JsonPropertyName("title")]
        string Title { get; set; }

        
    }
}
