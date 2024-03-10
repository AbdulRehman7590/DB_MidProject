using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Evaluation;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class EvaluationViewModel
    {
        private readonly Grid Panel;
        private readonly Label address;

        public EvaluationViewModel(Grid panel, Label address)
        {
            Panel = panel;
            this.address = address;
        }


        /// <summary>
        /// Relay Commands ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands addEvaluation => new RelayCommands(execute => AddEvaluation());
        public RelayCommands viewEvaluation => new RelayCommands(execute => ViewEvaluation());
        public RelayCommands markEvaluation => new RelayCommands(execute => MarkEvaluation());
        public RelayCommands viewMarkEvaluation => new RelayCommands(execute => ViewMarkEvaluation());


        /// <summary>
        /// Add Evaluation Handling ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void AddEvaluation()
        {
            Panel.Children.Clear();
            AddUpdateUC eval = new AddUpdateUC();

            eval.btnEnter.Content = "Add Evaluation";
            eval.btnEnter.Command = new RelayCommands(execute => AddEval(eval));
            
            Panel.Children.Add(eval);
            address.Content = "Home -> Evaluation Section -> Add Evaluation";
        }

        private void AddEval(AddUpdateUC eval)
        {
            if (canAddEval(eval))
            {
                if (addingEvaluationInDb(eval))
                {
                    MessageBox.Show("Evaluation Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearAddData(eval);
                }
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddEval(AddUpdateUC eval)
        {
            if (string.IsNullOrEmpty(eval.txtEvaluationName.Text) || string.IsNullOrEmpty(eval.txtMarks.Text) || string.IsNullOrEmpty(eval.txtWeightage.Text))
                return false;
            else
                return true;
        }

        private bool addingEvaluationInDb(AddUpdateUC eval)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Evaluation VALUES (@Name, @TotalMarks, @TotalWeightage)", con);
                    cmd.Parameters.AddWithValue("@Name", eval.txtEvaluationName.Text);
                    cmd.Parameters.AddWithValue("@TotalMarks", eval.txtMarks.Text);
                    cmd.Parameters.AddWithValue("@TotalWeightage", eval.txtWeightage.Text);
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

        private void clearAddData(AddUpdateUC eval)
        {
            eval.txtEvaluationName.Text = string.Empty;
            eval.txtMarks.Text = string.Empty;
            eval.txtWeightage.Text = string.Empty;
        }


        /// <summary>
        /// View Evaluation Handling ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewEvaluation()
        {
            Panel.Children.Clear();
            ViewData eval = new ViewData();
            eval.btnUpdate.Command = new RelayCommands(execute => UpdateEvaluation(eval), canExecute => eval.lvTableData.SelectedItem != null);
            eval.btnDelete.Command = new RelayCommands(execute => DeleteEvaluation(eval), canExecute => eval.lvTableData.SelectedItem != null);

            string query = @"SELECT E.Id, E.Name, E.TotalMarks, E.TotalWeightage FROM Evaluation E WHERE E.Name NOT LIKE '!!%'";
            Configuration.ShowData(eval.lvTableData, query);

            Panel.Children.Add(eval);
            address.Content = "Home -> Evaluation Section -> View Evaluations";
        }

        private void UpdateEvaluation(ViewData eval)
        {
            Panel.Children.Clear();
            AddUpdateUC upd = new AddUpdateUC();

            upd.txtEvaluationName.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[1].ToString();
            upd.txtMarks.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[2].ToString();
            upd.txtWeightage.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row.ItemArray[3].ToString();

            upd.btnEnter.Content = "Update Evaluation";
            upd.btnEnter.Command = new RelayCommands(execute => UpdateE(upd, ((DataRowView)eval.lvTableData.SelectedItem).Row["Id"].ToString()));
            
            Panel.Children.Add(upd);
            address.Content = "Home -> Evaluation Section -> View Evaluations -> Update Evaluation";
        }

        private void UpdateE(AddUpdateUC upd, string id)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE Evaluation SET Name = @Name, TotalMarks = @TotalMarks, TotalWeightage = @TotalWeightage WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", upd.txtEvaluationName.Text);
                    cmd.Parameters.AddWithValue("@TotalMarks", upd.txtMarks.Text);
                    cmd.Parameters.AddWithValue("@TotalWeightage", upd.txtWeightage.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Evaluation Updated Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearAddData(upd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ViewEvaluation();
            }
        }

        private void DeleteEvaluation(ViewData eval)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE Evaluation SET Name = @Name WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", ((DataRowView)eval.lvTableData.SelectedItem).Row["Id"]);
                    cmd.Parameters.AddWithValue("@Name", "!!" + ((DataRowView)eval.lvTableData.SelectedItem).Row["Name"]);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Evaluation Updated Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ViewEvaluation();
            }
        }



        /// <summary>
        /// Mark Evaluation Handling ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void MarkEvaluation()
        {
            Panel.Children.Clear();
            ViewData markE = new ViewData();

            string query = @"SELECT G.Id AS GroupID, Prj.Id AS ProjectID, Prj.Title, E.Id AS EvaluationID, E.Name AS EvaluationName, E.TotalMarks, E.TotalWeightage
                             FROM [Group] G
                             	JOIN GroupEvaluation GE ON G.Id = GE.GroupId
                             	JOIN Evaluation E ON GE.EvaluationId = E.Id
                             	JOIN GroupProject GP ON G.Id = GP.GroupId
                             	JOIN Project Prj ON GP.ProjectId = Prj.Id
                             WHERE GE.ObtainedMarks = 0";
            Configuration.ShowData(markE.lvTableData, query);

            markE.btnDelete.Visibility = Visibility.Hidden;
            markE.btnUpdate.HorizontalAlignment = HorizontalAlignment.Right;
            markE.btnUpdate.Content = "Mark";
            markE.btnUpdate.Command = new RelayCommands(execute => MarkEval(markE), canExecute => markE.lvTableData.SelectedItem != null);

            Panel.Children.Add(markE);
            address.Content = "Home -> Evaluation Section -> Mark Evaluation";
        }

        private void MarkEval(ViewData eval)
        {
            Panel.Children.Clear();
            MarkUpdateUC mark = new MarkUpdateUC();

            mark.tbGroupID.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["GroupID"].ToString();
            mark.tbProjID.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["ProjectID"].ToString();
            mark.tbProjTitle.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["Title"].ToString();
            mark.tbevalID.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["EvaluationID"].ToString();
            mark.tbEvalTitle.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["EvaluationName"].ToString();

            mark.btnEnter.Content = "Mark Evaluation";
            mark.btnEnter.Command = new RelayCommands(execute => AddMarkEval(mark), canExecute => mark.txtObMarks.Text != null && mark.tbProjID.Text != null);

            Panel.Children.Add(mark);
            address.Content = "Home -> Evaluation Section -> Mark Evaluation";
        }

        private void AddMarkEval(MarkUpdateUC eval)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE GroupEvaluation SET ObtainedMarks = @ObtainedMarks, EvaluationDate = @EvaluationDate WHERE EvaluationId = @EvaluationId AND GroupId = @GroupId", con);
                    cmd.Parameters.AddWithValue("@GroupId", eval.tbGroupID.Text);
                    cmd.Parameters.AddWithValue("@EvaluationId", eval.tbevalID.Text);
                    cmd.Parameters.AddWithValue("@ObtainedMarks", eval.txtObMarks.Text);
                    cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Data Updated Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                clearEvalData(eval);
            }
        }

        private void clearEvalData(MarkUpdateUC eval)
        {
            eval.tbevalID.Text = string.Empty;
            eval.tbEvalTitle.Text = string.Empty;
            eval.tbGroupID.Text = string.Empty;
            eval.tbProjID.Text = string.Empty;
            eval.tbProjTitle.Text = string.Empty;
            eval.txtObMarks.Text = string.Empty;
        }

        /// <summary>
        /// View AND Update Mark Evaluation Handling //////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewMarkEvaluation()
        {
            Panel.Children.Clear();
            ViewData markE = new ViewData();

            string query = @"SELECT G.Id AS GroupID, Prj.Id AS ProjectID, Prj.Title, E.Id AS EvaluationID, E.Name AS EvaluationName, GE.ObtainedMarks, E.TotalMarks, E.TotalWeightage
                             FROM [Group] G
                             	JOIN GroupEvaluation GE ON G.Id = GE.GroupId
                             	JOIN Evaluation E ON GE.EvaluationId = E.Id
                             	JOIN GroupProject GP ON G.Id = GP.GroupId
                             	JOIN Project Prj ON GP.ProjectId = Prj.Id
                             WHERE GE.ObtainedMarks <> 0";
            Configuration.ShowData(markE.lvTableData, query);

            markE.btnDelete.Visibility = Visibility.Hidden;
            markE.btnUpdate.HorizontalAlignment = HorizontalAlignment.Right;
            
            markE.btnUpdate.Command = new RelayCommands(execute => UpdateMarkEval(markE), canExecute => markE.lvTableData.SelectedItem != null);

            Panel.Children.Add(markE);
            address.Content = "Home -> Evaluation Section -> View Mark Evaluations";
        }

        private void UpdateMarkEval(ViewData eval)
        {
            Panel.Children.Clear();
            MarkUpdateUC mark = new MarkUpdateUC();

            mark.tbGroupID.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["GroupID"].ToString();
            mark.tbProjID.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["ProjectID"].ToString();
            mark.tbProjTitle.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["Title"].ToString();
            mark.tbevalID.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["EvaluationID"].ToString();
            mark.tbEvalTitle.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["EvaluationName"].ToString();
            mark.txtObMarks.Text = ((DataRowView)eval.lvTableData.SelectedItem).Row["ObtainedMarks"].ToString();

            mark.btnEnter.Content = "Update Evaluation";
            mark.btnEnter.Command = new RelayCommands(execute => AddMarkEval(mark));

            Panel.Children.Add(mark);
            address.Content = "Home -> Evaluation Section -> View Mark Evaluations -> Update";
        }

    }
}
