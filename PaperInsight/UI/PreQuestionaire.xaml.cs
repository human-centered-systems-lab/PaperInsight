using System;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaction logic for PreQuestionaire.xaml
    /// </summary>
    public partial class PreQuestionaire : Window
    {
        public PreQuestionaire()
        {
            InitializeComponent();
        }


        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            bool answersCorrect = CheckAnswers();
            string _answers = GetAnswers();
            Serilog.Log.Information("PreQuestionnaire_Answers;"+ _answers);
            if (!answersCorrect)
            {
                {
                    MessageBox.Show("You did not answer all questions correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ((App)Application.Current).FlowManager.Return();
                Close();
            }

        }

        private string GetAnswers()
        {
            return $"{Question1.GetTicked()};{Question2.GetTicked()};{Question3.GetTicked()};{Question4.GetTicked()};{Question5.GetTicked()}";
        }

        private bool CheckAnswers()
        {
            if (Question1.CorrectAnswer == Question1.GetTicked() 
                && Question2.CorrectAnswer == Question2.GetTicked() 
                && Question3.CorrectAnswer == Question3.GetTicked() 
                && Question4.CorrectAnswer == Question4.GetTicked() 
                && Question5.CorrectAnswer == Question5.GetTicked())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
