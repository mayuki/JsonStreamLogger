using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonStreamLogger.Serialization.JsonNet
{
    internal class ReadOnlyListKeyValuePairStringObjectConverter : JsonConverter<IReadOnlyList<KeyValuePair<string, object>>>
    {
        public static readonly ReadOnlyListKeyValuePairStringObjectConverter Instance = new ReadOnlyListKeyValuePairStringObjectConverter();

        public override IReadOnlyList<KeyValuePair<string, object>> ReadJson(JsonReader reader, Type objectType, IReadOnlyList<KeyValuePair<string, object>> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, IReadOnlyList<KeyValuePair<string, object>> value, JsonSerializer serializer)
        {
            var hashSet = new HashSet<string>();
            writer.WriteStartObject();
            foreach (var keyValue in value)
            {
                // special name for Microsoft.Extensions.Logging
                if (String.CompareOrdinal(keyValue.Key, "{OriginalFormat}") == 0) continue;

                // ignore a key which is already appeared.
                if (hashSet.Contains(keyValue.Key)) continue;
                hashSet.Add(keyValue.Key);

                writer.WritePropertyName(keyValue.Key);
                serializer.Serialize(writer, keyValue.Value);
            }
            writer.WriteEndObject();
        }
    }
}
