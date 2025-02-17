using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.Loggers.MouseLoggers;

namespace PaperInsight.Logging.Loggers
{
    internal class MouseLogger : AbstractCompositLogger
    {
        internal MouseLogger()
        {
            _loggers.Add(new MouseMovementLogger());
            _loggers.Add(new MouseButtonLogger());
        }
    }
}
