using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;

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
        }

        private void setSyntax()
        {
            textEditor.SyntaxHighlighting =
                ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinitionByExtension(
                    Path.GetExtension(FileLocation)
                );
        }

        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // создание нового файла
        {
            FileLocation = null;
            textEditor.Text = string.Empty;
            SavedString = string.Empty;
        }

        private async void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // открытие файла
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

        private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) 
        {
            if (FileLocation != null)
            {
                e.CanExecute = true;
            }
        }

        private async void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // сохранение текущего файла
        {
            if (FileLocation != null)
            {
                await File.WriteAllTextAsync(FileLocation, textEditor.Text);
                SavedString = textEditor.Text;
            }
        }

        private async void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // сохранить файл как
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

        private void ExitCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // обработка выхода из приложения
        {
            if (textEditor.Text != SavedString) 
            {
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Вы не сохранились!",
                    Content = new TextBlock { Text = "Выйти всё равно?", },
                    ResizeMode = ResizeMode.NoResize,
                    ShowInTaskbar = false,
                    ButtonLeftName = "Да, выйти",
                    ButtonRightName = "Нет"
                };

                messageBox.ButtonLeftClick += (s, e) =>
                {
                    App.Current.Shutdown();
                };
                messageBox.ButtonRightClick += (s, e) =>
                {
                    messageBox.Close();
                };

                messageBox.Show();
            }
            else
            {
                App.Current.Shutdown();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
        }
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit =
            new(
                "Exit Command",
                "Exit",
                typeof(CustomCommands),
                new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) }
            );
    }
}
