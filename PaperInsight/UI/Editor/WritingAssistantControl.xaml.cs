using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;
//using System.Windows.Forms;
using System.Windows.Media;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using Serilog;


namespace PaperInsight.UI.Editor
{
    /// <summary>
    /// Interaktionslogik für WritingAssistantControl.xaml
    /// </summary>
    public partial class WritingAssistantControl : UserControl
    {
        public class WAMessage
        {
            public string question { get; set; }
            public string userInput { get; set; }
            public string answer { get; set; }

            public ObservableCollection<FollowUpQuestion> _followUpQuestionCollection { get; set; }
        }

        public class FollowUpQuestion
        {
            public string question { get; set; }
            public string userInput { get; set; }
            public string answer { get; set; }

        }

        private OpenAIAPI api;
        private static string openAIDisclaimer = "The writing assistant uses the OpenAI API. OpenAI is a U.S. company, which means that data is transferred to the U.S. The EU Court of Justice does not consider the level of data protection in the U.S. to be adequate. Hereby we inform you that it is not necessary to send personal data to OpenAI via the writing assistant in order to solve the tasks, and you are asked not to enter any personal data about yourself and/or third parties during the experiment.";

        public string PaperContent { get; set; }
        private bool firstClickTextBox = true;
        private bool _buttonIsSelected = false;
        private string PreviousMessageQuestion;
        private string PreviousMessageUserInput;
        private string PreviousMessageAnswer;
        private readonly SolidColorBrush _blueButton = new((Color)ColorConverter.ConvertFromString("#FF2F75D4"));
        public Model OpenAIModel = Model.ChatGPTTurbo_16k;

