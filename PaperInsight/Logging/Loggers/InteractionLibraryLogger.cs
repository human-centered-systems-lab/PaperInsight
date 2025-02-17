using System;
using PaperInsight.Logging.Loggers.InteractionLibraryLoggers;
using Tobii.InteractionLib;
using System.Timers;
using PaperInsight.Logging.Abstraction;

namespace PaperInsight.Logging.Loggers
{
    internal class InteractionLibraryLogger : AbstractCompositLogger
    {
        internal readonly float width;
        internal readonly float height;
        internal const float offset = 0.0f;

        internal const int SAMPLING_INTERVAL = 100;

        private readonly IInteractionLib _intlib;
        private readonly Timer _reapetSampling;

        internal InteractionLibraryLogger()
        {
            var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            width = screen.Width;
            height = screen.Height;
            //MessageBox.Show(width + " " + height);
            _intlib = InteractionLibFactory.CreateInteractionLib(FieldOfUse.Analytical);
            _intlib.CoordinateTransformAddOrUpdateDisplayArea(width, height);
            _intlib.CoordinateTransformSetOriginOffset(offset, offset);

            _reapetSampling = new Timer
            {
                AutoReset = true,
                Interval = SAMPLING_INTERVAL
            };
            _reapetSampling.Elapsed += Sample;

            _loggers.Add(new InteractionLibraryGazeOriginLogger(_intlib));
            _loggers.Add(new InteractionLibraryHeadPoseLogger(_intlib));
            _loggers.Add(new InteractionLibraryPresenceLogger(_intlib));
            _loggers.Add(new InteractionLibraryGazePointLogger(_intlib));
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);
            _reapetSampling.Start();
        }
        public override void Stop()
        {
            base.Stop();        
            _reapetSampling.Stop();
            _loggers.Clear();
            _intlib.Dispose();
        }

        private void Sample(Object? source, ElapsedEventArgs? e) => _intlib.WaitAndUpdate();
    }
}
