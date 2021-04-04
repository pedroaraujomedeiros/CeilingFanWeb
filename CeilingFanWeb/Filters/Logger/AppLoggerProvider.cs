using System;
using Microsoft.Extensions.Logging;

namespace CeilingFanWeb.Filters.Logger
{
    public class AppLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> filter;
        private readonly string connectionString;

        public AppLoggerProvider(Func<string, LogLevel, bool> filter, string connectionString)
        {
            this.filter = filter;
            this.connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new AppLogger(categoryName, filter, connectionString);
        }

        public void Dispose()
        {

        }
    }
}
