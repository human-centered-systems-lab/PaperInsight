using System;
using System.Collections.Generic;

namespace PaperInsight.Logging.Abstraction
{
    internal abstract class AbstractCompositLogger : ILogger
    {
        protected readonly List<ILogger> _loggers = new();

        public virtual void Start(DateTime currentTime, string userID, string path, string fileEnding) => _loggers.ForEach(logger => logger.Start(currentTime, userID, path, fileEnding));

        public virtual void Stop() => _loggers.ForEach(logger => logger.Stop());

        public void Log() => _loggers.ForEach(logger => logger.Log());
    }
}
