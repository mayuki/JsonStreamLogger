using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace JsonStreamLogger.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(options =>
            {
                //options.AddConsole();
                options.AddJsonStream();
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger("Test");
            logger.Log(LogLevel.Information, "[{Id}] is {Hello}", 12345, "Konnnichiwa");
            logger.Log(LogLevel.Warning, new EventId(987, "NanikaEvent"), "[{Id}] is {Hello}", 67890, "Nya-n");

            try
            {
                Foo();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, new EventId(654, "ExceptionThrown"), ex, "[{Id}] is {ExceptionType}: {ExceptionMessage}", 77777, ex.GetType().FullName, ex.Message);
            }

            serviceProvider.Dispose();

            Console.ReadLine();
        }

        static void Foo()
        {
            try
            {
                Bar();
            }
            catch (Exception ex)
            {
                throw new Exception("Yabai", ex);
            }
        }

        static void Bar()
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}
