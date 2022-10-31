using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();


        cmb_FontColor.SelectedValue = "Black";
        cmb_FontStyle.SelectedIndex = 0;
        cmb_FontSize.SelectedIndex = 0;

        Type colorType = typeof(System.Drawing.Color);
        PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
        foreach (PropertyInfo prop in propInfos)
        {
            cmb_FontColor.Items.Add(prop.Name);

        }

        foreach (FontFamily font in Fonts.SystemFontFamilies)
        {
            cmb_FontStyle.Items.Add(font.Source);
        }

        for (int i = 9; i <= 72; i++)
        {
            cmb_FontSize.Items.Add(i);
        }
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Filter = "Text Files (.txt)|*.txt";
        var result = openFileDialog1.ShowDialog();
        switch (result)
        {
            case true:
                StreamReader sr = new(openFileDialog1.FileName);
                rchtxtbox.Document.Blocks.Clear();
                rchtxtbox.Document.Blocks.Add(new Paragraph(new Run(sr.ReadToEnd())));
                break;
        }
        
    }


    private void Save_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "Text Files (.txt)|*.txt";
        saveFileDialog1.FilterIndex = 1;
        var result = saveFileDialog1.ShowDialog();
        switch (result)
        {
            case true:
                StreamWriter sw = new(saveFileDialog1.FileName);
                TextRange range;
                range = new TextRange(rchtxtbox.Document.ContentStart, rchtxtbox.Document.ContentEnd);
                sw.Write(range.Text);
                break;
        }
    }

    private void ComboboxChanged(object sender, SelectionChangedEventArgs e)
    {
        if(sender is ComboBox cmb)
        {
            if (cmb.Name == "cmb_FontSize")
            {
                rchtxtbox.Selection.ApplyPropertyValue(FontSizeProperty,double.Parse(cmb_FontSize.SelectedValue.ToString()));
            }
            else if (cmb.Name == "cmb_FontColor")
            {
                rchtxtbox.Selection.ApplyPropertyValue(ForegroundProperty, cmb_FontColor.SelectedValue.ToString());
            }
            else if(cmb.Name == "cmb_FontStyle")
            {
                rchtxtbox.Selection.ApplyPropertyValue(FontFamilyProperty, cmb_FontStyle.SelectedValue.ToString());
            }
        }
       

    }

    private void AlignClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {

            var T = btn.Name switch
            {
                "LeftAlign" => rchtxtbox.Document.TextAlignment = TextAlignment.Left,
                "CenterAlign" => rchtxtbox.Document.TextAlignment = TextAlignment.Center,
                "RightAlign" => rchtxtbox.Document.TextAlignment = TextAlignment.Right
            };
            
        }
    }

    private void FontStyle_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn) {
            if (btn.Name == "btn_Bold")
            {
                if (rchtxtbox.Selection.Text.Length > 0)
                {

                    if (rchtxtbox.FontWeight == FontWeights.Bold)
                        rchtxtbox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
                    else
                        rchtxtbox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
                }
            }
            else if (btn.Name == "btn_Italic")
            {

                if (rchtxtbox.FontStyle == FontStyles.Italic)
                    rchtxtbox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
                                                                  
                else                                              
                    rchtxtbox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);


            }
            else if (btn.Name == "btn_Underline")
            {
                rchtxtbox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            rchtxtbox.Focus();
        }
    }
}
