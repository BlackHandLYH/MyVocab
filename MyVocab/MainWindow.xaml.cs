using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyVocab
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public string filepath = "D:/MyVocab/vocab.txt";
        public int wordIndex = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Finish();
        }

        private void MyVocabWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string strIndex = File.ReadAllLines(filepath).Last();

            wordIndex = Convert.ToInt32(strIndex);

            LabelIndex.Content = (wordIndex + 1).ToString();
        }

        private void TextBoxWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                TextBoxWord.Text += Environment.NewLine;
                TextBoxWord.SelectionStart = TextBoxWord.Text.Length;
                TextBoxWord.ScrollToEnd();
            }
            else if(e.Key == Key.Enter)
            {
                TextBoxMean.Focus();
            }
        }

        private void TextBoxMean_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                TextBoxMean.Text += Environment.NewLine;
                TextBoxMean.SelectionStart = TextBoxMean.Text.Length;
                TextBoxMean.ScrollToEnd();
            }
            else if (e.Key == Key.Enter)
            {
                TextBoxNote.Focus();
            }
        }

        private void TextBoxNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                TextBoxNote.Text += Environment.NewLine;
                TextBoxNote.SelectionStart = TextBoxNote.Text.Length;
                TextBoxNote.ScrollToEnd();
            }
            else if (e.Key == Key.Enter)
            {
                Finish();
            }
        }

        public void Finish()
        {
            FileStream fs = new FileStream(filepath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

            string word = TextBoxWord.Text;
            string mean = TextBoxMean.Text.Replace(Environment.NewLine, Environment.NewLine + '\t');
            string note = TextBoxNote.Text.Replace(Environment.NewLine, Environment.NewLine + '\t'); 

            wordIndex += 1;

            sw.Write("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-\n");
            sw.Write("Word\n");
            sw.Write('\t' + word + '\n');
            sw.Write("Meaning\n");
            sw.Write('\t' + mean + '\n');
            sw.Write("Note\n");
            sw.Write('\t' + note + '\n');
            sw.Write("Index\n");
            sw.Write('\t' + wordIndex.ToString() + '\n');

            sw.Flush();
            sw.Close();
            fs.Close();

            MessageBox.Show("Word No." + wordIndex.ToString() + ' ' + word + " is Recorded.");

            TextBoxWord.Text = "";
            TextBoxMean.Text = "";
            TextBoxNote.Text = "";

            TextBoxWord.Focus();
        }

        private void LabelExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MyVocabWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
