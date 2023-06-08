using System.Configuration;
using System.IO;
using System.Windows;

namespace Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Configuration Config { get; set; }
        public static UISettings UiSettings { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Editor.MainWindow.SavedString = string.Empty;

            if (Config.Sections["UISettings"] is null)
            {
                Config.Sections.Add("UISettings", new UISettings());
                Config.Save();
            }

            UiSettings = (UISettings)Config.GetSection("UISettings");

            if (e.Args.Length > 0)
            {
                Editor.MainWindow.FileLocation = e.Args[0].ToString();
                window.textEditor.Text = File.ReadAllText(Editor.MainWindow.FileLocation);
                window.textEditor.SyntaxHighlighting =
                    ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinitionByExtension(
                        Path.GetExtension(Editor.MainWindow.FileLocation)
                    );
                Editor.MainWindow.SavedString = window.textEditor.Text;
            }

            window.textEditor.FontSize = UiSettings.FontSize;
            window.textEditor.FontFamily = new System.Windows.Media.FontFamily( UiSettings.FontFamily );
            window.textEditor.TextArea.TextView.Margin = new Thickness(5, 0, 0, 0);
            window.Show();
        }
    }
}
