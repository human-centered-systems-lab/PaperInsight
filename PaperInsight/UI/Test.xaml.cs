using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using PaperInsight.UI.Editor;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
            Extensions.loadPaperTextHTMLAsync("https://dl.acm.org/doi/fullHtml/10.1145/3334480.3382938");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ////string content = Extensions.PaperText;
            ////string filePath = @"C:\Users\pasca\Downloads\test.txt";

            //try
            //{
            //    File.WriteAllText(filePath, content);
            //    Console.WriteLine("String has been written to the file.");
            //}
            //catch (IOException ex)
            //{
            //    Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            //}
            var mouseposition = Mouse.GetPosition(null);
            MessageBox.Show(mouseposition.X.ToString());
        }
    }
}
