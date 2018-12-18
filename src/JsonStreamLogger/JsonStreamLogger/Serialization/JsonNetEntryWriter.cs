using JsonStreamLogger.Serialization.JsonNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JsonStreamLogger.Serialization
{
    public class JsonNetEntryWriter : IEntryWriter
    {
        private readonly TextWriter _writer;
        private readonly Stream _stream;

        private static readonly JsonSerializer Serializer = new Newtonsoft.Json.JsonSerializer()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            ContractResolver = new JsonStreamLoggerContractResolver(),
        };

        public JsonNetEntryWriter(Stream stream)
        {
            _stream = stream;
            _writer = new StreamWriter(stream);
        }

        public async ValueTask WriteEntryAsync(LogEntry entry, CancellationToken cancellationToken)
        {
            try
            {
                Serializer.Serialize(_writer, entry);
                _writer.Write('\n');
            }
            catch (Exception ex)
            {
                _writer.Write('\n');
                Serializer.Serialize(_writer, new LogEntry(entry.Category, entry.LogLevel, entry.EventId, null, ex, entry.Message));
                _writer.Write('\n');
            }

            await _writer.FlushAsync();
            await _stream.FlushAsync(cancellationToken);
        }
    }
}
