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
                    MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO Evaluation VALUES (@Name, @TotalMarks, @TotalWeightage)", con);
                cmd.Parameters.AddWithValue("@Name", eval.txtEvaluationName.Text);
                cmd.Parameters.AddWithValue("@TotalMarks", eval.txtMarks.Text);
                cmd.Parameters.AddWithValue("@TotalWeightage", eval.txtWeightage.Text);
                cmd.ExecuteNonQuery();

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

            string query = @"SELECT E.Id, E.Name, E.TotalMarks, E.TotalWeightage FROM Evaluation E";
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
            upd.btnEnter.Command = new RelayCommands(execute => UpdateE(upd));
            
            Panel.Children.Add(upd);
            address.Content = "Home -> Evaluation Section -> View Evaluations -> Update Evaluation";
        }

        private void UpdateE(AddUpdateUC upd)
        {

        }

        private void DeleteEvaluation(ViewData eval)
        {

        }


        /// <summary>
        /// Mark Evaluation Handling ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void MarkEvaluation()
        {
            Panel.Children.Clear();
            MarkUpdateUC markEval = new MarkUpdateUC();

            markEval.btnEnter.Command = new RelayCommands(execute => MarkEval(markEval));

            Panel.Children.Add(markEval);
            address.Content = "Home -> Evaluation Section -> Mark Evaluation";
        }

        private void MarkEval(MarkUpdateUC eval)
        {
            if (canMarkEval(eval))
            {
                addingMarkEvaluationInDb(eval);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearMarkData(eval);
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canMarkEval(MarkUpdateUC eval)
        {
            if (string.IsNullOrEmpty(eval.cbevalID.Text) || string.IsNullOrEmpty(eval.cbGroupID.Text) || string.IsNullOrEmpty(eval.txtObMarks.Text) || string.IsNullOrEmpty(eval.txtdate.Text))
                return false;
            else
                return true;
        }

        private void addingMarkEvaluationInDb(MarkUpdateUC eval)
        {

        }

        private void clearMarkData(MarkUpdateUC eval)
        {
            eval.cbevalID.Text = string.Empty;
            eval.cbGroupID.Text = string.Empty;
            eval.txtObMarks.Text = string.Empty;
            eval.txtdate.Text = string.Empty;
        }


        /// <summary>
        /// View Mark Evaluation Handling ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ViewMarkEvaluation()
        {
            Panel.Children.Clear();
            ViewData eval = new ViewData();
            eval.btnUpdate.Command = new RelayCommands(execute => UpdateMarkEvaluation(eval), canExecute => eval.lvTableData.SelectedItem != null);
            eval.btnDelete.Command = new RelayCommands(execute => DeleteMarkEvaluation(eval), canExecute => eval.lvTableData.SelectedItem != null);

            // Data Source dena h 

            Panel.Children.Add(eval);
            address.Content = "Home -> Evaluation Section -> View Mark Evaluations";
        }

        private void UpdateMarkEvaluation(ViewData eval)
        {
            Panel.Children.Clear();
            MarkUpdateUC upd = new MarkUpdateUC();
            
            /*
            upd.txtevalID.Text = eval.lvTableData.SelectedItem.EvaluationID;
            upd.txtGroupID.Text = eval.lvTableData.SelectedItem.GroupID;
            upd.txtObMarks.Text = eval.lvTableData.SelectedItem.ObtainedMarks;
            upd.txtdate.Text = eval.lvTableData.SelectedItem.EvaluationDate;
            */
            
            upd.btnEnter.Content = "Update Project";
            upd.btnEnter.Command = new RelayCommands(execute => UpdateMarkE(upd));
            Panel.Children.Add(upd);
            address.Content = "Home -> Evaluation Section -> View Mark Evaluations -> Update Mark Evaluations";
        }

        private void UpdateMarkE(MarkUpdateUC upd)
        {

        }

        private void DeleteMarkEvaluation(ViewData eval)
        {
            
        }
    }
}
