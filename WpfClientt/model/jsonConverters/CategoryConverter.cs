﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.model.jsonConverters {
    public class CategoryConverter : JsonConverter<Category> {
        private static CategoryConverter instance;

        private ISet<Category> categories; 

        private CategoryConverter(ISet<Category> categories) {
            this.categories = categories;
        }

        public override Category Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            long categoryId = long.Parse(reader.GetString());
            return categories.Where(category => category.Id.Equals(category)).First();
        }

        public override void Write(Utf8JsonWriter writer, Category value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public static async Task<CategoryConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new CategoryConverter(await service.Categories());
            }

            return instance;
        }
    }
}