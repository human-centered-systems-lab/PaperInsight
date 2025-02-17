using Microsoft.Web.WebView2.Core;
using PaperInsight.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaktionslogik für Final.xaml
    /// </summary>
    public partial class Final : Window
    {
        public Final()
        {
            InitializeComponent(); 
            webViewPay.EnsureCoreWebView2Async();
            webViewPay.Source = new Uri("https://iism-im-survey.iism.kit.edu/limesurvey/index.php/753814?lang=de");
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            // Handle the message received from the website
            string message = e.TryGetWebMessageAsString();
            if (!string.IsNullOrEmpty(message))
            {
                if (message.Contains("SurveyComplete"))
                    // Do something with the message from the website
                    // For example, display it or process it in your application
                    //MessageBox.Show(message, "Message from Website", MessageBoxButton.OK, MessageBoxImage.Information);
                    ButtonNext.IsEnabled = true;
            }
        }

        private void WebViewPay_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;

            webViewPay.CoreWebView2.Settings.HiddenPdfToolbarItems =
                CoreWebView2PdfToolbarItems.Bookmarks
                | CoreWebView2PdfToolbarItems.FitPage
                | CoreWebView2PdfToolbarItems.PageLayout
                | CoreWebView2PdfToolbarItems.PageSelector
                | CoreWebView2PdfToolbarItems.Print
                | CoreWebView2PdfToolbarItems.Rotate
                | CoreWebView2PdfToolbarItems.Save
                | CoreWebView2PdfToolbarItems.SaveAs
                | CoreWebView2PdfToolbarItems.Search
                | CoreWebView2PdfToolbarItems.ZoomIn
                | CoreWebView2PdfToolbarItems.ZoomOut;
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Please open the cabin door and wait until the experimenter will come to fetch you. Now click on the 'Nein' Button.", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                ((App)System.Windows.Application.Current).FlowManager.Return();
                Close();
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            OBSController.StopRecording();
            MessageBox.Show("The recording of eye tracking, mouse, and keyboard data has been stopped now. Please proceed with answering the questionnaire.", "Caution", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

        }
    }
}
