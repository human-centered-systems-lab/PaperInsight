using System.Diagnostics;
using System.IO;

namespace PaperInsight.Tracking
{
    internal class Eyetracker
    {
        internal static void Calibrate()
        {
            var workingDirectory = Path.GetFullPath(@"C:\Program Files (x86)\Tobii\Tobii EyeX Config\");
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            using StreamReader reader = process.StandardOutput;
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    //sw.WriteLine(@"Tobii.EyeX.Configuration.exe -R");
                    sw.WriteLine(@"Tobii.EyeX.Configuration.exe -n");
                }
            }
            //the next line has to be there otherwise the program crashes
            string? output = process.StandardOutput.ReadToEnd();
        }

        internal static void CheckCalibration()
        {
            var workingDirectory = Path.GetFullPath(@"C:\Program Files (x86)\Tobii\Tobii EyeX Interaction\");
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            using StreamReader reader = process.StandardOutput;
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(@"Tobii.EyeX.Interaction.TestEyeTracking.exe");
                }
            }
            //the next line has to be there otherwise the program crashes
            string? output = process.StandardOutput.ReadToEnd();
        }

        internal static void ScreenSetup()
        {
            var workingDirectory = Path.GetFullPath(@"C:\Program Files (x86)\Tobii\Tobii EyeX Config\");
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            using StreamReader reader = process.StandardOutput;
            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(@"Tobii.EyeX.Configuration.exe -S");
                }
            }
            //the next line has to be there otherwise the program crashes
            string? output = process.StandardOutput.ReadToEnd();
        }
    }
}
