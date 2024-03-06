using Mid_Project.ViewModels;
using System.Windows.Controls;

namespace Mid_Project.Views
{
    /// <summary>
    /// Interaction logic for reportUC.xaml
    /// </summary>
    public partial class reportUC : UserControl
    {
        public reportUC(Grid panel, Label lbl)
        {
            InitializeComponent();
            DataContext = new ReportViewModel(panel, lbl);
        }
    }
}
