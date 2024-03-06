﻿using Mid_Project.ViewModels;
using System.Windows.Controls;

namespace Mid_Project.Views
{
    /// <summary>
    /// Interaction logic for projectUC.xaml
    /// </summary>
    public partial class projectUC : UserControl
    {
        public projectUC(Grid panel, Label lbl)
        {
            InitializeComponent();
            DataContext = new ProjectViewModel(panel, lbl);
        }
    }
}
