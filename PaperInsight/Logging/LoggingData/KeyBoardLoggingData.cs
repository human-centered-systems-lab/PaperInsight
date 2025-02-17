using System;
using System.Windows.Input;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;


namespace PaperInsight.Logging.LoggingData
{
    internal class KeyBoardLoggingData : AbstractLoggingData
    {
        public DateTime SystemTime;

        public Key Key;

        internal override string ToCSVString()
        {
            string[] content = new string[]
            {
                $"{SystemTime:dd/MM/yyyy HH:mm:ss}",
                Key.ToString(),
            };
            return content.ToCSVString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            KeyBoardLoggingData other = (KeyBoardLoggingData)obj;
            return SystemTime.Equals(other.SystemTime) && Key == other.Key;
        }

        public override int GetHashCode() => HashCode.Combine(SystemTime, Key);
    }
}
