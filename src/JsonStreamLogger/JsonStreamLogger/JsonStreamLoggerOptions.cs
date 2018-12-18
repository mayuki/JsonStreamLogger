using JsonStreamLogger.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonStreamLogger
{
    public class JsonStreamLoggerOptions
    {
        public Stream OutputStream { get; set; }

        public int BufferSize { get; set; } = 1024;

        public TimeSpan ShutdownDelay { get; set; } = TimeSpan.FromMilliseconds(150);

        public Func<Stream, IEntryWriter> WriterFactory { get; set; } = (stream) => new JsonNetEntryWriter(stream);
    }
}
