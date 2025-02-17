using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.LoggingData;
using System;
using Tobii.InteractionLib;

namespace PaperInsight.Logging.Loggers.InteractionLibraryLoggers
{
    internal class InteractionLibraryHeadPoseLogger : AbstractLeafLogger<HeadPoseLoggingData>
    {
        private readonly IInteractionLib _intlib;

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",
            "PositionValidity",
            "Position_x", "Position_y","Position_z",
            "RotationValidity_x", "RotationValidity_y", "RotationValidity_z",
            "Rotation_x","Rotation_y","Rotation_z"
        }.ToCSVString();

        internal InteractionLibraryHeadPoseLogger(IInteractionLib intlib)
        {
            _intlib = intlib;
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);

            _intlib.HeadPoseDataEvent += evt =>
            {
                if (evt.timestamp_us == 0) return;
                LoggingData.Enqueue(new(
                    timestamp_us: evt.timestamp_us,
                    position_validity: evt.position_validity,
                    position: new float[3] { evt.position_x, evt.position_y, evt.position_z },
                    rotation_validity: new Validity[3] { evt.rotation_validity_x, evt.rotation_validity_y, evt.rotation_validity_z },
                    rotation: new float[3] { evt.rotation_x, evt.rotation_y, evt.rotation_z }
                ));
            };
        }
    }
}
