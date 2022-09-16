using System.Windows;
using System.Windows.Controls;
using TestApp_DataGrid.ViewModels;

namespace TestApp_DataGrid.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.IsSelectionActive) { textBox.SelectionLength = textBox.Text.Length - textBox.SelectionStart; }
        }
    }
}
