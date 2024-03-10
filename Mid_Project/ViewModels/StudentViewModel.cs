using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Student;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class StudentViewModel
    {
        private readonly Grid Panel;
        private readonly Label address;
        
        public StudentViewModel(Grid panel, Label address)
        {
            this.Panel = panel;
            this.address = address;
        }


        /// <summary>
        /// Relay commands ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands addStudent => new RelayCommands(execute => Student());
        public RelayCommands manageStudents => new RelayCommands(execute => ManageStudents());
        public RelayCommands marksSheet => new RelayCommands(execute => MarksSheet());


        /// <summary>
        /// Student Add Handling /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Student()
        {
            Panel.Children.Clear();
            AddStudentUC stud = new AddStudentUC();
            
            stud.btnEnter.Content = "Add Student";
            stud.btnEnter.Command = new RelayCommands(execute => AddStudent(stud));
            
            Panel.Children.Add(stud);
            address.Content = "Home -> Student Section -> Add Student";
        }
        
        private void AddStudent(AddStudentUC stud)
        {
            if (canAddStudent(stud))
            {
                if (addingInDB(stud))
                {
                    MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData(stud);
                }
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddStudent(AddStudentUC stud)
        {
            if (string.IsNullOrEmpty(stud.txtFirstName.Text) || string.IsNullOrEmpty(stud.txtLastName.Text) || string.IsNullOrEmpty(stud.txtContact.Text) || string.IsNullOrEmpty(stud.txtDOB.Text) || string.IsNullOrEmpty(stud.txtEmail.Text) || string.IsNullOrEmpty(stud.txtRegNo.Text) || string.IsNullOrEmpty(stud.cbGender.Text))
                return false;
            else
                return true;
        }
        
        private bool addingInDB(AddStudentUC stud)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"BEGIN TRANSACTION 
                                                  INSERT INTO Person values (@FirstName, @LastName, @Contact, @Email, @DateOfBirth, @Gender)
                                                  DECLARE @ID INT = scope_identity();
                                                  INSERT INTO Student VALUES (@ID,@RegistrationNo)
                                                  COMMIT TRANSACTION", con);

                    cmd.Parameters.AddWithValue("@FirstName", stud.txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", stud.txtLastName.Text);
                    cmd.Parameters.AddWithValue("@Contact", stud.txtContact.Text);
                    cmd.Parameters.AddWithValue("@Email", stud.txtEmail.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(stud.txtDOB.SelectedDate.Value.ToString("yyyy-MM-dd")));
                    cmd.Parameters.AddWithValue("@Gender", stud.cbGender.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@RegistrationNo", stud.txtRegNo.Text);
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

        private void clearData(AddStudentUC stud)
        {
            stud.txtFirstName.Text = string.Empty;
            stud.txtLastName.Text = string.Empty;
            stud.txtContact.Text = string.Empty;
            stud.txtEmail.Text = string.Empty;
            stud.txtDOB.Text = string.Empty;
            stud.cbGender.Text = string.Empty;
            stud.txtRegNo.Text = string.Empty;
        }

        /// <summary>
        /// Marks Sheet of Students //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void MarksSheet()
        {
            string query = @"SELECT G.Id as [Group Id], S.RegistrationNo AS [Reg. No.], PR.FirstName, PR.LastName, P.Title AS [Project Title], E.TotalMarks, GE.ObtainedMarks 
                             FROM GroupEvaluation GE 
                                JOIN Evaluation E ON GE.EvaluationId = E.Id  
                                JOIN [Group] G ON G.Id = GE.GroupId 
                                JOIN GroupStudent GS ON GS.GroupId = G.Id AND GS.Status = 3 
                                JOIN Student S ON S.Id = GS.StudentId   
                                JOIN GroupProject GP ON GP.GroupId = GS.GroupId 
                                JOIN Project P ON P.Id = GP.ProjectId 
                                JOIN Person PR ON PR.Id = S.Id
                             WHERE PR.FirstName NOT LIKE '!!%'";
            PDFGenerator.GeneratePDF(query, "MarksSheet");
        }


        /// <summary>
        ///  Managing Students //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ManageStudents()
        {
            Panel.Children.Clear();
            ViewData man = new ViewData();
            man.btnUpdate.Command = new RelayCommands(execute => UpdateStudent(man), canExecute => man.lvTableData.SelectedItem != null);
            man.btnDelete.Command = new RelayCommands(execute => DeleteStudent(man), canExecute => man.lvTableData.SelectedItem != null);

            string query = @"SELECT S.Id, RegistrationNo, FirstName, LastName, Contact, Email, DateOfBirth, Lk.Value AS Gender
                             FROM Student S 
                                JOIN Person P ON S.Id = P.Id
                                JOIN Lookup Lk ON P.Gender = Lk.Id
                             WHERE P.FirstName NOT LIKE '!!%'";
            Configuration.ShowData(man.lvTableData, query);

            Panel.Children.Add(man);
            address.Content = "Home -> Student Section -> Manage Students";
        }

        private void UpdateStudent(ViewData man)
        {
            Panel.Children.Clear();
            AddStudentUC stud = new AddStudentUC();

            stud.txtRegNo.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[1].ToString();
            stud.txtFirstName.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[2].ToString();
            stud.txtLastName.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString();
            stud.txtContact.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[4].ToString();
            stud.txtEmail.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[5].ToString(); ;
            stud.txtDOB.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[6].ToString();
            stud.cbGender.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[7].ToString();
            
            stud.btnEnter.Content = "Update Student";
            stud.btnEnter.Command = new RelayCommands(execute => UpdateS(man,stud));
            Panel.Children.Add(stud);
            address.Content = "Home -> Student Section -> Manage Students -> Update Student";
        }

        private void UpdateS(ViewData man, AddStudentUC stud)
        {
            using (var con = Configuration.getInstance().getConnection())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE Student SET RegistrationNo = @RegistrationNo WHERE Id = @Id;
                                                  UPDATE Person SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, 
                                                  Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender WHERE Id = @Id;", con, transaction);

                    cmd.Parameters.AddWithValue("@RegistrationNo", stud.txtRegNo.Text);
                    cmd.Parameters.AddWithValue("@Id", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                    cmd.Parameters.AddWithValue("@FirstName", stud.txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", stud.txtLastName.Text);
                    cmd.Parameters.AddWithValue("@Contact", stud.txtContact.Text);
                    cmd.Parameters.AddWithValue("@Email", stud.txtEmail.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(stud.txtDOB.SelectedDate.Value.ToString("yyyy-MM-dd")));
                    cmd.Parameters.AddWithValue("@Gender", stud.cbGender.SelectedIndex + 1);

                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                    MessageBox.Show("Updated Successfully!!!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    transaction?.Rollback();
                }
            }
            ManageStudents();            
        }

        private void DeleteStudent(ViewData man)
        {
            if (!string.IsNullOrEmpty(((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString()))
            {
                try
                {
                    using (var con = Configuration.getInstance().getConnection())
                    {
                        con.Open();

                        SqlCommand cmd = new SqlCommand(@"BEGIN TRANSACTION
                                                      UPDATE Person SET FirstName = @FirstName WHERE Id = @Id;
                                                      UPDATE GroupStudent SET Status = 4 WHERE StudentId = @Id;
                                                      COMMIT TRANSACTION", con);

                        cmd.Parameters.AddWithValue("@Id", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                        cmd.Parameters.AddWithValue("@FirstName", "!!" + ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[2].ToString());
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Deleted Successfully!!!", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ManageStudents();
                }
            }
            else
                MessageBox.Show("Select Valid row!", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

    }
}
