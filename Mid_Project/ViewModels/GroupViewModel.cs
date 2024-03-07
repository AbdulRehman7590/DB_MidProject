using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Group;
using Mid_Project.Views.Project;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
                if (addingGroupInDb(grp))
                {
                    MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearGroupData(grp);
                }
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

        private bool addingGroupInDb(AddGroupUC grp)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO Group VALUES (@Created_On)", con);
                cmd.Parameters.AddWithValue("@FirstName", grp.txtdate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
                
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

            grp.btnUpdatePrj.Command = new RelayCommands(execute => UpdateGroupProject(grp), canExecute => grp.dgTableData.SelectedItem != null);
            grp.btnUpdateStu.Command = new RelayCommands(execute => UpdateGroupStudent(grp), canExecute => grp.dgTableData.SelectedItem != null);
            grp.btnAddEvaluation.Command = new RelayCommands(execute => AddEvaluation(grp), canExecute => grp.dgTableData.SelectedItem != null);
            grp.btnDelete.Command = new RelayCommands(execute => DeleteGroup(grp), canExecute => grp.dgTableData.SelectedItem != null);

            string query = @"SELECT G.Id, G.Created_On, GP.ProjectId, Prj.Title, GE.EvaluationId, COUNT(GS.StudentId) AS NumberOfStudents
                             FROM GroupStudent GS
                             	JOIN [Group] G ON GS.GroupId = G.Id
                             	JOIN GroupProject GP ON GP.GroupId = G.Id
                             	JOIN Project Prj ON GP.ProjectId = Prj.Id
                             	JOIN GroupEvaluation GE ON GE.GroupId = G.Id
                             GROUP BY G.Id, G.Created_On, GP.ProjectId, Prj.Title, GE.EvaluationId";
            Configuration.ShowData(grp.dgTableData, query);

            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> View Groups";
        }


        /// <summary>
        /// Updating the group Project ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void UpdateGroupProject(ManageGroups table)
        {
            Panel.Children.Clear();
            AssignProjectUC prj = new AssignProjectUC();

            string query = @"SELECT Id, Title, Description FROM Project";
            Configuration.ShowData(prj.dgProjects, query);
            
            prj.lbldata.Content = ((DataRowView)table.dgTableData.SelectedItem).Row.ItemArray[4].ToString();
            string gID = ((DataRowView)table.dgTableData.SelectedItem).Row.ItemArray[0].ToString();
            prj.btnEnter.Command = new RelayCommands(execute => UpdateGProject(prj, gID), canExecute => prj.dgProjects.SelectedItem != null);

            Panel.Children.Add(prj);
            address.Content = "Home -> Group Section -> View Groups -> Update Group Project";
        }

        private void UpdateGProject(AssignProjectUC prj, string id)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"UPDATE GroupProject SET ProjectId = @ProjectId, AssignmentDate = @AssignmentDate WHERE GroupId = @GroupId", con);
                cmd.Parameters.AddWithValue("@GroupId", id);
                cmd.Parameters.AddWithValue("@AssignmentDate", prj.txtDOB.SelectedDate.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ProjectId", ((DataRowView)prj.dgProjects.SelectedItem).Row.ItemArray[0].ToString());
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Delete Group //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void DeleteGroup(ManageGroups grp)
        {

        }


        /// <summary>
        /// Add Evaluation ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddEvaluation(ManageGroups grp)
        {
            Panel.Children.Clear();
            AssignProjectUC eval = new AssignProjectUC();

            eval.lblTable.Content = "Evaluations: ";
            eval.lbldata.Content = ((DataRowView)eval.dgProjects.SelectedItem).Row.ItemArray[5].ToString();

            string query = @"SELECT * FROM Evaluation";
            Configuration.ShowData(eval.dgProjects, query);

            string gId = ((DataRowView)eval.dgProjects.SelectedItem).Row.ItemArray[0].ToString();
            eval.btnEnter.Command = new RelayCommands(execute => AddEval(eval, gId), canExecute => eval.dgProjects.SelectedItem != null);

            Panel.Children.Add(eval);
            address.Content = "Home -> Group Section -> View Groups -> Add Evaluation";
        }

        private void AddEval(AssignProjectUC eval,  string gId)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                //SqlCommand cmd = new SqlCommand(@"INSERT INTO GroupEvaluation VALUES (@GroupId, @EvaluationId, @ObtainedMarks, @EvaluationDate)", con);
                //cmd.Parameters.AddWithValue("@GroupId", gId);
                //cmd.Parameters.AddWithValue("@EvaluationId", );
                //cmd.Parameters.AddWithValue("@ObtainedMarks", 0);
                //cmd.Parameters.AddWithValue("@EvaluationDate", );
                //cmd.ExecuteNonQuery();

                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updating Group Students ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void UpdateGroupStudent(ManageGroups grp)
        {
            Panel.Children.Clear();
            AddGroupStudentUC grpS = new AddGroupStudentUC();

            var con = Configuration.getInstance().getConnection();
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT GS.StudentId, S.RegistrationNo, P.FirstName, P.LastName, P.Contact, Lk.Value, (SELECT Value FROM Lookup WHERE GS.Status = Lookup.Id) AS Status, GS.AssignmentDate
                                              FROM GroupStudent GS
                                              	JOIN Student S ON GS.StudentId = S.Id
                                              	JOIN Person P ON S.Id = P.Id
                                                JOIN Lookup Lk ON P.Gender = Lk.Id
                                              WHERE GS.StudentId = @GrpID", con);
            cmd.Parameters.AddWithValue("@GrpID", ((DataRowView)grp.dgTableData.SelectedItem).Row.ItemArray[0].ToString());
            Configuration.ShowData(grpS.dgStudents, cmd);

            grpS.btnEnter.Command = new RelayCommands(execute => UpdateGS(grpS), canExecute => grpS.dgStudents.SelectedItem != null);

            Panel.Children.Add(grpS);
            address.Content = "Home -> Group Section -> View Groups -> Update Group Students";
        }

        private void UpdateGS(AddGroupStudentUC grpS)
        {
            grpS.cbStatus.Text = ((DataRowView)grpS.dgStudents.SelectedItem).Row.ItemArray[6].ToString();
            grpS.txtdate.Text = ((DataRowView)grpS.dgStudents.SelectedItem).Row.ItemArray[7].ToString();

            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"Update GroupStudent SET Status = @Status, AssignmentDate = @AssignmentDate WHERE StudentId = @StudentId",con);
                cmd.Parameters.AddWithValue("@StudentId", ((DataRowView)grpS.dgStudents.SelectedItem).Row.ItemArray[0].ToString());
                cmd.Parameters.AddWithValue("@Status", grpS.cbStatus.SelectedIndex + 3);
                cmd.Parameters.AddWithValue("@AssignmentDate", grpS.txtdate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data updated successfully!!!", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        /// <summary>
        /// View Assigned Projects ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void UpdateAssignedProjects(ManageGroups grp)
        {
            Panel.Children.Clear();
            ViewData grpAp = new ViewData();
            grpAp.btnUpdate.Command = new RelayCommands(execute => UpdateAP(grpAp), canExecute => grpAp.lvTableData.SelectedItem != null);
            grpAp.btnDelete.Command = new RelayCommands(execute => DeleteAssignProject(grpAp), canExecute => grpAp.lvTableData.SelectedItem != null);

            // Data Source dena h 

            Panel.Children.Add(grpAp);
            address.Content = "Home -> Group Section -> View Assigned Projects";
        }

        private void UpdateAP(ViewData grp)
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

        /*

        /// <summary>
        /// Add Student in Group /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddGroupStudent(ManageGroups grp)
        {
            Panel.Children.Clear();
            UpdateGroupData grpS = new UpdateGroupData();

            var con = Configuration.getInstance().getConnection();
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT GS.StudentId, S.RegistrationNo, P.FirstName, P.LastName, P.Contact, Lk.Value 
                                              FROM GroupStudent GS
                                              	JOIN Student S ON GS.StudentId = S.Id
                                              	JOIN Person P ON S.Id = P.Id
                                                JOIN Lookup Lk ON P.Gender = Lk.Id
                                              WHERE GS.StudentId = @GrpID", con);
            cmd.Parameters.AddWithValue("@GrpID", ((DataRowView)grp.dgTableData.SelectedItem).Row.ItemArray[0].ToString());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grpS.dgSelect.ItemsSource = dt.DefaultView;

            string query = @"SELECT S.Id, S.RegistrationNo, P.FirstName, P.LastName, P.Contact, Lk.Value 
                                               FROM GroupStudent GS
                                                  RIGHT JOIN Student S ON GS.StudentId = S.Id
                                                  JOIN Person P ON S.Id = P.Id
                                                  JOIN Lookup Lk ON P.Gender = Lk.Id
                                               WHERE GS.GroupId IS NULL";
            Configuration.ShowData(grpS.dgUnselect, query);

            grpS.btnUpdate.Command = new RelayCommands(execute => UpdateGS(grpS, grp), canExecute => grpS.dgUnselect.SelectedItem != null);
            grpS.btnDelete.Command = new RelayCommands(execute => DeleteGS(grpS, grp), canExecute => grpS.dgSelect.SelectedItem != null);


            Panel.Children.Add(grp);
            address.Content = "Home -> Group Section -> Add Student in Group";
        }

        private void AddGS(AddGroupStudentUC grp)
        {
            if (canAddGS(grp))
            {
                if (addingGSInDb(grp))
                {
                    MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearGSData(grp);
                }
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

        private bool addingGSInDb(AddGroupStudentUC grp)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO GroupStudent VALUES (@GroupId, @StudentId, @Status, @AssignmentDate)", con);
                cmd.Parameters.AddWithValue("@GroupId", ((DataRowView)grp.dgGroups.SelectedItem).Row.ItemArray[].ToString());
                cmd.Parameters.AddWithValue("@StudentId", ((DataRowView)grp.dgGroups.SelectedItem).Row.ItemArray[].ToString());
                cmd.Parameters.AddWithValue("@Status", grp.cbStatus.SelectedIndex + 3);
                cmd.Parameters.AddWithValue("@AssignmentDate", eval.txtTitle.Text);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void clearGSData(AddGroupStudentUC grp)
        {
            grp.cbGroupID.Text = string.Empty;
            grp.cbStudentID.Text = string.Empty;
            grp.cbStatus.Text = string.Empty;
            grp.txtdate.Text = string.Empty;
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


        */
    }
}
