using System;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Navigation;
using Microsoft.Web.WebView2.Core;
//using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;


namespace PaperInsight.UI.Editor
{
    /// <summary>
    /// Interaction logic for DocumentViewer.xaml
    /// </summary>
    public partial class DocumentViewer : UserControl
    {
        public DocumentViewer()
        {
            InitializeComponent();
            webView2.CreationProperties = new CoreWebView2CreationProperties
            {
                BrowserExecutableFolder = Environment.CurrentDirectory + @"\Resources\WebView2\",
            };

        }

        public void NavigateTo(string path) => webView2.Source = new Uri(path);

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e) => webView2.ZoomFactor += 0.25f;

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e) => webView2.ZoomFactor -= 0.25f;

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            this.webView2.Focus();
            System.Windows.Forms.SendKeys.SendWait("{F3}");
        }

        private void WebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;

            webView2.CoreWebView2.Settings.HiddenPdfToolbarItems =
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
