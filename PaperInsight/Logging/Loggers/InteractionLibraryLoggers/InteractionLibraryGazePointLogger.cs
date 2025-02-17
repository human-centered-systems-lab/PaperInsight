using System;
using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.LoggingData;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.Loggers.InteractionLibraryLoggers
{
    internal class InteractionLibraryGazePointLogger : AbstractLeafLogger<GazePointLoggingData>
    {
        private readonly IInteractionLib _intlib;

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",
            "Validity",
            "X",
            "Y"
        }.ToCSVString();

        internal InteractionLibraryGazePointLogger(IInteractionLib intlib)
        {
            _intlib = intlib;
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);

            _intlib.GazePointDataEvent += evt =>
            {
                if (evt.timestamp_us == 0) return;
                LoggingData.Enqueue(new(
                    time: evt.timestamp_us,
                    Val: evt.validity,
                    xPoint: evt.x,
                    yPoint: evt.y
                ));
            };
        }
    }
}
