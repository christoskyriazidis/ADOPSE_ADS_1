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
    /// Converter that converts long ids representing values of manufacturers to instances of the class Manufacturer.
    /// </summary>
    public class ManufacturerConverter : JsonConverter<Manufacturer> {

        private static ManufacturerConverter instance;

        private ISet<Manufacturer> manufacturers;

        private ManufacturerConverter(ISet<Manufacturer> manufacturers) {
            this.manufacturers = manufacturers;
        }

        public override Manufacturer Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            int manufacturerId = reader.GetInt32();
            return manufacturers.Where( manufacturer => manufacturer.Id.Equals(manufacturerId)).First();
        }

        public override void Write(Utf8JsonWriter writer, Manufacturer value, JsonSerializerOptions options) {
            writer.WriteNumber("Manufacturer", value.Id);
        }

        public static async Task<ManufacturerConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new ManufacturerConverter(await service.Manufacturers());
            }

            return instance;
        }
    }
}
