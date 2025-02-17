using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaction logic for PostQuestionaire.xaml
    /// </summary>
    public partial class PostQuestionaire : Window
    {
        public PostQuestionaire()
        {
            InitializeComponent();
            webViewPostQues.EnsureCoreWebView2Async();
            webViewPostQues.Source = new Uri("LINK");
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

        private void WebViewQues_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;

            webViewPostQues.CoreWebView2.Settings.HiddenPdfToolbarItems =
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
            ((App)Application.Current).FlowManager.Return();
            Close();
        }
    }

}
