using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model.jsonConverters {
    /// <summary>
    /// Converter that converts string values to instances of Uri class.
    /// </summary>
    class JsonStringToUriConverter : JsonConverter<Uri> {
        public override Uri Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            return new Uri(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Uri value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }
    }
}
