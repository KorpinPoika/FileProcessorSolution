using System;
using System.Composition;
using FileProcessorTool.Interfaces;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace FileProcessor.App
{
    [Export(typeof(ILoggerFacade))]
    public class LoggerImpl: ILoggerFacade
    {
        private static readonly Logger _logger = LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }
    }
}