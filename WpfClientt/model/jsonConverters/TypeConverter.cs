using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.model.jsonConverters {

    /// <summary>
    /// Converter that converts long ids representing values of ad types to instances of the class AdType.
    /// </summary>
    public class TypeConverter : JsonConverter<AdType> {

        private static TypeConverter instance;

        private ISet<AdType> types;

        private TypeConverter(ISet<AdType> types) {
            this.types = types;
        }

        public override AdType Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            int typeId = reader.GetInt32();
            return types.Where( type => type.Id.Equals(typeId)).First();
        }

        public override void Write(Utf8JsonWriter writer, AdType value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public static async Task<TypeConverter> getInstance(IAdDetailsService service) {

            if(instance == null) {
                instance = new TypeConverter(await service.Types());
            }

            return instance;
        }
    }
}
