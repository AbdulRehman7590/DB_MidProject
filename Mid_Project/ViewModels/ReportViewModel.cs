using Mid_Project.Models;
using Mid_Project.MVVM;
using System.Windows.Controls;

namespace Mid_Project.ViewModels
{
    internal class ReportViewModel
    {
        private readonly Grid Panel;
        private readonly Label address;

        public ReportViewModel(Grid panel, Label address)
        {
            Panel = panel;
            this.address = address;
        }


        /// <summary>
        /// Relay Commands ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public RelayCommands report1 => new RelayCommands(execute => Report1());
        public RelayCommands report2 => new RelayCommands(execute => Report2());
        public RelayCommands report3 => new RelayCommands(execute => Report3());
        public RelayCommands report4 => new RelayCommands(execute => Report4());
        public RelayCommands report5 => new RelayCommands(execute => Report5());



        /// <summary>
        /// Report No. 1 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report1()
        {
            string query = @"SELECT PA.ProjectId AS PID, 
                                 MAX(P.Title) AS Title, 
                                 MAX(CASE WHEN PA.AdvisorRole = 11 THEN A.Id END) AS MainAdvisorId,
                                 MAX(CASE WHEN PA.AdvisorRole = 11 THEN CONCAT(Pers.FirstName, ' ', Pers.LastName) END) AS MainAdvisor,
                                 MAX(CASE WHEN PA.AdvisorRole = 12 THEN A.Id END) AS CoAdvisorId, 
                                 MAX(CASE WHEN PA.AdvisorRole = 12 THEN CONCAT(Pers.FirstName, ' ', Pers.LastName) END) AS CoAdvisor, 
                                 MAX(CASE WHEN PA.AdvisorRole = 14 THEN A.Id END) AS IndustryAdvisorId,
                                 MAX(CASE WHEN PA.AdvisorRole = 14 THEN CONCAT(Pers.FirstName, ' ', Pers.LastName) END) AS IndustryAdvisor
                             FROM ProjectAdvisor PA 
                                 INNER JOIN Advisor A ON PA.AdvisorId = A.Id 
                                 JOIN Project P ON P.Id = PA.ProjectId 
                                 JOIN Person Pers ON Pers.Id = A.Id
                             WHERE Pers.FirstName NOT LIKE '!!%'
                             GROUP BY PA.ProjectId";
            PDFGenerator.GeneratePDF(query, "Project Advisors");
        }
        

        /// <summary>
        /// Report No. 2 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report2()
        {
            string query = @"SELECT A.Id, P.FirstName, P.LastName, L.Value AS [Designation], P.Email, P.Contact , A.Salary 
                           FROM Advisor A 
                              JOIN Person P ON A.Id = P.Id 
                              JOIN Lookup L on A.Designation = L.Id
                           WHERE P.FirstName NOT LIKE '!!%'";
            PDFGenerator.GeneratePDF(query, "Advisor Details");
        }


        /// <summary>
        /// Report No. 3 /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report3()
        {
            string query = @"SELECT S.RegistrationNo, P.FirstName, P.LastName, L.Value AS [Gender], P.Contact, P.Email, P.DateOfBirth 
                             FROM Student S 
                                JOIN Person P ON S.Id = P.Id 
                                JOIN Lookup L on P.Gender = L.Id
                             WHERE P.FirstName NOT LIKE '!!%'";
            PDFGenerator.GeneratePDF(query, "Student Details");
        }


        /// <summary>
        /// Report No. 4 ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report4()
        {
            string query = @"SELECT Prj.id AS ProjectID, Prj.Title, G.Id AS GroupID, S.RegistrationNo, P.FirstName, P.LastName, P.Contact, (SELECT Value FROM Lookup L WHERE P.Gender = L.Id) AS Gender
                             FROM Project Prj
                             	JOIN GroupProject GP ON GP.GroupId = Prj.Id
                             	JOIN [Group] G ON GP.GroupId = G.Id
                             	JOIN GroupStudent GS ON GS.GroupId =G.Id
                             	JOIN Student S ON GS.StudentId = S.Id
                             	JOIN Person P ON P.Id = S.Id
                             WHERE P.FirstName NOT LIKE '!!%' AND GS.Status = (SELECT Id FROM Lookup Lk WHERE Value = 'Active')";
            PDFGenerator.GeneratePDF(query, "Project Students");
        }
        

        /// <summary>
        /// Report No. 4 ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void Report5()
        {
            string query = @"SELECT P.Id AS ProjectID, P.Title, 
                                    CASE WHEN GP.GroupId IS NOT NULL THEN 'Assigned' ELSE 'Not Assigned' END AS Status, 
                                    P.Description
                             FROM Project P 
                             LEFT JOIN GroupProject GP ON P.Id = GP.ProjectId
                             WHERE P.Title NOT LIKE '!!%'";
            PDFGenerator.GeneratePDF(query, "Project Details");
        }

    }
}