        private ObservableCollection<WAMessage> _chatMessages = new();
        public ObservableCollection<WAMessage> ChatMessages
        {
            get { return _chatMessages; }
            set
            {
                _chatMessages = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FollowUpQuestion> _followUpQuestions = new();
        
        public ObservableCollection<FollowUpQuestion> FollowUpQuestions
        {
            get { return _followUpQuestions; }
            set
            {
                _followUpQuestions = value;
                // Notify property change if you are using MVVM pattern
            }
        }
        
        public WritingAssistantControl()
        {
            InitializeComponent();
            api = new OpenAIAPI("API_KEY");
            DataContext = this;
        }

        public async Task CallOpenAIAPIAsync(string UserInput, string Question)
        {
            var chatMessagesTemp = new List<ChatMessage>();
            foreach (FollowUpQuestion question in FollowUpQuestions)
            {
                chatMessagesTemp.Add(new ChatMessage(ChatMessageRole.User, question.userInput));
                chatMessagesTemp.Add(new ChatMessage(ChatMessageRole.Assistant, question.answer));
            }
            chatMessagesTemp.Add(new ChatMessage(ChatMessageRole.User, UserInput));
            Log.Information("OpenAICall;" + ButtonSelected().ToString() + ";" + string.Join(Environment.NewLine, chatMessagesTemp.Select(message => $"Role: {message.Role}, Message: {message.Content}")));
            //string logMessage = "OpenAICall;" + ButtonSelected().ToString() + ";" + string.Join(Environment.NewLine, chatMessagesTemp.Select(message => $"Role: {message.Role}, Message: {message.Content}"));
            //logMessage = logMessage.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            //Log.Information(logMessage);

            if (PreviousAnswerBubble.Visibility == Visibility.Visible)
            {
                //MessageBox.Show("two");
                var chat = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
                {
                    
                    Model = OpenAIModel,
                    Temperature = 0.5,
                    //MaxTokens = 50,
                    Messages = chatMessagesTemp.ToArray()
                });
                string chatResponse = chat.Choices[0].Message.Content;
                Log.Information("OpenAIResponse " + chatResponse);
                ChatMessages.Add(new WAMessage { question = Question, userInput = UserInput, answer = chatResponse, _followUpQuestionCollection = FollowUpQuestions });

            }
            else
            {
                //MessageBox.Show("one");
                var chat = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
                {
                    Model = OpenAIModel,
                    Temperature = 0.5,
                    //MaxTokens = 50,
                    Messages = chatMessagesTemp.ToArray()
                });
                string chatResponse = chat.Choices[0].Message.Content;
                Log.Information("OpenAIResponse " + chatResponse);
                ChatMessages.Add(new WAMessage { question = Question, userInput = UserInput, answer = chatResponse, _followUpQuestionCollection = FollowUpQuestions });
            }
            PreviousAnswerBubble.Visibility = Visibility.Collapsed;
            LabelRequest.Visibility = Visibility.Hidden;
            ScrollViewerMessages.ScrollToEnd();
            TextBoxInput.Text = string.Empty;
            string selectedButton = ButtonSelected();
            SelectButton(selectedButton);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userInput = string.Format("{0}: {1}", LabelCommand.Content, TextBoxInput.Text);
            string question = userInput;
            string selectedButton = ButtonSelected();

            switch (selectedButton)
            {
                case "QuestionPaper":
                    userInput += PaperContent;
                    break;
                case "ExplainPaper":
                    userInput += PaperContent;
                    break;
                case "Summary":
                    userInput += PaperContent;
                    break;
                default:
                    break;
            }

            if (PreviousAnswerBubble.Visibility == Visibility.Collapsed)
            {
                PreviousMessageQuestion = "";
                PreviousMessageAnswer = "";
                FollowUpQuestions.Clear();
            }

            //Log.Information( "OpenAICall;" + selectedButton + ";" + question);
            _ = CallOpenAIAPIAsync(userInput, question);
            LabelRequest.Visibility = Visibility.Visible;
            //MessageBox.Show(userInput);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void SelectButton(string button)
        {
            Serilog.Log.Information(button);
            switch (button)
            {
                case "QuestionPaper":
                    ButtonQuestionPaper.BorderBrush = _blueButton;
                    ButtonQuestionPaper.Foreground = _blueButton;
                    ButtonExplainPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.Foreground = Brushes.Black;
                    ButtonSummaryPaper.BorderBrush = Brushes.Black;
                    ButtonSummaryPaper.Foreground = Brushes.Black;
                    ButtonTranslateText.BorderBrush = Brushes.Black;
                    ButtonTranslateText.Foreground = Brushes.Black;
                    ButtonStyle.BorderBrush = Brushes.Black;
                    ButtonStyle.Foreground = Brushes.Black;
                    ButtonImprove.BorderBrush = Brushes.Black;
                    ButtonImprove.Foreground = Brushes.Black;
                    ButtonQuestionGeneral.BorderBrush = Brushes.Black;
                    ButtonQuestionGeneral.Foreground = Brushes.Black;
                    LabelCommand.Content = "Answer the following question considering the paper";
                    TextBoxInput.Text = "[insert question]";
                    break;
                    
                case "ExplainPaper":
                    ButtonExplainPaper.BorderBrush = _blueButton;
                    ButtonExplainPaper.Foreground = _blueButton;
                    ButtonSummaryPaper.BorderBrush = Brushes.Black;
                    ButtonSummaryPaper.Foreground = Brushes.Black;
                    ButtonQuestionPaper.BorderBrush = Brushes.Black;
                    ButtonQuestionPaper.Foreground = Brushes.Black;
                    ButtonTranslateText.BorderBrush = Brushes.Black;
                    ButtonTranslateText.Foreground = Brushes.Black;
                    ButtonStyle.BorderBrush = Brushes.Black;
                    ButtonStyle.Foreground = Brushes.Black;
                    ButtonImprove.BorderBrush = Brushes.Black;
                    ButtonImprove.Foreground = Brushes.Black;
                    ButtonQuestionGeneral.BorderBrush = Brushes.Black;
                    ButtonQuestionGeneral.Foreground = Brushes.Black;
                    LabelCommand.Content = "Explain the following term considering the paper";
                    TextBoxInput.Text = "[insert term]";
                    break;
                    
                case "Summary":
                    ButtonSummaryPaper.Foreground = _blueButton;
                    ButtonSummaryPaper.BorderBrush = _blueButton;
                    ButtonExplainPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.Foreground = Brushes.Black;
                    ButtonQuestionPaper.BorderBrush = Brushes.Black;
                    ButtonQuestionPaper.Foreground = Brushes.Black;
                    ButtonTranslateText.BorderBrush = Brushes.Black;
                    ButtonTranslateText.Foreground = Brushes.Black;
                    ButtonStyle.BorderBrush = Brushes.Black;
                    ButtonStyle.Foreground = Brushes.Black;
                    ButtonImprove.BorderBrush = Brushes.Black;
                    ButtonImprove.Foreground = Brushes.Black;
                    ButtonQuestionGeneral.BorderBrush = Brushes.Black;
                    ButtonQuestionGeneral.Foreground = Brushes.Black;
                    LabelCommand.Content = "Summarize paper with the following summary length";
                    TextBoxInput.Text = "[short, long, or key sentence]";
                    break;
                case "QuestionGeneral":
                    ButtonSummaryPaper.Foreground = Brushes.Black;
                    ButtonSummaryPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.Foreground = Brushes.Black;
                    ButtonQuestionPaper.BorderBrush = Brushes.Black;
                    ButtonQuestionPaper.Foreground = Brushes.Black;
                    ButtonTranslateText.BorderBrush = Brushes.Black;
                    ButtonTranslateText.Foreground = Brushes.Black;
                    ButtonStyle.BorderBrush = Brushes.Black;
                    ButtonStyle.Foreground = Brushes.Black;
                    ButtonImprove.BorderBrush = Brushes.Black;
                    ButtonImprove.Foreground = Brushes.Black;
                    ButtonQuestionGeneral.BorderBrush = _blueButton;
                    ButtonQuestionGeneral.Foreground = _blueButton;
                    LabelCommand.Content = "Answer the following question";
                    TextBoxInput.Text = "[insert question]";
                    break;
                case "TranslateText":
                    ButtonSummaryPaper.Foreground = Brushes.Black;
                    ButtonSummaryPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.Foreground = Brushes.Black;
                    ButtonQuestionPaper.BorderBrush = Brushes.Black;
                    ButtonQuestionPaper.Foreground = Brushes.Black;
                    ButtonTranslateText.BorderBrush = _blueButton;
                    ButtonTranslateText.Foreground = _blueButton;
                    ButtonStyle.BorderBrush = Brushes.Black;
                    ButtonStyle.Foreground = Brushes.Black;
                    ButtonImprove.BorderBrush = Brushes.Black;
                    ButtonImprove.Foreground = Brushes.Black;
                    ButtonQuestionGeneral.BorderBrush = Brushes.Black;
                    ButtonQuestionGeneral.Foreground = Brushes.Black;
                    LabelCommand.Content = "Translate";
                    TextBoxInput.Text = "[insert text]\nTo [German, English, further language]";
                    break;
                    
                case "Style":
                    ButtonSummaryPaper.Foreground = Brushes.Black;
                    ButtonSummaryPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.Foreground = Brushes.Black;
                    ButtonQuestionPaper.BorderBrush = Brushes.Black;
                    ButtonQuestionPaper.Foreground = Brushes.Black;
                    ButtonTranslateText.BorderBrush = Brushes.Black;
                    ButtonTranslateText.Foreground = Brushes.Black;
                    ButtonStyle.BorderBrush = _blueButton;
                    ButtonStyle.Foreground = _blueButton;
                    ButtonImprove.BorderBrush = Brushes.Black;
                    ButtonImprove.Foreground = Brushes.Black;
                    ButtonQuestionGeneral.BorderBrush = Brushes.Black;
                    ButtonQuestionGeneral.Foreground = Brushes.Black;
                    LabelCommand.Content = "Change the following text";
                    TextBoxInput.Text = "[insert text]\nTo [insert style: scientific, colloqiual, formal]";
                    break;

                case "Improve":
                    ButtonSummaryPaper.Foreground = Brushes.Black;
                    ButtonSummaryPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.Foreground = Brushes.Black;
                    ButtonQuestionPaper.BorderBrush = Brushes.Black;
                    ButtonQuestionPaper.Foreground = Brushes.Black;
                    ButtonTranslateText.BorderBrush = Brushes.Black;
                    ButtonTranslateText.Foreground = Brushes.Black;
                    ButtonStyle.BorderBrush = Brushes.Black;
                    ButtonStyle.Foreground = Brushes.Black;
                    ButtonImprove.BorderBrush = _blueButton;
                    ButtonImprove.Foreground = _blueButton;
                    ButtonQuestionGeneral.BorderBrush = Brushes.Black;
                    ButtonQuestionGeneral.Foreground = Brushes.Black;
                    LabelCommand.Content = "Improve grammar & spelling of the following text";
                    TextBoxInput.Text = "[insert text]";
                    break;
            }

        }

        private void DeselectButton(string button)
        {
            switch (button)
            {
                case "QuestionPaper":
                    ButtonQuestionPaper.BorderBrush = Brushes.Black;
                    ButtonQuestionPaper.Foreground = Brushes.Black;
                    LabelCommand.Content = "Please enter command";
                    TextBoxInput.Text = "[insert command]";
                    break;

                case "ExplainPaper":
                    ButtonExplainPaper.BorderBrush = Brushes.Black;
                    ButtonExplainPaper.Foreground = Brushes.Black;
                    LabelCommand.Content = "Please enter command";
                    TextBoxInput.Text = "[insert command]";
                    break;

                case "Summary":
                    ButtonSummaryPaper.BorderBrush = Brushes.Black;
                    ButtonSummaryPaper.Foreground = Brushes.Black;
                    LabelCommand.Content = "Please enter command";
                    TextBoxInput.Text = "[insert command]";
                    break;
                case "QuestionGeneral":
                    ButtonQuestionGeneral.BorderBrush = Brushes.Black;
                    ButtonQuestionGeneral.Foreground = Brushes.Black;
                    LabelCommand.Content = "Please enter command";
                    TextBoxInput.Text = "[insert command]";
                    break;
                case "TranslateText":
                    ButtonTranslateText.BorderBrush = Brushes.Black;
                    ButtonTranslateText.Foreground = Brushes.Black;
                    LabelCommand.Content = "Please enter command";
                    TextBoxInput.Text = "[insert command]";
                    break;
                case "Improve":
                    ButtonImprove.BorderBrush = Brushes.Black;
                    ButtonImprove.Foreground = Brushes.Black;
                    LabelCommand.Content = "Please enter command";
                    TextBoxInput.Text = "[insert command]";
                    break;
                case "Style":
                    ButtonStyle.BorderBrush = Brushes.Black;
                    ButtonStyle.Foreground = Brushes.Black;
                    LabelCommand.Content = "Please enter command";
                    TextBoxInput.Text = "[insert command]";
                    break;

            }
        }

        private void OnOptionQuestionClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.BorderBrush == _blueButton)
                DeselectButton("QuestionPaper");
            else
                SelectButton("QuestionPaper");
        }
        
