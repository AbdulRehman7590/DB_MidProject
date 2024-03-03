using Mid_Project.MVVM;
using Mid_Project.Views.Student;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class StudentViewModel
    {
        private readonly WrapPanel Panel;
        private readonly Label address;

        public StudentViewModel(WrapPanel panel, Label address)
        {
            this.Panel = panel;
            this.address = address;
        }

        public RelayCommands addStudent => new RelayCommands(execute => Student());
        
        private void Student()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new AddStudentUC());
            address.Content = "Home -> Student Section -> Add Student";
        }
        
        private void AddStudent()
        {
            
        }


        public RelayCommands manageStudents => new RelayCommands(execute => ManageStudents());

        private void ManageStudents()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new ManageStudentsUC());
            address.Content = "Home -> Student Section -> Manage Students";
        }

        private void UpdateStudent()
        {
            
        }

        private void DeleteStudent()
        {
            
        }

    }
}
