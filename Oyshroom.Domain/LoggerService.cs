
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyshroom.Domain
{
    public class LoggerService
    {
        private readonly Logger _logger;

        public LoggerService(string filePath)
        {
            _logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.File(filePath)
                 .CreateLogger();
        }

        public void Information(string message)
        {
            _logger.Information(message);
        }

        public void Error(Exception exception, params object[] values)
        {
            _logger.Error(exception.Message, values);
        }

        public void Error(string message, params object[] values)
        {
            _logger.Error(message, values);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception, "Error Encountered: ");
        }
    }
}
