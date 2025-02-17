using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.Loggers;
using Serilog;
using System;
using System.IO;
using System.Timers;

namespace PaperInsight.Logging
{
    internal class LoggingManager
    {
        internal bool IsRunning { get; private set; }

        private class Logger : AbstractCompositLogger
        {
            internal Logger()
            {
                _loggers.Add(new MouseLogger());
                _loggers.Add(new KeyBoardLogger());
                _loggers.Add(new InteractionLibraryLogger());
                _loggers.Add(new TobiiProSDKLogger());
            }
        }

        private readonly Logger _logger;
        private readonly Timer _repeatLogging;

        internal LoggingManager(int logging_interval)
        {
            _repeatLogging = new Timer
            {
                AutoReset = true,
                Interval = logging_interval
            };
            _repeatLogging.Elapsed += Log;
            
            _logger = new Logger();
        }


        internal void Start(string path, string userID, string filename, string fileEnding)
        {
            if (IsRunning) return;
            IsRunning = true;

            var starttime = DateTime.Now;
            var saveDirectory = CreateDirectory(path, filename, starttime, userID);

            _logger.Start(starttime, userID, saveDirectory, fileEnding);
            _repeatLogging.Start();

            App.Current.Dispatcher.Invoke(() =>
            {
                Serilog.Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(saveDirectory, $"SoftwareInterationLogs_{starttime:yyMMdd_HHmmss}_{userID}.txt"), rollingInterval: RollingInterval.Infinite)
                    .Enrich.FromLogContext()
                    .CreateLogger();

                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    Serilog.Log.Error($"{eventArgs.ExceptionObject}");
                };
            });
        }

        internal void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            _repeatLogging.Stop();
            _logger.Stop();
        }

        internal void Log() => _logger.Log();

        internal void Log(object? source, ElapsedEventArgs? e) => Log();

        private static string CreateDirectory(string path, string filename, DateTime currentTime, string userID)
        {
            string folderPath = Path.Combine(path, $"{filename}_{currentTime:yyMMdd_HHmmss}_{userID}");
            Directory.CreateDirectory(folderPath);
            return folderPath;
        } 
    }
}
