using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WineStore
{

    public partial class MainWindow : Window
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public MainWindow()
        {
           
            InitializeComponent();
          
            GridMain.Children.Clear();
            GridMain.Children.Add(new UserControls.Start_page(this));
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(50 + (150 * index), -500, 0, 0);


            switch (index)
            {
                case 0:
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new UserControls.Wines(this));
                    break;
                case 4:
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new UserControls.Basket(this));
                    break;
                case 5:
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new UserControls.About(this));
                    break;
                case 6:
                    GridMain.Children.Clear();
                    GridMain.Children.Add(new UserControls.My_account(this));
                    break;
              
            }

        }



        private void exitUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserLogOut())
            {
                Windows.login login = new Windows.login();
                login.Show();
                Close();
            }
            else
            {
                MessageBox.Show("No such user!");
               
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool UserLogOut()
        {

            string sqlExpression = "sp_UserLogOut";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlParameter resultParam = new SqlParameter
                {
                    ParameterName = "@result",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(resultParam);

                command.ExecuteNonQuery();


                if ((int)command.Parameters["@result"].Value == 1) return true;
                else return false;


            }
        }





    }
}