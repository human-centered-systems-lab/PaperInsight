using System;
using System.Diagnostics;
using System.Windows;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaktionslogik für StartUp.xaml
    /// </summary>
    public partial class StartUp : Window
    {
        public StartUp()
        {
            InitializeComponent();
            if (!IsOBSRunning()) MessageBox.Show("OBS is not running. Please start OBS before continuing.");

        }

        private bool IsOBSRunning()
        {
            Process[] processes = Process.GetProcessesByName("obs64"); // "obs64" for OBS Studio 64-bit, or "obs32" for OBS Studio 32-bit
            return processes.Length > 0;
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).FlowManager.Return();
            Close();
        }
    }
}
