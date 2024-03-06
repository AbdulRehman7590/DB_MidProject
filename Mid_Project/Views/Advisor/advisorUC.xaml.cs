using Mid_Project.ViewModels;
using System.Windows.Controls;

namespace Mid_Project.Views.Advisor
{
    /// <summary>
    /// Interaction logic for advisorUC.xaml
    /// </summary>
    public partial class advisorUC : UserControl
    {
        public advisorUC(Grid panel, Label lbl)
        {
            InitializeComponent();
            DataContext = new AdvisorViewModel(panel, lbl);
        }
    }
}
