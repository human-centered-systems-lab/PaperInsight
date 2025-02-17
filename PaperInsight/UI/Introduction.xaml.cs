using PaperInsight.Logging;
using System.Windows;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : Window
    {
        public Introduction()
        {
            InitializeComponent();
            Serilog.Log.Information(OBSController.userID);
            Serilog.Log.Information("Introduction");
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).FlowManager.Return();
            Close();

        }
    }
}
