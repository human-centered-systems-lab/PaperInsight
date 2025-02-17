using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;
using System.Collections.Generic;
using System;
//using System.Windows.Forms;

namespace PaperInsight.UI.Editor
{
    /// <summary>
    /// Interaction logic for TextEditorControl.xaml
    /// </summary>
    public partial class TextEditorControl : UserControl
    {
        private readonly List<double> _fontSizes = new() { 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36 };

        public TextEditorControl()
        {
            InitializeComponent();
            //InitializeFontComboBox();
            //InitializeFontSizeComboBox();
        }
        #region
        //private void InitializeFontComboBox()
        //{
        //    foreach (var fontFamily in Fonts.SystemFontFamilies)
        //    {
        //        FontComboBox.Items.Add(fontFamily);
        //    }
        //    FontComboBox.SelectedItem = EditorTextBox.FontFamily;
        //}
        //private void InitializeFontSizeComboBox()
        //{
        //    FontSizeComboBox.ItemsSource = _fontSizes;
        //    FontSizeComboBox.SelectedItem = EditorTextBox.FontSize;
        //}
        #endregion

        #region UI Eventhandler
        //private void OnFontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (FontComboBox.SelectedItem is null) return;
        //    SetPropertyOnSelctedTextTo(TextElement.FontFamilyProperty, (FontFamily)FontComboBox.SelectedItem);
        //}
        //private void OnFontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (FontSizeComboBox.SelectedItem is null) return;
        //    SetPropertyOnSelctedTextTo(TextElement.FontSizeProperty, (double)FontSizeComboBox.SelectedItem);         
        //}
        //private void OnBoldButton_Click(object sender, RoutedEventArgs e) => SetPropertyOnSelctedTextTo(TextElement.FontWeightProperty, BoldButton.IsChecked == true ? FontWeights.Bold : FontWeights.Normal);
        //private void OnItalicButton_Click(object sender, RoutedEventArgs e) => SetPropertyOnSelctedTextTo(TextElement.FontStyleProperty, ItalicButton.IsChecked == true ? FontStyles.Italic : FontStyles.Normal);
        //private void OnUnderlineButton_Click(object sender, RoutedEventArgs e) => SetPropertyOnSelctedTextTo(Inline.TextDecorationsProperty, UnderlineButton.IsChecked == true ? TextDecorations.Underline : null);
        //#endregion


        #region File
        private void New_Click(object sender, RoutedEventArgs e)
        {
            EditorTextBox.Document.Blocks.Clear();

        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Implement file open logic here
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Implement file save logic here
        }
        #endregion

        #region Undo/Redo
        private void Undo_Click(object sender, RoutedEventArgs e) => EditorTextBox.Undo();
        private void Redo_Click(object sender, RoutedEventArgs e) => EditorTextBox.Redo();
        #endregion

        #region Controller
        private void SetPropertyOnSelctedTextTo(DependencyProperty formattingProperty, object? value)
        {
            var selection = EditorTextBox.Selection;
            if (selection.IsEmpty)
            {
                // No text is selected, set the future text
                EditorTextBox.Selection.ApplyPropertyValue(formattingProperty, value);
            }
            else
            {
                // Change the selected text
                var selectedRange = new TextRange(selection.Start, selection.End);
                selectedRange.ApplyPropertyValue(formattingProperty, value);
            }
        }
        //private void OnSelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    var selection = EditorTextBox.Selection;
        //    bool isEmpty = selection.IsEmpty;
        //    var checkProperties = new (ToggleButton, DependencyProperty, object)[]
        //    {
        //        (BoldButton, TextElement.FontWeightProperty, FontWeights.Bold),
        //        (ItalicButton, TextElement.FontStyleProperty, FontStyles.Italic),
        //        (UnderlineButton, Inline.TextDecorationsProperty, TextDecorations.Underline)
        //    };

        //    foreach (var (toggleButton, dependencyProperty, value) in checkProperties)
        //    {
        //        if (isEmpty)
        //        {
        //            //REMOVE THIS TO CHANGE ON CLICK BEHAVIOUR
        //            toggleButton.IsChecked = selection.GetPropertyValue(dependencyProperty) != DependencyProperty.UnsetValue
        //            && selection.GetPropertyValue(dependencyProperty).Equals(value);
        //        }
        //        else
        //        {
        //            var selectedRange = new TextRange(selection.Start, selection.End);
        //            toggleButton.IsChecked = selectedRange.GetPropertyValue(dependencyProperty) != DependencyProperty.UnsetValue
        //            && selectedRange.GetPropertyValue(dependencyProperty).Equals(value);
        //        }
        //    }
        //}
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // Update the formatting of the new text being added
            var lastChange = e.Changes.LastOrDefault();
            if (lastChange is null) return;

            var addedTextStartPosition = lastChange.Offset;
            var addedTextLength = lastChange.AddedLength;

            var addedTextRange = new TextRange(EditorTextBox.Document.ContentStart.GetPositionAtOffset(addedTextStartPosition),
                                                EditorTextBox.Document.ContentStart.GetPositionAtOffset(addedTextStartPosition + addedTextLength));
            string text = Text();
            int wordCount = GetWordCount(text);
            WordCountTextBlock.Text = $"Word Count: {wordCount}";
            //addedTextRange.ApplyPropertyValue(TextElement.FontWeightProperty, BoldButton.IsChecked == true ? FontWeights.Bold : FontWeights.Normal);
            //addedTextRange.ApplyPropertyValue(TextElement.FontStyleProperty, ItalicButton.IsChecked == true ? FontStyles.Italic : FontStyles.Normal);
            //addedTextRange.ApplyPropertyValue(Inline.TextDecorationsProperty, UnderlineButton.IsChecked == true ? TextDecorations.Underline : null);           
        }

        public void pasteText()
        {
            // Get the text from the clipboard
            string clipboardText = Clipboard.GetText();

            // Append the clipboard text to the existing text in the RichTextBox
            TextRange currentTextRange = new TextRange(EditorTextBox.Document.ContentEnd, EditorTextBox.Document.ContentEnd);
            currentTextRange.Text = clipboardText;
        }


        public string Text (){
            return new TextRange(EditorTextBox.Document.ContentStart, EditorTextBox.Document.ContentEnd).Text;
        }

        private int GetWordCount(string text)
        {
            // Remove leading and trailing white spaces
            text = text.Trim();

            // Split the text into words
            string[] words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Return the count of words
            return words.Length;
        }
        #endregion
        
        private void PasteFromClipboard_Click(object sender, RoutedEventArgs e)
        {
            Paragraph paragraph = new Paragraph();
            Run run = new Run(Clipboard.GetText());
            paragraph.Inlines.Add(run);
            EditorTextBox.Document.Blocks.Add(paragraph);

        }
    }
    #endregion
}