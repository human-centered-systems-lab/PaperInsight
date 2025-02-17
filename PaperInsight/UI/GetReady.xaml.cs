using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Tobii.InteractionLib.Wpf;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaktionslogik für GetReady.xaml
    /// </summary>
    public partial class GetReady : Window
    {

        private readonly Stopwatch timer = new Stopwatch();
        private bool timerStarted;
        private readonly DispatcherTimer tickTimer = new DispatcherTimer();
        private TimeSpan ts = new TimeSpan();
        
        public GetReady()
        {
            ((App)Application.Current).IntlibWpf?.WpfBinding?.AddWindow(this);
            InitializeComponent();
            Behaviors.AddHasGazeChangedHandler(RecCalibration, Instruction_OnHasGazeChanged);

            timerStarted = false;
            tickTimer.Tick += new EventHandler(CheckCalibration);
            tickTimer.Interval = new TimeSpan(1000);
            tickTimer.Start();
        }

        private void CheckCalibration(object sender, EventArgs e)
        {
            if (timerStarted)
            {
                ts = timer.Elapsed;
                if (ts.TotalSeconds < 2)
                {
                    if (Button_Next.IsEnabled == false)
                    {
                        //RecCalibration.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFE100")); //FFFFE100
                    }
                }
                if (ts.TotalSeconds >= 2 && !Button_Next.IsEnabled)
                {
                    timer.Stop();
                    timer.Reset();
                    RecCalibration.Stroke = Brushes.SeaGreen;
                    RecCalibration.Fill = Brushes.SeaGreen;
                    Button_Next.IsEnabled = true;
                    Button_Next_Click(null, null);
                }
            }
            else
            {
                if (Button_Next.IsEnabled == false)
                {
                    RecCalibration.Fill = Brushes.White;
                }
            }
        }

        private void Instruction_OnHasGazeChanged(object sender, HasGazeChangedRoutedEventArgs e)
        {
            if (e.HasGaze)
            {
                RecCalibrationCenter.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFE100"));
                RecCalibrationCenter.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFE100"));
                if (timerStarted == false)
                {
                    timerStarted = true;
                    timer.Start();
                }

            }
            else if (!e.HasGaze)
            {
                RecCalibrationCenter.Stroke = Brushes.Black;
                if (timerStarted)
                {
                    timer.Stop();
                    timer.Reset();
                    timerStarted = false;
                    ts = timer.Elapsed;
                }
            }
        }
        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).FlowManager.Return();
            Close();

        }
    }
}
