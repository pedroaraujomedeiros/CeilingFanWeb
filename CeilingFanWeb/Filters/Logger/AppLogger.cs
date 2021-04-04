using System;
using Microsoft.Extensions.Logging;

namespace CeilingFanWeb.Filters.Logger
{
    public class AppLogger : ILogger
    {
        private const int MESSAGE_MAX_LENGTH = 4000;
        private readonly string categoryName;
        private readonly Func<string, LogLevel, bool> filter;

        public AppLogger(string categoryName, Func<string, LogLevel, bool> filter, string connectionString)
        {
            this.categoryName = categoryName;
            this.filter = filter;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> format)
        {
            if (!IsEnabled(logLevel))
                return;

            if (format == null)
                throw new ArgumentNullException(nameof(format));

            var message = format(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (exception != null)
                message += $"\n{exception}";


            message = message.Length > MESSAGE_MAX_LENGTH ? message.Substring(0, MESSAGE_MAX_LENGTH) : message;
            var eventLog = new ApplicationLog
            {
                Application = "Ceiling Fan Api",
                Action = state.ToString(),
                Message = message,
                LogLevel = logLevel.ToString(),
                EventDate = DateTime.Now
            };
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var retorno = (filter == null || filter(categoryName, logLevel));
            return retorno;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
