using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.LoggingData
{
    internal class PresenceLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;

        public long DeviceTime;

        public Presence Presence;

        public PresenceLoggingData(long timestamp_us, Presence presence)
        {
            SystemTime = DateTime.Now;
            DeviceTime = timestamp_us;
            Presence = presence;
        }

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                DeviceTime.ToString(),
                Presence.ToString()
            };
            return content.ToCSVString();
        }
    }
}
