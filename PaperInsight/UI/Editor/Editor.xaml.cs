using System;
using System.IO;
using System.Windows;
//using System.Windows.Forms;
using Serilog;

namespace PaperInsight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Editor : Window
    {
        private string task;
        private static string trialTaskDescription = "This is the trial round. Please familiarize yourself with the capabilities of the writing assistant and try out to write a summary covering the motivation, research question, method, results and contribution of the paper using the writing assistant. As this is the trail round, the summary will not be evaluated.";
        private static string roundTaskDescription = "Please explore, understand and write a summary of the paper using the writing assistant. The summary should be written in the summary text box.";
        private static string openAIDisclaimer = "The writing assistant uses the OpenAI API: OpenAI is a U.S. company, which means that data is transferred to the U.S. The EU Court of Justice does not consider the level of data protection in the U.S. to be adequate. Hereby we inform you that it is not necessary to send personal data to OpenAI via the writing assistant in order to solve the tasks, and you are asked not to enter any personal data about yourself and/or third parties during the experiment.";
        //For Tests as startup
        public Editor()
        {
            InitializeComponent();
            Viewer.NavigateTo(Environment.CurrentDirectory + @"\Resources\Nwagu_2023.pdf");
            TextEditor.EditorTextBox.AppendText("Task: Please try to understand the content of the paper to write a summary using the writing assistant. The summary should cover the research question, method, results and contribution of the paper.");
            WritingAssistant.PaperContent = File.ReadAllText(Environment.CurrentDirectory + @"\Resources\Nwagu_2023.txt");
            task = "Main";
            //Extensions.loadPaperTextHTMLAsync("https://dl.acm.org/doi/pdf/10.1145/3544549.3585627");
            
        }

        public Editor((string pdfPath, string contentPath, string task) input)
        {
            InitializeComponent();
            task = input.task;
            Viewer.NavigateTo(Environment.CurrentDirectory + input.pdfPath);
            TextEditor.EditorTextBox.AppendText(input.task);
            WritingAssistant.PaperContent = File.ReadAllText(Environment.CurrentDirectory + input.contentPath);
            if (task.Contains("Trial")) this.Title = "Trial";
            else this.Title = "Main Experiment";
            //Extensions.loadPaperTextHTMLAsync("https://dl.acm.org/doi/fullHtml/10.1145/3544549.3585627");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (task.Contains("Trial"))
            {
                MessageBox.Show(trialTaskDescription, "Trial Round", MessageBoxButton.OK, MessageBoxImage.Information);
                Serilog.Log.Information("MessageBoxTaskDescription_Trial");
            }
            else
            {
                MessageBox.Show(roundTaskDescription, "Your Task", MessageBoxButton.OK, MessageBoxImage.Information);
                Serilog.Log.Information("MessageBoxTaskDescription_Round1");
            }


        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to submit the summary?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            Serilog.Log.Information("Submit Summary Button clicked");
            if (result == MessageBoxResult.Yes)
            {
                Log.Information("Summary;" + TextEditor.Text());
                ((App)System.Windows.Application.Current).FlowManager.Return();
                Close();
            }
            
        }

        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //}
    }
}
