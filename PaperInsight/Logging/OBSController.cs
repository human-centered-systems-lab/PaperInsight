using OBSWebsocketDotNet;
//using System;
//using OBS.WebSocket.NET;
using System.Windows;
using System;

namespace PaperInsight.Logging
{
    internal class OBSController
    {
        //private static readonly ObsWebSocket obs = new ObsWebSocket();
        //private readonly OBSWebsocket _obs;


        private static OBSWebsocket _obs = new OBSWebsocket();
        public static string userID ="";

        public static async void ConnectOBSAsync(string address, string password)
        {

            try
            {
                _obs.ConnectAsync(address, password);
            }
            catch (AuthFailureException ex)
            {
                // Handle connection failure, e.g., show error message
                MessageBox.Show($"OBS connection failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        public static async void StartRecording()
        {
            //if (_obs.GetRecordStatus().IsRecording) _obs?.StopRecord();
            _obs?.StartRecord();
            Serilog.Log.Information("OBS Recording started");
        }

        public static async void StopRecording()
        {
            if (_obs.GetRecordStatus().IsRecording) _obs?.StopRecord();
            Serilog.Log.Information("OBS Recording stopped");
        }
    }
}
