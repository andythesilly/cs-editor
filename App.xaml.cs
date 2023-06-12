using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Media;

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
            MainWindow window = new MainWindow(); // создаем окно
            Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); // открываем конфиг

            /* устанавливаем первоначальные значения для редактора  */
            Editor.MainWindow.FileLocation = null;
            Editor.MainWindow.SavedString = string.Empty;

            if (Config.Sections["UISettings"] is null) // проверяем существуют ли уже сохраненные настройки
            {
                Config.Sections.Add("UISettings", new UISettings());
                Config.Save();
            }

            UiSettings = (UISettings)Config.GetSection("UISettings"); // получаем настройки

            if (e.Args.Length > 0) // настраиваем приложение при открытии файлов через "открыть с помощью"
            {
                Editor.MainWindow.FileLocation = e.Args[0].ToString();
                window.textEditor.SyntaxHighlighting =
                    ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinitionByExtension(
                        Path.GetExtension(Editor.MainWindow.FileLocation)
                    );
                window.textEditor.Load(Editor.MainWindow.FileLocation);
                Editor.MainWindow.SavedString = window.textEditor.Text;
            }

            /* настраиваем внешний вид редактора */
            window.textEditor.FontSize = UiSettings.FontSize;
            window.textEditor.FontFamily = new FontFamily(UiSettings.FontFamily);
            window.textEditor.WordWrap = UiSettings.Wrap;
            window.textEditor.TextArea.TextView.Margin = new Thickness(5, 0, 0, 0);

            window.Show(); // показываем окно
        }
    }
}
