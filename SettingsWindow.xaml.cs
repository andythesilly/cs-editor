using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Editor
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            FillFontComboBox(FontFamilyComboBox); // заполняем comboBox системными шрифтами

            this.DataContext = App.UiSettings;
        }

        /* обрабатываем ввод размера шрифта в textBox */
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) 
        {
            e.Handled = !IsValid(((System.Windows.Controls.TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 6 && i <= 48;
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
            ((MainWindow)App.Current.MainWindow).textEditor.FontSize = App.UiSettings.FontSize;
            ((MainWindow)App.Current.MainWindow).textEditor.FontFamily = new FontFamily(App.UiSettings.FontFamily);
            ((MainWindow)App.Current.MainWindow).textEditor.WordWrap = App.UiSettings.Wrap;
            App.Config.Save();
        }
    }
}
