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
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Project VALUES (@Title, @Description)", con);
                    cmd.Parameters.AddWithValue("@Title", eval.txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Description", eval.txtDescription.Text);
                    cmd.ExecuteNonQuery();
                }
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

            Configuration.ShowData(eval.lvTableData, @"SELECT Id, Title, Description FROM Project WHERE Title NOT LIKE '!!%'");

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
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE Project SET Title = @Title, Description = @Description WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Title", prj.txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Description", prj.txtDescription.Text);
                    cmd.Parameters.AddWithValue("@Id", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data Updated Successfully!!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ViewProjects();
            }
        }

        private void DeleteProject(ViewData eval)
        {
            if (!string.IsNullOrEmpty(((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[0].ToString()))
            {
                try
                {
                    using (var con = Configuration.getInstance().getConnection())
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand(@"BEGIN TRANSACTION
                                                          UPDATE Project SET Title = @Title, Description = @Description WHERE Id = @Id;
                                                          DELETE FROM ProjectAdvisor WHERE ProjectId  = @Id;
                                                          DELETE FROM GroupProject WHERE ProjectId = @Id;
                                                          COMMIT TRANSACTION", con);
                        cmd.Parameters.AddWithValue("@Id", ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                        cmd.Parameters.AddWithValue("@Title", "!!" + ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[1].ToString());
                        cmd.Parameters.AddWithValue("@Description", ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[2].ToString());
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Deleted Successfully!!!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally
                {
                    ViewProjects();
                }
            }
            else
                MessageBox.Show("Select Valid row!", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }



        /// <summary>
        /// Add Advisor in Project /////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddProjectAdvisor()
        {
            Panel.Children.Clear();
            ViewData grp = new ViewData();

            Configuration.ShowData(grp.lvTableData, @"SELECT Id, Title, Description FROM Project LEFT JOIN ProjectAdvisor PA ON Id = PA.ProjectId WHERE PA.AdvisorId IS NULL AND PA.AdvisorRole IS NULL AND Title NOT LIKE '!!%'");

            grp.btnUpdate.Visibility = Visibility.Hidden;
            grp.btnDelete.Visibility = Visibility.Hidden;

            grp.lvTableData.MouseDoubleClick += (sender, e) => AddPrjAdvisor(grp);

            Panel.Children.Add(grp);
            address.Content = "Home -> Project Section -> Add Project Advisor";
        }

        private void RefreshPage(DataGrid dg)
        {
            string query = @"SELECT Ad.Id, P.FirstName, P.LastName, Lk.Value AS Designation, Ad.Salary, P.Contact, P.Email, P.DateOfBirth, 
                                (SELECT Value FROM Lookup L WHERE P.Gender = L.Id AND L.Category = 'Gender') AS Gender
                            FROM Advisor Ad 
                            	JOIN Lookup Lk ON Ad.Designation = Lk.Id AND Lk.Category = 'Designation'
                            	JOIN Person P ON Ad.Id = P.Id
								LEFT JOIN ProjectAdvisor PA ON Ad.Id = PA.AdvisorId
							WHERE PA.ProjectId IS NULL AND P.FirstName NOT LIKE '!!%'";
            Configuration.ShowData(dg, query);
        }

        private void AddPrjAdvisor(ViewData eval)
        {
            Panel.Children.Clear();
            AddProjectAdvisorUC prj = new AddProjectAdvisorUC();

            RefreshPage(prj.dgAdvisor);

            prj.dgAdvisor.MouseDoubleClick += (sender, e) => PopulateData(prj);

            prj.btnEnter.Content = "Add Advisors";
            prj.btnEnter.Command = new RelayCommands(execute => AddPrjAdv(prj, eval), canExecute => FillDataMessage(prj));

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> Add Project Advisor -> Add Advisor in Project";
        }

        private void PopulateData(AddProjectAdvisorUC prj)
        {
            string advisor = ((DataRowView)prj.dgAdvisor.SelectedItem).Row.ItemArray[0].ToString() + " " + ((DataRowView)prj.dgAdvisor.SelectedItem).Row.ItemArray[1].ToString() + " " + ((DataRowView)prj.dgAdvisor.SelectedItem).Row.ItemArray[2].ToString();

            if (prj.rdoMain.IsChecked == true && (prj.txtCoRole.Text != advisor && prj.txtIndusRole.Text != advisor))
                prj.txtMainRole.Text = advisor;
            else if (prj.rdoCO.IsChecked == true && (prj.txtMainRole.Text != advisor && prj.txtIndusRole.Text != advisor))
                prj.txtCoRole.Text = advisor;
            else if (prj.rdoInd.IsChecked == true && (prj.txtCoRole.Text != advisor && prj.txtMainRole.Text != advisor))
                prj.txtIndusRole.Text = advisor;
            else
                MessageBox.Show("Advisor already added!!!", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AddPrjAdv(AddProjectAdvisorUC prj, ViewData eval)
        {
            using (var con = Configuration.getInstance().getConnection())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO ProjectAdvisor VALUES(@AdvisorId, @ProjectId, @AdvisorRole, @AssignmentDate)", con, transaction);
                    cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)eval.lvTableData.SelectedItem).Row["Id"]);
                    cmd.Parameters.AddWithValue("@AdvisorId", prj.txtMainRole.Text.ToString().Split(' ')[0]);
                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@AdvisorRole", 11);
                    cmd.ExecuteNonQuery();


                    cmd = new SqlCommand(@"INSERT INTO ProjectAdvisor VALUES(@AdvisorId, @ProjectId, @AdvisorRole, @AssignmentDate)", con, transaction);
                    cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)eval.lvTableData.SelectedItem).Row["Id"]);
                    cmd.Parameters.AddWithValue("@AdvisorId", prj.txtCoRole.Text.ToString().Split(' ')[0]);
                    cmd.Parameters.AddWithValue("@AdvisorRole", 12);
                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Now);
                    cmd.ExecuteNonQuery();


                    cmd = new SqlCommand(@"INSERT INTO ProjectAdvisor VALUES(@AdvisorId, @ProjectId, @AdvisorRole, @AssignmentDate)", con, transaction);
                    cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)eval.lvTableData.SelectedItem).Row["Id"]);
                    cmd.Parameters.AddWithValue("@AdvisorId", prj.txtIndusRole.Text.ToString().Split(' ')[0]);
                    cmd.Parameters.AddWithValue("@AdvisorRole", 14);
                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    transaction.Commit();

                    MessageBox.Show("Added Successfully!!!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception error)
                {
                    transaction.Rollback();
                    MessageBox.Show(error.Message);
                }
            }

            AddProjectAdvisor();            
        }

        private bool FillDataMessage(AddProjectAdvisorUC prj)
        {
            if (string.IsNullOrEmpty(prj.txtMainRole.Text) || string.IsNullOrEmpty(prj.txtCoRole.Text) || string.IsNullOrEmpty(prj.txtIndusRole.Text))
            {
                return false;
            }
            else
                return true;
        }



        /// <summary>
        /// View Project Advisors //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewProjectAdvisors()
        {
            Panel.Children.Clear();
            ViewData grp = new ViewData();

            string query = @"SELECT PA.ProjectId, 
                                 MAX(P.Title) AS Title, 
                                 MAX(CASE WHEN PA.AdvisorRole = 11 THEN A.Id END) AS MainAdvisorId,
                                 MAX(CASE WHEN PA.AdvisorRole = 11 THEN CONCAT(Pers.FirstName, ' ', Pers.LastName) END) AS MainAdvisor,
                                 MAX(CASE WHEN PA.AdvisorRole = 12 THEN A.Id END) AS CoAdvisorId, 
                                 MAX(CASE WHEN PA.AdvisorRole = 12 THEN CONCAT(Pers.FirstName, ' ', Pers.LastName) END) AS CoAdvisor, 
                                 MAX(CASE WHEN PA.AdvisorRole = 14 THEN A.Id END) AS IndustryAdvisorId,
                                 MAX(CASE WHEN PA.AdvisorRole = 14 THEN CONCAT(Pers.FirstName, ' ', Pers.LastName) END) AS IndustryAdvisor
                             FROM ProjectAdvisor PA 
                                 INNER JOIN Advisor A ON PA.AdvisorId = A.Id 
                                 JOIN Project P ON P.Id = PA.ProjectId 
                                 JOIN Person Pers ON Pers.Id = A.Id
                             WHERE Pers.FirstName NOT LIKE '!!%' AND P.Title NOT LIKE '!!%'
                             GROUP BY PA.ProjectId";

            Configuration.ShowData(grp.lvTableData, query);

            grp.btnDelete.Visibility = Visibility.Hidden;
            grp.btnUpdate.HorizontalAlignment = HorizontalAlignment.Right;

            grp.btnUpdate.Command = new RelayCommands(execute => UpdatePrjAdvisor(grp), canExecute => grp.lvTableData.SelectedItem != null);

            Panel.Children.Add(grp);
            address.Content = "Home -> Project Section -> View Project Advisors";

        }

        private void UpdatePrjAdvisor(ViewData man)
        {
            Panel.Children.Clear();
            AddProjectAdvisorUC prj = new AddProjectAdvisorUC();

            RefreshPage(prj.dgAdvisor);

            prj.txtMainRole.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[2].ToString() + " " + ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString();
            prj.txtCoRole.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[4].ToString() + " " + ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[5].ToString();
            prj.txtIndusRole.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[6].ToString() + " " + ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[7].ToString();

            prj.dgAdvisor.MouseDoubleClick += (sender, e) => PopulateData(prj);

            prj.btnEnter.Content = "Add Advisors";
            prj.btnEnter.Command = new RelayCommands(execute => UpdatePA(prj, man), canExecute => FillDataMessage(prj));

            Panel.Children.Add(prj);
            address.Content = "Home -> Project Section -> View Project Advisors -> Update Project Advisor";
        }

        private void UpdatePA(AddProjectAdvisorUC prj, ViewData eval)
        {
            using (var con = Configuration.getInstance().getConnection())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE ProjectAdvisor SET AdvisorId = @AdvisorId, AssignmentDate = @AssignmentDate WHERE ProjectId = @ProjectId, AdvisorRole = @AdvisorRole)", con, transaction);
                    cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[2]);
                    cmd.Parameters.AddWithValue("@AdvisorId", prj.txtMainRole.Text.ToString().Split(' ')[0]);
                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@AdvisorRole", 11);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(@"UPDATE ProjectAdvisor SET AdvisorId = @AdvisorId, AssignmentDate = @AssignmentDate WHERE ProjectId = @ProjectId, AdvisorRole = @AdvisorRole)", con, transaction);
                    cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[4]);
                    cmd.Parameters.AddWithValue("@AdvisorId", prj.txtCoRole.Text.ToString().Split(' ')[0]);
                    cmd.Parameters.AddWithValue("@AdvisorRole", 12);
                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(@"UPDATE ProjectAdvisor SET AdvisorId = @AdvisorId, AssignmentDate = @AssignmentDate WHERE ProjectId = @ProjectId, AdvisorRole = @AdvisorRole)", con, transaction);
                    cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[6]);
                    cmd.Parameters.AddWithValue("@AdvisorId", prj.txtIndusRole.Text.ToString().Split(' ')[0]);
                    cmd.Parameters.AddWithValue("@AdvisorRole", 14);
                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    transaction.Commit();

                    MessageBox.Show("Added Successfully!!!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception error)
                {
                    transaction.Rollback();
                    MessageBox.Show(error.Message);
                }
            }
            ViewProjectAdvisors();            
        }

    }
}
