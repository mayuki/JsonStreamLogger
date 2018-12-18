using JsonStreamLogger.Internal;
using JsonStreamLogger.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace JsonStreamLogger
{
    public class JsonStreamLoggerProvider : ILoggerProvider, IDisposable
    {
        private readonly JsonStreamLoggerBroker _broker;

        public JsonStreamLoggerProvider(JsonStreamLoggerOptions options)
        {
            var stream = options.OutputStream ?? Console.OpenStandardOutput();

            _broker = new JsonStreamLoggerBroker(stream, options.BufferSize, options.WriterFactory, options.ShutdownDelay);
            _broker.EnsureRunning();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new JsonStreamLogger(categoryName, _broker);
        }

        public void Dispose()
        {
            _broker.Dispose();
        }
    }
}
