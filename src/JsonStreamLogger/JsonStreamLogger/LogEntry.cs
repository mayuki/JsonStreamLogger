using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace JsonStreamLogger
{
    public sealed class LogEntry
    {
        public string Category { get; }
        public LogLevel LogLevel { get; }
        public EventId EventId { get; }
        public IReadOnlyList<KeyValuePair<string, object>> State { get; }
        public Exception Exception { get; }
        public string Message { get; }

        public LogEntry(string category, LogLevel logLevel, EventId eventId, IReadOnlyList<KeyValuePair<string, object>> state, Exception exception, string message)
        {
            Category = category;
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
            Message = message;
        }
    }
}
