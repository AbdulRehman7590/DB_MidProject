﻿#pragma checksum "..\..\..\..\Views\Group\AssignProjectUC.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "75821DCFE019BF2CFC480C7EFB90AD0AED208AEC3BF954E7CC443C1739F66436"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Mid_Project.Views.Group;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Mid_Project.Views.Group {
    
    
    /// <summary>
    /// AssignProjectUC
    /// </summary>
    public partial class AssignProjectUC : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\Views\Group\AssignProjectUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTable;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Views\Group\AssignProjectUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgProjects;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Views\Group\AssignProjectUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbldata;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Views\Group\AssignProjectUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker txtDOB;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Views\Group\AssignProjectUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEnter;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Mid_Project;component/views/group/assignprojectuc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Group\AssignProjectUC.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblTable = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.dgProjects = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.lbldata = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.txtDOB = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            this.btnEnter = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
