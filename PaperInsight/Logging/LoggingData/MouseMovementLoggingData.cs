using System;

namespace PaperInsight.Logging.LoggingData
{
    internal class MouseMovementLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;
        public double X;
        public double Y;

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                X.ToString(),
                Y.ToString()
            };
            return content.ToCSVString();
        }
    }
}
