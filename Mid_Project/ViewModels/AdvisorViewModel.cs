using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class AdvisorViewModel
    {
        private readonly WrapPanel Panel;
        private readonly Label address;

        public AdvisorViewModel(WrapPanel panel, Label address)
        {
            this.Panel = panel;
            this.address = address;
        }
    }
}
