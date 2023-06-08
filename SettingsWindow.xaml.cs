using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Configuration;

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
            FillFontComboBox(FontFamilyComboBox);

            this.DataContext = App.UiSettings;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((System.Windows.Controls.TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 48;
        }

        public void FillFontComboBox(System.Windows.Controls.ComboBox comboBoxFonts)
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                comboBoxFonts.Items.Add(fontFamily.Source);
            }

            comboBoxFonts.SelectedItem = ((MainWindow)Application.Current.MainWindow).textEditor.FontFamily;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).textEditor.FontSize = App.UiSettings.FontSize;
            ((MainWindow)App.Current.MainWindow).textEditor.FontFamily = new FontFamily(App.UiSettings.FontFamily);
            App.Config.Save();
        }
    }
}
