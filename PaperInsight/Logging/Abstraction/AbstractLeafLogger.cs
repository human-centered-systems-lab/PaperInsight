using System;
using System.IO;
using PaperInsight.Logging.LoggingData;
using System.Text;
using System.Collections.Concurrent;

namespace PaperInsight.Logging.Abstraction
{
    internal abstract class AbstractLeafLogger<LoggingDataType> : ILogger where LoggingDataType : AbstractLoggingData
    {
        #region PathInjection
        protected abstract string CSV_Header { get; }

        protected string? _filePath;

        protected void CreateFilePath(DateTime startTime, string userID, string path, string fileEnding)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();
            string fileName = $"{typeof(LoggingDataType).Name}_{startTime:yyMMdd_HHmmss}_{userID}.{fileEnding}";
            _filePath = Path.Combine(path, fileName);
            File.WriteAllText(_filePath, CSV_Header + Environment.NewLine);
        }

        #endregion

        #region Logging
        protected ConcurrentQueue<LoggingDataType> LoggingData = new();

        protected static object lockObj = new();

        public virtual void Log()
        {
            lock (lockObj)
            {
                int count = LoggingData.Count;
                if (_filePath == null || count == 0) return;

                StringBuilder sb = new();

                do
                {
                    LoggingData.TryDequeue(out LoggingDataType? data);
                    if (data == null) continue;
                    sb.Append(data.ToCSVString() + Environment.NewLine);
                    count--;
                } while (count > 0);

                while (true)
                {
                    try
                    {
                        File.AppendAllText(_filePath, sb.ToString());
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    break;
                }
            }
        }
        #endregion

        #region Start Stop
        public virtual void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {    
            CreateFilePath(currentTime, userID, path, fileEnding);
            Serilog.Log.Information($"{GetType().Name} startet");
        }

        public virtual void Stop()
        {
            Log();
        }
        #endregion
    }
}
