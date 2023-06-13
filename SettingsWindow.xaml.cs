using System;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Editor
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : UiWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            FillFontComboBox(FontFamilyComboBox); // заполняем comboBox системными шрифтами

            this.DataContext = App.UiSettings;
        }

        public void FillFontComboBox(System.Windows.Controls.ComboBox comboBoxFonts)
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                comboBoxFonts.Items.Add(fontFamily.Source);
            }

            comboBoxFonts.SelectedItem = ((MainWindow)Application.Current.MainWindow).textEditor.FontFamily;
        }

        private void Button_Click(object sender, RoutedEventArgs e) // обрабатываем сохранение настроек
        {
            if (App.UiSettings.FontSize <= 6)
            {
                FontSizeTextBox.Text = "6";
            } 

            if (App.UiSettings.FontSize >= 48)
            {
                FontSizeTextBox.Text = "48";
            }

            App.UiSettings.FontSize = Convert.ToInt32(FontSizeTextBox.Text);
            ((MainWindow)Application.Current.MainWindow).textEditor.FontSize = App.UiSettings.FontSize;
            ((MainWindow)Application.Current.MainWindow).textEditor.FontFamily = new FontFamily(App.UiSettings.FontFamily);
            ((MainWindow)Application.Current.MainWindow).textEditor.WordWrap = App.UiSettings.Wrap;
            App.Config.Save();
        }
    }
}
