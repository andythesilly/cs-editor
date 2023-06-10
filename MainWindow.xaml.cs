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
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;

            // Attach mouse wheel CTRL-key zoom support
            this.textEditor.PreviewMouseWheel += new MouseWheelEventHandler(
                TextEditor_PreviewMouseWheel
            );
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

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // открытие файла
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                FileLocation = dialog.FileName;
                setSyntax();
                textEditor.Load(dialog.FileName);
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

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // сохранение текущего файла
        {
            if (FileLocation != null)
            {
                textEditor.Save(FileLocation);
                SavedString = textEditor.Text;
            }
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) // сохранить файл как
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();

            if (dialog.ShowDialog() == true)
            {
                FileLocation = dialog.FileName;
                setSyntax();
                textEditor.Save(FileLocation);
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

        private void UndoCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textEditor.CanUndo)
            {
                e.CanExecute = true;
            }
        }

        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Undo();
        }

        private void RedoCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textEditor.CanRedo)
            {
                e.CanExecute = true;
            }
        }

        private void RedoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Redo();
        }

        private void CutCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Cut();
        }

        private void CopyCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Copy();
        }

        private void PasteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textEditor.Paste();
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
        }

        private void TextEditor_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                int fontSize = App.UiSettings.FontSize + e.Delta / 50;
                if (fontSize <= 6)
                {
                    fontSize = 6;
                    textEditor.FontSize = 6;
                } else if (fontSize >= 48)
                {
                    fontSize = 48;
                    textEditor.FontSize = 48;
                }
                else
                {
                    textEditor.FontSize = App.UiSettings.FontSize;
                }
                App.UiSettings.FontSize = fontSize;
                e.Handled = true;
            }
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
