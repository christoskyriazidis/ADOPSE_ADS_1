using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.model.jsonConverters {
    public class ConditionConverter : JsonConverter<Condition> {

        private static ConditionConverter instance;

        private ISet<Condition> conditions;

        private ConditionConverter(ISet<Condition> conditions) {
            this.conditions = conditions;
        }

        public override Condition Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            long conditionId = long.Parse(reader.GetString());
            return conditions.Where( condition => condition.Id.Equals(conditionId)).First();
        }

        public override void Write(Utf8JsonWriter writer, Condition value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public static async Task<ConditionConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new ConditionConverter(await service.Conditions());
            }

            return instance;
        }
    }
}
