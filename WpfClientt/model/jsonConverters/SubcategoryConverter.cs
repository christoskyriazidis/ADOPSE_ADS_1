using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.model.jsonConverters {
    public class SubcategoryConverter : JsonConverter<Subcategory> {

        private static SubcategoryConverter instance;

        private ISet<Subcategory> subcategories;

        private SubcategoryConverter(ISet<Subcategory> subcategories) {
            this.subcategories = subcategories;
        }

        public override Subcategory Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            long subcategoryId = long.Parse(reader.GetString());
            return subcategories.Where( subcategory => subcategory.Id.Equals(subcategoryId)).First();
        }

        public override void Write(Utf8JsonWriter writer, Subcategory value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public static async Task<SubcategoryConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new SubcategoryConverter(await service.Subcategories());
            }

            return instance;
        }

    }
}
