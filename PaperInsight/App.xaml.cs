using PaperInsight.Logging;
using PaperInsight.Manager;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;
using Tobii.InteractionLib.Wpf;

namespace PaperInsight
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public InteractionLibWpfHost IntlibWpf { get; private set; }

        public readonly FlowManager FlowManager;

        private readonly LoggingManager _loggingManager;

        private readonly string PATH = Environment.CurrentDirectory + @"/Logs";
        private const string FILENAME = "PaperInsight";
        private const int LOGGING_INTERVAL = 10000;
        private const string FILE_ENDING = "csv";
        private string _userID = "ERROR"; //Default value will be overwritten

        private bool _initialized = false;

        public App()
        {
            IntlibWpf = new InteractionLibWpfHost();
            FlowManager = new FlowManager();
            _loggingManager = new LoggingManager(LOGGING_INTERVAL);
        }

        public void StartLogging()
        {
            if (!_initialized) return;

            Task.Run(() => 
            {
                try
                {
                    _loggingManager.Start(PATH, _userID, FILENAME, FILE_ENDING);
                } catch (Exception ex)
                {
                    Log.Error($"Exception: {ex.Message}, {ex.StackTrace}");
                }
            }
            );
        }
        public void StopLogging() => _loggingManager.Stop();

        public void Start(string userid)
        {
            if (_initialized) return;
            _initialized = true;
            _userID = userid;
            FlowManager.Start();
            Log.Information("Started");
            OBSController.userID =  userid;

        }

        public void ResetLib()
        {
            IntlibWpf.Dispose();
            IntlibWpf.InteractionLib.Dispose();
            IntlibWpf = new InteractionLibWpfHost();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            OBSController.StopRecording();
            Log.Information("Application closed");
            StopLogging();
            IntlibWpf?.Dispose();
            base.OnExit(e);
        }
    }
}
