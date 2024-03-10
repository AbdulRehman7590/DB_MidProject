using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Group;
using System;
using System.Data;
using System.Data.SqlClient;
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
        public RelayCommands addGroupProject => new RelayCommands(execute => AddGroupProject());


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
            if (!string.IsNullOrEmpty(grp.txtdate.Text))
            {
                if (addingGroupInDb(grp))
                {
                    MessageBox.Show("Group Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearGroupData(grp);
                }
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool addingGroupInDb(AddGroupUC grp)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO [Group] VALUES (@Created_On)", con);
                    cmd.Parameters.AddWithValue("@Created_On", grp.txtdate.SelectedDate.Value.ToString("yyyy-MM-dd"));
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
            ManageGroups grp = new ManageGroups();
            
            string query = @"SELECT G.Id, G.Created_On, GP.ProjectId, Prj.Title, GP.AssignmentDate, GE.EvaluationId, GE.EvaluationDate, E.Name, 
                             	COALESCE(StudentCount.NumberOfActiveStudents, 0) AS NumberOfStudents
                             FROM [Group] G
                                 INNER JOIN GroupProject GP ON GP.GroupId = G.Id
                                 LEFT JOIN Project Prj ON GP.ProjectId = Prj.Id
                                 LEFT JOIN GroupEvaluation GE ON GE.GroupId = G.Id
                                 LEFT JOIN Evaluation E ON GE.EvaluationId = E.Id
                                 LEFT JOIN ( SELECT GS.GroupId, COUNT(*) AS NumberOfActiveStudents
                             				FROM GroupStudent GS INNER JOIN Lookup L ON GS.Status = L.Id
                             				WHERE L.Value = 'Active' GROUP BY GS.GroupId
                             				) AS StudentCount ON StudentCount.GroupId = G.Id";
            Configuration.ShowData(grp.dgTableData, query);

            grp.btnUpdatePrj.Command = new RelayCommands(execute => UpdateGroupProject(grp), canExecute => checkStuCount(grp));
            grp.btnUpdateStu.Command = new RelayCommands(execute => UpdateGroupStudent(grp), canExecute => grp.dgTableData.SelectedItem != null);
            grp.btnAddEvaluation.Command = new RelayCommands(execute => AddEvaluation(grp), canExecute => checkStuCount(grp));

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> View Groups";
        }
        
        private bool checkStuCount(ManageGroups grp)
        {
            if (grp.dgTableData.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)grp.dgTableData.SelectedItem;
                string numberOfStudents = selectedRow.Row["NumberOfStudents"].ToString();
                return !string.IsNullOrEmpty(numberOfStudents) && int.Parse(numberOfStudents) > 0;
            }
            return false;
        }

        /// <summary>
        /// Add Group Project ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddGroupProject()
        {
            Panel.Children.Clear();
            ViewData grp = new ViewData();

            string query = @"SELECT G.Id, G.Created_On
                             FROM [Group] G
                             LEFT JOIN GroupProject GP ON G.Id = GP.GroupId
                             WHERE GP.ProjectId IS NULL";
            Configuration.ShowData(grp.lvTableData, query);

            grp.btnDelete.Visibility = Visibility.Hidden;
            grp.btnUpdate.Visibility = Visibility.Hidden;

            grp.lvTableData.MouseDoubleClick += (sender, e) => AssignGroupProject(grp);

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> View Groups";
        }

        private void AssignGroupProject(ViewData table)
        {
            Panel.Children.Clear();
            AssignProjectUC prj = new AssignProjectUC();

            string gID = ((DataRowView)table.lvTableData.SelectedItem).Row.ItemArray[0].ToString();

            var con = Configuration.getInstance().getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT Id, Title, Description FROM Project P WHERE Title NOT LIKE '!!%'", con);
            cmd.Parameters.AddWithValue("@gid", gID);
            Configuration.ShowData(prj.dgProjects, cmd);
            
            prj.lbldata.Visibility = Visibility.Hidden;
            prj.lblcurrent.Visibility = Visibility.Hidden;
            prj.btnDelete.Visibility = Visibility.Hidden;

            prj.btnEnter.Content = "Assign Project";
            prj.btnEnter.Command = new RelayCommands(execute => UpdateGProject(prj, gID), canExecute => prj.dgProjects.SelectedItem != null && prj.txtdate.Text != null);

            Panel.Children.Add(prj);
            address.Content = "Home -> Group Section -> View Groups -> Assign Group Project";
        }


        /// <summary>
        /// Updating the group Project ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void UpdateGroupProject(ManageGroups grp)
        {
            Panel.Children.Clear();
            AssignProjectUC prj = new AssignProjectUC();

            string gID = ((DataRowView)grp.dgTableData.SelectedItem).Row.ItemArray[0].ToString();

            using (var con = Configuration.getInstance().getConnection())
            {
                con.Open();
                string query = @"SELECT Id, Title, Description 
                                 FROM Project P
                                    LEFT JOIN GroupProject GP ON P.id = GP.ProjectId
                                 WHERE GP.ProjectId IS NULL AND P.Title NOT LIKE '!!%'";
                Configuration.ShowData(prj.dgProjects, query);
            }
            prj.lbldata.Content = ((DataRowView)grp.dgTableData.SelectedItem).Row.ItemArray[3].ToString();
            prj.txtdate.Text = ((DataRowView)grp.dgTableData.SelectedItem).Row.ItemArray[4].ToString();

            prj.btnEnter.Command = new RelayCommands(execute => UpdateGProject(prj, gID), canExecute => prj.dgProjects.SelectedItem != null);
            prj.btnDelete.Command = new RelayCommands(execute => DeleteGProject(gID));

            Panel.Children.Add(prj);
            address.Content = "Home -> Group Section -> View Groups -> Update Group Project";
        }

        private void UpdateGProject(AssignProjectUC prj, string id)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    if (prj.btnEnter.Content.ToString() == "Assign Project")
                    {
                        SqlCommand cmd = new SqlCommand(@"INSERT INTO GroupProject VALUES (@ProjectId, @GroupId, @AssignmentDate)", con);
                        cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)prj.dgProjects.SelectedItem).Row.ItemArray[0].ToString());
                        cmd.Parameters.AddWithValue("@GroupId", id);
                        cmd.Parameters.AddWithValue("@AssignmentDate", prj.txtdate.SelectedDate.Value.ToString("yyyy-MM-dd"));

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand(@"UPDATE GroupProject SET ProjectId = @ProjectId, AssignmentDate = @AssignmentDate WHERE GroupId = @GroupId", con);
                        cmd.Parameters.AddWithValue("@GroupId", id);
                        cmd.Parameters.AddWithValue("@AssignmentDate", prj.txtdate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)prj.dgProjects.SelectedItem).Row.ItemArray[0].ToString());

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ViewGroup();
            }
        }

        private void DeleteGProject(string id)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"DELETE FROM GroupProject WHERE GroupId = @GroupId", con);
                    cmd.Parameters.AddWithValue("@GroupId", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ViewGroup();
            }
        }



        /// <summary>
        /// Add Evaluation ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddEvaluation(ManageGroups grp)
        {
            Panel.Children.Clear();
            AssignProjectUC eval = new AssignProjectUC();

            string gId = ((DataRowView)grp.dgTableData.SelectedItem).Row.ItemArray[0].ToString();
            
            eval.lblTable.Content = "Evaluations: ";
            eval.txtdate.Visibility = Visibility.Hidden;
            eval.lbldata.Visibility = Visibility.Hidden;
            eval.lblcurrent.Visibility = Visibility.Hidden;
            eval.btnDelete.Visibility = Visibility.Hidden;
            eval.lbldate.Visibility = Visibility.Hidden;

            eval.btnEnter.Margin = new Thickness(75,10,10,50);
            eval.btnEnter.Command = new RelayCommands(execute => AddEval(eval, gId), canExecute => eval.dgProjects.SelectedItem != null);

            var con = Configuration.getInstance().getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT E.Id, E.Name, E.TotalMarks, E.TotalWeightage 
                                      FROM Evaluation E 
                                          LEFT JOIN GroupEvaluation GE ON E.id = GE.EvaluationId AND GE.GroupId = @gid
                                      WHERE GE.GroupId IS NULL AND E.Name NOT LIKE '!!%'", con);
            cmd.Parameters.AddWithValue("@gid", gId);
            Configuration.ShowData(eval.dgProjects, cmd);
            
            Panel.Children.Add(eval);
            address.Content = "Home -> Group Section -> View Groups -> Add Evaluation";
        }

        private void AddEval(AssignProjectUC eval, string gId)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO GroupEvaluation VALUES (@GroupId, @EvaluationId, @ObtainedMarks, @EvaluationDate)", con);
                    cmd.Parameters.AddWithValue("@GroupId", gId);
                    cmd.Parameters.AddWithValue("@EvaluationId", ((DataRowView)eval.dgProjects.SelectedItem).Row.ItemArray[0].ToString());
                    cmd.Parameters.AddWithValue("@ObtainedMarks", 0);
                    cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ViewGroup();
            }
        }

       

        /// <summary>
        /// Updating Group Students ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void UpdateGroupStudent(ManageGroups grp)
        {
            Panel.Children.Clear();
            UpdateGroupData grpS = new UpdateGroupData();

            string gid = ((DataRowView)grp.dgTableData.SelectedItem).Row.ItemArray[0].ToString();
            RefreshPageData(grpS, gid);

            grpS.btnUpdate.Command = new RelayCommands(execute => UpdateGS(grpS, gid), canExecute => grpS.dgUnselect.SelectedItem != null);
            grpS.btnDelete.Command = new RelayCommands(execute => DeleteGS(grpS, gid), canExecute => grpS.dgSelect.SelectedItem != null);

            Panel.Children.Add(grpS);
            address.Content = "Home -> Group Section -> View Groups -> Update Group Students";
        }

        private void DeleteGS(UpdateGroupData grp, string gid)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE GroupStudent SET Status = 4 WHERE GroupId = @GroupId AND StudentId = @StudentId", con);
                    cmd.Parameters.AddWithValue("@GroupId", gid);
                    cmd.Parameters.AddWithValue("@StudentId", ((DataRowView)grp.dgSelect.SelectedItem).Row.ItemArray[0].ToString());
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                RefreshPageData(grp, gid);
            }
        }

        private void UpdateGS(UpdateGroupData grpS, string gid)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();
                    if (isGroupFree(gid))
                    {
                        SqlCommand cmd = new SqlCommand(@"INSERT INTO GroupStudent VALUES (@GroupId, @StudentId, @Status, @AssignmentDate)", con);
                        cmd.Parameters.AddWithValue("@GroupId", gid);
                        cmd.Parameters.AddWithValue("@StudentId", ((DataRowView)grpS.dgUnselect.SelectedItem).Row.ItemArray[0].ToString());
                        cmd.Parameters.AddWithValue("@Status", 3);
                        cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Now.Date);
                        cmd.ExecuteNonQuery();
                    }
                    else
                        MessageBox.Show("Group is already full!!!", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                RefreshPageData(grpS, gid);
            }
        }

        private bool isGroupFree(string gId)
        {
            int studentCount = 0;
            using (var con = Configuration.getInstance().getConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(Status) FROM GroupStudent JOIN Lookup Lk ON Status = Lk.Id WHERE GroupId = @GroupId AND Lk.Value = 'Active'", con);
                cmd.Parameters.AddWithValue("@GroupId", gId);
                studentCount = (int)cmd.ExecuteScalar();
            }
            return studentCount < 4;
        }

        private void RefreshPageData(UpdateGroupData grp, string gid)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    grp.dgSelect.ItemsSource = GetGroupStudentData(con, gid, true).DefaultView;

                    grp.dgUnselect.ItemsSource = GetGroupStudentData(con, gid, false).DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DataTable GetGroupStudentData(SqlConnection con, string gid, bool isActive)
        {
            string query = isActive ?
                @"SELECT GS.StudentId, S.RegistrationNo, P.FirstName, P.LastName, P.Contact, Lk.Value AS Gender, L2.Value AS Status, GS.AssignmentDate
          FROM GroupStudent GS
          JOIN Student S ON GS.StudentId = S.Id
          JOIN Person P ON S.Id = P.Id
          JOIN Lookup Lk ON P.Gender = Lk.Id
          JOIN Lookup L2 ON GS.Status = L2.Id
          WHERE GS.GroupId = @GrpID
          AND P.FirstName NOT LIKE '!!%'
          AND L2.Value = 'Active'"
                :
                @"SELECT S.Id AS StudentId, S.RegistrationNo, P.FirstName, P.LastName, P.Contact, Lk.Value AS Gender
          FROM Student S
          JOIN Person P ON S.Id = P.Id
          JOIN Lookup Lk ON P.Gender = Lk.Id
          LEFT JOIN GroupStudent GS ON GS.StudentId = S.Id
          WHERE (GS.StudentId IS NULL OR GS.Status = (SELECT Id FROM Lookup WHERE Value = 'InActive'))
          AND P.FirstName NOT LIKE '!!%'";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@GrpID", gid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }


    }
}
