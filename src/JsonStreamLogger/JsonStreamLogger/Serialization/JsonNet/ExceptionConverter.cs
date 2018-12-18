using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonStreamLogger.Serialization.JsonNet
{
    internal class ExceptionConverter : JsonConverter<Exception>
    {
        public static readonly ExceptionConverter Instance = new ExceptionConverter();

        public override Exception ReadJson(JsonReader reader, Type objectType, Exception existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Exception value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            {
                writer.WritePropertyName("Name");
                writer.WriteValue(value.GetType().FullName);
            }
            {
                writer.WritePropertyName("Message");
                writer.WriteValue(value.Message);
            }
            {
                writer.WritePropertyName("StackTrace");
                writer.WriteValue(value.StackTrace);
            }
            {
                writer.WritePropertyName("InnerException");
                serializer.Serialize(writer, value.InnerException);
            }
            writer.WriteEndObject();
        }
    }
}
