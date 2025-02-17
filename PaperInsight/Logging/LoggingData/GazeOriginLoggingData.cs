using System;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.LoggingData
{
    internal class GazeOriginLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;

        public long DeviceTime;

        public Validity LeftValidity;

        public float[] Left;

        public Validity RightValidity;

        public float[] Right;

        public GazeOriginLoggingData(long time, Validity leftVal, float[] leftOrigin, Validity rightVal, float[] rightOrigin)
        {
            SystemTime = DateTime.Now; 
            DeviceTime = time;
            LeftValidity = leftVal;
            this.Left = leftOrigin;
            RightValidity = rightVal;
            this.Right = rightOrigin;
        }

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                DeviceTime.ToString(),

                LeftValidity.ToString(),
                Left.ToCSVString(),

                RightValidity.ToString(),
                Right.ToCSVString(),
            };
            return content.ToCSVString();
        }
    }
}
