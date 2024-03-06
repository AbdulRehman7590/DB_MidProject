using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.Advisor;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Student;
using System.Collections.ObjectModel;
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
        /// Advisor Class List /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private ObservableCollection<AdvisorModel> advisorData;

        public ObservableCollection<AdvisorModel> AdvisorData
        {
            get { return advisorData; }
            set { advisorData = value; }
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
                addingAdvisorInDB(adv);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearData(adv);
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

        private void addingAdvisorInDB(AddAvisorUC adv)
        {

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
            
            // Data Source dena h 

            Panel.Children.Add(man);
            address.Content = "Home -> Advisor Section -> Manage Advisor";
        }

        private void UpdateAdvisor(ViewData man)
        {
            Panel.Children.Clear();
            AddAvisorUC adv = new AddAvisorUC();
            
            /*
            adv.txtFirstName.Text = man.lvAdvisors.SelectedItem.FirstName;
            adv.txtLastName.Text = man.lvAdvisors.SelectedItem.LastName;
            adv.txtEmail.Text = man.lvAdvisors.SelectedItem.Email;
            adv.txtContact.Text = man.lvAdvisors.SelectedItem.Contact;
            adv.txtDOB.Text = man.lvAdvisors.SelectedItem.DOB;
            adv.cbGender.Text = man.lvAdvisors.SelectedItem.Gender;
            adv.cbDesignation.Text = man.lvAdvisors.SelectedItem.Designation;
            adv.txtSalary.Text = man.lvAdvisors.SelectedItem.Salary;
            */

            adv.btnEnter.Content = "Update Student";
            adv.btnEnter.Command = new RelayCommands(execute => UpdateA(adv));
            Panel.Children.Add(adv);
            address.Content = "Home -> Advisor Section -> Manage Advisors -> Update Advisor";
        }

        private void UpdateA(AddAvisorUC adv)
        {

        }

        private void DeleteAdvisor(ViewData man)
        {

        }
    }
}