        private void OnOptionSummaryClicked(object sender, RoutedEventArgs e)
        {
            
            Button button = sender as Button;
            if (button.BorderBrush == _blueButton)
                DeselectButton("Summary");
            else
                SelectButton("Summary");
        }

        private void OnOptionExplainClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.BorderBrush == _blueButton)
                DeselectButton("ExplainPaper");
            else
                SelectButton("ExplainPaper");
        }

        private void OnOptionQuestionGeneralClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.BorderBrush == _blueButton)
                DeselectButton("QuestionGeneral");
            else
                SelectButton("QuestionGeneral");
        }
        private void OnOptionTranslateClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.BorderBrush == _blueButton)
                DeselectButton("TranslateText");
            else
                SelectButton("TranslateText");

        }

        private void OnOptionImproveClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.BorderBrush == _blueButton)
                DeselectButton("Improve");
            else
                SelectButton("Improve");
        }

        private void OnOptionStyleClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.BorderBrush == _blueButton)
                DeselectButton("Style");
            else
                SelectButton("Style");
        }
        
        private string ButtonSelected()
        {
            if (ButtonQuestionPaper.Foreground == _blueButton) return "QuestionPaper";
            if (ButtonExplainPaper.Foreground == _blueButton) return "ExplainPaper";
            if (ButtonSummaryPaper.Foreground == _blueButton) return "Summary";
            if (ButtonQuestionGeneral.Foreground == _blueButton) return "QuestionGeneral";
            if (ButtonTranslateText.Foreground == _blueButton) return "TranslateText";
            if (ButtonImprove.Foreground == _blueButton) return "Improve";
            if (ButtonStyle.Foreground == _blueButton) return "Style";       
            return string.Empty;
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var chatMessage = (WAMessage)button.DataContext;
            var answerText = chatMessage.answer;
            
            Clipboard.SetText(answerText);
            //TextEditorControl.pasteText();
            // Optionally, you can show a notification or perform any other action after copying.

            // Example: Show a message box with the copied text
            //MessageBox.Show("Answer copied to clipboard: " + answerText);
        }

        private string GetVisibleText(TextBlock textBlock, string Text)
        {
            // Calculate the maximum number of visible characters based on the width
            int maxVisibleCharacters = (int)((textBlock.ActualWidth / textBlock.FontSize)*2 - 4);

            // Get the text content
            //string fullText = textBlock.Text;

            // Limit the text to the maximum visible characters
            string visibleText = Text.Substring(0, Math.Min(maxVisibleCharacters, Text.Length)) + "...";

            return visibleText;
        }

        private void FollowUpButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected answer message
            WAMessage selectedMessage = (sender as Button)?.DataContext as WAMessage;

            if (selectedMessage != null)
            {
                // Show the previous answer speech bubble
                string textTextBox = ("Follow Up: " + selectedMessage.answer.ToString());
                PreviousAnswerText.Text = textTextBox;    
                //string textPreviousAnswerTextBox = GetVisibleText(PreviousAnswerText, textTextBox);
                //PreviousAnswerText.Text = textPreviousAnswerTextBox;
                PreviousAnswerBubble.Visibility = Visibility.Visible;
                PreviousMessageQuestion = selectedMessage.question;
                PreviousMessageUserInput = selectedMessage.userInput;
                PreviousMessageAnswer = selectedMessage.answer;
                FollowUpQuestions = selectedMessage._followUpQuestionCollection;

                FollowUpQuestions.Add(new FollowUpQuestion { question = selectedMessage.question, userInput = selectedMessage.userInput, answer = selectedMessage.answer });
                TextBoxInput.UpdateLayout();
            }
            DeselectButton(ButtonSelected());
        }

        private void ButtonPreviousAnswerClose_Click(object sender, RoutedEventArgs e)
        {
            PreviousAnswerBubble.Visibility = Visibility.Collapsed;
            TextBoxInput.UpdateLayout();
        }

        private void TextBoxInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (firstClickTextBox)
            {
                MessageBox.Show(openAIDisclaimer, "Important Disclaimer", MessageBoxButton.OK, MessageBoxImage.Information);
                firstClickTextBox = false;
            }
                
        }
        //private void OnOptionPaperClicked(object sender, RoutedEventArgs e)
        //{
        //    Button button = sender as Button;
        //    if (button.BorderBrush == _blueButton)
        //    {
        //        button.Foreground = Brushes.Black;
        //        button.BorderBrush = Brushes.Black;
        //    }
        //    else
        //    {
        //        button.Foreground = _blueButton;
        //        button.BorderBrush = _blueButton;
        //    }

        //}
    }
}
