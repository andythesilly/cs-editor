using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        public static string? SavedString { get; set; }
        public static string? FileLocation { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            textEditor.TextArea.TextView.Margin = new Thickness(5, 0, 0, 0);
        }

        private void setSyntax()
        {
            textEditor.SyntaxHighlighting =
                    ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(FileLocation));
        }

        private void NewCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            FileLocation = null;
            textEditor.Text = string.Empty;
            SavedString = string.Empty;
        }

        private async void OpenCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                FileLocation = dialog.FileName;
                setSyntax();
                textEditor.Text = await File.ReadAllTextAsync(dialog.FileName);
                SavedString = textEditor.Text;
            }
        }

        private void SaveCommandBinding_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            if (FileLocation != null)
            {
                e.CanExecute = true;
            }
        }

        private async void SaveCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            await File.WriteAllTextAsync(FileLocation, textEditor.Text);
            SavedString = textEditor.Text;
        }

        private async void SaveAsCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();

            if (dialog.ShowDialog() == true)
            {
                FileLocation = dialog.FileName;
                setSyntax();
                await File.WriteAllTextAsync(FileLocation, textEditor.Text);
                SavedString = textEditor.Text;
            }
        }

        private void ExitCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (textEditor.Text != SavedString)
            {
                MessageBox.Show("Вы не сохранились");
            }

            Application.Current.Shutdown();
        }
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Exit",
                "Exit",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        //Define more commands here, just like the one above
    }
}
