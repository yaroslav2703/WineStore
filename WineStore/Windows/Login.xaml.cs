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



namespace WineStore.Windows
{
   

    public partial class login : Window
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public login()
        {
            InitializeComponent();
                   
        }


        private void formRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registration registraition = new Registration();
            registraition.Show();
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (txbLogin.Text == "admin" && txbPassword.Password == "admin")
            {
                ForAdmin AdminWindow = new ForAdmin();
                AdminWindow.Show();
                Close();
                return;
            }
            try
            {

                if (txbLogin.Text != String.Empty && txbPassword.Password != String.Empty)
                {
                    if(UserVerification(txbLogin.Text, txbPassword.Password))
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No such user!");
                        txbLogin.Text = "";
                        txbPassword.Password = "";
                    }
                    
                  



                }
                else
                {
                    MessageBox.Show("Fill in all the fields!");
                    txbLogin.Text = "";
                    txbPassword.Password = "";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool UserVerification(string login, string password)
        {



           


            string sqlExpression = "sp_UserVerification";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
             
                command.CommandType = System.Data.CommandType.StoredProcedure;
              
                if(login=="admin" || password == "admin")
                {
                    return false;
                }
                SqlParameter loginParam = new SqlParameter
                {
                    ParameterName = "@login",
                    Value = login
                };
               
                command.Parameters.Add(loginParam);
                
                SqlParameter passwordParam = new SqlParameter
                {
                    ParameterName = "@password",
                    Value = password
                };

                command.Parameters.Add(passwordParam);

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