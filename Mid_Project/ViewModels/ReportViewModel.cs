using Mid_Project.MVVM;
using Mid_Project.Views.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class ReportViewModel
    {
        private readonly Grid Panel;
        private readonly Label address;

        public ReportViewModel(Grid panel, Label address)
        {
            Panel = panel;
            this.address = address;
        }


        /// <summary>
        /// Relay Commands ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands report1 => new RelayCommands(execute => Report1());
        public RelayCommands report2 => new RelayCommands(execute => Report2());
        public RelayCommands report3 => new RelayCommands(execute => Report3());
        public RelayCommands report4 => new RelayCommands(execute => Report4());
        public RelayCommands report5 => new RelayCommands(execute => Report5());



        /// <summary>
        /// Report No. 1 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report1()
        {

        }


        /// <summary>
        /// Report No. 2 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report2()
        {
            
        }


        /// <summary>
        /// Report No. 3 /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report3()
        {
            
        }


        /// <summary>
        /// Report No. 4 ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report4()
        {
            
        }


        /// <summary>
        /// Report No. 5 ///////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report5()
        {
            
        }

    }
}
