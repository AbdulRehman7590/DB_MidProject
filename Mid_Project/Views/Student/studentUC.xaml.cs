using Mid_Project.ViewModels;
using System.Windows.Controls;

namespace Mid_Project.Views
{
    /// <summary>
    /// Interaction logic for studentUC.xaml
    /// </summary>
    public partial class studentUC : UserControl
    {
        public studentUC(Grid panel, Label lbl)
        {
            InitializeComponent();
            DataContext = new StudentViewModel(panel, lbl);
        }
    }
}
