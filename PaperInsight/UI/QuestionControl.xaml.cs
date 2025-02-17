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

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaktionslogik für QuestionControl.xaml
    /// </summary>
    public partial class QuestionControl : UserControl
    {
        internal readonly Dictionary<int, CheckBox> _checkBoxes;
        public int _ticked;
        internal bool answered;
        
        public string Question
        {
            get { return (string)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }
        public static readonly DependencyProperty QuestionProperty =
        DependencyProperty.Register("Question", typeof(string), typeof(QuestionControl), new PropertyMetadata(null));

        public string Answer1
        {
            get { return (string)GetValue(Answer1Property); }
            set { SetValue(Answer1Property, value); }
        }
        public static readonly DependencyProperty Answer1Property =
        DependencyProperty.Register("Answer1", typeof(string), typeof(QuestionControl), new PropertyMetadata(null));

        public string Answer2
        {
            get { return (string)GetValue(Answer2Property); }
            set { SetValue(Answer2Property, value); }
        }
        public static readonly DependencyProperty Answer2Property =
        DependencyProperty.Register("Answer2", typeof(string), typeof(QuestionControl), new PropertyMetadata(null));

        public string Answer3
        {
            get { return (string)GetValue(Answer3Property); }
            set { SetValue(Answer3Property, value); }
        }
        public static readonly DependencyProperty Answer3Property =
        DependencyProperty.Register("Answer3", typeof(string), typeof(QuestionControl), new PropertyMetadata(null));

        public string Answer4
        {
            get { return (string)GetValue(Answer4Property); }
            set { SetValue(Answer4Property, value); }
        }
        public static readonly DependencyProperty Answer4Property =
        DependencyProperty.Register("Answer4", typeof(string), typeof(QuestionControl), new PropertyMetadata(null));
        
        public int CorrectAnswer
        {
            get { return (int)GetValue(CorrectAnswerProperty); }
            set { SetValue(CorrectAnswerProperty, value); }
        }
        public static readonly DependencyProperty CorrectAnswerProperty =
        DependencyProperty.Register("CorrectAnswer", typeof(int), typeof(QuestionControl), new PropertyMetadata(null));



        public QuestionControl()
        {
            InitializeComponent();
            _checkBoxes = new Dictionary<int, CheckBox>()
            {
                { 1, CheckBox0 },
                { 2, CheckBox1 },
                { 3, CheckBox2 },
                { 4, CheckBox3 }
            };
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int ownID = _checkBoxes.First(c => c.Value.Equals((CheckBox)sender)).Key;
            UntickAllExceptSelf(ownID);
            _ticked = ownID;
            answered = true;
        }
        
        private void UntickAllExceptSelf(int id)
        {
            List<CheckBox> allExceptSelf = _checkBoxes.Where(c => c.Key != id).Select(c => c.Value).ToList();
            allExceptSelf.ForEach(checkBox => checkBox.IsChecked = false);
        }

        public int GetTicked()
        {
            return _ticked;
        }

    }
}
