using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.Advisor;
using Mid_Project.Views.CommonUCs;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class AdvisorViewModel
    {
        private readonly Grid Panel;
        private readonly Label address;

        public AdvisorViewModel(Grid panel, Label address)
        {
            this.Panel = panel;
            this.address = address;
        }

        
        /// <summary>
        /// Relay Commands ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands advisor => new RelayCommands(execute => Advisor());
        public RelayCommands manageAdvisor => new RelayCommands(execute => Manage());

        
        /// <summary>
        /// Advisor add Handling ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Advisor()
        {
            Panel.Children.Clear();
            AddAvisorUC adv = new AddAvisorUC();
            
            adv.btnEnter.Content = "Add Advisor";
            adv.btnEnter.Command = new RelayCommands(execute => AddAdvisor(adv));
            
            Panel.Children.Add(adv);
            address.Content = "Home -> Advisor Section -> Add Advisor";
        }

        private void AddAdvisor(AddAvisorUC adv)
        {
            if (canAddAdvisor(adv))
            {
                if (addingAdvisorInDB(adv))
                {
                    MessageBox.Show("Advisor Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData(adv);
                }
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddAdvisor(AddAvisorUC adv)
        {
            if (string.IsNullOrEmpty(adv.txtFirstName.Text) || string.IsNullOrEmpty(adv.txtLastName.Text) || string.IsNullOrEmpty(adv.txtContact.Text) || string.IsNullOrEmpty(adv.txtDOB.Text) || string.IsNullOrEmpty(adv.txtEmail.Text) || string.IsNullOrEmpty(adv.txtSalary.Text) || string.IsNullOrEmpty(adv.cbDesignation.Text) || string.IsNullOrEmpty(adv.cbGender.Text))
                return false;
            else
                return true;
        }

        private bool addingAdvisorInDB(AddAvisorUC adv)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"BEGIN TRANSACTION
                                                  INSERT INTO Person VALUES (@FirstName, @LastName, @Contact, @Email, @DateOfBirth, @Gender)
                                                  DECLARE @ID INT = scope_identity();
                                                  INSERT INTO Advisor VALUES (@ID, @Designation, @Salary)
                                                  COMMIT TRANSACTION", con);

                cmd.Parameters.AddWithValue("@FirstName", adv.txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", adv.txtLastName.Text);
                cmd.Parameters.AddWithValue("@Contact", adv.txtContact.Text);
                cmd.Parameters.AddWithValue("@Email", adv.txtEmail.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(adv.txtDOB.SelectedDate.Value.ToString("yyyy-MM-dd")));
                cmd.Parameters.AddWithValue("@Gender", adv.cbGender.SelectedIndex + 1);
                cmd.Parameters.AddWithValue("@designation", adv.cbDesignation.SelectedIndex + 6);
                cmd.Parameters.AddWithValue("@Salary", adv.txtSalary.Text);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                con?.Close();
            }
        }

        private void clearData(AddAvisorUC adv)
        {
            adv.txtFirstName.Text = string.Empty;
            adv.txtLastName.Text = string.Empty;
            adv.txtContact.Text = string.Empty;
            adv.txtEmail.Text = string.Empty;
            adv.txtDOB.Text = string.Empty;
            adv.txtSalary.Text = string.Empty;
            adv.cbGender.Text = string.Empty;
            adv.cbDesignation.Text = string.Empty;
        }


        /// <summary>
        /// Manage Advisor ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Manage()
        {
            Panel.Children.Clear();
            ViewData man = new ViewData();
            man.btnUpdate.Command = new RelayCommands(execute => UpdateAdvisor(man), canExecute => man.lvTableData.SelectedItem != null);
            man.btnDelete.Command = new RelayCommands(execute => DeleteAdvisor(man), canExecute => man.lvTableData.SelectedItem != null);

            string query = @"SELECT Ad.Id, P.FirstName, P.LastName, Lk.Value AS Designation, Ad.Salary, P.Contact, P.Email, P.DateOfBirth, 
                                (SELECT Value FROM Lookup L WHERE P.Gender = L.Id AND L.Category = 'Gender') AS Gender
                            FROM Advisor Ad 
                            	JOIN Lookup Lk ON Ad.Designation = Lk.Id AND Lk.Category = 'Designation'
                            	JOIN Person P ON Ad.Id = P.Id
                            WHERE P.FirstName NOT LIKE '!!%'";
            Configuration.ShowData(man.lvTableData, query);

            Panel.Children.Add(man);
            address.Content = "Home -> Advisor Section -> Manage Advisor";
        }

        private void UpdateAdvisor(ViewData man)
        {
            Panel.Children.Clear();
            AddAvisorUC adv = new AddAvisorUC();

            adv.txtFirstName.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[1].ToString();
            adv.txtLastName.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[2].ToString();
            adv.cbDesignation.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString();
            adv.txtSalary.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[4].ToString();
            adv.txtContact.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[5].ToString();
            adv.txtEmail.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[6].ToString();
            adv.txtDOB.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[7].ToString();
            adv.cbGender.Text = ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[8].ToString();
            
            adv.btnEnter.Content = "Update Advisor";
            adv.btnEnter.Command = new RelayCommands(execute => UpdateA(adv,man));
            Panel.Children.Add(adv);
            address.Content = "Home -> Advisor Section -> Manage Advisors -> Update Advisor";
        }

        private void UpdateA(AddAvisorUC adv,ViewData man)
        {
            var con = Configuration.getInstance().getConnection();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand(@"BEGIN TRANSACTION
                                                  UPDATE Advisor SET Designation = @Designation, Salary = @Salary WHERE Id = @Id;
                                                  UPDATE Person SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, 
                                                  Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender WHERE Id = @Id
                                                  COMMIT TRANSACTION", con);
                
                cmd.Parameters.AddWithValue("@Designation", adv.cbDesignation.SelectedIndex + 6);
                cmd.Parameters.AddWithValue("@Salary", adv.txtSalary.Text);
                cmd.Parameters.AddWithValue("@Id", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                cmd.Parameters.AddWithValue("@FirstName", adv.txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", adv.txtLastName.Text);
                cmd.Parameters.AddWithValue("@Contact", adv.txtContact.Text);
                cmd.Parameters.AddWithValue("@Email", adv.txtEmail.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", adv.txtDOB.SelectedDate.Value);
                cmd.Parameters.AddWithValue("@Gender", adv.cbGender.SelectedIndex + 1);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data Updated Successfully!!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                con?.Close();
                GobackToView();
            }
        }

        private void DeleteAdvisor(ViewData man)
        {
            if (!string.IsNullOrEmpty(((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString()))
            {
                var con = Configuration.getInstance().getConnection();
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand(@"BEGIN TRANSACTION
                                                  UPDATE Advisor SET Designation = @Designation, Salary = @Salary WHERE Id = @Id;
                                                  UPDATE Person SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, 
                                                  Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender WHERE Id = @Id
                                                  COMMIT TRANSACTION"
                    , con);

                    cmd.Parameters.AddWithValue("@Id", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[0].ToString());
                    cmd.Parameters.AddWithValue("@FirstName", "!!"+((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[1].ToString());
                    cmd.Parameters.AddWithValue("@LastName", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[2].ToString());
                    
                    if (((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString() == "Professor")
                        cmd.Parameters.AddWithValue("@Designation", 6);
                    else if (((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString() == "Associate Professor")
                        cmd.Parameters.AddWithValue("@Designation", 7);
                    else if (((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString() == "Assistant Professor")
                        cmd.Parameters.AddWithValue("@Designation", 8);
                    else if (((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[3].ToString() == "Lecturer")
                        cmd.Parameters.AddWithValue("@Designation", 9);
                    else
                        cmd.Parameters.AddWithValue("@Designation", 10);

                    cmd.Parameters.AddWithValue("@Salary", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[4].ToString());
                    cmd.Parameters.AddWithValue("@Contact", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[5].ToString());
                    cmd.Parameters.AddWithValue("@Email", ((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[6].ToString());
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[7].ToString()));
                    
                    if (((DataRowView)man.lvTableData.SelectedItem).Row.ItemArray[8].ToString() == "Male")
                        cmd.Parameters.AddWithValue("@Gender", 1);
                    else
                        cmd.Parameters.AddWithValue("@Gender", 2);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Deleted Successfully!!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    con?.Close();
                    GobackToView();
                }
            }
            else
                MessageBox.Show("Select Valid row!", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void GobackToView()
        {
            Panel.Children.Clear();
            Manage();
        }

    }
}
