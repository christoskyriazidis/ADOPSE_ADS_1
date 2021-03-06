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
    /// Converter that converts long ids representing values of conditions to instances of the class Condition.
    /// </summary>
    public class ConditionConverter : JsonConverter<Condition> {

        private static ConditionConverter instance;

        private ISet<Condition> conditions;

        private ConditionConverter(ISet<Condition> conditions) {
            this.conditions = conditions;
        }

        public override Condition Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            int conditionId = reader.GetInt32();
            return conditions.Where( condition => condition.Id.Equals(conditionId)).First();
        }

        public override void Write(Utf8JsonWriter writer, Condition value, JsonSerializerOptions options) {
            writer.WriteNumber("Condition", value.Id);
        }

        public static async Task<ConditionConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new ConditionConverter(await service.Conditions());
            }

            return instance;
        }
    }
}
