using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaktionslogik für IntroductionAssistant.xaml
    /// </summary>
    public partial class IntroductionAssistant : Window
    {
        public IntroductionAssistant()
        {
            InitializeComponent();
            webViewIntroAssistant.CreationProperties = new CoreWebView2CreationProperties
            {
                BrowserExecutableFolder = Environment.CurrentDirectory + @"\Resources\WebView2\",
            };
            webViewIntroAssistant.EnsureCoreWebView2Async();
            webViewIntroAssistant.Source = new Uri(Environment.CurrentDirectory + @"\Resources\IntroductionWritingAssistant.pdf");
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e) => webViewIntroAssistant.ZoomFactor += .25f;

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e) => webViewIntroAssistant.ZoomFactor -= .25f;

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).FlowManager.Return();
            webViewIntroAssistant.Dispose();
            Close();

        }

        private void WebViewIntroAssistant_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;
            
            webViewIntroAssistant.CoreWebView2.Settings.HiddenPdfToolbarItems =
                    CoreWebView2PdfToolbarItems.FullScreen
                    | CoreWebView2PdfToolbarItems.MoreSettings
                    | CoreWebView2PdfToolbarItems.Bookmarks
                    | CoreWebView2PdfToolbarItems.FitPage
                    | CoreWebView2PdfToolbarItems.PageLayout
                    | CoreWebView2PdfToolbarItems.PageSelector
                    | CoreWebView2PdfToolbarItems.Print
                    | CoreWebView2PdfToolbarItems.Rotate
                    | CoreWebView2PdfToolbarItems.Save
                    | CoreWebView2PdfToolbarItems.SaveAs;
            
        }
    }
}
