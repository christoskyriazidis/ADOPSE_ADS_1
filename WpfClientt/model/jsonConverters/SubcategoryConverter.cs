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
    /// Converter that converts long ids representing values of subcategories to instances of the class Subcategory.
    /// </summary>
    public class SubcategoryConverter : JsonConverter<Subcategory> {

        private static SubcategoryConverter instance;

        private ISet<Subcategory> subcategories;

        private SubcategoryConverter(ISet<Subcategory> subcategories) {
            this.subcategories = subcategories;
        }

        public override Subcategory Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            int subcategoryId = reader.GetInt32();
            return subcategories.Where( subcategory => subcategory.Id.Equals(subcategoryId)).First();
        }

        public override void Write(Utf8JsonWriter writer, Subcategory value, JsonSerializerOptions options) {
            writer.WriteNumber("SubCategoryId", value.Id);
        }

        public static async Task<SubcategoryConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new SubcategoryConverter(await service.Subcategories());
            }

            return instance;
        }

    }
}
