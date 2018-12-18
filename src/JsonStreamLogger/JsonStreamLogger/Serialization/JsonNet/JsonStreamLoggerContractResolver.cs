using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonStreamLogger.Serialization.JsonNet
{
    public class JsonStreamLoggerContractResolver : DefaultContractResolver
    {
        public JsonStreamLoggerContractResolver()
        {
            NamingStrategy = new DefaultNamingStrategy();
        }

        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(Exception).IsAssignableFrom(objectType))
            {
                return ExceptionConverter.Instance;
            }
            if (typeof(IReadOnlyList<KeyValuePair<string, object>>).IsAssignableFrom(objectType))
            {
                return ReadOnlyListKeyValuePairStringObjectConverter.Instance;
            }

            return base.ResolveContractConverter(objectType);
        }
    }
}
