using System;
using Microsoft.Extensions.Logging;

namespace CeilingFanWeb.Filters.Logger
{
    public static class AppLoggerExtensions
    {
        public static ILoggerFactory AddContext(this ILoggerFactory factory, Func<string, LogLevel, bool> filter = null,
            string connectionString = null)
        {
            factory.AddProvider(new Logger.AppLoggerProvider(filter,connectionString));
            return factory;
        }

        public static ILoggerFactory AddContext(this ILoggerFactory factory, LogLevel minLevel, string connectionString)
        {
            return AddContext(factory, (_, loglevel) => loglevel >= minLevel, connectionString);
        }

    }
}
