using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.LoggingData
{
    internal class HeadPoseLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;

        public long DeviceTime;

        public Validity PositionValidity;

        public float[] Position;

        public Validity[] RotationValidity;

        public float[] Rotation;

        public HeadPoseLoggingData(long timestamp_us, Validity position_validity, float[] position, Validity[] rotation_validity, float[] rotation)
        {
            SystemTime = DateTime.Now;
            DeviceTime = timestamp_us;
            PositionValidity = position_validity;
            Position = position;
            RotationValidity = rotation_validity;
            Rotation = rotation;
        }

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                DeviceTime.ToString(),

                PositionValidity.ToString(),
                Position.ToCSVString(),

                RotationValidity.ToCSVString(),
                Rotation.ToCSVString(),
            };
            return content.ToCSVString();
        }
    }
}
