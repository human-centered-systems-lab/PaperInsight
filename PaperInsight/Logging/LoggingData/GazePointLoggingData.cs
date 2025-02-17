using System;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.LoggingData
{
    internal class GazePointLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;
        
        public long DeviceTime;

        public Validity Validity;

        public float X;

        public float Y;
        
        public GazePointLoggingData(long time, Validity Val, float xPoint, float yPoint)
        {
            SystemTime = DateTime.Now;
            DeviceTime = time;
            Validity = Val;
            X = xPoint;
            Y = yPoint;
        }
        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                DeviceTime.ToString(),
                Validity.ToString(),
                X.ToString(),
                Y.ToString()
            };
            return content.ToCSVString();
        }
    }
}
