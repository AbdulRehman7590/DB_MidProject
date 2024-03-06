using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Group;
using Mid_Project.Views.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class GroupViewModel
    {
        private readonly Grid Panel;
        private readonly Label address;

        public GroupViewModel(Grid panel, Label address)
        {
            Panel = panel;
            this.address = address;
        }


        /// <summary>
        /// Relay Commands ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands makeGroup => new RelayCommands(execute => MakeGroup());
        public RelayCommands viewGroup => new RelayCommands(execute => ViewGroup());
        public RelayCommands addGroupStudent => new RelayCommands(execute => AddGroupStudent());
        public RelayCommands viewGroupStudents => new RelayCommands(execute => ViewGroupStudents());
        public RelayCommands assignProject => new RelayCommands(execute => AssignProject());
        public RelayCommands viewAssignedProjects => new RelayCommands(execute => ViewAssignedProjects());



        /// <summary>
        /// Make Group /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void MakeGroup()
        {
            Panel.Children.Clear();
            AddGroupUC grp = new AddGroupUC();

            grp.btnEnter.Content = "Make Group";
            grp.btnEnter.Command = new RelayCommands(execute => MakeGrp(grp));

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> Make new Group";
        }

        private void MakeGrp(AddGroupUC grp)
        {
            if (canAddGrp(grp))
            {
                addingGroupInDb(grp);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearGroupData(grp);
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddGrp(AddGroupUC grp)
        {
            if (string.IsNullOrEmpty(grp.txtdate.Text))
                return false;
            else
                return true;
        }

        private void addingGroupInDb(AddGroupUC grp)
        {

        }

        private void clearGroupData(AddGroupUC grp)
        {
            grp.txtdate.Text = string.Empty;
        }


        /// <summary>
        /// View Group ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewGroup()
        {
            Panel.Children.Clear();
            ViewData grp = new ViewData();
            grp.btnUpdate.Command = new RelayCommands(execute => UpdateGroup(grp), canExecute => grp.lvTableData.SelectedItem != null);
            grp.btnDelete.Command = new RelayCommands(execute => DeleteGroup(grp), canExecute => grp.lvTableData.SelectedItem != null);

            // Data Source dena h 

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> View Groups";
        }

        private void UpdateGroup(ViewData table)
        {
            Panel.Children.Clear();
            AddGroupUC grp = new AddGroupUC();

            // grp.txtdate.Text = table.lvTableData.SelectedItem.AssignmentDate.ToString();

            grp.btnEnter.Content = "Update Group";
            grp.btnEnter.Command = new RelayCommands(execute => UpdateG(grp));

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> View Groups -> Update Group";
        }

        private void UpdateG(AddGroupUC grp)
        {

        }

        private void DeleteGroup(ViewData grp)
        {

        }



        /// <summary>
        /// Add Student in Group /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddGroupStudent()
        {
            Panel.Children.Clear();
            AddGroupStudentUC grp = new AddGroupStudentUC();

            grp.btnEnter.Content = "Add Student";
            grp.btnEnter.Command = new RelayCommands(execute => AddGS(grp));

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> Add Student in Group";
        }

        private void AddGS(AddGroupStudentUC grp)
        {
            if (canAddGS(grp))
            {
                addingGSInDb(grp);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearGSData(grp);
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddGS(AddGroupStudentUC grp)
        {
            if (string.IsNullOrEmpty(grp.txtdate.Text) || string.IsNullOrEmpty(grp.cbGroupID.Text) || string.IsNullOrEmpty(grp.cbStudentID.Text) || string.IsNullOrEmpty(grp.cbStatus.Text))
                return false;
            else
                return true;
        }

        private void addingGSInDb(AddGroupStudentUC grp)
        {

        }

        private void clearGSData(AddGroupStudentUC grp)
        {
            grp.cbGroupID.Text = string.Empty;
            grp.cbStudentID.Text = string.Empty;
            grp.cbStatus.Text = string.Empty;
            grp.txtdate.Text = string.Empty;
        }



        /// <summary>
        /// View All Group Students ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewGroupStudents()
        {
            Panel.Children.Clear();
            ViewData grpS = new ViewData();
            grpS.btnUpdate.Command = new RelayCommands(execute => UpdateGroupStudent(grpS), canExecute => grpS.lvTableData.SelectedItem != null);
            grpS.btnDelete.Command = new RelayCommands(execute => DeleteGroupStudent(grpS), canExecute => grpS.lvTableData.SelectedItem != null);

            // Data Source dena h 

            Panel.Children.Add(grpS);
            address.Content = "Home -> Group Section -> View Group Students";
        }

        private void UpdateGroupStudent(ViewData grpS)
        {
            Panel.Children.Clear();
            AddGroupStudentUC grp = new AddGroupStudentUC();

            /*
            grp.cbGroupID.Text = grpS.lvTableData.SelectedItem.GroupID.ToString();
            grp.cbStudentID.Text = grpS.lvTableData.SelectedItem.StudentID.ToString();
            grp.cbStatus.Text = grpS.lvTableData.SelectedItem.Status.ToString();
            grp.txtdate.Text = grpS.lvTableData.SelectedItem.AssignmentDate.ToString();
            */

            grp.btnEnter.Content = "Update Student";
            grp.btnEnter.Command = new RelayCommands(execute => UpdateGS(grp));

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> View Group Students -> Update ";
        }

        private void UpdateGS(AddGroupStudentUC grp)
        {

        }

        private void DeleteGroupStudent(ViewData grpS)
        {
            
        }



        /// <summary>
        /// Assign Project to Group ///////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AssignProject()
        {
            Panel.Children.Clear();
            AssignProjectUC grpPrj = new AssignProjectUC();

            grpPrj.btnEnter.Content = "Assign Project";
            grpPrj.btnEnter.Command = new RelayCommands(execute => AssignP(grpPrj));

            Panel.Children.Add(grpPrj);
            address.Content = "Home -> Group Section -> Assign a Project";
        }

        private void AssignP(AssignProjectUC grp)
        {
            if (canAssignPrj(grp))
            {
                addingAssignPrjInDb(grp);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearAssignData(grp);
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAssignPrj(AssignProjectUC grp)
        {
            if (string.IsNullOrEmpty(grp.cbGroupID.Text) || string.IsNullOrEmpty(grp.cbProjectID.Text) || string.IsNullOrEmpty(grp.txtdate.Text))
                return false;
            else
                return true;
        }

        private void addingAssignPrjInDb(AssignProjectUC grp)
        {

        }

        private void clearAssignData(AssignProjectUC grp)
        {
            grp.cbGroupID.Text = string.Empty;
            grp.cbProjectID.Text = string.Empty;
            grp.txtdate.Text = string.Empty;
        }


        /// <summary>
        /// View Assigned Projects /////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewAssignedProjects()
        {
            Panel.Children.Clear();
            ViewData grpAp = new ViewData();
            grpAp.btnUpdate.Command = new RelayCommands(execute => UpdateAssignProject(grpAp), canExecute => grpAp.lvTableData.SelectedItem != null);
            grpAp.btnDelete.Command = new RelayCommands(execute => DeleteAssignProject(grpAp), canExecute => grpAp.lvTableData.SelectedItem != null);

            // Data Source dena h 

            Panel.Children.Add(grpAp);
            address.Content = "Home -> Group Section -> View Assigned Projects";
        }

        private void UpdateAssignProject(ViewData grp)
        {
            Panel.Children.Clear();
            AssignProjectUC grpAp = new AssignProjectUC();

            /*
            grpAp.cbGroupID.Text = grp.lvTableData.SelectedItem.GroupID.ToString();
            grpAp.cbProjectID.Text = grp.lvTableData.SelectedItem.ProjectID.ToString();
            grpAp.txtdate.Text = grp.lvTableData.SelectedItem.AssignmentDate.ToString();
            */

            grpAp.btnEnter.Content = "Update Project";
            grpAp.btnEnter.Command = new RelayCommands(execute => UpdateAP(grpAp));

            Panel.Children.Add(grpAp);
            address.Content = "Home -> Group Section -> View Assigned Projects -> Update";
        }

        private void UpdateAP(AssignProjectUC prj)
        {

        }

        private void DeleteAssignProject(ViewData grp)
        {

        }
    }
}
