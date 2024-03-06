using Mid_Project.ViewModels;
using System.Windows.Controls;

namespace Mid_Project.Views
{
    /// <summary>
    /// Interaction logic for evaluationUC.xaml
    /// </summary>
    public partial class evaluationUC : UserControl
    {
        public evaluationUC(Grid panel, Label lbl)
        {
            InitializeComponent();
            DataContext = new EvaluationViewModel(panel, lbl);
        }
    }
}
