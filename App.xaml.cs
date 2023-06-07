using System.IO;
using System.Windows;

namespace Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            Editor.MainWindow.SavedString = string.Empty;

            if (e.Args.Length > 0)
            {
                window.textEditor.Text = File.ReadAllText(e.Args[0].ToString());
                window.textEditor.SyntaxHighlighting =
                    ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinitionByExtension(
                        Path.GetExtension(e.Args[0])
                    );
                Editor.MainWindow.FileLocation = e.Args[0].ToString();
            }

            window.textEditor.TextArea.TextView.Margin = new Thickness(5, 0, 0, 0);
            window.Show();
        }
    }
}
