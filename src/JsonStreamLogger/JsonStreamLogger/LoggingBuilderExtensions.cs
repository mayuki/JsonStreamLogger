using JsonStreamLogger;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddJsonStream(this ILoggingBuilder builder, Action<JsonStreamLoggerOptions> configure = null)
        {
            var options = new JsonStreamLoggerOptions();
            configure?.Invoke(options);

            builder.Services.AddSingleton<ILoggerProvider, JsonStreamLoggerProvider>(_ => new JsonStreamLoggerProvider(options));

            return builder;
        }
    }
}
