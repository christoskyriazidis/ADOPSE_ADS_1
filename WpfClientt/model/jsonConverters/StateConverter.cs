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
    /// Converter that converts long ids representing values of states to instances of the class State.
    /// </summary>
    public class StateConverter : JsonConverter<State> {

        private static StateConverter instance;

        private ISet<State> states;

        private StateConverter(ISet<State> states) {
            this.states = states;
        }

        public override State Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
            int stateId = reader.GetInt32();
            return states.Where(state => state.Id.Equals(stateId)).First();
        }

        public override void Write(Utf8JsonWriter writer, State value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public static async Task<StateConverter> getInstance(IAdDetailsService service) {
            if(instance == null) {
                instance = new StateConverter(await service.States());
            }

            return instance;
        }
    }
}
