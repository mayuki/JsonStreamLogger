using JsonStreamLogger.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonStreamLogger
{
    public class JsonStreamLogger : ILogger
    {
        private readonly JsonStreamLoggerBroker _broker;
        private readonly string _categoryName;

        internal JsonStreamLogger(string categoryName, JsonStreamLoggerBroker broker)
        {
            _categoryName = categoryName;
            _broker = broker;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            _broker.Post(new LogEntry(_categoryName, logLevel, eventId, state, exception, message));
        }
    }
}
