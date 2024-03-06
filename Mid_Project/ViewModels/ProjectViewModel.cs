using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Evaluation;
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
    internal class ProjectViewModel
    {
        private readonly Grid Panel;
        private readonly Label address;

        public ProjectViewModel(Grid panel, Label address)
        {
            Panel = panel;
            this.address = address;
        }


        /// <summary>
        /// Relay Commands ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands addProject => new RelayCommands(execute => AddProject());
        public RelayCommands viewProjects => new RelayCommands(execute => ViewProjects());
        public RelayCommands addProjectAdvisor => new RelayCommands(execute => AddProjectAdvisor());
        public RelayCommands viewProjectAdvisors => new RelayCommands(execute => ViewProjectAdvisors());


        /// <summary>
        /// Add Project /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddProject() 
        {
            Panel.Children.Clear();
            AddProjectUC prj = new AddProjectUC();

            prj.btnEnter.Content = "Add Project";
            prj.btnEnter.Command = new RelayCommands(execute => AddPrj(prj));

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> Add Project";
        }

        private void AddPrj(AddProjectUC eval)
        {
            if (canAddPrj(eval))
            {
                addingProjectInDb(eval);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearAddData(eval);
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddPrj(AddProjectUC eval)
        {
            if (string.IsNullOrEmpty(eval.txtTitle.Text) || string.IsNullOrEmpty(eval.txtDescription.Text))
                return false;
            else
                return true;
        }

        private void addingProjectInDb(AddProjectUC eval)
        {

        }

        private void clearAddData(AddProjectUC eval)
        {
            eval.txtTitle.Text = string.Empty;
            eval.txtDescription.Text = string.Empty;
        }


        /// <summary>
        /// View All Projects ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewProjects()
        {
            Panel.Children.Clear();
            ViewData eval = new ViewData();
            eval.btnUpdate.Command = new RelayCommands(execute => UpdateProject(eval), canExecute => eval.lvTableData.SelectedItem != null);
            eval.btnDelete.Command = new RelayCommands(execute => DeleteProject(eval), canExecute => eval.lvTableData.SelectedItem != null);

            // Data Source dena h 

            Panel.Children.Add(eval);
            address.Content = "Home -> Project Section -> View Projects";
        }

        private void UpdateProject(ViewData eval)
        {
            Panel.Children.Clear();
            AddProjectUC prj = new AddProjectUC();

            /*
            prj.txtTitle.Text = eval.lvTableData.SelectedItem.Title;
            prj.txtDescription.Text = eval.lvTableData.SelectedItem.Description;
            */

            prj.btnEnter.Content = "Update Evaluation";
            prj.btnEnter.Command = new RelayCommands(execute => UpdateP(prj));

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> View Projects -> Update Project";
        }

        private void UpdateP(AddProjectUC prj)
        {

        }

        private void DeleteProject(ViewData eval)
        {

        }



        /// <summary>
        /// Add Advisor in Project /////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddProjectAdvisor()
        {
            Panel.Children.Clear();
            AddProjectAdvisorUC prj = new AddProjectAdvisorUC();

            prj.btnEnter.Content = "Add Advisor";
            prj.btnEnter.Command = new RelayCommands(execute => AddPrjAdvisor(prj));

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> Add Project Advisor";
        }


        private void AddPrjAdvisor(AddProjectAdvisorUC eval)
        {
            if (canAddPrjAdvisor(eval))
            {
                addingPrjAdvisorInDb(eval);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearAddPrjAdvisorData(eval);
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddPrjAdvisor(AddProjectAdvisorUC eval)
        {
            if (string.IsNullOrEmpty(eval.txtdate.Text) || string.IsNullOrEmpty(eval.cbAdvisorID.Text) || string.IsNullOrEmpty(eval.cbProjectID.Text) || string.IsNullOrEmpty(eval.cbAdvisorRole.Text))
                return false;
            else
                return true;
        }

        private void addingPrjAdvisorInDb(AddProjectAdvisorUC eval)
        {

        }

        private void clearAddPrjAdvisorData(AddProjectAdvisorUC eval)
        {
            eval.cbProjectID.Text = string.Empty;
            eval.cbAdvisorID.Text = string.Empty;
            eval.cbAdvisorRole.Text = string.Empty;
            eval.txtdate.Text = string.Empty;
        }


        /// <summary>
        /// View Project Advisors //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewProjectAdvisors()
        {
            Panel.Children.Clear();
            ViewData eval = new ViewData();
            eval.btnUpdate.Command = new RelayCommands(execute => UpdatePrjAdvisor(eval), canExecute => eval.lvTableData.SelectedItem != null);
            eval.btnDelete.Command = new RelayCommands(execute => DeletePrjAdvisor(eval), canExecute => eval.lvTableData.SelectedItem != null);

            // Data Source dena h 

            Panel.Children.Add(eval);
            address.Content = "Home -> Project Section -> View Project Advisors";
        }

        private void UpdatePrjAdvisor(ViewData eval)
        {
            Panel.Children.Clear();
            AddProjectAdvisorUC prj = new AddProjectAdvisorUC();

            /*
            prj.cbProjectID.Text = eval.lvTableData.SelectedItem.ProjectID.ToString();
            prj.cbAdvisorID.Text = eval.lvTableData.SelectedItem.AdvisorID.ToString();
            prj.cbAdvisorRole.Text = eval.lvTableData.SelectedItem.AdvisorRole.ToString();
            prj.txtdate.Text = eval.lvTableData.SelectedItem.AssignmentDate.ToString();
            */

            prj.btnEnter.Content = "Update Project Advisor";
            prj.btnEnter.Command = new RelayCommands(execute => UpdatePA(prj));

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> View Project Advisors -> Update Project Advisor";
        }

        private void UpdatePA(AddProjectAdvisorUC prj)
        {

        }

        private void DeletePrjAdvisor(ViewData eval)
        {

        }

    }
}
