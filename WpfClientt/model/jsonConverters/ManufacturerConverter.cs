using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.model.jsonConverters {
    public class ManufacturerConverter : JsonConverter<Manufacturer> {

        private static ManufacturerConverter instance;

        private ISet<Manufacturer> manufacturers;

        private ManufacturerConverter(ISet<Manufacturer> manufacturers) {
            this.manufacturers = manufacturers;
        }

        public override Manufacturer Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            long manufacturerId = long.Parse(reader.GetString());

            return manufacturers.Where( manufacturer => manufacturer.Id.Equals(manufacturerId)).First();
        }

        public override void Write(Utf8JsonWriter writer, Manufacturer value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public static async Task<ManufacturerConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new ManufacturerConverter(await service.Manufacturers());
            }

            return instance;
        }
    }
}
