using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace JsonStreamLogger
{
    public readonly struct LogEntry
    {
        public string Category { get; }
        public LogLevel LogLevel { get; }
        public EventId EventId { get; }
        public object State { get; }
        public Exception Exception { get; }
        public string Message { get; }

        public LogEntry(string category, LogLevel logLevel, EventId eventId, object state, Exception exception, string message)
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
