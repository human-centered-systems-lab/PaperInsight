using Microsoft.Web.WebView2.Core;
using PaperInsight.Logging;
using System;
using System.Windows;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaction logic for Questionaire.xaml
    /// </summary>
    public partial class Questionaire : Window
    {
        public Questionaire(string paper)
        {
            InitializeComponent();
            webViewQues.EnsureCoreWebView2Async();
            if (paper.Equals("Chai")) webViewQues.Source = new Uri("LINK");
            else webViewQues.Source = new Uri("LINK");
            ButtonNext.IsEnabled = false;
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

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).FlowManager.Return();
            Close();
        }

        private void WebViewQues_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;
            
            webViewQues.CoreWebView2.Settings.HiddenPdfToolbarItems =
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

        //private void WindowLoaded(object sender, RoutedEventArgs e)
        //{
        //    OBSController.StopRecording();
        //    MessageBox.Show("The recording of eye tracking, mouse, and keyboard data has been stopped now. Please proceed with answering the questionnaire.", "Caution", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

        //}
    }
}
