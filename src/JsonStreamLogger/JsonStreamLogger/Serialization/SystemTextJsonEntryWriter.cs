using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JsonStreamLogger.Serialization
{
    public class SystemTextJsonEntryWriter : IEntryWriter
    {
        private readonly Stream _stream;
        private readonly Utf8JsonWriter _writer;
        private static readonly byte[] _newLine = new byte[] { 0x0a /* \n */ };

        public SystemTextJsonEntryWriter(Stream stream)
        {
            _stream = stream;
            _writer = new Utf8JsonWriter(stream, new JsonWriterOptions
            {
#if !DEBUG
                SkipValidation = true,
#endif
                Indented = false,
            });
        }

        public ValueTask WriteEntryAsync(in LogEntry entry, CancellationToken cancellationToken)
        {
            WriteLogEntry(_writer, entry);

            // Each JSON log entry is expected to be single-line.
            _writer.Flush();
            _stream.Write(_newLine, 0, 1);
            _writer.Reset();

            return new ValueTask(_stream.FlushAsync(cancellationToken));
        }

        private static void WriteLogEntry(Utf8JsonWriter writer, in LogEntry entry)
        {
            writer.WriteStartObject();
            {
                writer.WriteString("Category", entry.Category);
                writer.WriteNumber("LogLevel", (int)entry.LogLevel);
                writer.WriteNumber("EventId", entry.EventId.Id);
                writer.WritePropertyName("State");
                {
                    if (entry.State is IReadOnlyList<KeyValuePair<string, object>> keyValuePairs)
                    {
                        WriteState(writer, keyValuePairs);
                    }
                    else
                    {
                        WriteState(writer, entry.State);
                    }
                }
                writer.WritePropertyName("Exception");
                {
                    WriteException(writer, entry.Exception);
                }
                writer.WriteString("Message", entry.Message);
            }
            writer.WriteEndObject();
        }

        private static void WriteState(Utf8JsonWriter writer, object state)
        {
            if (state == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, state, state.GetType());
            }
        }

        private static void WriteState(Utf8JsonWriter writer, IReadOnlyList<KeyValuePair<string, object>> state)
        {
            if (state == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStartObject();
                {
                    foreach (var keyValue in state)
                    {
                        writer.WritePropertyName(keyValue.Key);
                        {
                            if (keyValue.Value == null)
                            {
                                writer.WriteNullValue();
                            }
                            else
                            {
                                JsonSerializer.Serialize(writer, keyValue.Value, keyValue.Value.GetType());
                            }
                        }
                    }
                }
                writer.WriteEndObject();
            }
        }

        private static void WriteException(Utf8JsonWriter writer, Exception ex)
        {
            if (ex == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStartObject();
                {
                    writer.WriteString("Name", ex.GetType().FullName);
                    writer.WriteString("Message", ex.Message);
                    writer.WriteString("StackTrace", ex.StackTrace);
                    writer.WritePropertyName("InnerException");
                    {
                        WriteException(writer, ex.InnerException);
                    }
                }
                writer.WriteEndObject();
            }
        }
    }
}
