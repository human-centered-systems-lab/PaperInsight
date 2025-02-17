using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.LoggingData;
using System;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.Loggers.InteractionLibraryLoggers
{
    internal class InteractionLibraryGazeOriginLogger : AbstractLeafLogger<GazeOriginLoggingData>
    {
        private readonly IInteractionLib _intlib;

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",
            "LeftValidity",
            "Left_x", "Left_y", "Left_z",
            "RightValidity",
            "Right_x", "Right_y", "Right_z"
        }.ToCSVString();

        internal InteractionLibraryGazeOriginLogger(IInteractionLib intlib)
        {
            _intlib = intlib;
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);

            _intlib.GazeOriginDataEvent += evt =>
            {
                if (evt.timestamp_us == 0) return;
                LoggingData.Enqueue(new(
                    time: evt.timestamp_us,
                    leftVal: evt.leftValidity,
                    leftOrigin: new float[3] { evt.left_x, evt.left_y, evt.left_z },
                    rightVal: evt.rightValidity,
                    rightOrigin: new float[3] { evt.right_x, evt.right_y, evt.right_z }
                    ));
            };
        }
    }
}
