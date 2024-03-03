using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class EvaluationViewModel
    {
        private readonly WrapPanel Panel;
        private readonly Label address;

        public EvaluationViewModel(WrapPanel panel, Label address)
        {
            Panel = panel;
            this.address = address;
        }
    }
}
