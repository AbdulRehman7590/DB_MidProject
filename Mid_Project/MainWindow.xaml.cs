using Mid_Project.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Mid_Project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            DataContext = new MainWindowViewModel(mainpanel, lblAddress);
        }
    }
}
