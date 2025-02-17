using System;
using Tobii.Research;
using PaperInsight.Logging.LoggingData;
using System.Windows;
using System.Linq;
using PaperInsight.Logging.Abstraction;

namespace PaperInsight.Logging.Loggers
{
    internal class TobiiProSDKLogger : AbstractLeafLogger<TobiiProSDKLoggingData>
    {
        private const int RESTART_THRESHOLD = 3;

        private IEyeTracker? _eyeTracker;

        private int _numberOfEmptyLogs = 0;

        #region AbstractLogger

        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "DeviceTime",

            "Pupil_Validity_Left", "PupilValidity_Right",
            "PupilSize_Left", "PupilSize_Right",

            "GazePoint_Validity_Left", "GazePoint_Validity_Right",
            "GazePoint_PositionOnDisplayArea_Left_X", "GazePoint_PositionOnDisplayArea_Left_Y",
            "GazePoint_PositionOnDisplayArea_Right_X", "GazePoint_PositionOnDisplayArea_Right_Y",
            "GazePoint_PositionInUserCoordinates_Left_X","GazePoint_PositionInUserCoordinates_Left_Y","GazePoint_PositionInUserCoordinates_Left_Z",
            "GazePoint_PositionInUserCoordinates_Right_X","GazePoint_PositionInUserCoordinates_Right_Y","GazePoint_PositionInUserCoordinates_Right_Z",

            "GazeOrigin_Validity_Left", "GazeOrigin_Validity_Right",
            "GazeOrigin_PositionInTrackBoxCoordinates_Left_X", "GazeOrigin_PositionInTrackBoxCoordinates_Left_Y","GazeOrigin_PositionInTrackBoxCoordinates_Left_Z",
            "GazeOrigin_PositionInTrackBoxCoordinates_Right_X","GazeOrigin_PositionInTrackBoxCoordinates_Right_Y","GazeOrigin_PositionInTrackBoxCoordinates_Right_Z",
            "GazeOrigin_PositionInUserCoordinates_Left_X","GazeOrigin_PositionInUserCoordinates_Left_Y", "GazeOrigin_PositionInUserCoordinates_Left_Z",
            "GazeOrigin_PositionInUserCoordinates_Right_X","GazeOrigin_PositionInUserCoordinates_Right_Y","GazeOrigin_PositionInUserCoordinates_Right_Z"
        }.ToCSVString();

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);
            StartUp();
        }

        public override void Log()
        {
            if (LoggingData.IsEmpty)
            {
                _numberOfEmptyLogs++;
                if (RESTART_THRESHOLD < _numberOfEmptyLogs) Restart();
                return;
            }
            _numberOfEmptyLogs = 0;
            base.Log();
        }

        public override void Stop()
        {
            Shutdown();
            base.Stop();
        }

        #endregion

        #region EyeTracker
        private void StartUp()
        {
            _numberOfEmptyLogs = 0;
            _eyeTracker = ConnectToEyeTracker();
            _eyeTracker.ConnectionLost += ConnectionLost;
            _eyeTracker.ConnectionRestored += ConnectionRestored;
            _eyeTracker.EventErrorOccurred += EventErrorOccurred;
            _eyeTracker.GazeDataReceived += GazeDataReceived;
        }

        private void Shutdown()
        {
            if (_eyeTracker == null) return;
            _eyeTracker.Dispose();
            //EyeTrackingOperations.Terminate();
        }

        private void Restart()
        {
            Serilog.Log.Warning("Restart TobiiPro");
            Shutdown();
            _eyeTracker = ConnectToEyeTracker();
            StartUp();
        }

        private static IEyeTracker ConnectToEyeTracker()
        {
            const int MAX_RETRIES = 5;
            int counter = 0;
            IEyeTracker? eyetracker;
            do
            {
                eyetracker = EyeTrackingOperations.FindAllEyeTrackers().First();
                if (eyetracker != null) return eyetracker;
                counter++;
            } while (counter < MAX_RETRIES);
            throw new ConnectionFailedException();
            //while (true)
            //{
            //    if (counter == MAX_RETRIES)
            //    {
            //        MessageBox.Show("Es wurde kein Eye Tracker gefunden! Bitte den Eye Tracker entfernen und erneut anschließen.\nFalls der Eye Tracker dann immer noch nicht erkannt wird, den Computer bitte neustarten.", "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
            //        throw new ConnectionFailedException();
            //    }

            //    try
            //    {
            //        eyetracker = EyeTrackingOperations.FindAllEyeTrackers().First();
            //    }
            //    catch (Exception)
            //    {
            //        counter++;
            //        continue;
            //    }
            //    break;
            //}
            //return eyetracker;
        }
         
        #endregion
        

        #region EventHandler

        private void GazeDataReceived(object? sender, GazeDataEventArgs e)
        {
            LoggingData.Enqueue(new
            (
                devicetime: DateTime.Now,
                time: e.DeviceTimeStamp,
                left: e.LeftEye,
                right: e.RightEye
            ));
        }

        private void EventErrorOccurred(object? sender, EventErrorEventArgs e)
        {
            MessageBox.Show("Ein Fehler ist aufgetreten. \n\nBitte überprüfen Sie, ob der Eye Tracker noch eingeschalten ist und die Datengröße der TobiiProSDK-Datei sich noch verändert.", "Fehler aufgetreten", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            Serilog.Log.Error("Source: " + e.Source.ToString() + "\nMessage:" + e.Message + " Time Stamp:" + e.SystemTimeStamp.ToString());
        }

        private void ConnectionRestored(object? sender, ConnectionRestoredEventArgs e)
        {
            MessageBox.Show("Die Verbindung zum Eye Tracker wurde kurzzeitig unterbrochen und wiederhergestellt. \n\nBitte überprüfen Sie, ob der Eye Tracker noch eingeschalten ist und die Datengröße der TobiiProSDK-Datei sich noch verändert.", "Fehler augetreten", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void ConnectionLost(object? sender, ConnectionLostEventArgs e)
        {
            MessageBox.Show("Die Verbindung zum Eye Tracker ist verloren gegangen und es werden aktuell keine Eye Tracking Daten aufgezeichnet. \n\nBitte schließen Sie die Anwendung und starten diese anschließend neu. Überprüfen Sie auch, ob der Eye Tracker noch eingeschalten ist und Daten aufgezeichnet werden.", "Fehler augetreten", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            Serilog.Log.Error("Eye Tracker Connection Lost: " + DateTime.Now.ToString());
        }

        #endregion


    }
}
