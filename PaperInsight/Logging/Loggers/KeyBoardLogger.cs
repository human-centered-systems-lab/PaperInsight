using System;
using System.Windows.Input;
using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.LoggingData;

namespace PaperInsight.Logging.Loggers
{
    internal class KeyBoardLogger : AbstractLeafLogger<KeyBoardLoggingData>
    {
        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "Key",
        }.ToCSVString();

        private bool _started = false;

        internal KeyBoardLogger() 
        {
            InputManager.Current.PreProcessInput += KeyInputHandler;
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);
            _started = true;
        }

        public override void Stop()
        {
            base.Stop();
            _started = false;
        }

        private KeyBoardLoggingData? _last;

        private void KeyInputHandler(object sender, PreProcessInputEventArgs e)
        {
            if (!_started) return;
            if (e.StagingItem.Input is KeyEventArgs keyArgs && keyArgs.IsDown)
            {
                KeyBoardLoggingData current = new()
                {
                    SystemTime = DateTime.Now,
                    Key = keyArgs.Key,
                };
                if (current.Equals(_last)) return;
                _last = current;
                LoggingData.Enqueue(current);
            }
        }
    }
}
