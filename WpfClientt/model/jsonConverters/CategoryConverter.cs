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
    /// Converter that converts long ids representing values of categories to instances of the class Category.
    /// </summary>
    public class CategoryConverter : JsonConverter<Category> {
        private static CategoryConverter instance;

        private ISet<Category> categories; 

        private CategoryConverter(ISet<Category> categories) {
            this.categories = categories;
        }

        public override Category Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            int categoryId = reader.GetInt32();
            return categories.Where(category => category.Id.Equals(categoryId)).First();
        }

        public override void Write(Utf8JsonWriter writer, Category value, JsonSerializerOptions options) {
            writer.WriteNumber("Category", value.Id);
        }

        public static async Task<CategoryConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new CategoryConverter(await service.Categories());
            }

            return instance;
        }
    }
}
