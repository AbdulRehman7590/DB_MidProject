using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
namespace Mid_Project.Models
{
    class Configuration
    {
        string ConnectionStr;
        private static Configuration _instance = null;
        public static Configuration getInstance()
        {
            if (_instance == null)
                _instance = new Configuration();
            return _instance;
        }
        private Configuration()
        {
            ConnectionStr = @"Data Source=(local);Initial Catalog=ProjectA;Integrated Security=True";
        }
        public SqlConnection getConnection()
        {
            if (string.IsNullOrEmpty(ConnectionStr))
            {
                ConnectionStr = @"Data Source=(local);Initial Catalog=ProjectA;Integrated Security=True";
            }
            return new SqlConnection(ConnectionStr);
        }

        public static void ShowData(DataGrid GV, string query)
        {
            var con = getInstance().getConnection();
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            ShowData(GV, cmd);
        }
        public static void ShowData(DataGrid GV, SqlCommand cmd)
        {
            new Thread(() =>
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GV.ItemsSource = dt.DefaultView;
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Error!!!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    });
                }
            }).Start();
        }

    }
}