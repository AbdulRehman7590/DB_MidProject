using Mid_Project.Models;
using Mid_Project.MVVM;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Student;
using System.Collections.ObjectModel;
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
        /// Student Class List //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// </summary>
        private ObservableCollection<StudentModel> studentData;

        public ObservableCollection<StudentModel> StudentData
        {
            get { return studentData; }
            set { studentData = value; }
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
                addingInDB(stud);
                MessageBox.Show("Student Added Successfully", "Information!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                clearData(stud);
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
        
        private void addingInDB(AddStudentUC stud)
        {

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
            
            // Data Source dena h

            Panel.Children.Add(man);
            address.Content = "Home -> Student Section -> Manage Students";
        }

        private void UpdateStudent(ViewData man)
        {
            Panel.Children.Clear();
            AddStudentUC stud = new AddStudentUC();
            
            /*
            stud.txtFirstName.Text = man.lvStudents.SelectedItem.FirstName;
            stud.txtLastName.Text = man.lvStudents.SelectedItem.LastName;
            stud.txtEmail.Text = man.lvStudents.SelectedItem.Email;
            stud.txtContact.Text = man.lvStudents.SelectedItem.Contact;
            stud.txtDOB.Text = man.lvStudents.SelectedItem.DOB;
            stud.txtRegNo.Text = man.lvStudents.SelectedItem.RegNo;
            stud.cbGender.Text = man.lvStudents.SelectedItem.Gender;
            */
            
            stud.btnEnter.Content = "Update Student";
            stud.btnEnter.Command = new RelayCommands(execute => UpdateS(stud));
            Panel.Children.Add(stud);
            address.Content = "Home -> Student Section -> Manage Students -> Update Student";
        }

        private void UpdateS(AddStudentUC stud)
        {
            
        }

        private void DeleteStudent(ViewData man)
        {
            
        }

    }
}
