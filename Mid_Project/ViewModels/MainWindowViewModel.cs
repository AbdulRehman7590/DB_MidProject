using Mid_Project.MVVM;
using Mid_Project.Views;
using Mid_Project.Views.Advisor;
using Mid_Project.Views.CommonUCs;
using Mid_Project.Views.Evaluation;
using Mid_Project.Views.Group;
using Mid_Project.Views.Project;
using Mid_Project.Views.Student;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Mid_Project.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly Grid Panel;
        private readonly Label address;
        
        public MainWindowViewModel(Grid panel, Label address)
        {
            this.Panel = panel;
            this.address = address;
            StartClock();   
        }

        /// <summary>
        /// Button Connections /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands studentSection => new RelayCommands(execute => StudentSection());
        public RelayCommands advisorSection => new RelayCommands(execute => AdvisorSection());
        public RelayCommands projectSection => new RelayCommands(execute => ProjectSection());
        public RelayCommands groupSection => new RelayCommands(execute => GroupSection());
        public RelayCommands evaluationSection => new RelayCommands(execute => EvaluationSection());
        public RelayCommands reportSection => new RelayCommands(execute => ReportSection());
        public RelayCommands goBack => new RelayCommands(execute => GoBack(), canExecute => Panel.Children.Count != 0);
        public RelayCommands logOut => new RelayCommands(execute => Application.Current.Shutdown());


        /// <summary>
        ///  Clock Working /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        }
    
        
        /// <summary>
        /// Button Functionalities /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        
        // Student Section
        private void StudentSection()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new studentUC(Panel, address));
            address.Content = "Home -> Student Section";
        }

        // Advisor Section
        private void AdvisorSection()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new advisorUC(Panel, address));
            address.Content = "Home -> Advisor Section";
        }

        // Project Section
        private void ProjectSection()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new projectUC(Panel, address));
            address.Content = "Home -> Project Section";
        }

        // Group Section
        private void GroupSection()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new groupUC(Panel, address));
            address.Content = "Home -> Group Section";
        }

        // Evaluation Section
        private void EvaluationSection()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new evaluationUC(Panel, address));
            address.Content = "Home -> Evaluation Section";
        }
        
        // Report Section
        private void ReportSection()
        {
            Panel.Children.Clear();
            Panel.Children.Add(new reportUC(Panel, address));
            address.Content = "Home -> Report Section";
        }

        // Go Back
        private void GoBack()
        {
            foreach (var child in Panel.Children)
            {
                if (child is AddStudentUC)
                {
                    StudentSection();
                    return;
                }
                else if (child is AddAvisorUC)
                {
                    AdvisorSection();
                    return;
                }
                else if (child is AddUpdateUC || child is MarkUpdateUC)
                {
                    EvaluationSection();
                    return;
                }
                else if (child is AddGroupUC || child is AssignProjectUC || child is ManageGroups || child is UpdateGroupData)
                {
                    GroupSection();
                    return;
                }
                else if (child is AddProjectUC || child is AddProjectAdvisorUC)
                {
                    ProjectSection();
                    return;
                }
            }

            Panel.Children.Clear();
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("Images/fyp.png", UriKind.RelativeOrAbsolute));
            Panel.Children.Add(img);

            address.Content = "Home";
        }

    }
}
