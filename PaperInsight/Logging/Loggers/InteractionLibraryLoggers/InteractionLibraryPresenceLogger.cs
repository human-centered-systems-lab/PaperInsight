using System;
using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.LoggingData;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.Loggers.InteractionLibraryLoggers
{
    internal class InteractionLibraryPresenceLogger : AbstractLeafLogger<PresenceLoggingData>
    {
        private readonly IInteractionLib _intlib;

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",
            "Presence"
        }.ToCSVString();

        internal InteractionLibraryPresenceLogger(IInteractionLib intlib)
        {
            _intlib = intlib;
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);

            _intlib.PresenceDataEvent += evt =>
            {
                if (evt.timestamp_us == 0) return;
                LoggingData.Enqueue(new(
                    timestamp_us: evt.timestamp_us,
                    presence: evt.presence
                ));
            };
        }
    }
}
