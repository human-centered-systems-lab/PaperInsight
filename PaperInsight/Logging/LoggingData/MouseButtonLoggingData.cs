using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PaperInsight.Logging.LoggingData
{
    internal class MouseButtonLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;

        public string Button;

        public double X;

        public double Y;

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                Button,
                X.ToString(),
                Y.ToString(),
            };
            return content.ToCSVString();
        }
    }
}
