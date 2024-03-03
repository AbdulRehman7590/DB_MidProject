using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class ProjectViewModel
    {
        private readonly WrapPanel Panel;
        private readonly Label address;

        public ProjectViewModel(WrapPanel panel, Label address)
        {
            Panel = panel;
            this.address = address;
        }
    }
}
