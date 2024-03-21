using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Rogue.Coe.Serialization
{
    public class GameBehaviourConverter : JsonConverter<IGameBehaviour>
    {
        public override bool CanRead => false;

        public override bool CanWrite => false;

        public override IGameBehaviour ReadJson(JsonReader reader, Type objectType, IGameBehaviour existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
            /*
            JObject jobj = serializer.Deserialize<JObject>(reader);
            IGameBehaviour beha;
            // Creates a new component if it is needed.
            if (hasExistingValue)
            {
                beha = existingValue;
            }
            else
            {
                beha = GameBehaviourUtil.CreateFromName(jobj.Value<string>("class"));
                // Invalid class name.
                if (beha == null)
                {
                    return null;
                }
            }
            
            serializer.Populate(jobj.CreateReader(), beha);
            return beha;
            */
        }

        public override void WriteJson(JsonWriter writer, IGameBehaviour value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
