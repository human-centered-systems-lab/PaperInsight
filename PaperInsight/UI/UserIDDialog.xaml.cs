using System;
using System.Threading.Tasks;
using System.Windows;

namespace PaperInsight.UI
{
    /// <summary>
    /// Interaktionslogik für UserIDDialog.xaml
    /// </summary>
    public partial class UserIDDialog : Window
    {        
        public UserIDDialog()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            string input = txtUserId.Text;

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please input valid id");
                return;
            }

            ((App)Application.Current).Start(input);
            this.Close();
        }
    }
}
