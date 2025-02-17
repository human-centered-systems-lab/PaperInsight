using System;

namespace PaperInsight.Logging.Abstraction
{
    internal interface ILogger
    {
        /// <summary>
        ///     Start collecting data and keep it in List of Serialisable Classes
        /// </summary>
        internal void Start(DateTime currentTime, string userID, string path, string fileEnding);
        /// <summary>
        ///     Stop collecting
        /// </summary>
        internal void Stop();
        /// <summary>
        ///     Write current List to file
        /// </summary>
        internal void Log();
    }
}
