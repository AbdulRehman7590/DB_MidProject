using Mid_Project.ViewModels;
using System.Windows.Controls;

namespace Mid_Project.Views
{
    /// <summary>
    /// Interaction logic for groupUC.xaml
    /// </summary>
    public partial class groupUC : UserControl
    {
        public groupUC(Grid panel, Label lbl)
        {
            InitializeComponent();
            DataContext = new GroupViewModel(panel, lbl);
        }
    }
}
