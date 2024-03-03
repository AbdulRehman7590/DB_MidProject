using Mid_Project.MVVM;
using Mid_Project.Views.Student;
using System.Windows;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class StudentViewModel
    {
        private readonly WrapPanel Panel;
        private readonly Label address;
        AddStudentUC stud;

        public StudentViewModel(WrapPanel panel, Label address)
        {
            this.Panel = panel;
            this.address = address;
        }


        /// <summary>
        /// Relay commands ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands addStudent => new RelayCommands(execute => Student());
        public RelayCommands manageStudents => new RelayCommands(execute => ManageStudents());
        
        
        /// <summary>
        /// Student Add Handling /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Student()
        {
            Panel.Children.Clear();
            stud = new AddStudentUC();
            stud.btnEnter.Content = "Add Student";
            stud.btnEnter.Command = new RelayCommands(execute => AddStudent());
            Panel.Children.Add(stud);
            address.Content = "Home -> Student Section -> Add Student";
        }
        
        private void AddStudent()
        {
            if (canAddStudent())
            {
                addingInDB();
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearData();
            }
            else
                MessageBox.Show("Please fill all the fields", "Error!!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool canAddStudent()
        {
            if (string.IsNullOrEmpty(stud.txtFirstName.Text) || string.IsNullOrEmpty(stud.txtLastName.Text) || string.IsNullOrEmpty(stud.txtContact.Text) || string.IsNullOrEmpty(stud.txtDOB.Text) || string.IsNullOrEmpty(stud.txtEmail.Text) || string.IsNullOrEmpty(stud.txtRegNo.Text) || string.IsNullOrEmpty(stud.cbGender.Text))
                return false;
            else
                return true;
        }
        
        private void addingInDB()
        {

        }
        
        private void clearData()
        {
            stud.txtFirstName.Text = "";
            stud.txtLastName.Text = "";
            stud.txtContact.Text = "";
            stud.txtEmail.Text = "";
            stud.txtDOB.Text = "";
            stud.cbGender.Text = "";
            stud.txtRegNo.Text = "";
        }


        /// <summary>
        ///  Managing Students //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void ManageStudents()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new ManageStudentsUC());
            address.Content = "Home -> Student Section -> Manage Students";
        }

        private void UpdateStudent()
        {
            Panel.Children.Clear();
            AddStudentUC stud = new AddStudentUC();
            stud.btnEnter.Content = "Update Student";
            Panel.Children.Add(stud);
            address.Content = "Home -> Student Section -> Update Student";
        }

        private void DeleteStudent()
        {
            
        }

    }
}
