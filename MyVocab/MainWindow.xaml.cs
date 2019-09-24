using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Resources;
using System.Drawing;
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
        public const string bgpath = @"./bg.png";
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

            BitmapImage bitmapImage = new BitmapImage();
            using(FileStream fs = new FileStream(bgpath, FileMode.Open))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = fs;
                bitmapImage.EndInit();
            }
            bitmapImage.Freeze();

            ImageBrush bg = new ImageBrush();
            bg.ImageSource = bitmapImage;
            bg.Stretch = Stretch.UniformToFill;
            MyVocabWindow.Background = bg;
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
            if(TextBoxWord.Text == "")
            {
                MessageBox.Show("Please input a word!", "No Word!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (TextBoxMean.Text == "")
            {
                MessageBox.Show("Please input its meaning!", "No Meaning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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

            MessageBox.Show("Word No." + wordIndex.ToString() + ' ' + word + " is Recorded.", "Done!", MessageBoxButton.OK, MessageBoxImage.Information);

            TextBoxWord.Text = "";
            TextBoxMean.Text = "";
            TextBoxNote.Text = "";
            LabelIndex.Content = (wordIndex + 1).ToString();

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

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            dlg.FilterIndex = 3;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string bgName = dlg.FileName;

                using (FileStream fs = new FileStream(bgName, FileMode.Open))
                {
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    byte[] bytes = ms.ToArray();

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(bytes);
                    image.EndInit();

                    ImageBrush bg = new ImageBrush();
                    bg.ImageSource = image;
                    bg.Stretch = Stretch.UniformToFill;
                    MyVocabWindow.Background = bg;

                    FileStream fsSave = new FileStream(bgpath, FileMode.OpenOrCreate);
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(fsSave);
                    fsSave.Close();
                }

            }
            else
            {
                MessageBox.Show("Please Choose a Picture File!", "Uh-Oh!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            
        }
    }
}
