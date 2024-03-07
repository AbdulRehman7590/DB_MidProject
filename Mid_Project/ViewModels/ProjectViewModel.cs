using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Project;
using System;
using System.Data;
using System.Data.SqlClient;
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
                if (addingProjectInDb(eval))
                {
                    MessageBox.Show("Project Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearAddData(eval);
                }
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

        private bool addingProjectInDb(AddProjectUC eval)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO Project VALUES (@Title, @Description)", con);
                cmd.Parameters.AddWithValue("@Title", eval.txtTitle.Text);
                cmd.Parameters.AddWithValue("@Description", eval.txtTitle.Text);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
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

            string query = @"SELECT Id, Title, Description FROM Project";
            Configuration.ShowData(eval.lvTableData, query);

            Panel.Children.Add(eval);
            address.Content = "Home -> Project Section -> View Projects";
        }

        private void UpdateProject(ViewData eval)
        {
            Panel.Children.Clear();
            AddProjectUC prj = new AddProjectUC();

            prj.txtTitle.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[1].ToString();
            prj.txtDescription.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[2].ToString();

            prj.btnEnter.Content = "Update Evaluation";
            prj.btnEnter.Command = new RelayCommands(execute => UpdateP(prj, eval));

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> View Projects -> Update Project";
        }

        private void UpdateP(AddProjectUC prj, ViewData man)
        {
            var con = Configuration.getInstance().getConnection();

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Project SET Title = @Title, Description = @Description WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Title", prj.txtTitle.Text);
                cmd.Parameters.AddWithValue("@Description", prj.txtDescription.Text);
                cmd.Parameters.AddWithValue("@Id", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data Updated Successfully!!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                GobackToView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProject(ViewData eval)
        {
            if (!string.IsNullOrEmpty(((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[0].ToString()))
            {
                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Delete From Project Where Id = @Id");
                    cmd.Parameters.AddWithValue("@Id", ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully!!!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                    GobackToView();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
                MessageBox.Show("Select Valid row!", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        private void GobackToView()
        {
            Panel.Children.Clear();
            ViewProjects();
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

            string query = @"SELECT Ad.Id, FirstName, LastName, Lk.Value AS Designation, Salary, Contact, Email,  DateOfBirth, (SELECT Value FROM Lookup WHERE Gender = Lookup.Id) AS Gender
                             FROM Advisor Ad 
                                JOIN Lookup Lk ON Ad.Designation = Lk.Id
                                JOIN Person P ON P.Gender = Lk.Id";
            Configuration.ShowData(prj.dgAdvisor,query);

            string query1 = @"SELECT Id, Title, Description FROM Project";
            Configuration.ShowData(prj.dgProject, query1);

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> Add Project Advisor";
        }

        private void AddPrjAdvisor(AddProjectAdvisorUC eval)
        {
            if (canAddPrjAdvisor(eval))
            {
                if (addingPrjAdvisorInDb(eval))
                {
                    MessageBox.Show("Project Advisor Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearAddPrjAdvisorData(eval);
                }
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddPrjAdvisor(AddProjectAdvisorUC eval)
        {
            if (string.IsNullOrEmpty(eval.txtdate.Text) || string.IsNullOrEmpty(((DataRowView)eval.dgAdvisor.SelectedItem).Row.ItemArray[0].ToString()) || string.IsNullOrEmpty(((DataRowView)eval.dgProject.SelectedItem).Row.ItemArray[0].ToString()) || string.IsNullOrEmpty(eval.cbAdvisorRole.Text))
                return false;
            else
                return true;

        }

        private bool addingPrjAdvisorInDb(AddProjectAdvisorUC eval)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO ProjectAdvisor values (@ProjectId, @AdvisorId, @AdvisorRole, @AssignmentDate)", con);
                cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)eval.dgProject.SelectedItem).Row.ItemArray[0].ToString());
                cmd.Parameters.AddWithValue("@AdvisorId", ((DataRowView)eval.dgAdvisor.SelectedItem).Row.ItemArray[0].ToString());
                cmd.Parameters.AddWithValue("@AdvisorRole", eval.cbAdvisorRole.SelectedIndex + 11);
                cmd.Parameters.AddWithValue("@AssignmentDate", eval.txtdate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void clearAddPrjAdvisorData(AddProjectAdvisorUC eval)
        {
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

            string query = @"SELECT PA.AdvisorId, CONCAT(P.FirstName, P.LastName) AS Name, PA.ProjectId, Prj.Title, PA.AdvisorRole, PA.AssignmentDate 
                             FROM ProjectAdvisor PA
	                            JOIN Person P ON PA.AdvisorID = P.Id
	                            JOIN Project Prj ON PA.ProjectId = Prj.Id";
            Configuration.ShowData(eval.lvTableData, query);

            Panel.Children.Add(eval);
            address.Content = "Home -> Project Section -> View Project Advisors";
        }

        private void UpdatePrjAdvisor(ViewData man)
        {
            Panel.Children.Clear();
            AddProjectAdvisorUC prj = new AddProjectAdvisorUC();

            string query = @"SELECT Ad.Id, P.FirstName, P.LastName, Lk.Value AS Designation, Salary, P.Contact, P.Email, P.DateOfBirth, (SELECT Value FROM Lookup WHERE P.Gender = Lookup.Id) AS Gender
                             FROM Advisor Ad 
                                JOIN Lookup Lk ON Ad.Designation = Lk.Id
                                JOIN Person P ON P.Gender = Lk.Id";
            Configuration.ShowData(prj.dgAdvisor, query);

            string query1 = @"SELECT Id, Title, Description FROM Project";
            Configuration.ShowData(prj.dgProject, query1);

            prj.cbAdvisorRole.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[2].ToString();
            prj.txtdate.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString();

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


        private void GobackToView1()
        {
            Panel.Children.Clear();
            ViewProjectAdvisors();
        }

    }
}
