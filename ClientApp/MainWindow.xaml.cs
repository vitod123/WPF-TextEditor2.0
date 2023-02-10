using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text.pdf;
using iTextSharp.text;

using PrintDialog = System.Windows.Controls.PrintDialog;
using DataFormats = System.Windows.DataFormats;
using Paragraph = System.Windows.Documents.Paragraph;
using MessageBox = System.Windows.MessageBox;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string textSize;
        string path;
        List<int> size;
        SolidColorBrush colorBrushWhite;
        SolidColorBrush colorBrushBlack;
        public MainWindow()
        {
            InitializeComponent();
            colorBrushWhite = new SolidColorBrush();
            colorBrushWhite.Color = Colors.White;
            colorBrushBlack = new SolidColorBrush();
            colorBrushBlack.Color = Colors.Black;
            size = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            size_comboBox.ItemsSource = size;
            size_comboBox.SelectedValue = 11;
        }

        private void Bolt_Click(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
            {
                richTB.SelectAll();
            }
            if ((FontWeight)richTB.Selection.GetPropertyValue(Inline.FontWeightProperty) == FontWeights.Bold)
            {
                richTB.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                richTB.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
            {
                richTB.SelectAll();
            }
            if ((FontStyle)richTB.Selection.GetPropertyValue(Inline.FontStyleProperty) == FontStyles.Italic)
            {
                richTB.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
            else
            {
                richTB.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
            {
                richTB.SelectAll();
            }

            if (richTB.Selection.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Underline)
            {
                richTB.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
            else
            {
                richTB.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (System.Windows.Controls.TextBox)e.OriginalSource;
            textSize = tb.Text;
            if (textSize != "0" && textSize != "" && textSize != null)
            {
                if (richTB.Selection.Text == "")
                {
                    richTB.SelectAll();
                }
                richTB.Selection.ApplyPropertyValue(Inline.FontSizeProperty, textSize);
            }
        }
        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            if (print.ShowDialog() == true)
            {
                print.PrintDocument(((IDocumentPaginatorSource)richTB.Document).DocumentPaginator, "print");
            }
        }

        public void FirstOpen()
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Filter = "rich text file, text file, portable document format(*.rtf;*.txt;*.pdf)|*.rtf;*.txt;*.pdf|text file(*.txt)|*.txt|rich text file(*.rtf)|*.rtf|portable document format(*.pdf)|*.pdf";
            if (open.ShowDialog() == true)
            {
                string text = File.ReadAllText(open.FileName);
                richTB.Document.Blocks.Clear();
                richTB.Document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run(text)));
            }
            path = open.FileName;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Filter = "rich text file, text file, portable document format(*.rtf;*.txt;*.pdf)|*.rtf;*.txt;*.pdf|text file(*.txt)|*.txt|rich text file(*.rtf)|*.rtf|portable document format(*.pdf)|*.pdf";
            if (open.ShowDialog() == true)
            {
                string text = File.ReadAllText(open.FileName);
                richTB.Document.Blocks.Clear();
                richTB.Document.Blocks.Add(new System.Windows.Documents.Paragraph(new Run(text)));
            }
            path = open.FileName;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            richTB.Copy();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            richTB.Cut();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            richTB.Paste();
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            richTB.SelectAll();
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            FontDialog dlg = new FontDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textSize = ((int)dlg.Font.Size).ToString();
                richTB.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, new FontFamily(dlg.Font.Name));
                richTB.Selection.ApplyPropertyValue(Inline.FontWeightProperty, dlg.Font.Bold ? FontWeights.Bold : FontWeights.Regular);
                richTB.Selection.ApplyPropertyValue(Inline.FontStyleProperty, dlg.Font.Italic ? FontStyles.Italic : FontStyles.Normal);
            }
        }
        private void Color_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (richTB.Selection.Text != "")
                {
                    richTB.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B)));
                }
                else
                {
                    richTB.Foreground = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
            {
                richTB.SelectAll();
            }
            richTB.Selection.Text = "";
        }

        private void richTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = new TextRange(richTB.Document.ContentStart, richTB.Document.ContentEnd).Text;
            sm.Text = (str.Count(s => s != '\r' && s != '\n')).ToString();
            var tmp = str.Split(' ', '.', ',', '-', '\n', '\t', '\r');
            wr.Text = tmp.Count(s => s != "").ToString();
            tmp = str.Split('\n');
            ln.Text = (tmp.Length - 1).ToString();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(richTB.Document.ContentStart, richTB.Document.ContentEnd);
            Microsoft.Win32.SaveFileDialog save = new();
            save.Filter = "rich text file, text file, portable document format(*.rtf;*.txt;*.pdf)|*.rtf;*.txt;*.pdf|text file(*.txt)|*.txt|rich text file(*.rtf)|*.rtf|portable document format(*.pdf)|*.pdf";
            if (save.ShowDialog() == true)
            {
                switch (System.IO.Path.GetExtension(save.FileName))
                {
                    case ".txt":
                        using (var file = new FileStream(save.FileName, FileMode.OpenOrCreate))
                        {
                            range.Save(file, DataFormats.Text);
                        }
                        break;
                    case ".rtf":
                        using (var file = new FileStream(save.FileName, FileMode.OpenOrCreate))
                        {
                            range.Save(file, DataFormats.Rtf);
                        }
                        break;
                    case ".pdf":
                        Document doc = new Document();
                        PdfWriter.GetInstance(doc, new FileStream(save.FileName, FileMode.Create));
                        doc.Open();
                        doc.Add(new iTextSharp.text.Paragraph(range.Text));
                        doc.Close();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string text = new TextRange(richTB.Document.ContentStart, richTB.Document.ContentEnd).Text;           
            File.WriteAllText(path, text);
        } 
        private void rightTextAlign(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
                richTB.SelectAll();
            richTB.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }
        private void centerTextAlign(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
                richTB.SelectAll();
            richTB.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }
        private void leftTextAlign(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
                richTB.SelectAll();
            richTB.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }
        private void justifyTextAlign(object sender, RoutedEventArgs e)
        {
            if (richTB.Selection.Text == "")
                richTB.SelectAll();
             richTB.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Justify);
        }

        private void darkTheme(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"dark.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = App.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            size_comboBox.Foreground = colorBrushWhite;
        }
        private void lightTheme(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"light.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = App.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            size_comboBox.Foreground = colorBrushBlack;
        }
        private void greenTheme(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"green.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = App.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            size_comboBox.Foreground = colorBrushWhite;
        }
        private void blueTheme(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"blue.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = App.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            size_comboBox.Foreground = colorBrushBlack;
        } 
        private void redTheme(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"red.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = App.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            size_comboBox.Foreground = colorBrushWhite;
        }

        private void Toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"light.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = App.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            size_comboBox.Foreground = colorBrushBlack;
        }

        private void Toggle_Checked(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"dark.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = App.LoadComponent(uri) as ResourceDictionary;
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            size_comboBox.Foreground = colorBrushWhite;
        }

        private void aboutAs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developers:\nSofiia Stepaniuk\nVitalii Marchuk\nYevhenii Parkonny\n\n2022.11.07", "About as",MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}