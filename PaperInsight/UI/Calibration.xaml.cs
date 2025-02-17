using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Tobii.InteractionLib.Wpf;
using PaperInsight.Tracking;
using PaperInsight.Manager;
using OBSWebsocketDotNet;
using System.Threading.Tasks;
using PaperInsight.Logging;

namespace PaperInsight
{
    /// <summary>
    /// Interaktionslogik für Calibration.xaml
    /// </summary>
    public partial class Calibration : Window
    {
        private Stopwatch timer = new Stopwatch();
        private bool timerStarted;
        private DispatcherTimer tickTimer = new DispatcherTimer();
        private TimeSpan ts = new TimeSpan();
        public OBSWebsocket _obs = new OBSWebsocket();

        public Calibration()
        {
            ((App)Application.Current).IntlibWpf?.WpfBinding?.AddWindow(this);           
            InitializeComponent();
            //Eyetracker.Calibrate();
            //Button_Next.IsEnabled = false;
            RecCalibration.Stroke = Brushes.Yellow;
            Behaviors.AddHasGazeChangedHandler(RecCalibration, Instruction_OnHasGazeChanged);

            //Timer for finding Walter
            timerStarted = false;
            tickTimer.Tick += new EventHandler(CheckCalibration);
            tickTimer.Interval = new TimeSpan(1000);
            tickTimer.Start();

            TextBlockCalibrationSuccess.Visibility = Visibility.Hidden;
            TextBlockCalibrationCheck.Visibility = Visibility.Hidden;
            TextBlockCalibrationInstruction.Visibility = Visibility.Hidden;
            RecCalibration.Visibility = Visibility.Hidden;
            RecCalibrationCenter.Visibility = Visibility.Hidden;
            TextBlock2.Visibility = Visibility.Hidden;

            //PC Institut
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "PyqOoFEdXfIBClPb");

            //Chai Cabins
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "8kvTxlL4lgIHOBS1"); //B10 (broken)
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "qPKxSrM9r4eM7aKa"); //B09
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "JqAADyvdu2g1cc9U"); //B01
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "DE51adGU1HFKKn5v"); //B07
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "nkdLM7EMQU7zvD5O"); //B16

            //Lotus Cabins
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "sn6rDYva608wktSO"); /*B08*/ (broken)
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "VoIH3qxm2f9TJ6vK"); //B05 (broken)
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "h6NEFiRQUyKGozO1"); //B02
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "4H1pYImqUzqRBQZn"); //B03
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "v483vfyBPYOVt5ml"); //B04 (broken)
            OBSController.ConnectOBSAsync("ws://localhost:4455", "DV1c0fyOSd9Z0gUU"); //B17

            //Broken Cabins

            //OBSController.ConnectOBSAsync("ws://localhost:4455", "4H1pYImqUzqRBQZn"); //B03
            //OBSController.ConnectOBSAsync("ws://localhost:4455", "S1ZQGskUAKZRIiw4"); //B06
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
                        //RecCalibration.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFE100"));
                    }
                }
                if (ts.TotalSeconds >= 2)
                {
                    timer.Stop();
                    timer.Reset();
                    RecCalibration.Stroke = Brushes.SeaGreen;
                    RecCalibration.Fill = Brushes.SeaGreen;
                    Button_Next.IsEnabled = true;
                    TextBlock_CalibrationSuccess.Text = "Eye Tracker successfully calibrated.";
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

        private void CalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            Eyetracker.Calibrate();
            OBSController.StopRecording();
            RecCalibration.Stroke = Brushes.Yellow;
            //Experiment
            //Button_Next.IsEnabled = false;
            TextBlock_CalibrationSuccess.Text = "";
            TextBlockCalibrationSuccess.Visibility = Visibility.Visible;
            TextBlockCalibrationCheck.Visibility = Visibility.Visible;
            TextBlockCalibrationInstruction.Visibility = Visibility.Visible;
            RecCalibration.Visibility = Visibility.Visible;
            RecCalibrationCenter.Visibility = Visibility.Visible;
            TextBlock2.Visibility = Visibility.Visible;
            CheckCalibrationButton.IsEnabled = true;
        }

        private void CheckCalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            Eyetracker.CheckCalibration();
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
                RecCalibrationCenter.Fill = Brushes.Black;
                if (timerStarted)
                {
                    timer.Stop();
                    timer.Reset();
                    timerStarted = false;
                    ts = timer.Elapsed;
                }
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Please be aware that from now on the eye tracking, mouse, keyboard, and screen data will be recorded. If you agree, please click okay.", "Caution", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result== MessageBoxResult.OK)
            {
                //OBSController.StartRecording();
                MessageBox.Show("The recording of " + OBSController.userID + " has started.", "Recording started", MessageBoxButton.OK, MessageBoxImage.Information);
                ((App)Application.Current).FlowManager.Return();
                Close();
            }

        }
        
    }
}
